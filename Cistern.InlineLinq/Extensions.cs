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

        // ----------------------------------------------------------------------------------------------------------

        // `TAccumulate Aggregate<TSource, TAccumulate>(this IEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> func);`
        // `TResult Aggregate<TSource, TAccumulate, TResult>(this IEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> func, Func<TAccumulate, TResult> resultSelector);`
        // `bool All<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate);`
        // `bool Any<TSource>(this IEnumerable<TSource> source);`
        // `bool Any<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate);`
        // `IEnumerable<TSource> Append<TSource>(this IEnumerable<TSource> source, TSource element);`
        // `IEnumerable<TSource> AsEnumerable<TSource>(this IEnumerable<TSource> source);`
        // `double Average<TSource>(this IEnumerable<TSource> source, Func<TSource, int> selector);`
        // `double Average<TSource>(this IEnumerable<TSource> source, Func<TSource, long> selector);`
        // `double? Average<TSource>(this IEnumerable<TSource> source, Func<TSource, double?> selector);`
        // `float Average<TSource>(this IEnumerable<TSource> source, Func<TSource, float> selector);`
        // `double? Average<TSource>(this IEnumerable<TSource> source, Func<TSource, long?> selector);`
        // `float? Average<TSource>(this IEnumerable<TSource> source, Func<TSource, float?> selector);`
        // `double Average<TSource>(this IEnumerable<TSource> source, Func<TSource, double> selector);`
        // `double? Average<TSource>(this IEnumerable<TSource> source, Func<TSource, int?> selector);`
        // `decimal Average<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal> selector);`
        // `decimal? Average<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal?> selector);`
        // `float? Average(this IEnumerable<float?> source);`
        // `double? Average(this IEnumerable<long?> source);`
        // `double? Average(this IEnumerable<int?> source);`
        // `double? Average(this IEnumerable<double?> source);`
        // `decimal? Average(this IEnumerable<decimal?> source);`
        // `double Average(this IEnumerable<long> source);`
        // `double Average(this IEnumerable<int> source);`
        // `double Average(this IEnumerable<double> source);`
        // `decimal Average(this IEnumerable<decimal> source);`
        // `float Average(this IEnumerable<float> source);`
        // `IEnumerable<TResult> Cast<TResult>(this IEnumerable source);`
        // `IEnumerable<TSource[]> Chunk<TSource>(this IEnumerable<TSource> source, int size);`
        // `IEnumerable<TSource> Concat<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second);`
        // `bool Contains<TSource>(this IEnumerable<TSource> source, TSource value, IEqualityComparer<TSource>? comparer);`
        // `bool Contains<TSource>(this IEnumerable<TSource> source, TSource value);`
        // `int Count<TSource>(this IEnumerable<TSource> source);`
        // `int Count<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate);`
        // `IEnumerable<TSource?> DefaultIfEmpty<TSource>(this IEnumerable<TSource> source);`
        // `IEnumerable<TSource> DefaultIfEmpty<TSource>(this IEnumerable<TSource> source, TSource defaultValue);`
        // `IEnumerable<TSource> Distinct<TSource>(this IEnumerable<TSource> source);`
        // `IEnumerable<TSource> Distinct<TSource>(this IEnumerable<TSource> source, IEqualityComparer<TSource>? comparer);`
        // `IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector);`
        // `IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey>? comparer);`
        // `TSource ElementAt<TSource>(this IEnumerable<TSource> source, Index index);`
        // `TSource ElementAt<TSource>(this IEnumerable<TSource> source, int index);`
        // `TSource? ElementAtOrDefault<TSource>(this IEnumerable<TSource> source, Index index);`
        // `TSource? ElementAtOrDefault<TSource>(this IEnumerable<TSource> source, int index);`
        // `IEnumerable<TResult> Empty<TResult>();`
        // `IEnumerable<TSource> Except<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second);`
        // `IEnumerable<TSource> Except<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second, IEqualityComparer<TSource>? comparer);`
        // `IEnumerable<TSource> ExceptBy<TSource, TKey>(this IEnumerable<TSource> first, IEnumerable<TKey> second, Func<TSource, TKey> keySelector);`
        // `IEnumerable<TSource> ExceptBy<TSource, TKey>(this IEnumerable<TSource> first, IEnumerable<TKey> second, Func<TSource, TKey> keySelector, IEqualityComparer<TKey>? comparer);`
        // `TSource First<TSource>(this IEnumerable<TSource> source);`
        // `TSource First<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate);`
        // `TSource? FirstOrDefault<TSource>(this IEnumerable<TSource> source);`
        // `TSource FirstOrDefault<TSource>(this IEnumerable<TSource> source, TSource defaultValue);`
        // `TSource? FirstOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate);`
        // `TSource FirstOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, TSource defaultValue);`
        // `IEnumerable<TResult> GroupBy<TSource, TKey, TElement, TResult>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, Func<TKey, IEnumerable<TElement>, TResult> resultSelector, IEqualityComparer<TKey>? comparer);`
        // `IEnumerable<TResult> GroupBy<TSource, TKey, TElement, TResult>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, Func<TKey, IEnumerable<TElement>, TResult> resultSelector);`
        // `IEnumerable<TResult> GroupBy<TSource, TKey, TResult>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TKey, IEnumerable<TSource>, TResult> resultSelector, IEqualityComparer<TKey>? comparer);`
        // `IEnumerable<TResult> GroupBy<TSource, TKey, TResult>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TKey, IEnumerable<TSource>, TResult> resultSelector);`
        // `IEnumerable<IGrouping<TKey, TSource>> GroupBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector);`
        // `IEnumerable<IGrouping<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector);`
        // `IEnumerable<IGrouping<TKey, TSource>> GroupBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey>? comparer);`
        // `IEnumerable<IGrouping<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey>? comparer);`
        // `IEnumerable<TResult> GroupJoin<TOuter, TInner, TKey, TResult>(this IEnumerable<TOuter> outer, IEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, IEnumerable<TInner>, TResult> resultSelector, IEqualityComparer<TKey>? comparer);`
        // `IEnumerable<TResult> GroupJoin<TOuter, TInner, TKey, TResult>(this IEnumerable<TOuter> outer, IEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, IEnumerable<TInner>, TResult> resultSelector);`
        // `IEnumerable<TSource> Intersect<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second, IEqualityComparer<TSource>? comparer);`
        // `IEnumerable<TSource> Intersect<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second);`
        // `IEnumerable<TSource> IntersectBy<TSource, TKey>(this IEnumerable<TSource> first, IEnumerable<TKey> second, Func<TSource, TKey> keySelector);`
        // `IEnumerable<TSource> IntersectBy<TSource, TKey>(this IEnumerable<TSource> first, IEnumerable<TKey> second, Func<TSource, TKey> keySelector, IEqualityComparer<TKey>? comparer);`
        // `IEnumerable<TResult> Join<TOuter, TInner, TKey, TResult>(this IEnumerable<TOuter> outer, IEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, TInner, TResult> resultSelector);`
        // `IEnumerable<TResult> Join<TOuter, TInner, TKey, TResult>(this IEnumerable<TOuter> outer, IEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, TInner, TResult> resultSelector, IEqualityComparer<TKey>? comparer);`
        // `TSource Last<TSource>(this IEnumerable<TSource> source);`
        // `TSource Last<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate);`
        // `TSource? LastOrDefault<TSource>(this IEnumerable<TSource> source);`
        // `TSource LastOrDefault<TSource>(this IEnumerable<TSource> source, TSource defaultValue);`
        // `TSource? LastOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate);`
        // `TSource LastOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, TSource defaultValue);`
        // `long LongCount<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate);`
        // `long LongCount<TSource>(this IEnumerable<TSource> source);`
        // `long Max<TSource>(this IEnumerable<TSource> source, Func<TSource, long> selector);`
        // `decimal Max<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal> selector);`
        // `double Max<TSource>(this IEnumerable<TSource> source, Func<TSource, double> selector);`
        // `int Max<TSource>(this IEnumerable<TSource> source, Func<TSource, int> selector);`
        // `decimal? Max<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal?> selector);`
        // `TSource? Max<TSource>(this IEnumerable<TSource> source, IComparer<TSource>? comparer);`
        // `int? Max<TSource>(this IEnumerable<TSource> source, Func<TSource, int?> selector);`
        // `long? Max<TSource>(this IEnumerable<TSource> source, Func<TSource, long?> selector);`
        // `float? Max<TSource>(this IEnumerable<TSource> source, Func<TSource, float?> selector);`
        // `TResult? Max<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector);`
        // `double? Max<TSource>(this IEnumerable<TSource> source, Func<TSource, double?> selector);`
        // `TSource? Max<TSource>(this IEnumerable<TSource> source);`
        // `float Max<TSource>(this IEnumerable<TSource> source, Func<TSource, float> selector);`
        // `float Max(this IEnumerable<float> source);`
        // `float? Max(this IEnumerable<float?> source);`
        // `long? Max(this IEnumerable<long?> source);`
        // `int? Max(this IEnumerable<int?> source);`
        // `double? Max(this IEnumerable<double?> source);`
        // `decimal? Max(this IEnumerable<decimal?> source);`
        // `long Max(this IEnumerable<long> source);`
        // `int Max(this IEnumerable<int> source);`
        // `double Max(this IEnumerable<double> source);`
        // `decimal Max(this IEnumerable<decimal> source);`
        // `TSource? MaxBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector);`
        // `TSource? MaxBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey>? comparer);`
        // `decimal Min(this IEnumerable<decimal> source);`
        // `TResult? Min<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector);`
        // `float Min<TSource>(this IEnumerable<TSource> source, Func<TSource, float> selector);`
        // `float? Min<TSource>(this IEnumerable<TSource> source, Func<TSource, float?> selector);`
        // `int? Min<TSource>(this IEnumerable<TSource> source, Func<TSource, int?> selector);`
        // `double? Min<TSource>(this IEnumerable<TSource> source, Func<TSource, double?> selector);`
        // `decimal? Min<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal?> selector);`
        // `long Min<TSource>(this IEnumerable<TSource> source, Func<TSource, long> selector);`
        // `int Min<TSource>(this IEnumerable<TSource> source, Func<TSource, int> selector);`
        // `decimal Min<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal> selector);`
        // `TSource? Min<TSource>(this IEnumerable<TSource> source, IComparer<TSource>? comparer);`
        // `TSource? Min<TSource>(this IEnumerable<TSource> source);`
        // `long? Min<TSource>(this IEnumerable<TSource> source, Func<TSource, long?> selector);`
        // `double Min<TSource>(this IEnumerable<TSource> source, Func<TSource, double> selector);`
        // `float Min(this IEnumerable<float> source);`
        // `float? Min(this IEnumerable<float?> source);`
        // `long? Min(this IEnumerable<long?> source);`
        // `int? Min(this IEnumerable<int?> source);`
        // `double? Min(this IEnumerable<double?> source);`
        // `decimal? Min(this IEnumerable<decimal?> source);`
        // `double Min(this IEnumerable<double> source);`
        // `long Min(this IEnumerable<long> source);`
        // `int Min(this IEnumerable<int> source);`
        // `TSource? MinBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey>? comparer);`
        // `TSource? MinBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector);`
        // `IEnumerable<TResult> OfType<TResult>(this IEnumerable source);`
        // `IOrderedEnumerable<TSource> OrderBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey>? comparer);`
        // `IOrderedEnumerable<TSource> OrderBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector);`
        // `IOrderedEnumerable<TSource> OrderByDescending<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector);`
        // `IOrderedEnumerable<TSource> OrderByDescending<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey>? comparer);`
        // `IEnumerable<TSource> Prepend<TSource>(this IEnumerable<TSource> source, TSource element);`
        // `IEnumerable<int> Range(int start, int count);`
        // `IEnumerable<TResult> Repeat<TResult>(TResult element, int count);`
        // `IEnumerable<TSource> Reverse<TSource>(this IEnumerable<TSource> source);`
        public static Enumeratorable<U, γSelectIndexed<T, U, TEnumeratorable>> Select<T, U, TEnumeratorable>(this in Enumeratorable<T, TEnumeratorable> source, Func<T, int, U> selector)
            where TEnumeratorable : struct, IEnumeratorable<T> =>
            new(new(source.Inner, selector));

        public static Enumeratorable<U, γSelect<T, U, TEnumeratorable>> Select<T, U, TEnumeratorable>(this in Enumeratorable<T, TEnumeratorable> source, Func<T, U> selector)
            where TEnumeratorable : struct, IEnumeratorable<T> =>
            new(new(source.Inner, selector));

        // `IEnumerable<TResult> SelectMany<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, int, IEnumerable<TResult>> selector);`
        // `IEnumerable<TResult> SelectMany<TSource, TCollection, TResult>(this IEnumerable<TSource> source, Func<TSource, IEnumerable<TCollection>> collectionSelector, Func<TSource, TCollection, TResult> resultSelector);`
        // `IEnumerable<TResult> SelectMany<TSource, TCollection, TResult>(this IEnumerable<TSource> source, Func<TSource, int, IEnumerable<TCollection>> collectionSelector, Func<TSource, TCollection, TResult> resultSelector);`
        // `IEnumerable<TResult> SelectMany<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, IEnumerable<TResult>> selector);`
        // `bool SequenceEqual<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second);`
        // `bool SequenceEqual<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second, IEqualityComparer<TSource>? comparer);`
        // `TSource Single<TSource>(this IEnumerable<TSource> source);`
        // `TSource Single<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate);`
        // `TSource SingleOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, TSource defaultValue);`
        // `TSource SingleOrDefault<TSource>(this IEnumerable<TSource> source, TSource defaultValue);`
        // `TSource? SingleOrDefault<TSource>(this IEnumerable<TSource> source);`
        // `TSource? SingleOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate);`
        // `IEnumerable<TSource> Skip<TSource>(this IEnumerable<TSource> source, int count);`
        // `IEnumerable<TSource> SkipLast<TSource>(this IEnumerable<TSource> source, int count);`
        // `IEnumerable<TSource> SkipWhile<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate);`
        // `IEnumerable<TSource> SkipWhile<TSource>(this IEnumerable<TSource> source, Func<TSource, int, bool> predicate);`
        // `int Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, int> selector);`
        // `long Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, long> selector);`
        // `decimal? Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal?> selector);`
        // `long? Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, long?> selector);`
        // `int? Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, int?> selector);`
        // `double Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, double> selector);`
        // `float? Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, float?> selector);`
        // `float Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, float> selector);`
        // `double? Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, double?> selector);`
        // `decimal Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal> selector);`
        // `long? Sum(this IEnumerable<long?> source);`
        // `float? Sum(this IEnumerable<float?> source);`
        // `int? Sum(this IEnumerable<int?> source);`
        // `double? Sum(this IEnumerable<double?> source);`
        // `decimal? Sum(this IEnumerable<decimal?> source);`
        // `long Sum(this IEnumerable<long> source);`
        // `int Sum(this IEnumerable<int> source);`
        // `double Sum(this IEnumerable<double> source);`
        // `decimal Sum(this IEnumerable<decimal> source);`
        // `float Sum(this IEnumerable<float> source);`
        // `IEnumerable<TSource> Take<TSource>(this IEnumerable<TSource> source, Range range);`
        // `IEnumerable<TSource> Take<TSource>(this IEnumerable<TSource> source, int count);`
        // `IEnumerable<TSource> TakeLast<TSource>(this IEnumerable<TSource> source, int count);`
        // `IEnumerable<TSource> TakeWhile<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate);`
        // `IEnumerable<TSource> TakeWhile<TSource>(this IEnumerable<TSource> source, Func<TSource, int, bool> predicate);`
        // `IOrderedEnumerable<TSource> ThenBy<TSource, TKey>(this IOrderedEnumerable<TSource> source, Func<TSource, TKey> keySelector);`
        // `IOrderedEnumerable<TSource> ThenBy<TSource, TKey>(this IOrderedEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey>? comparer);`
        // `IOrderedEnumerable<TSource> ThenByDescending<TSource, TKey>(this IOrderedEnumerable<TSource> source, Func<TSource, TKey> keySelector);`
        // `IOrderedEnumerable<TSource> ThenByDescending<TSource, TKey>(this IOrderedEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey>? comparer);`
        // `TSource[] ToArray<TSource>(this IEnumerable<TSource> source);`
        // `Dictionary<TKey, TSource> ToDictionary<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector) where TKey : notnull;`
        // `Dictionary<TKey, TSource> ToDictionary<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey>? comparer) where TKey : notnull;`
        // `Dictionary<TKey, TElement> ToDictionary<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector) where TKey : notnull;`
        // `Dictionary<TKey, TElement> ToDictionary<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey>? comparer) where TKey : notnull;`
        // `HashSet<TSource> ToHashSet<TSource>(this IEnumerable<TSource> source, IEqualityComparer<TSource>? comparer);`
        // `HashSet<TSource> ToHashSet<TSource>(this IEnumerable<TSource> source);`
        // `List<TSource> ToList<TSource>(this IEnumerable<TSource> source);`
        // `ILookup<TKey, TElement> ToLookup<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey>? comparer);`
        // `ILookup<TKey, TElement> ToLookup<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector);`
        // `ILookup<TKey, TSource> ToLookup<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector);`
        // `ILookup<TKey, TSource> ToLookup<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey>? comparer);`
        // `bool TryGetNonEnumeratedCount<TSource>(this IEnumerable<TSource> source, out int count);`
        // `IEnumerable<TSource> Union<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second);`
        // `IEnumerable<TSource> Union<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second, IEqualityComparer<TSource>? comparer);`
        // `IEnumerable<TSource> UnionBy<TSource, TKey>(this IEnumerable<TSource> first, IEnumerable<TSource> second, Func<TSource, TKey> keySelector);`
        // `IEnumerable<TSource> UnionBy<TSource, TKey>(this IEnumerable<TSource> first, IEnumerable<TSource> second, Func<TSource, TKey> keySelector, IEqualityComparer<TKey>? comparer);`
        public static Enumeratorable<T, γWhere<T, TEnumeratorable>> Where<T, TEnumeratorable>(this in Enumeratorable<T, TEnumeratorable> source, Func<T, bool> predicate)
            where TEnumeratorable : struct, IEnumeratorable<T> =>
            new(new(source.Inner, predicate));

        // `IEnumerable<TSource> Where<TSource>(this IEnumerable<TSource> source, Func<TSource, int, bool> predicate);`
        // `IEnumerable<(TFirst First, TSecond Second, TThird Third)> Zip<TFirst, TSecond, TThird>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, IEnumerable<TThird> third);`
        // `IEnumerable<(TFirst First, TSecond Second)> Zip<TFirst, TSecond>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second);`
        // `IEnumerable<TResult> Zip<TFirst, TSecond, TResult>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, Func<TFirst, TSecond, TResult> resultSelector);`
    }
}
