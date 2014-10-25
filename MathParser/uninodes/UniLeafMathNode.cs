using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dab.Library.MathParser
{
    public abstract class UniLeafMathNode : IMathNode
    {
        public UnitTypes UnitType { get; set; }

        /// <summary>
        /// Get the value of this node
        /// </summary>
        public object Value { get { return this.value; } }

        public UniLeafMathNode(object value)
        {
            this.value = value;
        }

        public UniLeafMathNode()
        {
        }

        /// <summary>
        /// Perform evaluation of its leaves recursively
        /// </summary>
        /// <returns>The calculated value</returns>
        public abstract UnitDouble Evaluate();
        /// <summary>
        /// Stores the value of this node
        /// </summary>
        protected object value;
    }
}
