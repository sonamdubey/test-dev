using Bikewale.Entities.Used;
using Bikewale.Entities.UsedBikes;
using System.Collections.Generic;

namespace Bikewale.Interfaces.Used
{
    /// <summary>
    /// Created By : Sushil Kumar on 29th August 2016
    /// Description : Used Bikes details interface for used bikes section
    /// Modified by : Sajal Gupta on 07/10/2016
    /// Description : Added GetInquiryDetailsByProfileId function.
    /// Modified by :   Sumit Kate on 02 Nov 2016
    /// Description :   Added method to get bike photos
    /// Modified by :Subodh jain  on 2 Jan 2017
    /// Description : Added GetUsedBikeByModelCountInCity function
    /// Modified by Sajal Gupta on 30-12-2016
    /// Desc : Added GetUsedBikeInCityCount method;
    /// Modified by :Subodh jain  on 2 Jan 2017
    /// Description : Added GetUsedBikeCountInCity function
    /// Modified by Sajal Gupta on 03-01-2016
    /// Desc : Added GetUsedBikeInCityCountByModel method;
    /// Modified by Sangram Nandkhile on 03-02-2016
    /// Desc : Added GetPopularUsedModelsByMake method;
    /// Modified By :-Subodh Jain on 15 March 2017
    /// Summary :-Added used GetUsedBike
    /// </summary>
    public interface IUsedBikeDetails
    {
        ClassifiedInquiryDetails GetProfileDetails(uint inquiryId);
        IEnumerable<BikeDetailsMin> GetSimilarBikes(uint inquiryId, uint cityId, uint modelId, ushort topCount);
        IEnumerable<OtherUsedBikeDetails> GetOtherBikesByCityId(uint inquiryId, uint cityId, ushort topCount);
        InquiryDetails GetInquiryDetailsByProfileId(string profileId, string customerId);
        IEnumerable<OtherUsedBikeDetails> GetRecentUsedBikesInIndia(ushort topCount);
        IEnumerable<BikePhoto> GetBikePhotos(uint inquiryId, bool isApproved);
        IEnumerable<MostRecentBikes> GetUsedBikeByModelCountInCity(uint makeid, uint cityid, uint topcount);
        IEnumerable<MostRecentBikes> GetUsedBikeCountInCity(uint cityid, uint topcount);
        IEnumerable<UsedBikesCountInCity> GetUsedBikeInCityCountByModel(uint modelId, ushort topCount);
        IEnumerable<UsedBikesCountInCity> GetUsedBikeInCityCountByMake(uint makeId, ushort topCount);
        IEnumerable<MostRecentBikes> GetPopularUsedModelsByMake(uint makeid, uint topcount);
        IEnumerable<MostRecentBikes> GetUsedBike(uint topcount);
    }
}
