using InputLogic;

IEnumerable<string> input = await InputHelper.GetInputAsync(day: 5);

IEnumerable<Line> lines = input.Select(ParseLine);
Dictionary<Point, int> numberOfLinesPerPoint = new();

foreach (Line line in lines)
{
    foreach (Point point in line.GetAllCoveredPoints())
    {
        if (numberOfLinesPerPoint.ContainsKey(point))
        {
            numberOfLinesPerPoint[point]++;
        }
        else
        {
            numberOfLinesPerPoint.Add(point, 1);
        }
    }
}

Console.WriteLine(numberOfLinesPerPoint.Values.Count(count => count > 1));

// part 2
numberOfLinesPerPoint.Clear();

foreach (Line line in lines)
{
    foreach (Point point in line.GetAllCoveredPoints(includeDiagonal: true))
    {
        if (numberOfLinesPerPoint.ContainsKey(point))
        {
            numberOfLinesPerPoint[point]++;
        }
        else
        {
            numberOfLinesPerPoint.Add(point, 1);
        }
    }
}

Console.WriteLine(numberOfLinesPerPoint.Values.Count(count => count > 1));


static Line ParseLine(string line)
{    
    Point[] points = line
        .Split(" -> ")
        .Select(ParsePoint)
        .ToArray();

    return new Line(Start: points[0], End: points[1]);
}

static Point ParsePoint(string point)
{
    int[] coords = point
        .Split(',')
        .Select(int.Parse)
        .ToArray();

    return new Point(X: coords[0], Y: coords[1]);
}

record struct Point(int X, int Y);
record struct Line(Point Start, Point End)
{
    public IEnumerable<Point> GetAllCoveredPoints(bool includeDiagonal = false)
    {
        if (Start.X == End.X)
        {
            int xCoord = Start.X;
            return GetCoordinates(Start, End, point => point.Y).Select(yCoord => new Point(xCoord, yCoord));
        }

        if (Start.Y == End.Y)
        {
            int yCoord = Start.Y;
            return GetCoordinates(Start, End, point => point.X).Select(xCoord => new Point(xCoord, yCoord));
        }

        if (!includeDiagonal)
            return Enumerable.Empty<Point>();

        return GetDiagnonals(Start, End);

        static IEnumerable<Point> GetDiagnonals(Point start, Point end)
        {
            int xDirection = Math.Sign(end.X - start.X);
            int yDirection = Math.Sign(end.Y - start.Y);

            int xCoord = start.X;
            int yCoord = start.Y;

            while ((xCoord += xDirection) != end.X && (yCoord += yDirection) != end.Y)
            {
                yield return new Point(xCoord, yCoord);
            }
            yield return end;
        }

        static IEnumerable<int> GetCoordinates(Point start, Point end, Func<Point, int> map) =>
            Enumerable.Range(Math.Min(map(start), map(end)), Math.Abs(map(start) - map(end)) + 1);
    }
}