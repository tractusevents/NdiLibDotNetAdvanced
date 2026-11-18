using System.Reflection;
using System.Runtime.InteropServices;

namespace Tractus.Ndi;

public static unsafe partial class NDIWrapper
{
    private static readonly object ResolverLock = new();
    private static bool resolverRegistered;
    private static bool useAdvanced;
    private static string? exactLookupPath;
    private static string? exactFileLookupPath;

    public static void Initialize(bool useAdvancedDynLib, string? exactLibLookupPath = null)
    {
        Initialize(useAdvancedDynLib, exactLibLookupPath, exactFileLibLookupPath: null);
    }

    /// <summary>
    /// Configures both the primary NDI runtime and the SDK 6.4 File runtime.
    /// The resolver is registered once; later calls update the configured paths.
    /// </summary>
    public static void Initialize(
        bool useAdvancedDynLib,
        string? exactLibLookupPath,
        string? exactFileLibLookupPath)
    {
        lock (ResolverLock)
        {
            useAdvanced = useAdvancedDynLib;
            exactLookupPath = exactLibLookupPath;
            exactFileLookupPath = exactFileLibLookupPath;

            if (resolverRegistered)
            {
                return;
            }

            NativeLibrary.SetDllImportResolver(typeof(NDIWrapper).Assembly, ResolveDllImport);
            resolverRegistered = true;
        }
    }

    public static string GetRuntimeIdentifier()
    {
        var architecture = RuntimeInformation.ProcessArchitecture switch
        {
            Architecture.X86 => "x86",
            Architecture.X64 => "x86-64",
            Architecture.Arm => "arm32",
            Architecture.Arm64 => "arm64",
            Architecture.Wasm => "wasm",
            _ => RuntimeInformation.ProcessArchitecture.ToString()
        };

        var platform =
            RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "win" :
            RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ? "osx" :
            RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ? "linux" :
            RuntimeInformation.IsOSPlatform(OSPlatform.FreeBSD) ? "freebsd" :
            "unknown";

        return $"{platform}-{architecture}";
    }

    private static nint ResolveDllImport(
        string libraryName,
        Assembly assembly,
        DllImportSearchPath? searchPath)
    {
        if (libraryName == LibraryName)
        {
            return TryLoadLibrary(assembly, exactLookupPath, false);
        }

        if (libraryName == FileLibraryName)
        {
            return TryLoadLibrary(assembly, exactFileLookupPath, true);
        }

        return nint.Zero;
    }

    private static nint TryLoadLibrary(
        Assembly assembly,
        string? exactPath,
        bool fileLibrary)
    {
        if (!string.IsNullOrWhiteSpace(exactPath))
        {
            return NativeLibrary.TryLoad(exactPath, out var forcedHandle)
                ? forcedHandle
                : nint.Zero;
        }

        var fileNames = fileLibrary ? GetFileLibraryFileNames() : GetNdiLibraryFileNames();
        foreach (var directory in GetLibrarySearchDirectories(assembly))
        {
            foreach (var fileName in fileNames)
            {
                if (NativeLibrary.TryLoad(Path.Combine(directory, fileName), out var handle))
                {
                    return handle;
                }
            }
        }

        foreach (var fileName in fileNames)
        {
            if (NativeLibrary.TryLoad(fileName, out var handle))
            {
                return handle;
            }
        }

        return nint.Zero;
    }

    private static string[] GetNdiLibraryFileNames()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            if (RuntimeInformation.ProcessArchitecture != Architecture.X64)
            {
                throw new NotImplementedException("Only x64 NDI libraries are supported on Windows.");
            }

            return useAdvanced
                ? ["Processing.NDI.Lib.Advanced.x64.dll"]
                : ["Processing.NDI.Lib.x64.dll"];
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            return useAdvanced
                ? ["libndi_advanced.so", "libndi_advanced.so.6", "libndi_advanced.so.6.4.0", "libNdiAdv.so", "libndi_adv.so"]
                : ["libndi.so", "libndi.so.6"];
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            return useAdvanced
                ? ["libndi_advanced.dylib", "libNdiAdv.dylib"]
                : ["libndi.dylib"];
        }

        throw new NotImplementedException($"{RuntimeInformation.OSDescription} is not supported.");
    }

    private static string[] GetFileLibraryFileNames()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return RuntimeInformation.ProcessArchitecture switch
            {
                Architecture.X64 => ["Processing.NDI.Lib.File.x64.dll", "Processing.NDI.Lib.File.dll"],
                Architecture.X86 => ["Processing.NDI.Lib.File.x86.dll", "Processing.NDI.Lib.File.dll"],
                _ => ["Processing.NDI.Lib.File.dll"]
            };
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            return
            [
                "libProcessing.NDI.Lib.File.so",
                "libndifile.so",
                "libndifile.so.6",
                "libndifile.so.6.4.0"
            ];
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            return ["libProcessing.NDI.Lib.File.dylib", "libndifile.dylib"];
        }

        throw new NotImplementedException($"{RuntimeInformation.OSDescription} is not supported.");
    }

    private static string[] GetLibrarySearchDirectories(Assembly assembly)
    {
        var directories = new List<string>();
        AddDirectory(directories, AppDomain.CurrentDomain.BaseDirectory);
        AddDirectory(directories, Directory.GetCurrentDirectory());
        AddDirectory(directories, Path.GetDirectoryName(assembly.Location));

        AddEnvironmentPath(directories, Environment.GetEnvironmentVariable("NDI_RUNTIME_DIR"));
        AddEnvironmentPath(directories, Environment.GetEnvironmentVariable("NDI_RUNTIME_DIR_V6"));
        AddEnvironmentPath(directories, Environment.GetEnvironmentVariable("NDI_FILE_RUNTIME_DIR"));
        AddEnvironmentPath(directories, Environment.GetEnvironmentVariable("NDI_LIBRARY_PATH"));
        AddSdkPath(directories, Environment.GetEnvironmentVariable("NDI_SDK_DIR"));

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            AddDirectory(directories, "/usr/local/lib");
            AddDirectory(directories, "/usr/lib");
            AddDirectory(directories, "/usr/lib/x86_64-linux-gnu");
            AddDirectory(directories, "/usr/lib/aarch64-linux-gnu");
        }

        return directories
            .Where(static path => !string.IsNullOrWhiteSpace(path))
            .Distinct(StringComparer.Ordinal)
            .ToArray();
    }

    private static void AddEnvironmentPath(List<string> directories, string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return;
        }

        foreach (var part in value.Split(
            Path.PathSeparator,
            StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        {
            AddDirectory(directories, part);
            AddDirectory(directories, Path.Combine(part, "lib"));
            AddDirectory(directories, Path.Combine(part, "bin"));
        }
    }

    private static void AddSdkPath(List<string> directories, string? sdkPath)
    {
        AddEnvironmentPath(directories, sdkPath);
        if (string.IsNullOrWhiteSpace(sdkPath))
        {
            return;
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            AddDirectory(directories, Path.Combine(sdkPath, "Bin", RuntimeInformation.ProcessArchitecture.ToString().ToLowerInvariant()));
        }

        var platformDirectory = GetSdkPlatformDirectory();
        if (platformDirectory is not null)
        {
            AddDirectory(directories, Path.Combine(sdkPath, "lib", platformDirectory));
        }
    }

    private static string? GetSdkPlatformDirectory()
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            return null;
        }

        return RuntimeInformation.ProcessArchitecture switch
        {
            Architecture.X64 => "x86_64-linux-gnu",
            Architecture.X86 => "i686-linux-gnu",
            Architecture.Arm64 => "aarch64-newtek-linux-gnu",
            Architecture.Arm => "arm-newtek-linux-gnueabihf",
            _ => null
        };
    }

    private static void AddDirectory(List<string> directories, string? directory)
    {
        if (!string.IsNullOrWhiteSpace(directory))
        {
            directories.Add(directory);
        }
    }
}
