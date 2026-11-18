namespace Tractus.Ndi;

public enum recv_bandwidth_e
{
    // Receive metadata.
    recv_bandwidth_metadata_only = -10,

    // Receive metadata audio.
    recv_bandwidth_audio_only = 10,

    // Receive metadata audio video at a lower bandwidth and resolution.
    recv_bandwidth_lowest = 0,

    // Receive metadata audio video at full resolution.
    recv_bandwidth_highest = 100,

    recv_bandwidth_video_only_lowest = 1,
    recv_bandwidth_video_only_highest = 101,
    recv_bandwidth_max = 0x7fffffff,
}
