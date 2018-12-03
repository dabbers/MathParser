
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dab.Library.MathParser
{
    public class VariableMathNode : UniLeafMathNode
    {
        public VariableMathNode(string variable)
            : base(variable)
        {
        }

        public override UnitDouble Evaluate()
        {
            throw new Exception(String.Format("VariableMathNode ({0})", this.Value.ToString()));
        }
    }
}
