using Cistern.InlineLinq.Transforms;
using Cistern.InlineLinq.Utils;
using System.Buffers;

namespace Cistern.InlineLinq.Optimizations;

public static partial class Reducers
{
    class SelectToArray
    {
        private static U[] ToArray<TEnumeratorable, T, U>(ref TEnumeratorable enumeratorable, Func<T, U> selector, ArrayPool<U>? maybeArrayPool, int? upperBound)
            where TEnumeratorable : struct, IEnumeratorable<T>
        {
            Builder<U>.MemoryChunk memoryChunk = new();
            using var builder = new Builder<U>(maybeArrayPool, memoryChunk.GetBufferofBuffers(), memoryChunk.GetBufferOfItems(), upperBound);
            while (enumeratorable.TryGetNext(out var item))
            {
                builder.Add(selector(item));
            }
            return builder.ToArray();
        }

        private static U[] ToArray<TEnumeratorable, T, U>(ref TEnumeratorable enumeratorable, U[] result, Func<T, U> selector)
            where TEnumeratorable : struct, IEnumeratorable<T>
        {
            var idx = 0;
            while (enumeratorable.TryGetNext(out var item))
            {
                result[idx++] = selector(item);
            }
            return result;
        }

        private static U[] ToArray<TEnumeratorable, T, U>(ref TEnumeratorable enumeratorable, U[] result, Func<T, U> selector, ReadOnlySpan<T> span)
            where TEnumeratorable : struct, IEnumeratorable<T>
        {
            var idx = 0;
            do
            {
                for(var i=0; i < span.Length; ++i)
                    result[idx++] = selector(span[i]);
            } while (enumeratorable.TryGetNextSpan(out span));

            return result;
        }

        private static U[] ToArray<TEnumeratorable, T, U>(ref TEnumeratorable enumeratorable, Func<T, U> selector, int count)
            where TEnumeratorable : struct, IEnumeratorable<T>
        {
            var result = new U[count];
            if (!enumeratorable.TryGetNextSpan(out var span))
                return ToArray(ref enumeratorable, result, selector);

            return ToArray(ref enumeratorable, result, selector, span);
        }

        public static U[] ToArray<T, U, TEnumeratorable>(in Enumeratorable<U, γSelect<T, U, TEnumeratorable>> source)
            where TEnumeratorable : struct, IEnumeratorable<T>
        {
            var select = source.Inner;
            var enumeratorable = select.Inner;

            enumeratorable.Initialize();
            try
            {
                var maybeCount = enumeratorable.TryGetCount(out var upperBound);
                if (maybeCount.HasValue)
                {
                    var count = maybeCount.Value;
                    if (count == 0)
                        return Array.Empty<U>();

                    return ToArray(ref enumeratorable, select.Selector, count);
                }
                else
                {
                    return ToArray(ref enumeratorable, select.Selector, null, upperBound);
                }
            }
            finally
            {
                enumeratorable.Dispose();
            }
        }
    }

    public static U[] ToArray<T, U, TEnumeratorable>(this in Enumeratorable<U, γSelect<T, U, TEnumeratorable>> source, ArrayPool<U>? _/*ignore*/)
        where TEnumeratorable : struct, IEnumeratorable<T> => SelectToArray.ToArray(in source);

    public static U[] ToArray<T, U, TEnumeratorable>(this in Enumeratorable<U, γSelect<T, U, TEnumeratorable>> source, bool _/*ignore*/)
        where TEnumeratorable : struct, IEnumeratorable<T> => SelectToArray.ToArray(in source);

    public static U[] ToArray<T, U, TEnumeratorable>(this in Enumeratorable<U, γSelect<T, U, TEnumeratorable>> source)
        where TEnumeratorable : struct, IEnumeratorable<T> => SelectToArray.ToArray(in source);
}
