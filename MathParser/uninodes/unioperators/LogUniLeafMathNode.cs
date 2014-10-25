using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dab.Library.MathParser
{
    public class LogUniLeafMathNode : UniLeafMathNode
    {
        public IMathNode Base { get; private set; }
        public IMathNode Inner { get; private set; }

        public LogUniLeafMathNode(IMathNode logbase, IMathNode inner)
            :base(null)
        {
            this.Base = logbase;
            this.Inner = inner;
        }

        /// <summary>
        /// Perform evaluation of its leaves recursively
        /// </summary>
        /// <returns>The calculated value</returns>
        public override UnitDouble Evaluate()
        {
            var tmp = this.Inner.Evaluate();
            var bse = this.Base.Evaluate();

            return new UnitDouble((decimal)Math.Log((double)tmp.Value, (double)bse.Value), tmp.UnitType, tmp.Unit, tmp.Converter);
        }
    }
}
