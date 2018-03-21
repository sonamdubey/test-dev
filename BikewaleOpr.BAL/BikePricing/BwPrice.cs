using Bikewale.ElasticSearch.Entities;
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

            IList<ModelPriceDocument> indexDocs = _showroomPricesRepository.GetModelPriceDocument(models, cities);

            NameValueCollection packet = new NameValueCollection();
            packet["ids"] = models;
            packet["indexName"] = BWOprConfiguration.Instance.BikeModelIndex;
            packet["documentType"] = "bikemodeldocument";
            packet["operationType"] = "udpate";

            BWESDocumentBuilder.PushToQueue(packet);

            foreach (ModelPriceDocument doc in indexDocs)
            {
                NameValueCollection nvc = new NameValueCollection();
                nvc["indexName"] = BWOprConfiguration.Instance.BikeModelPriceIndex;
                nvc["documentType"] = "modelpricedocument";
                nvc["documentId"] = doc.Id;
                nvc["operationType"] = "update";
                nvc["documentJson"] = JsonConvert.SerializeObject(doc);

                BWESIndexUpdater.PushToQueue(nvc);
            }

            var modelIds = models.Split(',').Distinct();
            var cityIdList = cities.Split(',').Distinct();
            BwMemCache.ClearDefaultPQVersionList(modelIds, cityIdList);
            BwMemCache.ClearVersionPrice(modelIds, cityIdList);
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
            IList<ModelPriceDocument> indexDocs = _showroomPricesRepository.GetModelPriceDocument(modelIds, cityIds);

            NameValueCollection packet = new NameValueCollection();
            packet["ids"] = modelIds;
            packet["indexName"] = BWOprConfiguration.Instance.BikeModelIndex;
            packet["documentType"] = "bikemodeldocument";
            packet["operationType"] = "update";

            BWESDocumentBuilder.PushToQueue(packet);

            foreach (ModelPriceDocument doc in indexDocs)
            {
                NameValueCollection nvc = new NameValueCollection();
                nvc["indexName"] = BWOprConfiguration.Instance.BikeModelPriceIndex;
                nvc["documentType"] = "modelpricedocument";
                nvc["documentId"] = doc.Id;
                nvc["operationType"] = "insert";
                nvc["documentJson"] = JsonConvert.SerializeObject(doc);

                BWESIndexUpdater.PushToQueue(nvc);
            }
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
    }
}
