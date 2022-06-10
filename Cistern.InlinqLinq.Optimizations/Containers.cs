using Cistern.InlineLinq.Transforms;
using System.Collections.Immutable;

namespace Cistern.InlineLinq.Optimizations;

public static class ImmutableArrayExtensions
{
    public static Enumeratorable<T, γWhere<T, γImmutableArray<T>>> Where<T, TEnumeratorable>(this ImmutableArray<T> source, Func<T, bool> predicate)
        where TEnumeratorable : struct, IEnumeratorable<T> =>
        new(new(new(source), predicate));

    public static Enumeratorable<U, γSelect<T, U, γImmutableArray<T>>> Select<T, U, TEnumeratorable>(this ImmutableArray<T> source, Func<T, U> selector)
        where TEnumeratorable : struct, IEnumeratorable<T> =>
        new(new(new(source), selector));

}

public static class ListExtensions
{
    public static Enumeratorable<T, γWhere<T, γList<T>>> Where<T>(this List<T> source, Func<T, bool> predicate) =>
        new(new(new(source), predicate));

    public static Enumeratorable<U, γSelect<T, U, γList<T>>> Select<T, U>(this List<T> source, Func<T, U> selector) =>
        new(new(new(source), selector));

}