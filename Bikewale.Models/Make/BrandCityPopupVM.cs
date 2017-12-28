using Bikewale.Entities.BikeData;
namespace Bikewale.Models
{
    /// <author>
    /// Create by: Sangram Nandkhile on 27-Mar-2017
    /// Summary:  View Model for brand city popup 
    /// </author>
    public class BrandCityPopupVM
    {

        public EnumBikeType PageType { get; set; }
        public bool IsOperaBrowser { get; set; }
        public uint CityId { get; set; }
        public uint MakeId { get; set; }
    }
}
