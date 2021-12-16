using InputLogic;

string[] input = (await InputHelper.GetInputAsync(day: 15)).ToArray();

const int height = 100, width = 100;
Dictionary<Location, int> locationRisks = new();
HashSet<Location> border = new();
locationRisks[new(0,0)] = 0;
border.Add(new(1, 0));
border.Add(new(0, 1));

while (border.Any())
{
    (int Risk, Location Location) minNeighbour = border
        .Select(location => (Risk: MinNeighbour(location) + Risk(location), Location: location))
        .MinBy(location => location.Risk);

    locationRisks[minNeighbour.Location] = minNeighbour.Risk;
    border.Remove(minNeighbour.Location);
    foreach (Location n in Neighbours(minNeighbour.Location).Where(n => !locationRisks.ContainsKey(n)))
    {
        border.Add(n);
    }
}

Console.WriteLine("Part 1: " + locationRisks[new(width - 1, height - 1)]);
Console.WriteLine("Part 2: " + locationRisks[new(5 * width - 1, 5 * height - 1)]);

int MinNeighbour(Location location) => 
    Neighbours(location)
        .Where(n => locationRisks.ContainsKey(n))
        .Select(n => locationRisks[n])
        .Min();

int Risk(Location location)
{
    int xMod = location.X / width;
    int yMod = location.Y / height;

    int risk = input[location.Y % height][location.X % width] - '0';
    risk = Reduce(risk + xMod + yMod);

    return risk;
}

static int Reduce(int n) => n switch
{
    < 10 => n,
    _ => Reduce((n % 10) + 1)
};

IEnumerable<Location> Neighbours(Location location)
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