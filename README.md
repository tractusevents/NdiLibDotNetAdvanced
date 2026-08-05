# .NET Wrapper for NDI® Basic and Advanced SDK (C#)

![GitHub](https://img.shields.io/github/license/tractusevents/NdiLibDotNetAdvanced) ![Nuget](https://img.shields.io/nuget/v/NdiLibDotNetAdvanced)


This library provides a wrapper to the NDI libraries for Windows, macOS, and Linux. Both the Basic (Free) and Advanced SDK functions are wrapped.

> This package contains a managed wrapper only. It does not include or license the
> native NDI runtime. Applications must provide a compatible NDI or NDI Advanced
> runtime and, for Advanced features, their own NDI Advanced license.

## Intended audience and abstraction level

This package is a thin managed interop layer over the native NDI SDK. Developers are
expected to be familiar with NDI concepts and API conventions, including object
lifetimes, frame ownership, pixel and audio formats, timecodes, threading, and the
requirement to free frames with the matching NDI function. The
[official NDI SDK documentation](https://docs.ndi.video/all/developing-with-ndi/introduction)
is the authoritative reference for those behaviors.

Only a small number of convenience wrappers are provided. Most functions, structures,
enums, pointers, return values, and ownership rules intentionally remain close to the
native C API so developers can use both Basic and Advanced SDK features without a
high-level abstraction hiding SDK behavior.

## Supported Platforms

- Microsoft Windows (x86-64)
- Linux (x86-64 and ARM64)
- macOS (ARM64)

## Notes

- This project exposes both safe (e.g. passing structs by `ref`) and `unsafe` methods (e.g. passing structs by pointer) for most SDK entrypoints.
- This project assumes use of NDI 6.3. Using older versions of the NDI SDK with this wrapper may result in undefined behavior.

## Successor to NDILibDotNetCoreBase

`NDILibDotNetAdvanced` supersedes the deprecated
[`NDILibDotNetCoreBase`](https://github.com/eliaspuurunen/NdiLibDotNetCoreBase)
package and repository. All ongoing wrapper development happens in this repository.

This is intentionally not a drop-in replacement. To migrate a project, replace its
package reference:

```shell
dotnet remove package NDILibDotNetCoreBase
dotnet add package NDILibDotNetAdvanced
```

Then update code from the old `NewTek` and `NewTek.NDI` namespaces to `Tractus.Ndi`.
The old `NewTek.NDIlib` low-level entry point is now `NDIWrapper`, and its structs and
enums are top-level types in `Tractus.Ndi`. Call `NDIWrapper.Initialize` once before
using native entry points. The old high-level `Sender`, `Router`, `Source`,
`VideoFrame`, and `AudioFrame` compatibility classes are not carried forward; use the
current `NDIWrapper` APIs and types instead.

## Usage

```csharp
using Tractus.Ndi;

var myName = "Recv Test";

// This signals to the wrapper that we should load the advanced SDK
// DLL/dynlib. Pass in false to have the wrapper try to load the
// basic SDK.
//
// Note that if an API entrypoint only exists in the advanced SDK,
// you'll get an exception if you try to call it when in basic
// SDK mode.
NDIWrapper.Initialize(useAdvancedDynLib: true);

using var sourceNameNdiString = new NDIInteropString("CAMERA (CAM-1)");
using var recvName = new NDIInteropString(myName);

var createSettings = new recv_create_v3_t()
{
    allow_video_fields = true,
    bandwidth = recv_bandwidth_e.recv_bandwidth_highest,
    color_format = recv_color_format_e.recv_color_format_fastest,
    source_to_connect_to = new source_t
    {
        p_ndi_name = sourceNameNdiString
    },
    p_ndi_recv_name = recvName
};

// TODO: Provide your VID JSON here.
using var vidJson = new NDIInteropString("");

// If using the basic SDK, use recv_create_v3.
var receiver = NDIWrapper.recv_create_v4(ref createSettings, vidJson);

// Unsafe version for demo purposes only. There's a ref version available of the
// capture method.
unsafe
{
    while(true)
    {
        var videoFrame = new video_frame_v2_t();
        var audioFrame = new audio_frame_v3_t();
        var metaFrame = new metadata_frame_t();

        var frameType = NDIWrapper.recv_capture_v3(
            captureReceiver,
            &videoFrame,
            &audioFrame,
            &tallyMessage,
            1000);

        // TODO: Something useful.
        // TODO: Free the frames.
    }
}

NDIWrapper.recv_destroy(receiver);

```

## `NDIInteropString`

Many NDI structures and functions accept pointers to null-terminated UTF-8 strings,
while a .NET `string` lives in managed memory and cannot safely be passed as a stable
native pointer. `NDIInteropString` bridges that boundary by allocating an unmanaged,
null-terminated UTF-8 copy of a managed string.

It converts implicitly to `nint`, which allows it to be assigned directly to pointer
fields such as `source_t.p_ndi_name`. It also converts back to the original managed
`string`. The unmanaged buffer belongs to the `NDIInteropString` instance:

```csharp
using var sourceName = new NDIInteropString("CAMERA (CAM-1)");

var source = new source_t
{
    p_ndi_name = sourceName
};

// Use source only while sourceName remains alive, unless the NDI SDK documentation
// explicitly states that the called function copies the string.
```

Keep the instance alive for every native call that may read its pointer. Disposing it
frees the unmanaged allocation and sets its pointer to zero; any previously copied
pointer is invalid after that point. The class has no finalizer, so use `using` or call
`Dispose` to avoid leaking unmanaged memory. Do not also call `Marshal.FreeHGlobal` on
its pointer because that would free the same allocation twice.

`null` and an empty string have different native representations:

```csharp
using var omitted = new NDIInteropString(null); // Converts to nint.Zero.
using var empty = new NDIInteropString("");     // Points to a single null terminator.
```

Use `null` for an optional native string that should be absent, and `""` only when the
NDI API expects a present but empty string.

## Native library lookup

When `exactLibLookupPath` is not supplied to `NDIWrapper.Initialize`, the loader
searches for these filenames. Passing `useAdvancedDynLib: true` selects the Advanced
column; passing `false` selects the Basic column.

| Platform | Basic SDK | Advanced SDK |
| --- | --- | --- |
| Windows x86-64 | `Processing.NDI.Lib.x64.dll` | `Processing.NDI.Lib.Advanced.x64.dll` |
| Linux | `libndi.so` | `libndi_advanced.so`, `libNdiAdv.so`, `libndi_adv.so` |
| macOS | `libndi.dylib` | `libndi_advanced.dylib`, `libNdiAdv.dylib` |

The names are tried in the application base directory, current working directory,
assembly directory, and paths supplied through `NDI_RUNTIME_DIR`, `NDI_LIBRARY_PATH`,
or `NDI_SDK_DIR`. For each environment path, its `lib` and `bin` subdirectories are
also searched. Linux additionally searches `/usr/local/lib`, `/usr/lib`, and
`/usr/lib/x86_64-linux-gnu`. Finally, the loader asks the operating system to resolve
each filename using its normal native-library search rules. Filename casing matters
on case-sensitive filesystems.

When `exactLibLookupPath` is supplied, only that exact path is tried and the normal
filename search is skipped. The automatic Windows lookup currently supports x86-64
processes only; other Windows process architectures throw `NotImplementedException`.

## NuGet packages

The managed assembly is platform-neutral, so CI produces one NuGet package rather
than six copies of the same package. Every push to `main` or `master` builds the
project on these native GitHub-hosted runners before creating the package:

- Windows x86-64 (`win-x64`) and ARM64 (`win-arm64`)
- macOS x86-64 (`osx-x64`) and Apple Silicon (`osx-arm64`)
- Linux x86-64 (`linux-x64`) and ARM64 (`linux-arm64`)

Successful runs expose the `.nupkg` and `.snupkg` files as a `nuget` workflow
artifact. The package version is `<VersionPrefix>-ci.<run number>`, which keeps each
mainline build immutable and uniquely versioned.

To create a stable release, set `VersionPrefix` in the project file and push a tag
with the same version prefixed by `v` (for example, `v2026.8.5.1`). A tag whose version
does not match `VersionPrefix` fails instead of publishing an incorrectly versioned
package.

Stable releases use [NuGet.org Trusted Publishing](https://learn.microsoft.com/nuget/nuget-org/trusted-publishing),
so the repository does not store a long-lived API key. Complete this one-time setup
before creating the first release:

1. In this GitHub repository, create an environment named `nuget`. Consider limiting
   it to protected `v*` tags and requiring a reviewer.
2. On NuGet.org, sign in as `eliaspuurunen`, choose **Trusted Publishing** from the
   account menu, and create a GitHub Actions policy with these values:

   | Field | Value |
   | --- | --- |
   | Policy owner | `eliaspuurunen` |
   | Repository owner | `tractusevents` |
   | Repository | `NdiLibDotNetAdvanced` |
   | Workflow file | `nuget.yml` |
   | Environment | `nuget` |

3. Merge the workflow into `main`, then push the matching version tag. The workflow
   exchanges GitHub's short-lived identity token for a temporary NuGet.org API key
   and publishes the package and symbols.

Enter only the workflow filename, not `.github/workflows/nuget.yml`. No GitHub secret
or permanent NuGet API key is required. Mainline builds remain downloadable workflow
artifacts; only version tags publish to NuGet.org.

If NuGet.org initially marks the policy as pending, make the first matching publish
within seven days. A successful publish permanently activates the policy; NuGet.org
also lets you restart an expired activation window.

Linux and macOS filename lookup works in ARM64 processes. The Windows ARM64 CI job
validates the managed package only; automatic native lookup is not currently supported
for that process architecture.

### Legal

NDI® is a registered trademark of Vizrt NDI AB.
