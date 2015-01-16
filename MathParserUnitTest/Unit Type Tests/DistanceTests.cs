using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using dab.Library.MathParser;

namespace MathParserUnitTest.Unit_Type_Tests
{
    [TestClass]
    public class DistanceTests
    {
        MathParser mp = new MathParser("", "");
        
        [TestMethod]
        public void SimpleDistanceConvert()
        {
            Assert.AreEqual("12 Inches", mp.Evaluate("1 foot to inches").ToString());
        }

        [TestMethod]
        public void DistanceLargeToSmaller()
        {
            Assert.AreEqual("5 Centimeters", mp.Evaluate(".05 meter").ToString());
        }

        [TestMethod]
        public void ReduceDistanceConvert()
        {
            Assert.AreEqual("1 Foot", mp.Evaluate("12in to foot").ToString());
        }

        [TestMethod]
        public void DistanceImperialToMetricConvert()
        {
            Assert.AreEqual("1,609.347088 Meters", mp.Evaluate("1 miles to meter").ToString());
        }

        [TestMethod]
        public void DistanceMetricToImperialConvert()
        {
            Assert.AreEqual("0.62137 Miles", mp.Evaluate("1km to miles").ToString());
        }

        [TestMethod]
        public void DistanceCheckReduceProperConvert()
        {
            Assert.AreEqual("1.999976 Yards", mp.Evaluate("2 feet + 4 foot").ToString());
        }
    }
}
