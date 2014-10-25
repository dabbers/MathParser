
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dab.Library.MathParser
{
    public class ExponentBiLeafMathNode : SymbolMathNode
    {
        public ExponentBiLeafMathNode(IMathNode left, IMathNode right)
            : base(left, right, '^')
        {
        }

        public override UnitDouble Evaluate()
        {
            var left = this.left.Evaluate();
            var right = this.right.Evaluate();

            return new UnitDouble((decimal)Math.Pow((double)left.Value, (double)right.Value), left.UnitType, left.Unit, left.Converter);
        }
    }
}
