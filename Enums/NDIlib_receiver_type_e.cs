namespace Tractus.Ndi;

public enum NDIlib_receiver_type_e : int
{
    none = 0,
    metadata = 1,
    video = 2,
    audio = 3,
    max = unchecked((int)0x7fffffff)
}
