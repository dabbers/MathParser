using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dab.Library.MathParser
{
    internal class UniLeafFactory
    {
        public string[] ParsePotentialUniLeaf(string expression)
        {
            expression = expression.Replace(" ", "");

            var functionName = new StringBuilder();
            var functiondone = false;
            var currentParam = new StringBuilder();

            List<string> res = new List<string>();

            int parendepth = 0;

            for(int i = 0; i < expression.Length; i++)
            {
                if (Char.IsLetterOrDigit(expression[i]))
                {
                    if (false == functiondone)
                    {
                        functionName.Append(expression[i]);
                    }
                    else
                    {
                        currentParam.Append(expression[i]);
                    }
                }
                else if (expression[i] == '(' && false == functiondone)
                {
                    functiondone = true;
                    res.Add(functionName.ToString().ToLower());
                }
                else if (expression[i] == '(' && true == functiondone)
                {
                    // we have something else in here...
                    parendepth++;
                    currentParam.Append(expression[i]);
                }
                else if (expression[i] == ')' && 0 != parendepth)
                {
                    parendepth--;
                    currentParam.Append(expression[i]);
                }
                else if ((expression[i] == ',' || expression[i] == ')') && 0 == parendepth)
                {
                    if (currentParam.Length > 0)
                    {
                        res.Add(currentParam.ToString());
                        currentParam.Clear();
                    }
                    else
                    {
                        throw new InvalidFunctionArgument(expression);
                    }
                }
                else if (true == functiondone)
                {
                    currentParam.Append(expression[i]);
                    // nothing for now
                }

            }
            
            if (currentParam.Length > 0)
            {
                res.Clear();
            }

            return res.ToArray();
            
        }

        public IMathNode CreateUniLeafNode(string expression, MathParser parser)
        {
            var parts = this.ParsePotentialUniLeaf(expression);
            
            if (parts.Length == 0)
                return null;

            switch(parts[0])
            {
                case "log":
                    if (parts.Count() != 3)
                    {
                        throw new InvalidArgumentAmount(parts[0]);
                    }

                    return new LogUniLeafMathNode(parser.Parse(parts[2]), parser.Parse(parts[1]));
                case "sin":
                    if (parts.Count() != 2)
                    {
                        throw new InvalidArgumentAmount(parts[0]);
                    }

                    return new SinUniLeafMathNode(parser.Parse(parts[1]));
                case "cos":
                    if (parts.Count() != 2)
                    {
                        throw new InvalidArgumentAmount(parts[0]);
                    }

                    return new CosUniLeafMathNode(parser.Parse(parts[1]));
                case "asin":
                    if (parts.Count() != 2)
                    {
                        throw new InvalidArgumentAmount(parts[0]);
                    }

                    return new ASinUniLeafMathNode(parser.Parse(parts[1]));
                case "acos":
                    if (parts.Count() != 2)
                    {
                        throw new InvalidArgumentAmount(parts[0]);
                    }

                    return new ACosUniLeafMathNode(parser.Parse(parts[1]));
                case "cosh":
                    if (parts.Count() != 2)
                    {
                        throw new InvalidArgumentAmount(parts[0]);
                    }

                    return new CoshUniLeafMathNode(parser.Parse(parts[1]));
                case "tan":
                    if (parts.Count() != 2)
                    {
                        throw new InvalidArgumentAmount(parts[0]);
                    }

                    return new TanUniLeafMathNode(parser.Parse(parts[1]));
                case "atan":
                    if (parts.Count() != 2)
                    {
                        throw new InvalidArgumentAmount(parts[0]);
                    }

                    return new ATanUniLeafMathNode(parser.Parse(parts[1]));
                case "sinh":
                    if (parts.Count() != 2)
                    {
                        throw new InvalidArgumentAmount(parts[0]);
                    }

                    return new SinhUniLeafMathNode(parser.Parse(parts[1]));
                case "log2":
                    if (parts.Count() != 2)
                    {
                        throw new InvalidArgumentAmount(parts[0]);
                    }

                    return new LogUniLeafMathNode(new NumericMathNode(2.0M), parser.Parse(parts[1]));
                case "log10":
                    if (parts.Count() != 2)
                    {
                        throw new InvalidArgumentAmount(parts[0]);
                    }

                    return new LogUniLeafMathNode(new NumericMathNode(10.0M), parser.Parse(parts[1]));
                case "sqrt":
                    if (parts.Count() != 2)
                    {
                        throw new InvalidArgumentAmount(parts[0]);
                    }

                    return new SqrtUniLeafMathNode(parser.Parse(parts[1]));
            }
            return null;
        }
    }
}
