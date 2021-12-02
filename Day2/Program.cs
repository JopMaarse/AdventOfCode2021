using InputLogic;

IEnumerable<string> input = await InputHelper.GetInputAsync(day: 2);
IEnumerable<(Direction Direction, int Units)> parsedInput = input.Select(ParseInput);
// Assignment 1
Dictionary<Direction, int> totals = parsedInput
    .GroupBy(movement => movement.Direction, movement => movement.Units)
    .Select(SumDistance)
    .ToDictionary(t => t.Direction, t => t.TotalDistance);

// Assignment 2
int aim = 0, depth = 0, distance = 0;
foreach ((Direction direction, int units) in parsedInput)
{
    switch (direction)
    {
        case Direction.Forward: 
            distance += units;
            depth += aim * units;
            break;
        case Direction.Up:
            aim -= units;
            break;
        case Direction.Down:
            aim += units;
            break;
    }
}

Console.WriteLine("Assignment 1 answer: " + totals[Direction.Forward] * (totals[Direction.Down] - totals[Direction.Up]));
Console.WriteLine("Assignment 2 answer: " + distance * depth);

static (Direction Direction, int Distance) ParseInput(string line)
{
    string[] parts = line.Split(' ');
    return (Enum.Parse<Direction>(parts[0], ignoreCase: true), int.Parse(parts[1]));
}

static (Direction Direction, int TotalDistance) SumDistance(IGrouping<Direction, int> grouping) => (grouping.Key, grouping.Sum());

enum Direction
{
    Forward,
    Up,
    Down
}
