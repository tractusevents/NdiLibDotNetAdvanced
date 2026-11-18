using System.Runtime.InteropServices;
using System.Text;

namespace Tractus.Ndi;

public class NdiSource
{
    private const byte Space = 0x20;

    private readonly byte[] nameUtf8;
    private readonly byte[] urlAddressUtf8;
    private readonly int spaceIndex;

    private string? cachedName;
    private string? cachedComputerName;
    private string? cachedSourceName;
    private string? cachedUrlAddress;

    public NdiSource(ref source_t source)
    {
        this.nameUtf8 = BorrowUtf8(source.p_ndi_name).ToArray();
        this.urlAddressUtf8 = BorrowUtf8(source.p_url_address).ToArray();
        this.spaceIndex = this.NameUtf8.IndexOf(Space);
    }

    public ReadOnlySpan<byte> NameUtf8 => this.nameUtf8;
    public ReadOnlySpan<byte> UrlAddressUtf8 => this.urlAddressUtf8;

    public ReadOnlySpan<byte> ComputerNameUtf8 =>
        this.spaceIndex >= 0 ? this.NameUtf8[..this.spaceIndex] : this.NameUtf8;

    public ReadOnlySpan<byte> SourceNameUtf8 =>
        this.spaceIndex >= 0 ? this.NameUtf8[(this.spaceIndex + 1)..] : this.NameUtf8;

    public string Name => this.cachedName ??= Encoding.UTF8.GetString(this.NameUtf8);

    public string ComputerName =>
        this.cachedComputerName ??= Encoding.UTF8.GetString(this.ComputerNameUtf8);

    public string SourceName => this.cachedSourceName ??= DecodeSourceName(this.SourceNameUtf8);

    public string UrlAddress =>
        this.cachedUrlAddress ??= Encoding.UTF8.GetString(this.UrlAddressUtf8);

    private static string DecodeSourceName(ReadOnlySpan<byte> utf8)
    {
        var decoded = Encoding.UTF8.GetString(utf8).Trim();
        if (decoded.Length >= 2 && decoded[0] == '(' && decoded[^1] == ')')
        {
            return decoded[1..^1].Trim();
        }

        return decoded;
    }

    internal bool Matches(in source_t source)
    {
        return this.NameUtf8.SequenceEqual(BorrowUtf8(source.p_ndi_name))
            && this.UrlAddressUtf8.SequenceEqual(BorrowUtf8(source.p_url_address));
    }

    internal static unsafe ReadOnlySpan<byte> BorrowUtf8(nint pointer)
    {
        return MemoryMarshal.CreateReadOnlySpanFromNullTerminated((byte*)pointer);
    }
}
