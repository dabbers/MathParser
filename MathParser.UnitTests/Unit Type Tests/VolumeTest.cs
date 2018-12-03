using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using dab.Library.MathParser;

namespace MathParserUnitTest.Unit_Type_Tests
{
    [TestClass]
    public class VolumeTest
    {
        MathParser mp = new MathParser("", "");

        [TestMethod]
        [ExpectedException(typeof(UnexpectedUnitException))]
        public void VolumeGallonToWeightOunces()
        {
            Assert.AreEqual("128.000984 Ounces", mp.Evaluate("1 gallon to ounces").ToString());
        }

        [TestMethod]
        public void VolumeGallonToFluidOunces()
        {
            Assert.AreEqual("128.000984 Ounces", mp.Evaluate("1 gallon to floz").ToString());
        }

        [TestMethod]
        public void VolumeMilliliterToCubicMM()
        {
            Assert.AreEqual((1/1000M).ToString() + " Cubic MilliMeters", mp.Evaluate("1 ml to mmc").ToString());
        }
    }
}
