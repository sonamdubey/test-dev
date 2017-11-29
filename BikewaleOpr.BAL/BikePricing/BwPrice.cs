using Bikewale.Notifications;
using BikewaleOpr.Cache;
using BikewaleOpr.Interface.BikePricing;
using BikewaleOpr.Interface.Dealers;
using System;
namespace BikewaleOpr.BAL.BikePricing
{
    /// <summary>
    /// Created by : Ashutosh Sharma on 10 Nov 2017
    /// Description : Provide BAL methods for Bikewale pricing.
    /// </summary>
    public class BwPrice : IBwPrice
    {
        private readonly IShowroomPricesRepository _showroomPricesRepository;
        public BwPrice(IShowroomPricesRepository showroomPricesRepository)
        {
            _showroomPricesRepository = showroomPricesRepository;
        }
        /// <summary>
        /// Created by : Ashutosh Sharma on 10 Nov 2017
        /// Description : Method to save bikewale prices in cities.
        /// </summary>
        /// <param name="versionAndPriceList">Bike version id list and price list in format "versionId#c0l#ex-showroom#c0l#insurance#c0l#rto|r0w|"</param>
        /// <param name="citiesList">City id list in format "cityid|r0w|"</param>
        /// <param name="updatedBy">User updation by</param>
        /// <returns></returns>
        public bool SaveBikePrices(string versionAndPriceList, string citiesList, uint makeId, string modelIds, int updatedBy)
        {
            bool IsSaved = false;
            try
            {
                if (!string.IsNullOrEmpty(versionAndPriceList) && !string.IsNullOrEmpty(citiesList) && !string.IsNullOrEmpty(modelIds) && makeId > 0 && updatedBy > 0)
                {
                    IsSaved = _showroomPricesRepository.SaveBikePrices(versionAndPriceList, citiesList, updatedBy);
                    if (IsSaved)
                    {
                        BwMemCache.ClearPopularBikesByMakeWithCityPriceCacheKey(makeId);
                        BwMemCache.ClearPopularBikesByMakes(makeId);
                        BwMemCache.ClearUpcomingBikes();

                        string[] modelIdList = modelIds.Split(',');
                        string[] versionPriceList = versionAndPriceList.Split(new string[] { "|r0w|" }, StringSplitOptions.RemoveEmptyEntries);
                        string[] cityIdList = citiesList.Split(new string[] { "|r0w|" }, StringSplitOptions.RemoveEmptyEntries);

                        foreach (var modelId in modelIdList)
                        {
                            BwMemCache.ClearVersionDetails(Convert.ToUInt32(modelId));
                            BwMemCache.ClearPriceQuoteOfTopCities(Convert.ToUInt32(modelId), 8);
                            foreach (var cityId in cityIdList)
                            {
                                BwMemCache.ClearModelPriceInNearestCities(Convert.ToUInt32(modelId), Convert.ToUInt32(cityId), 8);
                                BwMemCache.ClearMostPopularBikesByModelBodyStyle(Convert.ToUInt32(modelId), Convert.ToUInt32(cityId), 8);
                            }
                        }
                        foreach (var versionPrice in versionPriceList)
                        {
                            string versionId = versionPrice.Substring(0, versionPrice.IndexOf('#'));
                            foreach (var cityId in cityIdList)
                            {
                                BwMemCache.ClearSimilarBikesList(Convert.ToUInt32(versionId), 9, Convert.ToUInt32(cityId));
                            }
                        }
                        

                    }
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, string.Format("BAL.BwPrice.SaveBikePrices:_{0}_{1}_{2}_{3}_{4}", versionAndPriceList, citiesList, makeId, modelIds, updatedBy));
            }
            return IsSaved;
        }
    }
}
