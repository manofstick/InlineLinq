namespace InlineLinq.Tests.Vanilla;

using System.Collections.Immutable;
using Cistern.InlineLinq;

[TestClass]
public class ToImmutableArray
{
    static readonly IEnumerable<int>[] Sources = new IEnumerable<int>[]
    {
        System.Linq.Enumerable.Empty<int>(),
        System.Linq.Enumerable.Range(0, 1),
        System.Linq.Enumerable.Range(0, 1000),
        System.Linq.Enumerable.Repeat(int.MaxValue, 10),
        System.Linq.Enumerable.Repeat(int.MinValue, 1000),
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
        {
            var expected =
                enumerable
                .ToImmutableArray();

            var check =
                enumeratorable
                .ToImmutableArray();

            Assert.IsTrue(System.Linq.Enumerable.SequenceEqual(expected, check));
        }

        foreach (var where in Wheres)
        {
            var expected =
                enumerable
                .Where(where)
                .ToImmutableArray();

            var check =
                enumeratorable
                .Where(where)
                .ToImmutableArray();

            Assert.IsTrue(System.Linq.Enumerable.SequenceEqual(expected, check));
        }

        foreach (var select in Selects)
        {
            var expected =
                enumerable
                .Select(select)
                .ToImmutableArray();

            var check =
                enumeratorable
                .Select(select)
                .ToImmutableArray();

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
                    .ToImmutableArray();

                var check =
                    enumeratorable
                    .Where(where)
                    .Select(select)
                    .ToImmutableArray();

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
                    .ToImmutableArray();

                var check =
                    enumeratorable
                    .Select(select)
                    .Where(where)
                    .ToImmutableArray();

                Assert.IsTrue(System.Linq.Enumerable.SequenceEqual(expected, check));
            }
        }
    }

    [TestMethod]
    public void SourceArray()
    {
        static Enumeratorable<int, γMemory<int>> getContainer(IEnumerable<int> e) =>
            System.Linq.Enumerable.ToArray(e).ToInlineLinq();

        foreach (var source in Sources)
            RunChecks(source, getContainer(source));
    }

    [TestMethod]
    public void SourceSequence()
    {
        static Enumeratorable<int, γMemory<int>> getContainer(IEnumerable<int> e)
        {
            var asArray =
                System.Linq.Enumerable.ToArray(
                    System.Linq.Enumerable.Select(
                        e,
                        x => (ReadOnlyMemory<int>)new[] { x }));

            return asArray.ToInlineLinqOfT();
        }

        foreach (var source in Sources)
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
    public void SourceEnumerable()
    {
        static Enumeratorable<int, γEnumerable<int>> getContainer(IEnumerable<int> e) =>
            e.ToInlineLinq();

        foreach (var source in Sources)
            RunChecks(source, getContainer(source));
    }

    static private void DifferentLengthChecks<TEnumeratorable>(Func<IEnumerable<int>, Enumeratorable<int, TEnumeratorable>> getEnumeratorable)
        where TEnumeratorable : struct, IEnumeratorable<int>
    {
        for (var count = 0; count < 200; ++count)
        {
            var source = Enumerable.Range(0, count);

            var select = getEnumeratorable(source).Select(x => x * 2).ToImmutableArray();
            Assert.AreEqual(count, select.Length);

            var selectWhere = getEnumeratorable(source).Select(x => x * 2).Where(x => true).ToImmutableArray();
            Assert.AreEqual(count, selectWhere.Length);

            var where = getEnumeratorable(source).Where(x => true).ToImmutableArray();
            Assert.AreEqual(count, where.Length);

            var whereSelect = getEnumeratorable(source).Where(x => true).Select(x => x * 2).ToImmutableArray();
            Assert.AreEqual(count, whereSelect.Length);
        }
    }

    [TestMethod]
    public void TryDifferentLengthBoundaries()
    {
        DifferentLengthChecks(source => source.ToArray().ToInlineLinq());
        DifferentLengthChecks(source => source.ToInlineLinq());
        DifferentLengthChecks(source => source.ToImmutableArray().ToInlineLinq());
        DifferentLengthChecks(source => source.ToImmutableArray().ToInlineLinq());
    }
}
