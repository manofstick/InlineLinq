using System.Runtime.CompilerServices;

namespace Cistern.InlineLinq.Transforms;

public struct γSelect<T, U, TEnumeratorable>
    : IEnumeratorable<U>
    where TEnumeratorable : struct, IEnumeratorable<T>
{
    public TEnumeratorable Inner { get; }
    public Func<T, U> Selector { get; }

    public γSelect(TEnumeratorable original, Func<T, U> selector) =>
        (Inner, Selector, _current) = (original, selector, default);


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

    public int? TryGetCount(out int? count) => Inner.TryGetCount(out count);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TryGetNext(out U current)
    {
        if (_current.TryGetNext(out var t))
        {
            current = Selector(t);
            return true;
        }
        current = default!;
        return false;
    }

    public bool TryGetNextSpan(out ReadOnlySpan<U> span)
    {
        span = default;
        return false;
    }
}

