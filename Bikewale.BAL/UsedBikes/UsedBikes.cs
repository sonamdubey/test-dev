using Bikewale.Cache.Core;
using Bikewale.Cache.Used;
using Bikewale.DAL.Used;
using Bikewale.DAL.UsedBikes;
using Bikewale.Entities.Used;
using Bikewale.Entities.UsedBikes;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Used;
using Bikewale.Interfaces.UsedBikes;
using Bikewale.Notifications;
using Bikewale.Utility;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;

namespace Bikewale.BAL.UsedBikes
{
    /// <summary>
    /// Created By : Sajal Gupta on 14/09/2016
    /// Description : Business logic to get used bikes for model/make page.
    /// </summary>
    public class UsedBikes : IUsedBikes
    {
        /// <summary>
        /// Created By : Sajal Gupta on 14/09/2016
        /// Description : Logic to get used bikes for model/make page.
        /// </summary>
        public IEnumerable<MostRecentBikes> GetPopularUsedBikes(uint makeId, uint modelId, uint cityId, uint totalCount)
        {
            IUsedBikesRepository usedBikesRepository = null;

            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IUsedBikesRepository, UsedBikesRepository>();
                    usedBikesRepository = container.Resolve<IUsedBikesRepository>();
                }

                if (modelId != 0)
                {
                    if (cityId != 0)
                        return usedBikesRepository.GetUsedBikesbyModelCity(modelId, cityId, totalCount);
                    else
                        return usedBikesRepository.GetUsedBikesbyModel(modelId, totalCount);
                }
                else
                {
                    if (cityId != 0)
                        return usedBikesRepository.GetUsedBikesbyMakeCity(makeId, cityId, totalCount);
                    else
                        return usedBikesRepository.GetUsedBikesbyMake(makeId, totalCount);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, String.Format("Exception in Bikewale.BAL.UsedBikes.UsedBikes.GetPopularUsedBikes parametres makeId : {0}, modelId : {1}, cityId : {2}, totalCount : {3}", makeId, modelId, cityId, totalCount));
                objErr.SendMail();
                return null;
            }
        }

        /// <summary>
        /// Created by : Sajal Gupta on 06-10-2016
        /// Description : Getting used bike details  by profileId
        /// </summary>
        /// <param name="profileId"></param>
        /// <returns></returns>
        public InquiryDetails GetInquiryDetailsByProfileId(string profileId)
        {
            InquiryDetails objInquiryDetailsByProfileId = null;
            try
            {
                if (!UsedBikeProfileId.IsValidProfileId(profileId))
                    return null;

                profileId = profileId.Substring(1);

                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IUsedBikeDetailsCacheRepository, UsedBikeDetailsCache>()
                             .RegisterType<ICacheManager, MemcacheManager>()
                             .RegisterType<IUsedBikeDetails, UsedBikeDetailsRepository>();
                    UsedBikeDetailsCache obj = container.Resolve<UsedBikeDetailsCache>();
                    objInquiryDetailsByProfileId = obj.GetInquiryDetailsByProfileId(profileId);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("Exception in BAL Layer function GetInquiryDetailsByProfileId for profileId : {0}", profileId));
                objErr.SendMail();
            }
            return objInquiryDetailsByProfileId;
        }
    }

}


