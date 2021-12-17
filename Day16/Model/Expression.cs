using Day16.Logic.Abstraction;
using System.Collections;

namespace Day16.Model;

internal class Expression : IEnumerable<Expression>
{
    private readonly IEvaluator _evaluator;
        
    public short Version { get; }

    public ICollection<Expression> Children { get; }

    public Expression(short version, IEvaluator evaluator)
    {
        Version = version;
        _evaluator = evaluator;
        Children = new List<Expression>();        
    }

    public ulong Evaluate()
    {
        return _evaluator.Evaluate(Children);
    }

    //DFS
    public IEnumerator<Expression> GetEnumerator()
    {
        foreach (Expression child in Children)
            foreach (Expression enumerable in child)
                yield return enumerable;

        yield return this;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
