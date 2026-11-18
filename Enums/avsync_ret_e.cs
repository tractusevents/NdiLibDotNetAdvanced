namespace Tractus.Ndi;

public enum avsync_ret_e
{
    // We recovered the audio that you asked for, if you requested an exact number of samples it was
    // returned. If you do not specify the number of audio samples that you want then this is always returned.
    NDIlib_avsync_ret_success = 1,

    // We recovered the audio, however the number of samples you asked for needed to be different to avoid
    // dropping audio data. This is because the remote source is not clocking audio and video sufficiently
    // accurately on the same clock and a different number of audio samples was needed in order to keep it
    // exactly in sync.
    NDIlib_avsync_ret_success_num_samples_not_matched = 2,

    // No audio could be captured that matched this video frame. This is because there is currently no audio
    // from this source.
    NDIlib_avsync_ret_no_audio_stream_received = -1,

    // No audio could be capture that matched this video frame. Audio is currently on this source, however
    // none could be found that matched the video frame. This is likely because the sync, timestamps or
    // clocks on the remote source are so far away from the expectations. Or that the video has a timestamp
    // that is incorrect. This can also occur if the sender is putting audio and video into the stream in
    // such a way that they are out of sync.
    NDIlib_avsync_ret_no_samples_found = -2,

    // You specified an audio format, but it has changed. You specified an desired audio sample rate or no
    // audio channels however this is not what the audio settings currently are. NDIlib_audio_frame_v3_t has
    // been updated to correctly reflect the results but audio has not been filled in. Call again to get the
    // audio for this video frame.
    NDIlib_avsync_ret_format_changed = -3,

    // Some internal error occurred (e.g. p_avsync was NULL, p_audio_frame was NULL, etc...)
    NDIlib_avsync_ret_internal_error = -4
}
