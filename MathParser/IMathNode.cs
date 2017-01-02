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
    
    public enum UnitTypes
    {
        
        None = 0, // Default normal value
        DistanceImperial,
        DistanceMetric,

        WeightImperical,
        WeightMetric,
        
        CapacityDigital,

        SpeedDigital,

        Currency,

        Hexadecimal,
        Decimal,
        Octal,
        Binary,

        Time,


        SpeedMetric,
        SpeedImperical,

        VolumeMetric,
        VolumeImperical,
        
        Degrees,
        Radians,
        

        RBOMB

    }
}
