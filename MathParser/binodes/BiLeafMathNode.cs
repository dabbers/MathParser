
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dab.Library.MathParser
{
    /// <summary>
    /// Generic MathNode class containing basic items for a simple node
    /// </summary>
    public abstract class BiLeafMathNode : IMathNode
    {
        public UnitTypes UnitType { get; set; }
        /// <summary>
        /// Get the value of this node
        /// </summary>
        public object Value { get { return this.value; } }

        /// <summary>
        /// Get the left node on the binary tree.
        /// </summary>
        public IMathNode Left { get { return this.left; } }

        /// <summary>
        /// Get the right node on the binary tree.
        /// </summary>
        public IMathNode Right { get { return this.right; } }

        public BiLeafMathNode(IMathNode left, IMathNode right, object value)
        {
            this.left = left;
            this.right = right;
            this.value = value;
        }

        /// <summary>
        /// Perform evaluation of its leaves recursively
        /// </summary>
        /// <returns>The calculated value</returns>
        public abstract UnitDouble Evaluate();

        /// <summary>
        /// Get the left node on the binary tree.
        /// </summary>
        protected IMathNode left;

        /// <summary>
        /// Get the right node on the binary tree.
        /// </summary>
        protected IMathNode right;

        /// <summary>
        /// Stores the value of this node
        /// </summary>
        protected object value;
    }

}
