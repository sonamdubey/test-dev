using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.Dealer
{
    /// <summary>
    /// Created By : Sushil Kumar
    /// Created On : 21th October 2015
    /// </summary>
    public class ManufacturerLeadEntity  
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public uint VersionId { get; set; }
        public uint CityId { get; set; }
        public UInt32 DealerId { get; set; }
        public UInt32 PQId { get; set; }

    }
}
