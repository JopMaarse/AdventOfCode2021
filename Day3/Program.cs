using InputLogic;

IEnumerable<string> input = await InputHelper.GetInputAsync(day: 3);
int numberOfColumns = input.First().Length;

// Assignment 1
int numberOfRows = input.Count();
uint gammaRate = 0;
for (int i = 0; i < numberOfColumns; i++)
{
    int sum = input
        .Select(line => int.Parse(line.Substring(i, 1)))
        .Sum();

    if (sum > (numberOfRows - sum))
        gammaRate |= (uint)(1 << (11 - i));
}

uint mask = 0b0000_0000_0000_0000_0000_1111_1111_1111;
uint epsilonRate = ~gammaRate & mask;
Console.WriteLine("The answer to assignment 1 is: " + epsilonRate * gammaRate);

// Assignment 2
int[] report = input.Select(s => Convert.ToInt32(s, 2)).ToArray();
int oxygen = report.GetRating(new OxygenRating(), numberOfColumns);
int co2 = report.GetRating(new CO2Rating(), numberOfColumns);

Console.WriteLine("The answer to assignment 2 is: " + oxygen * co2);

static class Logic
{
    public static int GetRating(this int[] report, IRating measurement, int columns)
    {
        for (int i = 0; i < columns && report.Length > 1; i++)
        {
            int column = i;
            Bit criteria = measurement.Criteria(report, column);
            report = report.ApplyCriteriaToColumn(criteria, column).ToArray();
        }

        return report.Single();
    }

    public static Bit FindMostCommonBit(this int[] values, int column)
    {
        int oneCount = values.Select(value => ColumnSelector(value, column)).Sum();
        if (oneCount >= values.Length - oneCount)
            return Bit.One;
        return Bit.Zero;
    }

    public static IEnumerable<int> ApplyCriteriaToColumn(this IEnumerable<int> input, Bit criteria, int column) =>
        input.Where(value => ColumnSelector(value, column) == (int)criteria);

    public static int ColumnSelector(int value, int column)
    {
        value &= 1 << (11 - column);
        return Math.Min(1, value);
    }

    public static Bit Flip(this Bit bit) => bit ^ (Bit)1;
}

enum Bit
{
    Zero = 0,
    One = 1
}

interface IRating
{
    Bit Criteria(int[] values, int column);
}

class OxygenRating : IRating
{
    public Bit Criteria(int[] values, int column) => values.FindMostCommonBit(column);
}

class CO2Rating : IRating
{
    public Bit Criteria(int[] values, int column)
    {
        Bit mostCommonBit = values.FindMostCommonBit(column);
        return mostCommonBit.Flip();
    }
}