using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using dab.Library.MathParser;

namespace MathParserUnitTest.Unit_Type_Tests
{
    [TestClass]
    public class CapacityDigitalTests
    {
        MathParser mp = new MathParser("", "");

        [TestMethod]
        public void BitAsBit()
        {
            var result = mp.Evaluate("1bit");

            Assert.AreEqual(1, result.Value);
            Assert.AreEqual("1 Bit", result.ToString());
        }

        [TestMethod]
        public void BitPlusBitNotLabelled()
        {
            var result = mp.Evaluate("1bit + 1");
            Assert.AreEqual(2, result.Value);
            Assert.AreEqual("2 Bits", result.ToString());
        }
        [TestMethod]
        public void BitPlusBit()
        {
            Assert.AreEqual(2, mp.Evaluate("1bit + 1 bit").Value);
        }

        [TestMethod]
        public void BytePlusBit()
        {
            var result = mp.Evaluate("1 byte + 1 bit");
            Assert.AreEqual(9, result.Value);
            Assert.AreEqual("1.125 Bytes", result.ToString());
        }

        [TestMethod]
        public void BytePlusByteNotLabelled()
        {
            Assert.AreEqual(9, mp.Evaluate("1 byte + 1").Value);
        }

        [TestMethod]
        public void MegabyteToByte()
        {
            var result = mp.Evaluate("1 megabyte to byte");
            Assert.AreEqual(8388608, result.Value);
            Assert.AreEqual(CapacityDigitalUnits.Byte, result.DesiredUnit);
        }

        [TestMethod]
        public void KiloByteAssumedLargest()
        {
            var result = mp.Evaluate("1023kb + 1kb");
            Assert.AreEqual(8388608, result.Value);
            Assert.AreEqual("1 Megabyte", result.ToString());
        }

        [TestMethod]
        [ExpectedException(typeof(UnitMismatchException))]
        public void BitPlusOtherUnit()
        {
            Assert.AreEqual(2, mp.Evaluate("1bit + 1 foot").Value);
        }

        [TestMethod]
        [ExpectedException(typeof(UnitMismatchException))]
        public void BitMinusOtherUnit()
        {
            Assert.AreEqual(2, mp.Evaluate("1bit - 1 foot").Value);
        }

        [TestMethod]
        [ExpectedException(typeof(UnitMismatchException))]
        public void BitMultiplyOtherUnit()
        {
            Assert.AreEqual(2, mp.Evaluate("1bit * 1 foot").Value);
        }

        [TestMethod]
        [ExpectedException(typeof(UnitMismatchException))]
        public void BitDivideOtherUnit()
        {
            Assert.AreEqual(2, mp.Evaluate("1bit / 1 foot").Value);
        }

        [TestMethod]
        [ExpectedException(typeof(UnitMismatchException))]
        public void BitBitwiseOrOtherUnit()
        {
            Assert.AreEqual(1000, mp.Evaluate("1bit | 1 foot").Value);
        }

        [TestMethod]
        [ExpectedException(typeof(UnitMismatchException))]
        public void BitBitwiseAndOtherUnit()
        {
            Assert.AreEqual(1000, mp.Evaluate("1bit & 1 foot").Value);
        }

        [TestMethod]
        [ExpectedException(typeof(UnitMismatchException))]
        public void BitShiftLeftOtherUnit()
        {
            Assert.AreEqual(2, mp.Evaluate("1bit << 1 foot").Value);
        }

        [TestMethod]
        [ExpectedException(typeof(UnitMismatchException))]
        public void BitShiftRightOtherUnit()
        {
            Assert.AreEqual(2, mp.Evaluate("1bit >> 1 foot").Value);
        }

        [TestMethod]
        [ExpectedException(typeof(UnitMismatchException))]
        public void ModulusOtherUnit()
        {
            Assert.AreEqual(2, mp.Evaluate("1bit % 1 foot").Value);
        }

        [TestMethod]
        public void CapacityLargeUnitToSmaller()
        {
            Assert.AreEqual("1 Byte", mp.Evaluate("(1/1024) kb").ToString());
        }
    }
}
