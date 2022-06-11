using System.Runtime.CompilerServices;

namespace Cistern.InlineLinq
{
    public struct γList<T>
        : IEnumeratorable<T>
    {
        public List<T> List { get; }

        public γList(List<T> array) => (List, _index) = (array, int.MinValue);

        private int _index;
        public void Initialize() => _index = -1;
        public void Dispose() => _index = int.MinValue;

        public int? TryGetCount(out int? upperBound)
        {
            upperBound = List.Count;
            return upperBound;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetNext(out T current)
        {
            var nextIdx = _index + 1;
            if (nextIdx < List.Count)
            {
                _index = nextIdx;
                current = List[_index];
                return true;
            }
            current = default!;
            return false;
        }
        public bool TryGetSpan(out ReadOnlySpan<T> span)
        {
            span = System.Runtime.InteropServices.CollectionsMarshal.AsSpan(List);
            return true;
        }
    }
}
