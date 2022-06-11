using System.Collections.Immutable;
using System.Runtime.CompilerServices;

namespace Cistern.InlineLinq
{
    public struct γImmutableArray<T>
        : IEnumeratorable<T>
    {
        public ImmutableArray<T> ImmutableArray { get; }

        public γImmutableArray(ImmutableArray<T> array) => (ImmutableArray, _index) = (array, int.MinValue);

        private int _index;
        public void Initialize() => _index = -1;
        public void Dispose() => _index = int.MinValue;

        public int? TryGetCount(out int? upperBound)
        {
            upperBound = ImmutableArray.Length;
            return upperBound;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetNext(out T current)
        {
            var nextIdx = _index + 1;
            if (nextIdx < ImmutableArray.Length)
            {
                _index = nextIdx;
                current = ImmutableArray[_index];
                return true;
            }
            current = default!;
            return false;
        }

        public bool TryGetSpan(out ReadOnlySpan<T> span)
        {
            span = ImmutableArray.AsSpan();
            return true;
        }
    }
}
