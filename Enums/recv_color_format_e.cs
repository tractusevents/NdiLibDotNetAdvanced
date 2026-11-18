namespace Tractus.Ndi;

public enum recv_color_format_e
{
    // No alpha channel: BGRX Alpha channel: BGRA
    recv_color_format_BGRX_BGRA = 0,

    // No alpha channel: UYVY Alpha channel: BGRA
    recv_color_format_UYVY_BGRA = 1,

    // No alpha channel: RGBX Alpha channel: RGBA
    recv_color_format_RGBX_RGBA = 2,

    // No alpha channel: UYVY Alpha channel: RGBA
    recv_color_format_UYVY_RGBA = 3,

    // On Windows there are some APIs that require bottom to top images in RGBA format. Specifying
    // this format will return images in this format. The image data pointer will still point to the
    // "top" of the image, althought he stride will be negative. You can get the "bottom" line of the image
    // using : video_data.p_data + (video_data.yres - 1)*video_data.line_stride_in_bytes
    recv_color_format_BGRX_BGRA_flipped = 1000,

    // Read the SDK documentation to understand the pros and cons of this format.
    recv_color_format_fastest = 100,

    recv_color_format_best = 101,

    // Legacy definitions for backwards compatibility
    recv_color_format_e_BGRX_BGRA = recv_color_format_BGRX_BGRA,
    recv_color_format_e_UYVY_BGRA = recv_color_format_UYVY_BGRA,
    recv_color_format_e_RGBX_RGBA = recv_color_format_RGBX_RGBA,
    recv_color_format_e_UYVY_RGBA = recv_color_format_UYVY_RGBA,

    recv_color_format_best_444 = 102,
    recv_color_format_max = 0x7fffffff,
}
