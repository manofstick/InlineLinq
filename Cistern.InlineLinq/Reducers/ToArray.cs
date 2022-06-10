using Cistern.InlineLinq.Utils;
using System.Buffers;

namespace Cistern.InlineLinq;

public static partial class Reducers
{
    private static T[] ToArray<TEnumeratorable, T>(in TEnumeratorable enumeratorable, int count)
        where TEnumeratorable : struct, IEnumeratorable<T>
    {
        var result = new T[count];
        var idx = 0;

        var e = enumeratorable;
        e.Initialize();
        try
        {
            while (e.TryGetNext(out var item))
            {
                result[idx++] = item;
            }
            return result;
        }
        finally
        {
            e.Dispose();
        }
    }

    private static T[] ToArray<TEnumeratorable, T>(in TEnumeratorable enumeratorable, ArrayPool<T>? maybePool, int? upperBound)
        where TEnumeratorable : struct, IEnumeratorable<T>
    {
        var stackallocation = new Builder<T>.MemoryChunk();

        var e = enumeratorable;
        e.Initialize();
        try
        {
            using var builder = new Builder<T>(maybePool, stackallocation.GetBufferofBuffers(), stackallocation.GetBufferOfItems(), upperBound);

            while (e.TryGetNext(out var item))
                builder.Add(item);

            return builder.ToArray();
        }
        finally
        {
            e.Dispose();
        }
    }

    public static T[] ToArray<T, TEnumeratorable>(this in Enumeratorable<T, TEnumeratorable> source, ArrayPool<T>? maybePool)
        where TEnumeratorable : struct, IEnumeratorable<T>
    {
        var enumeratorable = source.Inner;

        var maybeCount = enumeratorable.TryGetCount(out var upperBound);
        if (maybeCount.HasValue)
        {
            var count = maybeCount.Value;
            if (count == 0)
                return Array.Empty<T>();

            return ToArray<TEnumeratorable, T>(in enumeratorable, count);
        }

        return ToArray(in enumeratorable, maybePool, upperBound);
    }

    public static T[] ToArray<T, TEnumeratorable>(this in Enumeratorable<T, TEnumeratorable> source, bool usePool = false)
        where TEnumeratorable : struct, IEnumeratorable<T>
        => source.ToArray(usePool ? ArrayPool<T>.Shared : null);
}
