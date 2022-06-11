using System.Runtime.CompilerServices;

namespace Cistern.InlineLinq.Transforms;

public struct γSelectIndexed<T, U, TEnumeratorable>
    : IEnumeratorable<U>
    where TEnumeratorable : struct, IEnumeratorable<T>
{
    public TEnumeratorable Inner { get; }
    public Func<T, int, U> Selector { get; }

    public γSelectIndexed(TEnumeratorable original, Func<T, int, U> selector) =>
        (Inner, Selector, _current, _index) = (original, selector, default, int.MinValue);


    private TEnumeratorable _current;
    private int _index;
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

    public int? TryGetCount(out int? count) => Inner.TryGetCount(out count);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TryGetNext(out U current)
    {
        if (_current.TryGetNext(out var t))
        {
            current = Selector(t, _index++);
            return true;
        }
        current = default!;
        return false;
    }

    public bool TryGetSpan(out ReadOnlySpan<U> span)
    {
        span = default;
        return false;
    }
}
