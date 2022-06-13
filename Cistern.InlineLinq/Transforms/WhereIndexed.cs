using System.Runtime.CompilerServices;

namespace Cistern.InlineLinq.Transforms;

public struct γWhereIndexed<T, TEnumeratorable>
    : IEnumeratorable<T>
    where TEnumeratorable : struct, IEnumeratorable<T>
{
    public TEnumeratorable Inner { get; }
    public Func<T, int, bool> Predicate { get; }

    public γWhereIndexed(TEnumeratorable original, Func<T, int, bool> predicate) =>
        (Inner, Predicate, _current, _index) = (original, predicate, default, int.MinValue);


    private TEnumeratorable _current;
    int _index;
    public void Initialize()
    {
        _index = 0;
        _current = Inner;
        _current.Initialize();
    }
    public void Dispose()
    {
        _current.Dispose();
        _current = default;
        _index = int.MinValue;
    }


    public int? TryGetCount(out int? upperBound)
    {
        Inner.TryGetCount(out upperBound);
        return upperBound == 0 ? 0 : null;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TryGetNext(out T current)
    {
        while (_current.TryGetNext(out current))
        {
            if (Predicate(current, _index++))
            {
                return true;
            }
        }
        return false;
    }

    public bool TryGetNextSpan(out ReadOnlySpan<T> span)
    {
        span = default;
        return false;
    }
}
