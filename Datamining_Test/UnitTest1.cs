using System;
using NUnit.Framework;

namespace Datamining_Test
{
    [TestFixture]
    public class UnitTest1
    {
        [Test]
        public void TestMethod1()
        {
            int i = 1;
            Assert.AreEqual(i + 1, 2);
        }
    }
}
