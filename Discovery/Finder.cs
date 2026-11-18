namespace Tractus.Ndi;

public class Finder : IDisposable
{
    public Finder(bool showLocalSources = false, string[]? groups = null, string[]? extraIps = null)
    {
        using var groupNames = new NDIInteropString(groups is null ? null : string.Join(',', groups));
        using var extraAddresses = new NDIInteropString(extraIps is null ? null : string.Join(',', extraIps));
        var settings = new find_create_t
        {
            show_local_sources = showLocalSources,
            p_groups = groupNames,
            p_extra_ips = extraAddresses
        };

        this._findInstancePtr = NDIWrapper.find_create_v2(ref settings);
        if (this._findInstancePtr == nint.Zero)
        {
            throw new InvalidOperationException("Unable to create the NDI finder.");
        }

        try
        {
            this.RefreshSourceSnapshot();
            this._findThread = new Thread(this.FindThreadProc) { IsBackground = true, Name = "NdiFindThread" };
            this._findThread.Start();
        }
        catch
        {
            NDIWrapper.find_destroy(this._findInstancePtr);
            this._findInstancePtr = nint.Zero;
            throw;
        }
    }

    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~Finder()
    {
        this.Dispose(false);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!this._disposed)
        {
            if (disposing)
            {
                // tell the thread to exit
                this._exitThread = true;

                // wait for it to exit
                if (this._findThread != null)
                {
                    this._findThread.Join();

                    this._findThread = null;
                }
            }

            if (this._findInstancePtr != nint.Zero)
            {
                NDIWrapper.find_destroy(this._findInstancePtr);
                this._findInstancePtr = nint.Zero;
            }

            this._disposed = true;
        }
    }

    private bool _disposed = false;

    // A published array is immutable. Only its replacement needs the write lock.
    private readonly ReaderWriterLockSlim _sourceSnapshotLock = new();
    // Only the finder thread uses the two work arrays, and only for changed order or membership.
    private int[] sourceIndices = [];
    private bool[] matchedSources = [];
    private NdiSource[] _sources = Array.Empty<NdiSource>();
    private int _sourceSnapshotVersion;
    private int _notificationWorkerQueued;

    private void FindThreadProc()
    {
        while (!this._exitThread)
        {
            if (!NDIWrapper.find_wait_for_sources(this._findInstancePtr, 250))
            {
                continue;
            }

            if (this.RefreshSourceSnapshot())
            {
                this.QueueSourceListChanged();
            }
        }
    }

    public NdiSource[] GetCurrentSourceList()
    {
        this._sourceSnapshotLock.EnterReadLock();
        try
        {
            if (this._sources.Length == 0)
            {
                return Array.Empty<NdiSource>();
            }

            var snapshot = new NdiSource[this._sources.Length];
            Array.Copy(this._sources, snapshot, this._sources.Length);
            return snapshot;
        }
        finally
        {
            this._sourceSnapshotLock.ExitReadLock();
        }
    }

    /// <summary>Returns a stable, allocation-free view of the last published source list.</summary>
    public ReadOnlySpan<NdiSource> GetCurrentSourceSnapshot()
    {
        this._sourceSnapshotLock.EnterReadLock();
        try
        {
            return this._sources;
        }
        finally
        {
            this._sourceSnapshotLock.ExitReadLock();
        }
    }

    private unsafe bool RefreshSourceSnapshot()
    {
        var pointer = NDIWrapper.find_get_current_sources(this._findInstancePtr, out var count);
        var incoming = new ReadOnlySpan<source_t>(pointer.ToPointer(), checked((int)count));
        // Refresh runs first in the constructor and subsequently only on the finder thread.
        var previous = this._sources;
        if (incoming.Length == previous.Length)
        {
            var index = 0;
            while (index < incoming.Length && previous[index].Matches(incoming[index]))
            {
                index++;
            }

            if (index == incoming.Length)
            {
                return false;
            }
        }

        if (this.sourceIndices.Length < incoming.Length)
        {
            this.sourceIndices = new int[incoming.Length];
        }
        if (this.matchedSources.Length < previous.Length)
        {
            this.matchedSources = new bool[previous.Length];
        }
        else
        {
            Array.Clear(this.matchedSources, 0, previous.Length);
        }

        var changed = incoming.Length != previous.Length;
        for (var i = 0; i < incoming.Length; i++)
        {
            var name = NdiSource.BorrowUtf8(incoming[i].p_ndi_name);
            var address = NdiSource.BorrowUtf8(incoming[i].p_url_address);
            var match = -1;
            for (var j = 0; j < previous.Length; j++)
            {
                if (!this.matchedSources[j] && previous[j].NameUtf8.SequenceEqual(name)
                    && previous[j].UrlAddressUtf8.SequenceEqual(address))
                {
                    match = j;
                    this.matchedSources[j] = true;
                    break;
                }
            }

            this.sourceIndices[i] = match;
            if (match < 0)
            {
                changed = true;
            }
        }

        // Preserve the published order when the SDK only reorders the same sources.
        if (!changed)
        {
            return false;
        }

        var next = incoming.IsEmpty ? Array.Empty<NdiSource>() : new NdiSource[incoming.Length];
        for (var i = 0; i < next.Length; i++)
        {
            var match = this.sourceIndices[i];
            if (match >= 0)
            {
                next[i] = previous[match];
            }
            else
            {
                var source = incoming[i];
                next[i] = new NdiSource(ref source);
            }
        }

        this._sourceSnapshotLock.EnterWriteLock();
        try
        {
            this._sources = next;
        }
        finally
        {
            this._sourceSnapshotLock.ExitWriteLock();
        }

        Interlocked.Increment(ref this._sourceSnapshotVersion);
        return true;
    }

    private void QueueSourceListChanged()
    {
        if (Interlocked.CompareExchange(ref this._notificationWorkerQueued, 1, 0) != 0)
        {
            return;
        }

        ThreadPool.QueueUserWorkItem(
            static state => ((Finder)state!).NotifySourceListChanges(),
            this);
    }

    private void NotifySourceListChanges()
    {
        while (true)
        {
            var notifiedVersion = Volatile.Read(ref this._sourceSnapshotVersion);
            this.NotifySourceListChanged();

            Volatile.Write(ref this._notificationWorkerQueued, 0);
            if (Volatile.Read(ref this._sourceSnapshotVersion) == notifiedVersion
                || Interlocked.CompareExchange(ref this._notificationWorkerQueued, 1, 0) != 0)
            {
                return;
            }
        }
    }

    private void NotifySourceListChanged()
    {
        var subscribers = this.SourceListChanged;
        if (subscribers is null)
        {
            return;
        }

        foreach (var subscriber in subscribers.GetInvocationList())
        {
            try
            {
                ((Action)subscriber)();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError(
                    "NDI source-list notification failed: {0}",
                    ex);
            }
        }
    }

    private nint _findInstancePtr = nint.Zero;

    // a thread to find on so that the UI isn't dragged down
    Thread? _findThread;

    // a way to exit the thread safely
    volatile bool _exitThread = false;

    public event Action? SourceListChanged;
}
