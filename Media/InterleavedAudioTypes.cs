using System.Runtime.InteropServices;

namespace Tractus.Ndi;

//public delegate void NewNdiSourceDiscovered(Source source);

//public delegate void NdiSourceListChanged(Source[] added, Source[] removed);

// This describes an audio frame
[StructLayoutAttribute(LayoutKind.Sequential)]
public struct audio_frame_interleaved_16s_t
{
	// The sample-rate of this buffer
	public int	sample_rate;

	// The number of audio channels
	public int	no_channels;

	// The number of audio samples per channel
	public int	no_samples;

	// The timecode of this frame in 100ns intervals
	public Int64	timecode;

	// The audio reference level in dB. This specifies how many dB above the reference level (+4dBU) is the full range of 16 bit audio.
	// If you do not understand this and want to just use numbers :
	//		-	If you are sending audio, specify +0dB. Most common applications produce audio at reference level.
	//		-	If receiving audio, specify +20dB. This means that the full 16 bit range corresponds to professional level audio with 20dB of headroom. Note that
	//			if you are writing it into a file it might sound soft because you have 20dB of headroom before clipping.
	public int	reference_level;

	// The audio data, interleaved 16bpp
	public nint	p_data;
}

// This describes an audio frame
[StructLayoutAttribute(LayoutKind.Sequential)]
public struct audio_frame_interleaved_32f_t
{
	// The sample-rate of this buffer
	public int	sample_rate;

	// The number of audio channels
	public int	no_channels;

	// The number of audio samples per channel
	public int	no_samples;

	// The timecode of this frame in 100ns intervals
	public Int64	timecode;

	// The audio data, interleaved 32bpp
	public nint	p_data;
}

// This describes an audio frame
[StructLayoutAttribute(LayoutKind.Sequential)]
public struct audio_frame_interleaved_32s_t
{
	// The sample-rate of this buffer
	public int sample_rate;

	// The number of audio channels
	public int no_channels;

	// The number of audio samples per channel
	public int no_samples;

	// The timecode of this frame in 100ns intervals
	public Int64 timecode;

	public int reference_level;

	// The audio data, interleaved 32bpp (Int32)
	public nint p_data;
}
