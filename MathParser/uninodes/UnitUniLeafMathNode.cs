using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dab.Library.MathParser
{
    public class UnitUniLeafMathNode : UniLeafMathNode
    {
        public IMathNode Inner { get; private set; }

        public UnitUniLeafMathNode(IMathNode inner, UnitTypes unitType, UnitConverter converter, Enum labelledUnit)
            :base(null)
        {
            this.Inner = inner;
            this.unitType = unitType;
            this.converter = converter;
            this.labelledUnit = labelledUnit;
        }

        public override UnitDouble Evaluate()
        {
            // Here we need to take a value from our children, and assign a unit to them.
            // We have the base unit. If the child already has a unit, this is an invalid expression
            // because it wouldn't make sense to say something like sqrt(14 inches) feet. 
            // Something like sqrt(14 feet) feet wouldn't make sense either.

            var val = this.Inner.Evaluate();


            // We get the Enum representation of these units.


            // Check that the child doesn't have a type associated with it yet.
            // UnitTypes.None should be the default value for unittypes.
            // This is of course unless we are converting from 1 to another
            if (val.UnitType != UnitTypes.None)
            {
                // Check to see if we're at least converting
                if (val.Converter != this.converter)
                {
                    throw new UnexpectedUnitException(val.UnitType.ToString(), this.labelledUnit.ToString());
                }

                val.Reduce = false;
                val.Value = converter.Convert(val.Value, val.Unit, this.labelledUnit);
                val.DesiredUnit = this.labelledUnit;
            }

            val.Converter = this.converter;

            val.UnitType = this.unitType;

            // We assign our UnitDouble our unit values. 

            // Now we need to convert our unit to the base unit of this unit type.
            // All UnitDouble s need to be represented in the base unit.

            // Then we convert.
            val.Value = converter.Convert(val.Value, this.labelledUnit, converter.BaseUnit);
            val.Unit = this.converter.BaseUnit;
            
            return val;
        }

        public override string ToString()
        {
            string number = this.Inner.ToString();
            string seperator = " ";

            var numeric = this.Inner as NumericMathNode;
            decimal expressedNumeric = Decimal.MaxValue;
            var unit = this.labelledUnit.ToString();

            if (null != numeric)
            {
                expressedNumeric = ((UnitDouble)numeric.Value).Value;

                if (UnitTypes.Hexadecimal == numeric.UnitType)
                {
                    number = "0x" + ((long)expressedNumeric).ToString("X");
                    unit = "";
                    seperator = "";
                }
                else if (UnitTypes.Octal == numeric.UnitType)
                {
                    number = "0" + Convert.ToString(((long)expressedNumeric), 8);
                    unit = "";
                    seperator = "";
                }
                else if (UnitTypes.Binary == numeric.UnitType)
                {
                    number = "0b" + Convert.ToString(((long)expressedNumeric), 2);
                    unit = "";
                    seperator = "";
                }
                else
                {
                    expressedNumeric = Math.Round(this.converter.Convert(expressedNumeric, this.converter.BaseUnit, this.labelledUnit), 6);
                    number = expressedNumeric.ToString(UnitDouble.FORMATTING_STRING_DEFAULT);
                }

                if (numeric.Value is UnitDouble && ((UnitDouble)numeric.Value).Converter is NumericBaseConverter)
                {
                    seperator = " as ";
                }
            }
            else
            {
                var unitleaf = this.Inner as UnitUniLeafMathNode;

                if (null != unitleaf)
                {
                    if (unitleaf.labelledUnit != this.labelledUnit)
                    {
                        seperator = " as ";
                    }
                }
                else
                {

                    var smn = this.Inner as StringMathNode;

                    if (null != smn)
                    {
                        seperator = " as ";
                    }
                }
            }

            if (unit != "")
            {
                if (expressedNumeric != 1 && expressedNumeric != Decimal.MaxValue)
                {
                    var tmp = this.labelledUnit.GetAttributeOfType<UnitPluralAttribute>();
                    if (tmp != null)
                    {
                        unit = tmp.FirstOrDefault().Plural;
                    }
                }
                else
                {
                    var templbl = this.labelledUnit.GetAttributeOfType<DisplayAttribute>();
                    if (templbl != null)
                    {
                        var singluar_label = templbl.FirstOrDefault().Display;

                        // If the label is empty, don't override the unit if we're converting
                        if (!String.IsNullOrEmpty(singluar_label) || seperator != " as ")
                        {
                            unit = singluar_label;
                        }
                    }
                }
            }

            if (unit == "")
            {
                seperator = "";
            }

            return "(" + number + seperator + unit + ")";
            
        }

        private UnitTypes unitType;
        private UnitConverter converter;
        private Enum labelledUnit;
    }
}
