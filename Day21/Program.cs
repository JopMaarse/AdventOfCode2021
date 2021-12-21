using Day21;
using InputLogic;

IEnumerable<string> input = await InputHelper.GetInputAsync(day: 21);
int part1 = new Part1().Solve(input);
Console.WriteLine($"Part 1: {part1}");
long part2 = new Part2().Solve(input);
Console.WriteLine($"Part 2: {part2}");
