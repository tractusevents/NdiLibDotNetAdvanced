using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using Tractus.Ndi;
using Tractus.Ndi.Sdk64;

if (args is ["--finder", var fixture])
{
    FinderChecks.Run(Path.GetFullPath(fixture));
    return;
}

SdkContract.Verify(Path.Combine(AppContext.BaseDirectory, "sdk-6.4.tsv"));

if (IntPtr.Size != 8)
{
    Console.WriteLine("ABI layout assertions currently target 64-bit SDK builds.");
    return;
}

AssertLayout<source_t>(16);
AssertLayout<send_create_t>(24);
AssertLayout<find_create_t>(24);
AssertLayout<recv_create_v3_t>(40);
AssertLayout<video_frame_v2_t>(72);
AssertLayout<audio_frame_v2_t>(56);
AssertLayout<audio_frame_v3_t>(64);
AssertLayout<video_frame_t>(56);
AssertLayout<audio_frame_t>(40);
AssertLayout<recv_create_t>(32);
AssertLayout<recv_recording_time_t>(24);
AssertLayout<metadata_frame_t>(24);
AssertLayout<tally_t>(2);
AssertLayout<compressed_packet_t>(44);
AssertLayout<audio_frame_interleaved_32s_t>(40,
    (nameof(audio_frame_interleaved_32s_t.reference_level), 24),
    (nameof(audio_frame_interleaved_32s_t.p_data), 32));
AssertLayout<frame_scatter_t>(16);
AssertLayout<routing_create_t>(16);
AssertLayout<NDIlib_recv_listener_create_t>(8);

AssertLayout<NDIlib_receiver_t>(72,
    (nameof(NDIlib_receiver_t.p_address), 32),
    (nameof(NDIlib_receiver_t.p_streams), 40),
    (nameof(NDIlib_receiver_t.p_commands), 56),
    (nameof(NDIlib_receiver_t.events_subscribed), 68));

AssertLayout<sender_t>(56,
    (nameof(sender_t.p_address), 24),
    (nameof(sender_t.p_groups), 40),
    (nameof(sender_t.events_subscribed), 52));

AssertLayout<NDIlib_file_video_track_format_t>(32);
AssertLayout<NDIlib_file_source_format_t>(88);
AssertLayout<NDIlib_file_recorder_source_t>(16);
AssertLayout<NDIlib_file_recorder_create_t>(48,
    (nameof(NDIlib_file_recorder_create_t.p_output_filename_pattern), 24),
    (nameof(NDIlib_file_recorder_create_t.rollover_max_file_size_bytes), 40));
AssertLayout<NDIlib_file_recorder_event_t>(6264,
    (nameof(NDIlib_file_recorder_event_t.source_format_payload), 6176));
AssertLayout<NDIlib_file_recorder_stats_t>(128);
AssertLayout<NDIlib_file_player_event_t>(5248,
    (nameof(NDIlib_file_player_event_t.source_format), 5160));
AssertLayout<NDIlib_file_player_file_info_t>(120,
    (nameof(NDIlib_file_player_file_info_t.source_format), 28));
AssertLayout<NDIlib_file_player_stats_t>(40);
AssertLayout<NDIlib_file_player_create_t>(16);

// The public receiver descriptor preserves its marshalled string fields for existing callers.
AssertBlittable<sender_t>();
AssertBlittable<NDIlib_file_recorder_event_t>();
AssertBlittable<NDIlib_file_player_event_t>();

AssertEqual(1, (int)recv_bandwidth_e.recv_bandwidth_video_only_lowest, "video-only low bandwidth");
AssertEqual(101, (int)recv_bandwidth_e.recv_bandwidth_video_only_highest, "video-only high bandwidth");

AssertReturnType(nameof(NDIWrapper.recv_advertiser_destroy), typeof(void));
AssertReturnType(nameof(NDIWrapper.recv_listener_wait_for_receivers), typeof(bool));
AssertReturnType(nameof(NDIWrapper.recv_listener_free_events), typeof(void));
AssertReturnType(nameof(NDIWrapper.recv_request_keyframe), typeof(void));
AssertReturnType(nameof(NDIWrapper.framesync_capture_audio), typeof(void));
AssertReturnType(nameof(NDIWrapper.framesync_capture_audio_v2), typeof(void));

AssertNativeBools(typeof(NDIWrapper).Assembly);
AssertNativeMethodsArePublic(typeof(NDIWrapper).Assembly);
AssertUtf8RoundTrip("München 東京 🎥");
AssertUtf8RoundTrip("already terminated\0");

var rejectedEmbeddedNull = false;
try
{
    _ = UTF.StringToUtf8("not\0representable");
}
catch (ArgumentException)
{
    rejectedEmbeddedNull = true;
}

AssertEqual(true, rejectedEmbeddedNull, "embedded-null rejection");

var advancedLibrary = Environment.GetEnvironmentVariable("NDI64_TEST_ADVANCED_LIBRARY");
var fileLibrary = Environment.GetEnvironmentVariable("NDI64_TEST_FILE_LIBRARY");
if (!string.IsNullOrWhiteSpace(advancedLibrary) && !string.IsNullOrWhiteSpace(fileLibrary))
{
    NDIWrapper.Initialize(true, advancedLibrary, fileLibrary);
    AssertEqual(true, NDIWrapper.is_supported_CPU(), "NDI CPU support");
    AssertEqual(true, NDIWrapper.initialize(), "NDI initialization");
    try
    {
        if (string.IsNullOrWhiteSpace(NDIWrapper.version()))
        {
            throw new InvalidOperationException("The native NDI version string was empty.");
        }

        var missingPath = Path.Combine(Path.GetTempPath(), $"ndi64-missing-{Guid.NewGuid():N}.mkv");
        AssertEqual(false, NDIFile.file_player_probe_file(missingPath, out _), "missing-file probe");
    }
    finally
    {
        NDIWrapper.destroy();
    }
}

Console.WriteLine("ABI and UTF-8 checks passed.");

static void AssertLayout<T>(int expectedSize, params (string Field, int Offset)[] fields)
    where T : struct
{
    AssertEqual(expectedSize, Marshal.SizeOf<T>(), $"{typeof(T).Name} marshal size");
    AssertEqual(expectedSize, Unsafe.SizeOf<T>(), $"{typeof(T).Name} managed size");
    foreach (var (field, expectedOffset) in fields)
    {
        AssertEqual(expectedOffset, Marshal.OffsetOf<T>(field).ToInt32(), $"{typeof(T).Name}.{field} offset");
    }
}

static void AssertBlittable<T>()
{
    AssertEqual(false, RuntimeHelpers.IsReferenceOrContainsReferences<T>(), $"{typeof(T).Name} contains references");
}

static void AssertReturnType(string methodName, Type expected)
{
    var methods = typeof(NDIWrapper)
        .GetMethods(BindingFlags.Public | BindingFlags.Static)
        .Where(method => method.Name == methodName)
        .ToArray();

    if (methods.Length == 0)
    {
        throw new InvalidOperationException($"Method {methodName} was not found.");
    }

    foreach (var method in methods)
    {
        AssertEqual(expected, method.ReturnType, $"{methodName} return type");
    }
}

static void AssertNativeBools(Assembly assembly)
{
    var nativeMethods = assembly
        .GetTypes()
        .SelectMany(type => type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static))
        .Where(method => method.GetCustomAttribute<DllImportAttribute>() is not null);

    foreach (var method in nativeMethods)
    {
        if (method.ReturnType == typeof(bool))
        {
            AssertOneByteMarshal(method.ReturnParameter, $"{method.DeclaringType?.Name}.{method.Name} return");
        }

        foreach (var parameter in method.GetParameters().Where(parameter => parameter.ParameterType == typeof(bool)))
        {
            AssertOneByteMarshal(parameter, $"{method.DeclaringType?.Name}.{method.Name}({parameter.Name})");
        }
    }
}

static void AssertNativeMethodsArePublic(Assembly assembly)
{
    var hiddenNativeMethods = assembly
        .GetTypes()
        .SelectMany(type => type.GetMethods(BindingFlags.NonPublic | BindingFlags.Static))
        .Where(method => method.GetCustomAttribute<DllImportAttribute>() is not null)
        .Select(method => $"{method.DeclaringType?.FullName}.{method.Name}")
        .OrderBy(name => name)
        .ToArray();

    if (hiddenNativeMethods.Length != 0)
    {
        throw new InvalidOperationException(
            $"Native header methods must be public. Hidden bindings: {string.Join(", ", hiddenNativeMethods)}");
    }
}

static void AssertOneByteMarshal(ICustomAttributeProvider provider, string name)
{
    var marshal = provider
        .GetCustomAttributes(typeof(MarshalAsAttribute), inherit: false)
        .Cast<MarshalAsAttribute>()
        .SingleOrDefault();

    if (marshal is null || marshal.Value is not (UnmanagedType.I1 or UnmanagedType.U1))
    {
        throw new InvalidOperationException($"{name} is not explicitly marshalled as a one-byte C bool.");
    }
}

static void AssertUtf8RoundTrip(string value)
{
    var logicalValue = value.EndsWith('\0') ? value[..^1] : value;
    var pointer = UTF.StringToUtf8(value, out var byteLength);
    try
    {
        AssertEqual(Encoding.UTF8.GetByteCount(logicalValue), byteLength, "UTF-8 byte length");
        AssertEqual((byte)0, Marshal.ReadByte(pointer, byteLength), "UTF-8 terminator");
        AssertEqual(logicalValue, UTF.Utf8ToString(pointer, (uint)(byteLength + 1)), "UTF-8 round trip");
    }
    finally
    {
        Marshal.FreeHGlobal(pointer);
    }
}

static void AssertEqual<T>(T expected, T actual, string name)
{
    if (!EqualityComparer<T>.Default.Equals(expected, actual))
    {
        throw new InvalidOperationException($"{name}: expected {expected}, got {actual}.");
    }
}
