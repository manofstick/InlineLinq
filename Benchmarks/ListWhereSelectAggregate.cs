using BenchmarkDotNet.Attributes;
using NetFabric.Hyperlinq;
using StructLinq;
using Cistern.InlineLinq;
using Cistern.InlineLinq.Optimizations;

namespace Benchmarks
{
    /*
    */

    [Config(typeof(PGOConfig))]
    [MemoryDiagnoser]
    public class ListWhereSelectAggregate
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

            Func<int>[] tests = new Func<int>[]
            {
                SystemLinq,
                InlineLinq,
                StructLinq,
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

        private static bool Equal(int lhs, int rhs) => lhs == rhs;

        [Benchmark(Baseline = true)]
        public int SystemLinq() =>
            ((IEnumerable<Blah>)data)
            .Where(x => x.X % 2 == 0)
            .Select(x => x.Y)
            .Aggregate(0, (a, c) => a + c);

        [Benchmark]
        public int InlineLinq() =>
            data
            .ToInlineLinq()
            .Where(x => x.X % 2 == 0)
            .Select(x => x.Y)
            .Aggregate(0, (a, c) => a + c);

        [Benchmark]
        public int StructLinq() =>
            data
            .ToStructEnumerable()
            .Where(x => x.X % 2 == 0)
            .Select(x => x.Y)
            .Aggregate(0, (a, c) => a + c);

        [Benchmark]
        public int HyperLinq() =>
            data
            .AsValueEnumerable()
            .Where(x => x.X % 2 == 0)
            .Select(x => x.Y)
            .Aggregate(0, (a, c) => a + c);
    }
}
