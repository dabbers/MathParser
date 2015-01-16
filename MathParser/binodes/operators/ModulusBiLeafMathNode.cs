using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dab.Library.MathParser
{
    public class ModulusBiLeafMathNode : SymbolMathNode
    {
        public ModulusBiLeafMathNode(IMathNode left, IMathNode right)
            : base(left, right, '%')
        {
        }

        public override UnitDouble Evaluate()
        {
            if (null == this.left || null == this.right) throw new dab.Library.MathParser.InvalidArgumentAmount("One or more required arguments are empty");

            var denom = right.Evaluate();

            if (0 == denom.Value)
            {
                // For sake of debugging in the future, I want to track MathParser thrown exceptions
                // vs system thrown ones. Will help tell between a flaw vs a bug
                throw new dab.Library.MathParser.DivideByZeroException();
            }

            return left.Evaluate() % right.Evaluate();
        }
    }
}
