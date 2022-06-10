using BenchmarkDotNet.Running;

namespace Benchmarks
{
    record struct Blah(int X, int Y, int Z);


    public class Program
    {
        public static void Main(string[] args)
        {
            var x = new ListWhereSelectToArray();
            x.N = 1000;
            x.GlobalSetup();

            var summary = BenchmarkRunner.Run<ListWhereSelectToArray>();
        }
    }
}