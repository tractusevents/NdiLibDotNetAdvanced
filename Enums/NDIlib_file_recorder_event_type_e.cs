namespace Tractus.Ndi.Sdk64;

public enum NDIlib_file_recorder_event_type_e
{
    none = 0,
    started = 1,
    rollover = 2,
    open_failed = 3,
    write_failed = 4,
    error = 5,
    format_changed = 6,
    first_frame = 7,
    source_lost = 8,
    source_connected = 9,
    file_closed = 10,
    proxy_missing = 11,
    max = 0x7fffffff
}
