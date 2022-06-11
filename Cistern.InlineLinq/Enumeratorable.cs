using System.Collections;

namespace Cistern.InlineLinq
{
    public struct Enumeratorable<T, TEnumeratorable>
        where TEnumeratorable : struct, IEnumeratorable<T>
    {
        public TEnumeratorable Inner { get; }

        public Enumeratorable(TEnumeratorable enumeratorable) => Inner = enumeratorable;

        public struct Enumerator : IDisposable
        {
            TEnumeratorable _enumeratorable;
            T _current;

            public Enumerator(in TEnumeratorable enumeratorable)
            {
                (_enumeratorable, _current) = (enumeratorable, default!);
                _enumeratorable.Initialize();
            }

            public void Dispose()
            {
                _enumeratorable.Dispose();
            }

            public T Current => _current;

            public bool MoveNext() => _enumeratorable.TryGetNext(out _current);
        }

        public Enumerator GetEnumerator() => new(Inner);

        public IEnumerable<T> GetEnumerable()
        {
            foreach (var t in this)
                yield return t;
        }
    }
}
