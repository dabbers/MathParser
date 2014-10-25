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
                new RBOMBConverter()
            };
        }

        private static Regex unitParser = new Regex(@"([0-9\.]+|.+\s)\s*([A-z]+)(?:\s+to\s+([A-z]+))*");
        private UnitConverter[] converters;

        public IMathNode TryParse(string expression, MathParser parser)
        {
            var results = unitParser.Match(expression);

            if (false == results.Success)
            {
                return null;
            }
            
            // Continue parsing the left side of this Unit declaration
            var valu = parser.Parse(results.Groups[1].Value);

            var converter = this.GetUnitConverter(results.Groups[2].Value);

            if (converter == null)
            {
                throw new InvalidUnitTypeException(results.Groups[2].Value);
            }

            // Determines things like DistanceImperical, DistanceMetric, etc
            Enum tmpEnum;
            converter.GetUnitFromString(results.Groups[2].Value, out tmpEnum);
            var hold = tmpEnum.GetAttributeOfType<UnitTypeAttribute>().FirstOrDefault();

            // create a new unit node with this unit at [2] and value of [1]
            if (String.IsNullOrEmpty(results.Groups[3].Value))
            {
                return new UnitUniLeafMathNode(valu, (hold == null ? UnitTypes.None : hold.UnitType), converter, tmpEnum);
            }
            else
            {
                Enum tmpEnum3;
                converter.GetUnitFromString(results.Groups[3].Value, out tmpEnum3);
                // Create a unit node converting value of [1] of [2] to [3]
                var inside = new UnitUniLeafMathNode(valu, (hold == null ? UnitTypes.None : hold.UnitType), converter, tmpEnum);

                return new UnitUniLeafMathNode(inside, (hold == null ? UnitTypes.None : hold.UnitType), converter, tmpEnum3);
            }
        }

        public UnitConverter GetUnitConverter(string unit)
        {
            Enum tmp;
            return converters.Where(p => p.GetUnitFromString(unit, out tmp)).FirstOrDefault();
        }

        public bool UnitsAreOfSameType(string unita, string unitb)
        {
            Enum tmp;
            var a = converters.Where(p => p.GetUnitFromString(unita, out tmp)).First();
            var b = converters.Where(p => p.GetUnitFromString(unitb, out tmp)).First();

            return (a.GetType() == b.GetType());
        }

        public bool UnitsAreOfSameType(Enum unita, Enum unitb)
        {
            return UnitsAreOfSameType(unita.ToString(), unitb.ToString());
        }
    }
}
