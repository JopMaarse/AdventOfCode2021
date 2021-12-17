using Day16.Logic.Abstraction;

namespace Day16.Logic;

internal class Sum : IEvaluator
{
    public ulong Evaluate(List<ulong> values) => values.Aggregate((total, next) => total + next);
}

internal class Product : IEvaluator
{
    public ulong Evaluate(List<ulong> values) => values.Aggregate((total, next) => total * next);
}

internal class Min : IEvaluator
{
    public ulong Evaluate(List<ulong> values) => values.Min();
}

internal class Max : IEvaluator
{
    public ulong Evaluate(List<ulong> values) => values.Max();
}

internal class Value : IEvaluator
{
    public ulong Evaluate(List<ulong> values) => values.Single();
}

internal class GreaterThan : IEvaluator
{
    public ulong Evaluate(List<ulong> values) => values[0] > values[1] ? 1UL : 0UL;
}

internal class LessThan : IEvaluator
{
    public ulong Evaluate(List<ulong> values) => values[0] < values[1] ? 1UL : 0UL;
}

internal class EqualTo : IEvaluator
{
    public ulong Evaluate(List<ulong> values) => values[0] == values[1] ? 1UL : 0UL;
}
