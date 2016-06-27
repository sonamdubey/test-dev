using Bikewale.Entities.Location;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Location;
using Bikewale.Notifications;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Bikewale.Cache.Location
{
    public class StateCacheRepository : IStateCacheRepository
    {
        private readonly IState _objState = null;
        private readonly ICacheManager _cache = null;

        /// <summary>
        /// Constructor to initialize the member variables
        /// </summary>
        /// <param name="objCity"></param>
        /// <param name="cache"></param>
        public StateCacheRepository(IState objState, ICacheManager cache)
        {
            _objState = objState;
            _cache = cache;
        }

        /// Created By : Vivek Gupta 
        /// Date : 24 june 2016
        /// Desc: get dealer states
        /// </summary>
        /// <param name="makeId"></param>
        /// <returns></returns>
        public IEnumerable<DealerStateEntity> GetDealerStates(uint makeId)
        {
            IEnumerable<DealerStateEntity> objStates = null;
            string key = string.Empty;
            try
            {
                key = String.Format("BW_StatewiseDealersCnt_Make_{0}", makeId);
                objStates = _cache.GetFromCache<IEnumerable<DealerStateEntity>>(key, new TimeSpan(30, 0, 0, 0), () => _objState.GetDealerStates(makeId));
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BikeCompareCacheRepository.GetDealerStates");
                objErr.SendMail();
            }
            return objStates;
        }

        public StateMaskingResponse GetStateMaskingResponse(string maskingName)
        {
            StateMaskingResponse response = new StateMaskingResponse();

            try
            {
                // Get MaskingNames from Memcache
                var htNewMaskingNames = _cache.GetFromCache<Hashtable>("BW_NewStateMaskingNames", new TimeSpan(1, 0, 0), () => _objState.GetMaskingNames());

                if (htNewMaskingNames.Contains(maskingName))
                {
                    response.StateId = Convert.ToUInt32(htNewMaskingNames[maskingName]);
                }

                // If modelId is not null 
                if (response.StateId > 0)
                {
                    response.MaskingName = maskingName;
                    response.StatusCode = 200;

                    return response;
                }
                else
                    response.StatusCode = 404;                // Not found. The given masking name does not exist on bikewale.
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "StateMaskingCache.GetStateMaskingResponse");
                objErr.SendMail();
            }

            return response;
        }
    }
}
