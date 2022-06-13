using BenchmarkDotNet.Attributes;
using NetFabric.Hyperlinq;
using StructLinq;
using Cistern.InlineLinq;

namespace Benchmarks
{
    /*
|      Method |    N |         Mean |     Error |    StdDev | Ratio | RatioSD |  Gen 0 |  Gen 1 | Allocated |
|------------ |----- |-------------:|----------:|----------:|------:|--------:|-------:|-------:|----------:|
|  SystemLinq |    0 |     74.60 ns |  0.170 ns |  0.159 ns |  1.00 |    0.00 | 0.0421 |      - |     176 B |
|  InlineLinq |    0 |     53.52 ns |  0.011 ns |  0.009 ns |  0.72 |    0.00 |      - |      - |         - |
| InlineLinq2 |    0 |     53.47 ns |  0.009 ns |  0.008 ns |  0.72 |    0.00 |      - |      - |         - |
|  StructLinq |    0 |    120.88 ns |  0.558 ns |  0.522 ns |  1.62 |    0.01 | 0.0286 |      - |     120 B |
|   HyperLinq |    0 |     35.35 ns |  0.153 ns |  0.143 ns |  0.47 |    0.00 |      - |      - |         - |
|             |      |              |           |           |       |         |        |        |           |
|  SystemLinq |    1 |     79.71 ns |  0.321 ns |  0.301 ns |  1.00 |    0.00 | 0.0421 |      - |     176 B |
|  InlineLinq |    1 |     93.91 ns |  0.294 ns |  0.275 ns |  1.18 |    0.00 |      - |      - |         - |
| InlineLinq2 |    1 |     94.93 ns |  0.269 ns |  0.251 ns |  1.19 |    0.01 |      - |      - |         - |
|  StructLinq |    1 |    177.50 ns |  0.657 ns |  0.615 ns |  2.23 |    0.01 | 0.0305 |      - |     128 B |
|   HyperLinq |    1 |    126.19 ns |  0.590 ns |  0.552 ns |  1.58 |    0.01 | 0.0076 |      - |      32 B |
|             |      |              |           |           |       |         |        |        |           |
|  SystemLinq |   10 |    231.82 ns |  1.153 ns |  1.079 ns |  1.00 |    0.00 | 0.0763 |      - |     320 B |
|  InlineLinq |   10 |    180.22 ns |  1.292 ns |  1.208 ns |  0.78 |    0.01 | 0.0095 |      - |      40 B |
| InlineLinq2 |   10 |    197.48 ns |  0.627 ns |  0.556 ns |  0.85 |    0.00 | 0.0095 |      - |      40 B |
|  StructLinq |   10 |    231.72 ns |  4.681 ns |  5.203 ns |  1.01 |    0.02 | 0.0324 |      - |     136 B |
|   HyperLinq |   10 |    164.51 ns |  0.620 ns |  0.580 ns |  0.71 |    0.00 | 0.0095 |      - |      40 B |
|             |      |              |           |           |       |         |        |        |           |
|  SystemLinq | 1000 | 12,691.60 ns | 47.689 ns | 44.608 ns |  1.00 |    0.00 | 1.5869 | 0.0153 |   6,640 B |
|  InlineLinq | 1000 | 14,268.42 ns | 32.439 ns | 30.343 ns |  1.12 |    0.00 | 1.3885 |      - |   5,872 B |
| InlineLinq2 | 1000 | 14,114.33 ns |  9.570 ns |  7.472 ns |  1.11 |    0.00 | 0.4730 |      - |   2,024 B |
|  StructLinq | 1000 | 10,714.04 ns | 61.712 ns | 57.726 ns |  0.84 |    0.00 | 0.4883 |      - |   2,072 B |
|   HyperLinq | 1000 | 10,752.15 ns | 12.934 ns | 10.801 ns |  0.85 |    0.00 | 0.4883 | 0.0153 |   2,088 B |
    */

    [Config(typeof(PGOConfig))]
    [MemoryDiagnoser]
    public class ListWhereSelectToArray
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

        private static bool Equal(int[] lhs, int[] rhs) =>
            Enumerable.SequenceEqual(lhs, rhs);

        [Benchmark(Baseline = true)]
        public int[] SystemLinq() =>
            ((IEnumerable<Blah>)data)
            .Where(x => x.X % 2 == 0)
            .Select(x => x.Y)
            .ToArray();

        [Benchmark]
        public int[] InlineLinq() =>
            data
            .ToInlineLinq()
            .Where(x => x.X % 2 == 0)
            .Select(x => x.Y)
            .ToArray();

        [Benchmark]
        public int[] InlineLinq2() =>
            data
            .ToInlineLinq()
            .Where(x => x.X % 2 == 0)
            .Select(x => x.Y)
            .ToArray(true);

        [Benchmark]
        public int[] StructLinq() =>
            data
            .ToStructEnumerable()
            .Where(x => x.X % 2 == 0)
            .Select(x => x.Y)
            .ToArray();

        [Benchmark]
        public int[] HyperLinq() =>
            data
            .AsValueEnumerable()
            .Where(x => x.X % 2 == 0)
            .Select(x => x.Y)
            .ToArray();
    }
}
