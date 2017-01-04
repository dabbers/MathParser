using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dab.Library.MathParser
{
    public class EvaluationTree
    {
        /// <summary>
        /// The root of our node.
        /// </summary>
        public IMathNode Root { get; set; }

        public EvaluationTree(IMathNode root)
        {
            this.Root = root;
            if (this.Root == null)
            {
                this.Root = new NumericMathNode(0);
            }

            this.Root.Evaluate(); // I guess I need to do this once?
        }

        public UnitDouble Evaluate()
        {

            return this.Root.Evaluate();
        }

        public string GetInterpretation()
        {
            return this.Root.ToString().TrimOuterParens();
        }

        public override string ToString()
        {
            return this.GetInterpretation() + " = " + this.Evaluate().ToString();
        }
    }
}
