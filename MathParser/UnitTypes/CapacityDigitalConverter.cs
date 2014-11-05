using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dab.Library.MathParser
{
    public enum CapacityDigitalUnits
    {
        [UnitAbbreviation("b")]
        [UnitPlural("Bits")]
        [UnitType(UnitTypes.CapacityDigital)]
        Bit,
        [UnitAbbreviation("B")]
        [UnitPlural("Bytes")]
        [UnitType(UnitTypes.CapacityDigital)]
        Byte,
        [UnitAbbreviation("kb")]
        [UnitPlural("Kilobytes")]
        [UnitType(UnitTypes.CapacityDigital)]
        Kilobyte,
        [UnitAbbreviation("mb")]
        [UnitPlural("Megabytes")]
        [UnitType(UnitTypes.CapacityDigital)]
        Megabyte,
        [UnitAbbreviation("gb")]
        [UnitPlural("Gigabytes")]
        [UnitType(UnitTypes.CapacityDigital)]
        Gigabyte,
        [UnitAbbreviation("tb")]
        [UnitPlural("Terabytes")]
        [UnitType(UnitTypes.CapacityDigital)]
        Terabyte,
        [UnitAbbreviation("pb")]
        [UnitPlural("Petabytes")]
        [UnitType(UnitTypes.CapacityDigital)]
        Petabyte,
        [UnitAbbreviation("eb")]
        [UnitPlural("Exabytes")]
        [UnitType(UnitTypes.CapacityDigital)]
        Exabyte,
        [UnitAbbreviation("zb")]
        [UnitPlural("Zettabytes")]
        [UnitType(UnitTypes.CapacityDigital)]
        Zettabyte,
        [UnitAbbreviation("yb")]
        [UnitPlural("Yottabytes")]
        [UnitType(UnitTypes.CapacityDigital)]
        Yottabyte,
        Unknown
    }

    public class CapacityDigitalConverter : UnitConverter
    {

        public override Enum BaseUnit { get { return CapacityDigitalUnits.Bit; } }

        /// <summary>
        /// Maps Meter to other distances. Meter can be considered the base unit
        /// </summary>
        private Dictionary<CapacityDigitalUnits, decimal> conversionMap = new Dictionary<CapacityDigitalUnits, decimal>();

        public CapacityDigitalConverter()
        {
            conversionMap.Add(CapacityDigitalUnits.Bit, 1);
            conversionMap.Add(CapacityDigitalUnits.Byte, 8);
            conversionMap.Add(CapacityDigitalUnits.Kilobyte, 8 * 1024);
            conversionMap.Add(CapacityDigitalUnits.Megabyte, 8 * 1024 * 1024);
            conversionMap.Add(CapacityDigitalUnits.Gigabyte, 8 * (long)1024 * 1024 * 1024);
            conversionMap.Add(CapacityDigitalUnits.Terabyte, 8 * (long)1024 * 1024 * 1024 * 1024);
            conversionMap.Add(CapacityDigitalUnits.Petabyte, 8 * (long)1024 * 1024 * 1024 * 1024 * 1024);
            conversionMap.Add(CapacityDigitalUnits.Exabyte, 8 * (decimal)1024 * 1024 * 1024 * 1024 * 1024 * 1024);
            conversionMap.Add(CapacityDigitalUnits.Zettabyte, 8 * (decimal)1024 * 1024 * 1024 * 1024 * 1024 * 1024 * 1024);
            conversionMap.Add(CapacityDigitalUnits.Yottabyte, 8 * (decimal)1024 * 1024 * 1024 * 1024 * 1024 * 1024 * 1024 * 1024);
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
            CapacityDigitalUnits fromDu = (CapacityDigitalUnits)from;
            CapacityDigitalUnits toDu = (CapacityDigitalUnits)to;

            decimal conversion;

            if (!conversionMap.TryGetValue(fromDu, out conversion))
            {
                throw new InvalidUnitTypeException(fromDu.ToString());
            }
            // Convert from to Meter
            value *= conversion;

            // Convert meter to to.
            if (!conversionMap.TryGetValue(toDu, out conversion))
            {
                throw new InvalidUnitTypeException(toDu.ToString());
            }

            return value / conversion;
        }

    }
}
