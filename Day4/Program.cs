using InputLogic;
using System.Collections;

IEnumerable<string> input = await InputHelper.GetInputAsync(day: 4);

int[] numbers = input
    .First()
    .Split(',')
    .Select(int.Parse)
    .ToArray();

List<Board> boards = new();
input = input.Skip(1);
while (input.Any())
{
    boards.Add(new(input
        .Take(Board.Size)
        .Select(row => row
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Select(int.Parse)
            .ToArray())
        .ToArray()));

    input = input.Skip(Board.Size);
}

int? part1 = null;
int part2 = default;

foreach (int number in numbers)
{
    foreach (Board board in boards)
    {
        if (!board.IsVictorious && board.MarkNumber(number, out int score))
        {
            part1 ??= score;
            part2 = score;
        }
    }
}

Console.WriteLine($"The answer to part 1 is: {part1}");
Console.WriteLine($"The answer to part 2 is: {part2}");

record class Square(int Number)
{
    public bool Marked { get; set; }
}

class Board : IEnumerable<Square>
{
    private readonly Square[,] squares;

    public const int Size = 5;

    public bool IsVictorious { get; private set; }

    public Board(int[][] numbers)
    {
        squares = new Square[Size, Size];

        for (int i = 0; i < Size; i++)
        {
            for (int j = 0; j < Size; j++)
            {
                squares[i, j] = new(numbers[i][j]);
            }
        }
    }

    public bool MarkNumber(int number, out int score)
    {
        score = default;
        foreach (Square square in this)
        {
            if (square.Number == number)
                square.Marked = true;
        }

        IsVictorious = CheckVictory();
        if (IsVictorious)
        {
            checked
            {
                score = GetStaticScore() * number;
            }
        }

        return IsVictorious;
    }

    public IEnumerator<Square> GetEnumerator()
    {
        for (int i = 0; i < Size; i++)
        {
            for (int j = 0; j < Size; j++)
            {
                yield return squares[i, j];
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    private IEnumerable<IEnumerable<Square>> Rows()
    {
        for (int row = 0; row < Size; row++)
        {
            yield return Enumerable
                .Range(0, Size)
                .Select(column => squares[row, column]);
        }
    }

    private IEnumerable<IEnumerable<Square>> Columns()
    {
        for (int column = 0; column < Size; column++)
        {
            yield return Enumerable
                .Range(0, Size)
                .Select(row => squares[row, column]);
        }
    }

    private bool CheckVictory()
    {
        foreach(IEnumerable<Square> row in Rows())
        {
            if (row.All(square => square.Marked))
                return true;
        }
        
        foreach(IEnumerable<Square> column in Columns())
        {
            if (column.All(square => square.Marked))
                return true;
        }

        return false;
    }

    private int GetStaticScore() =>
        this
            .Where(square => !square.Marked)
            .Sum(square => square.Number);
}