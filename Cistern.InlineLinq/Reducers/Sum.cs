namespace Cistern.InlineLinq;

public static partial class Reducers
{
    public static int Sum<TEnumeratorable>(this in Enumeratorable<int, TEnumeratorable> source)
        where TEnumeratorable : struct, IEnumeratorable<int>
    {
        var sum = 0;

        var e = source.Inner;
        e.Initialize();
        try
        {
            while (e.TryGetNext(out var current))
            {
                sum += current;
            }
            return sum;
        }
        finally
        {
            e.Dispose();
        }
    }
}
