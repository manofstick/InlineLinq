using System.Runtime.CompilerServices;

namespace Cistern.InlineLinq.Transforms;

public struct γSkip<T, TEnumeratorable>
    : IEnumeratorable<T>
    where TEnumeratorable : struct, IEnumeratorable<T>
{
    public TEnumeratorable Inner { get; }
    public int ToSkip { get; }

    public γSkip(TEnumeratorable original, int toSkip) =>
        (Inner, ToSkip, _current, _remaining) = (original, toSkip, default, int.MinValue);

    private TEnumeratorable _current;
    private int _remaining;

    public void Initialize()
    {
        _remaining = ToSkip;
        _current = Inner;
        _current.Initialize();
    }
    public void Dispose()
    {
        _current.Dispose();
        _current = default;
        _remaining = int.MaxValue;
    }

    public int? TryGetCount(out int? upperBound)
    {
        var count = Inner.TryGetCount(out upperBound);

        count      = count.HasValue      ? Math.Max(0, count.Value - ToSkip)      : null;
        upperBound = upperBound.HasValue ? Math.Max(0, upperBound.Value - ToSkip) : null;

        return count;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TryGetNext(out T current)
    {
        while (_remaining > 0)
        {
            if (!_current.TryGetNext(out current))
                return false;
            --_remaining;
        }
        return _current.TryGetNext(out current);
    }

    public bool TryGetNextSpan(out ReadOnlySpan<T> span)
    {
        if (!_current.TryGetNextSpan(out span))
            return false;

        if (span.Length <= _remaining)
        {
            _remaining -= span.Length;
            if (TryGetNextSpan(out span))
                return true;
            span = ReadOnlySpan<T>.Empty;
            return true;
        }

        span = span[_remaining..];
        _remaining = 0;

        return true;
    }
}

