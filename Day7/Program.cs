using InputLogic;

IEnumerable<string> input = await InputHelper.GetInputAsync(day: 7, separator: ",");
IEnumerable<int> crabPositions = input.Select(int.Parse);

Console.WriteLine("The answer to part 1 is: " + CalculateMinimalFuelCost(n => n));
Console.WriteLine("The answer to part 2 is: " + CalculateMinimalFuelCost(n => n * (n + 1) / 2));

int CalculateMinimalFuelCost(Func<int, int> fuelFactor)
{
    int minimalFuelCost = int.MaxValue;
    for (int targetPosition = crabPositions.Min(); targetPosition < crabPositions.Max(); targetPosition++)
    {
        int fuelCost = crabPositions
            .Select(crabPosition => fuelFactor(Math.Abs(crabPosition - targetPosition)))
            .Sum();

        minimalFuelCost = Math.Min(fuelCost, minimalFuelCost);
    }

    return minimalFuelCost;
}
