using System.Runtime.InteropServices;

namespace Tractus.Ndi;

public static unsafe partial class NDIWrapper
{
    [DllImport(LibraryName, EntryPoint = "NDIlib_routing_add_connection_metadata", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void routing_add_connection_metadata(nint p_instance, ref metadata_frame_t p_metadata);

    [DllImport(LibraryName, EntryPoint = "NDIlib_routing_add_connection_metadata", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern unsafe void routing_add_connection_metadata(nint p_instance, metadata_frame_t* p_metadata);

    [DllImport(LibraryName, EntryPoint = "NDIlib_routing_change", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool routing_change(nint p_instance, ref source_t source);

    [DllImport(LibraryName, EntryPoint = "NDIlib_routing_change", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern unsafe bool routing_change(nint p_instance, source_t* source);

    [DllImport(LibraryName, EntryPoint = "NDIlib_routing_clear", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern unsafe bool routing_clear(nint p_instance);

    [DllImport(LibraryName, EntryPoint = "NDIlib_routing_clear_connection_metadata", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void routing_clear_connection_metadata(nint p_instance);

    [DllImport(LibraryName, EntryPoint = "NDIlib_routing_create", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern nint routing_create(ref routing_create_t p_create_settings);

    [DllImport(LibraryName, EntryPoint = "NDIlib_routing_create", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern unsafe nint routing_create(routing_create_t* p_create_settings);

    [DllImport(LibraryName, EntryPoint = "NDIlib_routing_create_v2", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern nint routing_create_v2(ref routing_create_t p_create_settings, nint p_config_data);

    [DllImport(LibraryName, EntryPoint = "NDIlib_routing_create_v2", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public unsafe static extern nint routing_create_v2(routing_create_t* p_create_settings, nint p_config_data);

    [DllImport(LibraryName, EntryPoint = "NDIlib_routing_destroy", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern unsafe void routing_destroy(nint p_instance);

    [DllImport(LibraryName, EntryPoint = "NDIlib_routing_get_no_connections", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern int routing_get_no_connections(nint p_instance, uint timeoutInMilliseconds);

    public static int routing_get_no_connections(nint p_instance)
    {
        return routing_get_no_connections(p_instance, 0);
    }

    [DllImport(LibraryName, EntryPoint = "NDIlib_routing_get_source_name", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern nint routing_get_source_name(nint p_instance);

    public static string RoutingGetSourceNameWrapped(nint p_instance)
    {
        var result = routing_get_source_name(p_instance);
        var sourceDetails = Marshal.PtrToStructure<source_t>(result);
        var name = UTF.Utf8ToString(sourceDetails.p_ndi_name);

        return name;
    }
}

[StructLayout(LayoutKind.Sequential)]
public struct routing_create_t
{
    public nint p_ndi_name;
    public nint p_groups;
}
