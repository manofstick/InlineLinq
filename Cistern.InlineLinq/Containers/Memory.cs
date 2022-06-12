using System.Buffers;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;

namespace Cistern.InlineLinq
{
    public struct γMemory<T>
        : IEnumeratorable<T>
    {
        class SequenceContainer
        {
            public ReadOnlySequence<T> Sequence;
            public ReadOnlySequence<T>.Enumerator Enumerator;
        }

        private object? _internal;
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
            _internal = new SequenceContainer { Sequence = sequence, Enumerator = default };
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

        public γMemory(ReadOnlyMemory<T> memory) => (_internal, _memory, _length, _index) = (null, memory, memory.Length, int.MinValue);
        public γMemory(T[] array) : this(array.AsMemory()) {}
        public γMemory(ImmutableArray<T> array) : this(array.AsMemory()) {}

        private ReadOnlyMemory<T> _memory;
        private int _index;
        public void Initialize()
        {
            if (_internal != null)
            {
                var sequence = (SequenceContainer) _internal;
                sequence.Enumerator = sequence.Sequence.GetEnumerator();
                if (sequence.Enumerator.MoveNext())
                    _memory = sequence.Enumerator.Current;
                else
                    _memory = ReadOnlyMemory<T>.Empty;
            }
            _index = -1;
        }
        public void Dispose()
        {
            _index = int.MinValue;
            if (_internal != null)
            {
                var sequence = (SequenceContainer)_internal;
                sequence.Enumerator = default;
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
            if (_internal is SequenceContainer sequence && sequence.Enumerator.MoveNext())
            {
                _memory = sequence.Enumerator.Current;
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
