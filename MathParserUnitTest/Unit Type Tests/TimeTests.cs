using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using dab.Library.MathParser;

namespace MathParserUnitTest.Unit_Type_Tests
{
    [TestClass]
    public class TimeTests
    {
        MathParser mp = new MathParser("", "");

        [TestMethod]
        public void TimeTest()
        {
            Assert.AreEqual(60, Math.Round(mp.Evaluate("1 minute as seconds").Value, 6));
        }


        [TestMethod]
        public void TimeTestBiggerLabelToSmaller()
        {
            Assert.AreEqual("0.05 Seconds", mp.Evaluate("0.005 seconds").ToString());
        }
    }
}
