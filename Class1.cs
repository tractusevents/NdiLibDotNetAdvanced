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

public static class H264NalParser
{
    /// <summary>
    /// Parses a raw H.264 bytestream and returns a list of indices for each NAL unit.
    /// The returned indices include the start code.
    /// </summary>
    /// <param name="data">The complete H.264 bytestream (e.g. loaded from a file).</param>
    /// <returns>A list of NalUnitIndex structures containing offset, length, and type.</returns>
    public static List<NalUnitIndex> ParseNalUnits(ReadOnlySpan<byte> data,
        int length = -1)
    {
        if(length == -1)
        {
            length = data.Length;
        }

        var nalIndices = new List<NalUnitIndex>();
        int pos = 0;
        int dataLength = length;

        while (pos < dataLength)
        {
            // Find the next start code from the current position.
            int startCodeIndex = FindStartCode(data, pos);
            if (startCodeIndex < 0)
            {
                break;
            }

            // Determine the length of the start code (3 or 4 bytes).
            int startCodeLength = (data[startCodeIndex + 2] == 0x01) ? 3 : 4;

            // Now find the next start code after the current one.
            int nextStartCodeIndex = FindStartCode(data, startCodeIndex + startCodeLength);
            int nalLength = (nextStartCodeIndex >= 0)
                ? nextStartCodeIndex - startCodeIndex
                : dataLength - startCodeIndex;

            // The NAL unit header is after the start code.
            byte nalType = (byte)(data[startCodeIndex + startCodeLength] & 0x1F);

            nalIndices.Add(new NalUnitIndex
            {
                Offset = startCodeIndex,
                Length = nalLength,
                NalType = nalType
            });

            pos = startCodeIndex + nalLength;
        }

        return nalIndices;
    }

    /// <summary>
    /// Searches for a H.264 start code (0x000001 or 0x00000001) beginning at the given offset.
    /// Returns the index of the first byte of the start code, or -1 if not found.
    /// </summary>
    private static int FindStartCode(ReadOnlySpan<byte> data, int offset)
    {
        // Need at least 3 bytes to check for the smallest start code.
        for (int i = offset; i < data.Length - 3; i++)
        {
            if (data[i] == 0x00 && data[i + 1] == 0x00)
            {
                if (data[i + 2] == 0x01)
                    return i;
                if (i + 3 < data.Length && data[i + 2] == 0x00 && data[i + 3] == 0x01)
                    return i;
            }
        }
        return -1;
    }
}