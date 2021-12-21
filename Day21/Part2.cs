namespace Day21;

internal class Part2
{
    public long Solve(IEnumerable<string> input)
    {
        Dictionary<GameState, Vector> universes = new();
        GameState root = new()
        {
            PlayerOnePosition = ParseStartingPosition(input.First()),
            PlayerTwoPosition = ParseStartingPosition(input.Last()),
            IsPlayerOneTurn = true
        };

        const int winningScore = 21, boardSize = 10;
        (int total, int universeSplits)[] diracRolls = { (3, 1), (4, 3), (5, 6), (6, 7), (7, 6), (8, 3), (9, 1) };
        PlayTurn(root);
        return Math.Max(universes[root].PlayerOneWins, universes[root].PlayerTwoWins);

        static int ParseStartingPosition(string input) => (int)(char.GetNumericValue(input[^1]) - 1);

        void PlayTurn(GameState state)
        {
            foreach ((int total, int universeSplits) in diracRolls)
            {
                GameState child = state;
                child.IsPlayerOneTurn = !state.IsPlayerOneTurn;
                if (state.IsPlayerOneTurn)
                {
                    child.PlayerOnePosition = (child.PlayerOnePosition + total) % boardSize;
                    child.PlayerOneScore += child.PlayerOnePosition + 1;
                }
                else
                {
                    child.PlayerTwoPosition = (child.PlayerTwoPosition + total) % boardSize;
                    child.PlayerTwoScore += child.PlayerTwoPosition + 1;
                }

                if (!universes.ContainsKey(child))
                {
                    if (child.PlayerOneScore >= winningScore || child.PlayerTwoScore >= winningScore)
                    {
                        AddOrIncrement(universes, state, state.IsPlayerOneTurn ? new(universeSplits, 0) : new(0, universeSplits));
                        continue;
                    }

                    PlayTurn(child);
                }

                AddOrIncrement(universes, state, universes[child] * universeSplits);
            }
        }
    }

    static void AddOrIncrement<TKey>(Dictionary<TKey, Vector> dictionary, TKey key, Vector value) where TKey : notnull
    {
        if (!dictionary.ContainsKey(key))
            dictionary.Add(key, value);
        else
            dictionary[key] += value;
    }

    record struct GameState
    {
        public int PlayerOneScore { get; set; }
        public int PlayerTwoScore { get; set; }
        public int PlayerOnePosition { get; set; }
        public int PlayerTwoPosition { get; set; }
        public bool IsPlayerOneTurn { get; set; }
    }

    record struct Vector
    {
        public long PlayerOneWins { get; set; }
        public long PlayerTwoWins { get; set; }

        public Vector(long playerOneWins, long playerTwoWins)
        {
            PlayerOneWins = playerOneWins;
            PlayerTwoWins = playerTwoWins;
        }

        public static Vector operator +(Vector a, Vector b) => new()
        {
            PlayerOneWins = a.PlayerOneWins + b.PlayerOneWins,
            PlayerTwoWins = a.PlayerTwoWins + b.PlayerTwoWins
        };

        public static Vector operator +(Vector a, long value) => new()
        {
            PlayerOneWins = a.PlayerOneWins + value,
            PlayerTwoWins = a.PlayerTwoWins + value
        };

        public static Vector operator *(Vector a, Vector b) => new()
        {
            PlayerOneWins = a.PlayerOneWins * b.PlayerOneWins,
            PlayerTwoWins = a.PlayerTwoWins * b.PlayerTwoWins
        };

        public static Vector operator *(Vector a, long scalar) => new()
        {
            PlayerOneWins = a.PlayerOneWins * scalar,
            PlayerTwoWins = a.PlayerTwoWins * scalar
        };

        public void Deconstruct(out long playerOne, out long playerTwo)
        {
            playerOne = PlayerOneWins;
            playerTwo = PlayerTwoWins;
        }
    }
}
