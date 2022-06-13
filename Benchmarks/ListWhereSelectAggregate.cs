using BenchmarkDotNet.Attributes;
using NetFabric.Hyperlinq;
using StructLinq;
using Cistern.InlineLinq;

namespace Benchmarks
{
    /*
|     Method |    N |         Mean |     Error |    StdDev |       Median | Ratio |  Gen 0 | Allocated |
|----------- |----- |-------------:|----------:|----------:|-------------:|------:|-------:|----------:|
| SystemLinq |    0 |     86.68 ns |  0.321 ns |  0.285 ns |     86.56 ns |  1.00 | 0.0421 |     176 B |
| InlineLinq |    0 |     48.87 ns |  0.096 ns |  0.164 ns |     48.76 ns |  0.56 |      - |         - |
| StructLinq |    0 |     62.08 ns |  0.247 ns |  0.231 ns |     61.98 ns |  0.72 | 0.0229 |      96 B |
|  HyperLinq |    0 |     57.98 ns |  0.269 ns |  0.252 ns |     57.86 ns |  0.67 | 0.0229 |      96 B |
|            |      |              |           |           |              |       |        |           |
| SystemLinq |    1 |     93.69 ns |  0.328 ns |  0.307 ns |     93.50 ns |  1.00 | 0.0421 |     176 B |
| InlineLinq |    1 |     60.80 ns |  0.025 ns |  0.021 ns |     60.80 ns |  0.65 |      - |         - |
| StructLinq |    1 |     65.73 ns |  0.034 ns |  0.026 ns |     65.73 ns |  0.70 | 0.0229 |      96 B |
|  HyperLinq |    1 |     69.28 ns |  0.426 ns |  0.398 ns |     69.47 ns |  0.74 | 0.0229 |      96 B |
|            |      |              |           |           |              |       |        |           |
| SystemLinq |   10 |    215.04 ns |  1.920 ns |  1.796 ns |    215.15 ns |  1.00 | 0.0420 |     176 B |
| InlineLinq |   10 |    144.60 ns |  0.965 ns |  0.902 ns |    144.31 ns |  0.67 |      - |         - |
| StructLinq |   10 |    146.74 ns |  1.049 ns |  0.930 ns |    146.36 ns |  0.68 | 0.0229 |      96 B |
|  HyperLinq |   10 |    170.60 ns |  0.662 ns |  0.553 ns |    170.81 ns |  0.79 | 0.0229 |      96 B |
|            |      |              |           |           |              |       |        |           |
| SystemLinq | 1000 | 15,063.99 ns | 34.343 ns | 30.445 ns | 15,079.19 ns |  1.00 | 0.0305 |     176 B |
| InlineLinq | 1000 | 12,473.64 ns | 50.736 ns | 42.367 ns | 12,489.21 ns |  0.83 |      - |         - |
| StructLinq | 1000 | 12,330.51 ns | 66.402 ns | 62.113 ns | 12,341.57 ns |  0.82 | 0.0153 |      96 B |
|  HyperLinq | 1000 | 13,015.63 ns | 66.805 ns | 62.490 ns | 13,001.90 ns |  0.86 | 0.0153 |      96 B |
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
