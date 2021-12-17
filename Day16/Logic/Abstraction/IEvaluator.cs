using Day16.Model;

namespace Day16.Logic.Abstraction;

internal interface IEvaluator
{
    ulong Evaluate(ICollection<Expression> children);
}
