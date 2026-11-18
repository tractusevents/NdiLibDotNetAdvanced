using System.Runtime.InteropServices;
using System.Security;

namespace Tractus.Ndi;

[SuppressUnmanagedCodeSecurity]
public static unsafe partial class NDIWrapper
{
    // Based on how SDLSharp does things.
    // https://github.com/GabrielFrigo4/SDL-Sharp/blob/3daad4b05c11c1a3987ae24c12c78092be3aa9c3/SDL-Sharp/SDL/SDL.Loader.cs#L11

    private const string LibraryName = "NdiAdv";
    internal const string FileLibraryName = "NdiFile64";

    [DllImport(LibraryName, EntryPoint = "NDIlib_destroy", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void destroy();

    [DllImport(LibraryName, EntryPoint = "NDIlib_initialize", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool initialize();

    [DllImport(LibraryName, EntryPoint = "NDIlib_is_supported_CPU", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool is_supported_CPU();

    public static string version()
    {
        return UTF.Utf8ToString(version_native());
    }

    [DllImport(LibraryName, EntryPoint = "NDIlib_version", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern nint version_native();
}
