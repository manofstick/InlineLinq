using System.Collections.Immutable;
using System.Runtime.CompilerServices;

namespace Cistern.InlineLinq
{
    public struct γMemory<T>
        : IEnumeratorable<T>
    {
        public ReadOnlyMemory<T> Memory { get; }

        public γMemory(T[] array) => (Memory, _index) = (array, int.MinValue);
        public γMemory(ImmutableArray<T> array) => (Memory, _index) = (array.AsMemory(), int.MinValue);

        private int _index;
        public void Initialize() => _index = -1;
        public void Dispose() => _index = int.MinValue;

        public int? TryGetCount(out int? upperBound)
        {
            upperBound = Memory.Length;
            return upperBound;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetNext(out T current)
        {
            var nextIdx = _index + 1;
            if (nextIdx < Memory.Length)
            {
                _index = nextIdx;
                current = Memory.Span[_index];
                return true;
            }
            current = default!;
            return false;
        }

        public bool TryGetSpan(out ReadOnlySpan<T> span)
        {
            span = Memory.Span;
            return true;
        }
    }
}
