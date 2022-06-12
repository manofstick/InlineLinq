namespace Cistern.InlineLinq.Tests.Vanilla;

[TestClass]
public class WhereIndexed
{
    static readonly IEnumerable<int> Empty =
        System.Linq.Enumerable.Empty<int>();
    static readonly IEnumerable<int> ZeroToTen = 
        System.Linq.Enumerable.Range(0, 11);

    static private void EmptyCheck<TEnumeratorable>(in Enumeratorable<int, TEnumeratorable> zeroToTenContainer)
        where TEnumeratorable : struct, IEnumeratorable<int>
    {
        var where =
            zeroToTenContainer
            .Where((_, _) => true);

        UsingStructEnumerator(where);
        UsingEnumerable(where.GetEnumerable());

        static void UsingStructEnumerator(in Enumeratorable<int, Transforms.γWhereIndexed<int, TEnumeratorable>> where)
        {
            foreach (var item in where)
                Assert.Fail();
        }

        static void UsingEnumerable(IEnumerable<int> where)
        {
            foreach (var item in where)
                Assert.Fail();
        }
    }

    static private void ZeroToTenCheck<TEnumeratorable>(in Enumeratorable<int, TEnumeratorable> zeroToTenContainer)
        where TEnumeratorable : struct, IEnumeratorable<int>
    {
        var where =
            zeroToTenContainer
            .Where((idx, n) => idx % 2 == 0);

        UsingStructEnumerator(where);
        UsingEnumerable(where.GetEnumerable());

        static void UsingStructEnumerator(in Enumeratorable<int, Transforms.γWhereIndexed<int, TEnumeratorable>> where)
        {
            var check = 0;
            var count = 0;
            foreach (var item in where)
            {
                Assert.AreEqual(check, item);
                check += 2;
                count += 1;
            }
            Assert.AreEqual(6, count);
        }

        static void UsingEnumerable(IEnumerable<int> where)
        {
            var check = 0;
            var count = 0;
            foreach (var item in where)
            {
                Assert.AreEqual(check, item);
                check += 2;
                count += 1;
            }
            Assert.AreEqual(6, count);
        }
    }

    [TestMethod]
    public void SourceArray()
    {
        static Enumeratorable<int, γMemory<int>> getContainer(IEnumerable<int> e) =>
            System.Linq.Enumerable.ToArray(e).ToInlineLinq();

        var empty = getContainer(Empty);
        EmptyCheck(empty);

        var zeroToTen = getContainer(ZeroToTen);
        ZeroToTenCheck(zeroToTen);
    }

    [TestMethod]
    public void SourceList()
    {
        static Enumeratorable<int, γList<int>> getContainer(IEnumerable<int> e) =>
            System.Linq.Enumerable.ToList(e).ToInlineLinq();

        var empty = getContainer(Empty);
        EmptyCheck(empty);

        var zeroToTen = getContainer(ZeroToTen);
        ZeroToTenCheck(zeroToTen);
    }

    [TestMethod]
    public void SourceEnumerable()
    {
        static Enumeratorable<int, γEnumerable<int>> getContainer(IEnumerable<int> e) =>
            e.ToInlineLinq();

        var empty = getContainer(Empty);
        EmptyCheck(empty);

        var zeroToTen = getContainer(ZeroToTen);
        ZeroToTenCheck(zeroToTen);
    }
}
