using Day15;
using InputLogic;

string[] input = (await InputHelper.GetInputAsync(day: 15)).ToArray();
const int height = 100, width = 100;
Dijkstra<Location> dijkstra = new(source: new(0, 0), Neighbours, (_, location) => Risk(location), size: 500 * 500);
Console.WriteLine("Part 1: " + dijkstra.GetDistance(new(99, 99)));
Console.WriteLine("Part 2: " + dijkstra.GetDistance(new(499, 499)));

uint Risk(Location location)
{
    int risk = input[location.Y % height][location.X % width] - '0';
    int xMod = location.X / width;
    int yMod = location.Y / height;
    return (uint)Reduce(risk + xMod + yMod);
}

static int Reduce(int n) => n switch
{
    < 10 => n,
    _ => Reduce((n % 10) + 1)
};

static IEnumerable<Location> Neighbours(Location location)
{
    if (location.X + 1 < width * 5)
        yield return new(location.X + 1, location.Y);
    
    if (location.Y + 1 < height * 5)
        yield return new(location.X, location.Y + 1);
    
    if (location.X > 0)
        yield return new(location.X - 1, location.Y);
    
    if (location.Y > 0)
        yield return new(location.X, location.Y - 1);
}

readonly record struct Location(int X, int Y);