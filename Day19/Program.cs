using InputLogic;
using System.Text.RegularExpressions;

IEnumerable<string> input = await InputHelper.GetInputAsync(day: 19);

List<Scanner> scanners = new();
List<Beacon> beacons = new();
Parse();
InitScannerZero();

void Parse()
{
    Regex scannerPattern = new(@"--- scanner ([0-9]+) ---");
    int scannerId = -1;
    foreach (string reading in input)
    {
        Match scannerMatch = scannerPattern.Match(reading);
        if (scannerMatch.Success)
        {
            scannerId = int.Parse(scannerMatch.Groups[1].Value);
            scanners.Add(new Scanner { Id = scannerId });
            continue;
        }

        int[] coordinates = reading
            .Split(',')
            .Select(coordinate => int.Parse(coordinate))
            .ToArray();

        Vector location = new(coordinates[0], coordinates[1], coordinates[2]);
        beacons.Add(new Beacon { ScannerId = scannerId, RelativeLocation = location });
    }
}

void InitScannerZero()
{
    Scanner scannerZero = scanners.First(s => s.Id == 0);
    scannerZero.Rotation = Rotation.Zero;
    scannerZero.Direction = Direction.Forward;
    foreach (Beacon beacon in beacons.Where(b => b.ScannerId == 0))
    {
        beacon.AbsoluteLocation = beacon.RelativeLocation;
    }
}

class Scanner
{
    public int Id { get; set; }
    public Vector? Location { get; set; }
    public Direction? Direction { get; set; }
    public Rotation? Rotation { get; set; }
}

class Beacon
{
    public int ScannerId { get; set; }
    public Vector? RelativeLocation { get; set; }
    public Vector? AbsoluteLocation { get; set; }
}

record struct Vector(int X, int Y, int Z)
{
    public Vector Translate(Direction direction, Rotation rotation) => Turn(Rotate(this, rotation), direction);

    public static Vector operator -(Vector a, Vector b) => new(a.X - b.X, a.Y - b.Y, a.Z - b.Z);

    private static Vector Turn(Vector vector, Direction direction) => direction switch
    {
        Direction.Forward => vector,
        Direction.Backward => new(-vector.X, vector.Y, vector.Z),
        Direction.Left => new(vector.Y, -vector.X, vector.Z),
        Direction.Right => new(-vector.Y, vector.X, vector.Z),
        Direction.Up => new(-vector.Z, vector.Y, vector.X),
        Direction.Down => new(vector.Z, vector.Y, -vector.X),
        _ => throw new Exception(),
    };

    private static Vector Rotate(Vector vector, Rotation rotation) => rotation switch
    {
        Rotation.Zero => vector,
        Rotation.Quater => new(vector.X, vector.Z, -vector.Y),
        Rotation.Half => new(vector.X, vector.Y, -vector.Z),
        Rotation.ThreeQuater => new(vector.X, -vector.Z, vector.Y),
        _ => throw new Exception()
    };
}

enum Direction
{
    Forward,
    Backward,
    Left,
    Right,
    Up,
    Down
}

enum Rotation
{
    Zero,
    Quater,
    Half,
    ThreeQuater    
}
