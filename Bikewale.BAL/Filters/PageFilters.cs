using Bikewale.Entities.BikeData;
using Bikewale.Entities.Filters;
using Bikewale.Interfaces.Filters;
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

    }
}
