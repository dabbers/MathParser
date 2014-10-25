/**
 * David Barajas
 * 11329861
 * Cpts 322
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dab.Library.MathParser
{
    public class DivisionBiLeafMathNode : SymbolMathNode
    {
        public DivisionBiLeafMathNode(IMathNode left, IMathNode right)
            : base(left, right, '/')
        {
        }

        public override UnitDouble Evaluate()
        {
            var denom = right.Evaluate();

            if (0 == denom.Value)
            {
                // For sake of debugging in the future, I want to track MathParser thrown exceptions
                // vs system thrown ones. Will help tell between a flaw vs a bug
                throw new dab.Library.MathParser.DivideByZeroException();
            }

            return left.Evaluate() / right.Evaluate();
        }
    }
}
