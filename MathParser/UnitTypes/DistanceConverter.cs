using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dab.Library.MathParser
{
    public enum DistanceUnits
    {
        [UnitAbbreviation("cm")]
        [UnitPlural("Centimeters")]
        [UnitType(UnitTypes.DistanceMetric)]
        [UnitType(UnitTypes.DistanceImperial)]
        Centimeter,
        [UnitAbbreviation("ft")]
        [UnitPlural("Feet")]
        [UnitType(UnitTypes.DistanceImperial)]
        Foot,
        [UnitAbbreviation("yd")]
        [UnitPlural("Yards")]
        [UnitType(UnitTypes.DistanceImperial)]
        Yard,
        [UnitAbbreviation("mi")]
        [UnitPlural("Miles")]
        [UnitType(UnitTypes.DistanceImperial)]
        Mile,
        [UnitAbbreviation("in")]
        [UnitPlural("Inches")]
        [UnitType(UnitTypes.DistanceImperial)]
        Inch,
        [UnitAbbreviation("nm")]
        [UnitPlural("Nanometers")]
        [UnitType(UnitTypes.DistanceMetric)]
        Nanometer,
        [UnitAbbreviation("μm")]
        [UnitPlural("Micrometers")]
        [UnitType(UnitTypes.DistanceMetric)]
        Micrometer,
        [UnitAbbreviation("mm")]
        [UnitPlural("Millimeters")]
        [UnitType(UnitTypes.DistanceMetric)]
        Millimeter,
        [UnitAbbreviation("m")]
        [UnitPlural("Meters")]
        [UnitType(UnitTypes.DistanceMetric)]
        Meter,
        [UnitAbbreviation("dm")]
        [UnitPlural("Decimeters")]
        [UnitType(UnitTypes.DistanceMetric)]
        Decimeter,
        [UnitAbbreviation("km")]
        [UnitPlural("Kilometers")]
        [UnitType(UnitTypes.DistanceMetric)]
        Kilometer,
        Unknown
    }

    public class DistanceConverter : UnitConverter
    {

        public override Enum BaseUnit { get { return DistanceUnits.Meter; } }

        /// <summary>
        /// Maps Meter to other distances. Meter can be considered the base unit
        /// </summary>
        private Dictionary<DistanceUnits, decimal> conversionMap = new Dictionary<DistanceUnits, decimal>();

        public  DistanceConverter()
        {
            // Metric
            conversionMap.Add(DistanceUnits.Nanometer, 1000000000);
            conversionMap.Add(DistanceUnits.Micrometer,   1000000);
            conversionMap.Add(DistanceUnits.Millimeter,      1000);
            conversionMap.Add(DistanceUnits.Centimeter,       100);
            conversionMap.Add(DistanceUnits.Decimeter,         10);
            conversionMap.Add(DistanceUnits.Meter,              1);
            conversionMap.Add(DistanceUnits.Kilometer,          0.001M);

            // Imperical
            conversionMap.Add(DistanceUnits.Mile, 0.00062137M);
            conversionMap.Add(DistanceUnits.Yard, 1.0936M);
            conversionMap.Add(DistanceUnits.Foot, 3.2808399M);
            conversionMap.Add(DistanceUnits.Inch, 39.3700787M);
            
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
            DistanceUnits fromDu = (DistanceUnits)from;
            DistanceUnits toDu = (DistanceUnits)to;

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

        public override UnitDouble GetReducedUnit(UnitDouble value)
        {
            var types = Enum.GetValues(typeof(DistanceUnits));
            decimal smallestOver1 = value.Value;
            DistanceUnits smallestOver1Type;

            smallestOver1Type = (DistanceUnits)value.Unit;
            //(DistanceUnits)Enum.Parse(typeof(DistanceUnits), value.Unit);

            // Convert to Imperial if we are requesting imperial
            if (value.UnitType == UnitTypes.DistanceImperial)
            {
                smallestOver1 = this.Convert(smallestOver1, DistanceUnits.Meter, DistanceUnits.Foot);
                smallestOver1Type = DistanceUnits.Foot;
            }

            foreach (DistanceUnits type in types)
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
            var values = Enum.GetValues(typeof(DistanceUnits));
            unit = unit.ToLower();

            foreach(DistanceUnits value in values )
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

            result = DistanceUnits.Unknown;
            return false;
        }
    }
}
