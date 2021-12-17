using Day16.Model.Enums;

namespace Day16.Model;

sealed class Operator : Packet
{
    public TypeId TypeId { get; }

    public Operator(int version, TypeId typeId) : base(version)
    {
        TypeId = typeId;
    }
}
