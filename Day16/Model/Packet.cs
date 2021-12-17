namespace Day16.Model;

public abstract class Packet
{
    public int Version { get; }

    protected Packet(int version)
    {
        Version = version;
    }
}
