using System;
using System.Diagnostics;

namespace Tractus.Ndi.Utils;


[DebuggerDisplay("NAL Type {NalType} - {Length} at {Offset}")]
public struct NalUnitIndex
{
    /// <summary>
    /// Offset in the bytestream where the NAL unit begins, including its start code.
    /// </summary>
    public int Offset;

    /// <summary>
    /// Length of the NAL unit data, including the start code.
    /// </summary>
    public int Length;

    /// <summary>
    /// NAL unit type extracted from the header (after the start code).
    /// </summary>
    public byte NalType;
}
