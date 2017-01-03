using Bikewale.Entities.Used;
using Bikewale.Entities.UsedBikes;
using System.Collections.Generic;

namespace Bikewale.Interfaces.Used
{
    /// <summary>
    /// Created By : Sushil Kumar on 27th August 2016
    /// Description : Cache Interface for used bikes
    /// Modified by : Sajal Gupta on 07/10/2016
    /// Description : Added GetInquiryDetailsByProfileId function.
    /// Modified by : Sajal Gupta on 30-12-2016
    /// Desc Added GetUsedBikeInCityCount
    /// Modified by :Subodh jain  on 2 Jan 2017
    /// Description : Added GetUsedBikeByModelCountInCity function
    /// </summary>
    public interface IUsedBikeDetailsCacheRepository
    {
        ClassifiedInquiryDetails GetProfileDetails(uint inquiryId);
        IEnumerable<BikeDetailsMin> GetSimilarBikes(uint inquiryId, uint cityId, uint modelId, ushort topCount);
        IEnumerable<OtherUsedBikeDetails> GetOtherBikesByCityId(uint inquiryId, uint cityId, ushort topCount);
        InquiryDetails GetInquiryDetailsByProfileId(string profileId, string customerId);
        IEnumerable<OtherUsedBikeDetails> GetRecentUsedBikesInIndia(ushort topCount);
        IEnumerable<UsedBikesCountInCity> GetUsedBikeInCityCount(uint makeId);
        IEnumerable<MostRecentBikes> GetUsedBikeByModelCountInCity(uint makeid, uint cityid, uint topcount);
    }
}
