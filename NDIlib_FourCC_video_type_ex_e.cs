using System;

namespace Tractus.Ndi;
public enum NDIlib_FourCC_video_type_ex_e : uint
{
    NDIlib_FourCC_video_type_I420 = 808596553,
    NDIlib_FourCC_video_type_NV12 = 842094158,
    NDIlib_FourCC_video_type_YV12 = 842094169,
    NDIlib_FourCC_type_P216 = 909193808,
    NDIlib_FourCC_type_PA16 = 909197648,
    NDIlib_FourCC_type_RGBA = 1094862674,
    NDIlib_FourCC_type_BGRA = 1095911234,
    NDIlib_FourCC_type_UYVA = 1096178005,
    NDIlib_FourCC_type_RGBX = 1480738642,
    NDIlib_FourCC_type_BGRX = 1481787202,
    NDIlib_FourCC_type_UYVY = 1498831189,

    // SpeedHQ formats at the highest bandwidth (4:2:0)
    NDIlib_FourCC_video_type_ex_SHQ0_highest_bandwidth = ((uint)'S' | ((uint)'H' << 8) | ((uint)'Q' << 16) | ((uint)'0' << 24)),
    NDIlib_FourCC_type_SHQ0_highest_bandwidth = NDIlib_FourCC_video_type_ex_SHQ0_highest_bandwidth,

    // SpeedHQ formats at the highest bandwidth (4:2:2)
    NDIlib_FourCC_video_type_ex_SHQ2_highest_bandwidth = ((uint)'S' | ((uint)'H' << 8) | ((uint)'Q' << 16) | ((uint)'2' << 24)),
    NDIlib_FourCC_type_SHQ2_highest_bandwidth = NDIlib_FourCC_video_type_ex_SHQ2_highest_bandwidth,

    // SpeedHQ formats at the highest bandwidth (4:2:2:4)
    NDIlib_FourCC_video_type_ex_SHQ7_highest_bandwidth = ((uint)'S' | ((uint)'H' << 8) | ((uint)'Q' << 16) | ((uint)'7' << 24)),
    NDIlib_FourCC_type_SHQ7_highest_bandwidth = NDIlib_FourCC_video_type_ex_SHQ7_highest_bandwidth,

    // SpeedHQ formats at the lowest bandwidth (4:2:0)
    NDIlib_FourCC_video_type_ex_SHQ0_lowest_bandwidth = ((uint)'s' | ((uint)'h' << 8) | ((uint)'q' << 16) | ((uint)'0' << 24)),
    NDIlib_FourCC_type_SHQ0_lowest_bandwidth = NDIlib_FourCC_video_type_ex_SHQ0_lowest_bandwidth,

    // SpeedHQ formats at the lowest bandwidth (4:2:2)
    NDIlib_FourCC_video_type_ex_SHQ2_lowest_bandwidth = ((uint)'s' | ((uint)'h' << 8) | ((uint)'q' << 16) | ((uint)'2' << 24)),
    NDIlib_FourCC_type_SHQ2_lowest_bandwidth = NDIlib_FourCC_video_type_ex_SHQ2_lowest_bandwidth,

    // SpeedHQ formats at the lowest bandwidth (4:2:2:4)
    NDIlib_FourCC_video_type_ex_SHQ7_lowest_bandwidth = ((uint)'s' | ((uint)'h' << 8) | ((uint)'q' << 16) | ((uint)'7' << 24)),
    NDIlib_FourCC_type_SHQ7_lowest_bandwidth = NDIlib_FourCC_video_type_ex_SHQ7_lowest_bandwidth,

    // H.264 video at the highest bandwidth (data prefixed with NDIlib_compressed_packet_t)
    NDIlib_FourCC_video_type_ex_H264_highest_bandwidth = ((uint)'H' | ((uint)'2' << 8) | ((uint)'6' << 16) | ((uint)'4' << 24)),
    NDIlib_FourCC_type_H264_highest_bandwidth = NDIlib_FourCC_video_type_ex_H264_highest_bandwidth,

    // H.264 video at the lowest bandwidth
    NDIlib_FourCC_video_type_ex_H264_lowest_bandwidth = ((uint)'h' | ((uint)'2' << 8) | ((uint)'6' << 16) | ((uint)'4' << 24)),
    NDIlib_FourCC_type_H264_lowest_bandwidth = NDIlib_FourCC_video_type_ex_H264_lowest_bandwidth,

    // H.265/HEVC video at the highest bandwidth
    NDIlib_FourCC_video_type_ex_HEVC_highest_bandwidth = ((uint)'H' | ((uint)'E' << 8) | ((uint)'V' << 16) | ((uint)'C' << 24)),
    NDIlib_FourCC_type_HEVC_highest_bandwidth = NDIlib_FourCC_video_type_ex_HEVC_highest_bandwidth,

    // H.265/HEVC video at the lowest bandwidth
    NDIlib_FourCC_video_type_ex_HEVC_lowest_bandwidth = ((uint)'h' | ((uint)'e' << 8) | ((uint)'v' << 16) | ((uint)'c' << 24)),
    NDIlib_FourCC_type_HEVC_lowest_bandwidth = NDIlib_FourCC_video_type_ex_HEVC_lowest_bandwidth,

    // H.264 video with alpha at the highest bandwidth
    NDIlib_FourCC_video_type_ex_H264_alpha_highest_bandwidth = ((uint)'A' | ((uint)'2' << 8) | ((uint)'6' << 16) | ((uint)'4' << 24)),
    NDIlib_FourCC_type_h264_alpha_highest_bandwidth = NDIlib_FourCC_video_type_ex_H264_alpha_highest_bandwidth,

    // H.264 video with alpha at the lowest bandwidth
    NDIlib_FourCC_video_type_ex_H264_alpha_lowest_bandwidth = ((uint)'a' | ((uint)'2' << 8) | ((uint)'6' << 16) | ((uint)'4' << 24)),
    NDIlib_FourCC_type_h264_alpha_lowest_bandwidth = NDIlib_FourCC_video_type_ex_H264_alpha_lowest_bandwidth,

    // H.265/HEVC video with alpha at the highest bandwidth
    NDIlib_FourCC_video_type_ex_HEVC_alpha_highest_bandwidth = ((uint)'A' | ((uint)'E' << 8) | ((uint)'V' << 16) | ((uint)'C' << 24)),
    NDIlib_FourCC_type_HEVC_alpha_highest_bandwidth = NDIlib_FourCC_video_type_ex_HEVC_alpha_highest_bandwidth,

    // H.265/HEVC video with alpha at the lowest bandwidth
    NDIlib_FourCC_video_type_ex_HEVC_alpha_lowest_bandwidth = ((uint)'a' | ((uint)'e' << 8) | ((uint)'v' << 16) | ((uint)'c' << 24)),
    NDIlib_FourCC_type_HEVC_alpha_lowest_bandwidth = NDIlib_FourCC_video_type_ex_HEVC_alpha_lowest_bandwidth,

    // Ensure a 32-bit enumeration.
    NDIlib_FourCC_video_type_ex_max = 0x7fffffff,
}

