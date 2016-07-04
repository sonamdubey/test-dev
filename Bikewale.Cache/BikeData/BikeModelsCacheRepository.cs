﻿using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;

namespace Bikewale.Cache.BikeData
{
    /// <summary>
    /// Written By : Ashish G. Kamble on 7 Oct 2015
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    public class BikeModelsCacheRepository<T, U> : IBikeModelsCacheRepository<U>
    {
        private readonly ICacheManager _cache;
        private readonly IBikeModels<T, U> _objModels;
        private readonly IBikeModelsRepository<T, U> _modelRepository;

        /// <summary>
        /// Intitalize the references for the cache and BL
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="objModels"></param>
        public BikeModelsCacheRepository(ICacheManager cache, IBikeModels<T, U> objModels, IBikeModelsRepository<T, U> modelRepository)
        {
            _cache = cache;
            _objModels = objModels;
            _modelRepository = modelRepository;
        }

        /// <summary>
        /// Written By : Ashish G. Kamble on 8 Oct 2015
        /// Summary : Function to get the model page details from the cache. If data is not available in the cache it will return data from BL.
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns>Returns BikeModelPageEntity</returns>
        public BikeModelPageEntity GetModelPageDetails(U modelId)
        {
            BikeModelPageEntity objModelPage = null;
            string key = "BW_ModelDetails_" + modelId;

            try
            {
                objModelPage = _cache.GetFromCache<BikeModelPageEntity>(key, new TimeSpan(1, 0, 0), () => _objModels.GetModelPageDetails(modelId));
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BikeModelsCacheRepository.GetModelPageDetails");
                objErr.SendMail();
            }

            return objModelPage;
        }


        /// <summary>
        /// Written By : Sushil Kumar on 28th June 2016
        /// Summary : Function to get the upcoming bikes. If data is not available in the cache it will return data from BL.
        /// </summary>
        /// <param name="sortBy"></param>
        /// <param name="pageSize"></param>
        /// <param name="makeId"></param>
        /// <param name="modelId"></param>
        /// <param name="curPageNo"></param>
        /// <returns>Returns List<UpcomingBikeEntity></returns>
        public IEnumerable<UpcomingBikeEntity> GetUpcomingBikesList(EnumUpcomingBikesFilter sortBy, int pageSize, int? makeId = null, int? modelId = null, int? curPageNo = null)
        {
            IEnumerable<UpcomingBikeEntity> objUpcoming = null;
            string key = string.Format("BW_UpcomingBikes_Cnt_{0}_SO_{1}", pageSize, (int)sortBy);

            if (makeId.HasValue && makeId.Value > 0)
                key += "_MK_" + makeId;

            if (modelId.HasValue && modelId.Value > 0)
                key += "_MO_" + modelId;

            try
            {
                objUpcoming = _cache.GetFromCache<IEnumerable<UpcomingBikeEntity>>(key, new TimeSpan(1, 0, 0), () => _objModels.GetUpcomingBikesList(sortBy, pageSize, makeId, modelId, curPageNo));
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BikeModelsCacheRepository.GetUpcomingBikesList");
                objErr.SendMail();
            }

            return objUpcoming;
        }

        /// <summary>
        /// Written By : Sushil Kumar on 28th June 2016
        /// Summary : Function to get the popular bikes by make . If data is not available in the cache it will return data from BL.
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns>Returns BikeModelPageEntity</returns>
        public IEnumerable<MostPopularBikesBase> GetMostPopularBikesByMake(int makeId)
        {
            IEnumerable<MostPopularBikesBase> objBikes = null;
            string key = "BW_PopularBikesByMake_" + makeId;

            try
            {
                objBikes = _cache.GetFromCache<IEnumerable<MostPopularBikesBase>>(key, new TimeSpan(1, 0, 0), () => _modelRepository.GetMostPopularBikesByMake(makeId));
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BikeModelsCacheRepository.GetMostPopularBikesByMake");
                objErr.SendMail();
            }

            return objBikes;
        }


        /// <summary>
        /// Created by  :   Sumit Kate on 01 Jul 2016
        /// Description :   Returns New Launched Bike List
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        public NewLaunchedBikesBase GetNewLaunchedBikesList(int startIndex, int endIndex)
        {
            NewLaunchedBikesBase objBikes = null;
            string key = String.Format("BW_NewLaunchedBikes_SI_{0}_EI_{1}", startIndex, endIndex);

            try
            {
                objBikes = _cache.GetFromCache<NewLaunchedBikesBase>(key, new TimeSpan(1, 0, 0), () => _modelRepository.GetNewLaunchedBikesList(startIndex, endIndex));
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BikeModelsCacheRepository.GetMostPopularBikesByMake");
                objErr.SendMail();
            }

            return objBikes;
        }
    }
}
