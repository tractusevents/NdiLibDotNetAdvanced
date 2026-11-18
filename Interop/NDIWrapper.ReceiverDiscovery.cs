using System.Runtime.InteropServices;

namespace Tractus.Ndi;

public static unsafe partial class NDIWrapper
{
    // Copy strings while the listener is live, and release every acquired native batch.
    public static NDIlib_recv_listener_event[] GetListenerEvents(
        nint instance,
        uint timeoutInMilliseconds,
        out uint numEvents)
    {
        var eventsPtr = recv_listener_get_events(instance, out numEvents, timeoutInMilliseconds);
        if (eventsPtr == nint.Zero)
        {
            numEvents = 0;
            return Array.Empty<NDIlib_recv_listener_event>();
        }

        try
        {
            return CopyListenerEvents(eventsPtr, numEvents);
        }
        finally
        {
            recv_listener_free_events(instance, eventsPtr);
        }
    }

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_advertiser_add_receiver", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool recv_advertiser_add_receiver(
        nint p_instance,
        nint p_receiver,
        [MarshalAs(UnmanagedType.I1)] bool allow_controlling,
        [MarshalAs(UnmanagedType.I1)] bool allow_monitoring,
        [MarshalAs(UnmanagedType.LPUTF8Str)]string p_input_group_name);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_advertiser_add_receiver", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool recv_advertiser_add_receiver(
        nint p_instance,
        nint p_receiver,
        [MarshalAs(UnmanagedType.I1)] bool allow_controlling,
        [MarshalAs(UnmanagedType.I1)] bool allow_monitoring,
        nint p_input_group_name);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_advertiser_create", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern nint recv_advertiser_create(
        ref recv_advertiser_create_t p_create_settings);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_advertiser_create_ex", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern nint recv_advertiser_create_ex(
        ref recv_advertiser_create_t createSettings,
        [MarshalAs(UnmanagedType.LPUTF8Str)] string configData);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_advertiser_del_receiver", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool recv_advertiser_del_receiver(
        nint p_instance,
        nint p_receiver);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_advertiser_destroy", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void recv_advertiser_destroy(
        nint p_instance);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_listener_create", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern nint recv_listener_create(
        ref NDIlib_recv_listener_create_t listenerCreateSettings);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_listener_create_ex", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern nint recv_listener_create_ex(
    ref NDIlib_recv_listener_create_t listenerCreateSettings,
    [MarshalAs(UnmanagedType.LPUTF8Str)] string configData);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_listener_create_ex", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern nint recv_listener_create_ex(
        ref NDIlib_recv_listener_create_t listenerCreateSettings,
        nint configData);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_listener_destroy", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void recv_listener_destroy(nint pInstance);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_listener_free_events", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void recv_listener_free_events(
        nint pInstance,
        nint pEvents);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_listener_get_events", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern nint recv_listener_get_events(
        nint pInstance,
        out uint pNumEvents,
        uint timeoutMsec);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_listener_get_receivers", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern nint recv_listener_get_receivers(
        nint pInstance,
        out uint numListeners);

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
                    commandArray[ci] = (NDIlib_receiver_command_e)rawValue;
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
                    streamArray[ci] = (NDIlib_receiver_type_e)rawValue;
                }

                toAdd.p_streams = streamArray;
            }

            receivers[i] = toAdd;
        }
        return receivers;
    }

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_listener_get_server_url", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern nint recv_listener_get_server_url(nint pInstance);

    public static string recv_listener_get_server_url_string(nint pInstance)
    {
        return UTF.Utf8ToString(recv_listener_get_server_url(pInstance));
    }

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_listener_is_connected", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool recv_listener_is_connected(nint pInstance);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_listener_send_connect", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool recv_listener_send_connect(
        nint p_instance,
        [MarshalAs(UnmanagedType.LPUTF8Str)] string p_receiver_uuid,
        [MarshalAs(UnmanagedType.LPUTF8Str)] string? p_source_name
    );

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_listener_send_connect", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool recv_listener_send_connect(
    nint p_instance,
    nint p_receiver_uuid,
    nint p_source_name
    );

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

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_listener_wait_for_receivers", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool recv_listener_wait_for_receivers(
        nint pInstance,
        uint timeoutMsec);
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
public struct NDIlib_recv_listener_create_t
{
    [MarshalAs(UnmanagedType.LPUTF8Str)]
    public string p_url_address;
}
