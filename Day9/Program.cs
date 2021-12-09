using InputLogic;
using System.Diagnostics.CodeAnalysis;

string[] input = (await InputHelper.GetInputAsync(day: 9)).ToArray();

Map map = new(maxHeight: input.Length, maxWidth: input[0].Length);

for (int y = 0; y < map.MaxHeight; y++)
{
    for (int x = 0; x < map.MaxWidth; x++)
    {
        map.Locations.Add((x, y), new((int)char.GetNumericValue(input[y][x]), map, x, y));
    }
}

IEnumerable<Location> lowPoints = map.Locations.Values.Where(l => l.IsLocalMinimum);

int part1 = lowPoints
    .Select(l => l.Height + 1)
    .Sum();

Console.WriteLine(part1);

int part2 = lowPoints
    .Select(GetBasin)
    .Select(basin => basin.Count())
    .OrderByDescending(size => size)
    .Take(3)
    .Aggregate((current, next) => current * next);

Console.WriteLine(part2);

static IEnumerable<Location> GetBasin(Location location)
{
    return GetBasin(location, new HashSet<Location>(new Location.EqualityComparer()));

    static IEnumerable<Location> GetBasin(Location location, ISet<Location> basin)
    {
        basin.Add(location);
        Recursion(loction => location.Up);
        Recursion(loction => location.Down);
        Recursion(loction => location.Left);
        Recursion(loction => location.Right);

        return basin;

        void Recursion(Func<Location, Location?> direction)
        {
            if (direction(location).HasValue &&
                !basin.Contains(direction(location)!.Value) &&
                direction(location)!.Value.Height < 9)
            {
                basin.UnionWith(GetBasin(direction(location)!.Value, basin));
            }
        }
    }
}

class Map
{
    public Dictionary<(int, int), Location> Locations { get; }
    public int MaxHeight { get; }
    public int MaxWidth { get; }

    public Map(int maxHeight, int maxWidth)
    {
        Locations = new(capacity: maxHeight * maxWidth);
        MaxHeight = maxHeight;
        MaxWidth = maxWidth;
    }
}

record struct Location
{
    private readonly Map map;
    private readonly int x, y;
    
    public int Height { get; }

    public Location? Up => y > 0 
        ? map.Locations[(x, y - 1)] 
        : null;

    public Location? Down => y < map.MaxHeight - 1 
        ? map.Locations[(x, y + 1)] 
        : null;

    public Location? Left => x > 0 
        ? map.Locations[(x - 1, y)] 
        : null;

    public Location? Right => x < map.MaxWidth - 1 
        ? map.Locations[(x + 1, y)] 
        : null;

    public bool IsLocalMinimum =>
        (!Up   .HasValue || Height < Up   .Value.Height) &&
        (!Down .HasValue || Height < Down .Value.Height) &&
        (!Left .HasValue || Height < Left .Value.Height) &&
        (!Right.HasValue || Height < Right.Value.Height);        

    public Location(int height, Map map, int x, int y)
    {
        Height = height;
        this.map = map;
        this.x = x;
        this.y = y;
    }

    public override string ToString()
    {
        return $"({x},{y}): {Height}";
    }

    public class EqualityComparer : IEqualityComparer<Location>
    {
        public bool Equals(Location a, Location b)
        {
            return a.x == b.x && a.y == b.y;
        }

        public int GetHashCode([DisallowNull] Location location)
        {
            return location.GetHashCode();
        }
    }
}
