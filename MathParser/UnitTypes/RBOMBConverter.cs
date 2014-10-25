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

        public override UnitDouble GetReducedUnit(UnitDouble value)
        {
            var types = Enum.GetValues(typeof(RBOMBUnits));
            decimal smallestOver1 = value.Value;
            RBOMBUnits smallestOver1Type;

            smallestOver1Type = (RBOMBUnits)value.Unit;
            //(DistanceUnits)Enum.Parse(typeof(DistanceUnits), value.Unit);


            foreach (RBOMBUnits type in types)
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
            var values = Enum.GetValues(typeof(RBOMBUnits));
            unit = unit.ToLower();

            foreach (RBOMBUnits value in values)
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

            result = RBOMBUnits.Unknown;
            return false;
        }
    }
}
