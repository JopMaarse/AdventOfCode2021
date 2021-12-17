using Day16.Logic.Abstraction;
using Day16.Model;
using Day16.Model.Enums;

namespace Day16.Logic;

internal class Parser
{
    public Expression Parse(string input)
    {
        int position = 0;
        return ParseExpression(input, null, ref position);
    }

    private Expression ParseExpression(string input, Expression? parent, ref int position)
    {
        int currentPosition = position;
        short version = Convert.ToInt16(Read(3), fromBase: 2);
        TypeId typeId = (TypeId)Convert.ToInt32(Read(3), fromBase: 2);
        Expression result;
        if (typeId == TypeId.Literal)
            result = ParseLiteral();
        else
            result = ParseOperator();

        position = currentPosition;
        return result;

        Expression ParseLiteral()
        {
            string value = string.Empty;
            string group;
            do
            {
                group = Read(5);
                value += group[1..];
            }
            while (group[0] == '1');

            return new Expression(version, new Literal(Convert.ToUInt64(value, fromBase: 2)));
        }

        Expression ParseOperator()
        {
            IEvaluator evaluator = typeId switch
            {
                TypeId.Sum => new Sum(),
                TypeId.Product => new Product(),
                TypeId.Minimum => new Minimum(),
                TypeId.Maximum => new Maximum(),
                TypeId.GreaterThan => new GreaterThan(),
                TypeId.LessThan => new LessThan(),
                TypeId.EqualTo => new EqualTo(),
                _ => throw new Exception("Unkown TypeId")
            };

            Expression expression = new(version, evaluator);
            int length;
            if (Read(1) == "0")
            {
                length = Convert.ToInt32(Read(15), fromBase: 2);
                string children = Read(length);
                int childPosition = 0;
                while (childPosition < length)
                    expression.Children.Add(ParseExpression(children, expression, ref childPosition));

                return expression;
            }

            length = Convert.ToInt32(Read(11), fromBase: 2);
            for (int i = 0; i < length; i++)
                expression.Children.Add(ParseExpression(input, expression, ref currentPosition));

            return expression;
        }

        string Read(int length)
        {
            string result = input.Substring(currentPosition, length);
            currentPosition += length;
            return result;
        }
    }
}
