using Bikewale.Entities.Used;
using Bikewale.Entities.UsedBikes;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Used;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;

namespace Bikewale.Cache.Used
{
    /// <summary>
    /// Created By : Sushil Kumar on 29th August 2016
    /// Description : Used Bikes details cache repository for used bikes section
    /// </summary>
    public class UsedBikeDetailsCache : IUsedBikeDetailsCacheRepository
    {
        private readonly ICacheManager _cache;
        private readonly IUsedBikeDetails _objUsedBikes;

        /// <summary>
        /// Intitalize the references for the cache and DL
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="objUsedBikes"></param>
        public UsedBikeDetailsCache(ICacheManager cache, IUsedBikeDetails objUsedBikes)
        {
            _cache = cache;
            _objUsedBikes = objUsedBikes;
        }

        /// <summary>
        /// Created by  : Sushil Kumar on 29th August 2016
        /// Description : Cache layer profile details for the used bike
        /// </summary>
        /// <param name="inquiryId"></param>
        /// <returns></returns>
        public ClassifiedInquiryDetails GetProfileDetails(uint inquiryId)
        {
            ClassifiedInquiryDetails objUsedBikes = null;
            try
            {
                string key = String.Format("BW_ProfileDetails_V1_{0}", inquiryId);
                objUsedBikes = _cache.GetFromCache(key, new TimeSpan(0, 30, 0), () => _objUsedBikes.GetProfileDetails(inquiryId));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Cache.Used.GetProfileDetails");
            }
            return objUsedBikes;
        }


        /// <summary>
        /// Created by  : Sangram on 29th August 2016
        /// Description : Cache layer for similar used bikes for particular model
        /// </summary>
        /// <param name="inquiryId"></param>
        /// <param name="cityId"></param>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public IEnumerable<BikeDetailsMin> GetSimilarBikes(uint inquiryId, uint cityId, uint modelId, ushort topCount)
        {
            IEnumerable<BikeDetailsMin> objUsedBikes = null;
            string key = String.Format("BW_SimilarUsedBikes_Inquiry_{0}_Cnt_{1}", inquiryId, topCount);
            try
            {
                objUsedBikes = _cache.GetFromCache<IEnumerable<BikeDetailsMin>>(key, new TimeSpan(0, 30, 0), () => _objUsedBikes.GetSimilarBikes(inquiryId, cityId, modelId, topCount));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Cache.Used.GetSimilarBikes");
            }
            return objUsedBikes;
        }
        /// <summary>
        /// Created By : Subodh Jain on 2 jan 2017 
        /// Description : Get Used Bike By Model Count In City
        /// </summary>
        public IEnumerable<MostRecentBikes> GetUsedBikeByModelCountInCity(uint makeid, uint cityid, uint topcount)
        {
            IEnumerable<MostRecentBikes> objUsedBikes = null;
            string key = String.Format("BW_UsedBikeByModelCountCity_makeid_{0}_cityid_{1}_topcount_{2}", makeid, cityid, topcount);
            try
            {
                objUsedBikes = _cache.GetFromCache<IEnumerable<MostRecentBikes>>(key, new TimeSpan(1, 0, 0), () => _objUsedBikes.GetUsedBikeByModelCountInCity(makeid, cityid, topcount));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "UsedBikeDetailsCache.GetUsedBikeByModelCountInCity");
            }
            return objUsedBikes;
        }

        /// <summary>
        /// Created By : Sangram Nandkhile on 2 Feb 2017 
        /// Description : Get Used Bike models for India 
        /// </summary>
        public IEnumerable<MostRecentBikes> GetPopularUsedModelsByMake(uint makeid, uint topcount)
        {
            IEnumerable<MostRecentBikes> objUsedBikes = null;
            string key = String.Format("BW_UsedPopularModels_MK_{0}TC{1}", makeid, topcount);
            try
            {
                objUsedBikes = _cache.GetFromCache<IEnumerable<MostRecentBikes>>(key, new TimeSpan(1, 0, 0), () => _objUsedBikes.GetPopularUsedModelsByMake(makeid, topcount));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("UsedBikeDetailsCache.GetPopularUsedModelsByMake ==> makeId:{0}, TopCount {1}", makeid, topcount));
            }
            return objUsedBikes;
        }

        /// <summary>
        /// Created By : Subodh Jain on 2 jan 2017 
        /// Description : Get Used Bike By Model Count In City
        /// </summary>
        public IEnumerable<MostRecentBikes> GetUsedBikeCountInCity(uint cityid, uint topcount)
        {
            IEnumerable<MostRecentBikes> objUsedBikes = null;
            string key = String.Format("BW_GetUsedBikeCountInCity_cityid_{0}_topcount_{1}", cityid, topcount);
            try
            {
                objUsedBikes = _cache.GetFromCache<IEnumerable<MostRecentBikes>>(key, new TimeSpan(1, 0, 0), () => _objUsedBikes.GetUsedBikeCountInCity(cityid, topcount));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("UsedBikeDetailsCache.GetUsedBikeCountInCity:_cityid:{0}", cityid));
            }
            return objUsedBikes;
        }

        /// <summary>
        /// Created By : Subodh Jain on 2 jan 2017 
        /// Description : Get Used Bike By Model Count In City
        /// </summary>
        public IEnumerable<MostRecentBikes> GetUsedBike(uint topcount)
        {
            IEnumerable<MostRecentBikes> objUsedBikes = null;
            string key = String.Format("BW_GetUsedBikeCountInCity_topcount_{0}", topcount);
            try
            {
                objUsedBikes = _cache.GetFromCache<IEnumerable<MostRecentBikes>>(key, new TimeSpan(1, 0, 0), () => _objUsedBikes.GetUsedBike(topcount));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("UsedBikeDetailsCache.GetUsedBike:_topcount:{0}", topcount));
            }
            return objUsedBikes;
        }
        /// <summary>
        /// Created by  : Sangram on 29th August 2016
        /// Description : Cache layer other used bikes by city id
        /// </summary>
        /// <param name="inquiryId"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public IEnumerable<OtherUsedBikeDetails> GetOtherBikesByCityId(uint inquiryId, uint cityId, ushort topCount)
        {
            IEnumerable<OtherUsedBikeDetails> objUsedBikes = null;
            string key = String.Format("BW_OtherUsedBikes_Inquiry_{0}_Cnt_{1}", inquiryId, topCount);
            try
            {
                objUsedBikes = _cache.GetFromCache<IEnumerable<OtherUsedBikeDetails>>(key, new TimeSpan(0, 30, 0), () => _objUsedBikes.GetOtherBikesByCityId(inquiryId, cityId, topCount));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Cache.Used.GetOtherBikesByCityId");
            }
            return objUsedBikes;
        }


        /// <summary>
        /// Created by  : Sangram on 06th Oct 2016
        /// Description : Cache layer for recent bikes in India
        /// </summary>
        /// <returns></returns>
        public IEnumerable<OtherUsedBikeDetails> GetRecentUsedBikesInIndia(ushort topCount)
        {
            IEnumerable<OtherUsedBikeDetails> objUsedBikes = null;
            string key = String.Format("BW_RecentUsedBikes_Cnt_{0}", topCount);
            try
            {
                objUsedBikes = _cache.GetFromCache<IEnumerable<OtherUsedBikeDetails>>(key, new TimeSpan(0, 30, 0), () => _objUsedBikes.GetRecentUsedBikesInIndia(topCount));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Cache.Used.GetOtherBikesByCityId");
            }
            return objUsedBikes;
        }

        /// <summary>
        /// Created by : Sajal Gupta on 06-10-2016
        /// Description : Getting used bike details  by profileId
        /// </summary>
        /// <param name="profileId"></param>
        /// <returns></returns>
        public InquiryDetails GetInquiryDetailsByProfileId(string profileId, string customerId)
        {
            InquiryDetails objInquiryDetailsByProfileId = null;
            string key = String.Format("BW_Used_Profile_{0}_Cid_{1}", profileId, customerId);
            try
            {
                objInquiryDetailsByProfileId = _cache.GetFromCache<InquiryDetails>(key, new TimeSpan(1, 0, 0, 0), () => _objUsedBikes.GetInquiryDetailsByProfileId(profileId, customerId));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Exception in Cache Layer function GetInquiryDetailsByProfileId for profileId : {0}, customerId : {1}", profileId, customerId));
            }
            return objInquiryDetailsByProfileId;
        }

        /// <summary>
        /// Created by : Sajal Gupta on 30-12-2016
        /// Description : CAche function to read available used bikes in city by make
        /// </summary>
        /// <param name="profileId"></param>
        /// <returns></returns>
        public IEnumerable<UsedBikesCountInCity> GetUsedBikeInCityCountByMake(uint makeId, ushort topCount)
        {
            IEnumerable<UsedBikesCountInCity> bikesCountList = null;
            string key = String.Format("BW_Used_Bikes_City_Count_Make_{0}_Count_{1}", makeId, topCount);
            try
            {
                bikesCountList = _cache.GetFromCache<IEnumerable<UsedBikesCountInCity>>(key, new TimeSpan(1, 0, 0), () => _objUsedBikes.GetUsedBikeInCityCountByMake(makeId, topCount));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Exception in Cache Layer function GetUsedBikeInCityCount for makeId : {0}, {1}", makeId, topCount));
            }
            return bikesCountList;
        }

        /// <summary>
        /// Created by : Sajal Gupta on 03-01-2017
        /// Description : Cache function to read available used bikes in city by model
        /// </summary>
        /// <param name="profileId"></param>
        /// <returns></returns>
        public IEnumerable<UsedBikesCountInCity> GetUsedBikeInCityCountByModel(uint modelId, ushort topCount)
        {
            IEnumerable<UsedBikesCountInCity> bikesCountList = null;
            string key = String.Format("BW_Used_Bikes_City_Count_Model_{0}_Count_{1}", modelId, topCount);
            try
            {
                bikesCountList = _cache.GetFromCache<IEnumerable<UsedBikesCountInCity>>(key, new TimeSpan(1, 0, 0), () => _objUsedBikes.GetUsedBikeInCityCountByModel(modelId, topCount));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Exception in Cache Layer function GetUsedBikeInCityCountByModel for modelId : {0}, {1}", modelId, topCount));
            }
            return bikesCountList;
        }
    }
}
