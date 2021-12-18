using InputLogic;
using System.Text.RegularExpressions;

IEnumerable<string> input = await InputHelper.GetInputAsync(day: 18);

SnailFishNumber sum = input
    .Select(Parse)
    .Aggregate((current, next) => current.Add(next));

Console.WriteLine("Part 1: " + sum.Magnitude);

IEnumerable<(string, string)> cartesianProduct =
    from n in input
    from m in input
    select (n, m);

long maximum = 0;
foreach ((string n, string m)in cartesianProduct)
{
    if (n == m)
        continue;
    long magnitude;
    if ((magnitude = Parse(n).Add(Parse(m)).Magnitude) > maximum)
        maximum = magnitude;
    if ((magnitude = Parse(m).Add(Parse(n)).Magnitude) > maximum)
        maximum = magnitude;
}

Console.WriteLine("Part 2: " + maximum);

static SnailFishNumber Parse(string input)
{
    return (ParseRecursive(input) as SnailFishNumber)!;
}

static SnailFishNumberBase ParseRecursive(string input)
{
    if (input.Length == 1)
        return new Regular(int.Parse(input));

    Regex number = new(@"^\[((?:(?:(?'open'\[)+[0-9\,]*)+(?:(?'-open'\])+[0-9\,]*)+)+|(?:[0-9]))\,((?:(?:(?'open2'\[)+[0-9\,]*)+(?:(?'-open2'\])+[0-9\,]*)+)+|(?:[0-9]))\]$");
    Match match = number.Match(input);
    return new SnailFishNumber(
        ParseRecursive(match.Groups[1].Value),
        ParseRecursive(match.Groups[2].Value));
}

abstract class SnailFishNumberBase
{    
    public SnailFishNumber? Parent { get; set; }
    public abstract long Magnitude { get; }
}

class SnailFishNumber : SnailFishNumberBase
{
    private bool IsLeftChild => Parent?.Left == this;

    public SnailFishNumberBase Left { get; set; }
    
    public SnailFishNumberBase Right { get; set; }
    
    public override long Magnitude => 3 * Left.Magnitude + 2 * Right.Magnitude;

    public SnailFishNumber(SnailFishNumberBase left, SnailFishNumberBase right)
    {
        Left = left;
        Right = right;
        left.Parent = right.Parent = this;
    }

    public SnailFishNumber Add(SnailFishNumber number)
    {
        SnailFishNumber result = new(this, number);        
        Reduce(result);
        return result;
    }

    public override string ToString() => $"[{Left},{Right}]";

    private static void Reduce(SnailFishNumber number)
    {
        bool isReduced = false;
        while (!isReduced)
        {
            while (number.ReduceExplode(0));
            isReduced = !number.ReduceSplit();
        }
    }

    private bool ReduceExplode(int depth)
    {
        if (depth >= 4 && Left is Regular && Right is Regular)
        {
            Explode();
            return true;
        }

        if (Left is SnailFishNumber left)
            if(left.ReduceExplode(depth + 1))
                return true;

        if (Right is SnailFishNumber right)
            if (right.ReduceExplode(depth + 1))
                return true;

        return false;
    }

    private void Explode()
    {
        bool isLeftChild = IsLeftChild;
        SnailFishNumber parent = Parent!;
        while (parent.Parent != null)
        {
            if (isLeftChild && parent.IsLeftChild)
            {
                parent = parent.Parent;
                continue;
            }

            SnailFishNumberBase node = isLeftChild ? parent.Parent.Left : parent.Left;
            while (node is SnailFishNumber pair)
                node = pair.Right;
            (node as Regular)!.Value += (Left as Regular)!.Value;
            break;
        }

        parent = Parent!;
        while (parent.Parent != null)
        {
            if (!isLeftChild && !parent.IsLeftChild)
            {
                parent = parent.Parent;
                continue;
            }

            SnailFishNumberBase node = isLeftChild ? parent.Right : parent.Parent.Right;
            while (node is SnailFishNumber pair)
                node = pair.Left;
            (node as Regular)!.Value += (Right as Regular)!.Value;
            break;
        }

        Regular newNumber = new(0);
        newNumber.Parent = Parent;
        if (isLeftChild)
            Parent!.Left = newNumber;
        else
            Parent!.Right = newNumber;
    }   

    private bool ReduceSplit()
    {
        if (Left is Regular left && left.Magnitude >= 10)
        {
            Left = left.Split();            
            return true;
        }

        if (Left is SnailFishNumber leftNode && leftNode.ReduceSplit())
            return true;
        
        if (Right is SnailFishNumber rightNode && rightNode.ReduceSplit())
            return true;

        if (Right is Regular right && right.Magnitude >= 10)
        {
            Right = right.Split();
            return true;
        }

        return false;
    }
}

class Regular : SnailFishNumberBase
{
    public long Value { get; set; }
        
    public override long Magnitude => Value;

    public Regular(long value)
    {
        Value = value;
    }

    public SnailFishNumber Split()
    {
        SnailFishNumber split = new(new Regular(Value / 2), new Regular((Value + 1) / 2));
        split.Parent = Parent;
        return split;
    }

    public override string ToString() => Value.ToString();
}