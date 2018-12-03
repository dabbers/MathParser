using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using dab.Library.MathParser;

namespace MathParserUnitTest.Unit_Type_Tests
{
    [TestClass]
    public class TempTests
    {
        MathParser mp = new MathParser("", "");

        [TestMethod]
        public void CtoFConvert()
        {
            Assert.AreEqual("98.6 Fahrenheit", mp.Evaluate("37c to f").ToString());
        }
        [TestMethod]
        public void CtoKConvert()
        {
            Assert.AreEqual("310.15 Kelvins", mp.Evaluate("37c to K").ToString());
        }

        [TestMethod]
        public void FtoCConvert()
        {
            Assert.AreEqual("37 Celsius", mp.Evaluate("98.6f to c").ToString());
        }

        [TestMethod]
        public void FtoKConvert()
        {
            Assert.AreEqual("310.15 Kelvins", mp.Evaluate("98.6f to k").ToString());
        }

        [TestMethod]
        public void KtoCConvert()
        {
            Assert.AreEqual("37 Celsius", mp.Evaluate("310.15k to c").ToString());
        }

        [TestMethod]
        public void KtoFConvert()
        {
            Assert.AreEqual("98.6 Fahrenheit", mp.Evaluate("310.15k to f").ToString());
        }
    }
}
