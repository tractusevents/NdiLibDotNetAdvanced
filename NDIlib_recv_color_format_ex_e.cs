using System;

namespace Tractus.Ndi;
public enum NDIlib_recv_color_format_ex_e
{
    // Request that compressed video data is the desired format.
    NDIlib_recv_color_format_ex_compressed = 300,
    NDIlib_recv_color_format_compressed = NDIlib_recv_color_format_ex_compressed,

    // Allow only compressed SpeedHQ frames to pass through.
    NDIlib_recv_color_format_ex_compressed_v1 = 300,
    NDIlib_recv_color_format_compressed_v1 = NDIlib_recv_color_format_ex_compressed_v1,

    // Allow SpeedHQ frames along with UYVY video to pass through. This would mean HX and H.264 sources can
    // be decompressed and returned back as UYVY.
    NDIlib_recv_color_format_ex_compressed_v2 = 301,
    NDIlib_recv_color_format_compressed_v2 = NDIlib_recv_color_format_ex_compressed_v2,

    // This is like a combination of the NDIlib_recv_color_format_best and NDIlib_recv_color_format_compressed_v2
    // formats. Instead of delivering just UYVY if decompressed, a 16-bit format such as P216 can also be delivered.
    NDIlib_recv_color_format_ex_compressed_v2_best = 309,
    NDIlib_recv_color_format_compressed_v2_best = NDIlib_recv_color_format_ex_compressed_v2_best,

    // Allow SpeedHQ frames, compressed H.264 frames.
    NDIlib_recv_color_format_ex_compressed_v3 = 302,
    NDIlib_recv_color_format_compressed_v3 = NDIlib_recv_color_format_ex_compressed_v3,

    // This is like a combination of the NDIlib_recv_color_format_best and NDIlib_recv_color_format_compressed_v3
    // formats. Instead of delivering just UYVY if decompressed, a 16-bit format such as P216 can also be delivered.
    NDIlib_recv_color_format_ex_compressed_v3_best = 310,
    NDIlib_recv_color_format_compressed_v3_best = NDIlib_recv_color_format_ex_compressed_v3_best,

    // Allow SpeedHQ frames, compressed H.264 frames, along with compressed audio frames.
    NDIlib_recv_color_format_ex_compressed_v3_with_audio = 304,
    NDIlib_recv_color_format_compressed_v3_with_audio = NDIlib_recv_color_format_ex_compressed_v3_with_audio,

    // Allow SpeedHQ frames, compressed H.264 frames, HEVC frames.
    NDIlib_recv_color_format_ex_compressed_v4 = 303,
    NDIlib_recv_color_format_compressed_v4 = NDIlib_recv_color_format_ex_compressed_v4,

    // This is like a combination of the NDIlib_recv_color_format_best and NDIlib_recv_color_format_compressed_v4
    // formats. Instead of delivering just UYVY if decompressed, a 16-bit format such as P216 can also be delivered.
    NDIlib_recv_color_format_ex_compressed_v4_best = 311,
    NDIlib_recv_color_format_compressed_v4_best = NDIlib_recv_color_format_ex_compressed_v4_best,

    // Allow SpeedHQ frames, compressed H.264 frames, HEVC frames, along with compressed audio frames.
    NDIlib_recv_color_format_ex_compressed_v4_with_audio = 305,
    NDIlib_recv_color_format_compressed_v4_with_audio = NDIlib_recv_color_format_ex_compressed_v4_with_audio,

    // Allow SpeedHQ frames, compressed H.264 frames, HEVC frames and HEVC/H264 with alpha.
    NDIlib_recv_color_format_ex_compressed_v5 = 307,
    NDIlib_recv_color_format_compressed_v5 = NDIlib_recv_color_format_ex_compressed_v5,

    // This is like a combination of the NDIlib_recv_color_format_best and NDIlib_recv_color_format_compressed_v5
    // formats. Instead of delivering just UYVY or UYVA if decompressed, a 16-bit format such as P216 or PA16
    // can also be delivered.
    NDIlib_recv_color_format_ex_compressed_v5_best = 312,
    NDIlib_recv_color_format_compressed_v5_best = NDIlib_recv_color_format_ex_compressed_v5_best,

    // Allow SpeedHQ frames, compressed H.264 frames, HEVC frames and HEVC/H264 with alpha, along with
    // compressed audio frames and OPUS support.
    NDIlib_recv_color_format_ex_compressed_v5_with_audio = 308,
    NDIlib_recv_color_format_compressed_v5_with_audio = NDIlib_recv_color_format_ex_compressed_v5_with_audio,

    // Make sure this is a 32-bit enumeration.
    NDIlib_recv_color_format_ex_max = 0x7fffffff
}

