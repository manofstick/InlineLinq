using BenchmarkDotNet.Running;
using Cistern.InlineLinq;

namespace Benchmarks
{
    record struct Blah(int X, int Y, int Z);


    public class Program
    {
        public IEnumerable<int> Test<TEnumeratorable>(Enumeratorable<int, TEnumeratorable> stuff)
            where TEnumeratorable : struct, IEnumeratorable<int>
        {
            foreach (var item in stuff)
                yield return item;
        }

        public static void Main(string[] args)
        {
            var x = new ListWhereSelectToArray();
            x.N = 1000;
            x.GlobalSetup();

            var summary = BenchmarkRunner.Run<ListWhereIndexedSelectIndexedToArray>();
        }
    }
}