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

            Assert.AreEqual((decimal)1/8, result.Value);
            Assert.AreEqual("1 Bit", result.ToString());
        }

        [TestMethod]
        public void BitPlusByteNotLabelled() // bit + default (byte)
        {
            var result = mp.Evaluate("1bit + 1");
            Assert.AreEqual(1 + (decimal)1/8, result.Value);
            Assert.AreEqual("1.125 Bytes", result.ToString());
        }
        [TestMethod]
        public void BitPlusBit()
        {
            Assert.AreEqual((decimal)2/8, mp.Evaluate("1bit + 1 bit").Value);
        }

        [TestMethod]
        public void BytePlusBit()
        {
            var result = mp.Evaluate("1 byte + 1 bit");
            Assert.AreEqual((decimal)1.125, result.Value);
            Assert.AreEqual("1.125 Bytes", result.ToString());
        }

        [TestMethod]
        public void BytePlusByteNotLabelled()
        {
            Assert.AreEqual(2, mp.Evaluate("1 byte + 1").Value);
        }

        [TestMethod]
        public void MegabyteToByte()
        {
            var result = mp.Evaluate("1 megabyte to byte");
            Assert.AreEqual(8388608/8, result.Value);
            Assert.AreEqual(CapacityDigitalUnits.Byte, result.DesiredUnit);
        }

        [TestMethod]
        public void KiloByteAssumedLargest()
        {
            var result = mp.Evaluate("1023kb + 1kb");
            Assert.AreEqual(8388608/8, result.Value);
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

        /*
         * Do 1terabyte / 1mb and you get 128 kb which is incorrect.

(1 Terabyte) as Byte = 1,099,511,627,776 Bytes
 1 MB = 1,048,576 Bytes

Divide those and you get 1,048,576 which is 1 MB. 
*/

        [TestMethod]
        public void Capacity1TbTo1MbCheck()
        {
            Assert.AreEqual("1 Megabyte", mp.Evaluate("1 terabyte / 1mb").ToString());
        }

    }
}
