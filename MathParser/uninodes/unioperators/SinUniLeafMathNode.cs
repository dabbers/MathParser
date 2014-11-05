using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dab.Library.MathParser
{
    public class SinUniLeafMathNode : UniLeafMathNode
    {
        public IMathNode Inner { get; private set; }

        public SinUniLeafMathNode(IMathNode inner)
            :base(null)
        {
            this.Inner = inner;
        }

        /// <summary>
        /// Perform evaluation of its leaves recursively
        /// </summary>
        /// <returns>The calculated value</returns>
        public override UnitDouble Evaluate()
        {
            var tmp = this.Inner.Evaluate();
            return new UnitDouble((decimal)Math.Sin((double)tmp.Value), tmp.UnitType, tmp.Unit, tmp.Converter);
        }
        public override string ToString()
        {
            return "Sin(" + this.Inner.ToString().TrimOuterParens() + ")";
        }
    }
}
