using InputLogic;

IEnumerable<string> input = await InputHelper.GetInputAsync(day: 8);

// part 1
int part1 = input
    .SelectMany(line => line.Split(" | ")[1].Split(' '))
    .Count(signal => new[] { 2, 3, 4, 7 }.Contains(signal.Length));

Console.WriteLine(part1);

// part 2
IEnumerable<string[]> uniqueSignalReadings = input
    .Select(signals => signals
        .Split(" | ")[0]
        .Split(' '));

IEnumerable<string[]> outputValues = input
    .Select(signals => signals
        .Split(" | ")[1]
        .Split(' '));

int part2 = 0;

foreach ((string[] uniqueSignals, string[] outputDigits) in uniqueSignalReadings.Zip(outputValues))
{
    Solution solution = DeductSolution(uniqueSignals);
    part2 +=
        int.Parse(new(outputDigits
            .Select(digit => new Display(solution
                .Map(digit
                    .ToCharArray()
                    .Select(ParseSegment)))
                .Value)
            .ToArray()));
}

Console.WriteLine(part2);

static Solution DeductSolution(IEnumerable<string> signals)
{
    Solution solution = new();

    IEnumerable<Segment> one = signals
        .Single(signal => signal.Length == 2)
        .ToCharArray()
        .Select(ParseSegment);

    foreach (Segment on in one)
    {
        solution.C.Add(on);
        solution.F.Add(on);
    }

    IEnumerable<Segment> seven = signals
        .Single(signal => signal.Length == 3)
        .ToCharArray()
        .Select(ParseSegment);

    solution.A
        .Add(seven        
            .Except(solution.C)
            .Single());

    IEnumerable<Segment> four = signals
        .Single(signal => signal.Length == 4)
        .ToCharArray()
        .Select(ParseSegment)
        .Except(solution.C);

    foreach (Segment on in four)
    {
        solution.B.Add(on);
        solution.D.Add(on);
    }

    IEnumerable<IEnumerable<Segment>> zeroSixNine = signals
        .Where(signal => signal.Length == 6)
        .Select(signal => signal
            .ToCharArray()
            .Select(ParseSegment));

    solution.D.Remove(
        solution.D
            .Single(possibility => zeroSixNine
                .All(signals => signals
                    .Contains(possibility))));

    solution.B.Remove(solution.D.Single());
    
    solution.C.Remove(
        solution.C
            .Single(possibility => zeroSixNine
                .All(signals => signals
                    .Contains(possibility))));

    solution.F.Remove(solution.C.Single());

    IEnumerable<Segment> nine = zeroSixNine
        .Where(signals => signals.Contains(solution.D.Single()))
        .Where(signals => signals.Contains(solution.C.Single()))
        .Single();
    
    IEnumerable<Segment> zero = zeroSixNine
        .Where(signals => !signals.Contains(solution.D.Single()))        
        .Single();

    solution.E.Add(zero.Except(nine).Single());
    
    IEnumerable<IEnumerable<Segment>> twoThreeFive = signals
        .Where(signal => signal.Length == 5)
        .Select(signal => signal
            .ToCharArray()
            .Select(ParseSegment));

    solution.G.Add(Enum.GetValues<Segment>()
        .Except(solution.A)
        .Except(solution.B)
        .Except(solution.C)
        .Except(solution.D)
        .Except(solution.E)
        .Except(solution.F)
        .Single());

    return solution;
}

static Segment ParseSegment(char input) => input switch
{
    'a' => Segment.A,
    'b' => Segment.B,
    'c' => Segment.C,
    'd' => Segment.D,
    'e' => Segment.E,
    'f' => Segment.F,
    'g' => Segment.G,
    _ => throw new ArgumentOutOfRangeException(nameof(input))
};

enum Segment
{
    A = 0,
    B = 1,
    C = 2,
    D = 3,
    E = 4,
    F = 5,
    G = 6
}

class Solution
{
    public Solution()
    {
        A = new ();
        B = new ();
        C = new ();
        D = new ();
        E = new ();
        F = new ();
        G = new ();
    }

    public HashSet<Segment> A { get; }
    public HashSet<Segment> B { get; }
    public HashSet<Segment> C { get; }
    public HashSet<Segment> D { get; }
    public HashSet<Segment> E { get; }
    public HashSet<Segment> F { get; }
    public HashSet<Segment> G { get; }

    public IEnumerable<Segment> Map(IEnumerable<Segment> segments) =>
        segments.Select(segment => segment switch
        {
            Segment s when A.Single() == s => Segment.A,
            Segment s when B.Single() == s => Segment.B,
            Segment s when C.Single() == s => Segment.C,
            Segment s when D.Single() == s => Segment.D,
            Segment s when E.Single() == s => Segment.E,
            Segment s when F.Single() == s => Segment.F,
            Segment s when G.Single() == s => Segment.G,
            _ => throw new ArgumentOutOfRangeException(nameof(segment))
        });
}

struct Display
{
    private readonly bool A, B, C, D, E, F, G;

    public Display(IEnumerable<Segment> segments)
    {
        A = segments.Contains(Segment.A);
        B = segments.Contains(Segment.B);
        C = segments.Contains(Segment.C);
        D = segments.Contains(Segment.D);
        E = segments.Contains(Segment.E);
        F = segments.Contains(Segment.F);
        G = segments.Contains(Segment.G);
    }

    public char Value => this switch
    {
        Display _ when A && B && C && !D && E && F && G => '0',
        Display _ when !A && !B && C && !D && !E && F && !G => '1',
        Display _ when A && !B && C && D && E && !F && G => '2',
        Display _ when A && !B && C && D && !E && F && G => '3',
        Display _ when !A && B && C && D && !E && F && !G => '4',
        Display _ when A && B && !C && D && !E && F && G => '5',
        Display _ when A && B && !C && D && E && F && G => '6',
        Display _ when A && !B && C && !D && !E && F && !G => '7',
        Display _ when A && B && C && D && E && F && G => '8',
        Display _ when A && B && C && D && !E && F && G => '9',
        _ => throw new InvalidOperationException()
    };
}