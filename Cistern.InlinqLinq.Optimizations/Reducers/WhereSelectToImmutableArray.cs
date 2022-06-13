using Cistern.InlineLinq.Transforms;
using Cistern.InlineLinq.Utils;
using System.Buffers;
using System.Collections.Immutable;

namespace Cistern.InlineLinq.Optimizations;

public static partial class Reducers
{
    public static ImmutableArray<U> ToImmutableArray<T, U, TEnumeratorable>(this in Enumeratorable<U, γSelect<T, U, γWhere<T, TEnumeratorable>>> source, ArrayPool<U>? maybeArrayPool)
        where TEnumeratorable : struct, IEnumeratorable<T>
    {
        var result = source.ToArray(maybeArrayPool);
        return System.Runtime.CompilerServices.Unsafe.As<U[], ImmutableArray<U>>(ref result);
    }

    public static ImmutableArray<U> ToImmutableArray<T, U, TEnumeratorable>(this in Enumeratorable<U, γSelect<T, U, γWhere<T, TEnumeratorable>>> source, bool usePool)
        where TEnumeratorable : struct, IEnumeratorable<T>
    {
        var result = source.ToArray(usePool);
        return System.Runtime.CompilerServices.Unsafe.As<U[], ImmutableArray<U>>(ref result);
    }

    public static ImmutableArray<U> ToImmutableArray<T, U, TEnumeratorable>(this in Enumeratorable<U, γSelect<T, U, γWhere<T, TEnumeratorable>>> source)
        where TEnumeratorable : struct, IEnumeratorable<T>
    {
        var result = source.ToArray();
        return System.Runtime.CompilerServices.Unsafe.As<U[], ImmutableArray<U>>(ref result);
    }
}
