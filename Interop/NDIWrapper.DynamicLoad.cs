using System.Runtime.InteropServices;

namespace Tractus.Ndi;

public static unsafe partial class NDIWrapper
{
    [DllImport(LibraryName, EntryPoint = "NDIlib_v6_3_load", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern nint v6_3_load();

    [DllImport(LibraryName, EntryPoint = "NDIlib_v6_load", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern nint v6_load();

    [Obsolete("Use v6_3_load or v6_load.")]
    [DllImport(LibraryName, EntryPoint = "NDIlib_v5_load", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern nint v5_load();

    [Obsolete("Use v6_3_load or v6_load.")]
    [DllImport(LibraryName, EntryPoint = "NDIlib_v4_5_load", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern nint v4_5_load();

    [Obsolete("Use v6_3_load or v6_load.")]
    [DllImport(LibraryName, EntryPoint = "NDIlib_v4_load", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern nint v4_load();

    [Obsolete("Use v6_3_load or v6_load.")]
    [DllImport(LibraryName, EntryPoint = "NDIlib_v3_load", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern nint v3_load();

    [Obsolete("Use v6_3_load or v6_load.")]
    [DllImport(LibraryName, EntryPoint = "NDIlib_v2_load", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern nint v2_load();
}
