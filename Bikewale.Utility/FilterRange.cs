using Bikewale.Entities.Filters;
using System;
using System.Collections.Generic;


namespace Bikewale.Utility
{
    public class RangeBase
    {
        public string Unit { get; set; }
        public uint[] Range { get; set; }

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
            try
            {
                rangeObj = new RangeBase();
                switch (rangeType)
                {
                    case InPageFilterEnum.Budget:
                        rangeObj.Range = PriceRange;
                        rangeObj.Unit = "lakhs";
                        break;
                    case InPageFilterEnum.Displacement:
                        rangeObj.Range = Displacement;
                        rangeObj.Unit = "cc";
                        break;
                    case InPageFilterEnum.Mileage:
                        rangeObj.Range = Mileage;
                        rangeObj.Unit = "kmpl";
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
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
        public static FilterBase GetContextualFilters(InPageFilterEnum rangeType, int minValue)
        {
            FilterBase filterList = null;
            try
            {
                if (minValue > 0)
                {

                    RangeBase rangeObj = GetDefinedRange(rangeType);
                    filterList = new FilterBase();
                    filterList.RangeList = new List<uint>();
                    int key = 0;
                    if (rangeObj != null && rangeObj.Range != null && filterList != null && filterList.RangeList != null)
                    {
                        int len = rangeObj.Range.Length;

                        /* Case to handle when min price is greater than last 4 filters*/
                        if ((minValue > rangeObj.Range[len - 4]))
                        {
                            key = len - 3;
                            while (key < len)
                            {
                                filterList.RangeList.Add(rangeObj.Range[key++]);
                            }
                        }
                        else //when minprice is in the give scale
                        {
                            while (key < len && (minValue > rangeObj.Range[key]))
                            {
                                key++;
                            }

                            for (int i = 0; (i < 3 && key < len); i++)
                            {
                                filterList.RangeList.Add(rangeObj.Range[key++]);
                            }
                        }
                        filterList.Unit = rangeObj.Unit;
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return filterList;
        }
    }

}
