using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bikewale.Entities.Dealer;

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
