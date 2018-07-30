using Bikewale.Entities.BikeData;
using Bikewale.Entities.Used;
using Bikewale.Entities.UsedBikes;
using System.Collections.Generic;

namespace Bikewale.Interfaces.UsedBikes
{
    /// <summary>
    /// Created By Sajal Gupta On 14/09/2016
    /// Description : Function to getBikes based on model/city/make.
    /// Modified by : Sajal Gupta on 07/10/2016
    /// Description : Added GetInquiryDetailsByProfileId function.
    /// </summary>
    public interface IUsedBikes
    {
        IEnumerable<MostRecentBikes> GetPopularUsedBikes(uint makeId, uint modelId, uint cityId, uint totalCount);
        InquiryDetails GetInquiryDetailsByProfileId(string profileId, string customerId, string platformId);
        IEnumerable<UsedBikeMakeEntity> GetUsedBikeMakesWithCount();
        IEnumerable<MostRecentBikes> GetUsedBikesSeries(uint seriesid, uint cityId);
    }
}
