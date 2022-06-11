using System.Collections.Immutable;

namespace Cistern.InlineLinq.Utils
{
    public struct ReadOnlyArray<T>
    {
        private readonly T[] _array;

        private ReadOnlyArray(T[] array) => _array = array;

        public static ReadOnlyArray<T> Create(T[] array) => new(array);
        public static ReadOnlyArray<T> Create(ImmutableArray<T> immutableArray)
        {
            // https://github.com/dotnet/runtime/issues/25461#issuecomment-373388610
            var array = System.Runtime.CompilerServices.Unsafe.As<ImmutableArray<T>, T[]>(ref immutableArray);
            return new (array);
        }

        public T this[int idx] => _array[idx];
        public int Length => _array.Length;
        public ReadOnlySpan<T> AsSpan() => _array.AsSpan();
    }
}
