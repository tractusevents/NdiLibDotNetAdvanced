using System.Runtime.InteropServices;

namespace Tractus.Ndi;

/// <summary>
/// Native scatter/gather descriptor. Both arrays and every referenced data block
/// must remain valid until the corresponding synchronous or asynchronous send completes.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct frame_scatter_t
{
    public nint p_data_blocks;
    public nint p_data_blocks_size;
}
