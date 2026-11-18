using System.Runtime.InteropServices;

namespace Tractus.Ndi;

public static unsafe partial class NDIWrapper
{
    [Obsolete("Use the v2 or v3 conversion function.")]
    [DllImport(LibraryName, EntryPoint = "NDIlib_util_audio_from_interleaved_16s", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void util_audio_from_interleaved_16s(audio_frame_interleaved_16s_t* source, audio_frame_t* destination);

    [DllImport(LibraryName, EntryPoint = "NDIlib_util_audio_from_interleaved_16s_v2", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void util_audio_from_interleaved_16s_v2(
        ref audio_frame_interleaved_16s_t pSrc,
        ref audio_frame_v2_t pDst);

    [DllImport(LibraryName, EntryPoint = "NDIlib_util_audio_from_interleaved_16s_v3", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool util_audio_from_interleaved_16s_v3(
        ref audio_frame_interleaved_16s_t pSrc,
        ref audio_frame_v3_t pDst);

    [Obsolete("Use the v2 or v3 conversion function.")]
    [DllImport(LibraryName, EntryPoint = "NDIlib_util_audio_from_interleaved_32f", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void util_audio_from_interleaved_32f(audio_frame_interleaved_32f_t* source, audio_frame_t* destination);

    // Convert from interleaved floating point (float -> v2)
    [DllImport(LibraryName, EntryPoint = "NDIlib_util_audio_from_interleaved_32f_v2", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void util_audio_from_interleaved_32f_v2(
        ref audio_frame_interleaved_32f_t pSrc,
        ref audio_frame_v2_t pDst);

    // Convert from interleaved floating point (float -> v3), returns bool
    [DllImport(LibraryName, EntryPoint = "NDIlib_util_audio_from_interleaved_32f_v3", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool util_audio_from_interleaved_32f_v3(
        ref audio_frame_interleaved_32f_t pSrc,
        ref audio_frame_v3_t pDst);

    // Convert from interleaved 32-bit (int32 -> v2)
    [DllImport(LibraryName, EntryPoint = "NDIlib_util_audio_from_interleaved_32s_v2", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void util_audio_from_interleaved_32s_v2(
        ref audio_frame_interleaved_32s_t pSrc,
        ref audio_frame_v2_t pDst);

    // Convert from interleaved 32-bit (int32 -> v3), returns bool
    [DllImport(LibraryName, EntryPoint = "NDIlib_util_audio_from_interleaved_32s_v3", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool util_audio_from_interleaved_32s_v3(
        ref audio_frame_interleaved_32s_t pSrc,
        ref audio_frame_v3_t pDst);

    [Obsolete("Use the v2 or v3 conversion function.")]
    [DllImport(LibraryName, EntryPoint = "NDIlib_util_audio_to_interleaved_16s", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void util_audio_to_interleaved_16s(audio_frame_t* source, audio_frame_interleaved_16s_t* destination);

    [DllImport(LibraryName, EntryPoint = "NDIlib_util_audio_to_interleaved_16s_v2", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void util_audio_to_interleaved_16s_v2(
    ref audio_frame_v2_t pSrc,
    ref audio_frame_interleaved_16s_t pDst);

    [DllImport(LibraryName, EntryPoint = "NDIlib_util_audio_to_interleaved_16s_v3", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool util_audio_to_interleaved_16s_v3(
        ref audio_frame_v3_t pSrc,
        ref audio_frame_interleaved_16s_t pDst);

    [Obsolete("Use the v2 or v3 conversion function.")]
    [DllImport(LibraryName, EntryPoint = "NDIlib_util_audio_to_interleaved_32f", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void util_audio_to_interleaved_32f(audio_frame_t* source, audio_frame_interleaved_32f_t* destination);

    // Convert to interleaved floating point (v2 -> float)
    [DllImport(LibraryName, EntryPoint = "NDIlib_util_audio_to_interleaved_32f_v2", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void util_audio_to_interleaved_32f_v2(
        ref audio_frame_v2_t pSrc,
        ref audio_frame_interleaved_32f_t pDst);

    // Convert to interleaved floating point (v3 -> float), returns bool
    [DllImport(LibraryName, EntryPoint = "NDIlib_util_audio_to_interleaved_32f_v3", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool util_audio_to_interleaved_32f_v3(
        ref audio_frame_v3_t pSrc,
        ref audio_frame_interleaved_32f_t pDst);

    [DllImport(LibraryName, EntryPoint = "NDIlib_util_audio_to_interleaved_32s_v2", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void util_audio_to_interleaved_32s_v2(
    ref audio_frame_v2_t pSrc,
    ref audio_frame_interleaved_32s_t pDst);

    [DllImport(LibraryName, EntryPoint = "NDIlib_util_audio_to_interleaved_32s_v3", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool util_audio_to_interleaved_32s_v3(
    ref audio_frame_v3_t pSrc,
    ref audio_frame_interleaved_32s_t pDst);

    [DllImport(LibraryName, EntryPoint = "NDIlib_util_get_timestamp", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern long util_get_timestamp();

    // P216 -> V210 (video)
    [DllImport(LibraryName, EntryPoint = "NDIlib_util_P216_to_V210", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void util_P216_to_V210(
        ref video_frame_v2_t pSrcP216,
        ref video_frame_v2_t pDstV210);

    [DllImport(LibraryName, EntryPoint = "NDIlib_util_send_send_audio_interleaved_16s", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void util_send_send_audio_interleaved_16s(
    nint p_instance,
    ref audio_frame_interleaved_16s_t p_audio_data);

    [DllImport(LibraryName, EntryPoint = "NDIlib_util_send_send_audio_interleaved_32f", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void util_send_send_audio_interleaved_32f(
        nint p_instance,
        ref audio_frame_interleaved_32f_t p_audio_data);

    [DllImport(LibraryName, EntryPoint = "NDIlib_util_send_send_audio_interleaved_32s", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void util_send_send_audio_interleaved_32s(
    nint p_instance,
    ref audio_frame_interleaved_32s_t p_audio_data);

    // V210 -> P216 (video)
    [DllImport(LibraryName, EntryPoint = "NDIlib_util_V210_to_P216", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void util_V210_to_P216(
        ref video_frame_v2_t pSrcV210,
        ref video_frame_v2_t pDstP216);
}
