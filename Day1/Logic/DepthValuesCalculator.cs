namespace Day1.Logic;

internal interface IDepthValuesCalculator
{
    internal IEnumerable<int> GetDepthValues(IEnumerable<string> input);
}

internal class Assignment1DepthValuesCalculator : IDepthValuesCalculator
{
    IEnumerable<int> IDepthValuesCalculator.GetDepthValues(IEnumerable<string> input)
    {
        return input.Select(int.Parse);
    }
}

internal class Assignment2DepthValuesCalculator : IDepthValuesCalculator
{
    IEnumerable<int> IDepthValuesCalculator.GetDepthValues(IEnumerable<string> input)
    {
        int[] depths = input.Select(int.Parse).ToArray();
        int[] slidingWindows = new int[depths.Length - 2];
        for (int i = 0; i < slidingWindows.Length; i++)
        {
            slidingWindows[i] = depths[i] + depths[i + 1] + depths[i + 2];
        }

        return slidingWindows;
    }
}
