
namespace Day21;
internal class Part1
{
    public int Solve(IEnumerable<string> input)
    {
        Player[] players =
        {
            new Player{ Position = ParseStartingPosition(input.First()) },
            new Player{ Position = ParseStartingPosition(input.Last()) }
        };

        IDie die = new DeterministicDie(d: 100);
        const int rollsPerTurn = 3, winningScore = 1000, boardSize = 10;
        int turn = 0;
        while (players.All(p => p.Score < winningScore))
        {
            int roll = Enumerable.Range(0, rollsPerTurn).Sum(_ => die.Roll());
            players[turn].Position = (players[turn].Position + roll) % boardSize;
            players[turn].Score += players[turn].Position + 1;
            turn = (turn + 1) % players.Length;
        }

        return players[turn].Score * die.RollCounter;

        static int ParseStartingPosition(string input) => (int)(char.GetNumericValue(input[^1]) - 1);
    }

    interface IDie
    {
        int Roll();
        int RollCounter { get; }
    }

    class DeterministicDie : IDie
    {
        private int face = 0;
        private readonly int _d;

        public int RollCounter { get; private set; }

        public DeterministicDie(int d)
        {
            _d = d;
        }

        public int Roll()
        {
            RollCounter++;
            int roll = face + 1;
            face = (face + 1) % _d;
            return roll;
        }
    }

    record Player
    {
        public int Position { get; set; }
        public int Score { get; set; }
    }
}