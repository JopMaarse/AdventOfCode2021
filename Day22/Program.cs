using InputLogic;
using System.Text.RegularExpressions;

IEnumerable<string> input = await InputHelper.GetInputAsync(day: 22);
Regex ranges = new(@"(?:(?:x|y|z)=([\-0-9]+)\.\.([\-0-9]+)),?");
Instruction[] instructions = input
    .Select(line =>
    {
        MatchCollection matches = ranges.Matches(line);
        return new Instruction(
            TurnOn: line.StartsWith("on"),
            Cuboid: new(
                X: new(int.Parse(matches[0].Groups[1].Value), int.Parse(matches[0].Groups[2].Value)),
                Y: new(int.Parse(matches[1].Groups[1].Value), int.Parse(matches[1].Groups[2].Value)),
                Z: new(int.Parse(matches[2].Groups[1].Value), int.Parse(matches[2].Groups[2].Value))));
    })
    .ToArray();

Dictionary<(int, int, int), bool> cubes = new();
foreach (Instruction instruction in instructions)
{
    for (int x = Math.Max(instruction.Cuboid.X.Min, -50); x <= Math.Min(instruction.Cuboid.X.Max, 50); x++)
        for (int y = Math.Max(instruction.Cuboid.Y.Min, -50); y <= Math.Min(instruction.Cuboid.Y.Max, 50); y++)
            for (int z = Math.Max(instruction.Cuboid.Z.Min, -50); z <= Math.Min(instruction.Cuboid.Z.Max, 50); z++)
                cubes[(x, y, z)] = instruction.TurnOn;
}

Console.WriteLine("Part 1: " + cubes.Values.Count(on => on));

HashSet<Cuboid> cuboids = new();
foreach(Instruction instruction in instructions)
{
    Cuboid? overlap;
    while ((overlap = cuboids.Cast<Cuboid?>().FirstOrDefault(c => c!.Value.Overlaps(instruction.Cuboid))).HasValue)
    {
        cuboids.Remove(overlap.Value);
        cuboids.UnionWith(overlap.Value.Substract(instruction.Cuboid));
    }

    if (instruction.TurnOn)
        cuboids.Add(instruction.Cuboid);
}

Console.WriteLine("Part 2: " + cuboids.Sum(c => c.Size));

record struct Instruction (bool TurnOn, Cuboid Cuboid);

record struct Range(int Min, int Max)
{
    public bool Overlaps(Range range) =>
        (range.Min >= Min && range.Min <= Max) ||
        (range.Max >= Min && range.Max <= Max) ||
        (Min >= range.Min && Min <= range.Max) ||
        (Max >= range.Min && Max <= range.Max);

    public Range Overlap(Range range) =>
        new(Math.Max(range.Min, Min), Math.Min(range.Max, Max));

    public IEnumerable<Range> Divide(Range range)
    {
        if (range.Min <= Min && range.Max >= Max)
        {
            yield return this;
        }
        else
        {
            if (range.Min >= Min)
            {
                if (range.Max < Max)
                {
                    yield return new Range(Min, range.Min - 1);
                    yield return range;
                    yield return new Range(range.Max + 1, Max);
                }
                else
                {
                    yield return new Range(Min, range.Min - 1);
                    yield return new Range(range.Min, Max);
                }
            }
            else
            {
                yield return new Range(Min, range.Max);
                yield return new Range(range.Max + 1, Max);
            }
        }
    }
}

record struct Cuboid(Range X, Range Y, Range Z)
{
    public long Size => (long)(X.Max - X.Min + 1) * (Y.Max - Y.Min + 1) * (Z.Max - Z.Min + 1);

    public bool Overlaps(Cuboid cuboid) =>
        cuboid.X.Overlaps(X) && cuboid.Y.Overlaps(Y) && cuboid.Z.Overlaps(Z);

    public Cuboid Overlap(Cuboid cuboid) =>
        new(X.Overlap(cuboid.X), Y.Overlap(cuboid.Y), Z.Overlap(cuboid.Z));

    public IEnumerable<Cuboid> Substract(Cuboid cuboid)
    {
        Cuboid _this = this;
        return (from x in _this.X.Divide(cuboid.X)
                from y in _this.Y.Divide(cuboid.Y)
                from z in _this.Z.Divide(cuboid.Z)
                select new Cuboid(x, y, z))
                .Where(c => c != _this.Overlap(cuboid));
    }
}
