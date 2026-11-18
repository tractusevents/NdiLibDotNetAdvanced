using System.Runtime.InteropServices;
using System.Security;

namespace Tractus.Ndi.Sdk64;

/// <summary>
/// Raw NDI 6.4 File API. The caller owns all recorder/player handles and is
/// responsible for the lifecycle contracts documented by the native SDK.
/// </summary>
[SuppressUnmanagedCodeSecurity]
public static unsafe class NDIFile
{
    private const string LibraryName = NDIWrapper.FileLibraryName;

    public static long TimecodeFromHms(long hours, long minutes, long seconds)
    {
        return checked(((hours * 3600) + (minutes * 60) + seconds) * 10_000_000);
    }

    [DllImport(LibraryName, EntryPoint = "NDIlib_file_player_probe_file", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool file_player_probe_file(
        [MarshalAs(UnmanagedType.LPUTF8Str)] string filePath,
        out NDIlib_file_player_file_info_t info);

    [DllImport(LibraryName, EntryPoint = "NDIlib_file_player_probe_file", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool file_player_probe_file(
        nint filePath,
        NDIlib_file_player_file_info_t* info);

    [DllImport(LibraryName, EntryPoint = "NDIlib_file_player_create", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern nint file_player_create(ref NDIlib_file_player_create_t createSettings);

    [DllImport(LibraryName, EntryPoint = "NDIlib_file_player_create", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern nint file_player_create(NDIlib_file_player_create_t* createSettings);

    [DllImport(LibraryName, EntryPoint = "NDIlib_file_player_destroy", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void file_player_destroy(nint instance);

    [DllImport(LibraryName, EntryPoint = "NDIlib_file_player_start", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool file_player_start(
        nint instance,
        [MarshalAs(UnmanagedType.LPUTF8Str)] string filePath);

    [DllImport(LibraryName, EntryPoint = "NDIlib_file_player_start", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool file_player_start(nint instance, nint filePath);

    [DllImport(LibraryName, EntryPoint = "NDIlib_file_player_stop", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void file_player_stop(nint instance);

    [DllImport(LibraryName, EntryPoint = "NDIlib_file_player_is_running", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool file_player_is_running(nint instance);

    [DllImport(LibraryName, EntryPoint = "NDIlib_file_player_capture_event", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern NDIlib_file_player_event_type_e file_player_capture_event(
        nint instance,
        out NDIlib_file_player_event_t fileEvent,
        uint timeoutInMilliseconds);

    [DllImport(LibraryName, EntryPoint = "NDIlib_file_player_capture_event", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern NDIlib_file_player_event_type_e file_player_capture_event(
        nint instance,
        NDIlib_file_player_event_t* fileEvent,
        uint timeoutInMilliseconds);

    [DllImport(LibraryName, EntryPoint = "NDIlib_file_player_get_dropped_event_count", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong file_player_get_dropped_event_count(nint instance);

    [DllImport(LibraryName, EntryPoint = "NDIlib_file_player_get_stats", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool file_player_get_stats(nint instance, out NDIlib_file_player_stats_t stats);

    [DllImport(LibraryName, EntryPoint = "NDIlib_file_player_seek_to_time", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool file_player_seek_to_time(nint instance, ulong time100ns);

    [DllImport(LibraryName, EntryPoint = "NDIlib_file_player_seek_to_frame", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool file_player_seek_to_frame(nint instance, ulong frameIndex);

    [DllImport(LibraryName, EntryPoint = "NDIlib_file_player_set_paused", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void file_player_set_paused(
        nint instance,
        [MarshalAs(UnmanagedType.I1)] bool paused);

    [DllImport(LibraryName, EntryPoint = "NDIlib_file_player_get_paused", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool file_player_get_paused(nint instance);

    [DllImport(LibraryName, EntryPoint = "NDIlib_file_player_set_loop", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void file_player_set_loop(
        nint instance,
        [MarshalAs(UnmanagedType.I1)] bool loop);

    [DllImport(LibraryName, EntryPoint = "NDIlib_file_player_get_loop", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool file_player_get_loop(nint instance);

    [DllImport(LibraryName, EntryPoint = "NDIlib_file_player_get_current_duration", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool file_player_get_current_duration(
        nint instance,
        out ulong duration,
        out byte isFinal);

    public static bool file_player_get_current_duration(
        nint instance,
        out ulong duration,
        out bool isFinal)
    {
        var result = file_player_get_current_duration(instance, out duration, out byte nativeIsFinal);
        isFinal = nativeIsFinal != 0;
        return result;
    }

    [DllImport(LibraryName, EntryPoint = "NDIlib_file_player_get_current_position", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool file_player_get_current_position(nint instance, out ulong position);

    [DllImport(LibraryName, EntryPoint = "NDIlib_file_player_get_current_source_timecode", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool file_player_get_current_source_timecode(nint instance, out long timecode100ns);

    [DllImport(LibraryName, EntryPoint = "NDIlib_file_player_get_source_format", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool file_player_get_source_format(nint instance, out NDIlib_file_source_format_t format);

    [DllImport(LibraryName, EntryPoint = "NDIlib_file_player_get_advertised_source", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern nint file_player_get_advertised_source(nint instance);

    public static (string NdiName, string UrlAddress) file_player_get_advertised_source_snapshot(nint instance)
    {
        var sourcePointer = file_player_get_advertised_source(instance);
        if (sourcePointer == nint.Zero)
        {
            return (string.Empty, string.Empty);
        }

        var source = *(source_t*)sourcePointer;
        return (UTF.Utf8ToString(source.p_ndi_name), UTF.Utf8ToString(source.p_url_address));
    }

    [DllImport(LibraryName, EntryPoint = "NDIlib_file_recorder_create", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern nint file_recorder_create(ref NDIlib_file_recorder_create_t createSettings);

    [DllImport(LibraryName, EntryPoint = "NDIlib_file_recorder_create", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern nint file_recorder_create(NDIlib_file_recorder_create_t* createSettings);

    [DllImport(LibraryName, EntryPoint = "NDIlib_file_recorder_destroy", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void file_recorder_destroy(nint instance);

    [DllImport(LibraryName, EntryPoint = "NDIlib_file_recorder_start", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool file_recorder_start(
        nint instance,
        [MarshalAs(UnmanagedType.LPUTF8Str)] string outputPrefix);

    [DllImport(LibraryName, EntryPoint = "NDIlib_file_recorder_start", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool file_recorder_start(nint instance, nint outputPrefix);

    [DllImport(LibraryName, EntryPoint = "NDIlib_file_recorder_start_with_timecode", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool file_recorder_start_with_timecode(
        nint instance,
        [MarshalAs(UnmanagedType.LPUTF8Str)] string outputPrefix,
        long startTimecode100ns);

    [DllImport(LibraryName, EntryPoint = "NDIlib_file_recorder_start_with_timecode", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool file_recorder_start_with_timecode(
        nint instance,
        nint outputPrefix,
        long startTimecode100ns);

    [DllImport(LibraryName, EntryPoint = "NDIlib_file_recorder_capture_event", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern NDIlib_file_recorder_event_type_e file_recorder_capture_event(
        nint instance,
        out NDIlib_file_recorder_event_t recorderEvent,
        uint timeoutInMilliseconds);

    [DllImport(LibraryName, EntryPoint = "NDIlib_file_recorder_capture_event", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern NDIlib_file_recorder_event_type_e file_recorder_capture_event(
        nint instance,
        NDIlib_file_recorder_event_t* recorderEvent,
        uint timeoutInMilliseconds);

    [DllImport(LibraryName, EntryPoint = "NDIlib_file_recorder_get_dropped_event_count", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong file_recorder_get_dropped_event_count(nint instance);

    [DllImport(LibraryName, EntryPoint = "NDIlib_file_recorder_stop", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void file_recorder_stop(nint instance);

    [DllImport(LibraryName, EntryPoint = "NDIlib_file_recorder_rollover", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool file_recorder_rollover(
        nint instance,
        [MarshalAs(UnmanagedType.LPUTF8Str)] string newOutputPrefix);

    [DllImport(LibraryName, EntryPoint = "NDIlib_file_recorder_rollover", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool file_recorder_rollover(nint instance, nint newOutputPrefix);

    [DllImport(LibraryName, EntryPoint = "NDIlib_file_recorder_inject_metadata", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool file_recorder_inject_metadata(nint instance, ref metadata_frame_t metadata);

    [DllImport(LibraryName, EntryPoint = "NDIlib_file_recorder_set_source", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool file_recorder_set_source(
        nint instance,
        ref NDIlib_file_recorder_source_t source);

    [DllImport(LibraryName, EntryPoint = "NDIlib_file_recorder_get_source", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool file_recorder_get_source(
        nint instance,
        out NDIlib_file_recorder_source_t source);

    public static bool file_recorder_get_source_snapshot(
        nint instance,
        out NDIlib_file_recorder_source_snapshot_t source)
    {
        if (!file_recorder_get_source(instance, out var nativeSource))
        {
            source = default;
            return false;
        }

        source = new NDIlib_file_recorder_source_snapshot_t(
            UTF.Utf8ToString(nativeSource.p_ndi_name),
            UTF.Utf8ToString(nativeSource.p_url_address));
        return true;
    }

    [DllImport(LibraryName, EntryPoint = "NDIlib_file_recorder_is_recording", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool file_recorder_is_recording(nint instance);

    [DllImport(LibraryName, EntryPoint = "NDIlib_file_recorder_get_current_file", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool file_recorder_get_current_file(nint instance, out nint path);

    public static bool file_recorder_get_current_file(nint instance, out string path)
    {
        var result = file_recorder_get_current_file(instance, out nint pathPointer);
        path = result ? UTF.Utf8ToString(pathPointer) : string.Empty;
        return result;
    }

    [DllImport(LibraryName, EntryPoint = "NDIlib_file_recorder_get_stats", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool file_recorder_get_stats(nint instance, out NDIlib_file_recorder_stats_t stats);

    [DllImport(LibraryName, EntryPoint = "NDIlib_file_recorder_get_source_format", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool file_recorder_get_source_format(nint instance, out NDIlib_file_source_format_t format);

    [DllImport(LibraryName, EntryPoint = "NDIlib_file_v1_load", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern nint file_v1_load();
}
