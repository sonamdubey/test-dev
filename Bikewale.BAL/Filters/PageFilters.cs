using Bikewale.Entities.BikeData;
using Bikewale.Entities.Filters;
using Bikewale.Interfaces.Filters;
using Bikewale.Notifications;
using Bikewale.Utility;
using System;
using System.Collections.Generic;
namespace Bikewale.BAL.Filters
{
    /// <summary>
    /// Created by : Snehal Dange on 20th Feb 2018
    /// Description: Class contains the logic to give customized filters with respect to make.
    /// </summary>
    public class PageFilters : IPageFilters
    {
        /// <summary>
        /// Following dictionary contains the filtersPriority - MakeCategories mapping  
        /// </summary>
        private readonly IDictionary<BikeMakeCategoriesEnum, IEnumerable<InPageFilterEnum>> categoryFilterPriorityMap = new Dictionary<BikeMakeCategoriesEnum, IEnumerable<InPageFilterEnum>>
        {
            {BikeMakeCategoriesEnum.OnlyScooters, new List<InPageFilterEnum> {InPageFilterEnum.Budget,InPageFilterEnum.Mileage ,InPageFilterEnum.Displacement}},
            {BikeMakeCategoriesEnum.Commoners_1, new List<InPageFilterEnum> {InPageFilterEnum.Budget, InPageFilterEnum.BodyType, InPageFilterEnum.Mileage,InPageFilterEnum.Displacement}},
            {BikeMakeCategoriesEnum.Commoners_2,new List<InPageFilterEnum> {InPageFilterEnum.Budget, InPageFilterEnum.BodyType, InPageFilterEnum.Mileage,InPageFilterEnum.Displacement}},
            {BikeMakeCategoriesEnum.Premium_1, new List<InPageFilterEnum> {InPageFilterEnum.Budget, InPageFilterEnum.BodyType ,InPageFilterEnum.Displacement}},
            {BikeMakeCategoriesEnum.Premium_2,new List<InPageFilterEnum> {InPageFilterEnum.Budget, InPageFilterEnum.BodyType,InPageFilterEnum.Displacement}}
        };

        /// <summary>
        /// Created by : Snehal Dange on 21st Feb 2018
        /// Description: Method to get list of all the relevant filters on make page. 
        /// </summary>
        /// <param name="inputFilters"></param>
        /// <returns></returns>
        public IEnumerable<FilterBase> GetRelevantPageFilters(CustomInputFilters inputFilters)
        {
            IList<FilterBase> pageFilters = null;
            IEnumerable<InPageFilterEnum> categoryFilterObj = null;
            try
            {
                if (inputFilters != null)
                {
                    pageFilters = new List<FilterBase>();
                    categoryFilterObj = categoryFilterPriorityMap[(BikeMakeCategoriesEnum)inputFilters.MakeCategoryId];

                    if (categoryFilterObj != null)
                    {
                        FilterBase listObj = null;
                        double filterValue = 0;
                        foreach (var filter in categoryFilterObj)
                        {
                            if (InPageFilterEnum.Mileage.Equals(filter))
                            {
                                filterValue = inputFilters.MinMileage;
                            }
                            else if (InPageFilterEnum.Budget.Equals(filter))
                            {
                                filterValue = inputFilters.MinPrice;
                            }
                            else if (InPageFilterEnum.Displacement.Equals(filter))
                            {
                                filterValue = inputFilters.MinDisplacement;
                            }

                            listObj = RangeFactory.GetContextualFilters(filter, filterValue);

                            if (listObj != null)
                            {
                                pageFilters.Add(listObj);
                                listObj = null;
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.BAL.Filters.GetRelevantFilters");
            }
            return pageFilters;
        }

    }
}
