
namespace Bikewale.Utility
{
    public class RangeBase
    {
        public string Unit { get; set; }
        public uint[] Range { get; set; }

    }

    public enum Ranges
    {
        Budget,
        Displacement,
        Mileage
    }

    public class RangeFactory
    {
        public static readonly uint[] PriceRange = new uint[] { 50000, 1, 2, 4, 6, 10, 14, 18 };
        public static readonly uint[] Mileage = new uint[] { 30, 40, 50, 60, 70 };
        public static readonly uint[] Displacement = new uint[] { 110, 125, 150, 200, 250, 350, 450, 600, 750 };

        /// <summary>
        /// Created by : Snehal Dange on 20th Feb 2018
        /// Description: GetDefinedRange() method created to get the range scale for a particular filter type
        /// </summary>
        /// <param name="rangeType"></param>
        /// <returns></returns>
        public static RangeBase GetDefinedRange(Ranges rangeType)
        {
            RangeBase rangeObj = new RangeBase();
            switch (rangeType)
            {
                case Ranges.Budget:
                    rangeObj.Range = PriceRange;
                    rangeObj.Unit = "lakhs";
                    break;
                case Ranges.Displacement:
                    rangeObj.Range = Displacement;
                    rangeObj.Unit = "cc";
                    break;
                case Ranges.Mileage:
                    rangeObj.Range = Mileage;
                    rangeObj.Unit = "kmpl";
                    break;
                default:
                    break;
            }
            return rangeObj;

        }
    }

}
