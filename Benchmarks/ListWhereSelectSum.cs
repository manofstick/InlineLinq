using BenchmarkDotNet.Attributes;
using NetFabric.Hyperlinq;
using StructLinq;
using Cistern.InlineLinq;

namespace Benchmarks
{
    /*
|     Method |    N |         Mean |     Error |    StdDev |       Median | Ratio |  Gen 0 | Allocated |
|----------- |----- |-------------:|----------:|----------:|-------------:|------:|-------:|----------:|
| SystemLinq |    0 |     89.33 ns |  0.521 ns |  0.462 ns |     89.13 ns |  1.00 | 0.0421 |     176 B |
| InlineLinq |    0 |     58.98 ns |  0.293 ns |  0.274 ns |     58.80 ns |  0.66 |      - |         - |
| StructLinq |    0 |     88.10 ns |  0.478 ns |  0.447 ns |     87.85 ns |  0.99 | 0.0229 |      96 B |
|  HyperLinq |    0 |     38.77 ns |  0.833 ns |  0.779 ns |     38.87 ns |  0.44 |      - |         - |
|            |      |              |           |           |              |       |        |           |
| SystemLinq |    1 |    100.13 ns |  0.533 ns |  0.498 ns |     99.83 ns |  1.00 | 0.0421 |     176 B |
| InlineLinq |    1 |     60.95 ns |  0.067 ns |  0.056 ns |     60.94 ns |  0.61 |      - |         - |
| StructLinq |    1 |     94.46 ns |  0.337 ns |  0.315 ns |     94.38 ns |  0.94 | 0.0229 |      96 B |
|  HyperLinq |    1 |     47.05 ns |  0.163 ns |  0.152 ns |     46.94 ns |  0.47 |      - |         - |
|            |      |              |           |           |              |       |        |           |
| SystemLinq |   10 |    189.24 ns |  0.733 ns |  0.686 ns |    188.79 ns |  1.00 | 0.0420 |     176 B |
| InlineLinq |   10 |    143.17 ns |  0.764 ns |  0.715 ns |    143.02 ns |  0.76 |      - |         - |
| StructLinq |   10 |    150.56 ns |  1.641 ns |  1.370 ns |    150.97 ns |  0.80 | 0.0229 |      96 B |
|  HyperLinq |   10 |     85.09 ns |  0.443 ns |  0.415 ns |     84.88 ns |  0.45 |      - |         - |
|            |      |              |           |           |              |       |        |           |
| SystemLinq | 1000 | 14,607.51 ns | 41.048 ns | 38.396 ns | 14,638.75 ns |  1.00 | 0.0305 |     176 B |
| InlineLinq | 1000 | 11,571.28 ns | 60.982 ns | 57.043 ns | 11,574.52 ns |  0.79 |      - |         - |
| StructLinq | 1000 |  9,267.84 ns | 37.301 ns | 33.066 ns |  9,287.36 ns |  0.63 | 0.0153 |      96 B |
|  HyperLinq | 1000 |  8,897.85 ns | 42.873 ns | 35.801 ns |  8,918.80 ns |  0.61 |      - |         - |
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
