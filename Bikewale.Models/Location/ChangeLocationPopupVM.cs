using Bikewale.Entities.Location;

namespace Bikewale.Models
{
    /// <summary>
    /// Created by  :   Sumit Kate on 2017
    /// Description :   ChangeLocation Popup VM
    /// </summary>
    public class ChangeLocationPopupVM
    {
        public bool IsLocationChanged { get; set; }
        public GlobalCityAreaEntity CurrentLocation { get; set; }
        public GlobalCityAreaEntity UrlLocation { get; set; }
    }
}
