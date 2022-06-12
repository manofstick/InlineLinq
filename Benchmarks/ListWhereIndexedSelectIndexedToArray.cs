using BenchmarkDotNet.Attributes;
using NetFabric.Hyperlinq;
using StructLinq;
using Cistern.InlineLinq;

namespace Benchmarks
{
    /*
|      Method |    N |         Mean |      Error |     StdDev | Ratio |  Gen 0 |  Gen 1 | Allocated |
|------------ |----- |-------------:|-----------:|-----------:|------:|-------:|-------:|----------:|
|  SystemLinq |    0 |     98.38 ns |   0.848 ns |   0.793 ns |  1.00 | 0.0497 |      - |     208 B |
|  InlineLinq |    0 |     54.13 ns |   0.041 ns |   0.032 ns |  0.55 |      - |      - |         - |
| InlineLinq2 |    0 |     57.34 ns |   0.020 ns |   0.019 ns |  0.58 |      - |      - |         - |
|   HyperLinq |    0 |     79.93 ns |   0.315 ns |   0.294 ns |  0.81 |      - |      - |         - |
|             |      |              |            |            |       |        |        |           |
|  SystemLinq |    1 |    170.25 ns |   0.434 ns |   0.363 ns |  1.00 | 0.0668 |      - |     280 B |
|  InlineLinq |    1 |    119.93 ns |   0.083 ns |   0.077 ns |  0.70 | 0.0076 |      - |      32 B |
| InlineLinq2 |    1 |    105.23 ns |   0.073 ns |   0.057 ns |  0.62 |      - |      - |         - |
|   HyperLinq |    1 |     89.01 ns |   0.322 ns |   0.301 ns |  0.52 |      - |      - |         - |
|             |      |              |            |            |       |        |        |           |
|  SystemLinq |   10 |    431.61 ns |   1.702 ns |   1.592 ns |  1.00 | 0.0839 |      - |     352 B |
|  InlineLinq |   10 |    201.63 ns |   0.644 ns |   0.602 ns |  0.47 | 0.0076 |      - |      32 B |
| InlineLinq2 |   10 |    209.87 ns |   1.923 ns |   1.798 ns |  0.49 | 0.0114 |      - |      48 B |
|   HyperLinq |   10 |    261.41 ns |   1.441 ns |   1.348 ns |  0.61 | 0.0134 |      - |      56 B |
|             |      |              |            |            |       |        |        |           |
|  SystemLinq | 1000 | 24,408.61 ns | 109.767 ns | 102.676 ns |  1.00 | 1.6174 | 0.0305 |   6,768 B |
|  InlineLinq | 1000 | 14,205.89 ns |   5.847 ns |   4.565 ns |  0.58 | 1.3580 | 0.0153 |   5,720 B |
| InlineLinq2 | 1000 | 13,938.85 ns |  54.853 ns |  51.310 ns |  0.57 | 0.4883 | 0.0153 |   2,064 B |
|   HyperLinq | 1000 | 12,635.49 ns |  57.254 ns |  53.556 ns |  0.52 | 0.4730 |      - |   2,032 B |
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
