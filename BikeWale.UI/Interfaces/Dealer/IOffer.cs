using Bikewale.Entities.Dealer;
using System.Collections.Generic;

namespace Bikewale.Interfaces.Dealer
{
    /// <summary>
    /// Created By : Sangram Nandkhile on 3rd Feb 2016
    /// </summary>
    /// 
    public interface IOffer
    {
        List<Offer> GetOffersByDealerId(uint dealerId, uint modelId);
    }
}
