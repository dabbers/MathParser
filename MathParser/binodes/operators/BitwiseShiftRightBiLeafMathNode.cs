using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dab.Library.MathParser
{
    class BitwiseShiftRightBiLeafMathNode : SymbolMathNode
    {
        public BitwiseShiftRightBiLeafMathNode(IMathNode left, IMathNode right)
            : base(left, right, ">>")
        {
        }

        public override UnitDouble Evaluate()
        {
            if (null == this.left || null == this.right) throw new dab.Library.MathParser.InvalidArgumentAmount("One or more required arguments are empty");

            var lft = left.Evaluate();
            var rt = right.Evaluate();

            if (rt.UnitType != lft.UnitType && rt.UnitType != UnitTypes.None) throw new UnitMismatchException(lft.UnitType.ToString(), rt.UnitType.ToString());

            return lft >> (int)rt.Value;
        }
    }
}
