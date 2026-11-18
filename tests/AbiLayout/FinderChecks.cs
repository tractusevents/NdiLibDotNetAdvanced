using System.Reflection;
using System.Runtime.InteropServices;
using Tractus.Ndi;

internal static class FinderChecks
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int Change(int mode);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int Completed();

    public static void Run(string path)
    {
        var library = NativeLibrary.Load(path);
        try
        {
            NDIWrapper.Initialize(true, path);
            var change = Marshal.GetDelegateForFunctionPointer<Change>(NativeLibrary.GetExport(library, "finder_change"));
            var completed = Marshal.GetDelegateForFunctionPointer<Completed>(NativeLibrary.GetExport(library, "finder_completed"));
            using var finder = new Finder(groups: ["g", "g"], extraIps: ["1", "2"]);
            var calls = 0;
            finder.SourceListChanged += () => throw new InvalidOperationException("Fault-injected subscriber");
            finder.SourceListChanged += () => Interlocked.Increment(ref calls);

            void ChangeList(int mode)
            {
                var revision = change(mode);
                if (!SpinWait.SpinUntil(() => completed() >= revision, TimeSpan.FromSeconds(5)))
                {
                    throw new TimeoutException("Finder did not finish processing the source list.");
                }
            }

            var snapshot = finder.GetCurrentSourceSnapshot();
            Check(snapshot.Length == 2 && snapshot[0].SourceName == "One", "Initial source snapshot");
            var copy = finder.GetCurrentSourceList();
            copy[0] = copy[1];
            Check(snapshot[0].UrlAddress == "url1", "Caller-owned array isolation");

            ChangeList(1);
            var same = finder.GetCurrentSourceSnapshot();
            Check(ReferenceEquals(snapshot[0], same[0]) && ReferenceEquals(snapshot[1], same[1]),
                "Reordering equal source names preserves both identities and the published order");

            // The fixture is idle. Exercise the same refresh the polling thread performs on a change signal.
            var refresh = typeof(Finder).GetMethod("RefreshSourceSnapshot", BindingFlags.Instance | BindingFlags.NonPublic)!
                .CreateDelegate<Func<bool>>(finder);
            Measure(finder, refresh, "reordered");
            ChangeList(0);
            Measure(finder, refresh, "unchanged");
            Check(Volatile.Read(ref calls) == 0, "Equivalent lists must not send notifications");

            ChangeList(2);
            Check(SpinWait.SpinUntil(() => Volatile.Read(ref calls) > 0, TimeSpan.FromSeconds(5)), "An address change must notify");
            var next = finder.GetCurrentSourceSnapshot();
            Check(ReferenceEquals(snapshot[1], next[0]) && next[1].UrlAddress == "url3", "Reuse only unchanged sources");
            Check(snapshot[0].UrlAddress == "url1", "Old snapshot stays valid");
            ChangeList(3);
            Check(finder.GetCurrentSourceSnapshot().IsEmpty, "Removal publishes an empty snapshot");
            ChangeList(0);
            Check(finder.GetCurrentSourceSnapshot().Length == 2, "Sources can return after loss");
        }
        finally
        {
            NativeLibrary.Free(library);
        }
    }

    private static void Measure(Finder finder, Func<bool> refresh, string label)
    {
        for (var i = 0; i < 100; i++)
        {
            refresh();
            _ = finder.GetCurrentSourceSnapshot();
        }

        const int iterations = 10000;
        var before = GC.GetAllocatedBytesForCurrentThread();
        for (var i = 0; i < iterations; i++)
        {
            if (refresh() || finder.GetCurrentSourceSnapshot().Length != 2)
            {
                throw new InvalidOperationException("Unexpected source change during measurement.");
            }
        }
        var bytes = GC.GetAllocatedBytesForCurrentThread() - before;
        Check(bytes == 0, $"{label} refresh and snapshot reads allocated {bytes} bytes");
        Console.WriteLine($"Finder {label}: {iterations} refresh/read pairs, {bytes} managed bytes.");
    }

    private static void Check(bool ok, string message)
    {
        if (!ok)
        {
            throw new InvalidOperationException(message);
        }
    }
}
