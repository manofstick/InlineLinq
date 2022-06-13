using BenchmarkDotNet.Attributes;
using NetFabric.Hyperlinq;
using StructLinq;
using Cistern.InlineLinq;

namespace Benchmarks
{
    /*
|      Method |    N |         Mean |     Error |    StdDev | Ratio | RatioSD |  Gen 0 |  Gen 1 | Allocated |
|------------ |----- |-------------:|----------:|----------:|------:|--------:|-------:|-------:|----------:|
|  SystemLinq |    0 |     32.72 ns |  0.223 ns |  0.209 ns |  1.00 |    0.00 |      - |      - |         - |
|  InlineLinq |    0 |     79.31 ns |  0.332 ns |  0.310 ns |  2.42 |    0.02 |      - |      - |         - |
| InlineLinq2 |    0 |     80.79 ns |  0.311 ns |  0.291 ns |  2.47 |    0.02 |      - |      - |         - |
|  StructLinq |    0 |    117.96 ns |  0.744 ns |  0.696 ns |  3.61 |    0.03 | 0.0286 |      - |     120 B |
|   HyperLinq |    0 |     34.12 ns |  0.146 ns |  0.136 ns |  1.04 |    0.01 |      - |      - |         - |
|             |      |              |           |           |       |         |        |        |           |
|  SystemLinq |    1 |    101.79 ns |  0.705 ns |  0.625 ns |  1.00 |    0.00 | 0.0343 |      - |     144 B |
|  InlineLinq |    1 |    163.67 ns |  0.668 ns |  0.592 ns |  1.61 |    0.01 | 0.0076 |      - |      32 B |
| InlineLinq2 |    1 |    161.69 ns |  1.085 ns |  1.015 ns |  1.59 |    0.01 | 0.0076 |      - |      32 B |
|  StructLinq |    1 |    126.91 ns |  0.645 ns |  0.572 ns |  1.25 |    0.01 | 0.0286 |      - |     120 B |
|   HyperLinq |    1 |    125.60 ns |  0.753 ns |  0.704 ns |  1.23 |    0.01 | 0.0076 |      - |      32 B |
|             |      |              |           |           |       |         |        |        |           |
|  SystemLinq |   10 |    201.15 ns |  1.482 ns |  1.386 ns |  1.00 |    0.00 | 0.0610 |      - |     256 B |
|  InlineLinq |   10 |    288.51 ns |  2.147 ns |  2.009 ns |  1.43 |    0.02 | 0.0114 |      - |      48 B |
| InlineLinq2 |   10 |    267.29 ns |  2.039 ns |  1.907 ns |  1.33 |    0.01 | 0.0114 |      - |      48 B |
|  StructLinq |   10 |    245.27 ns |  1.478 ns |  1.234 ns |  1.22 |    0.01 | 0.0324 |      - |     136 B |
|   HyperLinq |   10 |    192.30 ns |  3.887 ns |  3.992 ns |  0.96 |    0.02 | 0.0095 |      - |      40 B |
|             |      |              |           |           |       |         |        |        |           |
|  SystemLinq | 1000 | 10,553.07 ns | 56.377 ns | 52.736 ns |  1.00 |    0.00 | 1.0834 | 0.0153 |   4,576 B |
|  InlineLinq | 1000 | 15,692.79 ns | 75.989 ns | 63.455 ns |  1.49 |    0.01 | 1.3428 | 0.0305 |   5,664 B |
| InlineLinq2 | 1000 | 15,476.33 ns | 13.655 ns | 10.661 ns |  1.47 |    0.01 | 0.4883 | 0.0305 |   2,080 B |
|  StructLinq | 1000 | 11,077.87 ns | 58.658 ns | 54.869 ns |  1.05 |    0.01 | 0.4883 |      - |   2,056 B |
|   HyperLinq | 1000 | 10,650.85 ns | 48.032 ns | 44.929 ns |  1.01 |    0.01 | 0.4730 |      - |   2,008 B |    */

    [Config(typeof(PGOConfig))]
    [MemoryDiagnoser]
    public class ArrayWhereSelectToArray
    {
        [Params(0, 1, 10, 1000)]
        public int N { get; set; }

        private Blah[] data = null!;

        [GlobalSetup]
        public void GlobalSetup()
        {
            var data = new List<Blah>(N);

            var r = new Random();
            for (var i = 0; i < N; ++i)
                data.Add(new(r.Next(1000), r.Next(1000), r.Next(1000)));

            this.data = System.Linq.Enumerable.ToArray(data);

            Func<int[]>[] tests = new Func<int[]>[]
            {
                SystemLinq,
                InlineLinq,
                InlineLinq2,
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
