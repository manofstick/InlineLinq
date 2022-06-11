using System.Runtime.CompilerServices;

namespace Cistern.InlineLinq
{
    public struct γArray<T>
        : IEnumeratorable<T>
    {
        public T[] Array { get; }

        public γArray(T[] array) => (Array, _index) = (array, int.MinValue);

        private int _index;
        public void Initialize() => _index = -1;
        public void Dispose() => _index = int.MinValue;

        public int? TryGetCount(out int? upperBound)
        {
            upperBound = Array.Length;
            return upperBound;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetNext(out T current)
        {
            var nextIdx = _index + 1;
            if (nextIdx < Array.Length)
            {
                _index = nextIdx;
                current = Array[_index];
                return true;
            }
            current = default!;
            return false;
        }

        public bool TryGetSpan(out ReadOnlySpan<T> span)
        {
            span = Array.AsSpan();
            return true;
        }
    }
}
