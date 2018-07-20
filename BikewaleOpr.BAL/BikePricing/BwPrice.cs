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
    /// Modified by : Prabhu Puredla on 19 june 2018
    /// Description : Added GetRegistrationCharges for calculating rto
    /// </summary>
    public class BwPrice : IBwPrice
    {
        private readonly IShowroomPricesRepository _showroomPricesRepository;
        private readonly IBikeModelsRepository _bikeModelsRepository;
        private readonly IBikeVersionsCacheRepository _versionCacheRepo;

        public BwPrice(IShowroomPricesRepository showroomPricesRepository, IBikeModelsRepository bikeModelsRepository, IBikeVersionsCacheRepository versionCacheRepo)
        {
            _showroomPricesRepository = showroomPricesRepository;
            _bikeModelsRepository = bikeModelsRepository;
            _versionCacheRepo = versionCacheRepo;
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
        /// Created By : Prabhu Puredla on 26 June 2018
        /// Description: BAL function to push recently updated price documents to RabbitMq.
        /// </summary>
        /// <param name="versionIds"></param>
        /// <param name="cityIds"></param>
        public void UpdateModelPriceDocumentV2(IEnumerable<uint> modelIds, IEnumerable<uint> cityIds)
        {
            string modelIdsString = string.Join(",", modelIds.Select(n => n));
            UpdateBikeIndex(modelIdsString);
            UpdatePricingIndex(modelIdsString, string.Join(",", cityIds.Select(n => n.ToString()).ToArray()));

            BwMemCache.ClearDefaultPQVersionList(modelIds, cityIds);
            BwMemCache.ClearVersionPrice(modelIds, cityIds);
            ClearModelPriceCacheV2(modelIds);
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
            if (ids.Length > 1)
            {
                ids = ids.Remove(ids.Length - 1);
            }
            return ids;
        }
        public double GetRegistrationCharges(uint versionId,uint stateId, double price)
        {
            double regCharges = 0, roadTax = 0;
            BikewaleOpr.Entities.BikeData.BikeVersionEntity objVersion = null;

            if (versionId > 0)
            {
                try
                {
                    //using (IUnityContainer container = new UnityContainer())
                    //{
                    //    container.RegisterType<Bikewale.Interfaces.Cache.Core.ICacheManager, Bikewale.Cache.Core.MemcacheManager>();
                    //    container.RegisterType<BikewaleOpr.Interface.BikeData.IBikeVersionsCacheRepository, BikewaleOpr.Cache.BikeData.BikeVersionsCacheRepository>();
                    //    container.RegisterType<BikewaleOpr.Interface.BikeData.IBikeVersions, BikewaleOpr.DALs.Bikedata.BikeVersionsRepository>();

                       /// BikewaleOpr.Interface.BikeData.IBikeVersionsCacheRepository versionsRepo = container.Resolve<BikewaleOpr.Interface.BikeData.IBikeVersionsCacheRepository>();

                    objVersion = _versionCacheRepo.GetVersionDetails(versionId);
                    //}
                }
                catch (Exception ex)
                {
                    ErrorClass.LogError(ex, String.Format("BikeWaleOpr.Common.CommonOpn.GetRegistrationCharges_version_{0}_state_{1}_price_{2}", versionId, stateId, price));
                }
            }
            

            if (objVersion != null)
            {
                double tmpTax = 0;
                switch (stateId)
                {
                    // Maharashtra.
                    /*
                        Maharashtra	Indian	0-99 cc	8%
                        Maharashtra	Indian	100-299 cc	9%
                        Maharashtra	Indian	300 + CC	10%
                        Maharashtra	Imported	0-99 cc	16%
                        Maharashtra	Imported	100-299 cc	18%

                    */
                    case 1:
                        if (objVersion.Displacement >= 0 && objVersion.Displacement <= 99)
                        {
                            if (objVersion.IsImported)
                            {
                                roadTax = price * 0.18;
                            }
                            else
                            {
                                roadTax = price * 0.10;
                            }
                        }
                        else if (objVersion.Displacement > 100 && objVersion.Displacement <= 299)
                        {
                            if (objVersion.IsImported)
                            {
                                roadTax = price * 0.20;
                            }
                            else
                            {
                                roadTax = price * 0.11;
                            }
                        }
                        else
                        {
                            if (objVersion.IsImported)
                            {
                                roadTax = price * 0.22;
                            }
                            else
                            {
                                roadTax = price * 0.12;
                            }
                        }


                        break;
                    // Tamilnadu	All		8%
                    case 11:
                        roadTax = price * 0.08;
                        break;
                    // Andhra Pradesh. 
                    // 12% if price upto 1000000
                    // 14% for others
                    case 6:
                        roadTax = price * 0.09;
                        break;

                    // calculation for telangana
                    case 41:
                        roadTax = price * 0.09;
                        break;

                    // Delhi. 
                    /*
                        Delhi		0-25000 Rs.	2%
                        Delhi		25000-40000	4%
                        Delhi		40000 - 60000	6%
                        Delhi		60000 +	8%
                    */
                    case 5:
                        if (price <= 25000)
                        {
                            roadTax = price * 0.02;
                        }
                        else if (price > 25000 && price <= 40000)
                        {
                            roadTax = price * 0.04;
                        }
                        else if (price > 40000 && price <= 60000)
                        {
                            roadTax = price * 0.06;
                        }
                        else
                        {
                            roadTax = price * 0.08;
                        }
                        break;
                    // GOA. 
                    // 5% if price less than or equal to 600000 + 310
                    // Above 6L = 6%+310
                    case 17:
                        if (price <= 200000)
                        {
                            roadTax = price * 0.08;
                            //roadTax += 310;
                        }
                        else
                        {
                            roadTax = price * 0.12;
                            //roadTax += 310;
                        }
                        break;

                    // ASSAM. 
                    //Assam	<65 kg		2600
                    //Assam	65-90 kg		3600
                    //Assam	90-135 kg		5000
                    //Assam	135-165 kg		5500
                    //Assam	>165 kg		6500
                    case 16:
                        if (objVersion.KerbWeight < 65)
                        {
                            roadTax = 2600;
                        }
                        else if (objVersion.KerbWeight >= 65 && objVersion.KerbWeight <= 90)
                        {
                            roadTax = 3600;
                        }
                        else if (objVersion.KerbWeight > 90 && objVersion.KerbWeight <= 135)
                        {
                            roadTax = 5000;
                        }
                        else if (objVersion.KerbWeight > 135 && objVersion.KerbWeight <= 165)
                        {
                            roadTax = 5500;
                        }
                        else
                        {
                            roadTax = 6500;
                        }
                        break;

                    //Uttar Pradesh

                    // Uttar Prdesh	All		10%
                    case 15:
                        roadTax = price * 0.1;
                        break;
                    // Madhya Pradesh. 7%!
                    case 3:
                        if (objVersion.BikeFuelType == 5) //FuelType - 5 (Electri Bikes)
                            roadTax = price * 0.05;
                        else
                            roadTax = price * 0.07;
                        break;
                    // Orissa. 5%!
                    case 20:
                        roadTax = price * 0.05;
                        break;
                    // Gujarat. 5.2174%, 10.434% (imported)!
                    case 9:
                        roadTax = price * 0.06;
                        break;

                    //Chhattisgarh 
                    //0-500000 - 5% of Ex
                    //500000+  - 7% of Ex
                    case 8:
                        if (price <= 500000)
                        {
                            roadTax = price * 0.05;
                        }
                        else
                        {
                            roadTax = price * 0.07;
                        }
                        break;
                    // Karnataka. 
                    // 14.3% if price up to 500000
                    // 15.4% if price upto 1000000
                    // 18.7% if price upto 2000000
                    // 19.8% otherwise.
                    case 2:
                        if (objVersion.BikeFuelType == 5)
                        {
                            roadTax = price * 0.04;
                        }
                        else
                        {
                            if (price <= 50000)
                            {
                                roadTax = price * 0.11;
                            }
                            else
                            {
                                roadTax = price * 0.13;
                            }
                        }
                        break;
                    // Bihar. 
                    case 14:
                        roadTax = price * 0.07;
                        break;
                    // Kerala. 6%!
                    case 4:
                        roadTax = price * 0.06;
                        break;
                    // Rajasthan. 
                    // 5% if price less than 6 lakh
                    // 8% if price less than 10 lakh
                    // 10% otherwise.
                    case 10:

                        if (objVersion.Displacement >= 50)
                        {
                            roadTax = price * 0.04;
                        }
                        else
                        {
                            roadTax = price * 0.06;
                        }
                        roadTax += 500; //Green Tax for Rajasthan
                        break;
                    // Uttaranchal. 2%!
                    case 25:
                        if (price <= 1000000)
                        {
                            roadTax = price * 0.04;
                        }
                        else
                        {
                            roadTax = price * 0.05;
                        }

                        //roadTax = price * 0.02;
                        break;
                    // Manipur. 
                    // Rs.2925 till 1000kg
                    // Rs.3600 till 1500kg
                    // Rs.4500 till 2000kg
                    // Rs. 4500 + 2925 more than 2000kg
                    case 36:
                        if (objVersion.KerbWeight <= 1000)
                        {
                            roadTax = 2925;
                        }
                        else if (objVersion.KerbWeight <= 1500)
                        {

                            roadTax = 3600;
                        }
                        else if (objVersion.KerbWeight <= 2000)
                        {
                            roadTax = 4500;
                        }
                        else
                        {
                            roadTax = 4500 + 2925;
                        }
                        break;
                    // Punjab. 2%!
                    case 18:
                        if (objVersion.Displacement >= 50)
                        {
                            roadTax = price * 0.015;
                        }
                        else
                        {
                            roadTax = price * 0.03;
                        }
                        break;
                    // Chandigarh. 
                    //if price < 7 lakh Then 2% + 3000.
                    //if price > 7 lakh and price < 20 lakh 3% + 5000
                    //if price > 20 lakh Then 4% + 10000.
                    case 21:
                        if (price <= 100000)
                            roadTax = (price * 0.0268);
                        else if (price > 100000 && price <= 400000)
                            roadTax = (price * 0.0357);
                        else
                            roadTax = (price * 0.0446);
                        break;
                    // Haryana. 
                    //Haryana	< 75,000		4%
                    //Haryana	75,000 - 2 Lakh		6%
                    //Haryana	2 + Lakh		8%

                    case 22:
                        if (price < 75000)
                            roadTax = (price * 0.04);
                        else if (price >= 75000 && price <= 200000)
                            roadTax = (price * 0.06);
                        else
                            roadTax = (price * 0.08);
                        break;

                    //west bengal

                    //West Bengal		Upto 80 cc	6.5% of vehicle cost or Rs 1800 (whichever is higher)
                    //West Bengal		Between 80 and 160 cc	9% of vehicle cost or Rs 3600 (whichever is higher)
                    //West Bengal		More than 160 cc	10% of vehicle cost or Rs 5800 (whichever is higher)
                    case 12:
                        //double tmpTax = 0;

                        if (objVersion.Displacement >= 0 && objVersion.Displacement <= 80)
                        {
                            tmpTax = price * 0.065;

                            if (tmpTax <= 1800)
                            {
                                roadTax = 1800;
                            }
                            else
                            {
                                roadTax = tmpTax;
                            }
                        }
                        else if (objVersion.Displacement > 80 && objVersion.Displacement <= 160)
                        {
                            tmpTax = price * 0.09;

                            if (tmpTax <= 3600)
                            {
                                roadTax = 3600;
                            }
                            else
                            {
                                roadTax = tmpTax;
                            }
                        }
                        else
                        {
                            tmpTax = price * 0.1;

                            if (tmpTax <= 5800)
                            {
                                roadTax = 5800;
                            }
                            else
                            {
                                roadTax = tmpTax;
                            }
                        }
                        break;

                    // Jharkhand. 
                    // 0-5 Seater 3% of Ex-showroom
                    // 6-8 seater 4% of Ex-showroom
                    //8 + seater 5% of Ex-showroom
                    case 23:
                        roadTax = price * 0.03;
                        break;

                    // Arunachal Pradesh
                    // Correct Logic:
                    //If weight < 100 Kgs		Rs. 2000 + Rs. 90
                    //If weight 100 - 135 Kgs	Rs.3000 + Rs. 90
                    //If weight > 135 Kgs		Rs. 3500 + Rs. 90
                    case 35:
                        if (objVersion.KerbWeight <= 100)
                        {
                            roadTax = 2090;
                        }
                        else if (objVersion.KerbWeight > 100 && objVersion.KerbWeight <= 135)
                        {
                            roadTax = 3090;
                        }
                        else
                        {
                            roadTax = 3590;
                        }
                        break;

                    // Daman & Diu
                    // If price < 2 lakh	8%
                    // If price > 2 lakh	12%
                    case 38:
                        if (objVersion.IsImported)
                        {
                            roadTax = price * 0.05;
                        }
                        else
                        {
                            roadTax = price * 0.025;
                        }
                        break;

                    //Jammu & Kashmir
                    //If Body-Style is Scooter	Rs. 2400
                    //If Body-Style is not Scooter	Rs. 4000
                    case 24:
                        if (objVersion.BodyStyleId == 5)
                        {
                            roadTax = 2400;
                        }
                        else
                        {
                            roadTax = 4000;
                        }
                        break;

                    //Meghalaya
                    //If weight < 65 Kgs		Rs. 1050
                    //If weight 65 - 90 Kgs		Rs. 1725
                    //If weight 90 - 135 Kgs	Rs. 2400
                    //If weight > 135 Kgs		Rs. 2850
                    case 13:
                        if (objVersion.KerbWeight <= 65)
                        {
                            roadTax = 1050;
                        }
                        else if (objVersion.KerbWeight > 65 && objVersion.KerbWeight <= 90)
                        {
                            roadTax = 1725;
                        }
                        else if (objVersion.KerbWeight > 90 && objVersion.KerbWeight <= 135)
                        {
                            roadTax = 2400;
                        }
                        else
                        {
                            roadTax = 2850;
                        }
                        break;

                    //Tripura
                    //If bike is without gear	Rs. 1000
                    //If bike is with gear
                    //If price < 1 lakh	RS. 2200
                    //If price > 1 lakh	RS. 2650
                    case 26:
                        if (objVersion.BodyStyleId == 5)
                        {
                            roadTax = 1000;
                        }
                        else
                        {
                            if (price <= 100000)
                            {
                                roadTax = 2200;
                            }
                            else
                            {
                                roadTax = 2650;
                            }
                        }
                        break;


                    default:
                        break;
                }

                // now include approx 1% of price or 4000 flat 
                // as dealer commission, service and handling charges etc.

                if (regCharges != 0)
                {
                    regCharges = roadTax + regCharges;
                }
                else
                {
                    if (price <= 500000)
                    {
                        regCharges = roadTax + 1500;
                    }
                    else if (price > 500000 && price <= 800000)
                    {
                        regCharges = roadTax + 5000;
                    }
                    else if (price > 800000 && price <= 1500000)
                    {
                        regCharges = roadTax + 8000;
                    }
                    else if (price > 1500000 && price <= 3000000)
                    {
                        regCharges = roadTax + 12000;
                    }
                    else if (price > 3000000 && price <= 8000000)
                    {
                        regCharges = roadTax + 25000;
                    }
                    else
                    {
                        regCharges = roadTax + 50000;
                    }

                    // RTO Calculations for electric bikes whose speed is less than 25
                    if (objVersion.BikeFuelType == 5)
                    {
                        if (objVersion.TopSpeed <= 25 && objVersion.TopSpeed > 0)
                            regCharges = 0;
                    }
                }
            }

            return regCharges;
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
        /// <summary>
        /// Created by  : Prabhu Puredla on 26 june 2018
        /// Description : Cache clear for model price cities
        /// </summary>
        /// <param name="modelIds"></param>
        private void ClearModelPriceCacheV2(IEnumerable<uint> modelIds)
        {
            foreach (var modelId in modelIds)
            {
                BwMemCache.ClearModelPriceCities(modelId, BWConstants.FinancePopularCityCount);               
            }
        }
        
    }
}
