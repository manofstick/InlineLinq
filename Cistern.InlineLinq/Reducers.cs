//using System;
//using System.Collections.Generic;
//using System.Collections.Immutable;
//using System.Linq;
//using System.Runtime.CompilerServices;
//using System.Text;
//using System.Threading.Tasks;

//namespace Cistern.InlineLinq
//{
//    public static class Reducers
//    {
//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//        public static int Sum<TEnumeratorable>(this in Enumeratorable<int, TEnumeratorable> source)
//            where TEnumeratorable : struct, IEnumeratorable<int>
//        {
//            var e = source.GetInitializedEnumeratorable();
//            var sum = 0;
//            while (e.TryGetNext(out var current))
//            {
//                sum += current;
//            }
//            return sum;
//        }

//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//        public static U[] ToArray<T, U, TEnumeratorable>(this in Enumeratorable<U, γSelect<T, U, TEnumeratorable>> source)
//            where TEnumeratorable : struct, IEnumeratorable<T>
//        {
//            var select = source.Inner;
//            var enumeratorable = select.Inner;
//            if (enumeratorable.TryGetSpan(out var span))
//            {
//                if (span.Length == 0)
//                    return Array.Empty<U>();

//                return ToArray(span, select.Selector);
//            }
//            else if (enumeratorable.TryGetCount(out var count))
//            {
//                if (count == 0)
//                    return Array.Empty<U>();

//                return ToArray(enumeratorable, select.Selector, count);
//            }
//            else
//            {
//                return ToArray(enumeratorable, select.Selector);
//            }
//        }

//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//        private static U[] ToArray<TEnumeratorable, T, U>(TEnumeratorable enumeratorable, Func<T, U> selector, int count)
//            where TEnumeratorable : struct, IEnumeratorable<T>
//        {
//            var result = new U[count];
//            var idx = 0;
//            while (enumeratorable.TryGetNext(out var item))
//                result[idx++] = selector(item);
//            return result;
//        }

//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//        private static U[] ToArray<TEnumeratorable, T, U>(TEnumeratorable enumeratorable, Func<T, U> selector)
//            where TEnumeratorable : struct, IEnumeratorable<T>
//        {
//            var builder = new List<U>();
//            while (enumeratorable.TryGetNext(out var item))
//                builder.Add(selector(item));
//            return builder.ToArray();
//        }

//        private static U[] ToArray<T, U>(ReadOnlySpan<T> span, Func<T, U> selector)
//        {
//            var result = new U[span.Length];
//            for(var i=0; i < result.Length; ++i)
//            {
//                result[i] = selector(span[i]);
//            }
//            return result;
//        }

//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//        public static T[] ToArray<T, TEnumeratorable>(this in Enumeratorable<T, TEnumeratorable> source)
//            where TEnumeratorable : struct, IEnumeratorable<T>
//        {
//            var e = source.GetInitializedEnumeratorable();

//            if (e.TryGetCount(out var count))
//            {
//                if (count == 0)
//                    return Array.Empty<T>();

//                var result = new T[count];
//                var idx = 0;
//                while(e.TryGetNext(out var item))
//                    result[idx++] = item;
//                return result;
//            }

//            var builder = ImmutableArray.CreateBuilder<T>();
//            while (e.TryGetNext(out var item))
//                builder.Add(item);
//            return builder.ToArray();
//        }
//    }
//}
