using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Location;

namespace Bikewale.Entities.Used
{
    /// <summary>
    /// Created By : Ashish G. Kamble
    /// Summary : Base entity for any used bike listing
    /// </summary>
    public class UsedBikeBase
    {
        public uint InquiryId { get; set; }
        public string ProfileId { set; get; }
        
        public uint AskingPrice { get; set; }
        public uint KmsDriven { get; set; }
        public string ModelYear { get; set; }
        public string ModelMonth { get; set; }
        public ushort NoOfOwners { get; set; }
        public ushort SellerType { get; set; }
        
        public BikePhoto Photo { get; set; }
        public ushort TotalPhotos { get; set; }

        public string CityName { get; set; }
        public string CityMaskingName { get; set; }

        public string MakeName { get; set; }        
        public string ModelName { get; set; }        
        public string MakeMaskingName { get; set; }        
        public string ModelMaskingName { get; set; }
        public string VersionName { get; set; }        
    }
}
