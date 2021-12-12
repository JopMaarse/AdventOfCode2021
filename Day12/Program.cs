using InputLogic;

IEnumerable<string> input = await InputHelper.GetInputAsync(day: 12);

const char separator = '-';
Dictionary<string, ICollection<string>> neighbours = new();

foreach (string connection in input)
{
    string[] parts = connection.Split(separator);
    ICollection<string> neighbours0 = GetOrAddCave(parts[0]);
    ICollection<string> neighbours1 = GetOrAddCave(parts[1]);
    neighbours0.Add(parts[1]);
    neighbours1.Add(parts[0]);
}

ICollection<string> GetOrAddCave(string name)
{
    if (!neighbours.ContainsKey(name))
        neighbours.Add(name, new List<string>());

    return neighbours[name];
}

List<string> part1Paths = GetPaths(allowSmallCaveDoubleVisit: false);
Console.WriteLine($"Part 1: {part1Paths.Count}");

List<string> part2Paths = GetPaths(allowSmallCaveDoubleVisit: true);
Console.WriteLine($"Part 2: {part2Paths.Count}");

List<string> GetPaths(bool allowSmallCaveDoubleVisit)
{
    List<string> result = new();
    Stack<string> paths = new();
    paths.Push("start");

    while (paths.TryPop(out string? path))
    {
        if (path.EndsWith("end"))
        {
            result.Add(path);
            continue;
        }

        foreach (string next in CalculateNextSteps(path, allowSmallCaveDoubleVisit))
        {
            paths.Push(next);
        }
    }

    return result;
}

IEnumerable<string> CalculateNextSteps(string path, bool allowSmallCaveDoubleVisit)
{
    foreach (string cave in neighbours[path[(path.LastIndexOf(separator) + 1)..]])
    {
        if (CanEnter(path, cave, allowSmallCaveDoubleVisit))
        {
            yield return path + separator + cave;
        }
    }
}

static bool CanEnter(string path, string cave, bool allowSmallCaveDoubleVisit)
{
    IEnumerable<string> smallCaves = path.Split(separator).Where(cave => char.IsLower(cave[0]));

    return cave != "start" && (char.IsUpper(cave[0]) || !path.Contains(cave) ||
        (allowSmallCaveDoubleVisit && smallCaves.Distinct().Count() == smallCaves.Count()));
}