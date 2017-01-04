
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
                return "0x"+((long)((UnitDouble)this.Value).Value).ToString("X");
            }
            else if (((UnitDouble)this.Value).UnitType == UnitTypes.Octal)
            {
                return "0" + Convert.ToString(((long)((UnitDouble)this.Value).Value), 8);
            }
            else if (((UnitDouble)this.Value).UnitType == UnitTypes.Binary)
            {
                return "0b" + Convert.ToString(((long)((UnitDouble)this.Value).Value), 2);
            }

            string number = ((UnitDouble)this.Value).Value.ToString(UnitDouble.FORMATTING_STRING_DEFAULT);
            // In case I need to manipulate it somehow in the future
            return number;
        }
    }
}
