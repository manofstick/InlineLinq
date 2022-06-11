﻿namespace Cistern.InlineLinq.Tests.Vanilla;

[TestClass]
public class Select
{
    static readonly IEnumerable<int> Empty =
        System.Linq.Enumerable.Empty<int>();
    static readonly IEnumerable<int> ZeroToTen =
        System.Linq.Enumerable.Range(0, 11);

    static private void EmptyCheck<TEnumeratorable>(in Enumeratorable<int, TEnumeratorable> zeroToTenContainer)
        where TEnumeratorable : struct, IEnumeratorable<int>
    {
        var select =
            zeroToTenContainer
            .Select(_ => true);

        UsingStructEnumerator(select);
        UsingEnumerable(select.GetEnumerable());

        static void UsingStructEnumerator(in Enumeratorable<bool, Transforms.γSelect<int, bool, TEnumeratorable>> select)
        {
            foreach (var item in select)
                Assert.Fail();
        }

        static void UsingEnumerable(IEnumerable<bool> select)
        {
            foreach (var item in select)
                Assert.Fail();
        }
    }

    static private void ZeroToTenCheck<TEnumeratorable>(in Enumeratorable<int, TEnumeratorable> zeroToTenContainer)
        where TEnumeratorable : struct, IEnumeratorable<int>
    {
        var select =
            zeroToTenContainer
            .Select(n => (decimal)n * 2);

        UsingStructEnumerator(select);
        UsingEnumerable(select.GetEnumerable());

        static void UsingStructEnumerator(in Enumeratorable<decimal, Transforms.γSelect<int, decimal, TEnumeratorable>> select)
        {
            var check = 0M;
            var count = 0;
            foreach (var item in select)
            {
                Assert.AreEqual(check, item);
                check += 2M;
                count += 1;
            }
            Assert.AreEqual(11, count);
        }

        static void UsingEnumerable(IEnumerable<decimal> select)
        {
            var check = 0M;
            var count = 0;
            foreach (var item in select)
            {
                Assert.AreEqual(check, item);
                check += 2M;
                count += 1;
            }
            Assert.AreEqual(11, count);
        }
    }

    [TestMethod]
    public void SourceArray()
    {
        static Enumeratorable<int, γArray<int>> getContainer(IEnumerable<int> e) =>
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
