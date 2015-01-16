using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dab.Library.MathParser
{
    public enum TemperatureUnits
    {
        [UnitAbbreviation("f")]
        [UnitPlural("Fahrenheit")]
        [UnitType(UnitTypes.Time)]
        Fahrenheit,
        [UnitAbbreviation("c")]
        [UnitPlural("Celsius")]
        [UnitType(UnitTypes.Time)]
        Celsius,
        [UnitAbbreviation("k")]
        [UnitPlural("Kelvins")]
        [UnitType(UnitTypes.Time)]
        Kelvin,

        Unknown
    }

    public class TemperatureConverter : UnitConverter
    {

        public override Enum BaseUnit { get { return TemperatureUnits.Celsius; } }

        /// <summary>
        /// Maps Meter to other distances. Meter can be considered the base unit
        /// </summary>
        //private Dictionary<Enum, decimal> conversionMap = new Dictionary<Enum, decimal>();

        public TemperatureConverter()
        {
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
            TemperatureUnits fromDu = (TemperatureUnits)from;
            TemperatureUnits toDu = (TemperatureUnits)to;

            decimal conversion;

            switch(fromDu)
            {
                case TemperatureUnits.Celsius:
                    switch(toDu)
                    {
                        case TemperatureUnits.Celsius:
                            return value;
                        case TemperatureUnits.Fahrenheit:
                            return ((value * 9) / 5) + 32;
                        case TemperatureUnits.Kelvin:
                            return value + 273.15M;
                    }
                    break;
                case TemperatureUnits.Fahrenheit:
                    switch (toDu)
                    {
                        case TemperatureUnits.Celsius:
                            return (value - 32) / 1.8M;
                    }
                    break;
                case TemperatureUnits.Kelvin:
                    switch (toDu)
                    {
                        case TemperatureUnits.Celsius:
                            return value - 273.15M;
                    }
                    break;
            }


            throw new dab.Library.MathParser.InvalidUnitTypeException("Unknown");
        }
    }
}
