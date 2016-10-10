﻿using Bikewale.Entities.Used;
using System.Collections.Generic;

namespace Bikewale.Interfaces.Used
{
    /// <summary>
    /// Created By : Sushil Kumar on 29th August 2016
    /// Description : Used Bikes details interface for used bikes section
    /// Modified by : Sajal Gupta on 07/10/2016
    /// Description : Added GetInquiryDetailsByProfileId function.
    /// </summary>
    public interface IUsedBikeDetails
    {
        ClassifiedInquiryDetails GetProfileDetails(uint inquiryId);
        IEnumerable<BikeDetailsMin> GetSimilarBikes(uint inquiryId, uint cityId, uint modelId, ushort topCount);
        IEnumerable<OtherUsedBikeDetails> GetOtherBikesByCityId(uint inquiryId, uint cityId, ushort topCount);
        InquiryDetails GetInquiryDetailsByProfileId(string profileId);
    }
}
