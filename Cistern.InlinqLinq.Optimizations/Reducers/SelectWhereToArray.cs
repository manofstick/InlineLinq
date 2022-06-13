using Cistern.InlineLinq.Transforms;
using Cistern.InlineLinq.Utils;
using System.Buffers;

namespace Cistern.InlineLinq.Optimizations;

public static partial class Reducers
{
    private static U[] ToArray<T, U>(ReadOnlySpan<T> span, Func<T, bool> predicate, Func<T, U> selector)
    {
        var pool = ArrayPool<U>.Shared;
        var buffer = pool.Rent(span.Length);
        try
        {
            var idx = 0;
            foreach (var item in span)
            {
                if (predicate(item))
                    buffer[idx++] = selector(item);
            }
            return buffer.AsSpan(0, idx).ToArray();
        }
        finally
        {
            pool.Return(buffer);
        }
    }

    private static U[] ToArray<TEnumeratorable, T, U>(TEnumeratorable enumeratorable, Func<T, bool> predicate, Func<T, U> selector, ArrayPool<U>? maybeArrayPool, int? upperBound)
        where TEnumeratorable : struct, IEnumeratorable<T>
    {
        Builder<U>.MemoryChunk memoryChunk = new();
        var builder = new Builder<U>(maybeArrayPool, memoryChunk.GetBufferofBuffers(), memoryChunk.GetBufferOfItems(), upperBound);
        while (enumeratorable.TryGetNext(out var item))
        {
            if (predicate(item))
                builder.Add(selector(item));
        }
        return builder.ToArray();
    }

    public static U[] ToArray<T, U, TEnumeratorable>(this in Enumeratorable<U, γSelect<T, U, γWhere<T, TEnumeratorable>>> source)
        where TEnumeratorable : struct, IEnumeratorable<T>
    {
        var select = source.Inner;
        var where = select.Inner;
        var enumeratorable = where.Inner;

        if (enumeratorable.TryGetNextSpan(out var span))
        {
            if (span.Length == 0)
                return Array.Empty<U>();

            return ToArray(span, where.Predicate, select.Selector);
        }
        else
        {
            var maybeCount = enumeratorable.TryGetCount(out var upperBound);
            if (maybeCount == 0)
                return Array.Empty<U>();

            return ToArray(enumeratorable, where.Predicate, select.Selector, null, upperBound);
        }
    }

}
