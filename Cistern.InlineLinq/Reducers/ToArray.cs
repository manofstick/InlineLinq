using Cistern.InlineLinq.Utils;
using System.Buffers;

namespace Cistern.InlineLinq;

public static partial class Reducers
{
    class VanillaToArray
    {
        private static T[] KnownSize<TEnumeratorable, T>(ref TEnumeratorable enumeratorable, int count)
            where TEnumeratorable : struct, IEnumeratorable<T>
        {
            var result = new T[count];
            if (enumeratorable.TryGetNextSpan(out var span))
                KnownSizeWithSpan(ref enumeratorable, result, span);
            else
                KnownSizeByElement(ref enumeratorable, result);
            return result;
        }

        private static void KnownSizeByElement<TEnumeratorable, T>(ref TEnumeratorable enumeratorable, T[] result)
            where TEnumeratorable : struct, IEnumeratorable<T>
        {
            var idx = 0;
            while (enumeratorable.TryGetNext(out var item))
            {
                result[idx++] = item;
            }
        }

        private static void KnownSizeWithSpan<TEnumeratorable, T>(ref TEnumeratorable enumeratorable, T[] result, ReadOnlySpan<T> span)
            where TEnumeratorable : struct, IEnumeratorable<T>
        {
            var ptr = result.AsSpan();
            do
            {
                span.CopyTo(ptr);
                ptr = ptr[span.Length..];
            } while (enumeratorable.TryGetNextSpan(out span));
        }

        private static T[] UseBuilder<TEnumeratorable, T>(ref TEnumeratorable enumeratorable, ArrayPool<T>? maybePool, int? upperBound)
            where TEnumeratorable : struct, IEnumeratorable<T>
        {
            var stackallocation = new Builder<T>.MemoryChunk();
            using var builder = new Builder<T>(maybePool, stackallocation.GetBufferofBuffers(), stackallocation.GetBufferOfItems(), upperBound);

            while (enumeratorable.TryGetNext(out var item))
                builder.Add(item);

            return builder.ToArray();
        }

        public static T[] ToArray<T, TEnumeratorable>(in Enumeratorable<T, TEnumeratorable> source, ArrayPool<T>? maybePool)
            where TEnumeratorable : struct, IEnumeratorable<T>
        {
            var enumeratorable = source.Inner;

            enumeratorable.Initialize();
            try
            {
                var maybeCount = enumeratorable.TryGetCount(out var upperBound);
                if (maybeCount.HasValue)
                {
                    var count = maybeCount.Value;
                    if (count == 0)
                        return Array.Empty<T>();

                    return KnownSize<TEnumeratorable, T>(ref enumeratorable, count);
                }

                return UseBuilder(ref enumeratorable, maybePool, upperBound);
            }
            finally
            {
                enumeratorable.Dispose();
            }
        }
    }

    public static T[] ToArray<T, TEnumeratorable>(this in Enumeratorable<T, TEnumeratorable> source, ArrayPool<T>? maybePool)
        where TEnumeratorable : struct, IEnumeratorable<T>
        => VanillaToArray.ToArray(in source, maybePool);

    public static T[] ToArray<T, TEnumeratorable>(this in Enumeratorable<T, TEnumeratorable> source, bool usePool = false)
        where TEnumeratorable : struct, IEnumeratorable<T>
        => ToArray(in source, usePool ? ArrayPool<T>.Shared : null);
}
