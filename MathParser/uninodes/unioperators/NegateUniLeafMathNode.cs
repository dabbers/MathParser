using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dab.Library.MathParser
{
    public class NegateUniLeafMathNode : UniLeafMathNode
    {
        public IMathNode Inner { get; private set; }

        public NegateUniLeafMathNode(IMathNode inner)
            :base(null)
        {
            this.Inner = inner;
        }

        /// <summary>
        /// Perform evaluation of its leaves recursively
        /// </summary>
        /// <returns>The calculated value</returns>
        public override UnitDouble Evaluate()
        {
            if (null == this.Inner) throw new dab.Library.MathParser.MathParserException("Bitwise Negate needs a number");

            var tmp = this.Inner.Evaluate();
            return ~tmp;
        }

        public override string ToString()
        {
            return "~(" + this.Inner.ToString().TrimOuterParens() + ")";
        }
    }
}
