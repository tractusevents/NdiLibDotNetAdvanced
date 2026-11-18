using System.Runtime.InteropServices;

namespace Tractus.Ndi;

public static unsafe partial class NDIWrapper
{
    public static Int64 recv_timestamp_undefined = Int64.MaxValue;

    public static bool GetReceiverSourceName(nint receiverPtr, out string name, uint timeout = 1000)
    {
        name = string.Empty;
        if(!recv_get_source_name(receiverPtr, out var pName, timeout)
            || pName == nint.Zero)
        {
            return false;
        }

        var toReturn = Marshal.PtrToStringUTF8(pName) ?? string.Empty;

        recv_free_string(receiverPtr, pName);

        name = toReturn;
        return true;
    }

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_add_connection_metadata", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void recv_add_connection_metadata(nint instance, ref metadata_frame_t metadata);

    [Obsolete("Use recv_capture_v2 or recv_capture_v3.")]
    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_capture", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern frame_type_e recv_capture(
        nint instance,
        video_frame_t* videoData,
        audio_frame_t* audioData,
        metadata_frame_t* metadata,
        uint timeoutInMilliseconds);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_capture_v2", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern frame_type_e recv_capture_v2(
        nint instance,
        video_frame_v2_t* videoData,
        audio_frame_v2_t* audioData,
        metadata_frame_t* metadata,
        uint timeoutInMilliseconds);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_capture_v2", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern frame_type_e recv_capture_v2(
        nint instance,
        ref video_frame_v2_t videoData,
        ref audio_frame_v2_t audioData,
        ref metadata_frame_t metadata,
        uint timeoutInMilliseconds);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_capture_v3", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern frame_type_e recv_capture_v3(
        nint p_instance,
        ref video_frame_v2_t p_video_data,
        ref audio_frame_v3_t p_audio_data,
        ref metadata_frame_t p_metadata,
        UInt32 timeout_in_ms);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_capture_v3", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static unsafe extern frame_type_e recv_capture_v3(
        nint p_instance,
        video_frame_v2_t* p_video_data,
        audio_frame_v3_t* p_audio_data,
        metadata_frame_t* p_metadata,
        uint timeout_in_ms);

    public static frame_type_e recv_capture_v3(
        nint p_instance,
        video_frame_v2_t* p_video_data,
        audio_frame_v3_t* p_audio_data,
        metadata_frame_t* p_metadata,
        int timeout_in_ms)
    {
        return recv_capture_v3(p_instance, p_video_data, p_audio_data, p_metadata, unchecked((uint)timeout_in_ms));
    }

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_clear_connection_metadata", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void recv_clear_connection_metadata(nint instance);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_connect", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void recv_connect(
        nint p_instance,
        ref source_t source);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_connect", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void recv_connect(
        nint p_instance,
        nint source);

    [Obsolete("Use recv_create_v3 or recv_create_v4.")]
    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_create", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern nint recv_create(recv_create_t* createSettings);

    [Obsolete("Use recv_create_v3 or recv_create_v4.")]
    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_create", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern nint recv_create(ref recv_create_t createSettings);

    [Obsolete("Use recv_create_v3 or recv_create_v4.")]
    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_create2", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern nint recv_create2(recv_create_t* createSettings);

    [Obsolete("Use recv_create_v3 or recv_create_v4.")]
    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_create2", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern nint recv_create2(ref recv_create_t createSettings);

    [Obsolete("Use recv_create_v3 or recv_create_v4.")]
    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_create_v2", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern nint recv_create_v2(recv_create_t* createSettings);

    [Obsolete("Use recv_create_v3 or recv_create_v4.")]
    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_create_v2", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern nint recv_create_v2(ref recv_create_t createSettings);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_create_v3", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern nint recv_create_v3(
        ref recv_create_v3_t createSettings);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_create_v3", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern nint recv_create_v3(recv_create_v3_t* createSettings);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_create_v4", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern nint recv_create_v4(
        ref recv_create_v3_t createSettings,
        nint configData);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_create_v4", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public unsafe static extern nint recv_create_v4(
    recv_create_v3_t* createSettings,
    nint configData);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_create_v4", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public unsafe static extern nint recv_create_v4(
    nint createSettings,
    nint configData);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_destroy", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void recv_destroy(nint p_instance);

    [Obsolete("Use recv_free_audio_v2 or recv_free_audio_v3.")]
    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_free_audio", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void recv_free_audio(nint instance, audio_frame_t* audioData);

    [Obsolete("Use recv_free_audio_v2 or recv_free_audio_v3.")]
    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_free_audio", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void recv_free_audio(nint instance, ref audio_frame_t audioData);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_free_audio_v2", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void recv_free_audio_v2(nint instance, audio_frame_v2_t* audioData);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_free_audio_v2", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void recv_free_audio_v2(nint instance, ref audio_frame_v2_t audioData);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_free_audio_v3", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void recv_free_audio_v3(nint p_instance, ref audio_frame_v3_t p_audio_data);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_free_audio_v3", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static unsafe extern void recv_free_audio_v3(nint p_instance, audio_frame_v3_t* p_audio_data);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_free_metadata", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void recv_free_metadata(nint p_instance, ref metadata_frame_t p_metadata);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_free_metadata", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static unsafe extern void recv_free_metadata(nint p_instance, metadata_frame_t* p_metadata);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_free_string", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void recv_free_string(nint p_instance, nint p_string);

    [Obsolete("Use recv_free_video_v2.")]
    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_free_video", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void recv_free_video(nint instance, video_frame_t* videoData);

    [Obsolete("Use recv_free_video_v2.")]
    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_free_video", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void recv_free_video(nint instance, ref video_frame_t videoData);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_free_video_v2", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void recv_free_video_v2(nint p_instance, ref video_frame_v2_t p_video_data);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_free_video_v2", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static unsafe extern void recv_free_video_v2(nint p_instance, video_frame_v2_t* p_video_data);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_get_audio_delay", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern int recv_get_audio_delay(nint instance);

    [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "NDIlib_recv_get_no_connections", ExactSpelling = true)]
    public static extern int recv_get_no_connections(nint p_instance);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_get_performance", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void recv_get_performance(
        nint instance,
        out recv_performance_t total,
        out recv_performance_t dropped);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_get_queue", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void recv_get_queue(nint instance, out recv_queue_t total);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_get_source_name", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool recv_get_source_name(
        nint p_instance,
        out nint p_name,
        uint timeoutMs);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_get_video_delay", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern int recv_get_video_delay(nint instance);

    [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "NDIlib_recv_get_web_control", ExactSpelling = true)]
    public static extern nint recv_get_web_control(nint p_instance);

    public static string recv_get_web_control_string(nint instance)
    {
        return UTF.Utf8ToString(recv_get_web_control(instance));
    }

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_kvm_is_supported", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool recv_kvm_is_supported(nint pInstance);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_kvm_send_clipboard_contents", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool recv_kvm_send_clipboard_contents(
        nint p_instance, [MarshalAs(UnmanagedType.LPUTF8Str)]string p_clipboard_contents);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_kvm_send_clipboard_contents", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool recv_kvm_send_clipboard_contents(nint instance, nint clipboardContents);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_kvm_send_horizontal_mouse_wheel", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool recv_kvm_send_horizontal_mouse_wheel(
        nint p_instance,
        float no_units);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_kvm_send_keyboard_press", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool recv_kvm_send_keyboard_press(
        nint p_instance, int key_sym_value);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_kvm_send_keyboard_release", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool recv_kvm_send_keyboard_release(
    nint p_instance, int key_sym_value);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_kvm_send_left_mouse_click", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool recv_kvm_send_left_mouse_click(
        nint p_instance);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_kvm_send_left_mouse_release", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool recv_kvm_send_left_mouse_release(
        nint p_instance);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_kvm_send_middle_mouse_click", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool recv_kvm_send_middle_mouse_click(
    nint p_instance);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_kvm_send_middle_mouse_release", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool recv_kvm_send_middle_mouse_release(
    nint p_instance);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_kvm_send_mouse_position", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool recv_kvm_send_mouse_position(
    nint p_instance,
        [In, MarshalAs(UnmanagedType.LPArray, SizeConst = 2)]
        float[] posn);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_kvm_send_mouse_position", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool recv_kvm_send_mouse_position(nint instance, float* position);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_kvm_send_right_mouse_click", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool recv_kvm_send_right_mouse_click(
        nint p_instance);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_kvm_send_right_mouse_release", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool recv_kvm_send_right_mouse_release(
        nint p_instance);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_kvm_send_touch_finish", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool recv_kvm_send_touch_finish(nint instance);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_kvm_send_touch_positions", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool recv_kvm_send_touch_positions(
        nint instance,
        int positionCount,
        float* positions);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_kvm_send_vertical_mouse_wheel", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool recv_kvm_send_vertical_mouse_wheel(
    nint p_instance,
    float no_units);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_ptz_auto_focus", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAsAttribute(UnmanagedType.U1)]
    public static extern bool recv_ptz_auto_focus(nint p_instance);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_ptz_exposure_auto", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool recv_ptz_exposure_auto(nint instance);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_ptz_exposure_manual", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool recv_ptz_exposure_manual(nint instance, float exposureLevel);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_ptz_exposure_manual_v2", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool recv_ptz_exposure_manual_v2(
        nint instance,
        float iris,
        float gain,
        float shutterSpeed);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_ptz_focus", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAsAttribute(UnmanagedType.U1)]
    public static extern bool recv_ptz_focus(nint p_instance, float focus_value);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_ptz_focus_speed", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAsAttribute(UnmanagedType.U1)]
    public static extern bool recv_ptz_focus_speed(nint p_instance, float focus_speed);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_ptz_is_supported", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool recv_ptz_is_supported(nint p_instance);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_ptz_pan_tilt", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAsAttribute(UnmanagedType.U1)]
    public static extern bool recv_ptz_pan_tilt(nint p_instance, float pan_value, float tilt_value);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_ptz_pan_tilt_speed", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAsAttribute(UnmanagedType.U1)]
    public static extern bool recv_ptz_pan_tilt_speed(nint p_instance, float pan_speed, float tilt_speed);

    [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "NDIlib_recv_ptz_recall_preset", ExactSpelling = true)]
    [return: MarshalAs(UnmanagedType.U1)]
    public static extern bool recv_ptz_recall_preset(nint p_instance, int preset_no, float speed);


    [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "NDIlib_recv_ptz_store_preset", ExactSpelling = true)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool recv_ptz_store_preset(nint p_instance, int preset_no);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_ptz_white_balance_auto", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool recv_ptz_white_balance_auto(nint instance);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_ptz_white_balance_indoor", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool recv_ptz_white_balance_indoor(nint instance);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_ptz_white_balance_manual", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool recv_ptz_white_balance_manual(nint instance, float red, float blue);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_ptz_white_balance_oneshot", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool recv_ptz_white_balance_oneshot(nint instance);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_ptz_white_balance_outdoor", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool recv_ptz_white_balance_outdoor(nint instance);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_ptz_zoom", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAsAttribute(UnmanagedType.U1)]
    public static extern bool recv_ptz_zoom(nint p_instance, float zoom_value);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_ptz_zoom_speed", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAsAttribute(UnmanagedType.U1)]
    public static extern bool recv_ptz_zoom_speed(nint p_instance, float zoom_speed);

    [Obsolete("The SDK deprecates remote receiver recording control; prefer the NDI File recorder API.")]
    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_recording_get_error", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern nint recv_recording_get_error(nint instance);

    [Obsolete("The SDK deprecates remote receiver recording control; prefer the NDI File recorder API.")]
    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_recording_get_filename", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern nint recv_recording_get_filename(nint instance);

    [Obsolete("The SDK deprecates remote receiver recording control; prefer the NDI File recorder API.")]
    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_recording_get_times", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool recv_recording_get_times(nint instance, recv_recording_time_t* times);

    [Obsolete("The SDK deprecates remote receiver recording control; prefer the NDI File recorder API.")]
    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_recording_get_times", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool recv_recording_get_times(nint instance, out recv_recording_time_t times);

    [Obsolete("The SDK deprecates remote receiver recording control; prefer the NDI File recorder API.")]
    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_recording_is_recording", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool recv_recording_is_recording(nint instance);

    [Obsolete("The SDK deprecates remote receiver recording control; prefer the NDI File recorder API.")]
    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_recording_is_supported", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool recv_recording_is_supported(nint instance);

    [Obsolete("The SDK deprecates remote receiver recording control; prefer the NDI File recorder API.")]
    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_recording_set_audio_level", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool recv_recording_set_audio_level(nint instance, float levelDb);

    [Obsolete("The SDK deprecates remote receiver recording control; prefer the NDI File recorder API.")]
    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_recording_start", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool recv_recording_start(nint instance, nint filenameHint);

    [Obsolete("The SDK deprecates remote receiver recording control; prefer the NDI File recorder API.")]
    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_recording_start", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool recv_recording_start(
        nint instance,
        [MarshalAs(UnmanagedType.LPUTF8Str)] string? filenameHint);

    [Obsolete("The SDK deprecates remote receiver recording control; prefer the NDI File recorder API.")]
    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_recording_stop", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool recv_recording_stop(nint instance);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_request_keyframe", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void recv_request_keyframe(nint recvPtr);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_send_metadata", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool recv_send_metadata(
        nint pInstance,
        ref metadata_frame_t metadataFrame);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_set_audio_allocator", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void recv_set_audio_allocator(
        nint pinstance,
        nint pOpaque,
        CustomAudioAllocatorCallback allocator,
        CustomAudioFreeCallback freer);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_set_audio_allocator", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void recv_set_audio_allocator(
        nint instance,
        nint opaque,
        nint allocator,
        nint deallocator);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_set_audio_delay", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool recv_set_audio_delay(nint instance, int delayInMilliseconds);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_set_bandwidth", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool recv_set_bandwidth(
        nint instance,
        recv_bandwidth_e bandwidth);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_set_enabled_streams", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool recv_set_enabled_streams(
        nint instance,
        [MarshalAs(UnmanagedType.I1)] bool enableVideo,
        [MarshalAs(UnmanagedType.I1)] bool enableAudio,
        [MarshalAs(UnmanagedType.I1)] bool enableMetadata);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_set_tally", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool recv_set_tally(
        nint p_instance,
        ref tally_t p_tally);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_set_video_allocator", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void recv_set_video_allocator(
        nint pinstance,
        nint pOpaque,
        CustomVideoAllocatorCallback allocator,
        CustomVideoFreeCallback freer);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_set_video_allocator", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void recv_set_video_allocator(
        nint instance,
        nint opaque,
        nint allocator,
        nint deallocator);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_set_video_delay", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool recv_set_video_delay(nint instance, int delayInMilliseconds);
}

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
[return: MarshalAs(UnmanagedType.I1)]
public delegate bool CustomVideoAllocatorCallback(nint pOpaque, ref video_frame_v2_t videoData);

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
[return: MarshalAs(UnmanagedType.I1)]
public delegate bool CustomVideoFreeCallback(nint pOpaque, ref video_frame_v2_t videoData);

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
[return: MarshalAs(UnmanagedType.I1)]
public delegate bool CustomAudioAllocatorCallback(nint pOpaque, ref audio_frame_v3_t audioData);

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
[return: MarshalAs(UnmanagedType.I1)]
public delegate bool CustomAudioFreeCallback(nint pOpaque, ref audio_frame_v3_t audioData);

// The creation structure that is used when you are creating a receiver
[StructLayoutAttribute(LayoutKind.Sequential)]
public struct recv_create_v3_t
{
    // The source that you wish to connect to.
    public source_t source_to_connect_to;

    // Your preference of color space. See above.
    public recv_color_format_e color_format;

    // The bandwidth setting that you wish to use for this video source. Bandwidth
    // controlled by changing both the compression level and the resolution of the source.
    // A good use for low bandwidth is working on WIFI connections.
    public recv_bandwidth_e bandwidth;

    // When this flag is FALSE, all video that you receive will be progressive. For sources
    // that provide fields, this is de-interlaced on the receiving side (because we cannot change
    // what the up-stream source was actually rendering. This is provided as a convenience to
    // down-stream sources that do not wish to understand fielded video. There is almost no
    // performance impact of using this function.
    [MarshalAsAttribute(UnmanagedType.U1)]
    public bool allow_video_fields;

    // The name of the NDI receiver to create. This is a NULL terminated UTF8 string and should be
    // the name of receive channel that you have. This is in many ways symettric with the name of
    // senders, so this might be "Channel 1" on your system.
    public nint p_ndi_recv_name;
}

// This allows you determine the current performance levels of the receiving to be able to detect whether frames have been dropped
[StructLayoutAttribute(LayoutKind.Sequential)]
public struct recv_performance_t
{
    // The number of video frames
    public Int64 video_frames;

    // The number of audio frames
    public Int64 audio_frames;

    // The number of metadata frames
    public Int64 metadata_frames;
}

// Get the current queue depths
[StructLayoutAttribute(LayoutKind.Sequential)]
public struct recv_queue_t
{
    // The number of video frames
    public int video_frames;

    // The number of audio frames
    public int audio_frames;

    // The number of metadata frames
    public int metadata_frames;
}
