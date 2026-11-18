using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using Tractus.Ndi;
using Tractus.Ndi.Sdk64;

internal static class SdkContract
{
    public static void Verify(string file)
    {
        var assembly = typeof(NDIWrapper).Assembly;
        var enums = assembly.GetExportedTypes().Where(t => t.IsEnum).ToArray();
        var imports = assembly.GetExportedTypes()
            .SelectMany(t => t.GetMethods(BindingFlags.Public | BindingFlags.Static))
            .Select(m => m.GetCustomAttribute<DllImportAttribute>()?.EntryPoint)
            .OfType<string>().ToHashSet(StringComparer.Ordinal);
        var count = 0;
        foreach (var line in File.ReadLines(file))
        {
            var parts = line.Split('\t');
            if (parts.Length == 1)
            {
                if (!imports.Remove(line))
                {
                    throw new InvalidOperationException($"Missing SDK export {line}");
                }
                continue;
            }

            var nativeType = parts[0];
            var name = parts[1];
            var expected = uint.Parse(parts[2], CultureInfo.InvariantCulture);
            if (nativeType == "file_buffer_size")
            {
                var value = name switch
                {
                    "NDIlib_file_recorder_event_path_max_size" => NDIlib_file_recorder_event_t.PathMaxSize,
                    "NDIlib_file_recorder_event_message_max_size" => NDIlib_file_recorder_event_t.MessageMaxSize,
                    "NDIlib_file_recorder_event_source_format_max_size" => NDIlib_file_recorder_event_t.SourceFormatMaxSize,
                    "NDIlib_file_player_event_path_max_size" => NDIlib_file_player_event_t.PathMaxSize,
                    "NDIlib_file_player_event_message_max_size" => NDIlib_file_player_event_t.MessageMaxSize,
                    _ => throw new InvalidOperationException(name)
                };
                if (expected != value)
                {
                    throw new InvalidOperationException(name);
                }
            }
            else
            {
                var type = nativeType == "NDIlib_FourCC_video_type_e" ? typeof(FourCC_type_e)
                    : enums.Single(t => t.Name == nativeType || "NDIlib_" + t.Name == nativeType);
                CheckMember(type, nativeType, name, expected);
                if (nativeType.Contains("FourCC", StringComparison.Ordinal) || nativeType == "NDIlib_recv_color_format_ex_e")
                {
                    var constant = typeof(NDIConstants).GetField(name);
                    if (constant is null || Convert.ToUInt32(constant.GetRawConstantValue()) != expected)
                    {
                        throw new InvalidOperationException($"NDIConstants.{name} must have bit pattern 0x{expected:X8}");
                    }
                }
                if (nativeType == "NDIlib_FourCC_video_type_e")
                {
                    CheckMember(typeof(FourCC_video_type_ex_e), nativeType, name, expected);
                }
                if (nativeType == "NDIlib_recv_color_format_e")
                {
                    CheckMember(typeof(recv_color_format_ex_e), nativeType, name, expected);
                }
            }
            count++;
        }

        if (imports.Count != 0)
        {
            throw new InvalidOperationException($"Imports outside SDK contract: {string.Join(", ", imports)}");
        }
        Console.WriteLine($"Verified all SDK entry points and {count} enum members and buffer constants.");
    }

    private static void CheckMember(Type type, string nativeType, string name, uint expected)
    {
        var prefix = nativeType[..^1]; // Drop the trailing e from the native type name.
        var field = type.GetField(name) ?? type.GetField(name["NDIlib_".Length..]);
        if (field is null && name.StartsWith(prefix, StringComparison.Ordinal))
        {
            field = type.GetField(name[prefix.Length..]);
        }
        if (field is null || unchecked((uint)Convert.ToInt64(field.GetRawConstantValue())) != expected)
        {
            throw new InvalidOperationException($"{type.Name}.{name} must have bit pattern 0x{expected:X8}");
        }
    }
}
