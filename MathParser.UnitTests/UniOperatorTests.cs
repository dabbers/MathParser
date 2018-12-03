using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using dab.Library.MathParser;

namespace MathParserUnitTest
{
    [TestClass]
    public class UniOperatorTests
    {
        MathParser mp = new MathParser("", "");

        [TestMethod]
        public void ACosUnitTestValid()
        {
            Assert.AreEqual((decimal)Math.Acos(.001), mp.Evaluate("Acos(.001)").Value);
            Assert.AreEqual("1.569796", mp.Evaluate("Acos(.001)").ToString());
        }

        [TestMethod]
        [ExpectedException(typeof(dab.Library.MathParser.InvalidArgumentAmount))]
        public void ACosUnitTestInvalid()
        {
            mp.Evaluate("Acos(15, 17)");
        }

        [TestMethod]
        public void ASinUnitTestValid()
        {
            var result = mp.Evaluate("Asin(.001)");

            Assert.AreEqual((decimal)Math.Asin(.001), result.Value);

            Assert.AreEqual("0.001", result.ToString());
        }

        [TestMethod]
        [ExpectedException(typeof(dab.Library.MathParser.InvalidArgumentAmount))]
        public void ASinUnitTestInvalid()
        {
            mp.Evaluate("Asin(15, 17)");
        }

        [TestMethod]
        public void ATanUnitTestValid()
        {
            Assert.AreEqual((decimal)Math.Atan(.001), mp.Evaluate("ATan(.001)").Value);
            Assert.AreEqual("0.001", mp.Evaluate("ATan(.001)").ToString());
        }

        [TestMethod]
        [ExpectedException(typeof(dab.Library.MathParser.InvalidArgumentAmount))]
        public void ATanUnitTestInvalid()
        {
            mp.Evaluate("ATan(15, 17)");
        }

        [TestMethod]
        public void CoshUnitTestValid()
        {
            Assert.AreEqual((decimal)Math.Cosh(.001), mp.Evaluate("Cosh(.001)").Value);
        }

        [TestMethod]
        [ExpectedException(typeof(dab.Library.MathParser.InvalidArgumentAmount))]
        public void CoshUnitTestInvalid()
        {
            mp.Evaluate("Cosh(15, 17)");
        }

        [TestMethod]
        public void CosUnitTestValid()
        {
            Assert.AreEqual((decimal)Math.Cos(.001), mp.Evaluate("Cos(.001)").Value);
        }

        [TestMethod]
        [ExpectedException(typeof(dab.Library.MathParser.InvalidArgumentAmount))]
        public void CosUnitTestInvalid()
        {
            mp.Evaluate("Cos(15, 17)");
        }

        [TestMethod]
        public void LogUnitTestValid()
        {
            Assert.AreEqual((decimal)Math.Log(3, 2), mp.Evaluate("Log(3, 2)").Value);
        }

        [TestMethod]
        [ExpectedException(typeof(dab.Library.MathParser.InvalidArgumentAmount))]
        public void LogUnitTestInvalidMore()
        {
            mp.Evaluate("Log(15, 17, 15)");
        }

        [TestMethod]
        [ExpectedException(typeof(dab.Library.MathParser.InvalidArgumentAmount))]
        public void LogUnitTestInvalidLess()
        {
            mp.Evaluate("Log(15)");
        }

        [TestMethod]
        public void SinhUnitTestValid()
        {
            Assert.AreEqual((decimal)Math.Sinh(.001), mp.Evaluate("Sinh(.001)").Value);
        }

        [TestMethod]
        [ExpectedException(typeof(dab.Library.MathParser.InvalidArgumentAmount))]
        public void SinhUnitTestInvalid()
        {
            mp.Evaluate("Sinh(15, 17)");
        }

        [TestMethod]
        [ExpectedException(typeof(dab.Library.MathParser.InvalidArgumentAmount))]
        public void SinUnitTestInvalid()
        {
            mp.Evaluate("Sin(15, 17)");
        }

        [TestMethod]
        public void SinUnitTestValid()
        {
            Assert.AreEqual((decimal)Math.Sin(.001), mp.Evaluate("Sin(.001)").Value);
        }

        
        [TestMethod]
        [ExpectedException(typeof(dab.Library.MathParser.InvalidArgumentAmount))]
        public void TanUnitTestInvalid()
        {
            mp.Evaluate("Tan(15, 17)");
        }

        [TestMethod]
        public void TanUnitTestValid()
        {
            Assert.AreEqual((decimal)Math.Tan(.001), mp.Evaluate("Tan(.001)").Value);
        }

        [TestMethod]
        public void Log10UnitTest()
        {
           Assert.AreEqual((decimal)Math.Log10(15), mp.Evaluate("log10(15)").Value);
        }

        [TestMethod]
        [ExpectedException(typeof(dab.Library.MathParser.InvalidArgumentAmount))]
        public void Log10UnitTestInvalid()
        {
            mp.Evaluate("log10(15, 17)");
        }


        [TestMethod]
        public void Log2UnitTest()
        {
            Assert.AreEqual((decimal)Math.Log(15, 2), mp.Evaluate("log2(15)").Value);
        }

        [TestMethod]
        [ExpectedException(typeof(dab.Library.MathParser.InvalidArgumentAmount))]
        public void Log2UnitTestInvalid()
        {
            mp.Evaluate("log2(15, 17)");
        }

        [TestMethod]
        [ExpectedException(typeof(dab.Library.MathParser.InvalidArgumentAmount))]
        public void SqrtUnitTestInvalid()
        {
            mp.Evaluate("Sqrt(15, 17)");
        }
    }
}
