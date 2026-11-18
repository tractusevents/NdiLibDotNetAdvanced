namespace Tractus.Ndi;

// FourCC values for audio frames
public enum FourCC_audio_type_e
{
    // Planar 32-bit floating point. Be sure to specify the channel stride.
    FourCC_audio_type_FLTP = 0x70544c46,
    FourCC_type_FLTP = FourCC_audio_type_FLTP,

    // Ensure that the size is 32bits
    FourCC_audio_type_max = 0x7fffffff
}
