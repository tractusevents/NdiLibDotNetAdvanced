using System;
using System.Runtime.InteropServices;

namespace Tractus.Ndi;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct NDIlib_compressed_packet_t
{
    public int version;
    public NDIlib_compressed_FourCC_type_e fourCC;
    public long pts;
    public long dts;
    public ulong reserved;

    public uint flags;
    public uint data_size;
    public uint extra_data_size;

    // Data goes here. This struct should NEVER be new'ed up.
}

