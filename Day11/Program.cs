using InputLogic;

IEnumerable<string> input = await InputHelper.GetInputAsync(day: 11);

Console.WriteLine("Part 1: " + new Simulation(input.ToArray()).Simulate(steps: 100));
Console.WriteLine("Part 2: " + new Simulation(input.ToArray()).SimulateUntilSynced());

class Simulation
{
    private readonly Octopus[,] _octopusses;

    private int Width => _octopusses.GetLength(0);

    private int Height => _octopusses.GetLength(1);

    public ulong FlashCounter { get; private set; }

    public Simulation(string[] input)
    {
        _octopusses = new Octopus[input[0].Length, input.Length];

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                _octopusses[x, y] = new((int)char.GetNumericValue(input[y][x]));
            }
        }

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                _octopusses[x, y].Flash += (sender, e) => FlashCounter++;
                _octopusses[x, y].SetNeighbours(GetNeighbours(x, y).Select(location => _octopusses[location.X, location.Y]));
            }
        }
    }

    public ulong Simulate(int steps)
    {
        for (int step = 0; step < steps; step++)
        {
            Step();
            Reset();
        }

        return FlashCounter;
    }

    public int SimulateUntilSynced()
    {
        int step = 0;
        
        while (_octopusses.Cast<Octopus>().Where(o => o.Flashed).Count() < _octopusses.Length)
        {
            step++;
            Reset();
            Step();
        }

        return step;
    }

    private void Step()
    {
        foreach (Octopus octopus in _octopusses)
        {
            octopus.IncreaseEnergy();
        }
    }

    private void Reset()
    {
        foreach (Octopus octopus in _octopusses)
        {
            octopus.Flashed = false;
        }
    }

    private IEnumerable<(int X, int Y)> GetNeighbours(int x, int y)
    {
        if (x > 0 && y > 0)
            yield return (x - 1, y - 1);

        if (y > 0)
            yield return (x, y - 1);

        if (x < Width - 1 && y > 0)
            yield return (x + 1, y - 1);

        if (x > 0)
            yield return (x - 1, y);

        if (x < Width - 1)
            yield return (x + 1, y);

        if (x > 0 && y < Height - 1)
            yield return (x - 1, y + 1);

        if (y < Height - 1)
            yield return (x, y + 1);

        if (x < Width - 1 && y < Height - 1)
            yield return (x + 1, y + 1);
    }
}

class Octopus
{
    private const int energyThreshold = 10;    
    private int energy;

    public bool Flashed { get; set; }

    public Octopus(int energy)
    {
        this.energy = energy;
        Flash += (sender, e) => OnFlash();
    }

    public void IncreaseEnergy()
    {
        if (Flashed)
            return;
        
        energy++;

        if (energy >= energyThreshold)
            Flash.Invoke(this, EventArgs.Empty);
    }

    public void SetNeighbours(IEnumerable<Octopus> neighbours)
    {        
        foreach (Octopus neighbour in neighbours)
        {
            neighbour.Flash += (sender, e) => IncreaseEnergy();
        }
    }

    private void OnFlash()
    {
        Flashed = true;
        energy = 0;
    }

    public event EventHandler Flash;
}