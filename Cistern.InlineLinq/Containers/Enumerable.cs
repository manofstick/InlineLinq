using System.Collections.Immutable;
using System.Runtime.CompilerServices;

namespace Cistern.InlineLinq
{
    public struct γEnumerable<T>
        : IEnumeratorable<T>
    {
        public IEnumerable<T> Enumerable { get; }

        public γEnumerable(IEnumerable<T> enumerable) => (Enumerable, _enumerator) = (enumerable, null);

        private IEnumerator<T>? _enumerator;
        public void Initialize() => _enumerator = Enumerable.GetEnumerator();
        public void Dispose()
        {
            _enumerator?.Dispose();
            _enumerator = null!;
        }

        public int? TryGetCount(out int? upperBound)
        {
            if (Enumerable is ICollection<T> collection)
            {
                upperBound = collection.Count;
                return upperBound;
            }
            upperBound = null;
            return null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetNext(out T current)
        {
            if (_enumerator!.MoveNext())
            {
                current = _enumerator.Current;
                return true;
            }
            current = default!;
            return false;
        }

        public bool TryGetNextSpan(out ReadOnlySpan<T> span)
        {
            if (_enumerator is not null)
            {
                if (Enumerable is T[] array)
                {
                    span = array.AsSpan();
                    _enumerator = null;
                    return true;
                }
                else if (Enumerable is List<T> list)
                {
                    span = System.Runtime.InteropServices.CollectionsMarshal.AsSpan(list);
                    _enumerator = null;
                    return true;
                }
                else if (Enumerable is ImmutableArray<T> immutableArray)
                {
                    span = immutableArray.AsSpan();
                    _enumerator = null;
                    return true;
                }
            }

            span = default;
            return false;
        }
    }
}
