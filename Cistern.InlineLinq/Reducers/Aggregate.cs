namespace Cistern.InlineLinq;

public static partial class Reducers
{
    public static TAccumulate Aggregate<TEnumeratorable, TSource, TAccumulate>(this in Enumeratorable<TSource, TEnumeratorable> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> func)
        where TEnumeratorable : struct, IEnumeratorable<TSource>
    {
        var enumeratorable = source.Inner;
        enumeratorable.Initialize();
        try
        {
            if (enumeratorable.TryGetNextSpan(out var span))
                return ViaSpan(ref enumeratorable, seed, func, span);

            return ViaElement(ref enumeratorable, seed, func);
        }
        finally
        {
            enumeratorable.Dispose();
        }

        static TAccumulate ViaSpan(ref TEnumeratorable enumeratorable, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> func, ReadOnlySpan<TSource> span)
        {
            var accumulate = seed;
            do
            {
                for (var i = 0; i < span.Length; ++i)
                {
                    accumulate = func(accumulate, span[i]);
                }
            } while (enumeratorable.TryGetNextSpan(out span));
            return accumulate;
        }

        static TAccumulate ViaElement(ref TEnumeratorable enumeratorable, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> func)
        {
            var accumulate = seed;
            while (enumeratorable.TryGetNext(out var current))
            {
                accumulate = func(accumulate, current);
            }
            return accumulate;
        }
    }
}