
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dab.Library.MathParser
{
    public delegate double GetValueOfVariable(string variable );

    /// <summary>
    /// Parse a math equation, storing info about the expression such as Variables and Symbols
    /// </summary>
    public class MathParser
    {
        /// <summary>
        /// The root of our node.
        /// </summary>
        public IMathNode Root { get; set; }

        public GetValueOfVariable GetValueOfVariable { get; set; }

        /// <summary>
        /// Stores all the possible symbols that have a value
        /// </summary>
        public IDictionary<string, decimal> Symbols { get { return this.symbols; } }

        /// <summary>
        /// Store a list of variables that are referenced in the expression.
        /// </summary>
        public List<string> Variables { get; set; }

        public UnitFactory UnitFactory;
        
        public MathParser(string currency, string cache)
            : this(new Dictionary<string, decimal>(), currency, cache)
        {
        }

        /// <summary>
        /// Create a new math parser
        /// </summary>
        /// <param name="symbols">A list of symbols to pass into the </param>
        public MathParser(IDictionary<string, decimal> symbols, string currency, string cache)
        {
            this.symbols = symbols;
            this.UnitFactory = new UnitFactory(currency, cache);

            if (!this.symbols.ContainsKey("pi"))
            {
                this.symbols.Add("pi", (decimal)Math.PI);
                this.symbols.Add("PI", (decimal)Math.PI);
            }
            if (!this.symbols.ContainsKey("e"))
            {
                this.symbols.Add("e", (decimal)Math.E);
                this.symbols.Add("E", (decimal)Math.E);
            }
        }

        protected IDictionary<string, decimal> symbols;

        private System.Text.RegularExpressions.Regex clearSpace = new System.Text.RegularExpressions.Regex(@"([-+*/])(\s+)([-+*/])");
        /// <summary>
        /// Evaluate an expression. Do not call Parse as this will call parse itself.
        /// </summary>
        /// <param name="expression">The expression you wish to get a value of</param>
        /// <returns>The value from the expression</returns>
        public UnitDouble Evaluate(string expression)
        {
            //expression = expression.Replace(" ", "");
            expression = clearSpace.Replace(expression, "$1$3");
            this.Root = this.Parse(expression);

            return evaluate(this.Root);
        }

        private UnitDouble evaluate(IMathNode node)
        {
            if (node == null)
            {
                return new UnitDouble(0);
            }

            return node.Evaluate();
        }
        /// <summary>
        /// Parse an expression. left and right values and stores the whole expression into the Root.
        /// </summary>
        /// <param name="expression">Valid expression. Exception will be thrown if the expression is invalid.</param>
        public IMathNode Parse(string expression)
        {
            this.Variables = new List<string>();
            return parse(expression);
        }

        private IMathNode parse(string expression)
        {
            if (String.IsNullOrEmpty(expression))
                return null;

            // Remove outer parenthesis
            while(expression[0] == '(' && expression.Last() == ')')
            {
                // Ensure the last parenthesis belongs to the first
                int parenCount = 0;

                int pos = expression.Length - 1;

                for(; pos >= 0; pos--)
                {
                    if (expression[pos] == ')')
                    {
                        parenCount++;
                    }
                    else if (expression[pos] == '(')
                    {
                        parenCount--;
                    }

                    if ((parenCount == 0) && pos > 0) break;
                }

                // If our first and last parenthesis are together, remove them
                if (pos == -1)
                {
                    expression = expression.Substring(1, expression.Length - 2);
                }
                else // We can't perform any removal today.
                {
                    break;
                }
            }
            
            short lowestop = -1;
            int oppos = -1;
            int parens = 0;

            // Check for misplaced operators ( + - / * ^ )
            if (this.invalidPreceedingOperators.Keys.Contains(expression[0]) || this.invalidPreceedingOperators.Keys.Contains(expression.Last()))
            {
                throw new InvalidMathExpressionException(expression);
            }

            int startPos = 0;

            if (expression[0] == '-') startPos++;
            int lastNonWhiteSpace = -1;

            // get first smallest operator and parse left and right
            for (int pos = expression.Length - 1; pos >= startPos; pos--)
            {
                short opval;
                char tmp = expression[pos];

                if (expression[pos] == ')')
                {
                    lastNonWhiteSpace = pos;
                    parens++;
                }

                else if (expression[pos] == '(')
                {
                    lastNonWhiteSpace = pos;
                    parens--;
                }

                if (parens < 0)
                {
                    throw new InvalidMathExpressionException(expression);
                }

                if (!this.operators.ContainsKey(expression[pos]))
                {
                    if (!Char.IsWhiteSpace(expression[pos]))
                    {
                        lastNonWhiteSpace = pos;
                    }
                    continue;
                }

                if (this.invalidPreceedingOperators.ContainsKey(expression[pos]) && this.operators.ContainsKey(expression[pos - 1]))
                {
                    throw new InvalidMathExpressionException("OP: " + expression);
                }

                opval = this.operators[expression[pos]];

                if (parens != 0 || lowestop > opval)
                {
                    // check for a - sign after this just in case.
                    if (expression[pos + 1] != '-')
                    {
                        continue;
                    }
                }

                lowestop = opval;
                oppos = pos;
            }

            if (parens != 0)
            {
                throw new InvalidMathExpressionException(expression);
            }

            //expression = expression.Trim();
            // No opperator was found, we are parsing something else
            if (oppos == -1)
            {
                decimal dbl;

                if (decimal.TryParse(expression.Trim(), out dbl))
                {
                    return new NumericMathNode(dbl);
                }
                else // This isn't a number... So it must be a function or variable
                {
                    // expression stores our function(args)

                    var node = uniLeafFact.CreateUniLeafNode(expression.Trim(), this);
                    if (null == node)
                    {
                        decimal val;
                        // Check for a constant if not a function
                        if (symbols.TryGetValue(expression.Trim(), out val))
                        {
                            node = new NumericMathNode(val);
                        }
                        else
                        {
                            // Check for unit label if not a function nor a constant
                            var result = UnitFactory.TryParse(expression, this);

                            if (result != null)
                            {
                                return result;
                            }

                            if (expression[0] == '-')
                            {
                                return new MultiplicationBiLeafMathNode(new NumericMathNode(-1), this.Parse(expression.Substring(1)));
                            }

                            //DistanceConverter.Convert()
                            throw new InvalidMathExpressionException(expression);
                        }
                    }

                    return node;
                }
            }
            else
            {
                string left = String.Empty;

                if (oppos-1 > 0)
                {
                    left = expression.Substring(0, oppos);
                }
                else
                {
                    left = expression[0].ToString();
                }

                char smbl = expression[oppos];

                string right = expression.Substring(oppos + 1, expression.Length - oppos - 1);

                return operFact.CreateOperatorNode(smbl, this.parse(left), this.parse(right));
            }
        }

        private UniLeafFactory uniLeafFact = new UniLeafFactory();
        private OperatorFactory operFact = new OperatorFactory();
        private Dictionary<char, short> operators = new Dictionary<char, short>() { { '+', 5 }, { '-', 4 }, { '/', 3 }, { '*', 2 }, { '^', 1 } };
        private Dictionary<char, short> invalidPreceedingOperators = new Dictionary<char, short>() { { '+', 4 }, { '/', 3 }, { '*', 2 }, { '^', 1 } };
    }

#region Exceptions

    public class MathParserException : Exception
    {
        public MathParserException(string msg)
            : base(msg)
        { 
        }
    }

    /// <summary>
    /// Exception to be thrown when an invalid expression is parsed
    /// </summary>
    public class InvalidMathExpressionException : MathParserException
    {
        /// <summary>
        /// The expression that failed to be parsed
        /// </summary>
        public string Expression { get { return this.expression; } }

        public InvalidMathExpressionException(string expression)
            : base(String.Format("The following math expression is invalid: {0}", expression))
        {
            this.expression = expression;
        }

        private string expression = String.Empty;
    }

    public class InvalidOperatorException : MathParserException
    {
        public char Operator { get; private set; }
        public string Expression { get; private set; }

        public InvalidOperatorException(char c, string expression)
            :base(String.Format("Invalid operator {0} for expression {1}", c, expression))
        {
            Operator = c;
            Expression = expression;
        }
    }

    public class InvalidFunctionArgument : MathParserException
    {
        public InvalidFunctionArgument(string expression)
            : base(String.Format("{0} contains an improperly formatted argument", expression))
        {
        }
    }

    public class InvalidArgumentAmount : MathParserException
    {
        public InvalidArgumentAmount(string expression)
            : base(String.Format("{0} was passed an invalid number of arguments", expression))
        {
        }
    }

    public class UnitMismatchException : MathParserException
    {
        public string LeftUnit { get; private set; }
        public string RightUnit { get; private set; }

        public UnitMismatchException(string leftUnit, string rightUnit)
            :base(String.Format("Unit mismatch of {0} and {1}", leftUnit, rightUnit))
        {
            this.LeftUnit = leftUnit;
            this.RightUnit = rightUnit;
        }
    }
    public class UnexpectedUnitException : MathParserException
    {
        public string UnitType { get; private set; }
        public string UnitLabel { get; private set; }

        public UnexpectedUnitException(string unitType, string unitLabel)
            : base(String.Format("Unexpected unit of type {0} labelled as {1}", unitType, unitLabel))
        {
            this.UnitType = unitType;
            this.UnitLabel = unitLabel;
        }
    }

    public class InvalidUnitTypeException : MathParserException
    {
        public string Unit { get; private set; }

        public InvalidUnitTypeException(string unit)
            : base(String.Format("Invalid Unit Type: {0}", unit))
        {
            this.Unit = unit;
        }

    }

    public class MissingVariableException : MathParserException
    {
        public MissingVariableException(string variable)
            : base(String.Format("Cannot find a definition for variable: {0}", variable))
        {
        }
    }

    public class InvalidMathNodeException : MathParserException
    {
        public string NodeType { get; private set; }
        public InvalidMathNodeException(string node)
            : base(String.Format("This node should not be called here: {0}", node))
        {
            this.NodeType = node;
        }
    }

    public class DivideByZeroException : MathParserException
    {
        public DivideByZeroException()
            : base("Cannot perform division with zero in the denominator")
        {
        }
    }
#endregion
}
