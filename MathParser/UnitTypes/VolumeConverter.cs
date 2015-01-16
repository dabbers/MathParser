using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dab.Library.MathParser
{
    public enum VolumeUnits
    {
        [UnitAbbreviation("floz")]
        [UnitPlural("Ounces")]
        [UnitType(UnitTypes.VolumeImperical)]
        Ounce,
        [UnitPlural("Cups")]
        [UnitType(UnitTypes.VolumeImperical)]
        Cup,
        [UnitAbbreviation("p")]
        [UnitPlural("Pints")]
        [UnitType(UnitTypes.VolumeImperical)]
        Pint,
        [UnitAbbreviation("qt")]
        [UnitPlural("Quarts")]
        [UnitType(UnitTypes.VolumeImperical)]
        Quart,
        [UnitAbbreviation("gal")]
        [UnitAbbreviation("gals")]
        [UnitPlural("Gallons")]
        [UnitType(UnitTypes.VolumeImperical)]
        Gallon,
        [UnitAbbreviation("l")]
        [UnitPlural("Liters")]
        [UnitType(UnitTypes.VolumeMetric)]
        Liter,
        [UnitAbbreviation("ml")]
        [UnitPlural("Millilitera")]
        [UnitType(UnitTypes.VolumeMetric)]
        Milliliter,
        [UnitAbbreviation("mc")]
        [UnitPlural("Cubic Meters")]
        [UnitType(UnitTypes.VolumeMetric)]
        CubicMeter,
        [UnitAbbreviation("mmc")]
        [UnitPlural("Cubic MilliMeters")]
        [UnitType(UnitTypes.VolumeMetric)]
        CubicMilliMeter,
        Unknown
    }

    public class VolumeConverter : UnitConverter
    {

        public override Enum BaseUnit { get { return VolumeUnits.Milliliter; } }

        /// <summary>
        /// Maps Meter to other distances. Meter can be considered the base unit
        /// </summary>
        private Dictionary<VolumeUnits, decimal> conversionMap = new Dictionary<VolumeUnits, decimal>();

        public VolumeConverter()
        {
            conversionMap.Add(VolumeUnits.Ounce, 0.03381402M);
            conversionMap.Add(VolumeUnits.Milliliter, 1);
            conversionMap.Add(VolumeUnits.Liter, .001M);
            conversionMap.Add(VolumeUnits.Gallon, 0.00026417M);
            conversionMap.Add(VolumeUnits.Quart, 0.00105669M);
            conversionMap.Add(VolumeUnits.Pint, 0.00211338M);
            conversionMap.Add(VolumeUnits.Cup, 0.00422675M);
            conversionMap.Add(VolumeUnits.CubicMeter, 1M / 1000000000M);
            conversionMap.Add(VolumeUnits.CubicMilliMeter, 1M / 1000M);
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
            VolumeUnits fromDu = (VolumeUnits)from;
            VolumeUnits toDu = (VolumeUnits)to;

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
