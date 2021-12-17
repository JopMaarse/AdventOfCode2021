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

int minX = matches[0], maxX = matches[1], minY = matches[2], maxY = matches[3];
Dictionary<int, (int Min, int Max)> stepsPerLaunch = CalculateStepsPerVerticalLaunch();
(int maxHeight, int launchesOnTarget) = CalulateLaunchesOnTarget();
Console.WriteLine($"Part 1: {maxHeight}");
Console.WriteLine($"Part 2: {launchesOnTarget}");

Dictionary<int, (int, int)> CalculateStepsPerVerticalLaunch()
{
    Dictionary<int, (int, int)> stepsPerLaunch = new();
    for (int verticalLaunchVelocity = minY; verticalLaunchVelocity < -minY; verticalLaunchVelocity++)
    {
        int minSteps = 0, maxSteps = 0;
        int height = Sum(verticalLaunchVelocity);
        for (int velocity = verticalLaunchVelocity < 0 ? verticalLaunchVelocity : 0; height >= minY; velocity--)
        {
            if (height > maxY)
                minSteps++;

            maxSteps++;
            height += velocity;
        }

        if (minSteps != maxSteps)
            stepsPerLaunch[verticalLaunchVelocity] = (minSteps + Math.Max(0, verticalLaunchVelocity), maxSteps + Math.Max(0, verticalLaunchVelocity));
    }

    return stepsPerLaunch;
}

(int, int) CalulateLaunchesOnTarget()
{
    int maxHeight = 0, launchesOnTarget = 0;
    for (int verticalLaunchVelocity = stepsPerLaunch.Keys.Min(); verticalLaunchVelocity <= stepsPerLaunch.Keys.Max(); verticalLaunchVelocity++)
    {
        if (!stepsPerLaunch.ContainsKey(verticalLaunchVelocity))
            continue;

        if (HitsTarget(verticalLaunchVelocity, out int launches))
        {
            maxHeight = Sum(verticalLaunchVelocity);
            launchesOnTarget += launches;
        }
    }

    return (maxHeight, launchesOnTarget);
}

bool HitsTarget(int verticalLaunchVelocity, out int numberOfLaunches)
{
    HashSet<int> launches = new();
    for (int horizontalLauchVelocity = 0; horizontalLauchVelocity <= maxX; horizontalLauchVelocity++)
    {
        for (int step = stepsPerLaunch[verticalLaunchVelocity].Min; step < stepsPerLaunch[verticalLaunchVelocity].Max; step++)
        {
            int horizontalPosition = Sum(horizontalLauchVelocity) - Sum(horizontalLauchVelocity - step);
            if (horizontalPosition <= maxX && horizontalPosition >= minX)
            {                
                launches.Add(horizontalLauchVelocity);
            }
        }        
    }

    numberOfLaunches = launches.Count;

    return numberOfLaunches > 0;
}

static int Sum(int n) => n switch
{
    > 0 => (n + 1) * n / 2,
    _ => 0
};
