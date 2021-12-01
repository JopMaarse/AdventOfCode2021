using Day1.Logic;

IEnumerable<string> lines = await GetInputAsync(sessionToken: args[0]);

IDepthValuesCalculator depthValueCalculator =
    args[1] switch
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

static async Task<IEnumerable<string>> GetInputAsync(string sessionToken)
{
    HttpClient httpClient = new();
    httpClient.DefaultRequestHeaders.Add("cookie", $"session={sessionToken}");
    HttpResponseMessage response = await httpClient.GetAsync("https://adventofcode.com/2021/day/1/input");
    response.EnsureSuccessStatusCode();
    string content = await response.Content.ReadAsStringAsync();
    return content.Split('\n').Where(s => !string.IsNullOrWhiteSpace(s));
}
