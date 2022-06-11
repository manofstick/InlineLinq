namespace Cistern.InlineLinq.Tests.Vanilla;

[TestClass]
public class ToList
{
    static readonly IEnumerable<int>[] Sources = new IEnumerable<int>[]
    {
        System.Linq.Enumerable.Empty<int>(),
        System.Linq.Enumerable.Range(0, 1),
        System.Linq.Enumerable.Range(0, 100),
        System.Linq.Enumerable.Repeat(int.MaxValue, 10),
        System.Linq.Enumerable.Repeat(int.MinValue, 10),
    };

    static readonly Func<int, bool>[] Wheres = new Func<int, bool>[]
    {
        x => true,
        x => false,
        x => x % 2 == 0,
        x => x < 5,
    };

    static readonly Func<int, int>[] Selects = new Func<int, int>[]
    {
        x => x,
        x => 0,
        x => x * 2,
        x => -x,
    };

    static private void RunChecks<TEnumeratorable>(IEnumerable<int> enumerable, in Enumeratorable<int, TEnumeratorable> enumeratorable)
        where TEnumeratorable : struct, IEnumeratorable<int>
    {
        foreach (var where in Wheres)
        {
            var expected =
                enumerable
                .Where(where)
                .ToList();

            var check =
                enumeratorable
                .Where(where)
                .ToList();

            Assert.IsTrue(System.Linq.Enumerable.SequenceEqual(expected, check));
        }

        foreach (var select in Selects)
        {
            var expected =
                enumerable
                .Select(select)
                .ToList();

            var check =
                enumeratorable
                .Select(select)
                .ToList();

            Assert.IsTrue(System.Linq.Enumerable.SequenceEqual(expected, check));
        }

        foreach (var where in Wheres)
        {
            foreach (var select in Selects)
            {
                var expected =
                    enumerable
                    .Where(where)
                    .Select(select)
                    .ToList();

                var check =
                    enumeratorable
                    .Where(where)
                    .Select(select)
                    .ToList();

                Assert.IsTrue(System.Linq.Enumerable.SequenceEqual(expected, check));
            }
        }

        foreach (var select in Selects)
        {
            foreach (var where in Wheres)
            {
                var expected =
                    enumerable
                    .Select(select)
                    .Where(where)
                    .ToList();

                var check =
                    enumeratorable
                    .Select(select)
                    .Where(where)
                    .ToList();

                Assert.IsTrue(System.Linq.Enumerable.SequenceEqual(expected, check));
            }
        }
    }

    [TestMethod]
    public void SourceArray()
    {
        static Enumeratorable<int, γArray<int>> getContainer(IEnumerable<int> e) =>
            System.Linq.Enumerable.ToArray(e).ToInlineLinq();

        foreach(var source in Sources)
            RunChecks(source, getContainer(source));
    }

    [TestMethod]
    public void SourceList()
    {
        static Enumeratorable<int, γList<int>> getContainer(IEnumerable<int> e) =>
            System.Linq.Enumerable.ToList(e).ToInlineLinq();

        foreach (var source in Sources)
            RunChecks(source, getContainer(source));
    }

    [TestMethod]
    public void SourceImmutableArray()
    {
        static Enumeratorable<int, γImmutableArray<int>> getContainer(IEnumerable<int> e) =>
            System.Collections.Immutable.ImmutableArray.ToImmutableArray(e).ToInlineLinq();

        foreach (var source in Sources)
            RunChecks(source, getContainer(source));
    }

    [TestMethod]
    public void SourceEnumerable()
    {
        static Enumeratorable<int, γEnumerable<int>> getContainer(IEnumerable<int> e) =>
            e.ToInlineLinq();

        foreach (var source in Sources)
            RunChecks(source, getContainer(source));
    }
}
