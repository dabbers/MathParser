using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dab.Library.MathParser
{
    public class StringMathNode : UniLeafMathNode
    {
        public StringMathNode(IMathNode inner)
        {
            this.value = inner;
        }
        public StringMathNode(string inner)
        {
            this.value = inner;
        }

        public override UnitDouble Evaluate()
        {

            if (this.value is IMathNode)
            {
                return new StringFromDouble(((IMathNode)this.value).Evaluate());
            }
            else if (this.value is string)
            {
                var bytez = System.Text.Encoding.UTF8.GetBytes((string)this.value).Reverse().ToArray();
                byte[] bytez2 = new byte[sizeof(long)];

                //var offset = ;
                var offset = Math.Max(0, (bytez.Length - 1) - (bytez2.Length - 1));
                var end = Math.Min(bytez.Length - 1, bytez2.Length - 1);

                for(var i = 0; i <= end; i++)
                {
                    bytez2[i] = bytez[i + +offset];
                }

                long l = BitConverter.ToInt64(bytez2, 0);

                return new StringFromDouble(l);
            }

            // Shouldn't ever happen?
            throw new MathParserException("Invalid type for StringMathNode Value");

        }

        public override string ToString()
        {
            if (this.value is IMathNode)
            {
                return ((IMathNode)this.value).ToString();
            }
            else if (this.value is string)
            {
                return "\"" + this.value + "\"";
            }


            // Shouldn't ever happen?
            throw new MathParserException("Invalid type for StringMathNode Value");
        }
    }
}
