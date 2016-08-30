using Bikewale.Entities.Used;
using System.Collections.Generic;

namespace Bikewale.Interfaces.Used
{
    /// <summary>
    /// Created By : Sushil Kumar on 29th August 2016
    /// Description : Used Bikes details interface for used bikes section
    /// </summary>
    public interface IUsedBikeDetails
    {
        ClassifiedInquiryDetails GetProfileDetails(uint inquiryId);
        IEnumerable<BikeDetailsMin> GetSimilarBikes(uint inquiryId, uint cityId, uint modelId, ushort topCount);
        IEnumerable<BikeDetailsMin> GetBikesByCityId(uint inquiryId, uint cityId);
    }
}
