namespace Tractus.Ndi.Sdk64;

[Flags]
public enum NDIlib_file_recorder_track_flags_e
{
    none = 0,
    video = 1 << 0,
    audio = 1 << 1,
    metadata = 1 << 2,
    all = video | audio | metadata,
    max = 0x7fffffff
}
