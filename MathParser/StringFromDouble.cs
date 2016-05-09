using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dab.Library.MathParser
{
    public class StringFromDouble : UnitDouble
    {
        public StringFromDouble(UnitDouble copy) : base(copy)
        {
        }

        public StringFromDouble(decimal value) : base(value)
        {
        }
        public StringFromDouble(long value) : base(value)
        {
        }

        public StringFromDouble(decimal value, UnitTypes unitType, Enum unit, UnitConverter convert) : base(value, unitType, unit, convert)
        {
        }


        public override string ToString()
        {
            //		Unit	Decimal	System.Enum {dab.Library.MathParser.NumericBaseUnits}
            if (this.Unit != null && (NumericBaseUnits)this.Unit == NumericBaseUnits.Decimal)
            {
                return base.ToString();
            }
            else
            {
                var bytes = BitConverter.GetBytes((long)this.Value).Reverse().ToArray();
                var val = System.Text.Encoding.UTF8.GetString(bytes).Replace("\0", "");
                return val;
            }

        }
    }
}
