using Cistern.InlineLinq.Transforms;
using System.Buffers;
using System.Collections.Immutable;

namespace Cistern.InlineLinq.Optimizations;

public static partial class Reducers
{
    public static ImmutableArray<U> ToImmutableArray<T, U, TEnumeratorable>(this in Enumeratorable<U, γSelect<T, U, TEnumeratorable>> source, ArrayPool<U>? _/*ignore*/)
        where TEnumeratorable : struct, IEnumeratorable<T>
    {
        var result = source.ToArray();
        return System.Runtime.CompilerServices.Unsafe.As<U[], ImmutableArray<U>>(ref result);
    }

    public static ImmutableArray<U> ToImmutableArray<T, U, TEnumeratorable>(this in Enumeratorable<U, γSelect<T, U, TEnumeratorable>> source, bool _/*ignore*/)
        where TEnumeratorable : struct, IEnumeratorable<T>
    {
        var result = source.ToArray();
        return System.Runtime.CompilerServices.Unsafe.As<U[], ImmutableArray<U>>(ref result);
    }

    public static ImmutableArray<U> ToImmutableArray<T, U, TEnumeratorable>(this in Enumeratorable<U, γSelect<T, U, TEnumeratorable>> source)
        where TEnumeratorable : struct, IEnumeratorable<T>
    {
        var result = source.ToArray();
        return System.Runtime.CompilerServices.Unsafe.As<U[], ImmutableArray<U>>(ref result);
    }
}
