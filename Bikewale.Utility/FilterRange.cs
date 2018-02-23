using Bikewale.Entities.Filters;
using System.Collections.Generic;


namespace Bikewale.Utility
{
    public class RangeBase
    {
        public string Unit { get; set; }
        public uint[] Range { get; set; }
        public string Type { get; set; }

    }

    public class RangeFactory
    {
        private static readonly uint[] PriceRange = new uint[] { 50000, 100000, 200000, 400000, 600000, 1000000, 1400000, 1800000 };
        private static readonly uint[] Mileage = new uint[] { 30, 40, 50, 60, 70 };
        private static readonly uint[] Displacement = new uint[] { 110, 125, 150, 200, 250, 350, 450, 600, 750 };

        /// <summary>
        /// Created by : Snehal Dange on 20th Feb 2018
        /// Description: GetDefinedRange() method created to get the range scale for a particular filter type
        /// </summary>
        /// <param name="rangeType"></param>
        /// <returns></returns>
        public static RangeBase GetDefinedRange(InPageFilterEnum rangeType)
        {
            RangeBase rangeObj = null;
            if (rangeType != null)
            {
                rangeObj = new RangeBase();
                string rangeUnit = null;
                uint[] rangeScale = null;
                string type = null;
                switch (rangeType)
                {
                    case InPageFilterEnum.Budget:
                        rangeScale = PriceRange;
                        rangeUnit = "lakhs";
                        type = "price";
                        break;
                    case InPageFilterEnum.Displacement:
                        rangeScale = Displacement;
                        rangeUnit = "cc";
                        type = "displacement";
                        break;
                    case InPageFilterEnum.Mileage:
                        rangeScale = Mileage;
                        rangeUnit = "kmpl";
                        type = "mileage";
                        break;
                    default:
                        break;
                }
                rangeObj.Range = rangeScale;
                rangeObj.Unit = rangeUnit;
                rangeObj.Type = type;
            }
            return rangeObj;

        }

        /// <summary>
        /// Created by : Snehal Dange on 21st Feb 2018
        /// Description: Method to get the relevant 4 filters for a particular make . Logic works on minValue for displacement,mileage and budget.
        /// </summary>
        /// <param name="rangeType"></param>
        /// <param name="minValue"></param>
        /// <returns></returns>
        public static FilterBase GetContextualFilters(InPageFilterEnum rangeType, double minValue)
        {
            FilterBase filterList = null;
            if (minValue > 0)
            {
                RangeBase rangeObj = GetDefinedRange(rangeType);
                if (rangeObj != null && rangeObj.Range != null)
                {
                    int key = 0;
                    byte endRangeIndex = 4;  //last 3 numbers on rangescale needed to get the filters
                    byte rangeListLength = 3; // need 3 numbers from rangescale to define the filters
                    filterList = new FilterBase();
                    IList<uint> filterRangeList = new List<uint>();
                    var rangeList = rangeObj.Range;
                    int len = rangeList.Length;


                    /* Case to handle when min price is greater than last 4 filters*/
                    if ((minValue > rangeList[len - endRangeIndex]))
                    {
                        key = len - (endRangeIndex - 1);
                        while (key < len)
                        {
                            filterRangeList.Add(rangeList[key++]);
                        }
                    }
                    else //when minprice is in the give scale
                    {
                        while ((key < len) && (minValue > rangeList[key]))
                        {
                            key++;
                        }

                        for (int i = 0; (i < rangeListLength && (key < len)); i++)
                        {
                            filterRangeList.Add(rangeList[key++]);
                        }
                    }

                    filterList.RangeList = filterRangeList;
                    filterList.Unit = rangeObj.Unit;
                    filterList.FilterType = rangeObj.Type;
                }
            }
            return filterList;
        }
    }

}
