
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dab.Library.MathParser
{
    public class NumericMathNode : UniLeafMathNode
    {

        public NumericMathNode(decimal value)
        {
            this.value = new UnitDouble(value);
        }
        public NumericMathNode(UnitDouble value)
        {
            this.value = value;
        }

        public override UnitDouble Evaluate()
        {
            return (UnitDouble)this.value;
        }
    }
}
