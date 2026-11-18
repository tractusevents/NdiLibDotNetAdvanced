using System.Runtime.InteropServices;

namespace Tractus.Ndi;

public static unsafe partial class NDIWrapper
{
    [Obsolete("Use find_create_v2.")]
    [DllImport(LibraryName, EntryPoint = "NDIlib_find_create", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern nint find_create(find_create_t* createSettings);

    [Obsolete("Use find_create_v2.")]
    [DllImport(LibraryName, EntryPoint = "NDIlib_find_create", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern nint find_create(ref find_create_t createSettings);

    [Obsolete("Use find_create_v2.")]
    [DllImport(LibraryName, EntryPoint = "NDIlib_find_create2", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern nint find_create2(find_create_t* createSettings);

    [Obsolete("Use find_create_v2.")]
    [DllImport(LibraryName, EntryPoint = "NDIlib_find_create2", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern nint find_create2(ref find_create_t createSettings);

    // find_create_v2
    [DllImport(LibraryName, EntryPoint = "NDIlib_find_create_v2", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern nint find_create_v2(ref find_create_t p_create_settings);

    [DllImport(LibraryName, EntryPoint = "NDIlib_find_create_v2", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern nint find_create_v2(find_create_t* createSettings);

    [DllImport(LibraryName, EntryPoint = "NDIlib_find_create_v3", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern nint find_create_v3(
        ref find_create_t createSettings,
        nint configData);

    [DllImport(LibraryName, EntryPoint = "NDIlib_find_destroy", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void find_destroy(
        nint p_instance);

    // find_get_current_sources
    [DllImport(LibraryName, EntryPoint = "NDIlib_find_get_current_sources", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern nint find_get_current_sources(nint p_instance, out uint p_no_sources);

    // NDIlib_find_get_current_sources_v2
    // find_get_current_sources
    [DllImport(LibraryName, EntryPoint = "NDIlib_find_get_current_sources_v2", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern nint find_get_current_sources_v2(nint p_instance, out uint p_no_sources);

    public static source_v2_t[] find_get_current_sources_v2(
        nint pInstance)
    {
        var sourcesPtr = find_get_current_sources_v2(pInstance, out var numSources);
        if(sourcesPtr == nint.Zero)
        {
            return [];
        }

        var sources = new source_v2_t[numSources];

        var structSize = Marshal.SizeOf<source_v2_t>();
        for(var i = 0; i < numSources; i++)
        {
            var currentPtr = nint.Add(sourcesPtr, i * structSize);
            sources[i] = Marshal.PtrToStructure<source_v2_t>(currentPtr);
        }

        return sources;
    }

    [Obsolete("Use find_wait_for_sources followed by find_get_current_sources.")]
    [DllImport(LibraryName, EntryPoint = "NDIlib_find_get_sources", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern nint find_get_sources(nint instance, out uint sourceCount, uint timeoutInMilliseconds);

    [DllImport(LibraryName, EntryPoint = "NDIlib_find_wait_for_sources", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAsAttribute(UnmanagedType.U1)]
    public static extern bool find_wait_for_sources(nint p_instance, UInt32 timeout_in_ms);
}

[StructLayout(LayoutKind.Sequential)]
public struct source_v2_t
{
    [MarshalAs(UnmanagedType.LPUTF8Str)]
    public string p_ndi_name;

    [MarshalAs(UnmanagedType.LPUTF8Str)]
    public string p_url_address;

    [MarshalAs(UnmanagedType.LPUTF8Str)]
    public string p_metadata;
}

// This is a descriptor of a NDI source available on the network.
[StructLayoutAttribute(LayoutKind.Sequential)]
public struct source_t
{
    // A UTF8 string that provides a user readable name for this source.
    // This can be used for serialization, etc... and comprises the machine
    // name and the source name on that machine. In the form
    //		MACHINE_NAME (NDI_SOURCE_NAME)
    // If you specify this parameter either as NULL, or an EMPTY string then the
    // specific ip addres adn port number from below is used.
    public nint p_ndi_name;

    // A UTF8 string that provides the actual network address and any parameters.
    // This is not meant to be application readable and might well change in the future.
    // This can be nullptr if you do not know it and the API internally will instantiate
    // a finder that is used to discover it even if it is not yet available on the network.
    public nint p_url_address;
}

[StructLayoutAttribute(LayoutKind.Sequential)]
public struct find_create_t
{
    // Do we want to incluide the list of NDI sources that are running
    // on the local machine ?
    // If TRUE then local sources will be visible, if FALSE then they
    // will not.
    [MarshalAsAttribute(UnmanagedType.U1)]
    public bool show_local_sources;

    // Which groups do you want to search in for sources
    public nint p_groups;

    // The list of additional IP addresses that exist that we should query for
    // sources on. For instance, if you want to find the sources on a remote machine
    // that is not on your local sub-net then you can put a comma seperated list of
    // those IP addresses here and those sources will be available locally even though
    // they are not mDNS discoverable. An example might be "12.0.0.8,13.0.12.8".
    // When none is specified the registry is used.
    // Default = NULL;
    public nint p_extra_ips;
}
