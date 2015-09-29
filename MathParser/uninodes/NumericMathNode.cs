
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dab.Library.MathParser
{
    public class NumericMathNode : UniLeafMathNode
    {

        public NumericMathNode(decimal value)
        {
            this.value = new UnitDouble(value);
        }
        public NumericMathNode(UnitDouble value)
        {
            this.value = value;
        }

        public override UnitDouble Evaluate()
        {
            var undub = this.value as UnitDouble;
            //undub.UnitType = this.UnitType;
            return undub;
        }

        public override string ToString()
        {
            if (((UnitDouble)this.Value).UnitType == UnitTypes.Hexadecimal)
            {
                return "0x"+((int)((UnitDouble)this.Value).Value).ToString("X");
            }
            else if (((UnitDouble)this.Value).UnitType == UnitTypes.Octal)
            {
                return "0" + Convert.ToString(((int)((UnitDouble)this.Value).Value), 8);
            }

            string number = ((UnitDouble)this.Value).Value.ToString();
            // In case I need to manipulate it somehow in the future
            return number;
        }
    }
}
