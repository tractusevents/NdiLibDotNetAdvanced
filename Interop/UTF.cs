using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace Tractus.Ndi;

[SuppressUnmanagedCodeSecurity]
public static unsafe partial class UTF
{
    /// <summary>
    /// Allocates a null-terminated UTF-8 string. The caller must release it with
    /// <see cref="Marshal.FreeHGlobal(nint)"/>.
    /// </summary>
    public static nint StringToUtf8(string managedString)
    {
        return StringToUtf8(managedString, out _);
    }

    /// <summary>
    /// Allocates a null-terminated UTF-8 string and returns its byte length,
    /// excluding the terminator. The caller must release the pointer with
    /// <see cref="Marshal.FreeHGlobal(nint)"/>.
    /// </summary>
    public static nint StringToUtf8(string managedString, out int utf8Length)
    {
        ArgumentNullException.ThrowIfNull(managedString);

        var characters = managedString.AsSpan();
        if (!characters.IsEmpty && characters[^1] == '\0')
        {
            characters = characters[..^1];
        }

        if (characters.IndexOf('\0') >= 0)
        {
            throw new ArgumentException("Embedded null characters cannot be represented by a C string.", nameof(managedString));
        }

        utf8Length = Encoding.UTF8.GetByteCount(characters);
        var nativeUtf8 = Marshal.AllocHGlobal(checked(utf8Length + 1));

        try
        {
            var destination = new Span<byte>(nativeUtf8.ToPointer(), utf8Length + 1);
            var bytesWritten = Encoding.UTF8.GetBytes(characters, destination);
            destination[bytesWritten] = 0;
            return nativeUtf8;
        }
        catch
        {
            Marshal.FreeHGlobal(nativeUtf8);
            throw;
        }
    }

    /// <summary>
    /// Copies a native UTF-8 string into a managed string. When length is supplied,
    /// decoding stops at the first null byte within that buffer.
    /// </summary>
    public static string Utf8ToString(nint nativeUtf8, uint? length = null)
    {
        if (nativeUtf8 == nint.Zero)
        {
            return string.Empty;
        }

        if (!length.HasValue)
        {
            return Marshal.PtrToStringUTF8(nativeUtf8) ?? string.Empty;
        }

        if (length.Value > int.MaxValue)
        {
            throw new ArgumentOutOfRangeException(nameof(length));
        }

        return Utf8ToString(new ReadOnlySpan<byte>(nativeUtf8.ToPointer(), (int)length.Value));
    }

    /// <summary>
    /// Decodes a fixed or counted UTF-8 buffer without allocating an intermediate
    /// managed byte array.
    /// </summary>
    public static string Utf8ToString(ReadOnlySpan<byte> utf8)
    {
        var terminator = utf8.IndexOf((byte)0);
        if (terminator >= 0)
        {
            utf8 = utf8[..terminator];
        }

        return utf8.IsEmpty ? string.Empty : Encoding.UTF8.GetString(utf8);
    }
}

/// <summary>
/// Stack-only ownership for a transient null-terminated UTF-8 string.
/// </summary>
public ref struct Utf8String
{
    private nint pointer;

    public Utf8String(string? value)
    {
        this.pointer = value is null ? nint.Zero : UTF.StringToUtf8(value);
    }

    public readonly nint Pointer => this.pointer;

    public static implicit operator nint(Utf8String value) => value.pointer;

    public void Dispose()
    {
        if (this.pointer == nint.Zero)
        {
            return;
        }

        Marshal.FreeHGlobal(this.pointer);
        this.pointer = nint.Zero;
    }
}
