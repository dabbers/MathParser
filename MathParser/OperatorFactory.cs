using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dab.Library.MathParser
{
    internal class OperatorFactory
    {
        /// <summary>
        /// Create an operator node based on a character
        /// </summary>
        /// <param name="c">The operator being + - * / or ^</param>
        /// <param name="left">The left part of the operator</param>
        /// <param name="right">The right part of the operator</param>
        /// <returns>Valid operators return IMathNode, null if no operator</returns>
        public IMathNode CreateOperatorNode(char c, IMathNode left, IMathNode right)
        {
            switch(c)
            {
                case '+':
                    return new AdditionBiLeafMathNode(left, right);
                case '-':
                    return new SubtractionBiLeafMathNode(left, right);
                case '*':
                    return new MultiplicationBiLeafMathNode(left, right);
                case '/':
                    return new DivisionBiLeafMathNode(left, right);
                case '^':
                    return new ExponentBiLeafMathNode(left, right);
                default:
                    return null;
            }
        }
    }
}
