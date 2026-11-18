namespace Tractus.Ndi;

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
