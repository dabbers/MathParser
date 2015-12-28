using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using dab.Library.MathParser;

namespace MathParserUnitTest
{
    [TestClass]
    public class BiOperatorTests
    {
        MathParser mp = new MathParser("", "");

        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentAmount))]
        public void AdditionSideNull()
        {
            SymbolMathNode n = new AdditionBiLeafMathNode(null, null);

            Assert.IsNull(n.Left);
            Assert.IsNull(n.Right);

            n.Evaluate();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentAmount))]
        public void BitwiseAndSideNull()
        {
            SymbolMathNode n = new BitwiseAndBiLeafMathNode(null, null);

            Assert.IsNull(n.Left);
            Assert.IsNull(n.Right);

            n.Evaluate();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentAmount))]
        public void BitwiseOrSideNull()
        {
            SymbolMathNode n = new BitwiseOrBiLeafMathNode(null, null);

            Assert.IsNull(n.Left);
            Assert.IsNull(n.Right);

            n.Evaluate();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentAmount))]
        public void BitwiseShiftLeftSideNull()
        {
            SymbolMathNode n = new BitwiseShiftLeftBiLeafMathNode(null, null);

            Assert.IsNull(n.Left);
            Assert.IsNull(n.Right);

            n.Evaluate();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentAmount))]
        public void BitwiseShiftRightSideNull()
        {
            SymbolMathNode n = new BitwiseShiftRightBiLeafMathNode(null, null);

            Assert.IsNull(n.Left);
            Assert.IsNull(n.Right);

            n.Evaluate();
        }
    }
}
