using System.Runtime.InteropServices;

namespace Tractus.Ndi;

/// <summary>Legacy video-frame layout retained by the native compatibility API.</summary>
[StructLayout(LayoutKind.Sequential)]
public struct video_frame_t
{
    public int xres;
    public int yres;
    public FourCC_type_e FourCC;
    public int frame_rate_N;
    public int frame_rate_D;
    public float picture_aspect_ratio;
    public frame_format_type_e frame_format_type;
    public long timecode;
    public nint p_data;
    public int line_stride_in_bytes;
}

/// <summary>Legacy planar floating-point audio-frame layout retained by the native compatibility API.</summary>
[StructLayout(LayoutKind.Sequential)]
public struct audio_frame_t
{
    public int sample_rate;
    public int no_channels;
    public int no_samples;
    public long timecode;
    public nint p_data;
    public int channel_stride_in_bytes;
}

/// <summary>Legacy receiver creation settings retained by the native compatibility API.</summary>
[StructLayout(LayoutKind.Sequential)]
public struct recv_create_t
{
    public source_t source_to_connect_to;
    public recv_color_format_e color_format;
    public recv_bandwidth_e bandwidth;

    [MarshalAs(UnmanagedType.I1)]
    public bool allow_video_fields;
}

/// <summary>Remote-recording timing information from the deprecated receiver recording API.</summary>
[StructLayout(LayoutKind.Sequential)]
public struct recv_recording_time_t
{
    public long no_frames;
    public long start_time;
    public long last_time;
}
