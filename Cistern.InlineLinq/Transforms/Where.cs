using System.Runtime.CompilerServices;

namespace Cistern.InlineLinq.Transforms;

public struct γWhere<T, TEnumeratorable>
    : IEnumeratorable<T>
    where TEnumeratorable : struct, IEnumeratorable<T>
{
    public TEnumeratorable Inner { get; }
    public Func<T, bool> Predicate { get; }

    public γWhere(TEnumeratorable original, Func<T, bool> predicate) =>
        (Inner, Predicate, _current) = (original, predicate, default);


    private TEnumeratorable _current;
    public void Initialize()
    {
        _current = Inner;
        _current.Initialize();
    }
    public void Dispose()
    {
        _current.Dispose();
        _current = default;
    }


    public int? TryGetCount(out int? upperBound)
    {
        Inner.TryGetCount(out upperBound);
        return default;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TryGetNext(out T current)
    {
        while (_current.TryGetNext(out current))
        {
            if (Predicate(current))
            {
                return true;
            }
        }
        return false;
    }

    public bool TryGetSpan(out ReadOnlySpan<T> span)
    {
        span = default;
        return false;
    }
}
