using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Interfaces.Dealer
{
    /// <summary>
    /// Auth: Sangram Nandkhile
    /// Created on: 06 Jan 2016
    /// Desc: Interface for App Alert
    /// </summary>
    public interface ITerms
    {
        bool SaveImeiGcmData(string imei, string gcmId, string osType, string subsMasterId);
    }
}
