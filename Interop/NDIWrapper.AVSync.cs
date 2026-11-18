using System.Runtime.InteropServices;

namespace Tractus.Ndi;

public static unsafe partial class NDIWrapper
{
    [DllImport(LibraryName, EntryPoint = "NDIlib_avsync_create", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern nint avsync_create(nint p_receiver);

    [DllImport(LibraryName, EntryPoint = "NDIlib_avsync_destroy", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void avsync_destroy(nint p_avsync);

    [DllImport(LibraryName, EntryPoint = "NDIlib_avsync_free_audio", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern unsafe void avsync_free_audio(
        nint p_avsync,
        ref audio_frame_v3_t p_audio_frame);

    [DllImport(LibraryName, EntryPoint = "NDIlib_avsync_free_audio", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern unsafe void avsync_free_audio(
        nint p_avsync,
        audio_frame_v3_t* p_audio_frame);

    [DllImport(LibraryName, EntryPoint = "NDIlib_avsync_synchronize", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern avsync_ret_e avsync_synchronize(nint p_avsync, ref video_frame_v2_t p_video_frame, ref audio_frame_v3_t p_audio_frame);

    [DllImport(LibraryName, EntryPoint = "NDIlib_avsync_synchronize", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern unsafe avsync_ret_e avsync_synchronize(
        nint p_avsync,
        video_frame_v2_t* p_video_frame,
        audio_frame_v3_t* p_audio_frame);
}
