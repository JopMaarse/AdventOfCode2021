namespace Day16.Model;

internal sealed class Literal : Packet
{
    public ulong Value { get; }

    public Literal(int version, ulong value) : base(version)
    {
        Value = value;
    }
}
