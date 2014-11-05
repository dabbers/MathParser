using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dab.Library.MathParser
{
    static class RemoveOuterParens
    {
        internal static string TrimOuterParens(this string exp)
        {
            var expression = exp;

            // Remove outer parenthesis
            while (expression[0] == '(' && expression.Last() == ')')
            {
                // Ensure the last parenthesis belongs to the first
                int parenCount = 0;

                int pos = expression.Length - 1;

                for (; pos >= 0; pos--)
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

            return expression;
        }
    }
}
