using System;

namespace Tractus.Ndi;
public enum NDIlib_FourCC_audio_type_ex_e : uint
{
    NDIlib_FourCC_audio_type_ex_AAC = 0x00ff,

    // Multi channel Opus stream -- unlike other compressed formats, the data field is not expected to be
    // prefixed with the NDIlib_compressed_packet_t structure.
    NDIlib_FourCC_audio_type_ex_OPUS = ((uint)'O' | ((uint)'p' << 8) | ((uint)'u' << 16) | ((uint)'s' << 24)),  //NDI_LIB_FOURCC('O', 'p', 'u', 's'),

    // Make sure this is a 32-bit enumeration.
    NDIlib_FourCC_audio_type_ex_max = 0x7fffffff
}

