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

        public override UnitDouble GetReducedUnit(UnitDouble value)
        {
            var types = Enum.GetValues(typeof(CapacityDigitalUnits));
            decimal smallestOver1 = value.Value;
            CapacityDigitalUnits smallestOver1Type;

            smallestOver1Type = (CapacityDigitalUnits)value.Unit;
            //(DistanceUnits)Enum.Parse(typeof(DistanceUnits), value.Unit);


            foreach (CapacityDigitalUnits type in types)
            {
                var unitTypes = type.GetAttributeOfType<UnitTypeAttribute>();
                
                if (unitTypes == null) continue;

                if (unitTypes.Where(p => p.UnitType == value.UnitType).Count() > 0)
                {
                    var convertedValue = this.Convert(smallestOver1, smallestOver1Type, type);
                    
                    // Determine if we need to go to a smaller scale, bigger number
                    if (smallestOver1 < 1)
                    {
                        if (convertedValue > smallestOver1 && convertedValue < 10000)
                        {
                            smallestOver1 = convertedValue;
                            smallestOver1Type = type;
                        }
                    }
                    else // we need to go bigger scale, smaller number
                    {
                        if (convertedValue < smallestOver1 && convertedValue > .5M)
                        {
                            smallestOver1 = convertedValue;
                            smallestOver1Type = type;
                        }
                    }
                }
            }
            
            return new UnitDouble(smallestOver1, value.UnitType, smallestOver1Type, this);
        }

        public override bool GetUnitFromString(string unit, out Enum result)
        {
            var values = Enum.GetValues(typeof(CapacityDigitalUnits));
            unit = unit.ToLower();

            foreach (CapacityDigitalUnits value in values)
            {
                if (value.ToString().ToLower() == unit)
                {
                    result = value;
                    return true;
                }

                var abbreviations = value.GetAttributeOfType<UnitAbbreviationAttribute>();
                if (abbreviations != null)
                {
                    foreach (var abbreviation in abbreviations)
                    {
                        if (abbreviation.Abbreviation.ToLower() == unit)
                        {
                            result = value;
                            return true;
                        }
                    }
                }

                var plurals = value.GetAttributeOfType<UnitPluralAttribute>();
                if (plurals != null)
                {
                    foreach (var plural in plurals)
                    {
                        if (plural.Plural.ToLower() == unit)
                        {
                            result = value;
                            return true;
                        }
                    }
                }
            }

            result = CapacityDigitalUnits.Unknown;
            return false;
        }
    }
}
