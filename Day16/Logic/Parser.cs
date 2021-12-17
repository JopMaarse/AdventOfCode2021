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
        int version = Convert.ToInt32(Read(3), fromBase: 2);
        TypeId typeId = (TypeId)Convert.ToInt32(Read(3), fromBase: 2);

        if (typeId == TypeId.Literal)
        {
            Expression result = ParseLiteral();
            position = currentPosition;
            return result;
        }
        else
        {
            Expression result = ParseOperator();
            position = currentPosition;
            return result;
        }

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

            Literal packet = new(version, Convert.ToUInt64(value, fromBase: 2));
            return new Expression(packet, parent);
        }

        Expression ParseOperator()
        {
            LengthTypeId lengthTypeId = Enum.Parse<LengthTypeId>(Read(1));
            Operator packet = new(version, typeId);
            Expression node = new(packet, parent);
            int length;
            if (lengthTypeId == LengthTypeId.TotalLength)
            {
                length = Convert.ToInt32(Read(15), fromBase: 2);
                string children = Read(length);
                int childPosition = 0;
                while (childPosition < length)
                {
                    node.Children.Add(ParseExpression(children, node, ref childPosition));
                }

                return node;
            }
            
            length = Convert.ToInt32(Read(11), fromBase: 2);
            for (int i = 0; i < length; i++)
            {
                node.Children.Add(ParseExpression(input, node, ref currentPosition));
            }

            return node;
        }

        string Read(int length)
        {
            string result = input.Substring(currentPosition, length);
            currentPosition += length;
            return result;
        }
    }
}
