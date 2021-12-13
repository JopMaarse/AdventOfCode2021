using InputLogic;
using System.Text.RegularExpressions;

IEnumerable<string> input = await InputHelper.GetInputAsync(day: 13);

HashSet<Point> paper = input
    .Where<string>(line => !string.IsNullOrWhiteSpace(line) && !line.StartsWith("fold"))
    .Select(ParsePoint)
    .ToHashSet();

static Point ParsePoint(string point)
{
    string[] coordinates = point.Split(',');
    return new(int.Parse(coordinates[0]), int.Parse(coordinates[1]));
}

Regex foldPattern = new(@"fold along (x|y)=([0-9]+)");

(Axis Axis, int Line)[] folds = input
    .Where(line => foldPattern.IsMatch(line))
    .Select(line => foldPattern.Match(line).Groups)
    .Select(groups => (Enum.Parse<Axis>(groups[1].Value, ignoreCase: true), int.Parse(groups[2].Value)))
    .ToArray();

Fold(folds[0].Axis, folds[0].Line);
int part1 = paper.Count;
Console.WriteLine($"Part 1: {part1}");

for (int i = 1; i < folds.Length; i++)
{
    Fold(folds[i].Axis, folds[i].Line);
}

Console.WriteLine("Part 2:");
Print();

void Fold(Axis axis, int line)
{
    paper.UnionWith(paper
        .Where(BelowFold)
        .Select(MirrorImage)
        .ToArray());

    paper.RemoveWhere(BelowFold);

    bool BelowFold(Point point) => axis switch
    {
        Axis.X => point.X > line,
        Axis.Y => point.Y > line,
        _ => throw new ArgumentOutOfRangeException(nameof(axis))
    };

    Point MirrorImage(Point point) => axis switch
    {
        Axis.X => new(Mirror(point.X), point.Y),
        Axis.Y => new(point.X, Mirror(point.Y)),
        _ => throw new ArgumentOutOfRangeException(nameof(axis))
    };

    int Mirror(int point) => 2 * line - point;
}

void Print()
{
    int maxX = paper.Select(point => point.X).Max();
    int maxY = paper.Select(point => point.Y).Max();

    for (int y = 0; y <= maxY; y++)
    {
        for (int x = 0; x <= maxX; x++)
        {
            Console.Write(paper.Contains(new(x, y)) ? '#' : '.');
        }

        Console.Write(Environment.NewLine);
    }
}

enum Axis { X, Y }

readonly record struct Point(int X, int Y);
