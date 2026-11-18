using System.Runtime.InteropServices;

namespace Tractus.Ndi;

public static unsafe partial class NDIWrapper
{
    // framesync_audio_queue_depth
    [DllImport(LibraryName, EntryPoint = "NDIlib_framesync_audio_queue_depth", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern int framesync_audio_queue_depth(nint p_instance);

    // framesync_capture_audio
    [DllImport(LibraryName, EntryPoint = "NDIlib_framesync_capture_audio", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void framesync_capture_audio(nint p_instance, ref audio_frame_v2_t p_audio_data, int sample_rate, int no_channels, int no_samples);

    // framesync_capture_audio
    [DllImport(LibraryName, EntryPoint = "NDIlib_framesync_capture_audio", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static unsafe extern void framesync_capture_audio(nint p_instance, audio_frame_v2_t* p_audio_data, int sample_rate, int no_channels, int no_samples);

    // framesync_capture_audio_v2
    [DllImport(LibraryName, EntryPoint = "NDIlib_framesync_capture_audio_v2", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void framesync_capture_audio_v2(nint p_instance, ref audio_frame_v3_t p_audio_data, int sample_rate, int no_channels, int no_samples);

    // framesync_capture_audio_v2
    [DllImport(LibraryName, EntryPoint = "NDIlib_framesync_capture_audio_v2", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static unsafe extern void framesync_capture_audio_v2(nint p_instance, audio_frame_v3_t* p_audio_data, int sample_rate, int no_channels, int no_samples);

    // framesync_capture_video
    [DllImport(LibraryName, EntryPoint = "NDIlib_framesync_capture_video", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void framesync_capture_video(nint p_instance, ref video_frame_v2_t p_video_data, frame_format_type_e field_type);

    // framesync_capture_video
    [DllImport(LibraryName, EntryPoint = "NDIlib_framesync_capture_video", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public unsafe static extern void framesync_capture_video(nint p_instance, video_frame_v2_t* p_video_data, frame_format_type_e field_type);

    // framesync_create_v2
    [DllImport(LibraryName, EntryPoint = "NDIlib_framesync_create", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern nint framesync_create(nint p_receiver);

    // framesync_destroy
    [DllImport(LibraryName, EntryPoint = "NDIlib_framesync_destroy", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void framesync_destroy(nint p_instance);

    // framesync_free_audio
    [DllImport(LibraryName, EntryPoint = "NDIlib_framesync_free_audio", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static unsafe extern void framesync_free_audio(nint p_instance, audio_frame_v2_t* p_audio_data);

    // framesync_free_audio
    [DllImport(LibraryName, EntryPoint = "NDIlib_framesync_free_audio", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void framesync_free_audio(nint p_instance, ref audio_frame_v2_t p_audio_data);

    // framesync_free_audio_v2
    [DllImport(LibraryName, EntryPoint = "NDIlib_framesync_free_audio_v2", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void framesync_free_audio_v2(nint p_instance, ref audio_frame_v3_t p_audio_data);

    // framesync_free_audio_v2
    [DllImport(LibraryName, EntryPoint = "NDIlib_framesync_free_audio_v2", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static unsafe extern void framesync_free_audio_v2(nint p_instance, audio_frame_v3_t* p_audio_data);

    // framesync_free_video
    [DllImport(LibraryName, EntryPoint = "NDIlib_framesync_free_video", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void framesync_free_video(nint p_instance, ref video_frame_v2_t p_video_data);

    // framesync_free_video
    [DllImport(LibraryName, EntryPoint = "NDIlib_framesync_free_video", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public unsafe static extern void framesync_free_video(nint p_instance, video_frame_v2_t* p_video_data);
}
