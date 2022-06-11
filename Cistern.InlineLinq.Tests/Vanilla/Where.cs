namespace Cistern.InlineLinq.Tests.Vanilla
{
    [TestClass]
    public class Where
    {
        static readonly IEnumerable<int> Empty =
            System.Linq.Enumerable.Empty<int>();
        static readonly IEnumerable<int> ZeroToTen = 
            System.Linq.Enumerable.Range(0, 11);

        static private void EmptyCheck<TEnumeratorable>(Enumeratorable<int, TEnumeratorable> zeroToTenContainer)
            where TEnumeratorable : struct, IEnumeratorable<int>
        {
            var where =
                zeroToTenContainer
                .Where(_ => true);

            foreach (var item in where)
                Assert.Fail();
        }

        static private void ZeroToTenCheck<TEnumeratorable>(Enumeratorable<int, TEnumeratorable> zeroToTenContainer)
            where TEnumeratorable : struct, IEnumeratorable<int>
        {
            var where = 
                zeroToTenContainer
                .Where(n => n % 2 == 0);

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
        public void SourceImmutableArray()
        {
            static Enumeratorable<int, γImmutableArray<int>> getContainer(IEnumerable<int> e) =>
                System.Collections.Immutable.ImmutableArray.ToImmutableArray(e).ToInlineLinq();

            var empty = getContainer(Empty);
            EmptyCheck(empty);

            var zeroToTen = getContainer(ZeroToTen);
            ZeroToTenCheck(zeroToTen);
        }
    }
}
