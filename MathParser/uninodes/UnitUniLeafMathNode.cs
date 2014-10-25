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

            val.Converter = converter;
            val.Unit = converter.BaseUnit;

            // Check that the child doesn't have a type associated with it yet.
            // UnitTypes.None should be the default value for unittypes.
            // This is of course unless we are converting from 1 to another
            if (val.UnitType != UnitTypes.None)
            {
                // Check to see if we're at least converting
                if (val.Converter != this.converter)
                {
                    throw new UnexpectedUnitException(val.UnitType.ToString(), val.Unit.ToString());
                }

                val.Reduce = false;
                val.Value = converter.Convert(val.Value, converter.BaseUnit, this.labelledUnit);
            }
            val.Unit = this.labelledUnit;

            val.UnitType = this.unitType;

            // We assign our UnitDouble our unit values. 

            // Now we need to convert our unit to the base unit of this unit type.
            // All UnitDouble s need to be represented in the base unit.

            // Then we convert.
            val.Value = converter.Convert(val.Value, this.labelledUnit, converter.BaseUnit);
            
            return val;
        }

        private UnitTypes unitType;
        private UnitConverter converter;
        private Enum labelledUnit;
    }
}
