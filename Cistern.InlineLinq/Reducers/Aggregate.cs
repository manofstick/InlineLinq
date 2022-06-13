namespace Cistern.InlineLinq;

public static partial class Reducers
{
    public static TAccumulate Aggregate<TEnumeratorable, TSource, TAccumulate>(this in Enumeratorable<TSource, TEnumeratorable> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> func)
        where TEnumeratorable : struct, IEnumeratorable<TSource>
    {
        var accumulate = seed;

        var e = source.Inner;
        e.Initialize();
        try
        {
            while (e.TryGetNext(out var current))
            {
                accumulate = func(accumulate, current);
            }
            return accumulate;
        }
        finally
        {
            e.Dispose();
        }
    }
}