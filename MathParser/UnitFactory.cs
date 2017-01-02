using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace dab.Library.MathParser
{
    public class UnitFactory
    {
        public UnitFactory(string exchangeUrl, string cachePath)
        {
            this.converters = new UnitConverter[] {
                new DistanceConverter(),
                new WeightConverter(),
                new CapacityDigitalConverter(),
                new CurrencyConverter(exchangeUrl, cachePath),
                new TimeConverter(),
                new VolumeConverter(),
                new TemperatureConverter(),
                new NumericBaseConverter()
            };
        }

        // Breaks up the expression into either a conversion of (expr) to unit or (exp) unit or unit(exp) with whitespace optional.
        private static Regex unitParser = new Regex
        (
            @"^(?:(?<value>.+)\s+(?:to|as|in)\s+(?<to>[A-z$€£฿]+)|(?<value>(?!\s(?:to|as)\s).+?)\s*(?<from>[A-z$€£฿]+)|(?<from>[$€£฿])\s*(?<value>(?!\s(?:to|as)\s).+?))$"
        );

        private UnitConverter[] converters;

        public IMathNode TryParse(string expression, MathParser parser)
        {
            var possiblyhex = this.isSpeicalFormattedNumber(expression);
            if (possiblyhex != null)
            {
                return possiblyhex;
            }

            var results = unitParser.Match(expression);

            if (false == results.Success)
            {
                return null;
            }

            var expr = results.Groups["value"];
            var to = results.Groups["to"];
            var from = results.Groups["from"];
            Group actionable;

            // Continue parsing the left side of this Unit declaration
            // If this is a conversion, this will come back here until there
            // are no more units left to parse through
            
            // Check for " " to indicate a string literal.
            //if (expr.Value.Contains('"'))
            //{
            //    // Otherwise, we are parsing a complex expression
            //    var valu = parser.Parse(expr.Value);
            //}
            //else
            //{
            //    // Otherwise, we are parsing a complex expression
            //    var valu = parser.Parse(expr.Value);
            //}

            // Otherwise, we are parsing a complex expression
            var valu = parser.Parse(expr.Value);


            // If we are finally inside the expression of expr units
            if (from.Success)
            {
                actionable = from;
            }
            else
            {
                actionable = to;
            }

            UnitConverter converter = this.GetUnitConverter(actionable.Value);

            if (converter == null)
            {
                if (actionable.Value.ToLower() != "string" && actionable.Value.ToLower() != "str")
                {
                    throw new InvalidUnitTypeException(actionable.Value);
                }

                // Convert the int/long/hex value into a string/char.
                return new StringMathNode(valu);
            }


            // Determines things like DistanceImperical, DistanceMetric, etc
            Enum tmpEnum;
            converter.GetUnitFromString(actionable.Value, out tmpEnum);
            var hold = tmpEnum.GetAttributeOfType<UnitTypeAttribute>().FirstOrDefault();

            if (hold != null)
            {
                valu.UnitType = hold.UnitType;
            }

            return new UnitUniLeafMathNode(valu, (hold == null ? UnitTypes.None : hold.UnitType), converter, tmpEnum);

        }

        private IMathNode isSpeicalFormattedNumber(string expression)
        {
            expression = expression.Trim();

            // Probably formatted via 0x hex? Check for hex input
            if (expression[0] == '0' && (expression[1] == 'x' || expression[1] == 'X'))
            {
                try
                {
                    string number = expression.Substring(2);
                    var val = (decimal)Int64.Parse(number, System.Globalization.NumberStyles.HexNumber);
                    return new NumericMathNode(new UnitDouble(val, UnitTypes.Hexadecimal, NumericBaseUnits.Hexadecimal, new NumericBaseConverter()));
                }
                catch { }

            }
            if (expression[0] == '0' && (expression[1] == 'b' || expression[1] == 'B'))
            {
                try
                {
                    string number = expression.Substring(2);
                    var val = (decimal)Convert.ToInt64(number, 2);
                    return new NumericMathNode(new UnitDouble(val, UnitTypes.Binary, NumericBaseUnits.Binary, new NumericBaseConverter()));
                }
                catch { }

            }
            if (expression[0] == '0')
            {
                try
                {
                    var val = (decimal)Convert.ToInt64(expression, 8);
                    return new NumericMathNode(new UnitDouble(val, UnitTypes.Octal, NumericBaseUnits.Octal, new NumericBaseConverter()));
                }
                catch { }
            }

            return null;

        }

        public UnitConverter GetUnitConverter(string unit)
        {
            Enum tmp;
            return converters.Where(p => p.GetUnitFromString(unit, out tmp)).FirstOrDefault();
        }
    }
}
