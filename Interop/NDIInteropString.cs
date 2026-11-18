using System.Runtime.InteropServices;

namespace Tractus.Ndi;

public class NDIInteropString : IDisposable
{
    public readonly string value;
    public nint utf8Ptr;
    private bool disposedValue;

    public NDIInteropString(string? value)
    {
        this.value = value ?? "";
        this.utf8Ptr = value is null
            ? nint.Zero
            : UTF.StringToUtf8(value);
    }

    public static implicit operator nint(NDIInteropString s) => s.utf8Ptr;
    public static implicit operator string(NDIInteropString s) => s.value;

    protected virtual void Dispose(bool disposing)
    {
        if (this.disposedValue)
        {
            return;
        }


        if (this.utf8Ptr != nint.Zero)
        {
            Marshal.FreeHGlobal(this.utf8Ptr);
            this.utf8Ptr = nint.Zero;
        }

        this.disposedValue = true;
    }

    // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
    // ~NDIInteropString()
    // {
    //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
    //     Dispose(disposing: false);
    // }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        this.Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
