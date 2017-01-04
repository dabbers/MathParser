using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using dab.Library.MathParser;

namespace MathParserUnitTest.Unit_Type_Tests
{
    [TestClass]
    public class StringTest
    {
        MathParser mp = new MathParser("", "");
        [TestMethod]
        public void LongToStringTest()
        {
            Assert.AreEqual("helo", mp.Evaluate("1751477359 as string").ToString());
        }

        [TestMethod]
        public void HexToStringTest()
        {
            Assert.AreEqual("helo", mp.Evaluate("0x68656C6F as string").ToString());
        }

        [TestMethod]
        public void StringToLong()
        {
            Assert.AreEqual("1,751,477,359", mp.Evaluate("\"helo\" as decimal").ToString());


            Assert.AreEqual("\"helo\"", mp.GetInterpretation("\"helo\" as string"));
            Assert.AreEqual("\"helo\"", mp.GetInterpretation("\"helo\""));

            Assert.AreEqual("\"helo\" as Decimal", mp.GetInterpretation("\"helo\" as decimal"));
        }

        [TestMethod]
        public void StringToHex()
        {
            Assert.AreEqual("0x68656C6F", mp.Evaluate("\"helo\" as hex").ToString());
            Assert.AreEqual("0x68656C6C6F", mp.Evaluate("\"hello\" as hex").ToString());
            Assert.AreEqual("0x68656C6C6F6F6F6F", mp.Evaluate("\"helloooo\" as hex").ToString());
            Assert.AreEqual("0x68656C6C6F6F6F6F", mp.Evaluate("\"hellooooooooo\" as hex").ToString());
        }
    }
}
