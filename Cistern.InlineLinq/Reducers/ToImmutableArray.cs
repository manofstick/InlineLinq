using System.Buffers;
using System.Collections.Immutable;

namespace Cistern.InlineLinq;

public static partial class Reducers
{
    public static ImmutableArray<T> ToImmutableArray<T, TEnumeratorable>(this in Enumeratorable<T, TEnumeratorable> source, ArrayPool<T>? maybePool)
        where TEnumeratorable : struct, IEnumeratorable<T>
    {
        var result = source.ToArray(maybePool);
        return System.Runtime.CompilerServices.Unsafe.As<T[], ImmutableArray<T>>(ref result);
    }

    public static ImmutableArray<T> ToImmutableArray<T, TEnumeratorable>(this in Enumeratorable<T, TEnumeratorable> source, bool usePool = false)
        where TEnumeratorable : struct, IEnumeratorable<T>
    {
        var result = source.ToArray(usePool);
        return System.Runtime.CompilerServices.Unsafe.As<T[], ImmutableArray<T>>(ref result);
    }
}
