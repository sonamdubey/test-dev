
using Bikewale.Entities.Dealer;
using System.Collections.Specialized;
namespace Bikewale.Interfaces.Lead
{
    /// <summary>
    /// Created by : Snehal Dange on 2nd May 2018
    /// Description : To manage dealer and manufacture lead related methods
    /// </summary>
    public interface ILead
    {
        uint ProcessESLead(ManufacturerLeadEntity input, NameValueCollection headers);
    }
}
