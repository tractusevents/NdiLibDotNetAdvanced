using System.Runtime.InteropServices;

namespace Tractus.Ndi;

public static unsafe partial class NDIWrapper
{
    [DllImport(LibraryName, EntryPoint = "NDIlib_genlock_connect", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void genlock_connect(nint p_instance, ref source_t p_src_settings);

    [DllImport(LibraryName, EntryPoint = "NDIlib_genlock_connect", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern unsafe void genlock_connect(nint p_instance, source_t* p_src_settings);

    // find_create_v2
    [DllImport(LibraryName, EntryPoint = "NDIlib_genlock_create", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern nint genlock_create(ref source_t p_src_settings, nint configData);

    [DllImport(LibraryName, EntryPoint = "NDIlib_genlock_create", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern unsafe nint genlock_create(source_t* p_src_settings, nint configData);

    [DllImport(LibraryName, EntryPoint = "NDIlib_genlock_destroy", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void genlock_destroy(nint p_instance);

    [DllImport(LibraryName, EntryPoint = "NDIlib_genlock_is_active", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool genlock_is_active(nint p_instance);

    [DllImport(LibraryName, EntryPoint = "NDIlib_genlock_wait_audio", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool genlock_wait_audio(nint p_instance, ref audio_frame_v3_t p_audio_data);

    [DllImport(LibraryName, EntryPoint = "NDIlib_genlock_wait_audio", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern unsafe bool genlock_wait_audio(nint p_instance, audio_frame_v3_t* p_audio_data);

    [DllImport(LibraryName, EntryPoint = "NDIlib_genlock_wait_video", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool genlock_wait_video(nint p_instance, ref video_frame_v2_t p_video_data);

    [DllImport(LibraryName, EntryPoint = "NDIlib_genlock_wait_video", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern unsafe bool genlock_wait_video(nint p_instance, video_frame_v2_t* p_video_data);
}
