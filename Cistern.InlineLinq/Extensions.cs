using Cistern.InlineLinq.Transforms;
using System.Collections.Immutable;

namespace Cistern.InlineLinq
{
    public static class Extensions
    {
        public static Enumeratorable<T, γArray<T>> ToInlineLinq<T>(this ImmutableArray<T> source) => new(new(source));
        public static Enumeratorable<T, γList<T>> ToInlineLinq<T>(this List<T> source) => new(new(source));
        public static Enumeratorable<T, γArray<T>> ToInlineLinq<T>(this T[] source) => new(new(source));
        public static Enumeratorable<T, γEnumerable<T>> ToInlineLinq<T>(this IEnumerable<T> source) => new(new(source));


        public static Enumeratorable<T, γWhere<T, TEnumeratorable>> Where<T, TEnumeratorable>(this in Enumeratorable<T, TEnumeratorable> source, Func<T, bool> predicate)
            where TEnumeratorable : struct, IEnumeratorable<T> =>
            new (new (source.Inner, predicate));

        public static Enumeratorable<U, γSelect<T, U, TEnumeratorable>> Select<T, U, TEnumeratorable>(this in Enumeratorable<T, TEnumeratorable> source, Func<T, U> selector)
            where TEnumeratorable : struct, IEnumeratorable<T> =>
            new (new(source.Inner, selector));
    }
}
