using System.Runtime.InteropServices;

namespace Tractus.Ndi;

public static unsafe partial class NDIWrapper
{
    public static Int64 send_timecode_synthesize = Int64.MaxValue;

    public static void AddMetadata(nint senderPtr, string[] metadata, bool replaceAll = false)
    {
        ArgumentNullException.ThrowIfNull(metadata);
        if (replaceAll)
        {
            send_clear_connection_metadata(senderPtr);
        }

        foreach (var item in metadata)
        {
            WriteMetadata(senderPtr, item, true);
        }
    }

    public static void AddMetadata(nint sender, IEnumerable<string> metadata, bool replaceAll = false)
    {
        ArgumentNullException.ThrowIfNull(metadata);
        if (metadata is string[] array)
        {
            AddMetadata(sender, array, replaceAll);
            return;
        }
        if (replaceAll)
        {
            send_clear_connection_metadata(sender);
        }

        foreach (var item in metadata)
        {
            WriteMetadata(sender, item, true);
        }
    }

    //NDIlib_send_add_connection_metadata
    [DllImport(LibraryName, EntryPoint = "NDIlib_send_add_connection_metadata", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void send_add_connection_metadata(
        nint pInstance,
        ref metadata_frame_t metadataFrame);

    [DllImport(LibraryName, EntryPoint = "NDIlib_send_capture", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern frame_type_e send_capture(
        nint pInstance,
        ref metadata_frame_t pMetadata,
        uint timeoutInMs);

    [DllImport(LibraryName, EntryPoint = "NDIlib_send_capture", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern unsafe frame_type_e send_capture(
        nint pInstance,
        metadata_frame_t* pMetadata,
        uint timeoutInMs);

    //NDIlib_send_add_connection_metadata
    [DllImport(LibraryName, EntryPoint = "NDIlib_send_clear_connection_metadata", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void send_clear_connection_metadata(
        nint pInstance);

    [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "NDIlib_send_create", ExactSpelling = true)]
    public static extern nint send_create(ref send_create_t p_create_settings);

    [DllImport(LibraryName, EntryPoint = "NDIlib_send_create", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern nint send_create(send_create_t* createSettings);

    [DllImport(LibraryName, EntryPoint = "NDIlib_send_create_v2", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern nint send_create_v2(
        ref send_create_t create,
        [MarshalAs(UnmanagedType.LPStr)] string p_config_data);

    [DllImport(LibraryName, EntryPoint = "NDIlib_send_create_v2", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern nint send_create_v2(
        ref send_create_t create,
        nint p_config_data);

    [DllImport(LibraryName, EntryPoint = "NDIlib_send_destroy", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void send_destroy(nint p_instance);

    [DllImport(LibraryName, EntryPoint = "NDIlib_send_free_metadata", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void send_free_metadata(
        nint pInstance,
        ref metadata_frame_t pMetadata);

    [DllImport(LibraryName, EntryPoint = "NDIlib_send_free_metadata", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void send_free_metadata(
        nint pInstance,
        metadata_frame_t* pMetadata);

    [DllImport(LibraryName, EntryPoint = "NDIlib_send_get_no_connections", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern int send_get_no_connections(nint p_instance, uint timeout_in_ms);

    [DllImport(LibraryName, EntryPoint = "NDIlib_send_get_q_factor", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern int send_get_q_factor(nint instance, video_frame_v2_t* videoData);

    [DllImport(LibraryName, EntryPoint = "NDIlib_send_get_q_factor_v2", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern int send_get_q_factor_v2(
        nint instance,
        video_frame_v2_t* videoData,
        [MarshalAs(UnmanagedType.I1)] bool supportsNewFormat,
        byte* enableShortDc);

    public static int send_get_q_factor_v2(
        nint instance,
        ref video_frame_v2_t videoData,
        bool supportsNewFormat,
        out bool enableShortDc)
    {
        byte nativeEnableShortDc = 0;
        int result;
        fixed (video_frame_v2_t* videoPointer = &videoData)
        {
            result = send_get_q_factor_v2(
                instance,
                videoPointer,
                supportsNewFormat,
                &nativeEnableShortDc);
        }

        enableShortDc = nativeEnableShortDc != 0;
        return result;
    }

    [DllImport(LibraryName, EntryPoint = "NDIlib_send_get_source_name", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern nint send_get_source_name(nint p_instance);

    [DllImport(LibraryName, EntryPoint = "NDIlib_send_get_tally", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.U1)]
    public static extern bool send_get_tally(
        nint pInstance,
        ref tally_t tally,
        uint timeoutInMs);

    [DllImport(LibraryName, EntryPoint = "NDIlib_send_get_target_bit_rate", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern int send_get_target_bit_rate(nint instance, video_frame_v2_t* videoData);

    [DllImport(LibraryName, EntryPoint = "NDIlib_send_get_target_frame_size", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern int send_get_target_frame_size(nint instance, video_frame_v2_t* videoData);

    [DllImport(LibraryName, EntryPoint = "NDIlib_send_is_keyframe_required", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern unsafe bool send_is_keyframe_required(nint senderPtr, video_frame_v2_t* p_video_data);

    [DllImport(LibraryName, EntryPoint = "NDIlib_send_is_keyframe_required", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool send_is_keyframe_required(nint senderPtr, ref video_frame_v2_t p_video_data);

    [Obsolete("Use send_send_audio_v2 or send_send_audio_v3.")]
    [DllImport(LibraryName, EntryPoint = "NDIlib_send_send_audio", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void send_send_audio(nint instance, audio_frame_t* audioData);

    [Obsolete("Use send_send_audio_v2 or send_send_audio_v3.")]
    [DllImport(LibraryName, EntryPoint = "NDIlib_send_send_audio", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void send_send_audio(nint instance, ref audio_frame_t audioData);

    [DllImport(LibraryName, EntryPoint = "NDIlib_send_send_audio_scatter", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void send_send_audio_scatter(
        nint instance,
        audio_frame_v3_t* audioData,
        frame_scatter_t* audioScatter);

    [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "NDIlib_send_send_audio_v2", ExactSpelling = true)]
    public static extern void send_send_audio_v2(nint p_instance, ref audio_frame_v2_t p_audio_data);

    [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "NDIlib_send_send_audio_v2", ExactSpelling = true)]
    public static extern unsafe void send_send_audio_v2(nint p_instance, audio_frame_v2_t* p_audio_data);

    [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "NDIlib_send_send_audio_v3", ExactSpelling = true)]
    public static extern void send_send_audio_v3(nint p_instance, ref audio_frame_v3_t p_audio_data);


    [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "NDIlib_send_send_audio_v3", ExactSpelling = true)]
    public static extern unsafe void send_send_audio_v3(nint p_instance, audio_frame_v3_t *p_audio_data);

    [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "NDIlib_send_send_metadata", ExactSpelling = true)]
    public static extern void send_send_metadata(nint p_instance, ref metadata_frame_t p_metadata);

    [DllImport(LibraryName, EntryPoint = "NDIlib_send_send_metadata", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void send_send_metadata(nint instance, metadata_frame_t* metadata);

    [Obsolete("Use send_send_video_v2.")]
    [DllImport(LibraryName, EntryPoint = "NDIlib_send_send_video", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void send_send_video(nint instance, video_frame_t* videoData);

    [Obsolete("Use send_send_video_v2.")]
    [DllImport(LibraryName, EntryPoint = "NDIlib_send_send_video", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void send_send_video(nint instance, ref video_frame_t videoData);

    [Obsolete("Use send_send_video_async_v2.")]
    [DllImport(LibraryName, EntryPoint = "NDIlib_send_send_video_async", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void send_send_video_async(nint instance, video_frame_t* videoData);

    [Obsolete("Use send_send_video_async_v2.")]
    [DllImport(LibraryName, EntryPoint = "NDIlib_send_send_video_async", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void send_send_video_async(nint instance, ref video_frame_t videoData);

    [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "NDIlib_send_send_video_async_v2", ExactSpelling = true)]
    public static extern void send_send_video_async_v2(nint p_instance, ref video_frame_v2_t p_video_data);

    [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "NDIlib_send_send_video_async_v2", ExactSpelling = true)]
    public static extern unsafe void send_send_video_async_v2(nint p_instance, video_frame_v2_t* p_video_data);

    // void NDIlib_send_send_video_async_v2(NDIlib_send_instance_t p_instance, const NDIlib_video_frame_v2_t* p_video_data);
    [DllImport(LibraryName, EntryPoint = "NDIlib_send_send_video_async_v2", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void send_send_video_async_v2_adv(nint pInstance, ref video_frame_v2_t pVideoData);

    // void NDIlib_send_send_video_async_v2(NDIlib_send_instance_t p_instance, const NDIlib_video_frame_v2_t* p_video_data);
    [DllImport(LibraryName, EntryPoint = "NDIlib_send_send_video_async_v2", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void send_send_video_async_v2_adv(nint pInstance, video_frame_v2_t *pVideoData);

    [DllImport(LibraryName, EntryPoint = "NDIlib_send_send_video_scatter", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void send_send_video_scatter(
        nint instance,
        video_frame_v2_t* videoData,
        frame_scatter_t* videoScatter);

    [DllImport(LibraryName, EntryPoint = "NDIlib_send_send_video_scatter_async", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void send_send_video_scatter_async(
        nint instance,
        video_frame_v2_t* videoData,
        frame_scatter_t* videoScatter);

    // send_send_video_v2
    [DllImport(LibraryName, EntryPoint = "NDIlib_send_send_video_v2", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void send_send_video_v2(
        nint p_instance,
        ref video_frame_v2_t p_video_data);

    // send_send_video_v2
    [DllImport(LibraryName, EntryPoint = "NDIlib_send_send_video_v2", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static unsafe extern void send_send_video_v2(
        nint p_instance,
        video_frame_v2_t* p_video_data);

    [DllImport(LibraryName, EntryPoint = "NDIlib_send_set_failover", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void send_set_failover(nint instance, ref source_t failoverSource);

    [DllImport(LibraryName, EntryPoint = "NDIlib_send_set_failover", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void send_set_failover(nint instance, source_t* failoverSource);

    [DllImport(LibraryName, EntryPoint = "NDIlib_send_set_tally", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool send_set_tally(nint instance, ref tally_t tally);

    [DllImport(LibraryName, EntryPoint = "NDIlib_send_set_video_async_completion", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static unsafe extern void send_set_video_async_completion(
        nint p_instance,
        nint p_opaque,
        VideoSendAsyncCompletion callback);

    [DllImport(LibraryName, EntryPoint = "NDIlib_send_set_video_async_completion", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void send_set_video_async_completion(
        nint instance,
        nint opaque,
        nint callback);

    [DllImport(LibraryName, EntryPoint = "NDIlib_send_wait_for_keyframe_request", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool send_wait_for_keyframe_request(
        nint instance,
        uint timeoutInMilliseconds,
        video_frame_v2_t* videoData);

    public static string SendGetSourceNameWrapped(nint p_instance)
    {
        var result = send_get_source_name(p_instance);
        var sourceDetails = Marshal.PtrToStructure<source_t>(result);
        var name = UTF.Utf8ToString(sourceDetails.p_ndi_name);

        return name;
    }

    public static void SendMetadataFrame(nint senderPtr, string metadata)
    {
        WriteMetadata(senderPtr, metadata, false);
    }

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate void VideoSendAsyncCompletion(nint p_opaque, in video_frame_v2_t p_video);

    private static void WriteMetadata(nint sender, string metadata, bool connectionMetadata)
    {
        var pointer = UTF.StringToUtf8(metadata, out var byteCount);
        try
        {
            var frame = new metadata_frame_t
            {
                p_data = pointer,
                length = checked(byteCount + 1),
                timecode = send_timecode_synthesize
            };

            if (connectionMetadata)
            {
                send_add_connection_metadata(sender, ref frame);
            }
            else
            {
                send_send_metadata(sender, ref frame);
            }
        }
        finally
        {
            Marshal.FreeHGlobal(pointer);
        }
    }
}

[StructLayoutAttribute(LayoutKind.Sequential)]
public struct send_create_t
{
    // The name of the NDI source to create. This is a NULL terminated UTF8 string.
    public nint p_ndi_name;

    // What groups should this source be part of. NULL means default.
    public nint p_groups;

    // Do you want audio and video to "clock" themselves. When they are clocked then
    // by adding video frames, they will be rate limited to match the current frame-rate
    // that you are submitting at. The same is true for audio. In general if you are submitting
    // video and audio off a single thread then you should only clock one of them (video is
    // probably the better of the two to clock off). If you are submtiting audio and video
    // of separate threads then having both clocked can be useful.
    [MarshalAsAttribute(UnmanagedType.U1)]
    public bool clock_video, clock_audio;
}
