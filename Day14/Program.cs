using InputLogic;

IEnumerable<string> input = await InputHelper.GetInputAsync(day: 14);

Dictionary<string, char> rules =
    input
        .Skip(1)
        .ToDictionary(
            line => line[0..2],
            line => line[6]);

Dictionary<string, ulong> ruleTriggers = new();
Dictionary<char, ulong> elementOccurences = new();

const int steps = 40;
string polymer = input.First();

foreach (char c in polymer)
{
    elementOccurences.Increment(c);
}

for (int i = 0; i < polymer.Length - 1; i++)
{
    ruleTriggers.Increment(polymer.Substring(i, 2));
}

foreach (KeyValuePair<string, ulong> rule in ruleTriggers)
{
    elementOccurences.Increment(rules[rule.Key], rule.Value);
}

for (int i = 1; i < steps; i++)
{
    Dictionary<string, ulong> nextRuleTriggers = new();

    foreach (KeyValuePair<string, ulong> rule in ruleTriggers)
    {
        
        nextRuleTriggers.Increment($"{rule.Key[0]}{rules[rule.Key]}", rule.Value);
        nextRuleTriggers.Increment($"{rules[rule.Key]}{rule.Key[1]}", rule.Value);        
    }

    foreach (KeyValuePair<string, ulong> rule in nextRuleTriggers)
    {
        elementOccurences.Increment(rules[rule.Key], rule.Value);
    }

    ruleTriggers = nextRuleTriggers;
}

IEnumerable<ulong> occurences = elementOccurences.Select(occurence => occurence.Value);
Console.WriteLine(occurences.Max() - occurences.Min());

static class DictionaryExtensions
{
    public static void Increment<TKey>(
        this Dictionary<TKey, ulong> dictionary,
        TKey key,
        ulong amount = 1)
        where TKey : notnull
    {
        if (!dictionary.ContainsKey(key))
            dictionary[key] = default;

        dictionary[key] += amount;
    }
}
