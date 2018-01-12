using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Notifications;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Bikewale.Cache.BikeData
{
    /// <summary>
    /// Created by  :   Sumit Kate on 03 Mar 2016
    /// Description :   Bike Makes Repository Cache
    /// </summary>
    public class BikeMakesCacheRepository : IBikeMakesCacheRepository
    {
        private readonly ICacheManager _cache;
        private readonly IBikeMakes<BikeMakeEntity, int> _objMakes;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="objMakes"></param>
        public BikeMakesCacheRepository(ICacheManager cache, IBikeMakes<BikeMakeEntity, int> objMakes)
        {
            _cache = cache;
            _objMakes = objMakes;
        }
        /// <summary>
        /// Created by  :   Sumit Kate on 03 Mar 2016
        /// Summary     :   Gets the Makes list
        /// Modified by :   Rajan Chauhan
        /// Description :   Changed key
        /// </summary>
        /// <param name="makeType">Type of make</param>
        /// <returns></returns>
        public IEnumerable<Entities.BikeData.BikeMakeEntityBase> GetMakesByType(Entities.BikeData.EnumBikeType makeType)
        {
            IEnumerable<Entities.BikeData.BikeMakeEntityBase> makes = null;
            string key = String.Format("BW_Makes_{0}", makeType.ToString());
            try
            {
                makes = _cache.GetFromCache<IEnumerable<Entities.BikeData.BikeMakeEntityBase>>(key, new TimeSpan(1, 0, 0, 0), () => _objMakes.GetMakesByType(makeType));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikeMakesCacheRepository.GetMakesByType");

            }
            return makes;
        }

        /// <summary>
        /// Created by  :   Sangram Nandkhile on 17 June 2016
        /// Summary     :   Gets the Discontinued Models for make
        /// </summary>
        /// <param name="makeType">Type of make</param>
        /// <returns></returns>
        public IEnumerable<BikeVersionEntity> GetDiscontinuedBikeModelsByMake(uint makeId)
        {
            IEnumerable<BikeVersionEntity> bikes = null;
            string key = String.Format("BW_DiscontinuedBikes_Make_{0}", makeId.ToString());
            try
            {
                bikes = _cache.GetFromCache<IEnumerable<BikeVersionEntity>>(key, new TimeSpan(24, 0, 0), () => _objMakes.GetDiscontinuedBikeModelsByMake(makeId));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikeMakesCacheRepository.GetDiscontinuedBikeModelsByMake");

            }
            return bikes;
        }

        /// <summary>
        /// Created by  : Sushil Kumar on 28th June 2016
        /// Summary     : Gets the bike description for make
        /// </summary>
        /// <param name="makeId"></param>
        /// <returns></returns>
        public BikeDescriptionEntity GetMakeDescription(uint makeId)
        {
            BikeDescriptionEntity objMakeDesc = null;
            string key = String.Format("BW_MakeDescription_{0}", makeId);
            try
            {
                objMakeDesc = _cache.GetFromCache<BikeDescriptionEntity>(key, new TimeSpan(1, 0, 0), () => _objMakes.GetMakeDescription((int)makeId));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikeMakesCacheRepository.GetMakeDescription");

            }
            return objMakeDesc;
        }



        /// <summary>
        /// Created by  : Sushil Kumar on 28th June 2016
        /// Summary     : Gets the bike details for make
        /// </summary>
        /// <param name="makeId"></param>
        /// <returns></returns>
        public BikeMakeEntityBase GetMakeDetails(uint makeId)
        {
            BikeMakeEntityBase objMakeDetails = null;
            string key = String.Format("BW_MakeDetails_{0}", makeId);
            try
            {
                objMakeDetails = _cache.GetFromCache<BikeMakeEntityBase>(key, new TimeSpan(1, 0, 0), () => _objMakes.GetMakeDetails(makeId));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikeMakesCacheRepository.GetMakeDetails");

            }
            return objMakeDetails;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 13 Sep 2016
        /// Description :   Gets All make and models from Cache if available else calls DAL
        /// </summary>
        /// <returns></returns>
        public IEnumerable<BikeMakeModelBase> GetAllMakeModels()
        {
            IEnumerable<BikeMakeModelBase> makeModels = null;
            string key = String.Format("BW_All_MakeModel");
            try
            {
                makeModels = _cache.GetFromCache<IEnumerable<BikeMakeModelBase>>(key, new TimeSpan(1, 0, 0), () => _objMakes.GetAllMakeModels());
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikeMakesCacheRepository.GetMakeDetails");

            }
            return makeModels;
        }


        /// <summary>
        /// Created by  :   Sumit Kate on 16 Sep 2016
        /// Description :   Checks valid make masking name
        /// </summary>
        /// <param name="maskingName"></param>
        /// <returns></returns>
        public MakeMaskingResponse GetMakeMaskingResponse(string maskingName)
        {
            MakeMaskingResponse response = new MakeMaskingResponse();
            Bikewale.Cache.Memcache.BWMemcache objMemcache = null;
            try
            {
                // Get MaskingNames from Memcache
                objMemcache = new Memcache.BWMemcache();
                var htNewMaskingNames = _cache.GetFromCache<Hashtable>("BW_MakeMapping", new TimeSpan(1, 0, 0), () => objMemcache.GetHashTable("BW_MakeMapping"));

                if (htNewMaskingNames != null && htNewMaskingNames.Contains(maskingName))
                {
                    response.MakeId = Convert.ToUInt32(htNewMaskingNames[maskingName]);
                }

                // If modelId is not null 
                if (response.MakeId > 0)
                {
                    response.MaskingName = maskingName;
                    response.StatusCode = 200;

                    return response;
                }
                else
                {
                    // Get old MaskingNames from memcache
                    var htOldMaskingNames = _cache.GetFromCache<Hashtable>("BW_OldMakeMaskingNames", new TimeSpan(1, 0, 0), () => _objMakes.GetOldMaskingNames());

                    // new masking name found for given masking name. Its renamed so 301 permanant redirect.
                    if (htOldMaskingNames != null && htOldMaskingNames[maskingName] != null)
                    {
                        response.MaskingName = htOldMaskingNames[maskingName].ToString();
                        response.StatusCode = 301;
                    }
                    else
                        response.StatusCode = 404;                // Not found. The given masking name does not exist on bikewale.
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("GetMakeMaskingResponse({0})", maskingName));

            }
            return response;
        }

        /// <summary>
        /// Created by : Sangram Nandkhile on 10 Mar 2017
        /// Summary: Cache layer for scooter's brands
        /// </summary>
        public IEnumerable<BikeMakeEntityBase> GetScooterMakes()
        {
            IEnumerable<BikeMakeEntityBase> makes = null;
            string key = String.Format("BW_Scooters_All_Brands");
            try
            {
                makes = _cache.GetFromCache<IEnumerable<BikeMakeEntityBase>>(key, new TimeSpan(1, 0, 0), () => _objMakes.GetScooterMakes());
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikeMakesCacheRepository.GetScooterMakes");
            }
            return makes;
        }
        /// <summary>
        /// Created by : Aditi Srivastava on 15 Mar 2017
        /// Summary    : Get scooter synopsis
        /// </summary>
        public BikeDescriptionEntity GetScooterMakeDescription(uint makeId)
        {
            BikeDescriptionEntity scooterDesc = null;
            string key = string.Format("BW_Scooter_Synopsis_MK_{0}", makeId);
            try
            {
                scooterDesc = _cache.GetFromCache<BikeDescriptionEntity>(key, new TimeSpan(1, 0, 0), () => _objMakes.GetScooterMakeDescription(makeId));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BikeMakesCacheRepository.GetScooterMakeDescription MakeId : {0}", makeId));
            }
            return scooterDesc;
        }

        /// <summary>
        /// Created By: Snehal Dange on 22nd Nov 2017
        /// Description: To cache sub-footer description and model price list for make
        /// </summary>
        /// <param name="makeId"></param>
        /// <returns></returns>
        public MakeSubFooterEntity GetMakeFooterCategoriesandPrice(uint makeId)
        {
            MakeSubFooterEntity footerContent = null;
            try
            {
                if (makeId > 0)
                {
                    string key = string.Format("BW_FooterCategoriesandPrice_MK_{0}", makeId);
                    footerContent = _cache.GetFromCache<MakeSubFooterEntity>(key, new TimeSpan(24, 0, 0), () => _objMakes.GetMakeFooterCategoriesandPrice(makeId));
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, string.Format("BikeMakesCacheRepository.GetMakeFooterCategoriesandPrice MakeId : {0}", makeId));
            }
            return footerContent;
        }


        /// <summary>
        /// Created By: Snehal Dange on 13th Dec 2017
        /// Description:  Method to get list of  makes in which dealer showroom is present for city
        /// </summary>
        /// <param name="makeId"></param>
        /// <returns></returns>
        public IEnumerable<BikeMakeEntityBase> GetDealerBrandsInCity(uint cityId)
        {
            IEnumerable<BikeMakeEntityBase> objMakesList = null;
            try
            {
                if (cityId > 0)
                {
                    string key = string.Format("BW_DealerBrandsInCity_City_{0}", cityId);
                    objMakesList = _cache.GetFromCache<IEnumerable<BikeMakeEntityBase>>(key, new TimeSpan(24, 0, 0), () => _objMakes.GetDealerBrandsInCity(cityId));
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, string.Format("BikeMakesCacheRepository.GetDealerBrandsInCity_CityId : {0}", cityId));
            }
            return objMakesList;
        }

        /// <summary>
        /// Created By: Snehal Dange on 13th Dec 2017
        /// Description:  Method to get list of  makes in which service center is present for city
        /// </summary>
        /// <param name="makeId"></param>
        /// <returns></returns>
        public IEnumerable<BikeMakeEntityBase> GetServiceCenterBrandsInCity(uint cityId)
        {
            IEnumerable<BikeMakeEntityBase> objMakesList = null;
            try
            {
                if (cityId > 0)
                {
                    string key = string.Format("BW_ServiceCenterBrandsInCity_City_{0}", cityId);
                    objMakesList = _cache.GetFromCache<IEnumerable<BikeMakeEntityBase>>(key, new TimeSpan(24, 0, 0), () => _objMakes.GetServiceCenterBrandsInCity(cityId));
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, string.Format("BikeMakesCacheRepository.GetServiceCenterBrandsInCity_CityId : {0}", cityId));
            }
            return objMakesList;
        }

    }
}
