using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dab.Library.MathParser
{
    public enum RBOMBUnits
    {
        [UnitAbbreviation("RBOMB")]
        [UnitPlural("RBOMBS")]
        [UnitType(UnitTypes.RBOMB)]
        RBOMB,
        [UnitAbbreviation("ABOMB")]
        [UnitPlural("ABOMBS")]
        [UnitType(UnitTypes.RBOMB)]
        ABOMB,
        Unknown
    }

    public class RBOMBConverter : UnitConverter
    {

        public override Enum BaseUnit { get { return RBOMBUnits.RBOMB; } }

        /// <summary>
        /// Maps Meter to other distances. Meter can be considered the base unit
        /// </summary>
        private Dictionary<RBOMBUnits, decimal> conversionMap = new Dictionary<RBOMBUnits, decimal>();

        public RBOMBConverter()
        {
            // Metric
            conversionMap.Add(RBOMBUnits.RBOMB, 1);
            conversionMap.Add(RBOMBUnits.ABOMB, 16);
            
        }

        /// <summary>
        /// Converts from 1 distance to another. By default, converts to Meter and then back to the other measurements
        /// </summary>
        /// <param name="value">The value of the from unit</param>
        /// <param name="from">From unit can be anything listed in DistanceUnits</param>
        /// <param name="to">To unit can be anything listed in DistanceUnits</param>
        /// <returns></returns>
        public override decimal Convert(decimal value, Enum from, Enum to)
        {
            RBOMBUnits fromDu = (RBOMBUnits)from;
            RBOMBUnits toDu = (RBOMBUnits)to;

            decimal conversion;

            if (!conversionMap.TryGetValue(fromDu, out conversion))
            {
                throw new InvalidUnitTypeException(fromDu.ToString());
            }
            // Convert from to Meter
            value /= conversion;

            // Convert meter to to.
            if (!conversionMap.TryGetValue(toDu, out conversion))
            {
                throw new InvalidUnitTypeException(toDu.ToString());
            }

            return value * conversion;
        }

    }
}
