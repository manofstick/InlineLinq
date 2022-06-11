namespace Cistern.InlineLinq;

public static partial class Reducers
{
    public static List<T> ToList<T, TEnumeratorable>(this in Enumeratorable<T, TEnumeratorable> source)
        where TEnumeratorable : struct, IEnumeratorable<T>
    {
        var enumeratorable = source.Inner;

        var maybeCount = enumeratorable.TryGetCount(out var _);

        return ToList<T, TEnumeratorable >(in enumeratorable, maybeCount);
    }

    private static List<T> ToList<T, TEnumeratorable>(in TEnumeratorable enumeratorable, int? maybeCount)
        where TEnumeratorable : struct, IEnumeratorable<T>
    {
        var result =
            maybeCount.HasValue 
                ? new List<T>(maybeCount.Value) 
                : new List<T>();

        var e = enumeratorable;
        e.Initialize();
        try
        {
            while (e.TryGetNext(out var item))
            {
                result.Add(item);
            }
            return result;
        }
        finally
        {
            e.Dispose();
        }
    }
}
