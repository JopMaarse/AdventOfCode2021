namespace Day1.Logic;

internal interface IMarginalGainsCalculator
{
    internal int GetMarginalGains(int[] depths);
}

internal class MarginalGainsCalculator : IMarginalGainsCalculator
{
    int IMarginalGainsCalculator.GetMarginalGains(int[] depths)
    {
        int numberOfMarginalGains = 0;
        for (int i = 0; i < depths.Length - 1; i++)
            if (depths[i + 1] > depths[i])
                numberOfMarginalGains++;
        
        return numberOfMarginalGains;
    }
}
