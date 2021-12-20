using InputLogic;

IEnumerable<string> input = await InputHelper.GetInputAsync(day: 20);
string imageEnhancementAlgorithm = input.First();
string[] image = input.Skip(1).ToArray();
const int passesPart1 = 2;
const int passesPart2 = 50;
bool flash = imageEnhancementAlgorithm[0] == '#' && imageEnhancementAlgorithm[^1] == '.';
int part1 = LitPixelsAfterPasses(start: 0, end: passesPart1);
Console.WriteLine("Part 1: " + part1);
int part2 = LitPixelsAfterPasses(start: passesPart1, end: passesPart2);
Console.WriteLine("Part 1: " + part2);

int LitPixelsAfterPasses(int start, int end)
{
    for (int i = start; i < end; i++)
        image = ProcessImage(imageEnhancementAlgorithm, image, padWithLit: flash && (i & 1) == 1);
    
    return image.Sum(line => line.Count(c => c == '#'));
}

static string[] ProcessImage(string imageEnhancementAlgorithm, string[] image, bool padWithLit)
{
    char pad = padWithLit ? '#' : '.';

    image = image
        .Prepend(new(pad, image[0].Length))
        .Append(new(pad, image[0].Length))
        .Select(line => $"{pad}{line}{pad}")
        .ToArray();

    return image
        .Select((line, y) => line.Aggregate(string.Empty, (aggregate, _) => aggregate + imageEnhancementAlgorithm[ConvoluteKernel(image, (aggregate.Length, y), pad)]))
        .ToArray();
}

static int ConvoluteKernel(string[] image, (int X, int Y) pixel, char pad)
{
    string read = 
        (pixel.Y == 0 ? new(pad, 3) : ReadPixels(image[pixel.Y - 1], pixel.X, pad)) +
        ReadPixels(image[pixel.Y], pixel.X, pad) +
        (pixel.Y == image.Length - 1 ? new(pad, 3) : ReadPixels(image[pixel.Y + 1], pixel.X, pad));

    return Convert.ToInt32(
        read.Replace('.', '0')
            .Replace('#', '1'),
        fromBase: 2);
}

static string ReadPixels(string line, int x, char pad)
{
    if (x == 0)
        return pad + line[x..(x + 2)];
    if (x + 1 == line.Length)
        return line[(x - 1)..(x + 1)] + pad;
    return line[(x - 1)..(x + 2)];
}