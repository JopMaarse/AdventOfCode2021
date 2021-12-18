namespace Day15;

public class BinaryMinHeap<TKey, TValue> where TKey : IEquatable<TKey> where TValue : struct
{
    private readonly KeyValuePair<TKey, TValue>[] _elements;
    private readonly IComparer<KeyValuePair<TKey, TValue>> _comparer;
    private readonly Dictionary<TKey, int> _indices;

    public int Count => _indices.Count;

    public BinaryMinHeap(int size, IComparer<KeyValuePair<TKey, TValue>> comparer)
    {
        _elements = new KeyValuePair<TKey, TValue>[size];
        _comparer = comparer;
        _indices = new();
    }

    public TValue? this[TKey key]
    {
        get => _indices.ContainsKey(key) ? _elements[_indices[key]].Value : default(TValue?);
        set
        {
            KeyValuePair<TKey, TValue> newValue = new(key, value!.Value);
            if (_indices.ContainsKey(key))
                _elements[_indices[key]] = newValue;
            else
                Insert(newValue);
        }
    }

    public TValue GetValueOrDefault(TKey key, TValue defaulValue = default!) =>
        this[key] ?? defaulValue;

    public KeyValuePair<TKey, TValue> ExtractMin()
    {
        if (Count < 0)
            throw new Exception("Cannot extract from empty heap.");
        KeyValuePair<TKey, TValue> value = _elements[0];
        _indices.Remove(value.Key);
        _elements[0] = _elements[Count];
        MinHeapify(0);
        return value;
    }

    public void Insert(KeyValuePair<TKey, TValue> element)
    {
        int i = Count;
        _elements[i] = element;
        _indices[element.Key] = i;
        while (i > 0 && _comparer.Compare(_elements[Parent(i)], _elements[i]) > 0)
        {
            Swap(i, Parent(i));
            i = Parent(i);
        }
    }

    private static int Parent(int i) => (i + 1) >> 1;
    
    private static int Left(int i) => (i << 1) + 1;
    
    private static int Right(int i) => (i << 1) + 2;

    private void Swap(int i, int j)
    {
        KeyValuePair<TKey, TValue> temp = _elements[i];
        _elements[i] = _elements[j];
        _elements[j] = temp;
        _indices[_elements[i].Key] = i;
        _indices[_elements[j].Key] = j;
    }
    
    private void MinHeapify(int i)
    {
        int smallest;
        int l = Left(i);
        int r = Right(i);
        if (l < Count && _comparer.Compare(_elements[l], _elements[i]) < 0)
            smallest = l;
        else
            smallest = i;
        if (r < Count && _comparer.Compare(_elements[r], _elements[smallest]) < 0)
            smallest = r;
        if (smallest != i)
        {
            Swap(i, smallest);
            MinHeapify(smallest);
        }
    }
}

