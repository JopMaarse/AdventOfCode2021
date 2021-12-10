using InputLogic;

IEnumerable<string> input = await InputHelper.GetInputAsync(day: 10);

const int parenthesesError = 3,
    bracketError = 57,
    braceError = 1197,
    chevronError = 25137,
    parenthesesComplete = 1,
    bracketComplete = 2,
    braceComplete = 3,
    chevronComplete = 4,
    completeMultiplier = 5;

int part1 = input
    .Select(GetErrorScore)
    .Sum();

Console.WriteLine($"The awnser to part 1 is: {part1}");

long[] scores = input
    .Where(line => GetErrorScore(line) == 0)
    .Select(GetCompleteScore)
    .OrderBy(s => s)
    .ToArray();

long part2 = scores[scores.Length / 2];

Console.WriteLine($"The awnser to part 2 is: {part2}");

static int GetErrorScore(string line)
{
    Stack<char> scope = new();

    foreach (char c in line)
    {
        switch (c)
        {
            case '(' or '[' or '{' or '<': scope.Push(c); break;
            case ')' when !scope.TryPeek(out char top) || top != '(': return parenthesesError;
            case ']' when !scope.TryPeek(out char top) || top != '[': return bracketError;
            case '}' when !scope.TryPeek(out char top) || top != '{': return braceError;
            case '>' when !scope.TryPeek(out char top) || top != '<': return chevronError;
            case ')' or ']' or '}' or '>': scope.Pop(); break;
        };
    }

    return 0;
}

static long GetCompleteScore(string line)
{
    Stack<char> scope = new();

    foreach (char c in line)
    {
        if (IsOpen(c))
            scope.Push(c);
        else
            scope.Pop();
    }

    return scope.Aggregate(0L, (score, next) => score * completeMultiplier + next switch
    {
        '(' => parenthesesComplete,
        '[' => bracketComplete,
        '{' => braceComplete,
        '<' => chevronComplete,
        _ => throw new ArgumentOutOfRangeException(nameof(next))
    });
}

static bool IsOpen(char c) => c is '(' or '[' or '{' or '<';