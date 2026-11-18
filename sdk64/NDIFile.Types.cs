using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Tractus.Ndi.Sdk64;

[StructLayout(LayoutKind.Sequential)]
public struct NDIlib_file_video_track_format_t
{
    public byte is_high_quality;
    public int xres;
    public int yres;
    public int frame_rate_n;
    public int frame_rate_d;
    public uint fourcc;
    public NDIlib_file_video_frame_format_e frame_format_type;
    public float picture_aspect_ratio;

    public bool IsHighQuality
    {
        readonly get => this.is_high_quality != 0;
        set => this.is_high_quality = value ? (byte)1 : (byte)0;
    }
}

[InlineArray(2)]
public struct NDIlib_file_video_track_format_array_t
{
    private NDIlib_file_video_track_format_t element0;
}

[StructLayout(LayoutKind.Sequential)]
public struct NDIlib_file_source_format_t
{
    public uint video_track_count;
    public NDIlib_file_video_track_format_array_t video_tracks;
    public int audio_sample_rate;
    public int audio_no_channels;
    public int audio_no_samples;
    public uint audio_bit_depth;
    public uint audio_fourcc;
}

[StructLayout(LayoutKind.Sequential)]
public struct NDIlib_file_recorder_source_t
{
    public nint p_ndi_name;
    public nint p_url_address;
}

[StructLayout(LayoutKind.Sequential)]
public struct NDIlib_file_recorder_create_t
{
    public NDIlib_file_recorder_source_t source_to_connect_to;
    public NDIlib_file_recorder_track_flags_e track_flags;
    public byte record_float_audio_planar_32f;
    public byte disable_auto_rollover_on_format_change;
    public nint p_output_filename_pattern;
    public byte disable_proxy_track;
    public ulong rollover_max_file_size_bytes;

    public bool RecordFloatAudioPlanar32f
    {
        readonly get => this.record_float_audio_planar_32f != 0;
        set => this.record_float_audio_planar_32f = value ? (byte)1 : (byte)0;
    }

    public bool DisableAutoRolloverOnFormatChange
    {
        readonly get => this.disable_auto_rollover_on_format_change != 0;
        set => this.disable_auto_rollover_on_format_change = value ? (byte)1 : (byte)0;
    }

    public bool DisableProxyTrack
    {
        readonly get => this.disable_proxy_track != 0;
        set => this.disable_proxy_track = value ? (byte)1 : (byte)0;
    }
}

[StructLayout(LayoutKind.Sequential)]
public unsafe struct NDIlib_file_recorder_event_t
{
    public const int PathMaxSize = 4096;
    public const int MessageMaxSize = 1024;
    public const int SourceFormatMaxSize = 1024;

    public NDIlib_file_recorder_event_type_e type;
    public fixed byte p_path[PathMaxSize];
    public fixed byte p_message[MessageMaxSize];
    public NDIlib_file_recorder_track_flags_e track_flags;
    public ulong dropped_packet_count;
    public ulong first_pts_100ns;
    public ulong last_pts_100ns;
    public fixed byte source_format[SourceFormatMaxSize];
    public NDIlib_file_source_format_t source_format_payload;

    public string GetPath()
    {
        fixed (byte* pointer = this.p_path)
        {
            return UTF.Utf8ToString(new ReadOnlySpan<byte>(pointer, PathMaxSize));
        }
    }

    public string GetMessage()
    {
        fixed (byte* pointer = this.p_message)
        {
            return UTF.Utf8ToString(new ReadOnlySpan<byte>(pointer, MessageMaxSize));
        }
    }

    public string GetSourceFormat()
    {
        fixed (byte* pointer = this.source_format)
        {
            return UTF.Utf8ToString(new ReadOnlySpan<byte>(pointer, SourceFormatMaxSize));
        }
    }
}

[StructLayout(LayoutKind.Sequential)]
public struct NDIlib_file_recorder_stats_t
{
    public ulong bytes_written;
    public ulong video_frames;
    public ulong audio_frames;
    public ulong metadata_frames;
    public ulong receiver_dropped_video_frames;
    public ulong receiver_dropped_audio_frames;
    public ulong receiver_dropped_metadata_frames;
    public ulong pipeline_dropped_video_frames;
    public ulong pipeline_dropped_audio_frames;
    public ulong pipeline_dropped_metadata_frames;
    public ulong writer_dropped_frames;
    public ulong rollover_count;
    public ulong recording_time_100ns;
    public ulong last_video_pts_100ns;
    public ulong last_audio_pts_100ns;
    public ulong last_metadata_pts_100ns;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe struct NDIlib_file_player_event_t
{
    public const int PathMaxSize = 4096;
    public const int MessageMaxSize = 1024;

    public NDIlib_file_player_event_type_e type;
    public fixed byte p_path[PathMaxSize];
    public fixed byte p_message[MessageMaxSize];
    public ulong first_pts_100ns;
    public ulong last_pts_100ns;
    public byte has_source_timecode;
    public long source_timecode_100ns;
    public NDIlib_file_source_format_t source_format;

    public bool HasSourceTimecode => this.has_source_timecode != 0;

    public string GetPath()
    {
        fixed (byte* pointer = this.p_path)
        {
            return UTF.Utf8ToString(new ReadOnlySpan<byte>(pointer, PathMaxSize));
        }
    }

    public string GetMessage()
    {
        fixed (byte* pointer = this.p_message)
        {
            return UTF.Utf8ToString(new ReadOnlySpan<byte>(pointer, MessageMaxSize));
        }
    }
}

[StructLayout(LayoutKind.Sequential)]
public struct NDIlib_file_player_file_info_t
{
    public ulong duration_100ns;
    public byte duration_is_final;
    public uint track_count;
    public uint video_track_count;
    public uint audio_track_count;
    public uint metadata_track_count;
    public NDIlib_file_source_format_t source_format;

    public bool DurationIsFinal => this.duration_is_final != 0;
}

[InlineArray(2)]
public struct NDIlib_file_ulong_array_t
{
    private ulong element0;
}

[StructLayout(LayoutKind.Sequential)]
public struct NDIlib_file_player_stats_t
{
    public NDIlib_file_ulong_array_t video_frames_sent;
    public ulong audio_frames_sent;
    public ulong metadata_frames_sent;
    public ulong playback_wallclock_100ns;
}

[StructLayout(LayoutKind.Sequential)]
public struct NDIlib_file_player_create_t
{
    public nint p_ndi_send_name;
    public nint p_groups;
}

public readonly struct NDIlib_file_recorder_source_snapshot_t
{
    public string NdiName { get; }
    public string UrlAddress { get; }

    public NDIlib_file_recorder_source_snapshot_t(string ndiName, string urlAddress)
    {
        this.NdiName = ndiName;
        this.UrlAddress = urlAddress;
    }
}
