using System.Collections.Generic;

namespace Bikewale.Interfaces.MobileVerification
{
    /// <summary>
    /// Created by : Aditi Srivastava on 14 Feb 2017
    /// Summary    : Interface for cache layer
    /// </summary>
    public interface IMobileVerificationCache
    {
        IEnumerable<string> GetBlockedNumbers();
    }
}
