using Day1.Logic;
using InputLogic;

IEnumerable<string> lines = await InputHelper.GetInputAsync(day: 1);

IDepthValuesCalculator depthValueCalculator =
    args[0] switch
    {
        "1" => new Assignment1DepthValuesCalculator(),
        "2" => new Assignment2DepthValuesCalculator(),
        _ => throw new ArgumentOutOfRangeException()
    };

int[] depths = depthValueCalculator
    .GetDepthValues(lines)
    .ToArray();

IMarginalGainsCalculator marginalGainsCalculator = new MarginalGainsCalculator();
int answer = marginalGainsCalculator.GetMarginalGains(depths);
Console.WriteLine(answer);