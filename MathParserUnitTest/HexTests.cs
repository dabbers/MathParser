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
        public void HexAsDecimal()
        {
            Assert.AreEqual("1", mp.Evaluate("0x1 as decimal").ToString());
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

        [TestMethod]
        public void OctalTest1()
        {
            // 10 + 13 = 23
            Assert.AreEqual("027", mp.Evaluate("012 + 015").ToString());
        }

        [TestMethod]
        public void OctalPlusHex()
        {
            // 10 + 13 = 23
            Assert.AreEqual("013", mp.Evaluate("012 + 0x1").ToString());
        }

        [TestMethod]
        public void HexPlusOct()
        {
            // 10 + 13 = 23
            Assert.AreEqual("0xB", mp.Evaluate("0x1 + 012").ToString());
        }

        [TestMethod]
        public void OctTestConvertHex()
        {
            // 10 + 13 = 23
            Assert.AreEqual("0x17", mp.Evaluate("(012 + 015) as Hex").ToString());
        }

        [TestMethod]
        public void HexPlusHexAsEcimal()
        {
            Assert.AreEqual("2", mp.Evaluate("(0x1 + 0x1) as Decimal").ToString());
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
        [ExpectedException(typeof(InvalidMathExpressionException))]
        public void InvalidHex()
        {
            Assert.AreEqual(0x1 + 1, mp.Evaluate("0xZ04 + 1").Value);
        }

        [TestMethod]
        public void BinaryTest()
        {
            Assert.AreEqual(3, mp.Evaluate("0b11").Value);
        }

        [TestMethod]
        public void BinaryTest2()
        {
            Assert.AreEqual(6, mp.Evaluate("0b11 + 3").Value);
        }

        [TestMethod]
        public void HexPlusBinary()
        {
            Assert.AreEqual(4, mp.Evaluate("0b11 + 0x01").Value);
        }

        [TestMethod]
        public void CastToBinary()
        {
            Assert.AreEqual("0b11", mp.Evaluate("(0b10 + 0x01) as Binary").ToString());
        }

        [TestMethod]
        public void GetInterpretation()
        {
            Assert.AreEqual("(0x13) as Binary", mp.GetInterpretation("0x13 as binary"));
            Assert.AreEqual("45 as Binary", mp.GetInterpretation("45 as binary"));
        }
    }
}
