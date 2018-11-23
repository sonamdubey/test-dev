

using Bikewale.Entities.PriceQuote;
using Bikewale.Entities.Dealer;
using System.Collections.Specialized;
using Bikewale.Entities.MaskingNumber;
namespace Bikewale.Interfaces.Lead
{
    /// <summary>
    /// Created by : Snehal Dange on 2nd May 2018
    /// Description : To manage dealer and manufacture lead related methods
    /// Modified by : Kartik Rathod on 19 nov 2019
    /// Desc        : Added ProcessMaskingLead
    /// </summary>
    public interface ILead
    {
        uint ProcessESLead(ManufacturerLeadEntity input, NameValueCollection headers);

        PQCustomerDetailOutputEntity ProcessPQCustomerDetailInputWithPQ(Entities.PriceQuote.PQCustomerDetailInput pqInput, System.Collections.Specialized.NameValueCollection requestHeaders);

        PQCustomerDetailOutputEntity ProcessPQCustomerDetailInputWithoutPQ(PQCustomerDetailInput pqInput, System.Collections.Specialized.NameValueCollection requestHeaders);

        Bikewale.Entities.PriceQuote.v2.PQCustomerDetailOutputEntity ProcessPQCustomerDetailInputWithPQV2(Bikewale.Entities.PriceQuote.v2.PQCustomerDetailInput pqInput, NameValueCollection requestHeaders);

        uint ProcessMaskingLead(MaskingNumberLeadEntity objLead);
        Bikewale.Entities.PriceQuote.v2.PQCustomerDetailOutputEntity ProcessPQCustomerDetailInputWithoutPQV2(Bikewale.Entities.PriceQuote.v2.PQCustomerDetailInput pqInput, System.Collections.Specialized.NameValueCollection requestHeaders);
    }
}
