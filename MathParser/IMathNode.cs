using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dab.Library.MathParser
{
    public interface IMathNode
    {
        UnitTypes UnitType { get; set; }
        UnitDouble Evaluate();
    }

    /// <summary>
    /// Specifies a double with an optional unit type.
    /// </summary>
    public class UnitDouble
    {
        public UnitTypes UnitType { get; set; }

        public decimal Value { get; set; }

        public Enum Unit { get; set; }

        public UnitConverter Converter { get; set; }

        public bool Reduce { get; set; }

        public UnitDouble(decimal value)
        {
            this.Value = value;
            this.Reduce = true;
        }

        public UnitDouble(decimal value, UnitTypes unitType, Enum unit, UnitConverter convert)
            :this(value)
        {
            this.UnitType = unitType;
            this.Converter = convert;
            this.Unit = unit;
            
        }
        

        public override string ToString()
        {
            var value = this;

            if (this.Converter != null && Reduce)
            {
                value = this.Converter.GetReducedUnit(this);
            }
            else if (this.Converter != null && !Reduce)
            {
                value.Value = this.Converter.Convert(value.Value, Converter.BaseUnit, value.Unit);
            }

            string formatting = "###,###,###,###,###.############";

            if (Reduce && value.Value > 10000000000000000)
            {
                formatting = "E";
            }
            
            if (value.UnitType == UnitTypes.Currency)
            {
                formatting = "###,###,###,###,##0.############;-###,###,###,###,##0.############";
                value.Value = Math.Round(value.Value, 2);
            }


            value.Value = Math.Round(value.Value, 6);
            string unitlabel = String.Empty;

            if (this.Converter != null)
            {
                unitlabel = value.Unit.ToString();

                if (value.Value != 1)
                {
                    var temp = value.Unit.GetAttributeOfType<UnitPluralAttribute>().FirstOrDefault();
                    if (temp != null)
                    {
                        unitlabel = temp.Plural;
                    }
                }
            }

            return value.Value.ToString(formatting) + (this.Converter != null ? " " + unitlabel : "");
        }

        public static UnitDouble operator +(UnitDouble left, UnitDouble right)
        {
            if (left.Converter != right.Converter && right.UnitType != UnitTypes.None && left.UnitType != UnitTypes.None)
            {
                throw new UnitMismatchException(left.UnitType.ToString(), right.UnitType.ToString());
            }

            var convert = (left.UnitType == UnitTypes.None ? right.Converter : left.Converter);
            var unittype = (left.UnitType == UnitTypes.None ? right.UnitType : left.UnitType);
            var unit = (left.UnitType == UnitTypes.None ? right.Unit : left.Unit);

            return new UnitDouble(left.Value + right.Value, unittype, unit, convert);
        }
        public static UnitDouble operator -(UnitDouble left, UnitDouble right)
        {
            if (left.Converter != right.Converter && right.UnitType != UnitTypes.None && left.UnitType != UnitTypes.None)
            {
                throw new UnitMismatchException(left.UnitType.ToString(), right.UnitType.ToString());
            }

            var convert = (left.UnitType == UnitTypes.None ? right.Converter : left.Converter);
            var unittype = (left.UnitType == UnitTypes.None ? right.UnitType : left.UnitType);
            var unit = (left.UnitType == UnitTypes.None ? right.Unit : left.Unit);

            return new UnitDouble(left.Value - right.Value, unittype, unit, convert);
        }
        public static UnitDouble operator /(UnitDouble left, UnitDouble right)
        {
            if (left.Converter != right.Converter && right.UnitType != UnitTypes.None && left.UnitType != UnitTypes.None)
            {
                throw new UnitMismatchException(left.UnitType.ToString(), right.UnitType.ToString());
            }

            var convert = (left.UnitType == UnitTypes.None ? right.Converter : left.Converter);
            var unittype = (left.UnitType == UnitTypes.None ? right.UnitType : left.UnitType);
            var unit = (left.UnitType == UnitTypes.None ? right.Unit : left.Unit);

            return new UnitDouble(left.Value / right.Value, unittype, unit, convert);
        }
        public static UnitDouble operator *(UnitDouble left, UnitDouble right)
        {
            if (left.Converter != right.Converter && right.UnitType != UnitTypes.None && left.UnitType != UnitTypes.None)
            {
                throw new UnitMismatchException(left.UnitType.ToString(), right.UnitType.ToString());
            }

            var convert = (left.UnitType == UnitTypes.None ? right.Converter : left.Converter);
            var unittype = (left.UnitType == UnitTypes.None ? right.UnitType : left.UnitType);
            var unit = (left.UnitType == UnitTypes.None ? right.Unit : left.Unit);

            return new UnitDouble(left.Value * right.Value, unittype, unit, convert);
        }

    }

    public enum UnitTypes
    {
        
        None = 0, // Default normal value
        DistanceImperial,
        DistanceMetric,

        WeightImperical,
        WeightMetric,
        
        CapacityDigital,

        SpeedDigital,
        
        SpeedMetric,
        SpeedImperical,
        
        Degrees,
        Radians,
        
        Currency,

        RBOMB

    }
}
