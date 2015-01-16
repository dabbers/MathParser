using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using dab.Library.MathParser;

namespace MathParserUnitTest
{
    [TestClass]
    public class HexTests
    {
        MathParser mp = new MathParser("", "");

        [TestMethod]
        public void HexPlusHex()
        {
            Assert.AreEqual(0x1+0x1, mp.Evaluate("0x1 + 0x1").Value);
        }

        [TestMethod]
        public void HexPlusNumber()
        {
            Assert.AreEqual(0x1 + 2, mp.Evaluate("0x1 + 2").Value);
        }

        [TestMethod]
        public void HexPlusNumberReversed()
        {
            Assert.AreEqual(0x1 + 1, mp.Evaluate("1 + 0x1").Value);
        }

        [TestMethod]
        public void HexPlusNumberString()
        {
            Assert.AreEqual("0x2", mp.Evaluate("1 + 0x1").ToString());
        }

        // TODO: Fix this test. Allow capacity digital to be represented as hex.
        // Currently an implementation limitation.
        [TestMethod]
        [ExpectedException(typeof(dab.Library.MathParser.UnitMismatchException))]
        public void HexPlusBits()
        {
            Assert.AreEqual(0x1 + 1, mp.Evaluate("0x04 + 1bit").Value);
        }

        // Todo: Redirect exception into our own MathParser Hex Format Exception
        [TestMethod]
        [ExpectedException(typeof(System.FormatException))]
        public void InvalidHex()
        {
            Assert.AreEqual(0x1 + 1, mp.Evaluate("0xZ04 + 1").Value);
        }
    }
}
