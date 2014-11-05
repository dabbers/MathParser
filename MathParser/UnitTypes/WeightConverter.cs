using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dab.Library.MathParser
{
    public enum WeightUnits
    {
        [UnitAbbreviation("oz")]
        [UnitPlural("Ounces")]
        [UnitType(UnitTypes.WeightImperical)]
        Ounce,
        [UnitAbbreviation("g")]
        [UnitAbbreviation("gr")]
        [UnitPlural("Grams")]
        [UnitType(UnitTypes.WeightMetric)]
        Gram,
        [UnitAbbreviation("mg")]
        [UnitPlural("Milligrams")]
        [UnitType(UnitTypes.WeightMetric)]
        Milligram,
        [UnitAbbreviation("µg")]
        [UnitPlural("Micrograms")]
        [UnitType(UnitTypes.WeightMetric)]
        Microgram,
        [UnitAbbreviation("lbs")]
        [UnitPlural("Pounds")]
        [UnitType(UnitTypes.WeightImperical)]
        Pound,
        [UnitAbbreviation("kg")]
        [UnitPlural("Kilograms")]
        [UnitType(UnitTypes.WeightMetric)]
        Kilogram,
        [UnitAbbreviation("st")]
        [UnitPlural("Stones")]
        [UnitType(UnitTypes.WeightImperical)]
        [UnitType(UnitTypes.WeightMetric)]
        Stone,
        Unknown
    }

    public class WeightConverter : UnitConverter
    {

        public override Enum BaseUnit { get { return WeightUnits.Gram; } }

        /// <summary>
        /// Maps Meter to other distances. Meter can be considered the base unit
        /// </summary>
        private Dictionary<WeightUnits, decimal> conversionMap = new Dictionary<WeightUnits, decimal>();

        public WeightConverter()
        {
            conversionMap.Add(WeightUnits.Ounce, 0.03527396M);
            conversionMap.Add(WeightUnits.Gram, 1);
            conversionMap.Add(WeightUnits.Milligram, 1000);
            conversionMap.Add(WeightUnits.Microgram, 1000000);
            conversionMap.Add(WeightUnits.Pound, 0.00220462M);
            conversionMap.Add(WeightUnits.Kilogram, 0.001M);
            conversionMap.Add(WeightUnits.Stone, 0.00015747M);
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
            WeightUnits fromDu = (WeightUnits)from;
            WeightUnits toDu = (WeightUnits)to;

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
