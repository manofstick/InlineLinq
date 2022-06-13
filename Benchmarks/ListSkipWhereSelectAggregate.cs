using BenchmarkDotNet.Attributes;
using NetFabric.Hyperlinq;
using StructLinq;
using Cistern.InlineLinq;
#if OPTIMIZED
using Cistern.InlineLinq.Optimizations;
#endif

namespace Benchmarks
{
    /*
|     Method |    N |        Mean |     Error |    StdDev | Ratio |  Gen 0 | Allocated |
|----------- |----- |------------:|----------:|----------:|------:|-------:|----------:|
| SystemLinq |    0 |    99.84 ns |  0.105 ns |  0.082 ns |  1.00 | 0.0440 |     184 B |
| InlineLinq |    0 |   106.33 ns |  0.043 ns |  0.036 ns |  1.07 |      - |         - |
| StructLinq |    0 |    77.94 ns |  0.060 ns |  0.056 ns |  0.78 | 0.0305 |     128 B |
|  HyperLinq |    0 |    71.09 ns |  0.658 ns |  0.616 ns |  0.71 | 0.0229 |      96 B |
|            |      |             |           |           |       |        |           |
| SystemLinq |    1 |   119.75 ns |  0.569 ns |  0.532 ns |  1.00 | 0.0439 |     184 B |
| InlineLinq |    1 |   116.60 ns |  0.551 ns |  0.515 ns |  0.97 |      - |         - |
| StructLinq |    1 |   101.93 ns |  0.367 ns |  0.343 ns |  0.85 | 0.0305 |     128 B |
|  HyperLinq |    1 |    76.11 ns |  0.272 ns |  0.241 ns |  0.64 | 0.0229 |      96 B |
|            |      |             |           |           |       |        |           |
| SystemLinq |   10 |   174.74 ns |  0.778 ns |  0.728 ns |  1.00 | 0.0439 |     184 B |
| InlineLinq |   10 |   185.97 ns |  0.806 ns |  0.754 ns |  1.06 |      - |         - |
| StructLinq |   10 |   126.05 ns |  1.056 ns |  0.936 ns |  0.72 | 0.0305 |     128 B |
|  HyperLinq |   10 |   110.27 ns |  0.463 ns |  0.433 ns |  0.63 | 0.0229 |      96 B |
|            |      |             |           |           |       |        |           |
| SystemLinq | 1000 | 9,640.16 ns | 42.226 ns | 39.498 ns |  1.00 | 0.0305 |     184 B |
| InlineLinq | 1000 | 8,625.44 ns | 35.065 ns | 32.800 ns |  0.89 |      - |         - |
| StructLinq | 1000 | 6,152.00 ns | 38.330 ns | 33.979 ns |  0.64 | 0.0305 |     128 B |
|  HyperLinq | 1000 | 6,332.87 ns | 30.318 ns | 28.360 ns |  0.66 | 0.0229 |      96 B |

#if OPTIMIZED

|     Method |    N |        Mean |     Error |    StdDev | Ratio |  Gen 0 | Allocated |
|----------- |----- |------------:|----------:|----------:|------:|-------:|----------:|
| SystemLinq |    0 |   101.14 ns |  0.460 ns |  0.407 ns |  1.00 | 0.0440 |     184 B |
| InlineLinq |    0 |   113.11 ns |  0.361 ns |  0.337 ns |  1.12 |      - |         - |
| StructLinq |    0 |    78.82 ns |  0.124 ns |  0.097 ns |  0.78 | 0.0305 |     128 B |
|  HyperLinq |    0 |    71.67 ns |  0.337 ns |  0.315 ns |  0.71 | 0.0229 |      96 B |
|            |      |             |           |           |       |        |           |
| SystemLinq |    1 |   117.49 ns |  0.160 ns |  0.125 ns |  1.00 | 0.0439 |     184 B |
| InlineLinq |    1 |   120.68 ns |  0.622 ns |  0.581 ns |  1.03 |      - |         - |
| StructLinq |    1 |    85.91 ns |  0.412 ns |  0.385 ns |  0.73 | 0.0305 |     128 B |
|  HyperLinq |    1 |    76.40 ns |  0.425 ns |  0.398 ns |  0.65 | 0.0229 |      96 B |
|            |      |             |           |           |       |        |           |
| SystemLinq |   10 |   185.01 ns |  0.780 ns |  0.730 ns |  1.00 | 0.0439 |     184 B |
| InlineLinq |   10 |   135.58 ns |  0.561 ns |  0.498 ns |  0.73 |      - |         - |
| StructLinq |   10 |   133.96 ns |  0.566 ns |  0.529 ns |  0.72 | 0.0305 |     128 B |
|  HyperLinq |   10 |   134.60 ns |  0.784 ns |  0.695 ns |  0.73 | 0.0229 |      96 B |
|            |      |             |           |           |       |        |           |
| SystemLinq | 1000 | 9,314.95 ns | 50.217 ns | 46.973 ns |  1.00 | 0.0305 |     184 B |
| InlineLinq | 1000 | 5,451.32 ns | 24.560 ns | 21.772 ns |  0.59 |      - |         - |
| StructLinq | 1000 | 6,146.80 ns | 27.061 ns | 25.313 ns |  0.66 | 0.0305 |     128 B |
|  HyperLinq | 1000 | 6,431.55 ns | 36.793 ns | 34.416 ns |  0.69 | 0.0229 |      96 B |
    */

    [Config(typeof(PGOConfig))]
    [MemoryDiagnoser]
    public class ListSkipWhereSelectAggregate
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
            .Skip(N/2)
            .Where(x => x.X % 2 == 0)
            .Select(x => x.Y)
            .Aggregate(0, (a, c) => a + c);

        [Benchmark]
        public int InlineLinq() =>
            data
            .ToInlineLinq()
            .Skip(N / 2)
            .Where(x => x.X % 2 == 0)
            .Select(x => x.Y)
            .Aggregate(0, (a, c) => a + c);

        [Benchmark]
        public int StructLinq() =>
            data
            .ToStructEnumerable()
            .Skip(N / 2)
            .Where(x => x.X % 2 == 0)
            .Select(x => x.Y)
            .Aggregate(0, (a, c) => a + c);

        [Benchmark]
        public int HyperLinq() =>
            data
            .AsValueEnumerable()
            .Skip(N / 2)
            .Where(x => x.X % 2 == 0)
            .Select(x => x.Y)
            .Aggregate(0, (a, c) => a + c);
    }
}
