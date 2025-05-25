
using NewTek;
using System;
using System.Buffers.Binary;
using System.Buffers;
using System.Formats.Asn1;
using System.Reflection;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Security;
using static NewTek.NDIlib;
using NewTek.NDI;

namespace Tractus.Ndi;

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public delegate bool CustomVideoAllocatorCallback(nint pOpaque, ref NDIlib.video_frame_v2_t videoData);

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public delegate bool CustomVideoFreeCallback(nint pOpaque, ref NDIlib.video_frame_v2_t videoData);

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public delegate bool CustomAudioAllocatorCallback(nint pOpaque, ref NDIlib.audio_frame_v3_t audioData);

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public delegate bool CustomAudioFreeCallback(nint pOpaque, ref NDIlib.audio_frame_v3_t audioData);


[StructLayout(LayoutKind.Sequential)]
public struct NDIlib_source_v2_t
{
    [MarshalAs(UnmanagedType.LPUTF8Str)]
    public string p_ndi_name;

    [MarshalAs(UnmanagedType.LPUTF8Str)]
    public string p_url_address;

    [MarshalAs(UnmanagedType.LPUTF8Str)]
    public string p_metadata;
}

[SuppressUnmanagedCodeSecurity]
public static unsafe partial class NdiAdvanced
{
    // Based on how SDLSharp does things.
    // https://github.com/GabrielFrigo4/SDL-Sharp/blob/3daad4b05c11c1a3987ae24c12c78092be3aa9c3/SDL-Sharp/SDL/SDL.Loader.cs#L11

    private const string LibraryName = "NdiAdv";


    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_set_video_allocator", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void recv_set_video_allocator(
        nint pinstance, 
        nint pOpaque,
        CustomVideoAllocatorCallback allocator,
        CustomVideoFreeCallback freer);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_set_audio_allocator", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void recv_set_audio_allocator(
        nint pinstance,
        nint pOpaque,
        CustomAudioAllocatorCallback allocator,
        CustomAudioFreeCallback freer);


    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_free_video_v2", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void recv_free_video_v2(IntPtr p_instance, ref video_frame_v2_t p_video_data);
    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_free_audio_v3", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void recv_free_audio_v3(IntPtr p_instance, ref audio_frame_v3_t p_audio_data);
    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_free_metadata", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void recv_free_metadata(IntPtr p_instance, ref metadata_frame_t p_metadata);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_free_video_v2", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static unsafe extern void recv_free_video_v2(IntPtr p_instance, video_frame_v2_t* p_video_data);
    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_free_audio_v3", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static unsafe extern void recv_free_audio_v3(IntPtr p_instance, audio_frame_v3_t* p_audio_data);
    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_free_metadata", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static unsafe extern void recv_free_metadata(IntPtr p_instance, metadata_frame_t* p_metadata);




    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_free_string", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void recv_free_string(IntPtr p_instance, IntPtr p_string);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_ptz_is_supported", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern bool recv_ptz_is_supported(IntPtr p_instance);

    //NDIlib_send_add_connection_metadata
    [DllImport(LibraryName, EntryPoint = "NDIlib_send_add_connection_metadata", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void send_add_connection_metadata(
        nint pInstance,
        ref metadata_frame_t metadataFrame);

    //NDIlib_send_add_connection_metadata
    [DllImport(LibraryName, EntryPoint = "NDIlib_send_clear_connection_metadata", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void send_clear_connection_metadata(
        nint pInstance);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_capture_v3", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern frame_type_e recv_capture_v3(
        IntPtr p_instance, 
        ref video_frame_v2_t p_video_data, 
        ref audio_frame_v3_t p_audio_data, 
        ref metadata_frame_t p_metadata, 
        UInt32 timeout_in_ms);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_capture_v3", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static unsafe extern frame_type_e recv_capture_v3(
        IntPtr p_instance,
        video_frame_v2_t* p_video_data,
        audio_frame_v3_t* p_audio_data,
        metadata_frame_t* p_metadata,
        uint timeout_in_ms);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_capture_v3", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static unsafe extern frame_type_e recv_capture_v3(
        nint p_instance,
        out nint p_video_data,
        out nint p_audio_data,
        out nint p_metadata,
        uint timeout_in_ms);

    [DllImport(LibraryName, EntryPoint = "NDIlib_send_capture", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern frame_type_e send_capture(
        nint pInstance,
        ref metadata_frame_t pMetadata,
        uint timeoutInMs);

    [DllImport(LibraryName, EntryPoint = "NDIlib_send_free_metadata", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void send_free_metadata(
        nint pInstance,
        ref metadata_frame_t pMetadata);

    // recv_capture_v3 
    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_capture_v3", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public unsafe static extern frame_type_e recv_capture_v3(
        IntPtr p_instance,
        video_frame_v2_t* p_video_data,
        audio_frame_v3_t* p_audio_data,
        metadata_frame_t* p_metadata,
        int timeout_in_ms);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_send_metadata", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern bool recv_send_metadata(
        nint pInstance,
        ref NDIlib.metadata_frame_t metadataFrame);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_destroy", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void recv_destroy(IntPtr p_instance);


    // framesync_create_v2 
    [DllImport(LibraryName, EntryPoint = "NDIlib_framesync_create", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr framesync_create(IntPtr p_receiver);

    // framesync_destroy 
    [DllImport(LibraryName, EntryPoint = "NDIlib_framesync_destroy", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void framesync_destroy(IntPtr p_instance);

    // framesync_capture_audio 
    [DllImport(LibraryName, EntryPoint = "NDIlib_framesync_capture_audio", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr framesync_capture_audio(IntPtr p_instance, ref audio_frame_v2_t p_audio_data, int sample_rate, int no_channels, int no_samples);

    // framesync_capture_audio_v2
    [DllImport(LibraryName, EntryPoint = "NDIlib_framesync_capture_audio_v2", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr framesync_capture_audio_v2(IntPtr p_instance, ref audio_frame_v3_t p_audio_data, int sample_rate, int no_channels, int no_samples);

    // framesync_free_audio 
    [DllImport(LibraryName, EntryPoint = "NDIlib_framesync_free_audio", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void framesync_free_audio(IntPtr p_instance, ref audio_frame_v2_t p_audio_data);

    // framesync_free_audio_v2
    [DllImport(LibraryName, EntryPoint = "NDIlib_framesync_free_audio_v2", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void framesync_free_audio_v2(IntPtr p_instance, ref audio_frame_v3_t p_audio_data);

    // framesync_audio_queue_depth 
    [DllImport(LibraryName, EntryPoint = "NDIlib_framesync_audio_queue_depth", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern int framesync_audio_queue_depth(IntPtr p_instance);

    // framesync_capture_video 
    [DllImport(LibraryName, EntryPoint = "NDIlib_framesync_capture_video", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void framesync_capture_video(IntPtr p_instance, ref video_frame_v2_t p_video_data, frame_format_type_e field_type);

    // framesync_free_video 
    [DllImport(LibraryName, EntryPoint = "NDIlib_framesync_free_video", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void framesync_free_video(IntPtr p_instance, ref video_frame_v2_t p_video_data);


    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_kvm_is_supported", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern bool recv_kvm_is_supported(nint pInstance);





    [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "NDIlib_send_create", ExactSpelling = true)]
    public static extern nint send_create(ref send_create_t p_create_settings);


    [DllImport(LibraryName, EntryPoint = "NDIlib_send_create_v2", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr send_create_v2(
        ref NDIlib.send_create_t create, 
        [MarshalAs(UnmanagedType.LPStr)] string p_config_data);

    [DllImport(LibraryName, EntryPoint = "NDIlib_send_create_v2", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr send_create_v2(
        ref NDIlib.send_create_t create,
        nint p_config_data);

    [DllImport(LibraryName, EntryPoint = "NDIlib_send_destroy", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void send_destroy(IntPtr p_instance);

    // send_send_video_v2 
    [DllImport(LibraryName, EntryPoint = "NDIlib_send_send_video_v2", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void send_send_video_v2(
        nint p_instance, 
        ref video_frame_v2_t p_video_data);

    // send_send_video_v2 
    [DllImport(LibraryName, EntryPoint = "NDIlib_send_send_video_v2", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static unsafe extern void send_send_video_v2(
        nint p_instance,
        video_frame_v2_t* p_video_data);


    [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "NDIlib_send_send_video_async_v2", ExactSpelling = true)]
    public static extern void send_send_video_async_v2(nint p_instance, ref video_frame_v2_t p_video_data);

    [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "NDIlib_send_send_video_async_v2", ExactSpelling = true)]
    public static extern unsafe void send_send_video_async_v2(nint p_instance, video_frame_v2_t* p_video_data);


    // void NDIlib_send_send_video_async_v2(NDIlib_send_instance_t p_instance, const NDIlib_video_frame_v2_t* p_video_data);
    [DllImport(LibraryName, EntryPoint = "NDIlib_send_send_video_async_v2", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void send_send_video_async_v2_adv(nint pInstance, ref video_frame_v2_t pVideoData);


    [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "NDIlib_send_send_audio_v3", ExactSpelling = true)]
    public static extern void send_send_audio_v3(nint p_instance, ref audio_frame_v3_t p_audio_data);


    [DllImport(LibraryName, EntryPoint = "NDIlib_find_create_v3", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern nint find_create_v3(
        ref NDIlib.find_create_t createSettings,
        nint configData);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_create_v4", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern nint recv_create_v4(
        ref NDIlib.recv_create_v3_t createSettings,
        nint configData);


    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_set_bandwidth", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern bool recv_set_bandwidth(
        nint instance,
        NDIlib.recv_bandwidth_e bandwidth);

    [DllImport(LibraryName, EntryPoint = "NDIlib_find_get_current_sources_v2", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern nint find_get_current_sources_v2(
        nint p_instance,
        out uint p_no_sources);

    [DllImport(LibraryName, EntryPoint = "NDIlib_find_destroy", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern void find_destroy(
        nint p_instance);

    [DllImport(LibraryName, EntryPoint = "NDIlib_recv_request_keyframe", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    public static extern bool recv_request_keyframe(nint recvPtr);

    [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "NDIlib_recv_get_no_connections", ExactSpelling = true)]
    public static extern int recv_get_no_connections(nint p_instance);

    [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "NDIlib_recv_get_web_control", ExactSpelling = true)]
    public static extern nint recv_get_web_control(nint p_instance);

    [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "NDIlib_send_send_metadata", ExactSpelling = true)]
    public static extern void send_send_metadata(nint p_instance, ref metadata_frame_t p_metadata);

    
    [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "NDIlib_recv_ptz_store_preset", ExactSpelling = true)]
    public static extern bool recv_ptz_store_preset(nint p_instance, int preset_no);

    [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "NDIlib_recv_ptz_recall_preset", ExactSpelling = true)]
    [return: MarshalAs(UnmanagedType.U1)]
    public static extern bool recv_ptz_recall_preset(nint p_instance, int preset_no, float speed);


    /// <summary>
    /// Sends a metadata frame to any subscribed listeners.
    /// </summary>
    /// <param name="senderPtr">The sender pointer.</param>
    /// <param name="metadata">The metadata XML to be sent. Should be null-terminated.</param>
    public static unsafe void SendMetadataFrame(nint senderPtr, string metadata)
    {
        var metadataXml = metadata;
        if (metadataXml[metadataXml.Length -1] != '\0')
        {
            metadataXml += "\0";
        }

        var metaFrame = new metadata_frame_t()
        {
            p_data = UTF.StringToUtf8(metadataXml),
            timecode = NDIlib.send_timecode_synthesize,
        };

        send_send_metadata(senderPtr, ref metaFrame);

        send_free_metadata(senderPtr, ref metaFrame);
    }

    public static unsafe void AddMetadata(nint senderPtr, string[] metadata, bool replaceAll = false)
    {
        if (replaceAll)
        {
            send_clear_connection_metadata(senderPtr);
        }

        foreach(var item in metadata)
        {
            var metadataXml = item;
            if (metadataXml[metadataXml.Length - 1] != '\0')
            {
                metadataXml = metadataXml + "\0";
            }

            var metadataPtr = UTF.StringToUtf8(metadataXml);

            var metaFrame = new metadata_frame_t()
            {
                p_data = metadataPtr,
            };

            send_add_connection_metadata(senderPtr, ref metaFrame);

            Marshal.FreeHGlobal(metadataPtr);
        }
    }


    public static NDIlib_source_v2_t[] find_get_current_sources_v2(
        nint pInstance)
    {
        var sourcesPtr = find_get_current_sources_v2(pInstance, out var numSources);
        if(sourcesPtr == nint.Zero)
        {
            return [];
        }

        var sources = new NDIlib_source_v2_t[numSources];

        var structSize = Marshal.SizeOf<NDIlib_source_v2_t>();
        for(var i = 0; i < numSources; i++)
        {
            var currentPtr = nint.Add(sourcesPtr, i * structSize);
            sources[i] = Marshal.PtrToStructure<NDIlib_source_v2_t>(currentPtr);
        }

        return sources;
    }

    static NdiAdvanced()
	{
		NativeLibrary.SetDllImportResolver(typeof(NdiAdvanced).Assembly, ResolveDllImport);
	}

    public static string GetRuntimeIdentifier() 
    {
        var arch = "unknown";
        switch(RuntimeInformation.ProcessArchitecture)
        {
            case Architecture.X86: 
                arch = "x86";
                break;
            case Architecture.X64:
                arch = "x86-64";
                break;
            case Architecture.Arm:
                arch = "arm32";
                break;
            case Architecture.Arm64:
                arch = "arm64";
                break;
            case Architecture.Wasm:
                arch = "wasm";
                break;
            default:
                arch = RuntimeInformation.ProcessArchitecture.ToString();
                break;
        }

        var platform = "unknown";
        if(RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            platform = "win";
        }
        else if(RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            platform = "osx";
        }
        else if(RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            platform = "linux";
        }
        else if(RuntimeInformation.IsOSPlatform(OSPlatform.FreeBSD))
        {
            platform = "freebsd";
        }

        return $"{platform}-{arch}";
    }

	private static nint ResolveDllImport(string libraryName, Assembly assembly, DllImportSearchPath? searchPath)
	{
		var libName = string.Empty;
        var useAlternateLoadLogic = false;

		if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
		{
			if (RuntimeInformation.ProcessArchitecture == Architecture.X64)
			{
				libName = "Processing.NDI.Lib.Advanced.x64.dll";
			}
			else
			{
				throw new NotImplementedException("Non-x86-based arch not supported on Windows.");
			}
		}
		else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
		{
            useAlternateLoadLogic = true;
			libName = "libndi_adv.so";
		}
		else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
		{
			libName = "libndi_advanced.dylib";
		}
		else
		{
			throw new NotImplementedException($"{RuntimeInformation.OSDescription} not supported.");
		}

        var handle = nint.Zero;

        if(useAlternateLoadLogic)
        {
            var libPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                libName
            );

            NativeLibrary.TryLoad(libPath, out handle);
        }
        else
        {
    		NativeLibrary.TryLoad(libName, out handle);
        }


		return handle;
    }
}

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

    public const uint NDIlib_recv_color_format_ex_max = 0x7fffffff;
}
public struct NdiKvmMouseClickEvent
{
    public MouseButton Button;
    public bool Clicked;
}
public enum MouseButton
{
    Left,
    Middle,
    Right
}

public struct NdiKvmMouseMoveEvent
{
    public float X;
    public float Y;

    public readonly void ToScreenCoords(int width, int height, out int x, out int y)
    {
        x = (int)(width * this.X);
        y = (int)(height * this.Y);
    }
}

public class KvmEventArgs : EventArgs
{
    public NdiKvmMouseClickEvent? MouseClick { get; }
    public NdiKvmMouseMoveEvent? MouseMove { get; }
    public byte[] Data { get; }
    public byte OpCode => this.Data[0];
    public NdiKvmKeyboardEvent? KeyEvent { get; }

    public KvmEventArgs(NdiKvmKeyboardEvent keyEvent, byte[] data)
    {
        this.KeyEvent = keyEvent;
        this.Data = data;
    }

    public KvmEventArgs(NdiKvmMouseClickEvent mouseClick, byte[] data)
    {
        this.MouseClick = mouseClick;
        this.Data = data;
    }

    public KvmEventArgs(NdiKvmMouseMoveEvent? mouseMove, byte[] data)
    {
        this.MouseMove = mouseMove;
        this.Data = data;
    }

    public KvmEventArgs(byte[] data)
    {
        this.Data = data;
    }
}

public enum NdiKvmEventType
{
    MouseDown,
    MouseUp,
    MouseMove,
    MouseWheel,
    KeyDown,
    KeyUp,
    Clipboard,
    Unknown
}

public class NdiKvmParser
{
    private const string Prefix = "<ndi_kvm u=\"";
    private const string Suffix = "\"/>";

    public static void Dump(string base64, string outputPath = null)
    {
        byte[] data;
        try
        {
            data = Convert.FromBase64String(base64);
        }
        catch (FormatException ex)
        {
            Console.WriteLine($"Invalid Base64: {ex.Message}");
            return;
        }

        Console.WriteLine($"Decoded {data.Length} bytes:");
        for (int i = 0; i < data.Length; i++)
        {
            byte b = data[i];
            // hex
            Console.Write(b.ToString("X2"));
            Console.Write(" ");
            // binary
            Console.WriteLine(Convert.ToString(b, 2).PadLeft(8, '0'));
        }

        if (!string.IsNullOrEmpty(outputPath))
        {
            try
            {
                File.WriteAllBytes(outputPath, data);
                Console.WriteLine($"Wrote {data.Length} bytes to {outputPath}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Failed to write file: {e.Message}");
            }
        }
    }

    public static bool TryParse(
        string xml,
        out KvmEventArgs? args,
        out NdiKvmEventType eventType)
    {
        args = null;
        eventType = NdiKvmEventType.Unknown;

        // Quick check and spanify
        if (!xml.StartsWith(Prefix, StringComparison.Ordinal))
            return false;

        var span = xml.AsSpan();
        var start = Prefix.Length;
        var end = span.LastIndexOf(Suffix, StringComparison.Ordinal);
        if (end <= start)
            return false;

        var payloadSpan = span.Slice(start, end - start);

        // Decode Base64 directly into a byte buffer
        var maxLen = payloadSpan.Length * 3 / 4;
        var buffer = ArrayPool<byte>.Shared.Rent(maxLen);
        try
        {
            if (!Convert.TryFromBase64Chars(
                    payloadSpan,
                    buffer,
                    out var bytesWritten))
                return false;

            ReadOnlySpan<byte> data = buffer.AsSpan(0, bytesWritten);
            var opcode = data[0];

            switch (opcode)
            {
                case 0x0C:
                    {
                        // Keyboard
                        var keycode = data[1];
                        var shift = keycode is 0xE1 or 0xE2;
                        var ctrl = keycode is 0xE3 or 0xE4;
                        var down = data.Length > 5 && data[5] == 1;

                        eventType = down
                            ? NdiKvmEventType.KeyDown
                            : NdiKvmEventType.KeyUp;
                        args = new KvmEventArgs(
                            new NdiKvmKeyboardEvent
                            {
                                Keycode = keycode,
                                ShiftKey = shift,
                                CtrlKey = ctrl
                            },
                            data.ToArray());
                        return true;
                    }

                case 0x03:
                    {
                        // Mouse move
                        eventType = NdiKvmEventType.MouseMove;
                        var x = BinaryPrimitives.ReadSingleLittleEndian(data.Slice(1));
                        var y = BinaryPrimitives.ReadSingleLittleEndian(data.Slice(5));
                        args = new KvmEventArgs(
                            new NdiKvmMouseMoveEvent { X = x, Y = y },
                            data.ToArray());
                        return true;
                    }

                // Mouse down (left=0x04 / right=0x06)
                case 0x04:
                case 0x06:
                    {
                        eventType = NdiKvmEventType.MouseDown;
                        var button = opcode == 0x04
                            ? MouseButton.Left
                            : MouseButton.Right;
                        args = new KvmEventArgs(
                            new NdiKvmMouseClickEvent { Button = button, Clicked = true },
                            data.ToArray());
                        return true;
                    }

                // Mouse up (left=0x07 / right=0x09)
                case 0x07:
                case 0x09:
                    {
                        eventType = NdiKvmEventType.MouseUp;
                        var button = opcode == 0x07
                            ? MouseButton.Left
                            : MouseButton.Right;
                        args = new KvmEventArgs(
                            new NdiKvmMouseClickEvent { Button = button, Clicked = false },
                            data.ToArray());
                        return true;
                    }

                default:
                    {
                        eventType = NdiKvmEventType.Unknown;
                        args = new KvmEventArgs(data.ToArray());
                        return true;
                    }
            }
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buffer);
        }
    }


    public static KvmEventArgs? GetEventArgsFromMetadataXml(string metadataXml, out NdiKvmEventType eventType)
    {
        if (!metadataXml.StartsWith("<ndi_kvm u="))
        {
            eventType = NdiKvmEventType.Unknown;
            return null;
        }

        // TODO: A better way to do this.
        var payload = metadataXml.Replace("<ndi_kvm u=\"", "").Replace("\"/>", "");

        try
        {
            var binary = Convert.FromBase64String(payload);
            Dump(payload);
            var opcode = binary[0];
            switch (opcode)
            {
                case 0x0C:
                    // Keyboard event of some sort.
                    var keycode = binary[1];
                    var modifierKey = binary[2] == 0xFF;

                    var shiftKey = binary[1] == 0xE1 || binary[1] == 0xE2;
                    var ctrlKey = binary[1] == 0xE3 || binary[1] == 0xE4;

                    var keydown = binary[5] == 0x1;

                    eventType = keydown ? NdiKvmEventType.KeyDown : NdiKvmEventType.KeyUp;
                    return new KvmEventArgs(new NdiKvmKeyboardEvent()
                    {
                        Keycode = keycode,
                        ShiftKey = shiftKey,
                        CtrlKey = ctrlKey,
                    }, binary);
                case 0x03:
                    // Mouse move
                    eventType = NdiKvmEventType.MouseMove;
                    var x = BitConverter.ToSingle(binary, 1);
                    var y = BitConverter.ToSingle(binary, 5);
                    return new KvmEventArgs(new NdiKvmMouseMoveEvent()
                    {
                        X = x,
                        Y = y
                    }, binary);
                case 0x04:
                    // Left MB down
                    eventType = NdiKvmEventType.MouseDown;
                    return new KvmEventArgs(new NdiKvmMouseClickEvent
                    {
                        Button = MouseButton.Left,
                        Clicked = true
                    }, binary);
                case 0x07:
                    // Left MB up
                    eventType = NdiKvmEventType.MouseUp;
                    return new KvmEventArgs(new NdiKvmMouseClickEvent
                    {
                        Button = MouseButton.Left,
                        Clicked = false
                    }, binary);
                case 0x06:
                    // Right MB down
                    eventType = NdiKvmEventType.MouseDown;
                    return new KvmEventArgs(new NdiKvmMouseClickEvent
                    {
                        Button = MouseButton.Right,
                        Clicked = true
                    }, binary);
                case 0x09:
                    // Right MB up
                    eventType = NdiKvmEventType.MouseUp;
                    return new KvmEventArgs(new NdiKvmMouseClickEvent
                    {
                        Button = MouseButton.Right,
                        Clicked = false
                    }, binary);
                default:
                    eventType = NdiKvmEventType.Unknown;
                    return new KvmEventArgs(binary);
            }

        }
        catch (Exception)
        {
            eventType = NdiKvmEventType.Unknown;
            return null;
        }
    }
}

