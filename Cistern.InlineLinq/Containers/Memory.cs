using System.Buffers;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;

namespace Cistern.InlineLinq
{
    public struct γMemory<T>
        : IEnumeratorable<T>
    {
        public ReadOnlySequence<T> Memory { get; }
        private readonly int _length;

        internal class Segment : ReadOnlySequenceSegment<T>
        {
            public Segment(ReadOnlyMemory<T> memory) { Memory = memory; }

            public Segment Append(ReadOnlyMemory<T> memory)
            {
                var segment = new Segment(memory) { RunningIndex = RunningIndex + Memory.Length };
                Next = segment;
                return segment;
            }
        }

        public γMemory(ReadOnlySequence<T> sequence)
        {
            Memory = sequence;
            _sequenceEnumerator = default;
            _index = int.MinValue;
            
            var length = 0;
            var e = sequence.GetEnumerator();
            if (!e.MoveNext())
            {
                _memory = ReadOnlyMemory<T>.Empty;
            }
            else
            {
                checked // I suppose I could lift this as a constraint?
                {
                    _memory = e.Current;
                    length = _memory.Length;
                    while (e.MoveNext())
                        length += e.Current.Length;
                }
            }
            _length = length;
        }

        public γMemory(ReadOnlyMemory<T> memory) => (Memory, _length, _sequenceEnumerator, _memory, _index) = (new ReadOnlySequence<T>(memory), memory.Length, default, memory, int.MinValue);
        public γMemory(T[] array) : this(array.AsMemory()) {}
        public γMemory(ImmutableArray<T> array) : this(array.AsMemory()) {}

        private ReadOnlySequence<T>.Enumerator _sequenceEnumerator;
        private ReadOnlyMemory<T> _memory;
        private int _index;
        public void Initialize()
        {
            if (_memory.Length != _length)
            {
                _sequenceEnumerator = Memory.GetEnumerator();
                if (_sequenceEnumerator.MoveNext())
                    _memory = _sequenceEnumerator.Current;
                else
                    _memory = ReadOnlyMemory<T>.Empty;
            }
            _index = -1;
        }
        public void Dispose()
        {
            _index = int.MinValue;
            if (_memory.Length != _length)
            {
                _sequenceEnumerator = default;
                _memory = ReadOnlyMemory<T>.Empty;
            }
        }

        public int? TryGetCount(out int? upperBound)
        {
            upperBound = _length;
            return upperBound;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetNext(out T current)
        {
            var nextIdx = _index + 1;
            if (nextIdx < _memory.Length)
            {
                _index = nextIdx;
                current = _memory.Span[_index];
                return true;
            }
            return TryGetNextChangeMemory(out current);
        }

        private bool TryGetNextChangeMemory(out T current)
        {
            if (_memory.Length != _length && _sequenceEnumerator.MoveNext())
            {
                _memory = _sequenceEnumerator.Current;
                _index = -1;
                return TryGetNext(out current);
            }
            current = default!;
            return false;
        }

        public bool TryGetSpan(out ReadOnlySpan<T> span)
        {
            span = default;
            return true;
        }
    }
}
