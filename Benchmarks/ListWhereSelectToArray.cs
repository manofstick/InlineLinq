using BenchmarkDotNet.Attributes;
using NetFabric.Hyperlinq;
using StructLinq;
using Cistern.InlineLinq;

namespace Benchmarks
{
    /*
|      Method |    N |         Mean |     Error |    StdDev | Ratio |  Gen 0 |  Gen 1 | Allocated |
|------------ |----- |-------------:|----------:|----------:|------:|-------:|-------:|----------:|
|  SystemLinq |    0 |     86.50 ns |  0.330 ns |  0.309 ns |  1.00 | 0.0421 |      - |     176 B |
|  InlineLinq |    0 |     73.94 ns |  0.181 ns |  0.169 ns |  0.85 |      - |      - |         - |
| InlineLinq2 |    0 |     71.87 ns |  0.026 ns |  0.022 ns |  0.83 |      - |      - |         - |
|  StructLinq |    0 |    120.12 ns |  0.487 ns |  0.431 ns |  1.39 | 0.0286 |      - |     120 B |
|   HyperLinq |    0 |     35.29 ns |  0.406 ns |  0.360 ns |  0.41 |      - |      - |         - |
|             |      |              |           |           |       |        |        |           |
|  SystemLinq |    1 |    109.20 ns |  0.183 ns |  0.153 ns |  1.00 | 0.0497 |      - |     208 B |
|  InlineLinq |    1 |     94.44 ns |  0.271 ns |  0.253 ns |  0.86 |      - |      - |         - |
| InlineLinq2 |    1 |     79.11 ns |  0.346 ns |  0.307 ns |  0.72 |      - |      - |         - |
|  StructLinq |    1 |    178.79 ns |  0.879 ns |  0.823 ns |  1.64 | 0.0305 |      - |     128 B |
|   HyperLinq |    1 |    125.60 ns |  0.532 ns |  0.498 ns |  1.15 | 0.0076 |      - |      32 B |
|             |      |              |           |           |       |        |        |           |
|  SystemLinq |   10 |    195.64 ns |  1.143 ns |  1.070 ns |  1.00 | 0.0515 |      - |     216 B |
|  InlineLinq |   10 |    223.12 ns |  3.181 ns |  2.820 ns |  1.14 | 0.0114 |      - |      48 B |
| InlineLinq2 |   10 |    188.86 ns |  1.115 ns |  0.988 ns |  0.97 | 0.0095 |      - |      40 B |
|  StructLinq |   10 |    235.55 ns |  1.442 ns |  1.349 ns |  1.20 | 0.0324 |      - |     136 B |
|   HyperLinq |   10 |    177.51 ns |  1.298 ns |  1.214 ns |  0.91 | 0.0095 |      - |      40 B |
|             |      |              |           |           |       |        |        |           |
|  SystemLinq | 1000 | 12,279.62 ns | 89.656 ns | 83.864 ns |  1.00 | 1.0834 | 0.0153 |   4,576 B |
|  InlineLinq | 1000 | 13,947.63 ns | 41.823 ns | 37.075 ns |  1.14 | 1.3733 | 0.0153 |   5,760 B |
| InlineLinq2 | 1000 | 13,761.31 ns | 61.715 ns | 54.709 ns |  1.12 | 0.5035 | 0.0153 |   2,128 B |
|  StructLinq | 1000 | 11,108.97 ns | 52.411 ns | 49.026 ns |  0.90 | 0.5188 | 0.0153 |   2,184 B |
|   HyperLinq | 1000 | 10,518.35 ns | 62.849 ns | 58.789 ns |  0.86 | 0.4730 |      - |   2,024 B |
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
