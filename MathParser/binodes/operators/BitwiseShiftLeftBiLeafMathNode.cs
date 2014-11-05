using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dab.Library.MathParser
{
    class BitwiseShiftLeftBiLeafMathNode : SymbolMathNode
    {
        public BitwiseShiftLeftBiLeafMathNode(IMathNode left, IMathNode right)
            : base(left, right, "<<")
        {
        }

        public override UnitDouble Evaluate()
        {
            return left.Evaluate() << (int)right.Evaluate().Value;
        }
    }
}
