using Cistern.InlineLinq.Transforms;

namespace Cistern.InlineLinq.Optimizations;

public static partial class Reducers
{
    class SelectWhereAggregate
    {
        public static TAccumulate Aggregate<TEnumeratorable, T, TSource, TAccumulate>(in Enumeratorable<TSource, γSelect<T, TSource, γWhere<T, TEnumeratorable>>> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> func)
            where TEnumeratorable : struct, IEnumeratorable<T>
        {
            var select = source.Inner;
            var where = select.Inner;
            var enumeratorable = where.Inner;

            enumeratorable.Initialize();
            try
            {
                if (enumeratorable.TryGetNextSpan(out var span))
                    return AggregateViaSpan(ref enumeratorable, select.Selector, where.Predicate, seed, func, span);

                return AggregateViaElement(ref enumeratorable, select.Selector, where.Predicate, seed, func);
            }
            finally
            {
                enumeratorable.Dispose();
            }
        }

        private static TAccumulate AggregateViaElement<TEnumeratorable, T, TSource, TAccumulate>(ref TEnumeratorable enumeratorable, Func<T, TSource> selector, Func<T, bool> predicate, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> func)
            where TEnumeratorable : struct, IEnumeratorable<T>
        {
            var accumulate = seed;
            while (enumeratorable.TryGetNext(out var item))
            {
                if (predicate(item))
                    accumulate = func(accumulate, selector(item));
            }
            return accumulate;
        }

        private static TAccumulate AggregateViaSpan<TEnumeratorable, T, TSource, TAccumulate>(ref TEnumeratorable enumeratorable, Func<T, TSource> selector, Func<T, bool> predicate, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> func, ReadOnlySpan<T> span)
            where TEnumeratorable : struct, IEnumeratorable<T>
        {
            var accumulate = seed;
            do
            {
                for (var i = 0; i < span.Length; ++i)
                {
                    if (predicate(span[i]))
                        accumulate = func(accumulate, selector(span[i]));
                }
            } while (enumeratorable.TryGetNextSpan(out span));
            return accumulate;
        }
    }

    public static TAccumulate Aggregate<TEnumeratorable, T, TSource, TAccumulate>(this in Enumeratorable<TSource, γSelect<T, TSource, γWhere<T, TEnumeratorable>>> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> func)
        where TEnumeratorable : struct, IEnumeratorable<T> => SelectWhereAggregate.Aggregate(in source, seed, func);
}
