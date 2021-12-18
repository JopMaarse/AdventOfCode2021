namespace Day15;

public class Dijkstra<T> where T : notnull, IEquatable<T>
{
    private readonly Func<T, IEnumerable<T>> _edges;
    private readonly Func<T, T, uint> _weight;

    Dictionary<T, uint> Distances { get; }
    BinaryMinHeap<T, uint> Remaining { get; }

    public Dijkstra(T source, Func<T, IEnumerable<T>> edges, Func<T, T, uint> weight, int size)
    {
        Remaining = new(size, new Comparer());
        Distances = new();
        Distances[source] = 0;
        _edges = edges;
        _weight = weight;
        SetAdjacentEstimates(source);
    }

    public uint GetDistance(T destination)
    {
        while(!Distances.ContainsKey(destination))
        {
            (T vertex, uint distance) = Remaining.ExtractMin();
            Distances[vertex] = distance;
            SetAdjacentEstimates(vertex);
        }

        return Distances[destination];
    }

    private void SetAdjacentEstimates(T vertex)
    {
        foreach(T adjacent in _edges(vertex).Where(v => !Distances.ContainsKey(v)))
        {
            Remaining[adjacent] = Math.Min(
                Remaining[adjacent] ?? uint.MaxValue,
                Distances[vertex] + _weight(vertex, adjacent));
        }
    }

    private class Comparer : IComparer<KeyValuePair<T, uint>>
    {
        int IComparer<KeyValuePair<T, uint>>.Compare(KeyValuePair<T, uint> x, KeyValuePair<T, uint> y) =>
            x.Value.CompareTo(y.Value);
    }
}
