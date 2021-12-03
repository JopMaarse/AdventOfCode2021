using InputLogic;

IEnumerable<string> input = await InputHelper.GetInputAsync(day: 3);
int numberOfBits = input.First().Length;

// Assignment 1
int numberOfReadings = input.Count();
ushort gammaRate = 0;
for (int i = 0; i < numberOfBits; i++)
{
    int sum = input
        .Select(line => int.Parse(line.Substring(i, 1)))
        .Sum();

    if (sum > (numberOfReadings - sum))
        gammaRate |= (ushort)(1 << (11 - i));
}

ushort mask = 0b0000_1111_1111_1111;
int epsilonRate = ~gammaRate & mask;
Console.WriteLine("The answer to assignment 1 is: " + epsilonRate * gammaRate);

// Assignment 2
int[] report = input.Select(s => Convert.ToInt32(s, fromBase: 2)).ToArray();
int oxygen = new OxygenRating(report, numberOfBits).Value;
int co2 = new CO2Rating(report, numberOfBits).Value;
Console.WriteLine("The answer to assignment 2 is: " + oxygen * co2);

static class Logic
{
    public static int FindMostCommonBit(this int[] values, int index)
    {
        int oneCount = values.Select(value => value.GetBitAtIndex(index)).Sum();
        return Convert.ToInt32(oneCount >= values.Length - oneCount);
    }
    public static int GetBitAtIndex(this int number, int index)
    {
        number &= 1 << (11 - index);
        return Math.Min(1, number);
    }

    public static IEnumerable<int> FilterOnIndex(this IEnumerable<int> numbers, int index, int value) =>
        numbers.Where(number => number.GetBitAtIndex(index) == value);

    public static int Flip(this int bit) => bit ^ 1;
}

interface IRating
{
    int Value { get; }
}

abstract class RatingBase : IRating
{
    protected abstract int GetCriteria(int[] values, int column);

    public int Value { get; }

    protected RatingBase(int[] report, int numberOfBits)
    {        
        for (int i = 0; i < numberOfBits && report.Length > 1; i++)
        {
            int index = i;
            int criteria = GetCriteria(report, index);
            report = report
                .FilterOnIndex(index, criteria)
                .ToArray();
        }

        Value = report.Single();
    }
}

class OxygenRating : RatingBase
{
    public OxygenRating(int[] report, int numberOfBits) : base(report, numberOfBits) { }

    protected override int GetCriteria(int[] values, int index) =>
        values.FindMostCommonBit(index);
}

class CO2Rating : RatingBase
{
    public CO2Rating(int[] report, int numberOfBits) : base(report, numberOfBits) { }

    protected override int GetCriteria(int[] values, int index) =>
        values
            .FindMostCommonBit(index)
            .Flip();
}