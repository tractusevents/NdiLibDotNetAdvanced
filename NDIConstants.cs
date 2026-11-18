namespace Tractus.Ndi;

public static class NDIConstants
{
    // FourCC constants for video frame types
    public const uint NDIlib_FourCC_video_type_ex_SHQ0_highest_bandwidth =
          ((uint)'S') | (((uint)'H') << 8) | (((uint)'Q') << 16) | (((uint)'0') << 24);
    public const uint NDIlib_FourCC_type_SHQ0_highest_bandwidth = NDIlib_FourCC_video_type_ex_SHQ0_highest_bandwidth;

    public const uint NDIlib_FourCC_video_type_ex_SHQ2_highest_bandwidth =
          ((uint)'S') | (((uint)'H') << 8) | (((uint)'Q') << 16) | (((uint)'2') << 24);
    public const uint NDIlib_FourCC_type_SHQ2_highest_bandwidth = NDIlib_FourCC_video_type_ex_SHQ2_highest_bandwidth;

    public const uint NDIlib_FourCC_video_type_ex_SHQ7_highest_bandwidth =
          ((uint)'S') | (((uint)'H') << 8) | (((uint)'Q') << 16) | (((uint)'7') << 24);
    public const uint NDIlib_FourCC_type_SHQ7_highest_bandwidth = NDIlib_FourCC_video_type_ex_SHQ7_highest_bandwidth;

    public const uint NDIlib_FourCC_video_type_ex_SHQ0_lowest_bandwidth =
          ((uint)'s') | (((uint)'h') << 8) | (((uint)'q') << 16) | (((uint)'0') << 24);
    public const uint NDIlib_FourCC_type_SHQ0_lowest_bandwidth = NDIlib_FourCC_video_type_ex_SHQ0_lowest_bandwidth;

    public const uint NDIlib_FourCC_video_type_ex_SHQ2_lowest_bandwidth =
          ((uint)'s') | (((uint)'h') << 8) | (((uint)'q') << 16) | (((uint)'2') << 24);
    public const uint NDIlib_FourCC_type_SHQ2_lowest_bandwidth = NDIlib_FourCC_video_type_ex_SHQ2_lowest_bandwidth;

    public const uint NDIlib_FourCC_video_type_ex_SHQ7_lowest_bandwidth =
          ((uint)'s') | (((uint)'h') << 8) | (((uint)'q') << 16) | (((uint)'7') << 24);
    public const uint NDIlib_FourCC_type_SHQ7_lowest_bandwidth = NDIlib_FourCC_video_type_ex_SHQ7_lowest_bandwidth;

    public const uint NDIlib_FourCC_video_type_ex_H264_highest_bandwidth =
          ((uint)'H') | (((uint)'2') << 8) | (((uint)'6') << 16) | (((uint)'4') << 24);
    public const uint NDIlib_FourCC_type_H264_highest_bandwidth = NDIlib_FourCC_video_type_ex_H264_highest_bandwidth;

    public const uint NDIlib_FourCC_video_type_ex_H264_lowest_bandwidth =
          ((uint)'h') | (((uint)'2') << 8) | (((uint)'6') << 16) | (((uint)'4') << 24);
    public const uint NDIlib_FourCC_type_H264_lowest_bandwidth = NDIlib_FourCC_video_type_ex_H264_lowest_bandwidth;

    public const uint NDIlib_FourCC_video_type_ex_HEVC_highest_bandwidth =
          ((uint)'H') | (((uint)'E') << 8) | (((uint)'V') << 16) | (((uint)'C') << 24);
    public const uint NDIlib_FourCC_type_HEVC_highest_bandwidth = NDIlib_FourCC_video_type_ex_HEVC_highest_bandwidth;

    public const uint NDIlib_FourCC_video_type_ex_HEVC_lowest_bandwidth =
          ((uint)'h') | (((uint)'e') << 8) | (((uint)'v') << 16) | (((uint)'c') << 24);
    public const uint NDIlib_FourCC_type_HEVC_lowest_bandwidth = NDIlib_FourCC_video_type_ex_HEVC_lowest_bandwidth;

    public const uint NDIlib_FourCC_video_type_ex_H264_alpha_highest_bandwidth =
          ((uint)'A') | (((uint)'2') << 8) | (((uint)'6') << 16) | (((uint)'4') << 24);
    public const uint NDIlib_FourCC_type_h264_alpha_highest_bandwidth = NDIlib_FourCC_video_type_ex_H264_alpha_highest_bandwidth;

    public const uint NDIlib_FourCC_video_type_ex_H264_alpha_lowest_bandwidth =
          ((uint)'a') | (((uint)'2') << 8) | (((uint)'6') << 16) | (((uint)'4') << 24);
    public const uint NDIlib_FourCC_type_h264_alpha_lowest_bandwidth = NDIlib_FourCC_video_type_ex_H264_alpha_lowest_bandwidth;

    public const uint NDIlib_FourCC_video_type_ex_HEVC_alpha_highest_bandwidth =
          ((uint)'A') | (((uint)'E') << 8) | (((uint)'V') << 16) | (((uint)'C') << 24);
    public const uint NDIlib_FourCC_type_HEVC_alpha_highest_bandwidth = NDIlib_FourCC_video_type_ex_HEVC_alpha_highest_bandwidth;

    public const uint NDIlib_FourCC_video_type_ex_HEVC_alpha_lowest_bandwidth =
          ((uint)'a') | (((uint)'e') << 8) | (((uint)'v') << 16) | (((uint)'c') << 24);
    public const uint NDIlib_FourCC_type_HEVC_alpha_lowest_bandwidth = NDIlib_FourCC_video_type_ex_HEVC_alpha_lowest_bandwidth;

    public const uint NDIlib_FourCC_video_type_ex_max = 0x7fffffff;

    // FourCC constants for audio frame types
    public const uint NDIlib_FourCC_audio_type_ex_AAC = 0x00ff;
    public const uint NDIlib_FourCC_audio_type_ex_OPUS =
          ((uint)'O') | (((uint)'p') << 8) | (((uint)'u') << 16) | (((uint)'s') << 24);
    public const uint NDIlib_FourCC_audio_type_ex_max = 0x7fffffff;

    // FourCC constants for compressed packet types
    public const uint NDIlib_compressed_FourCC_type_H264 =
          ((uint)'H') | (((uint)'2') << 8) | (((uint)'6') << 16) | (((uint)'4') << 24);
    public const uint NDIlib_FourCC_type_H264 = NDIlib_compressed_FourCC_type_H264;

    public const uint NDIlib_compressed_FourCC_type_HEVC =
          ((uint)'H') | (((uint)'E') << 8) | (((uint)'V') << 16) | (((uint)'C') << 24);
    public const uint NDIlib_FourCC_type_HEVC = NDIlib_compressed_FourCC_type_HEVC;

    public const uint NDIlib_compressed_FourCC_type_AAC = 0x00ff;
    public const uint NDIlib_FourCC_type_AAC = NDIlib_compressed_FourCC_type_AAC;

    public const uint NDIlib_compressed_FourCC_type_max = 0x7fffffff;

    // Constants for color formats
    public const uint NDIlib_recv_color_format_ex_compressed = 300;
    public const uint NDIlib_recv_color_format_compressed = NDIlib_recv_color_format_ex_compressed;

    public const uint NDIlib_recv_color_format_ex_compressed_v1 = 300;
    public const uint NDIlib_recv_color_format_compressed_v1 = NDIlib_recv_color_format_ex_compressed_v1;

    public const uint NDIlib_recv_color_format_ex_compressed_v2 = 301;
    public const uint NDIlib_recv_color_format_compressed_v2 = NDIlib_recv_color_format_ex_compressed_v2;

    public const uint NDIlib_recv_color_format_ex_compressed_v2_best = 309;
    public const uint NDIlib_recv_color_format_compressed_v2_best = NDIlib_recv_color_format_ex_compressed_v2_best;

    public const uint NDIlib_recv_color_format_ex_compressed_v3 = 302;
    public const uint NDIlib_recv_color_format_compressed_v3 = NDIlib_recv_color_format_ex_compressed_v3;

    public const uint NDIlib_recv_color_format_ex_compressed_v3_best = 310;
    public const uint NDIlib_recv_color_format_compressed_v3_best = NDIlib_recv_color_format_ex_compressed_v3_best;

    public const uint NDIlib_recv_color_format_ex_compressed_v3_with_audio = 304;
    public const uint NDIlib_recv_color_format_compressed_v3_with_audio = NDIlib_recv_color_format_ex_compressed_v3_with_audio;

    public const uint NDIlib_recv_color_format_ex_compressed_v4 = 303;
    public const uint NDIlib_recv_color_format_compressed_v4 = NDIlib_recv_color_format_ex_compressed_v4;

    public const uint NDIlib_recv_color_format_ex_compressed_v4_best = 311;
    public const uint NDIlib_recv_color_format_compressed_v4_best = NDIlib_recv_color_format_ex_compressed_v4_best;

    public const uint NDIlib_recv_color_format_ex_compressed_v4_with_audio = 305;
    public const uint NDIlib_recv_color_format_compressed_v4_with_audio = NDIlib_recv_color_format_ex_compressed_v4_with_audio;

    public const uint NDIlib_recv_color_format_ex_compressed_v5 = 307;
    public const uint NDIlib_recv_color_format_compressed_v5 = NDIlib_recv_color_format_ex_compressed_v5;

    public const uint NDIlib_recv_color_format_ex_compressed_v5_best = 312;
    public const uint NDIlib_recv_color_format_compressed_v5_best = NDIlib_recv_color_format_ex_compressed_v5_best;

    public const uint NDIlib_recv_color_format_ex_compressed_v5_with_audio = 308;
    public const uint NDIlib_recv_color_format_compressed_v5_with_audio = NDIlib_recv_color_format_ex_compressed_v5_with_audio;

    // Allow SpeedHQ frames, compressed H.264 frames, HEVC frames and HEVC/H264 with alpha -- protected or not.
    public const uint NDIlib_recv_color_format_ex_compressed_v6 = 313;
    public const uint NDIlib_recv_color_format_compressed_v6 = NDIlib_recv_color_format_ex_compressed_v6;

    // This is like a combination of the NDIlib_recv_color_format_best and NDIlib_recv_color_format_compressed_v6
    // formats. Instead of delivering just UYVY or UYVA if decompressed, a 16-bit format such as P216 or PA16
    // can also be delivered.
    public const uint NDIlib_recv_color_format_ex_compressed_v6_best = 314;
    public const uint NDIlib_recv_color_format_compressed_v6_best = NDIlib_recv_color_format_ex_compressed_v6_best;

    // Allow SpeedHQ frames, compressed H.264 frames, HEVC frames and HEVC/H264 with alpha, along with
    // compressed audio frames and OPUS support -- protected or not.
    public const uint NDIlib_recv_color_format_ex_compressed_v6_with_audio = 315;
    public const uint NDIlib_recv_color_format_compressed_v6_with_audio = NDIlib_recv_color_format_ex_compressed_v6_with_audio;

    public const uint NDIlib_recv_color_format_ex_max = 0x7fffffff;

    public const uint NDIlib_FourCC_video_type_UYVY = (uint)FourCC_type_e.NDIlib_FourCC_video_type_UYVY;
    public const uint NDIlib_FourCC_type_UYVY = (uint)FourCC_type_e.FourCC_type_UYVY;
    public const uint NDIlib_FourCC_video_type_UYVA = (uint)FourCC_type_e.NDIlib_FourCC_video_type_UYVA;
    public const uint NDIlib_FourCC_type_UYVA = (uint)FourCC_type_e.FourCC_type_UYVA;
    public const uint NDIlib_FourCC_video_type_P216 = (uint)FourCC_type_e.NDIlib_FourCC_video_type_P216;
    public const uint NDIlib_FourCC_type_P216 = (uint)FourCC_type_e.FourCC_type_P216;
    public const uint NDIlib_FourCC_video_type_PA16 = (uint)FourCC_type_e.NDIlib_FourCC_video_type_PA16;
    public const uint NDIlib_FourCC_type_PA16 = (uint)FourCC_type_e.FourCC_type_PA16;
    public const uint NDIlib_FourCC_video_type_YV12 = (uint)FourCC_type_e.NDIlib_FourCC_video_type_YV12;
    public const uint NDIlib_FourCC_type_YV12 = (uint)FourCC_type_e.FourCC_type_YV12;
    public const uint NDIlib_FourCC_video_type_I420 = (uint)FourCC_type_e.NDIlib_FourCC_video_type_I420;
    public const uint NDIlib_FourCC_type_I420 = (uint)FourCC_type_e.FourCC_type_I420;
    public const uint NDIlib_FourCC_video_type_NV12 = (uint)FourCC_type_e.NDIlib_FourCC_video_type_NV12;
    public const uint NDIlib_FourCC_type_NV12 = (uint)FourCC_type_e.FourCC_type_NV12;
    public const uint NDIlib_FourCC_video_type_BGRA = (uint)FourCC_type_e.NDIlib_FourCC_video_type_BGRA;
    public const uint NDIlib_FourCC_type_BGRA = (uint)FourCC_type_e.FourCC_type_BGRA;
    public const uint NDIlib_FourCC_video_type_BGRX = (uint)FourCC_type_e.NDIlib_FourCC_video_type_BGRX;
    public const uint NDIlib_FourCC_type_BGRX = (uint)FourCC_type_e.FourCC_type_BGRX;
    public const uint NDIlib_FourCC_video_type_RGBA = (uint)FourCC_type_e.NDIlib_FourCC_video_type_RGBA;
    public const uint NDIlib_FourCC_type_RGBA = (uint)FourCC_type_e.FourCC_type_RGBA;
    public const uint NDIlib_FourCC_video_type_RGBX = (uint)FourCC_type_e.NDIlib_FourCC_video_type_RGBX;
    public const uint NDIlib_FourCC_type_RGBX = (uint)FourCC_type_e.FourCC_type_RGBX;
    public const uint NDIlib_FourCC_video_type_NA48 = (uint)FourCC_type_e.NDIlib_FourCC_video_type_NA48;
    public const uint NDIlib_FourCC_type_NA48 = (uint)FourCC_type_e.FourCC_type_NA48;
    public const uint NDIlib_FourCC_video_type_NB48 = (uint)FourCC_type_e.NDIlib_FourCC_video_type_NB48;
    public const uint NDIlib_FourCC_type_NB48 = (uint)FourCC_type_e.FourCC_type_NB48;
    public const uint NDIlib_FourCC_video_type_NA4G = (uint)FourCC_type_e.NDIlib_FourCC_video_type_NA4G;
    public const uint NDIlib_FourCC_type_NA4G = (uint)FourCC_type_e.FourCC_type_NA4G;
    public const uint NDIlib_FourCC_video_type_NB4G = (uint)FourCC_type_e.NDIlib_FourCC_video_type_NB4G;
    public const uint NDIlib_FourCC_type_NB4G = (uint)FourCC_type_e.FourCC_type_NB4G;
    public const uint NDIlib_FourCC_video_type_max = (uint)FourCC_type_e.NDIlib_FourCC_video_type_max;
    public const uint NDIlib_FourCC_video_type_ex_SHQ0_highest_bandwidth_protected = (uint)FourCC_video_type_ex_e.NDIlib_FourCC_video_type_ex_SHQ0_highest_bandwidth_protected;
    public const uint NDIlib_FourCC_type_SHQ0_highest_bandwidth_protected = (uint)FourCC_video_type_ex_e.NDIlib_FourCC_type_SHQ0_highest_bandwidth_protected;
    public const uint NDIlib_FourCC_video_type_ex_SHQ2_highest_bandwidth_protected = (uint)FourCC_video_type_ex_e.NDIlib_FourCC_video_type_ex_SHQ2_highest_bandwidth_protected;
    public const uint NDIlib_FourCC_type_SHQ2_highest_bandwidth_protected = (uint)FourCC_video_type_ex_e.NDIlib_FourCC_type_SHQ2_highest_bandwidth_protected;
    public const uint NDIlib_FourCC_video_type_ex_SHQ7_highest_bandwidth_protected = (uint)FourCC_video_type_ex_e.NDIlib_FourCC_video_type_ex_SHQ7_highest_bandwidth_protected;
    public const uint NDIlib_FourCC_type_SHQ7_highest_bandwidth_protected = (uint)FourCC_video_type_ex_e.NDIlib_FourCC_type_SHQ7_highest_bandwidth_protected;
    public const uint NDIlib_FourCC_video_type_ex_SHQ0_lowest_bandwidth_protected = (uint)FourCC_video_type_ex_e.NDIlib_FourCC_video_type_ex_SHQ0_lowest_bandwidth_protected;
    public const uint NDIlib_FourCC_type_SHQ0_lowest_bandwidth_protected = (uint)FourCC_video_type_ex_e.NDIlib_FourCC_type_SHQ0_lowest_bandwidth_protected;
    public const uint NDIlib_FourCC_video_type_ex_SHQ2_lowest_bandwidth_protected = (uint)FourCC_video_type_ex_e.NDIlib_FourCC_video_type_ex_SHQ2_lowest_bandwidth_protected;
    public const uint NDIlib_FourCC_type_SHQ2_lowest_bandwidth_protected = (uint)FourCC_video_type_ex_e.NDIlib_FourCC_type_SHQ2_lowest_bandwidth_protected;
    public const uint NDIlib_FourCC_video_type_ex_SHQ7_lowest_bandwidth_protected = (uint)FourCC_video_type_ex_e.NDIlib_FourCC_video_type_ex_SHQ7_lowest_bandwidth_protected;
    public const uint NDIlib_FourCC_type_SHQ7_lowest_bandwidth_protected = (uint)FourCC_video_type_ex_e.NDIlib_FourCC_type_SHQ7_lowest_bandwidth_protected;
    public const uint NDIlib_FourCC_video_type_ex_H264_highest_bandwidth_protected = (uint)FourCC_video_type_ex_e.NDIlib_FourCC_video_type_ex_H264_highest_bandwidth_protected;
    public const uint NDIlib_FourCC_type_H264_highest_bandwidth_protected = (uint)FourCC_video_type_ex_e.NDIlib_FourCC_type_H264_highest_bandwidth_protected;
    public const uint NDIlib_FourCC_video_type_ex_H264_lowest_bandwidth_protected = (uint)FourCC_video_type_ex_e.NDIlib_FourCC_video_type_ex_H264_lowest_bandwidth_protected;
    public const uint NDIlib_FourCC_type_H264_lowest_bandwidth_protected = (uint)FourCC_video_type_ex_e.NDIlib_FourCC_type_H264_lowest_bandwidth_protected;
    public const uint NDIlib_FourCC_video_type_ex_HEVC_highest_bandwidth_protected = (uint)FourCC_video_type_ex_e.NDIlib_FourCC_video_type_ex_HEVC_highest_bandwidth_protected;
    public const uint NDIlib_FourCC_type_HEVC_highest_bandwidth_protected = (uint)FourCC_video_type_ex_e.NDIlib_FourCC_type_HEVC_highest_bandwidth_protected;
    public const uint NDIlib_FourCC_video_type_ex_HEVC_lowest_bandwidth_protected = (uint)FourCC_video_type_ex_e.NDIlib_FourCC_video_type_ex_HEVC_lowest_bandwidth_protected;
    public const uint NDIlib_FourCC_type_HEVC_lowest_bandwidth_protected = (uint)FourCC_video_type_ex_e.NDIlib_FourCC_type_HEVC_lowest_bandwidth_protected;
    public const uint NDIlib_FourCC_video_type_ex_H264_alpha_highest_bandwidth_protected = (uint)FourCC_video_type_ex_e.NDIlib_FourCC_video_type_ex_H264_alpha_highest_bandwidth_protected;
    public const uint NDIlib_FourCC_type_h264_alpha_highest_bandwidth_protected = (uint)FourCC_video_type_ex_e.NDIlib_FourCC_type_h264_alpha_highest_bandwidth_protected;
    public const uint NDIlib_FourCC_video_type_ex_H264_alpha_lowest_bandwidth_protected = (uint)FourCC_video_type_ex_e.NDIlib_FourCC_video_type_ex_H264_alpha_lowest_bandwidth_protected;
    public const uint NDIlib_FourCC_type_h264_alpha_lowest_bandwidth_protected = (uint)FourCC_video_type_ex_e.NDIlib_FourCC_type_h264_alpha_lowest_bandwidth_protected;
    public const uint NDIlib_FourCC_video_type_ex_HEVC_alpha_highest_bandwidth_protected = (uint)FourCC_video_type_ex_e.NDIlib_FourCC_video_type_ex_HEVC_alpha_highest_bandwidth_protected;
    public const uint NDIlib_FourCC_type_HEVC_alpha_highest_bandwidth_protected = (uint)FourCC_video_type_ex_e.NDIlib_FourCC_type_HEVC_alpha_highest_bandwidth_protected;
    public const uint NDIlib_FourCC_video_type_ex_HEVC_alpha_lowest_bandwidth_protected = (uint)FourCC_video_type_ex_e.NDIlib_FourCC_video_type_ex_HEVC_alpha_lowest_bandwidth_protected;
    public const uint NDIlib_FourCC_type_HEVC_alpha_lowest_bandwidth_protected = (uint)FourCC_video_type_ex_e.NDIlib_FourCC_type_HEVC_alpha_lowest_bandwidth_protected;
    public const uint NDIlib_FourCC_audio_type_FLTP = (uint)FourCC_audio_type_e.FourCC_audio_type_FLTP;
    public const uint NDIlib_FourCC_type_FLTP = (uint)FourCC_audio_type_e.FourCC_type_FLTP;
    public const uint NDIlib_FourCC_audio_type_max = (uint)FourCC_audio_type_e.FourCC_audio_type_max;
    public const uint NDIlib_FourCC_audio_type_ex_AAC_protected = (uint)NDIlib_FourCC_audio_type_ex_e.NDIlib_FourCC_audio_type_ex_AAC_protected;
    public const uint NDIlib_FourCC_audio_type_ex_OPUS_protected = (uint)NDIlib_FourCC_audio_type_ex_e.NDIlib_FourCC_audio_type_ex_OPUS_protected;
}
