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

        public abstract bool GetUnitFromString(string unit, out Enum result);

        public abstract UnitDouble GetReducedUnit(UnitDouble value);

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

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
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
