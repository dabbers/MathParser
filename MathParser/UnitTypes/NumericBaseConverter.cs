using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dab.Library.MathParser
{
    public enum NumericBaseUnits
    {
        [UnitAbbreviation("Oct")]
        [UnitPlural("Oct")]
        [UnitType(UnitTypes.Octal)]
        Octal,

        [UnitAbbreviation("Hex")]
        [UnitPlural("Hex")]
        [UnitType(UnitTypes.Hexadecimal)]
        Hexadecimal,

        [UnitAbbreviation("")]
        [UnitPlural("")]
        [UnitType(UnitTypes.Decimal)]
        [Display("")]
        Decimal,
    }
    public class NumericBaseConverter : UnitConverter
    {
        public override Enum BaseUnit { get { return NumericBaseUnits.Decimal; } }

        public override decimal Convert(decimal value, Enum from, Enum to)
        {
            return value;
        }

        public override UnitDouble GetReducedUnit(UnitDouble value)
        {
            return value;
        }
    }


}
