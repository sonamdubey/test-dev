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
    /// Desc Added GetRecentUsedBikesInIndia
    /// Modified by :Subodh jain  on 2 Jan 2017
    /// Description : Added GetUsedBikeByModelCountInCity function
    /// Modified by :Subodh jain  on 2 Jan 2017
    /// Description : Added GetUsedBikeCountInCity function
    /// Modified by : Sajal Gupta on 3 jan 2017
    /// Desc Added GetUsedBikeInCityCountByModel function
    /// Modified By :-Subodh Jain on 15 March 2017
    /// Summary :-Added used GetUsedBike
    /// </summary>
    public interface IUsedBikeDetailsCacheRepository
    {
        ClassifiedInquiryDetails GetProfileDetails(uint inquiryId);
        IEnumerable<BikeDetailsMin> GetSimilarBikes(uint inquiryId, uint cityId, uint modelId, ushort topCount);
        IEnumerable<OtherUsedBikeDetails> GetOtherBikesByCityId(uint inquiryId, uint cityId, ushort topCount);
        InquiryDetails GetInquiryDetailsByProfileId(string profileId, string customerId);
        IEnumerable<OtherUsedBikeDetails> GetRecentUsedBikesInIndia(ushort topCount);
        IEnumerable<MostRecentBikes> GetUsedBikeByModelCountInCity(uint makeid, uint cityid, uint topcount);
        IEnumerable<MostRecentBikes> GetUsedBikeCountInCity(uint cityid, uint topcount);
        IEnumerable<UsedBikesCountInCity> GetUsedBikeInCityCountByModel(uint modelId, ushort topCount);
        IEnumerable<UsedBikesCountInCity> GetUsedBikeInCityCountByMake(uint makeId, ushort topCount);
        IEnumerable<MostRecentBikes> GetPopularUsedModelsByMake(uint makeid, uint topcount);
        IEnumerable<MostRecentBikes> GetUsedBike(uint topcount);
    }
}
