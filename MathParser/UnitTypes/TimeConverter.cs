using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dab.Library.MathParser
{
    public enum TimeUnits
    {
        [UnitAbbreviation("ns")]
        [UnitPlural("Nanoseconds")]
        [UnitType(UnitTypes.Time)]
        Nanosecond,
        [UnitAbbreviation("μs")]
        [UnitPlural("Microseconds")]
        [UnitType(UnitTypes.Time)]
        Microsecond,
        [UnitAbbreviation("ms")]
        [UnitPlural("Milliseconds")]
        [UnitType(UnitTypes.Time)]
        Millisecond,
        [UnitAbbreviation("sec")]
        [UnitAbbreviation("s")]
        [UnitPlural("Seconds")]
        [UnitType(UnitTypes.Time)]
        Second,
        [UnitAbbreviation("ks")]
        [UnitPlural("Seconds")]
        [UnitType(UnitTypes.Time)]
        Kilosecond,
        [UnitAbbreviation("min")]
        [UnitAbbreviation("m")]
        [UnitPlural("Minutes")]
        [UnitType(UnitTypes.Time)]
        Minute,
        [UnitAbbreviation("hr")]
        [UnitPlural("Hours")]
        [UnitType(UnitTypes.Time)]
        Hour,
        [UnitPlural("Days")]
        [UnitType(UnitTypes.Time)]
        Day,
        [UnitAbbreviation("mo")]
        [UnitPlural("Months")]
        [UnitType(UnitTypes.Time)]
        Month,
        [UnitAbbreviation("yr")]
        [UnitPlural("Years")]
        [UnitType(UnitTypes.Time)]
        Year,

        Unknown
    }

    public class TimeConverter : UnitConverter
    {

        public override Enum BaseUnit { get { return TimeUnits.Second; } }

        /// <summary>
        /// Maps Meter to other distances. Meter can be considered the base unit
        /// </summary>
        private Dictionary<Enum, decimal> conversionMap = new Dictionary<Enum, decimal>();

        public TimeConverter()
        {
            conversionMap.Add(TimeUnits.Nanosecond, decimal.Parse("1E+09", System.Globalization.NumberStyles.Float));
            conversionMap.Add(TimeUnits.Microsecond, 1000000);
            conversionMap.Add(TimeUnits.Millisecond, 1000);
            conversionMap.Add(TimeUnits.Second, 1);
            conversionMap.Add(TimeUnits.Kilosecond, (decimal)1/1000);
            conversionMap.Add(TimeUnits.Minute, (decimal)1 / 60);
            conversionMap.Add(TimeUnits.Hour, (decimal)1 / 60 / 60);
            conversionMap.Add(TimeUnits.Day, (decimal)1 / 60 / 60 / 24);
            conversionMap.Add(TimeUnits.Month, decimal.Parse("3.8026E-07", System.Globalization.NumberStyles.Float));
            conversionMap.Add(TimeUnits.Year, decimal.Parse("3.1688E-08", System.Globalization.NumberStyles.Float));
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
            TimeUnits fromDu = (TimeUnits)from;
            TimeUnits toDu = (TimeUnits)to;

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
