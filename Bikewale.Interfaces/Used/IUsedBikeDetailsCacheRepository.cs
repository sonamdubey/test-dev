using Bikewale.Entities.Used;
using System.Collections.Generic;

namespace Bikewale.Interfaces.Used
{
    /// <summary>
    /// Created By : Sushil Kumar on 27th August 2016
    /// Description : Cache Interface for used bikes
    /// </summary>
    public interface IUsedBikeDetailsCacheRepository
    {
        ClassifiedInquiryDetails GetProfileDetails(uint inquiryId);
        IEnumerable<BikeDetailsMin> GetSimilarBikes(uint inquiryId, uint cityId, uint modelId);
        IEnumerable<BikeDetailsMin> GetBikesByCityId(uint inquiryId, uint cityId);
    }
}
