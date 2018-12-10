using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Enum
{
    public enum CarBodyStyle
    {
        Sedan = 1,
        Coupe = 2,
        Hatchback = 3,
        Minivan = 4,
        Convertible = 5,
        SUV = 6,
        Truck = 7,
        StationWagon = 8,
        MUV = 9,
        CompactSedan = 10
    }

    /// Written by Meet Shah on 19 July, 2017.
    /// <summary>
    /// This extension method is used to output friendly names 
    /// for the car body styles.
    /// </summary>
    public static class CarBodyStyleExtensions
    {
        public static string ToFriendlyString(this CarBodyStyle type)
        {
            switch (type)
            {
                case CarBodyStyle.Sedan:
                    return "Sedan";
                case CarBodyStyle.Coupe:
                    return "Coupe";
                case CarBodyStyle.Hatchback:
                    return "Hatchback";
                case CarBodyStyle.Minivan:
                    return "Minivan";
                case CarBodyStyle.Convertible:
                    return "Convertible";
                case CarBodyStyle.SUV:
                    return "SUV";
                case CarBodyStyle.Truck:
                    return "Truck";
                case CarBodyStyle.StationWagon:
                    return "Station Wagon";
                case CarBodyStyle.MUV:
                    return "MUV";
                case CarBodyStyle.CompactSedan:
                    return "Compact Sedan";
                default:
                    return string.Empty;
            }
        }
        public static string ToUrlString(this CarBodyStyle type)
        {
            switch (type)
            {
                case CarBodyStyle.Sedan:
                    return "sedans";
                case CarBodyStyle.Coupe:
                    return "coupes";
                case CarBodyStyle.Hatchback:
                    return "hatchbacks";
                case CarBodyStyle.Minivan:
                    return "minivans";
                case CarBodyStyle.Convertible:
                    return "convertibles";
                case CarBodyStyle.SUV:
                    return "suvs";
                case CarBodyStyle.Truck:
                    return "trucks";
                case CarBodyStyle.StationWagon:
                    return "stationwagons";
                case CarBodyStyle.MUV:
                    return "muvs";
                case CarBodyStyle.CompactSedan:
                    return "compactsedans";
                default:
                    return string.Empty;
            }
        }
    }
}
