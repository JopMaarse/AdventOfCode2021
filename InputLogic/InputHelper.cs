namespace InputLogic;
public static class InputHelper
{
    public static async Task<IEnumerable<string>> GetInputAsync(int day, string separator = "\n")
    {
        HttpClient httpClient = new();
        httpClient.DefaultRequestHeaders.Add("cookie", $"session={Environment.GetEnvironmentVariable("sessionToken")}");
        HttpResponseMessage response = await httpClient.GetAsync($"https://adventofcode.com/2021/day/{day}/input");
        response.EnsureSuccessStatusCode();
        string content = await response.Content.ReadAsStringAsync();
        return content.Split(separator).Where(s => !string.IsNullOrWhiteSpace(s));
    }
}
