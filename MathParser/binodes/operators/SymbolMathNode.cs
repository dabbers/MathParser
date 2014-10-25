
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dab.Library.MathParser
{
    public abstract class SymbolMathNode : BiLeafMathNode
    {
        public SymbolMathNode(IMathNode left, IMathNode right, char symbol)
            : base(left, right, symbol)
        {
        }

    }
}
