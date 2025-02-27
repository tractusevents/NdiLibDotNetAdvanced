using System;

namespace Tractus.Ndi;
public enum NDIlib_compressed_FourCC_type_e : uint
{
    NDIlib_compressed_FourCC_type_H264 = ((uint)'H' | ((uint)'2' << 8) | ((uint)'6' << 16) | ((uint)'4' << 24)), //NDI_LIB_FOURCC('H', '2', '6', '4'),
    NDIlib_FourCC_type_H264 = NDIlib_compressed_FourCC_type_H264,

    // Used in the NDIlib_compressed_packet type to signify H.265/HEVC video data that is prefixed with the
    // NDIlib_compressed_packet_t structure.
    NDIlib_compressed_FourCC_type_HEVC = ((uint)'H' | ((uint)'E' << 8) | ((uint)'V' << 16) | ((uint)'C' << 24)), //NDI_LIB_FOURCC('H', 'E', 'V', 'C'),
    NDIlib_FourCC_type_HEVC = NDIlib_compressed_FourCC_type_HEVC,

    // Used in the NDIlib_compressed_packet type to signify AAC audio data that is prefixed with the
    // NDIlib_compressed_packet_t structure.
    NDIlib_compressed_FourCC_type_AAC = 0x00ff,
    NDIlib_FourCC_type_AAC = NDIlib_compressed_FourCC_type_AAC,

    // Make sure this is a 32-bit enumeration.
    NDIlib_compressed_FourCC_type_max = 0x7fffffff
}

