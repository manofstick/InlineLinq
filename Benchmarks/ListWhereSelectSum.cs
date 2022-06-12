using BenchmarkDotNet.Attributes;
using NetFabric.Hyperlinq;
using StructLinq;
using Cistern.InlineLinq;

namespace Benchmarks
{
    /*
|     Method |    N |         Mean |     Error |    StdDev | Ratio |  Gen 0 | Allocated |
|----------- |----- |-------------:|----------:|----------:|------:|-------:|----------:|
| SystemLinq |    0 |     83.57 ns |  0.093 ns |  0.082 ns |  1.00 | 0.0421 |     176 B |
| InlineLinq |    0 |     50.53 ns |  0.011 ns |  0.009 ns |  0.60 |      - |         - |
| StructLinq |    0 |     89.23 ns |  0.260 ns |  0.230 ns |  1.07 | 0.0229 |      96 B |
|  HyperLinq |    0 |     37.76 ns |  0.124 ns |  0.110 ns |  0.45 |      - |         - |
|            |      |              |           |           |       |        |           |
| SystemLinq |    1 |    100.68 ns |  0.302 ns |  0.283 ns |  1.00 | 0.0421 |     176 B |
| InlineLinq |    1 |     60.24 ns |  0.272 ns |  0.254 ns |  0.60 |      - |         - |
| StructLinq |    1 |     98.97 ns |  0.051 ns |  0.039 ns |  0.98 | 0.0229 |      96 B |
|  HyperLinq |    1 |     46.42 ns |  0.247 ns |  0.231 ns |  0.46 |      - |         - |
|            |      |              |           |           |       |        |           |
| SystemLinq |   10 |    180.87 ns |  0.638 ns |  0.597 ns |  1.00 | 0.0420 |     176 B |
| InlineLinq |   10 |    124.53 ns |  0.057 ns |  0.047 ns |  0.69 |      - |         - |
| StructLinq |   10 |    159.62 ns |  1.074 ns |  1.004 ns |  0.88 | 0.0229 |      96 B |
|  HyperLinq |   10 |     91.30 ns |  1.383 ns |  1.294 ns |  0.50 |      - |         - |
|            |      |              |           |           |       |        |           |
| SystemLinq | 1000 | 14,606.78 ns | 82.641 ns | 77.303 ns |  1.00 | 0.0305 |     176 B |
| InlineLinq | 1000 | 11,683.48 ns | 35.967 ns | 33.644 ns |  0.80 |      - |         - |
| StructLinq | 1000 |  9,113.08 ns | 48.597 ns | 45.457 ns |  0.62 | 0.0153 |      96 B |
|  HyperLinq | 1000 |  8,867.97 ns |  3.532 ns |  2.758 ns |  0.61 |      - |         - |
    */

    [Config(typeof(PGOConfig))]
    [MemoryDiagnoser]
    public class ListWhereSelectSum
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
            .Sum();

        [Benchmark]
        public int InlineLinq() =>
            data
            .ToInlineLinq()
            .Where(x => x.X % 2 == 0)
            .Select(x => x.Y)
            .Sum();

        [Benchmark]
        public int StructLinq() =>
            data
            .ToStructEnumerable()
            .Where(x => x.X % 2 == 0)
            .Select(x => x.Y)
            .Sum();

        [Benchmark]
        public int HyperLinq() =>
            data
            .AsValueEnumerable()
            .Where(x => x.X % 2 == 0)
            .Select(x => x.Y)
            .Sum();
    }
}
