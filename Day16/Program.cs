using Day16.Logic;
using Day16.Model;
using InputLogic;

string hexadecimal = (await InputHelper.GetInputAsync(day: 16)).First();

string binary = hexadecimal
    .Replace("0", "0000")
    .Replace("1", "0001")
    .Replace("2", "0010")
    .Replace("3", "0011")
    .Replace("4", "0100")
    .Replace("5", "0101")
    .Replace("6", "0110")
    .Replace("7", "0111")
    .Replace("8", "1000")
    .Replace("9", "1001")
    .Replace("A", "1010")
    .Replace("B", "1011")
    .Replace("C", "1100")
    .Replace("D", "1101")
    .Replace("E", "1110")
    .Replace("F", "1111");

Expression expressionTree = new Parser().Parse(binary);
Console.WriteLine("Part 1: " + expressionTree.Sum(expression => expression.Version));
Console.WriteLine("Part 2: " + expressionTree.Evaluate());
