using InputLogic;
using System.Text.RegularExpressions;

string input = (await InputHelper.GetInputAsync(day: 17)).First();
Regex targetAreaPattern = new(@"\=(\-?[0-9]+)\.\.(\-?[0-9]+)");
int[] matches = targetAreaPattern
    .Matches(input)
    .SelectMany(match => match.Groups.Values
        .Skip(1)
        .Select(group =>int.Parse(group.Value)))
    .ToArray();

Vector min = new(matches[0], matches[2]);
Vector max = new(matches[1], matches[3]);
const int simulationLenth = 1000;
Dictionary<long, (int Min, int Max)> velocitySteps = new();
for (int verticalLaunchVelocity = -simulationLenth; verticalLaunchVelocity < simulationLenth; verticalLaunchVelocity++)
{
    int minSteps = 0, maxSteps = 0;
    long height = Sum(verticalLaunchVelocity);
    for (int velocity = verticalLaunchVelocity < 0 ? verticalLaunchVelocity : 0; height >= min.Y; velocity--)
    {
        if (height > max.Y)
            minSteps++;
        maxSteps++;
        height += velocity;
    }
    if (minSteps != maxSteps)
        velocitySteps[verticalLaunchVelocity] = (minSteps + Math.Max(0, verticalLaunchVelocity), maxSteps + Math.Max(0, verticalLaunchVelocity));
}

long maxHeight = 0, possibleLaunches = 0;
for (long launch = velocitySteps.Keys.Min(); launch <= velocitySteps.Keys.Max(); launch++)
{
    if (!velocitySteps.ContainsKey(launch))
        continue;

    if (HitsTarget(launch, out long reachedHeight, out int launches))
    {
        maxHeight = reachedHeight;
        possibleLaunches += launches;
    }
}

Console.WriteLine($"Part 1: {maxHeight}");
Console.WriteLine($"Part 2: {possibleLaunches}");

bool HitsTarget(long verticalLaunchVelocity, out long maxHeight, out int possibleLaunches)
{
    HashSet<int> possibleX = new();
    maxHeight = default;    
    for (int x = 0; x <= max.X; x++)
    {
        for (int step = velocitySteps[verticalLaunchVelocity].Min; step < velocitySteps[verticalLaunchVelocity].Max; step++)
        {
            long horizontalPosition = Sum(x) - Sum(x - step);

            if (horizontalPosition <= max.X && horizontalPosition >= min.X)
            {
                maxHeight = Sum(verticalLaunchVelocity);
                possibleX.Add(x);
            }
        }        
    }

    possibleLaunches = possibleX.Count;

    return possibleLaunches > 0;
}

static long Sum(long n) => n switch
{
    > 0 => (n + 1) * n / 2,
    _ => 0
};

record struct Vector(int X, int Y);