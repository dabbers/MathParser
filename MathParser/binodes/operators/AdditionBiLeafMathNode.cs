
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dab.Library.MathParser
{
    public class AdditionBiLeafMathNode : SymbolMathNode
    {
        public AdditionBiLeafMathNode(IMathNode left, IMathNode right)
            : base(left, right, '+')
        {
        }

        public override UnitDouble Evaluate()
        {
            return left.Evaluate() + right.Evaluate();
        }
    }
}
