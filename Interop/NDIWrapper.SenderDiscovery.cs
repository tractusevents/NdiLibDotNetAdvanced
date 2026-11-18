using System.Runtime.InteropServices;

namespace Tractus.Ndi;

public static unsafe partial class NDIWrapper
{
    private static string[] CopyUtf8PointerArray(nint pointer, uint count)
    {
        if (pointer == nint.Zero || count == 0)
        {
            return [];
        }

        if (count > int.MaxValue)
        {
            throw new InvalidOperationException("The native sender returned too many groups.");
        }

        var result = new string[(int)count];
        var pointers = new ReadOnlySpan<nint>(pointer.ToPointer(), result.Length);
        for (var i = 0; i < pointers.Length; i++)
        {
            result[i] = UTF.Utf8ToString(pointers[i]);
        }

        return result;
    }

    public static NDIlib_recv_listener_event[] GetSendListenerEvents(
        nint instance,
        uint timeoutInMilliseconds,
        out uint numEvents)
    {
        var eventPointer = send_listener_get_events(instance, out numEvents, timeoutInMilliseconds);
        if (eventPointer == nint.Zero)
        {
            numEvents = 0;
            return [];
        }

        try
        {
            return CopyListenerEvents(eventPointer, numEvents);
        }
        finally
        {
            send_listener_free_events(instance, eventPointer);
        }
    }

    [DllImport(LibraryName, EntryPoint = "NDIlib_send_advertiser_add_sender", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool send_advertiser_add_sender(
        nint p_instance,
        nint p_sender,
        [MarshalAs(UnmanagedType.I1)] bool allow_monitoring);

    [DllImport(LibraryName, EntryPoint = "NDIlib_send_advertiser_create", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern nint send_advertiser_create(ref send_advertiser_create_t p_create_settings);

    [DllImport(LibraryName, EntryPoint = "NDIlib_send_advertiser_create", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public unsafe static extern nint send_advertiser_create(send_advertiser_create_t* p_create_settings);

    [DllImport(LibraryName, EntryPoint = "NDIlib_send_advertiser_create_ex", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern nint send_advertiser_create_ex(
        ref send_advertiser_create_t createSettings,
        [MarshalAs(UnmanagedType.LPUTF8Str)] string configData);

    [DllImport(LibraryName, EntryPoint = "NDIlib_send_advertiser_create_ex", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern nint send_advertiser_create_ex(
        send_advertiser_create_t* createSettings,
        nint configData);

    [DllImport(LibraryName, EntryPoint = "NDIlib_send_advertiser_del_sender", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool send_advertiser_del_sender(
        nint p_instance,
        nint p_sender);

    [DllImport(LibraryName, EntryPoint = "NDIlib_send_advertiser_destroy", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void send_advertiser_destroy(nint p_instance);

    [DllImport(LibraryName, EntryPoint = "NDIlib_send_listener_create", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern nint send_listener_create(ref send_listener_create_t p_create_settings);

    [DllImport(LibraryName, EntryPoint = "NDIlib_send_listener_create", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static unsafe extern nint send_listener_create(send_listener_create_t* p_create_settings);

    [DllImport(LibraryName, EntryPoint = "NDIlib_send_listener_create_ex", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern nint send_listener_create_ex(
        ref send_listener_create_t createSettings,
        [MarshalAs(UnmanagedType.LPUTF8Str)] string configData);

    [DllImport(LibraryName, EntryPoint = "NDIlib_send_listener_create_ex", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern nint send_listener_create_ex(
        send_listener_create_t* createSettings,
        nint configData);

    [DllImport(LibraryName, EntryPoint = "NDIlib_send_listener_destroy", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void send_listener_destroy(nint p_instance);

    [DllImport(LibraryName, EntryPoint = "NDIlib_send_listener_free_events", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void send_listener_free_events(nint instance, nint events);

    [DllImport(LibraryName, EntryPoint = "NDIlib_send_listener_get_events", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern nint send_listener_get_events(
        nint instance,
        out uint numEvents,
        uint timeoutInMilliseconds);

    [DllImport(LibraryName, EntryPoint = "NDIlib_send_listener_get_senders", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern nint send_listener_get_senders(nint p_instance, out uint p_num_senders);

    public static sender_t_wrapped[] send_listener_get_senders_wrapped(
        nint instance,
        out uint numSenders)
    {
        var senderPointer = send_listener_get_senders(instance, out numSenders);
        if (senderPointer == nint.Zero || numSenders == 0)
        {
            numSenders = 0;
            return [];
        }

        if (numSenders > int.MaxValue)
        {
            throw new InvalidOperationException("The native listener returned too many senders.");
        }

        var nativeSenders = new ReadOnlySpan<sender_t>(senderPointer.ToPointer(), (int)numSenders);
        var senders = new sender_t_wrapped[nativeSenders.Length];

        for (var i = 0; i < nativeSenders.Length; i++)
        {
            ref readonly var nativeSender = ref nativeSenders[i];
            senders[i] = new sender_t_wrapped
            {
                p_uuid = UTF.Utf8ToString(nativeSender.p_uuid),
                p_name = UTF.Utf8ToString(nativeSender.p_name),
                p_metadata = UTF.Utf8ToString(nativeSender.p_metadata),
                p_address = UTF.Utf8ToString(nativeSender.p_address),
                port = nativeSender.port,
                p_groups = CopyUtf8PointerArray(nativeSender.p_groups, nativeSender.num_groups),
                num_groups = nativeSender.num_groups,
                events_subscribed = nativeSender.events_subscribed
            };
        }

        return senders;
    }

    [DllImport(LibraryName, EntryPoint = "NDIlib_send_listener_get_server_url", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern nint send_listener_get_server_url(nint p_instance);

    public static string send_listener_get_server_url_string(nint instance)
    {
        return UTF.Utf8ToString(send_listener_get_server_url(instance));
    }

    [DllImport(LibraryName, EntryPoint = "NDIlib_send_listener_is_connected", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool send_listener_is_connected(nint instance);

    [DllImport(LibraryName, EntryPoint = "NDIlib_send_listener_subscribe_events", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void send_listener_subscribe_events(
        nint instance,
        [MarshalAs(UnmanagedType.LPUTF8Str)] string senderUuid);

    [DllImport(LibraryName, EntryPoint = "NDIlib_send_listener_subscribe_events", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void send_listener_subscribe_events(nint instance, nint senderUuid);

    [DllImport(LibraryName, EntryPoint = "NDIlib_send_listener_unsubscribe_events", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void send_listener_unsubscribe_events(
        nint instance,
        [MarshalAs(UnmanagedType.LPUTF8Str)] string senderUuid);

    [DllImport(LibraryName, EntryPoint = "NDIlib_send_listener_unsubscribe_events", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void send_listener_unsubscribe_events(nint instance, nint senderUuid);

    [DllImport(LibraryName, EntryPoint = "NDIlib_send_listener_wait_for_senders", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool send_listener_wait_for_senders(nint p_instance, uint timeout_in_ms);
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
    public nint p_address;
    public int port;
    public nint p_groups;
    public uint num_groups;
    [MarshalAs(UnmanagedType.I1)]
    public bool events_subscribed;
}

public struct sender_t_wrapped
{
    public string p_uuid;
    public string p_name;
    public string p_metadata;
    public string p_address;
    public int port;
    public string[] p_groups;
    public uint num_groups;
    public bool events_subscribed;
}
