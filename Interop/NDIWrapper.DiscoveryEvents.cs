using System.Runtime.InteropServices;

namespace Tractus.Ndi;

public static unsafe partial class NDIWrapper
{
    private static NDIlib_recv_listener_event[] CopyListenerEvents(nint pointer, uint count)
    {
        if (count == 0)
        {
            return [];
        }

        var nativeEvents = new ReadOnlySpan<NDIlib_listener_event_native_t>(pointer.ToPointer(), checked((int)count));
        var result = new NDIlib_recv_listener_event[nativeEvents.Length];
        for (var i = 0; i < result.Length; i++)
        {
            ref readonly var item = ref nativeEvents[i];
            result[i] = new NDIlib_recv_listener_event
            {
                p_uuid = Marshal.PtrToStringUTF8(item.p_uuid)!,
                p_name = Marshal.PtrToStringUTF8(item.p_name)!,
                p_value = Marshal.PtrToStringUTF8(item.p_value)!
            };
        }

        return result;
    }
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

[StructLayout(LayoutKind.Sequential)]
internal struct NDIlib_listener_event_native_t
{
    public nint p_uuid;
    public nint p_name;
    public nint p_value;
}
