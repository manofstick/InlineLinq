namespace Cistern.InlineLinq
{
    public struct Enumeratorable<T, TEnumeratorable>
        where TEnumeratorable : struct, IEnumeratorable<T>
    {
        public TEnumeratorable Inner { get; }

        public Enumeratorable(TEnumeratorable enumeratorable) => Inner = enumeratorable;

        public struct Enumerator
        {
            TEnumeratorable _enumeratorable;
            T _current;

            public Enumerator(TEnumeratorable enumeratorable) => (_enumeratorable, _current) = (enumeratorable, default!);

            public T Current => _current;

            public bool MoveNext() => _enumeratorable.TryGetNext(out _current);
        }

        public Enumerator GetEnumerator() => new(Inner);
    }
}
