using Bikewale.ElasticSearch.Entities;
using Bikewale.Notifications;
using Bikewale.Utility;
using BikewaleOpr.Cache;
using BikewaleOpr.Interface.BikeData;
using BikewaleOpr.Interface.BikePricing;
using BikewaleOpr.Interface.Dealers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
namespace BikewaleOpr.BAL.BikePricing
{
    /// <summary>
    /// Created by : Ashutosh Sharma on 10 Nov 2017
    /// Description : Provide BAL methods for Bikewale pricing.
    /// </summary>
    public class BwPrice : IBwPrice
    {
        private readonly IShowroomPricesRepository _showroomPricesRepository;
        private readonly IBikeModelsRepository _bikeModelsRepository;

        public BwPrice(IShowroomPricesRepository showroomPricesRepository, IBikeModelsRepository bikeModelsRepository)
        {
            _showroomPricesRepository = showroomPricesRepository;
            _bikeModelsRepository = bikeModelsRepository;
        }
        /// <summary>
        /// Created by : Ashutosh Sharma on 10 Nov 2017
        /// Description : Method to save bikewale prices in cities.
        /// Modified by : Pratibha Verma on 23 May 2018 
        /// Description : added call to update index methods and model price cache clear
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
                        BwMemCache.ClearDefaultPQVersionList(modelIdList, cityIdList);
                        BwMemCache.ClearVersionPrice(modelIdList, cityIdList);
                        ClearModelPriceCache(modelIdList);
                        UpdateBikeIndex(modelIds);
                        string cities = citiesList.Replace("|r0w|", ",");
                        UpdatePricingIndex(modelIds, cities);
                    }
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, string.Format("BAL.BwPrice.SaveBikePrices:_{0}_{1}_{2}_{3}_{4}", versionAndPriceList, citiesList, makeId, modelIds, updatedBy));
            }
            return IsSaved;
        }

        /// <summary>
        /// Created By : Deepak Israni on 22 Feb 2018
        /// Description: BAL function to push recently updated price documents to RabbitMq.
        /// Modified By: Deepak Israni on 12 March 2018
        /// Description: Updated function to push to BWEsDocumentBuilder for updating bikeindex
        /// </summary>
        /// <param name="versionIds"></param>
        /// <param name="cityIds"></param>
        public void UpdateModelPriceDocument(string versionIds, string cityIds)
        {
            string versions = ParseInput(versionIds, new string[] { "#c0l#", "|r0w|" }, 4);
            string cities = ParseInput(cityIds, new string[] { "|r0w|" }, 1);
            string models = _bikeModelsRepository.GetModelsByVersions(versions);

            UpdateBikeIndex(models);
            UpdatePricingIndex(models, cities);

            var modelIds = models.Split(',').Distinct();
            var cityIdList = cities.Split(',').Distinct();
            BwMemCache.ClearDefaultPQVersionList(modelIds, cityIdList);
            BwMemCache.ClearVersionPrice(modelIds, cityIdList);
            ClearModelPriceCache(modelIds);
        }


        /// <summary>
        /// Created By : Deepak Israni on 22 Feb 2018
        /// Description: BAL function to push newly created price documents to RabbitMq.
        /// Modified By: Deepak Israni on 12 March 2018
        /// Description: Updated function to push to BWEsDocumentBuilder for updating bikeindex
        /// </summary>
        /// <param name="modelIds"></param>
        /// <param name="cityIds"></param>
        public void CreateModelPriceDocument(string modelIds, string cityIds)
        {

            #region Update BikeIndex
            NameValueCollection packet = new NameValueCollection();
            packet["ids"] = modelIds;
            packet["indexName"] = BWOprConfiguration.Instance.BikeModelIndex;
            packet["documentType"] = "bikemodeldocument";
            packet["operationType"] = "update";
            BWESDocumentBuilder.PushToQueue(packet);
            #endregion


            #region Update BikePriceIndex
            NameValueCollection packetBikeModelPriceIndex = new NameValueCollection();
            packetBikeModelPriceIndex["modelIds"] = modelIds;
            packetBikeModelPriceIndex["cityIds"] = cityIds;
            packetBikeModelPriceIndex["indexName"] = BWOprConfiguration.Instance.BikeModelPriceIndex;
            packetBikeModelPriceIndex["documentType"] = "modelpricedocument";
            packetBikeModelPriceIndex["operationType"] = "insert";

            BWESDocumentBuilder.PushToQueue(packetBikeModelPriceIndex);
            #endregion


        }


        /// <summary>
        /// Created By : Deepak Israni on 22 Feb 2018
        /// Description: Helper function to parse input and returning it as a comma seperated string.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="seperators"></param>
        /// <param name="step"></param>
        /// <returns></returns>
        public string ParseInput(string value, string[] separators, int step)
        {
            string[] words = value.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            string ids = "";
            var len = words.Length;
            for (int i = 0; i < len; i += step)
            {
                ids += string.Format("{0},", words[i]);
            }
            ids = ids.Remove(ids.Length - 1);
            return ids;
        }

        /// <summary>
        /// Created by  : Pratibha Verma on 23 May 2018
        /// Description : method to update bike index
        /// </summary>
        /// <param name="models"></param>
        private void UpdateBikeIndex(string models)
        {
            try
            {
                NameValueCollection packet = new NameValueCollection();
                packet["ids"] = models;
                packet["indexName"] = BWOprConfiguration.Instance.BikeModelIndex;
                packet["documentType"] = "bikemodeldocument";
                packet["operationType"] = "udpate";

                BWESDocumentBuilder.PushToQueue(packet);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikewaleOpr.BAL.BikePricing.UpdateBikeIndex");
            }
        }

        /// <summary>
        /// Created by  : Pratibha Verma on 23 May 2018
        /// Description : method to update pricing index
        /// </summary>
        /// <param name="models"></param>
        /// <param name="cities"></param>
        private void UpdatePricingIndex(string models, string cities)
        {
            try
            {
                NameValueCollection nvc = new NameValueCollection();
                nvc["indexName"] = BWOprConfiguration.Instance.BikeModelPriceIndex;
                nvc["modelIds"] = models;
                nvc["cityIds"] = cities;
                nvc["documentType"] = "modelpricedocument";
                nvc["operationType"] = "update";

                BWESDocumentBuilder.PushToQueue(nvc);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikewaleOpr.BAL.BikePricing.UpdatePricingIndex");
            }
        }

        /// <summary>
        /// Created by  : Pratibha Verma on 23 May 2018
        /// Description : cache clear for model price cities
        /// </summary>
        /// <param name="modelIds"></param>
        private void ClearModelPriceCache(IEnumerable<string> modelIds)
        {
            foreach (var modelId in modelIds)
            {
                uint model;
                if (uint.TryParse(modelId, out model))
                {
                    BwMemCache.ClearModelPriceCities(model, BWConstants.FinancePopularCityCount);
                }
            }
        }
    }
}
