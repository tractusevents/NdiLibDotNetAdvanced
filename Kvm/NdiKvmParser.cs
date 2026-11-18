using System.Buffers;
using System.Buffers.Binary;
using System.Text;

namespace Tractus.Ndi;

public struct NdiKvmMouseClickEvent
{
    public MouseButton Button;
    public bool Clicked;
}

public struct NdiKvmMouseWheelEvent
{
    public MouseWheel Wheel;
    public float Units;
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

public struct NdiKvmClipboardEvent
{
    public string Clipboard { get; set; }
}

public class KvmEventArgs : EventArgs
{
    public NdiKvmMouseClickEvent? MouseClick { get; }
    public NdiKvmMouseMoveEvent? MouseMove { get; }
    public NdiKvmMouseWheelEvent? MouseWheel { get; }
    public NdiKvmClipboardEvent? Clipboard { get; }

    public byte[] Data { get; }
    public byte OpCode => this.Data[0];
    public NdiKvmKeyboardEvent? KeyEvent { get; }
    public string MetadataXml { get; }

    public KvmEventArgs(NdiKvmClipboardEvent clipboardEvent, byte[] data, string metadataXml)
    {
        this.Clipboard = clipboardEvent;
        this.Data = data;
        this.MetadataXml = metadataXml;
    }

    public KvmEventArgs(NdiKvmMouseWheelEvent wheelEvent, byte[] data, string metadataXml)
    {
        this.MouseWheel = wheelEvent;
        this.Data = data;
        this.MetadataXml = metadataXml;
    }

    public KvmEventArgs(NdiKvmKeyboardEvent keyEvent, byte[] data, string metadataXml)
    {
        this.KeyEvent = keyEvent;
        this.Data = data;
        this.MetadataXml = metadataXml;
    }

    public KvmEventArgs(NdiKvmMouseClickEvent mouseClick, byte[] data, string metadataXml)
    {
        this.MetadataXml = metadataXml;
        this.MouseClick = mouseClick;
        this.Data = data;
    }

    public KvmEventArgs(NdiKvmMouseMoveEvent? mouseMove, byte[] data, string metadataXml)
    {
        this.MetadataXml = metadataXml;
        this.MouseMove = mouseMove;
        this.Data = data;
    }

    public KvmEventArgs(byte[] data, string metadataXml)
    {
        this.MetadataXml = metadataXml;
        this.Data = data;
    }
}

public class NdiKvmParser
{
    private const string Prefix = "<ndi_kvm u=\"";
    private const string Suffix = "\"/>";

    public static void Dump(string base64, string? outputPath = null)
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
                        int keySym = (int)BinaryPrimitives.ReadUInt32LittleEndian(data.Slice(1, 4));
                        var pressed = data[5] == 1;

                        eventType = pressed
                            ? NdiKvmEventType.KeyDown
                            : NdiKvmEventType.KeyUp;
                        args = new KvmEventArgs(
                            new NdiKvmKeyboardEvent
                            {
                                KeySym = keySym,
                                KeyDown = pressed
                            },
                            data.ToArray(),
                            xml);
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
                            data.ToArray(),
                            xml);
                        return true;
                    }

                // Mouse down (left=0x04 / middle=0x05 / right=0x06)
                case 0x04:
                case 0x05:
                case 0x06:
                    {
                        eventType = NdiKvmEventType.MouseDown;
                        var button = opcode switch
                        {
                            0x05 => MouseButton.Middle,
                            0x06 => MouseButton.Right,
                            _ => MouseButton.Left
                        };
                        args = new KvmEventArgs(
                            new NdiKvmMouseClickEvent { Button = button, Clicked = true },
                            data.ToArray(),
                            xml);
                        return true;
                    }

                // Mouse up (left=0x07 / middle=0x08 / right=0x09)
                case 0x07:
                case 0x08:
                case 0x09:
                    {
                        eventType = NdiKvmEventType.MouseUp;
                        var button = opcode switch
                        {
                            0x08 => MouseButton.Middle,
                            0x09 => MouseButton.Right,
                            _ => MouseButton.Left
                        };
                        args = new KvmEventArgs(
                            new NdiKvmMouseClickEvent { Button = button, Clicked = false },
                            data.ToArray(),
                            xml);
                        return true;
                    }

                case 0x0A:
                case 0x0B:
                    {
                        eventType = opcode == 0x0A
                            ? NdiKvmEventType.MouseVerticalWheel
                            : NdiKvmEventType.MouseHorizontalWheel;
                        var units = BinaryPrimitives.ReadSingleLittleEndian(data.Slice(1, 4));
                        args = new KvmEventArgs(
                            new NdiKvmMouseWheelEvent
                            {
                                Units = units,
                                Wheel = opcode == 0x0A ? MouseWheel.Vertical : MouseWheel.Horizontal
                            },
                            data.ToArray(),
                            xml);
                        return true;
                    }

                case 0x0D:
                    {
                        eventType = NdiKvmEventType.Clipboard;
                        var clipboardContents = Encoding.UTF8.GetString(data.Slice(5));
                        args = new KvmEventArgs(
                            new NdiKvmClipboardEvent { Clipboard = clipboardContents },
                            data.ToArray(),
                            xml);
                        return true;
                    }

                default:
                    {
                        eventType = NdiKvmEventType.Unknown;
                        args = new KvmEventArgs(data.ToArray(), xml);
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
            var data = binary.AsSpan<byte>();
            var opcode = binary[0];
            switch (opcode)
            {
                case 0x0C:
                    // Keyboard event of some sort.
                    int keySym = (int)BinaryPrimitives.ReadUInt32LittleEndian(data.Slice(1, 4));
                    var pressed = data[5] == 1;

                    eventType = pressed
                        ? NdiKvmEventType.KeyDown
                        : NdiKvmEventType.KeyUp;

                    return new KvmEventArgs(new NdiKvmKeyboardEvent()
                    {
                        KeySym = keySym,
                        KeyDown = pressed
                    }, binary, metadataXml);
                case 0x03:
                    // Mouse move
                    eventType = NdiKvmEventType.MouseMove;
                    var x = BitConverter.ToSingle(binary, 1);
                    var y = BitConverter.ToSingle(binary, 5);
                    return new KvmEventArgs(new NdiKvmMouseMoveEvent()
                    {
                        X = x,
                        Y = y
                    }, binary, metadataXml);
                case 0x04:
                    // Left MB down
                    eventType = NdiKvmEventType.MouseDown;
                    return new KvmEventArgs(new NdiKvmMouseClickEvent
                    {
                        Button = MouseButton.Left,
                        Clicked = true
                    }, binary, metadataXml);
                case 0x07:
                    // Left MB up
                    eventType = NdiKvmEventType.MouseUp;
                    return new KvmEventArgs(new NdiKvmMouseClickEvent
                    {
                        Button = MouseButton.Left,
                        Clicked = false
                    }, binary, metadataXml);
                case 0x06:
                    // Right MB down
                    eventType = NdiKvmEventType.MouseDown;
                    return new KvmEventArgs(new NdiKvmMouseClickEvent
                    {
                        Button = MouseButton.Right,
                        Clicked = true
                    }, binary, metadataXml);
                case 0x09:
                    // Right MB up
                    eventType = NdiKvmEventType.MouseUp;
                    return new KvmEventArgs(new NdiKvmMouseClickEvent
                    {
                        Button = MouseButton.Right,
                        Clicked = false
                    }, binary, metadataXml);
                case 0x0A:
                case 0x0B:
                    eventType = opcode == 0x0A
                        ? NdiKvmEventType.MouseVerticalWheel
                        : NdiKvmEventType.MouseHorizontalWheel;
                    var units = BinaryPrimitives.ReadSingleLittleEndian(data.Slice(1, 4));
                    return new KvmEventArgs(new NdiKvmMouseWheelEvent()
                    {
                        Units = units,
                        Wheel = opcode == 0x0A ? MouseWheel.Vertical : MouseWheel.Horizontal
                    }, binary, metadataXml);
                case 0x0D:
                    // Clipboard event.
                    eventType = NdiKvmEventType.Clipboard;
                    var clipboardContents = Encoding.UTF8.GetString(data.Slice(5));
                    return new KvmEventArgs(new NdiKvmClipboardEvent()
                    {
                        Clipboard = clipboardContents,
                    }, binary, metadataXml);
                default:
                    eventType = NdiKvmEventType.Unknown;
                    return new KvmEventArgs(binary, metadataXml);
            }

        }
        catch (Exception)
        {
            eventType = NdiKvmEventType.Unknown;
            return null;
        }
    }
}
