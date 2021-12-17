using Day16.Logic.Abstraction;
using Day16.Model;
using Day16.Model.Enums;
using System.Collections;

namespace Day16.Logic;

internal class Expression : IEnumerable<Expression>
{
    private readonly List<ulong> _values;

    private readonly IEvaluator _evaluator;

    public Packet Packet { get; }

    public Expression? Parent { get; }

    public ICollection<Expression> Children { get; }

    public Expression(Packet packet, Expression? parent)
    {
        Packet = packet;
        Parent = parent;
        Children = new List<Expression>();
        if (packet is Literal literal)
        {
            _values = new List<ulong> { literal.Value };
            _evaluator = new Value();
            return;
        }

        _values = new();
        _evaluator = (packet as Operator)!.TypeId switch
        {
            TypeId.Sum => new Sum(),
            TypeId.Product => new Product(),
            TypeId.Minimum => new Min(),
            TypeId.Maximum => new Max(),
            TypeId.GreaterThan => new GreaterThan(),
            TypeId.LessThan => new LessThan(),
            TypeId.EqualTo => new EqualTo(),
            _ => throw new Exception("Unknown TypeId")
        };
    }

    public ulong EvaluateTree()
    {
        //queue is used as a stack, because elements are added in reverse
        Queue<Expression> evaluationStack = new();
        foreach (Expression expression in this)
            evaluationStack.Enqueue(expression);

        while (evaluationStack.TryDequeue(out Expression? expression))
        {
            ulong result = expression.Evaluate();
            if (expression == this)
                return result;

            expression.Parent?._values.Add(result);
        }

        throw new Exception("Invalid expression tree");
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

    private ulong Evaluate()
    {
        return _evaluator.Evaluate(_values);
    }
}
