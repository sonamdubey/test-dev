using Bikewale.Entities.BikeData;
using Bikewale.Entities.Used;
using Bikewale.Entities.UsedBikes;
using Bikewale.Interfaces.Used;
using Bikewale.Interfaces.UsedBikes;
using Bikewale.Notifications;
using Bikewale.Utility;
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
        private readonly IUsedBikesRepository _usedBikesRepository = null;
        private readonly IUsedBikeDetailsCacheRepository _usedBikeDetailsCacheRepository = null;
        public UsedBikes(IUsedBikesRepository usedBikesRepository, IUsedBikeDetailsCacheRepository usedBikeDetailsCacheRepository)
        {
            _usedBikesRepository = usedBikesRepository;
            _usedBikeDetailsCacheRepository = usedBikeDetailsCacheRepository;
        }
        /// <summary>
        /// Created By : Sajal Gupta on 14/09/2016
        /// Description : Logic to get used bikes for model/make page.
        /// </summary>
        public IEnumerable<MostRecentBikes> GetPopularUsedBikes(uint makeId, uint modelId, uint cityId, uint totalCount)
        {
            try
            {
                if (modelId != 0)
                {
                    if (cityId != 0)
                        return _usedBikesRepository.GetUsedBikesbyModelCity(modelId, cityId, totalCount);
                    else
                        return _usedBikesRepository.GetUsedBikesbyModel(modelId, totalCount);
                }
                else
                {
                    if (cityId != 0)
                        return _usedBikesRepository.GetUsedBikesbyMakeCity(makeId, cityId, totalCount);
                    else
                        return _usedBikesRepository.GetUsedBikesbyMake(makeId, totalCount);
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
        /// Created by: Sangram Nandkhile on 06th oct 2016
        /// Summary: Fetch used bikes make from DAL
        /// </summary>
        /// <returns></returns>
        public IEnumerable<UsedBikeMakeEntity> GetUsedBikeMakesWithCount()
        {
            IEnumerable<UsedBikeMakeEntity> usedBikeMakes = null;
            try
            {
                usedBikeMakes = _usedBikesRepository.GetUsedBikeMakesWithCount();
                return usedBikeMakes;
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception in Bikewale.BAL.UsedBikes.UsedBikes.GetUsedBikeMakesWithCount");
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

                string argProfileId = profileId.Substring(1);

                objInquiryDetailsByProfileId = _usedBikeDetailsCacheRepository.GetInquiryDetailsByProfileId(argProfileId);

                if (objInquiryDetailsByProfileId != null)
                    objInquiryDetailsByProfileId.ProfileId = profileId;
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


