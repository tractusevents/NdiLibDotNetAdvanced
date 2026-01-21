
using System;
using System.Buffers;
using System.Buffers.Binary;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;

namespace Tractus.Ndi;

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public delegate bool CustomVideoAllocatorCallback(nint pOpaque, ref video_frame_v2_t videoData);

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public delegate bool CustomVideoFreeCallback(nint pOpaque, ref video_frame_v2_t videoData);

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public delegate bool CustomAudioAllocatorCallback(nint pOpaque, ref audio_frame_v3_t audioData);

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public delegate bool CustomAudioFreeCallback(nint pOpaque, ref audio_frame_v3_t audioData);

public enum NDIlib_receiver_type_e : int
{
    none = 0,
    metadata = 1,
    video = 2,
    audio = 3,
    max = unchecked((int)0x7fffffff)
}

public enum NDIlib_receiver_command_e : int
{
    none = 0,
    connect = 1,
    max = unchecked((int)0x7fffffff)
}

[StructLayout(LayoutKind.Sequential)]
public struct NDIlib_receiver_t
{
    [MarshalAs(UnmanagedType.LPUTF8Str)]
    public string p_uuid;

    [MarshalAs(UnmanagedType.LPUTF8Str)]
    public string p_name;

    [MarshalAs(UnmanagedType.LPUTF8Str)]
    public string p_input_uuid;

    [MarshalAs(UnmanagedType.LPUTF8Str)]
    public string p_input_name;

    [MarshalAs(UnmanagedType.LPUTF8Str)]
    public string p_address;

    // These come back as C pointers to arrays of enums
    public nint p_streams;
    public uint num_streams;

    public nint p_commands;
    public uint num_commands;

    [MarshalAs(UnmanagedType.I1)]
    public bool events_subscribed;
}

[StructLayout(LayoutKind.Sequential)]
public struct routing_create_t
{
    public nint p_ndi_name;
    public nint p_groups;
}

public struct NDIlib_receiver_t_wrapped
{
    [MarshalAs(UnmanagedType.LPUTF8Str)]
    public string p_uuid;

    [MarshalAs(UnmanagedType.LPUTF8Str)]
    public string p_name;

    [MarshalAs(UnmanagedType.LPUTF8Str)]
    public string p_input_uuid;

    [MarshalAs(UnmanagedType.LPUTF8Str)]
    public string p_input_name;

    [MarshalAs(UnmanagedType.LPUTF8Str)]
    public string p_address;

    // These come back as C pointers to arrays of enums
    public NDIlib_receiver_type_e[] p_streams;
    public uint num_streams;

    public NDIlib_receiver_command_e[] p_commands;
    public uint num_commands;

    [MarshalAs(UnmanagedType.I1)]
    public bool events_subscribed;
}



[StructLayout(LayoutKind.Sequential)]
public struct recv_advertiser_create_t
{
    [MarshalAs(UnmanagedType.LPUTF8Str)]
    public string p_url_address;
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

[StructLayout(LayoutKind.Sequential)]
public struct NDIlib_recv_listener_create_t
{
    [MarshalAs(UnmanagedType.LPUTF8Str)]
    public string p_url_address;
}

[StructLayout(LayoutKind.Sequential)]
public struct NDIlib_recv_listener_event
{
    [MarshalAs(UnmanagedType.LPUTF8Str)]
    public string p_uuid;

    [MarshalAs(UnmanagedType.LPUTF8Str)]
    public string p_name;

    [MarshalAs(UnmanagedType.LPUTF8Str)]
    public string p_value;
}


[SuppressUnmanagedCodeSecurity]
public static unsafe partial class NDIWrapper
{
    public static Int64 send_timecode_synthesize = Int64.MaxValue;
    public static Int64 recv_timestamp_undefined = Int64.MaxValue;

    // Based on how SDLSharp does things.
    // https://github.com/GabrielFrigo4/SDL-Sharp/blob/3daad4b05c11c1a3987ae24c12c78092be3aa9c3/SDL-Sharp/SDL/SDL.Loader.cs#L11

    private const string LibraryName = "NdiAdv";

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_advertiser_create_ex", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern nint recv_advertiser_create_ex(
        ref recv_advertiser_create_t createSettings,
        [MarshalAs(UnmanagedType.LPUTF8Str)] string configData);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_listener_create_ex", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern nint recv_listener_create_ex(
    ref NDIlib_recv_listener_create_t listenerCreateSettings,
    [MarshalAs(UnmanagedType.LPUTF8Str)] string configData);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_listener_create", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern nint recv_listener_create(
        ref NDIlib_recv_listener_create_t listenerCreateSettings);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_listener_destroy", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void recv_listener_destroy(nint pInstance);


    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_listener_is_connected", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern bool recv_listener_is_connected(nint pInstance);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_listener_get_server_url", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern nint recv_listener_get_server_url(nint pInstance);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_listener_get_receivers", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern nint recv_listener_get_receivers(
        nint pInstance,
        out uint numListeners);

    // find_get_current_sources 
    [DllImport(LibraryName, EntryPoint = "NDIlib_find_get_current_sources", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    internal static extern nint find_get_current_sources(nint p_instance, ref uint p_no_sources);

    // NDIlib_find_get_current_sources_v2
    // find_get_current_sources 
    [DllImport(LibraryName, EntryPoint = "NDIlib_find_get_current_sources_v2", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    internal static extern nint find_get_current_sources_v2(nint p_instance, out uint p_no_sources);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_listener_wait_for_receivers", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern nint recv_listener_wait_for_receivers(
        nint pInstance,
        uint timeoutMsec);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_listener_subscribe_events", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void recv_listener_subscribe_events(
        nint pInstance,
        [MarshalAs(UnmanagedType.LPUTF8Str)]string p_receiver_uuid);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_listener_subscribe_events", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void recv_listener_subscribe_events(
    nint pInstance,
    nint p_receiver_uuid);


    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_listener_unsubscribe_events", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void recv_listener_unsubscribe_events(
    nint pInstance,
    [MarshalAs(UnmanagedType.LPUTF8Str)] string p_receiver_uuid);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_listener_unsubscribe_events", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void recv_listener_unsubscribe_events(
        nint pInstance,
        nint p_receiver_uuid);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_listener_get_events", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern nint recv_listener_get_events(
        nint pInstance,
        out uint pNumEvents,
        uint timeoutMsec);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_listener_free_events", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern nint recv_listener_free_events(
        nint pInstance,
        nint pEvents);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_listener_send_connect", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern bool recv_listener_send_connect(
        nint p_instance,
        [MarshalAs(UnmanagedType.LPUTF8Str)] string p_receiver_uuid,
        [MarshalAs(UnmanagedType.LPUTF8Str)] string p_source_name
    );

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_listener_send_connect", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern bool recv_listener_send_connect(
    nint p_instance,
    nint p_receiver_uuid,
    nint p_source_name
    );


    public static NDIlib_recv_listener_event[] GetListenerEvents(
        nint instance,
        uint timeoutInMilliseconds,
        out uint numEvents
)
    {
        nint pEvents = recv_listener_get_events(instance, out numEvents, timeoutInMilliseconds);
        if (pEvents == nint.Zero || numEvents == 0)
        {
            numEvents = 0;
            return Array.Empty<NDIlib_recv_listener_event>();
        }

        int structSize = Marshal.SizeOf<NDIlib_recv_listener_event>();
        var managed = new NDIlib_recv_listener_event[numEvents];
        for (uint i = 0; i < numEvents; i++)
        {
            var ptr = nint.Add(pEvents, (int)(i * structSize));
            managed[i] = Marshal.PtrToStructure<NDIlib_recv_listener_event>(ptr);
        }

        // free the native array
        recv_listener_free_events(instance, pEvents);
        return managed;
    }


    public static NDIlib_receiver_t_wrapped[] recv_listener_get_receivers_wrapped(
            nint p_instance,
            out UInt32 numReceivers
        )
    {
        var pArray = recv_listener_get_receivers(p_instance, out numReceivers);
        if (pArray == nint.Zero || numReceivers == 0)
        {
            return Array.Empty<NDIlib_receiver_t_wrapped>();
        }

        var size = Marshal.SizeOf<NDIlib_receiver_t>();
        var receivers = new NDIlib_receiver_t_wrapped[numReceivers];
        for (uint i = 0; i < numReceivers; i++)
        {
            nint pElem = nint.Add(pArray, (int)(i * size));
            var toWrap = Marshal.PtrToStructure<NDIlib_receiver_t>(pElem);

            var toAdd = new NDIlib_receiver_t_wrapped
            {
                events_subscribed = toWrap.events_subscribed,
                num_commands = toWrap.num_commands,
                num_streams = toWrap.num_streams,
                p_address = toWrap.p_address,
                p_input_name = toWrap.p_input_name,
                p_input_uuid = toWrap.p_input_uuid,
                p_name = toWrap.p_name,
                p_uuid = toWrap.p_uuid,
            };

            if(toWrap.p_commands == nint.Zero)
            {
                toAdd.p_commands = Array.Empty<NDIlib_receiver_command_e>();                
            }
            else
            {
                var commandArray = new NDIlib_receiver_command_e[toWrap.num_commands];
                for(var ci = 0; ci < toWrap.num_commands; ci++)
                {
                    var rawValue = Marshal.ReadInt32(toWrap.p_commands, ci * sizeof(int));
                    commandArray[ci] = (NDIlib_receiver_command_e)Enum.ToObject(typeof(NDIlib_receiver_command_e), rawValue);
                }

                toAdd.p_commands = commandArray;
            }

            if(toWrap.p_streams == nint.Zero)
            {
                toAdd.p_streams = Array.Empty<NDIlib_receiver_type_e>();
            }
            else
            {
                var streamArray = new NDIlib_receiver_type_e[toWrap.num_streams];
                for (var ci = 0; ci < toWrap.num_streams; ci++)
                {
                    var rawValue = Marshal.ReadInt32(toWrap.p_streams, ci * sizeof(int));
                    streamArray[ci] = (NDIlib_receiver_type_e)Enum.ToObject(typeof(NDIlib_receiver_type_e), rawValue);
                }

                toAdd.p_streams = streamArray;
            }

            receivers[i] = toAdd;
        }
        return receivers;
    }

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_ptz_zoom", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAsAttribute(UnmanagedType.U1)]
    public static extern bool recv_ptz_zoom(nint p_instance, float zoom_value);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_ptz_zoom_speed", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAsAttribute(UnmanagedType.U1)]
    public static extern bool recv_ptz_zoom_speed(nint p_instance, float zoom_speed);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_ptz_pan_tilt", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAsAttribute(UnmanagedType.U1)]
    public static extern bool recv_ptz_pan_tilt(nint p_instance, float pan_value, float tilt_value);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_ptz_pan_tilt_speed", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAsAttribute(UnmanagedType.U1)]
    public static extern bool recv_ptz_pan_tilt_speed(nint p_instance, float pan_speed, float tilt_speed);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_ptz_auto_focus", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAsAttribute(UnmanagedType.U1)]
    public static extern bool recv_ptz_auto_focus(nint p_instance);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_ptz_focus_speed", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAsAttribute(UnmanagedType.U1)]
    public static extern bool recv_ptz_focus_speed(nint p_instance, float focus_speed);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_ptz_focus", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAsAttribute(UnmanagedType.U1)]
    public static extern bool recv_ptz_focus(nint p_instance, float focus_value);


    [DllImport(LibraryName, EntryPoint = "NDIlib_avsync_create", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern nint avsync_create(nint p_receiver);

    [DllImport(LibraryName, EntryPoint = "NDIlib_avsync_destroy", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void avsync_destroy(nint p_avsync);

    [DllImport(LibraryName, EntryPoint = "NDIlib_avsync_synchronize", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern avsync_ret_e avsync_synchronize(nint p_avsync, ref video_frame_v2_t p_video_frame, ref audio_frame_v3_t p_audio_frame);

    [DllImport(LibraryName, EntryPoint = "NDIlib_avsync_synchronize", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern unsafe avsync_ret_e avsync_synchronize(
        nint p_avsync, 
        video_frame_v2_t* p_video_frame, 
        audio_frame_v3_t* p_audio_frame);

    [DllImport(LibraryName, EntryPoint = "NDIlib_avsync_free_audio", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern unsafe void avsync_free_audio(
        nint p_avsync,
        ref audio_frame_v3_t p_audio_frame);


    [DllImport(LibraryName, EntryPoint = "NDIlib_avsync_free_audio", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern unsafe void avsync_free_audio(
        nint p_avsync,
        audio_frame_v3_t* p_audio_frame);


    [DllImport(LibraryName, EntryPoint = "NDIlib_util_send_send_audio_interleaved_32f", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void util_send_send_audio_interleaved_32f(
        nint p_instance,
        ref audio_frame_interleaved_32f_t p_audio_data);

    [DllImport(LibraryName, EntryPoint = "NDIlib_util_send_send_audio_interleaved_16s", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void util_send_send_audio_interleaved_16s(
    nint p_instance,
    ref audio_frame_interleaved_16s_t p_audio_data);

    [DllImport(LibraryName, EntryPoint = "NDIlib_util_send_send_audio_interleaved_32s", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void util_send_send_audio_interleaved_32s(
    nint p_instance,
    ref audio_frame_interleaved_32s_t p_audio_data);

    [DllImport(LibraryName, EntryPoint = "NDIlib_util_audio_to_interleaved_16s_v2", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void util_audio_to_interleaved_16s_v2(
    ref audio_frame_v2_t pSrc,
    ref audio_frame_interleaved_16s_t pDst);

    [DllImport(LibraryName, EntryPoint = "NDIlib_util_audio_to_interleaved_16s_v3", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void util_audio_to_interleaved_16s_v3(
        ref audio_frame_v3_t pSrc,
        ref audio_frame_interleaved_16s_t pDst);

    [DllImport(LibraryName, EntryPoint = "NDIlib_util_audio_from_interleaved_16s_v2", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void util_audio_from_interleaved_16s_v2(
        ref audio_frame_interleaved_16s_t pSrc,
        ref audio_frame_v2_t pDst);

    [DllImport(LibraryName, EntryPoint = "NDIlib_util_audio_from_interleaved_16s_v3", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void util_audio_from_interleaved_16s_v3(
        ref audio_frame_interleaved_16s_t pSrc,
        ref audio_frame_v3_t pDst);

    [DllImport(LibraryName, EntryPoint = "NDIlib_util_audio_to_interleaved_32s_v2", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void util_audio_to_interleaved_32s_v2(
    ref audio_frame_v2_t pSrc,
    ref audio_frame_interleaved_32s_t pDst);


    [DllImport(LibraryName, EntryPoint = "NDIlib_util_audio_to_interleaved_32s_v3", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void util_audio_to_interleaved_32s_v3(
    ref audio_frame_v3_t pSrc,
    ref audio_frame_interleaved_32s_t pDst);

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

    // V210 -> P216 (video)
    [DllImport(LibraryName, EntryPoint = "NDIlib_util_V210_to_P216", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void util_V210_to_P216(
        ref video_frame_v2_t pSrcV210,
        ref video_frame_v2_t pDstP216);

    // P216 -> V210 (video)
    [DllImport(LibraryName, EntryPoint = "NDIlib_util_P216_to_V210", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void util_P216_to_V210(
        ref video_frame_v2_t pSrcP216,
        ref video_frame_v2_t pDstV210);


    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_kvm_send_clipboard_contents", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern bool recv_kvm_send_clipboard_contents(
        nint p_instance, [MarshalAs(UnmanagedType.LPUTF8Str)]string p_clipboard_contents);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_kvm_send_left_mouse_click", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern bool recv_kvm_send_left_mouse_click(
        nint p_instance);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_kvm_send_middle_mouse_click", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern bool recv_kvm_send_middle_mouse_click(
    nint p_instance);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_kvm_send_right_mouse_click", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern bool recv_kvm_send_right_mouse_click(
        nint p_instance);


    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_kvm_send_left_mouse_release", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern bool recv_kvm_send_left_mouse_release(
        nint p_instance);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_kvm_send_middle_mouse_release", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern bool recv_kvm_send_middle_mouse_release(
    nint p_instance);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_kvm_send_right_mouse_release", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern bool recv_kvm_send_right_mouse_release(
        nint p_instance);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_kvm_send_vertical_mouse_wheel", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern bool recv_kvm_send_vertical_mouse_wheel(
    nint p_instance,
    float no_units);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_kvm_send_horizontal_mouse_wheel", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern bool recv_kvm_send_horizontal_mouse_wheel(
        nint p_instance,
        float no_units);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_kvm_send_mouse_position", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern bool recv_kvm_send_mouse_position(
    nint p_instance,
        [In, MarshalAs(UnmanagedType.LPArray, SizeConst = 2)]
        float[] posn);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_kvm_send_keyboard_press", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern bool recv_kvm_send_keyboard_press(
        nint p_instance, int key_sym_value);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_kvm_send_keyboard_release", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern bool recv_kvm_send_keyboard_release(
    nint p_instance, int key_sym_value);


    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_connect", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void recv_connect(
        nint p_instance,
        ref source_t source);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_connect", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void recv_connect(
        nint p_instance,
        nint source);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_get_source_name", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern bool recv_get_source_name(
        nint p_instance,
        out nint p_name,
        uint timeoutMs);

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

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_advertiser_create", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern nint recv_advertiser_create(
        ref recv_advertiser_create_t p_create_settings);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_advertiser_destroy", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern nint recv_advertiser_destroy(
        nint p_instance);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_advertiser_add_receiver", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern bool recv_advertiser_add_receiver(
        nint p_instance,
        nint p_receiver,
        bool allow_controlling,
        bool allow_monitoring,
        [MarshalAs(UnmanagedType.LPUTF8Str)]string p_input_group_name);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_advertiser_add_receiver", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern bool recv_advertiser_add_receiver(
        nint p_instance,
        nint p_receiver,
        bool allow_controlling,
        bool allow_monitoring,
        nint p_input_group_name);


    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_advertiser_del_receiver", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern bool recv_advertiser_del_receiver(
        nint p_instance,
        nint p_receiver);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_set_tally", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern bool recv_set_tally(
        nint p_instance,
        ref tally_t p_tally);


    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_set_video_allocator", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void recv_set_video_allocator(
        nint pinstance, 
        nint pOpaque,
        CustomVideoAllocatorCallback allocator,
        CustomVideoFreeCallback freer);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_set_audio_allocator", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void recv_set_audio_allocator(
        nint pinstance,
        nint pOpaque,
        CustomAudioAllocatorCallback allocator,
        CustomAudioFreeCallback freer);


    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_free_video_v2", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void recv_free_video_v2(nint p_instance, ref video_frame_v2_t p_video_data);
    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_free_audio_v3", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void recv_free_audio_v3(nint p_instance, ref audio_frame_v3_t p_audio_data);
    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_free_metadata", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void recv_free_metadata(nint p_instance, ref metadata_frame_t p_metadata);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_free_video_v2", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static unsafe extern void recv_free_video_v2(nint p_instance, video_frame_v2_t* p_video_data);
    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_free_audio_v3", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static unsafe extern void recv_free_audio_v3(nint p_instance, audio_frame_v3_t* p_audio_data);
    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_free_metadata", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static unsafe extern void recv_free_metadata(nint p_instance, metadata_frame_t* p_metadata);




    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_free_string", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void recv_free_string(nint p_instance, nint p_string);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_ptz_is_supported", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern bool recv_ptz_is_supported(nint p_instance);

    //NDIlib_send_add_connection_metadata
    [DllImport(LibraryName, EntryPoint = "NDIlib_send_add_connection_metadata", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void send_add_connection_metadata(
        nint pInstance,
        ref metadata_frame_t metadataFrame);

    //NDIlib_send_add_connection_metadata
    [DllImport(LibraryName, EntryPoint = "NDIlib_send_clear_connection_metadata", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void send_clear_connection_metadata(
        nint pInstance);

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

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_capture_v3", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static unsafe extern frame_type_e recv_capture_v3(
        nint p_instance,
        out nint p_video_data,
        out nint p_audio_data,
        out nint p_metadata,
        uint timeout_in_ms);

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

    [DllImport(LibraryName, EntryPoint = "NDIlib_send_free_metadata", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void send_free_metadata(
        nint pInstance,
        ref metadata_frame_t pMetadata);

    [DllImport(LibraryName, EntryPoint = "NDIlib_send_free_metadata", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void send_free_metadata(
        nint pInstance,
        metadata_frame_t* pMetadata);

    // recv_capture_v3 
    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_capture_v3", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public unsafe static extern frame_type_e recv_capture_v3(
        nint p_instance,
        video_frame_v2_t* p_video_data,
        audio_frame_v3_t* p_audio_data,
        metadata_frame_t* p_metadata,
        int timeout_in_ms);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_send_metadata", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern bool recv_send_metadata(
        nint pInstance,
        ref metadata_frame_t metadataFrame);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_destroy", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void recv_destroy(nint p_instance);


    // framesync_create_v2 
    [DllImport(LibraryName, EntryPoint = "NDIlib_framesync_create", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern nint framesync_create(nint p_receiver);

    // framesync_destroy 
    [DllImport(LibraryName, EntryPoint = "NDIlib_framesync_destroy", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void framesync_destroy(nint p_instance);

    // framesync_capture_audio 
    [DllImport(LibraryName, EntryPoint = "NDIlib_framesync_capture_audio", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern nint framesync_capture_audio(nint p_instance, ref audio_frame_v2_t p_audio_data, int sample_rate, int no_channels, int no_samples);

    // framesync_capture_audio_v2
    [DllImport(LibraryName, EntryPoint = "NDIlib_framesync_capture_audio_v2", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern nint framesync_capture_audio_v2(nint p_instance, ref audio_frame_v3_t p_audio_data, int sample_rate, int no_channels, int no_samples);

    // framesync_free_audio 
    [DllImport(LibraryName, EntryPoint = "NDIlib_framesync_free_audio", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void framesync_free_audio(nint p_instance, ref audio_frame_v2_t p_audio_data);

    // framesync_free_audio_v2
    [DllImport(LibraryName, EntryPoint = "NDIlib_framesync_free_audio_v2", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void framesync_free_audio_v2(nint p_instance, ref audio_frame_v3_t p_audio_data);

    // framesync_audio_queue_depth 
    [DllImport(LibraryName, EntryPoint = "NDIlib_framesync_audio_queue_depth", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern int framesync_audio_queue_depth(nint p_instance);

    // framesync_capture_video 
    [DllImport(LibraryName, EntryPoint = "NDIlib_framesync_capture_video", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void framesync_capture_video(nint p_instance, ref video_frame_v2_t p_video_data, frame_format_type_e field_type);

    // framesync_free_video 
    [DllImport(LibraryName, EntryPoint = "NDIlib_framesync_free_video", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void framesync_free_video(nint p_instance, ref video_frame_v2_t p_video_data);

    // framesync_capture_video 
    [DllImport(LibraryName, EntryPoint = "NDIlib_framesync_capture_video", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public unsafe static extern void framesync_capture_video(nint p_instance, video_frame_v2_t* p_video_data, frame_format_type_e field_type);

    // framesync_free_video 
    [DllImport(LibraryName, EntryPoint = "NDIlib_framesync_free_video", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public unsafe static extern void framesync_free_video(nint p_instance, video_frame_v2_t* p_video_data);


    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_kvm_is_supported", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern bool recv_kvm_is_supported(nint pInstance);





    [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "NDIlib_send_create", ExactSpelling = true)]
    public static extern nint send_create(ref send_create_t p_create_settings);


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

    [DllImport(LibraryName, EntryPoint = "NDIlib_send_set_video_async_completion", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static unsafe extern void send_set_video_async_completion(
        nint p_instance,
        nint p_opaque,
        VideoSendAsyncCompletion callback);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate void VideoSendAsyncCompletion(nint p_opaque, in video_frame_v2_t p_video);


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


    [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "NDIlib_send_send_audio_v3", ExactSpelling = true)]
    public static extern void send_send_audio_v3(nint p_instance, ref audio_frame_v3_t p_audio_data);
    
    [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "NDIlib_send_send_audio_v3", ExactSpelling = true)]
    public static extern unsafe void send_send_audio_v3(nint p_instance, audio_frame_v3_t *p_audio_data);


    [DllImport(LibraryName, EntryPoint = "NDIlib_find_create_v3", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern nint find_create_v3(
        ref find_create_t createSettings,
        nint configData);

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


    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_create_v3", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern nint recv_create_v3(
        ref recv_create_v3_t createSettings);


    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_set_bandwidth", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern bool recv_set_bandwidth(
        nint instance,
        recv_bandwidth_e bandwidth);

    [DllImport(LibraryName, EntryPoint = "NDIlib_find_destroy", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void find_destroy(
        nint p_instance);

    [DllImport(LibraryName, EntryPoint = "NDIlib_find_wait_for_sources", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAsAttribute(UnmanagedType.U1)]
    public static extern bool find_wait_for_sources(nint p_instance, UInt32 timeout_in_ms);


    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_request_keyframe", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern bool recv_request_keyframe(nint recvPtr);

    [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "NDIlib_recv_get_no_connections", ExactSpelling = true)]
    public static extern int recv_get_no_connections(nint p_instance);

    [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "NDIlib_recv_get_web_control", ExactSpelling = true)]
    public static extern nint recv_get_web_control(nint p_instance);

    [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "NDIlib_send_send_metadata", ExactSpelling = true)]
    public static extern void send_send_metadata(nint p_instance, ref metadata_frame_t p_metadata);

    
    [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "NDIlib_recv_ptz_store_preset", ExactSpelling = true)]
    public static extern bool recv_ptz_store_preset(nint p_instance, int preset_no);

    [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "NDIlib_recv_ptz_recall_preset", ExactSpelling = true)]
    [return: MarshalAs(UnmanagedType.U1)]
    public static extern bool recv_ptz_recall_preset(nint p_instance, int preset_no, float speed);

    // find_create_v2 
    [DllImport(LibraryName, EntryPoint = "NDIlib_find_create_v2", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern nint find_create_v2(ref find_create_t p_create_settings);

    // find_create_v2 
    [DllImport(LibraryName, EntryPoint = "NDIlib_genlock_create", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern nint genlock_create(ref source_t p_src_settings, nint configData);

    [DllImport(LibraryName, EntryPoint = "NDIlib_genlock_create", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern unsafe nint genlock_create(source_t* p_src_settings, nint configData);


    [DllImport(LibraryName, EntryPoint = "NDIlib_genlock_destroy", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void genlock_destroy(nint p_instance);

    [DllImport(LibraryName, EntryPoint = "NDIlib_genlock_connect", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void genlock_connect(nint p_instance, ref source_t p_src_settings);
    [DllImport(LibraryName, EntryPoint = "NDIlib_genlock_connect", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern unsafe void genlock_connect(nint p_instance, source_t* p_src_settings);

    [DllImport(LibraryName, EntryPoint = "NDIlib_genlock_is_active", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern bool genlock_is_active(nint p_instance);


    [DllImport(LibraryName, EntryPoint = "NDIlib_genlock_wait_video", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern bool genlock_wait_video(nint p_instance, ref video_frame_v2_t p_video_data);

    [DllImport(LibraryName, EntryPoint = "NDIlib_genlock_wait_audio", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern bool genlock_wait_audio(nint p_instance, ref audio_frame_v3_t p_audio_data);

    [DllImport(LibraryName, EntryPoint = "NDIlib_genlock_wait_video", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern unsafe bool genlock_wait_video(nint p_instance, video_frame_v2_t* p_video_data);

    [DllImport(LibraryName, EntryPoint = "NDIlib_genlock_wait_audio", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern unsafe bool genlock_wait_audio(nint p_instance, audio_frame_v3_t* p_audio_data);


    [DllImport(LibraryName, EntryPoint = "NDIlib_send_get_source_name", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern nint send_get_source_name(nint p_instance);


    [DllImport(LibraryName, EntryPoint = "NDIlib_routing_create", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern nint routing_create(ref routing_create_t p_create_settings);

    [DllImport(LibraryName, EntryPoint = "NDIlib_routing_create", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern unsafe nint routing_create(routing_create_t* p_create_settings);

    [DllImport(LibraryName, EntryPoint = "NDIlib_routing_destroy", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern unsafe void routing_destroy(nint p_instance);

    [DllImport(LibraryName, EntryPoint = "NDIlib_routing_clear", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern unsafe bool routing_clear(nint p_instance);

    [DllImport(LibraryName, EntryPoint = "NDIlib_routing_change", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern bool routing_change(nint p_instance, ref source_t source);

    [DllImport(LibraryName, EntryPoint = "NDIlib_routing_change", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern unsafe bool routing_change(nint p_instance, source_t* source);

    [DllImport(LibraryName, EntryPoint = "NDIlib_routing_get_no_connections", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern int routing_get_no_connections(nint p_instance);

    [DllImport(LibraryName, EntryPoint = "NDIlib_routing_get_source_name", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern nint routing_get_source_name(nint p_instance);


    [DllImport(LibraryName, EntryPoint = "NDIlib_routing_create_v2", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern nint routing_create_v2(ref routing_create_t p_create_settings, nint p_config_data);

    [DllImport(LibraryName, EntryPoint = "NDIlib_routing_create_v2", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public unsafe static extern nint routing_create_v2(routing_create_t* p_create_settings, nint p_config_data);


    [DllImport(LibraryName, EntryPoint = "NDIlib_routing_add_connection_metadata", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void routing_add_connection_metadata(nint p_instance, ref metadata_frame_t p_metadata);

    [DllImport(LibraryName, EntryPoint = "NDIlib_routing_add_connection_metadata", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern unsafe void routing_add_connection_metadata(nint p_instance, metadata_frame_t* p_metadata);

    [DllImport(LibraryName, EntryPoint = "NDIlib_routing_clear_connection_metadata", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void routing_clear_connection_metadata(nint p_instance);


    #region NDI 6.3

    [DllImport(LibraryName, EntryPoint = "NDIlib_send_advertiser_create", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern nint send_advertiser_create(ref send_advertiser_create_t p_create_settings);

    [DllImport(LibraryName, EntryPoint = "NDIlib_send_advertiser_create", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public unsafe static extern nint send_advertiser_create(send_advertiser_create_t* p_create_settings);

    [DllImport(LibraryName, EntryPoint = "NDIlib_send_advertiser_destroy", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void send_advertiser_destroy(nint p_instance);

    [DllImport(LibraryName, EntryPoint = "NDIlib_send_advertiser_add_sender", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern bool send_advertiser_add_sender(
        nint p_instance,
        nint p_sender,
        bool allow_monitoring);

    [DllImport(LibraryName, EntryPoint = "NDIlib_send_advertiser_del_sender", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern bool send_advertiser_del_sender(
        nint p_instance,
        nint p_sender);

    [DllImport(LibraryName, EntryPoint = "NDIlib_send_listener_create", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern nint send_listener_create(ref send_listener_create_t p_create_settings);

    [DllImport(LibraryName, EntryPoint = "NDIlib_send_listener_create", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static unsafe extern nint send_listener_create(send_listener_create_t* p_create_settings);

    [DllImport(LibraryName, EntryPoint = "NDIlib_send_listener_destroy", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void send_listener_destroy(nint p_instance);

    [DllImport(LibraryName, EntryPoint = "NDIlib_send_listener_get_server_url", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern nint send_listener_get_server_url(nint p_instance);

    [DllImport(LibraryName, EntryPoint = "NDIlib_send_listener_get_senders", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern nint send_listener_get_senders(nint p_instance, out uint p_num_senders);

    [DllImport(LibraryName, EntryPoint = "NDIlib_send_listener_wait_for_senders", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern bool send_listener_wait_for_senders(nint p_instance, uint timeout_in_ms);

    #endregion

    public static string RoutingGetSourceNameWrapped(nint p_instance)
    {
        var result = routing_get_source_name(p_instance);
        var sourceDetails = Marshal.PtrToStructure<source_t>(result);
        var name = UTF.Utf8ToString(sourceDetails.p_ndi_name);

        return name;
    }

    public static string SendGetSourceNameWrapped(nint p_instance)
    {
        var result = send_get_source_name(p_instance);
        var sourceDetails = Marshal.PtrToStructure<source_t>(result);
        var name = UTF.Utf8ToString(sourceDetails.p_ndi_name);

        return name;
    }

    /// <summary>
    /// Sends a metadata frame to any subscribed listeners.
    /// </summary>
    /// <param name="senderPtr">The sender pointer.</param>
    /// <param name="metadata">The metadata XML to be sent. Should be null-terminated.</param>
    public static unsafe void SendMetadataFrame(nint senderPtr, string metadata)
    {
        var metadataXml = metadata;
        if (metadataXml[metadataXml.Length -1] != '\0')
        {
            metadataXml += "\0";
        }

        var metaFrame = new metadata_frame_t()
        {
            p_data = UTF.StringToUtf8(metadataXml),
            timecode = NDIWrapper.send_timecode_synthesize,
        };

        send_send_metadata(senderPtr, ref metaFrame);

        send_free_metadata(senderPtr, ref metaFrame);
    }

    public static unsafe void AddMetadata(nint senderPtr, string[] metadata, bool replaceAll = false)
    {
        if (replaceAll)
        {
            send_clear_connection_metadata(senderPtr);
        }

        foreach(var item in metadata)
        {
            var metadataXml = item;
            if (metadataXml[metadataXml.Length - 1] != '\0')
            {
                metadataXml = metadataXml + "\0";
            }

            var metadataPtr = UTF.StringToUtf8(metadataXml);

            var metaFrame = new metadata_frame_t()
            {
                p_data = metadataPtr,
            };

            send_add_connection_metadata(senderPtr, ref metaFrame);

            Marshal.FreeHGlobal(metadataPtr);
        }
    }


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

    private static bool useAdvanced;
    private static string? exactLookupPath;

    public static void Initialize(bool useAdvancedDynLib, string? exactLibLookupPath = null)
    {
        exactLookupPath = exactLibLookupPath;
        useAdvanced = useAdvancedDynLib;
        NativeLibrary.SetDllImportResolver(typeof(NDIWrapper).Assembly, ResolveDllImport);
    }

    public static string GetRuntimeIdentifier() 
    {
        var arch = "unknown";
        switch(RuntimeInformation.ProcessArchitecture)
        {
            case Architecture.X86: 
                arch = "x86";
                break;
            case Architecture.X64:
                arch = "x86-64";
                break;
            case Architecture.Arm:
                arch = "arm32";
                break;
            case Architecture.Arm64:
                arch = "arm64";
                break;
            case Architecture.Wasm:
                arch = "wasm";
                break;
            default:
                arch = RuntimeInformation.ProcessArchitecture.ToString();
                break;
        }

        var platform = "unknown";
        if(RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            platform = "win";
        }
        else if(RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            platform = "osx";
        }
        else if(RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            platform = "linux";
        }
        else if(RuntimeInformation.IsOSPlatform(OSPlatform.FreeBSD))
        {
            platform = "freebsd";
        }

        return $"{platform}-{arch}";
    }

	private static nint ResolveDllImport(string libraryName, Assembly assembly, DllImportSearchPath? searchPath)
	{
        if(libraryName != LibraryName)
        {
            return nint.Zero;
        }

        if (!string.IsNullOrEmpty(exactLookupPath))
        {
            var forcedHandle = nint.Zero;
            NativeLibrary.TryLoad(exactLookupPath, out forcedHandle);
            return forcedHandle;
        }

		var libName = string.Empty;
        var useAlternateLoadLogic = false;

		if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
		{
			if (RuntimeInformation.ProcessArchitecture == Architecture.X64)
			{
				libName = 
                    useAdvanced ? "Processing.NDI.Lib.Advanced.x64.dll" : "Processing.NDI.Lib.x64.dll";
			}
			else
			{
				throw new NotImplementedException("Non-x86-based arch not supported on Windows.");
			}
		}
		else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
		{
            useAlternateLoadLogic = true;
			libName = useAdvanced ? "libndi_adv.so" : "libndi.so";
		}
		else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
		{
			libName = useAdvanced ? "libndi_advanced.dylib" : "libndi.dynlib";
		}
		else
		{
			throw new NotImplementedException($"{RuntimeInformation.OSDescription} not supported.");
		}

        var handle = nint.Zero;

        if(useAlternateLoadLogic)
        {
            var libPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                libName
            );

            NativeLibrary.TryLoad(libPath, out handle);
        }
        else
        {
    		NativeLibrary.TryLoad(libName, out handle);
        }


		return handle;
    }
}

public static class NDIConstants
{
    // FourCC constants for video frame types
    public const uint NDIlib_FourCC_video_type_ex_SHQ0_highest_bandwidth =
          ((uint)'S') | (((uint)'H') << 8) | (((uint)'Q') << 16) | (((uint)'0') << 24);
    public const uint NDIlib_FourCC_type_SHQ0_highest_bandwidth = NDIlib_FourCC_video_type_ex_SHQ0_highest_bandwidth;

    public const uint NDIlib_FourCC_video_type_ex_SHQ2_highest_bandwidth =
          ((uint)'S') | (((uint)'H') << 8) | (((uint)'Q') << 16) | (((uint)'2') << 24);
    public const uint NDIlib_FourCC_type_SHQ2_highest_bandwidth = NDIlib_FourCC_video_type_ex_SHQ2_highest_bandwidth;

    public const uint NDIlib_FourCC_video_type_ex_SHQ7_highest_bandwidth =
          ((uint)'S') | (((uint)'H') << 8) | (((uint)'Q') << 16) | (((uint)'7') << 24);
    public const uint NDIlib_FourCC_type_SHQ7_highest_bandwidth = NDIlib_FourCC_video_type_ex_SHQ7_highest_bandwidth;

    public const uint NDIlib_FourCC_video_type_ex_SHQ0_lowest_bandwidth =
          ((uint)'s') | (((uint)'h') << 8) | (((uint)'q') << 16) | (((uint)'0') << 24);
    public const uint NDIlib_FourCC_type_SHQ0_lowest_bandwidth = NDIlib_FourCC_video_type_ex_SHQ0_lowest_bandwidth;

    public const uint NDIlib_FourCC_video_type_ex_SHQ2_lowest_bandwidth =
          ((uint)'s') | (((uint)'h') << 8) | (((uint)'q') << 16) | (((uint)'2') << 24);
    public const uint NDIlib_FourCC_type_SHQ2_lowest_bandwidth = NDIlib_FourCC_video_type_ex_SHQ2_lowest_bandwidth;

    public const uint NDIlib_FourCC_video_type_ex_SHQ7_lowest_bandwidth =
          ((uint)'s') | (((uint)'h') << 8) | (((uint)'q') << 16) | (((uint)'7') << 24);
    public const uint NDIlib_FourCC_type_SHQ7_lowest_bandwidth = NDIlib_FourCC_video_type_ex_SHQ7_lowest_bandwidth;

    public const uint NDIlib_FourCC_video_type_ex_H264_highest_bandwidth =
          ((uint)'H') | (((uint)'2') << 8) | (((uint)'6') << 16) | (((uint)'4') << 24);
    public const uint NDIlib_FourCC_type_H264_highest_bandwidth = NDIlib_FourCC_video_type_ex_H264_highest_bandwidth;

    public const uint NDIlib_FourCC_video_type_ex_H264_lowest_bandwidth =
          ((uint)'h') | (((uint)'2') << 8) | (((uint)'6') << 16) | (((uint)'4') << 24);
    public const uint NDIlib_FourCC_type_H264_lowest_bandwidth = NDIlib_FourCC_video_type_ex_H264_lowest_bandwidth;

    public const uint NDIlib_FourCC_video_type_ex_HEVC_highest_bandwidth =
          ((uint)'H') | (((uint)'E') << 8) | (((uint)'V') << 16) | (((uint)'C') << 24);
    public const uint NDIlib_FourCC_type_HEVC_highest_bandwidth = NDIlib_FourCC_video_type_ex_HEVC_highest_bandwidth;

    public const uint NDIlib_FourCC_video_type_ex_HEVC_lowest_bandwidth =
          ((uint)'h') | (((uint)'e') << 8) | (((uint)'v') << 16) | (((uint)'c') << 24);
    public const uint NDIlib_FourCC_type_HEVC_lowest_bandwidth = NDIlib_FourCC_video_type_ex_HEVC_lowest_bandwidth;

    public const uint NDIlib_FourCC_video_type_ex_H264_alpha_highest_bandwidth =
          ((uint)'A') | (((uint)'2') << 8) | (((uint)'6') << 16) | (((uint)'4') << 24);
    public const uint NDIlib_FourCC_type_h264_alpha_highest_bandwidth = NDIlib_FourCC_video_type_ex_H264_alpha_highest_bandwidth;

    public const uint NDIlib_FourCC_video_type_ex_H264_alpha_lowest_bandwidth =
          ((uint)'a') | (((uint)'2') << 8) | (((uint)'6') << 16) | (((uint)'4') << 24);
    public const uint NDIlib_FourCC_type_h264_alpha_lowest_bandwidth = NDIlib_FourCC_video_type_ex_H264_alpha_lowest_bandwidth;

    public const uint NDIlib_FourCC_video_type_ex_HEVC_alpha_highest_bandwidth =
          ((uint)'A') | (((uint)'E') << 8) | (((uint)'V') << 16) | (((uint)'C') << 24);
    public const uint NDIlib_FourCC_type_HEVC_alpha_highest_bandwidth = NDIlib_FourCC_video_type_ex_HEVC_alpha_highest_bandwidth;

    public const uint NDIlib_FourCC_video_type_ex_HEVC_alpha_lowest_bandwidth =
          ((uint)'a') | (((uint)'e') << 8) | (((uint)'v') << 16) | (((uint)'c') << 24);
    public const uint NDIlib_FourCC_type_HEVC_alpha_lowest_bandwidth = NDIlib_FourCC_video_type_ex_HEVC_alpha_lowest_bandwidth;

    public const uint NDIlib_FourCC_video_type_ex_max = 0x7fffffff;

    // FourCC constants for audio frame types
    public const uint NDIlib_FourCC_audio_type_ex_AAC = 0x00ff;
    public const uint NDIlib_FourCC_audio_type_ex_OPUS =
          ((uint)'O') | (((uint)'p') << 8) | (((uint)'u') << 16) | (((uint)'s') << 24);
    public const uint NDIlib_FourCC_audio_type_ex_max = 0x7fffffff;

    // FourCC constants for compressed packet types
    public const uint NDIlib_compressed_FourCC_type_H264 =
          ((uint)'H') | (((uint)'2') << 8) | (((uint)'6') << 16) | (((uint)'4') << 24);
    public const uint NDIlib_FourCC_type_H264 = NDIlib_compressed_FourCC_type_H264;

    public const uint NDIlib_compressed_FourCC_type_HEVC =
          ((uint)'H') | (((uint)'E') << 8) | (((uint)'V') << 16) | (((uint)'C') << 24);
    public const uint NDIlib_FourCC_type_HEVC = NDIlib_compressed_FourCC_type_HEVC;

    public const uint NDIlib_compressed_FourCC_type_AAC = 0x00ff;
    public const uint NDIlib_FourCC_type_AAC = NDIlib_compressed_FourCC_type_AAC;

    public const uint NDIlib_compressed_FourCC_type_max = 0x7fffffff;

    // Constants for color formats
    public const uint NDIlib_recv_color_format_ex_compressed = 300;
    public const uint NDIlib_recv_color_format_compressed = NDIlib_recv_color_format_ex_compressed;

    public const uint NDIlib_recv_color_format_ex_compressed_v1 = 300;
    public const uint NDIlib_recv_color_format_compressed_v1 = NDIlib_recv_color_format_ex_compressed_v1;

    public const uint NDIlib_recv_color_format_ex_compressed_v2 = 301;
    public const uint NDIlib_recv_color_format_compressed_v2 = NDIlib_recv_color_format_ex_compressed_v2;

    public const uint NDIlib_recv_color_format_ex_compressed_v2_best = 309;
    public const uint NDIlib_recv_color_format_compressed_v2_best = NDIlib_recv_color_format_ex_compressed_v2_best;

    public const uint NDIlib_recv_color_format_ex_compressed_v3 = 302;
    public const uint NDIlib_recv_color_format_compressed_v3 = NDIlib_recv_color_format_ex_compressed_v3;

    public const uint NDIlib_recv_color_format_ex_compressed_v3_best = 310;
    public const uint NDIlib_recv_color_format_compressed_v3_best = NDIlib_recv_color_format_ex_compressed_v3_best;

    public const uint NDIlib_recv_color_format_ex_compressed_v3_with_audio = 304;
    public const uint NDIlib_recv_color_format_compressed_v3_with_audio = NDIlib_recv_color_format_ex_compressed_v3_with_audio;

    public const uint NDIlib_recv_color_format_ex_compressed_v4 = 303;
    public const uint NDIlib_recv_color_format_compressed_v4 = NDIlib_recv_color_format_ex_compressed_v4;

    public const uint NDIlib_recv_color_format_ex_compressed_v4_best = 311;
    public const uint NDIlib_recv_color_format_compressed_v4_best = NDIlib_recv_color_format_ex_compressed_v4_best;

    public const uint NDIlib_recv_color_format_ex_compressed_v4_with_audio = 305;
    public const uint NDIlib_recv_color_format_compressed_v4_with_audio = NDIlib_recv_color_format_ex_compressed_v4_with_audio;

    public const uint NDIlib_recv_color_format_ex_compressed_v5 = 307;
    public const uint NDIlib_recv_color_format_compressed_v5 = NDIlib_recv_color_format_ex_compressed_v5;

    public const uint NDIlib_recv_color_format_ex_compressed_v5_best = 312;
    public const uint NDIlib_recv_color_format_compressed_v5_best = NDIlib_recv_color_format_ex_compressed_v5_best;

    public const uint NDIlib_recv_color_format_ex_compressed_v5_with_audio = 308;
    public const uint NDIlib_recv_color_format_compressed_v5_with_audio = NDIlib_recv_color_format_ex_compressed_v5_with_audio;

    // Allow SpeedHQ frames, compressed H.264 frames, HEVC frames and HEVC/H264 with alpha -- protected or not.
    public const uint NDIlib_recv_color_format_ex_compressed_v6 = 313;
    public const uint NDIlib_recv_color_format_compressed_v6 = NDIlib_recv_color_format_ex_compressed_v6;

    // This is like a combination of the NDIlib_recv_color_format_best and NDIlib_recv_color_format_compressed_v6
    // formats. Instead of delivering just UYVY or UYVA if decompressed, a 16-bit format such as P216 or PA16
    // can also be delivered.
    public const uint NDIlib_recv_color_format_ex_compressed_v6_best = 314;
    public const uint NDIlib_recv_color_format_compressed_v6_best = NDIlib_recv_color_format_ex_compressed_v6_best;

    // Allow SpeedHQ frames, compressed H.264 frames, HEVC frames and HEVC/H264 with alpha, along with
    // compressed audio frames and OPUS support -- protected or not.
    public const uint NDIlib_recv_color_format_ex_compressed_v6_with_audio = 315;
    public const uint NDIlib_recv_color_format_compressed_v6_with_audio = NDIlib_recv_color_format_ex_compressed_v6_with_audio;

    public const uint NDIlib_recv_color_format_ex_max = 0x7fffffff;
}
public struct NdiKvmMouseClickEvent
{
    public MouseButton Button;
    public bool Clicked;
}
public struct NdiKvmMouseWheelEvent
{
    public MouseWheel Wheel;
    public float Units;
}

public enum MouseWheel
{
    Horizontal,
    Vertical
}

public enum MouseButton
{
    Left,
    Middle,
    Right
}

public struct NdiKvmMouseMoveEvent
{
    public float X;
    public float Y;

    public readonly void ToScreenCoords(int width, int height, out int x, out int y)
    {
        x = (int)(width * this.X);
        y = (int)(height * this.Y);
    }
}

public struct NdiKvmClipboardEvent
{
    public string Clipboard { get; set; }
}

public class KvmEventArgs : EventArgs
{
    public NdiKvmMouseClickEvent? MouseClick { get; }
    public NdiKvmMouseMoveEvent? MouseMove { get; }
    public NdiKvmMouseWheelEvent? MouseWheel { get; }
    public NdiKvmClipboardEvent? Clipboard { get; }

    public byte[] Data { get; }
    public byte OpCode => this.Data[0];
    public NdiKvmKeyboardEvent? KeyEvent { get; }
    public string MetadataXml { get; }

    public KvmEventArgs(NdiKvmClipboardEvent clipboardEvent, byte[] data, string metadataXml)
    {
        this.Clipboard = clipboardEvent;
        this.Data = data;
        this.MetadataXml = metadataXml;
    }

    public KvmEventArgs(NdiKvmMouseWheelEvent wheelEvent, byte[] data, string metadataXml)
    {
        this.MouseWheel = wheelEvent;
        this.Data = data;
        this.MetadataXml = metadataXml;
    }

    public KvmEventArgs(NdiKvmKeyboardEvent keyEvent, byte[] data, string metadataXml)
    {
        this.KeyEvent = keyEvent;
        this.Data = data;
        this.MetadataXml = metadataXml;
    }

    public KvmEventArgs(NdiKvmMouseClickEvent mouseClick, byte[] data, string metadataXml)
    {
        this.MetadataXml = metadataXml;
        this.MouseClick = mouseClick;
        this.Data = data;
    }

    public KvmEventArgs(NdiKvmMouseMoveEvent? mouseMove, byte[] data, string metadataXml)
    {
        this.MetadataXml = metadataXml;
        this.MouseMove = mouseMove;
        this.Data = data;
    }

    public KvmEventArgs(byte[] data, string metadataXml)
    {
        this.MetadataXml = metadataXml;
        this.Data = data;
    }
}

public enum NdiKvmEventType
{
    MouseDown,
    MouseUp,
    MouseMove,
    MouseVerticalWheel,
    KeyDown,
    KeyUp,
    Clipboard,
    Unknown
}

public class NdiKvmParser
{
    private const string Prefix = "<ndi_kvm u=\"";
    private const string Suffix = "\"/>";

    public static void Dump(string base64, string outputPath = null)
    {
        byte[] data;
        try
        {
            data = Convert.FromBase64String(base64);
        }
        catch (FormatException ex)
        {
            Console.WriteLine($"Invalid Base64: {ex.Message}");
            return;
        }

        Console.WriteLine($"Decoded {data.Length} bytes:");
        for (int i = 0; i < data.Length; i++)
        {
            byte b = data[i];
            // hex
            Console.Write(b.ToString("X2"));
            Console.Write(" ");
            // binary
            Console.WriteLine(Convert.ToString(b, 2).PadLeft(8, '0'));
        }

        if (!string.IsNullOrEmpty(outputPath))
        {
            try
            {
                File.WriteAllBytes(outputPath, data);
                Console.WriteLine($"Wrote {data.Length} bytes to {outputPath}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Failed to write file: {e.Message}");
            }
        }
    }

    public static bool TryParse(
        string xml,
        out KvmEventArgs? args,
        out NdiKvmEventType eventType)
    {
        args = null;
        eventType = NdiKvmEventType.Unknown;

        // Quick check and spanify
        if (!xml.StartsWith(Prefix, StringComparison.Ordinal))
            return false;

        var span = xml.AsSpan();
        var start = Prefix.Length;
        var end = span.LastIndexOf(Suffix, StringComparison.Ordinal);
        if (end <= start)
            return false;

        var payloadSpan = span.Slice(start, end - start);

        // Decode Base64 directly into a byte buffer
        var maxLen = payloadSpan.Length * 3 / 4;
        var buffer = ArrayPool<byte>.Shared.Rent(maxLen);
        try
        {
            if (!Convert.TryFromBase64Chars(
                    payloadSpan,
                    buffer,
                    out var bytesWritten))
                return false;

            ReadOnlySpan<byte> data = buffer.AsSpan(0, bytesWritten);
            var opcode = data[0];

            switch (opcode)
            {
                case 0x0C:
                    {
                        // Keyboard
                        int keySym = (int)BinaryPrimitives.ReadUInt32LittleEndian(data.Slice(1, 4));
                        var pressed = data[5] == 1;

                        eventType = pressed
                            ? NdiKvmEventType.KeyDown
                            : NdiKvmEventType.KeyUp;
                        args = new KvmEventArgs(
                            new NdiKvmKeyboardEvent
                            {
                                KeySym = keySym,
                                KeyDown = pressed
                            },
                            data.ToArray(),
                            xml);
                        return true;
                    }

                case 0x03:
                    {
                        // Mouse move
                        eventType = NdiKvmEventType.MouseMove;
                        var x = BinaryPrimitives.ReadSingleLittleEndian(data.Slice(1));
                        var y = BinaryPrimitives.ReadSingleLittleEndian(data.Slice(5));
                        args = new KvmEventArgs(
                            new NdiKvmMouseMoveEvent { X = x, Y = y },
                            data.ToArray(),
                            xml);
                        return true;
                    }

                // Mouse down (left=0x04 / right=0x06)
                case 0x04:
                case 0x06:
                    {
                        eventType = NdiKvmEventType.MouseDown;
                        var button = opcode == 0x04
                            ? MouseButton.Left
                            : MouseButton.Right;
                        args = new KvmEventArgs(
                            new NdiKvmMouseClickEvent { Button = button, Clicked = true },
                            data.ToArray(),
                            xml);
                        return true;
                    }

                // Mouse up (left=0x07 / right=0x09)
                case 0x07:
                case 0x09:
                    {
                        eventType = NdiKvmEventType.MouseUp;
                        var button = opcode == 0x07
                            ? MouseButton.Left
                            : MouseButton.Right;
                        args = new KvmEventArgs(
                            new NdiKvmMouseClickEvent { Button = button, Clicked = false },
                            data.ToArray(),
                            xml);
                        return true;
                    }

                default:
                    {
                        eventType = NdiKvmEventType.Unknown;
                        args = new KvmEventArgs(data.ToArray(), xml);
                        return true;
                    }
            }
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buffer);
        }
    }


    public static KvmEventArgs? GetEventArgsFromMetadataXml(string metadataXml, out NdiKvmEventType eventType)
    {
        if (!metadataXml.StartsWith("<ndi_kvm u="))
        {
            eventType = NdiKvmEventType.Unknown;
            return null;
        }

        // TODO: A better way to do this.
        var payload = metadataXml.Replace("<ndi_kvm u=\"", "").Replace("\"/>", "");

        try
        {
            var binary = Convert.FromBase64String(payload);
            var data = binary.AsSpan<byte>();
            var opcode = binary[0];
            switch (opcode)
            {
                case 0x0C:
                    // Keyboard event of some sort.
                    int keySym = (int)BinaryPrimitives.ReadUInt32LittleEndian(data.Slice(1, 4));
                    var pressed = data[5] == 1;

                    eventType = pressed
                        ? NdiKvmEventType.KeyDown
                        : NdiKvmEventType.KeyUp;
                   
                    return new KvmEventArgs(new NdiKvmKeyboardEvent()
                    {
                        KeySym = keySym,
                        KeyDown = pressed
                    }, binary, metadataXml);
                case 0x03:
                    // Mouse move
                    eventType = NdiKvmEventType.MouseMove;
                    var x = BitConverter.ToSingle(binary, 1);
                    var y = BitConverter.ToSingle(binary, 5);
                    return new KvmEventArgs(new NdiKvmMouseMoveEvent()
                    {
                        X = x,
                        Y = y
                    }, binary, metadataXml);
                case 0x04:
                    // Left MB down
                    eventType = NdiKvmEventType.MouseDown;
                    return new KvmEventArgs(new NdiKvmMouseClickEvent
                    {
                        Button = MouseButton.Left,
                        Clicked = true
                    }, binary, metadataXml);
                case 0x07:
                    // Left MB up
                    eventType = NdiKvmEventType.MouseUp;
                    return new KvmEventArgs(new NdiKvmMouseClickEvent
                    {
                        Button = MouseButton.Left,
                        Clicked = false
                    }, binary, metadataXml);
                case 0x06:
                    // Right MB down
                    eventType = NdiKvmEventType.MouseDown;
                    return new KvmEventArgs(new NdiKvmMouseClickEvent
                    {
                        Button = MouseButton.Right,
                        Clicked = true
                    }, binary, metadataXml);
                case 0x09:
                    // Right MB up
                    eventType = NdiKvmEventType.MouseUp;
                    return new KvmEventArgs(new NdiKvmMouseClickEvent
                    {
                        Button = MouseButton.Right,
                        Clicked = false
                    }, binary, metadataXml);
                case 0x0A:
                    eventType = NdiKvmEventType.MouseVerticalWheel;
                    var units = BinaryPrimitives.ReadSingleLittleEndian(data.Slice(1, 4));
                    return new KvmEventArgs(new NdiKvmMouseWheelEvent()
                    {
                        Units = units,
                        Wheel = MouseWheel.Vertical
                    }, binary, metadataXml);
                    break;
                case 0x0D:
                    // Clipboard event.
                    eventType = NdiKvmEventType.Clipboard;
                    var clipboardContents = Encoding.UTF8.GetString(data.Slice(5));
                    return new KvmEventArgs(new NdiKvmClipboardEvent()
                    {
                        Clipboard = clipboardContents,
                    }, binary, metadataXml);
                default:
                    eventType = NdiKvmEventType.Unknown;
                    return new KvmEventArgs(binary, metadataXml);
            }

        }
        catch (Exception)
        {
            eventType = NdiKvmEventType.Unknown;
            return null;
        }
    }
}

// An enumeration to specify the type of a packet returned by the functions
public enum frame_type_e
{
    frame_type_none = 0,
    frame_type_video = 1,
    frame_type_audio = 2,
    frame_type_metadata = 3,
    frame_type_error = 4,

    // This indicates that the settings on this input have changed. This value will be returned from one of
    // the NDIlib_recv_capture functions when the device is known to have new settings, for instance the web
    // URL has changed or the device is now known to be a PTZ camera.
    frame_type_status_change = 100,

    // This indicates that the source has changed. This value will be returned from one of the
    // NDIlib_recv_capture functions when the source that the receiver is connected to has changed.
    frame_type_source_change = 101,

    // Make sure this is a 32-bit enumeration.
    frame_type_max = 0x7fffffff
}

public enum FourCC_type_e
{
    FourCC_type_UYVY = 0x59565955,

    // 4:2:0 formats
    NDIlib_FourCC_video_type_YV12 = 0x32315659,
    NDIlib_FourCC_video_type_NV12 = 0x3231564E,
    NDIlib_FourCC_video_type_I420 = 0x30323449,

    // BGRA
    FourCC_type_BGRA = 0x41524742,
    FourCC_type_BGRX = 0x58524742,

    // RGBA
    FourCC_type_RGBA = 0x41424752,
    FourCC_type_RGBX = 0x58424752,

    // P216/PA16
    FourCC_type_P216 = 0x36313250,
    FourCC_type_PA16 = 0x36314150,

    // This is a UYVY buffer followed immediately by an alpha channel buffer.
    // If the stride of the YCbCr component is "stride", then the alpha channel
    // starts at image_ptr + yres*stride. The alpha channel stride is stride/2.
    FourCC_type_UYVA = 0x41565955,

    FourCC_type_ex_H264_highest_bandwidth = 875967048
}

public enum frame_format_type_e
{
    // A progressive frame
    frame_format_type_progressive = 1,

    // A fielded frame with the field 0 being on the even lines and field 1 being
    // on the odd lines/
    frame_format_type_interleaved = 0,

    // Individual fields
    frame_format_type_field_0 = 2,
    frame_format_type_field_1 = 3
}

// FourCC values for audio frames
public enum FourCC_audio_type_e
{
    // Planar 32-bit floating point. Be sure to specify the channel stride.
    FourCC_audio_type_FLTP = 0x70544c46,
    FourCC_type_FLTP = FourCC_audio_type_FLTP,

    // Ensure that the size is 32bits
    FourCC_audio_type_max = 0x7fffffff
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

// This describes a video frame
[StructLayoutAttribute(LayoutKind.Sequential)]
public struct video_frame_v2_t
{
    // The resolution of this frame
    public int xres, yres;

    // What FourCC this is with. This can be two values
    public FourCC_type_e FourCC;

    public readonly FourCC_video_type_ex_e FourCCEx => (FourCC_video_type_ex_e)this.FourCC;

    // What is the frame-rate of this frame.
    // For instance NTSC is 30000,1001 = 30000/1001 = 29.97fps
    public int frame_rate_N, frame_rate_D;

    // What is the picture aspect ratio of this frame.
    // For instance 16.0/9.0 = 1.778 is 16:9 video
    // 0 means square pixels
    public float picture_aspect_ratio;

    // Is this a fielded frame, or is it progressive
    public frame_format_type_e frame_format_type;

    // The timecode of this frame in 100ns intervals
    public Int64 timecode;

    // The video data itself
    public nint p_data;

    // The inter line stride of the video data, in bytes.
    public int line_stride_in_bytes;

    // Per frame metadata for this frame. This is a NULL terminated UTF8 string that should be
    // in XML format. If you do not want any metadata then you may specify NULL here.
    public nint p_metadata;

    // This is only valid when receiving a frame and is specified as a 100ns time that was the exact
    // moment that the frame was submitted by the sending side and is generated by the SDK. If this
    // value is NDIlib_recv_timestamp_undefined then this value is not available and is NDIlib_recv_timestamp_undefined.
    public Int64 timestamp;
}

// This describes an audio frame
[StructLayoutAttribute(LayoutKind.Sequential)]
public struct audio_frame_v2_t
{
    // The sample-rate of this buffer
    public int sample_rate;

    // The number of audio channels
    public int no_channels;

    // The number of audio samples per channel
    public int no_samples;

    // The timecode of this frame in 100ns intervals
    public Int64 timecode;

    // The audio data
    public nint p_data;

    // The inter channel stride of the audio channels, in bytes
    public int channel_stride_in_bytes;

    // Per frame metadata for this frame. This is a NULL terminated UTF8 string that should be
    // in XML format. If you do not want any metadata then you may specify NULL here.
    public nint p_metadata;

    // This is only valid when receiving a frame and is specified as a 100ns time that was the exact
    // moment that the frame was submitted by the sending side and is generated by the SDK. If this
    // value is NDIlib_recv_timestamp_undefined then this value is not available and is NDIlib_recv_timestamp_undefined.
    public Int64 timestamp;
}

// This describes an audio frame
[StructLayoutAttribute(LayoutKind.Sequential)]
public struct audio_frame_v3_t
{
    // The sample-rate of this buffer
    public int sample_rate;

    // The number of audio channels
    public int no_channels;

    // The number of audio samples per channel
    public int no_samples;

    // The timecode of this frame in 100ns intervals
    public Int64 timecode;

    // What FourCC describing the type of data for this frame
    public FourCC_audio_type_e FourCC;

    // The audio data
    public nint p_data;

    // If the FourCC is not a compressed type and the audio format is planar,
    // then this will be the stride in bytes for a single channel.
    // If the FourCC is a compressed type, then this will be the size of the
    // p_data buffer in bytes.
    public int channel_stride_in_bytes;

    // Per frame metadata for this frame. This is a NULL terminated UTF8 string that should be
    // in XML format. If you do not want any metadata then you may specify NULL here.
    public nint p_metadata;

    // This is only valid when receiving a frame and is specified as a 100ns time that was the exact
    // moment that the frame was submitted by the sending side and is generated by the SDK. If this
    // value is NDIlib_recv_timestamp_undefined then this value is not available and is NDIlib_recv_timestamp_undefined.
    public Int64 timestamp;
}

// The data description for metadata
[StructLayoutAttribute(LayoutKind.Sequential)]
public struct metadata_frame_t
{
    // The length of the string in UTF8 characters. This includes the NULL terminating character.
    // If this is 0, then the length is assume to be the length of a NULL terminated string.
    public int length;

    // The timecode of this frame in 100ns intervals
    public Int64 timecode;

    // The metadata as a UTF8 XML string. This is a NULL terminated string.
    public nint p_data;
}

// Tally structures
[StructLayoutAttribute(LayoutKind.Sequential)]
public struct tally_t
{
    // Is this currently on program output
    [MarshalAsAttribute(UnmanagedType.U1)]
    public bool on_program;

    // Is this currently on preview output
    [MarshalAsAttribute(UnmanagedType.U1)]
    public bool on_preview;
}



public class Finder : IDisposable
{
    //public List<Source> Sources
    //{ get; }
    //    = new List<Source>();

    public Finder(bool showLocalSources = false, string[] groups = null, string[] extraIps = null)
    {
        nint groupsNamePtr = nint.Zero;

        // make a flat list of groups if needed
        if (groups != null)
        {
            StringBuilder flatGroups = new StringBuilder();
            foreach (string group in groups)
            {
                flatGroups.Append(group);
                if (group != groups.Last())
                {
                    flatGroups.Append(',');
                }
            }

            groupsNamePtr = UTF.StringToUtf8(flatGroups.ToString());
        }

        // This is also optional.
        // The list of additional IP addresses that exist that we should query for 
        // sources on. For instance, if you want to find the sources on a remote machine
        // that is not on your local sub-net then you can put a comma seperated list of 
        // those IP addresses here and those sources will be available locally even though
        // they are not mDNS discoverable. An example might be "12.0.0.8,13.0.12.8".
        // When none is specified (nint.Zero) the registry is used.
        // Create a UTF-8 buffer from our string
        // Must use Marshal.FreeHGlobal() after use!
        // nint extraIpsPtr = NDI.Common.StringToUtf8("12.0.0.8,13.0.12.8")
        nint extraIpsPtr = nint.Zero;

        // make a flat list of ip addresses as comma separated strings
        if (extraIps != null)
        {
            StringBuilder flatIps = new StringBuilder();
            foreach (string ipStr in extraIps)
            {
                flatIps.Append(ipStr);
                if (ipStr != groups.Last())
                {
                    flatIps.Append(',');
                }
            }

            extraIpsPtr = UTF.StringToUtf8(flatIps.ToString());
        }

        // how we want our find to operate
        find_create_t findDesc = new find_create_t()
        {
            p_groups = groupsNamePtr,
            show_local_sources = showLocalSources,
            p_extra_ips = extraIpsPtr

        };

        // create our find instance
        this._findInstancePtr = NDIWrapper.find_create_v2(ref findDesc);

        // free our UTF-8 buffer if we created one
        if (groupsNamePtr != nint.Zero)
        {
            Marshal.FreeHGlobal(groupsNamePtr);
        }

        if (extraIpsPtr != nint.Zero)
        {
            Marshal.FreeHGlobal(extraIpsPtr);
        }

        // start up a thread to update on
        this._findThread = new Thread(this.FindThreadProc) { IsBackground = true, Name = "NdiFindThread" };
        this._findThread.Start();
    }

    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~Finder()
    {
        this.Dispose(false);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!this._disposed)
        {
            if (disposing)
            {
                // tell the thread to exit
                this._exitThread = true;

                // wait for it to exit
                if (this._findThread != null)
                {
                    this._findThread.Join();

                    this._findThread = null;
                }
            }

            if (this._findInstancePtr != nint.Zero)
            {
                NDIWrapper.find_destroy(this._findInstancePtr);
                this._findInstancePtr = nint.Zero;
            }

            this._disposed = true;
        }
    }

    private bool _disposed = false;

    private object checkLock = new object();

    private unsafe void FindThreadProc()
    {
        // the size of an source_t, for pointer offsets
        int SourceSizeInBytes = Marshal.SizeOf(typeof(source_t));

        var lastNumberOfSources = 0u;
        while (!this._exitThread)
        {
            lock (this.checkLock)
            {
                if (!NDIWrapper.find_wait_for_sources(this._findInstancePtr, 250))
                {
                    continue;
                }

                var numSources = 0u;
                var sourcesPtr = NDIWrapper.find_get_current_sources(this._findInstancePtr, ref numSources);

                if (numSources != lastNumberOfSources)
                {
                    lastNumberOfSources = numSources;
                    this.SourceListChanged?.Invoke();
                }
            }
        }
    }

    public unsafe NdiSource[] GetCurrentSourceList()
    {
        lock (this.checkLock)
        {
            var numSources = 0u;
            var sourcesPtr = NDIWrapper.find_get_current_sources(this._findInstancePtr, ref numSources);

            if(numSources == 0)
            {
                return Array.Empty<NdiSource>();
            }

            var toReturn = new NdiSource[(int)numSources];

            var sourceSpan = new ReadOnlySpan<source_t>(sourcesPtr.ToPointer(), (int)numSources);

            for (var i = 0; i < numSources; i++)
            {
                var source = sourceSpan[i];
                var toAdd = new NdiSource(ref source);
                toReturn[i] = toAdd;
            }

            return toReturn;
        }
    }

    private nint _findInstancePtr = nint.Zero;

    private object _sourceLock = new object();

    // a thread to find on so that the UI isn't dragged down
    Thread _findThread = null;

    // a way to exit the thread safely
    bool _exitThread = false;

    public event Action SourceListChanged;
}

public class NdiSource
{
    private readonly byte[] nameUtf8;
    public ReadOnlySpan<byte> NameUtf8 => this.nameUtf8;

    private readonly int SpaceIndex;

    public ReadOnlySpan<byte> ComputerNameUtf8 => this.SpaceIndex >= 0 ? this.NameUtf8.Slice(0, this.SpaceIndex) : this.NameUtf8;
    public ReadOnlySpan<byte> SourceNameUtf8 => this.SpaceIndex >= 0 ? this.NameUtf8.Slice(this.SpaceIndex + 1) : this.NameUtf8;

    private string? cachedName;
    private string? cachedComputerName;
    private string? cachedSourceName;

    public string Name => this.cachedName ??= Encoding.UTF8.GetString(this.nameUtf8);
    public string ComputerName => this.cachedComputerName ??= Encoding.UTF8.GetString(this.ComputerNameUtf8);
    public string SourceName
    {
        get
        {
            var toReturn = this.cachedSourceName = this.cachedSourceName ?? Encoding.UTF8.GetString(this.SourceNameUtf8);
            return toReturn[1..(toReturn.Length - 1)].Trim();
        }
    }

    private const byte SPACE_CHARCODE = 0x20;

    public NdiSource(ref source_t source)
    {
        this.nameUtf8 = CopyUtf8FromPointer(source.p_ndi_name);
        this.SpaceIndex = this.NameUtf8.IndexOf(SPACE_CHARCODE);
    }

    private static unsafe byte[] CopyUtf8FromPointer(nint ptr)
    {
        if (ptr == nint.Zero)
            return Array.Empty<byte>();

        byte* p = (byte*)ptr;
        int len = 0;
        while (p[len] != 0) len++;

        var buf = new byte[len];
        fixed (byte* dst = buf)
            Buffer.MemoryCopy(p, dst, len, len);

        return buf;
    }
}


//public delegate void NewNdiSourceDiscovered(Source source);

//public delegate void NdiSourceListChanged(Source[] added, Source[] removed);



// This describes an audio frame
[StructLayoutAttribute(LayoutKind.Sequential)]
public struct audio_frame_interleaved_16s_t
{
	// The sample-rate of this buffer
	public int	sample_rate;

	// The number of audio channels
	public int	no_channels;

	// The number of audio samples per channel
	public int	no_samples;

	// The timecode of this frame in 100ns intervals
	public Int64	timecode;

	// The audio reference level in dB. This specifies how many dB above the reference level (+4dBU) is the full range of 16 bit audio.
	// If you do not understand this and want to just use numbers :
	//		-	If you are sending audio, specify +0dB. Most common applications produce audio at reference level.
	//		-	If receiving audio, specify +20dB. This means that the full 16 bit range corresponds to professional level audio with 20dB of headroom. Note that
	//			if you are writing it into a file it might sound soft because you have 20dB of headroom before clipping.
	public int	reference_level;

	// The audio data, interleaved 16bpp
	public nint	p_data;
}

// This describes an audio frame
[StructLayoutAttribute(LayoutKind.Sequential)]
public struct audio_frame_interleaved_32f_t
{
	// The sample-rate of this buffer
	public int	sample_rate;

	// The number of audio channels
	public int	no_channels;

	// The number of audio samples per channel
	public int	no_samples;

	// The timecode of this frame in 100ns intervals
	public Int64	timecode;

	// The audio data, interleaved 32bpp
	public nint	p_data;
}

// This describes an audio frame
[StructLayoutAttribute(LayoutKind.Sequential)]
public struct audio_frame_interleaved_32s_t
{
	// The sample-rate of this buffer
	public int sample_rate;

	// The number of audio channels
	public int no_channels;

	// The number of audio samples per channel
	public int no_samples;

	// The timecode of this frame in 100ns intervals
	public Int64 timecode;

	// The audio data, interleaved 32bpp (Int32)
	public nint p_data;
}

public class NDIInteropString : IDisposable
{
    public readonly string value;
    public nint utf8Ptr;
    private bool disposedValue;

    public NDIInteropString(string? value)
    {
        this.value = value ?? "";
        this.utf8Ptr = value is null 
            ? nint.Zero 
            : UTF.StringToUtf8(value);
    }

    public static implicit operator nint(NDIInteropString s) => s.utf8Ptr;
    public static implicit operator string(NDIInteropString s) => s.value;

    protected virtual void Dispose(bool disposing)
    {
        if (this.disposedValue)
        {
            return;
        }


        if (this.utf8Ptr != nint.Zero)
        {
            Marshal.FreeHGlobal(this.utf8Ptr);
            this.utf8Ptr = nint.Zero;
        }

        this.disposedValue = true;
    }

    // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
    // ~NDIInteropString()
    // {
    //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
    //     Dispose(disposing: false);
    // }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        this.Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}

[SuppressUnmanagedCodeSecurity]
public static partial class UTF
{
    // This REQUIRES you to use Marshal.FreeHGlobal() on the returned pointer!
    public static nint StringToUtf8(string managedString)
    {
        int len = Encoding.UTF8.GetByteCount(managedString);

        byte[] buffer = new byte[len + 1];

        Encoding.UTF8.GetBytes(managedString, 0, managedString.Length, buffer, 0);

        nint nativeUtf8 = Marshal.AllocHGlobal(buffer.Length);

        Marshal.Copy(buffer, 0, nativeUtf8, buffer.Length);

        return nativeUtf8;
    }

    // this version will also return the length of the utf8 string
    // This REQUIRES you to use Marshal.FreeHGlobal() on the returned pointer!
    public static nint StringToUtf8(string managedString, out int utf8Length)
    {
        utf8Length = Encoding.UTF8.GetByteCount(managedString);

        byte[] buffer = new byte[utf8Length + 1];

        Encoding.UTF8.GetBytes(managedString, 0, managedString.Length, buffer, 0);

        nint nativeUtf8 = Marshal.AllocHGlobal(buffer.Length);

        Marshal.Copy(buffer, 0, nativeUtf8, buffer.Length);

        return nativeUtf8;
    }

    // Length is optional, but recommended
    // This is all potentially dangerous
    public static string Utf8ToString(nint nativeUtf8, uint? length = null)
    {
        if (nativeUtf8 == nint.Zero)
            return string.Empty;

        uint len = 0;

        if (length.HasValue)
        {
            len = length.Value;
        }
        else
        {
            // try to find the terminator
            while (Marshal.ReadByte(nativeUtf8, (int)len) != 0)
            {
                ++len;
            }
        }

        byte[] buffer = new byte[len];

        Marshal.Copy(nativeUtf8, buffer, 0, buffer.Length);

        return Encoding.UTF8.GetString(buffer);
    }

} // class NDILib

// The creation structure that is used when you are creating a sender
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


public enum recv_bandwidth_e
{
    // Receive metadata.
    recv_bandwidth_metadata_only = -10,

    // Receive metadata audio.
    recv_bandwidth_audio_only = 10,

    // Receive metadata audio video at a lower bandwidth and resolution.
    recv_bandwidth_lowest = 0,

    // Receive metadata audio video at full resolution.
    recv_bandwidth_highest = 100
}

public enum avsync_ret_e
{
    // We recovered the audio that you asked for, if you requested an exact number of samples it was
    // returned. If you do not specify the number of audio samples that you want then this is always returned.
    NDIlib_avsync_ret_success = 1,

    // We recovered the audio, however the number of samples you asked for needed to be different to avoid
    // dropping audio data. This is because the remote source is not clocking audio and video sufficiently
    // accurately on the same clock and a different number of audio samples was needed in order to keep it
    // exactly in sync.
    NDIlib_avsync_ret_success_num_samples_not_matched = 2,

    // No audio could be captured that matched this video frame. This is because there is currently no audio
    // from this source.
    NDIlib_avsync_ret_no_audio_stream_received = -1,

    // No audio could be capture that matched this video frame. Audio is currently on this source, however
    // none could be found that matched the video frame. This is likely because the sync, timestamps or
    // clocks on the remote source are so far away from the expectations. Or that the video has a timestamp
    // that is incorrect. This can also occur if the sender is putting audio and video into the stream in
    // such a way that they are out of sync.
    NDIlib_avsync_ret_no_samples_found = -2,

    // You specified an audio format, but it has changed. You specified an desired audio sample rate or no
    // audio channels however this is not what the audio settings currently are. NDIlib_audio_frame_v3_t has
    // been updated to correctly reflect the results but audio has not been filled in. Call again to get the
    // audio for this video frame.
    NDIlib_avsync_ret_format_changed = -3,

    // Some internal error occurred (e.g. p_avsync was NULL, p_audio_frame was NULL, etc...)
    NDIlib_avsync_ret_internal_error = -4
}

public enum recv_color_format_e
{
    // No alpha channel: BGRX Alpha channel: BGRA
    recv_color_format_BGRX_BGRA = 0,

    // No alpha channel: UYVY Alpha channel: BGRA
    recv_color_format_UYVY_BGRA = 1,

    // No alpha channel: RGBX Alpha channel: RGBA
    recv_color_format_RGBX_RGBA = 2,

    // No alpha channel: UYVY Alpha channel: RGBA
    recv_color_format_UYVY_RGBA = 3,

    // On Windows there are some APIs that require bottom to top images in RGBA format. Specifying
    // this format will return images in this format. The image data pointer will still point to the
    // "top" of the image, althought he stride will be negative. You can get the "bottom" line of the image
    // using : video_data.p_data + (video_data.yres - 1)*video_data.line_stride_in_bytes
    recv_color_format_BGRX_BGRA_flipped = 200,

    // Read the SDK documentation to understand the pros and cons of this format.
    recv_color_format_fastest = 100,

    NDIlib_recv_color_format_best = 101,

    // Legacy definitions for backwards compatibility
    recv_color_format_e_BGRX_BGRA = recv_color_format_BGRX_BGRA,
    recv_color_format_e_UYVY_BGRA = recv_color_format_UYVY_BGRA,
    recv_color_format_e_RGBX_RGBA = recv_color_format_RGBX_RGBA,
    recv_color_format_e_UYVY_RGBA = recv_color_format_UYVY_RGBA
}

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

[StructLayoutAttribute(LayoutKind.Sequential)]
public struct send_advertiser_create_t
{
    public nint p_url_address;
}

[StructLayoutAttribute(LayoutKind.Sequential)]
public struct send_listener_create_t
{
    public nint p_url_address;
}

[StructLayoutAttribute(LayoutKind.Sequential)]
public struct sender_t
{
    public nint p_uuid;
    public nint p_name;
    public nint p_metadata;
    public uint p_address;
    public int port;
    public nint p_groups;
    public uint num_groups;
    public bool events_subscribed;
}