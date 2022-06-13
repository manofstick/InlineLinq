using Cistern.InlineLinq.Transforms;
using Cistern.InlineLinq.Utils;
using System.Buffers;

namespace Cistern.InlineLinq.Optimizations;

public static partial class Reducers
{
    class SelectWhereToArray
    {
        private static U[] PopulateViaBuilder<TEnumeratorable, T, U>(ref TEnumeratorable enumeratorable, Func<T, bool> predicate, Func<T, U> selector, ArrayPool<U>? maybeArrayPool, int? upperBound)
            where TEnumeratorable : struct, IEnumeratorable<T>
        {
            Builder<U>.MemoryChunk memoryChunk = new();
            var builder = new Builder<U>(maybeArrayPool, memoryChunk.GetBufferofBuffers(), memoryChunk.GetBufferOfItems(), upperBound);
            try
            {
                if (enumeratorable.TryGetNextSpan(out var span))
                {
                    PopulateBuilderViaTryGetNextSpan(ref enumeratorable, ref builder, predicate, selector, span);
                }
                else
                {
                    PopulateBuilderViaTryGetNext(ref enumeratorable, ref builder, predicate, selector);
                }

                return builder.ToArray();
            }
            finally
            {
                builder.Dispose();
            }
        }
        private static void PopulateBuilderViaTryGetNext<TEnumeratorable, T, U>(ref TEnumeratorable enumeratorable, ref Builder<U> builder, Func<T, bool> predicate, Func<T, U> selector) where TEnumeratorable : struct, IEnumeratorable<T>
        {
            while (enumeratorable.TryGetNext(out var item))
            {
                if (predicate(item))
                    builder.Add(selector(item));
            }
        }

        private static void PopulateBuilderViaTryGetNextSpan<TEnumeratorable, T, U>(ref TEnumeratorable enumeratorable, ref Builder<U> builder, Func<T, bool> predicate, Func<T, U> selector, ReadOnlySpan<T> span)
            where TEnumeratorable : struct, IEnumeratorable<T>
        {
            do
            {
                for(var i=0; i < span.Length; ++i)
                {
                    if (predicate(span[i]))
                        builder.Add(selector(span[i]));
                }
            } while (enumeratorable.TryGetNextSpan(out span));
        }

        public static U[] ToArray<T, U, TEnumeratorable>(in Enumeratorable<U, γSelect<T, U, γWhere<T, TEnumeratorable>>> source, ArrayPool<U>? maybeArrayPool)
            where TEnumeratorable : struct, IEnumeratorable<T>
        {
            var select = source.Inner;
            var where = select.Inner;
            var enumeratorable = where.Inner;

            enumeratorable.Initialize();
            try
            {
                var maybeCount = enumeratorable.TryGetCount(out var upperBound);
                if (maybeCount == 0)
                    return Array.Empty<U>();

                return PopulateViaBuilder(ref enumeratorable, where.Predicate, select.Selector, maybeArrayPool, upperBound);
            }
            finally
            {
                enumeratorable.Dispose();
            }
        }
    }

    public static U[] ToArray<T, U, TEnumeratorable>(this in Enumeratorable<U, γSelect<T, U, γWhere<T, TEnumeratorable>>> source, ArrayPool<U>? maybeArrayPool)
        where TEnumeratorable : struct, IEnumeratorable<T> => SelectWhereToArray.ToArray(in source, maybeArrayPool);

    public static U[] ToArray<T, U, TEnumeratorable>(this in Enumeratorable<U, γSelect<T, U, γWhere<T, TEnumeratorable>>> source, bool usePool)
        where TEnumeratorable : struct, IEnumeratorable<T> => ToArray(in source, usePool ? ArrayPool<U>.Shared : null);

    public static U[] ToArray<T, U, TEnumeratorable>(this in Enumeratorable<U, γSelect<T, U, γWhere<T, TEnumeratorable>>> source)
        where TEnumeratorable : struct, IEnumeratorable<T> => ToArray(in source, false);
}
