using BenchmarkDotNet.Attributes;
using NetFabric.Hyperlinq;
using StructLinq;
using Cistern.InlineLinq;

namespace Benchmarks
{
    /*
    */

    [Config(typeof(PGOConfig))]
    [MemoryDiagnoser]
    public class ListWhereIndexedSelectIndexedToArray
    {
        [Params(0, 1, 10, 1000)]
        public int N { get; set; }

        private List<Blah> data = null!;

        [GlobalSetup]
        public void GlobalSetup()
        {
            data = new List<Blah>(N);

            var r = new Random();
            for (var i = 0; i < N; ++i)
                data.Add(new(r.Next(1000), r.Next(1000), r.Next(1000)));

            Func<int[]>[] tests = new Func<int[]>[]
            {
                SystemLinq,
                InlineLinq,
                //StructLinq,
                HyperLinq,
            };

            var check = tests[0]();
            foreach (var test in tests)
            {
                var result = test();
                if (!Equal(result, check))
                    throw new Exception();
            }
        }

        private static bool Equal(int[] lhs, int[] rhs) =>
            Enumerable.SequenceEqual(lhs, rhs);

        [Benchmark(Baseline = true)]
        public int[] SystemLinq() =>
            ((IEnumerable<Blah>)data)
            .Where((x, idx) => x.X % 2 == 0)
            .Select((x, idx) => x.Y)
            .ToArray();

        [Benchmark]
        public int[] InlineLinq() =>
            data
            .ToInlineLinq()
            .Where((x, idx) => x.X % 2 == 0)
            .Select((x, idx) => x.Y)
            .ToArray();

        [Benchmark]
        public int[] InlineLinq2() =>
            data
            .ToInlineLinq()
            .Where((x, idx) => x.X % 2 == 0)
            .Select((x, idx) => x.Y)
            .ToArray(true);

        // StructLinq does not support indexed operators 
        //public int[] StructLinq();

        [Benchmark]
        public int[] HyperLinq() =>
            data
            .AsValueEnumerable()
            .Where((x, idx) => x.X % 2 == 0)
            .Select((x, idx) => x.Y)
            .ToArray();
    }
}
