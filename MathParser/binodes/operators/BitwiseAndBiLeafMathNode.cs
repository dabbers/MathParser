using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dab.Library.MathParser
{
    public class BitwiseAndBiLeafMathNode : SymbolMathNode
    {
        public BitwiseAndBiLeafMathNode(IMathNode left, IMathNode right)
            : base(left, right, '&')
        {
        }

        public override UnitDouble Evaluate()
        {
            if (null == this.left || null == this.right) throw new dab.Library.MathParser.InvalidArgumentAmount("One or more required arguments are empty");

            return left.Evaluate() & right.Evaluate();
        }
    }
}
