namespace Tractus.Ndi;

public enum FourCC_type_e
{
    FourCC_type_UYVY = 0x59565955,

    // 4:2:0 formats
    NDIlib_FourCC_video_type_YV12 = 0x32315659,
    NDIlib_FourCC_video_type_NV12 = 0x3231564E,
    NDIlib_FourCC_video_type_I420 = 0x30323449,

    // BGRA
    FourCC_type_BGRA = 0x41524742,
    FourCC_type_BGRX = 0x58524742,

    // RGBA
    FourCC_type_RGBA = 0x41424752,
    FourCC_type_RGBX = 0x58424752,

    // P216/PA16
    FourCC_type_P216 = 0x36313250,
    FourCC_type_PA16 = 0x36314150,

    // This is a UYVY buffer followed immediately by an alpha channel buffer.
    // If the stride of the YCbCr component is "stride", then the alpha channel
    // starts at image_ptr + yres*stride. The alpha channel stride is stride/2.
    FourCC_type_UYVA = 0x41565955,

    FourCC_type_ex_H264_highest_bandwidth = 875967048,

    NDIlib_FourCC_video_type_UYVY = FourCC_type_UYVY,
    NDIlib_FourCC_video_type_UYVA = FourCC_type_UYVA,
    NDIlib_FourCC_video_type_P216 = FourCC_type_P216,
    NDIlib_FourCC_video_type_PA16 = FourCC_type_PA16,
    FourCC_type_YV12 = NDIlib_FourCC_video_type_YV12,
    FourCC_type_I420 = NDIlib_FourCC_video_type_I420,
    FourCC_type_NV12 = NDIlib_FourCC_video_type_NV12,
    NDIlib_FourCC_video_type_BGRA = FourCC_type_BGRA,
    NDIlib_FourCC_video_type_BGRX = FourCC_type_BGRX,
    NDIlib_FourCC_video_type_RGBA = FourCC_type_RGBA,
    NDIlib_FourCC_video_type_RGBX = FourCC_type_RGBX,
    NDIlib_FourCC_video_type_NA48 = 0x3834414E,
    FourCC_type_NA48 = NDIlib_FourCC_video_type_NA48,
    NDIlib_FourCC_video_type_NB48 = 0x3834424E,
    FourCC_type_NB48 = NDIlib_FourCC_video_type_NB48,
    NDIlib_FourCC_video_type_NA4G = 0x4734414E,
    FourCC_type_NA4G = NDIlib_FourCC_video_type_NA4G,
    NDIlib_FourCC_video_type_NB4G = 0x4734424E,
    FourCC_type_NB4G = NDIlib_FourCC_video_type_NB4G,
    NDIlib_FourCC_video_type_max = 0x7FFFFFFF,
}
