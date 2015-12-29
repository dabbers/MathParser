using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dab.Library.MathParser
{
    public abstract class UnitConverter
    {
        public abstract decimal Convert(decimal value, Enum from, Enum to);

        //public abstract bool GetUnitFromString(string unit, out Enum result);
        
        public virtual UnitDouble GetReducedUnit(UnitDouble value)
        {
            var types = Enum.GetValues(BaseUnit.GetType());
            decimal smallestOver1 = value.Value;
            Enum smallestOver1Type;

            smallestOver1Type = value.Unit;

            foreach (Enum type in types)
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


        public virtual bool GetUnitFromString(string unit, out Enum result)
        {
            var values = Enum.GetValues(BaseUnit.GetType());
            unit = unit.ToLower();

            foreach (Enum value in values)
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

            result = (Enum)Enum.Parse(BaseUnit.GetType(), "Unknown");

            return false;
        }

        public abstract Enum BaseUnit { get; }

        public static bool operator ==(UnitConverter left, UnitConverter right)
        {
            if ((object)left == null)
            {
                if ((object)right == null)
                    return true;
                else
                    return false;
            }
            else if ((object)right == null)
            {
                return false;
            }

            var tmpl = left.GetType();
            var tmpr = right.GetType();
            return tmpl.Name == tmpr.Name;
        }
        public static bool operator !=(UnitConverter left, UnitConverter right)
        {
            return !(left == right);
        }
    }

    public interface IBaseUnit
    {

    }

    public interface IDistanceUnit : IBaseUnit
    {

    }

    [AttributeUsage(AttributeTargets.Field, AllowMultiple=true)]
    public sealed class UnitAbbreviationAttribute : Attribute
    {
        public string Abbreviation { get; private set; }

        public UnitAbbreviationAttribute(string abbreviation)
        {
            this.Abbreviation = abbreviation;
        }
    }


    public sealed class DisplayAttribute : Attribute
    {
        public string Display { get; private set; }

        public DisplayAttribute(string display)
        {
            this.Display = display;
        }
    }

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public sealed class UnitPluralAttribute : Attribute
    {
        public string Plural { get; private set; }

        public UnitPluralAttribute(string plural)
        {
            this.Plural = plural;
        }
    }

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public sealed class UnitTypeAttribute : Attribute
    {
        public UnitTypes UnitType { get; private set; }

        public UnitTypeAttribute(UnitTypes unittype)
        {
            this.UnitType = unittype;
        }
    }


    public static class EnumHelper
    {
        /// <summary>
        /// Gets an attribute on an enum field value
        /// </summary>
        /// <typeparam name="T">The type of the attribute you want to retrieve</typeparam>
        /// <param name="enumVal">The enum value</param>
        /// <returns>The attribute of type T that exists on the enum value</returns>
        public static T[] GetAttributeOfType<T>(this Enum enumVal) where T : System.Attribute
        {
            var type = enumVal.GetType();
            var memInfo = type.GetMember(enumVal.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(T), false);
            return (attributes.Length > 0) ? (T[])attributes : null;
        }
    }
}
