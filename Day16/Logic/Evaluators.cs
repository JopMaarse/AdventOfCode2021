using Day16.Logic.Abstraction;
using Day16.Model;

namespace Day16.Logic;

internal class Sum : IEvaluator
{
    public ulong Evaluate(ICollection<Expression> children) => 
        children
            .Select(c => c.Evaluate())
            .Aggregate((total, next) => total + next);
}

internal class Product : IEvaluator
{
    public ulong Evaluate(ICollection<Expression> children) =>
        children
            .Select(c => c.Evaluate())
            .Aggregate((total, next) => total * next);
}

internal class Minimum : IEvaluator
{
    public ulong Evaluate(ICollection<Expression> children) =>
        children
            .Select(c => c.Evaluate())
            .Min();
}

internal class Maximum : IEvaluator
{
    public ulong Evaluate(ICollection<Expression> children) =>
        children
            .Select(c => c.Evaluate())
            .Max();
}

internal class Literal : IEvaluator
{
    private readonly ulong _value;

    public Literal(ulong value)
    {
        _value = value;
    }

    public ulong Evaluate(ICollection<Expression> _) => _value;
}

internal class GreaterThan : IEvaluator
{
    public ulong Evaluate(ICollection<Expression> children)
    {
        ulong[] values = children
            .Select(c => c.Evaluate())
            .ToArray();

        return values[0] > values[1] ? 1UL : 0UL;
    }
}

internal class LessThan : IEvaluator
{
    public ulong Evaluate(ICollection<Expression> children)
    {
        ulong[] values = children
            .Select(c => c.Evaluate())
            .ToArray();

        return values[0] < values[1] ? 1UL : 0UL;
    }
}

internal class EqualTo : IEvaluator
{
    public ulong Evaluate(ICollection<Expression> children)
    {
        ulong[] values = children
            .Select(c => c.Evaluate())
            .ToArray();

        return values[0] == values[1] ? 1UL : 0UL;
    }
}
