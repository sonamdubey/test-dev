using Bikewale.Entities.Location;

namespace Bikewale.Models
{
    /// <summary>
    /// Created by  :   Sumit Kate on 27 Mar 2017
    /// Description :   Model to handle user location for url and cookie city mismatch
    /// </summary>
    public class ChangeLocationPopup
    {
        public GlobalCityAreaEntity UrlLocation { get; private set; }

        /// <summary>
        /// Created by  :   Sumit Kate on 27 Mar 2017
        /// Description :   Constructor to intialize UrlLocation
        /// </summary>
        /// <param name="urlLocation"></param>
        public ChangeLocationPopup(GlobalCityAreaEntity urlLocation)
        {
            UrlLocation = urlLocation;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 27 Mar 2017
        /// Description :   Processes url Location and returns the View Model for rendering
        /// </summary>
        /// <returns></returns>
        public ChangeLocationPopupVM GetData()
        {
            GlobalCityAreaEntity objLocation = Bikewale.Utility.GlobalCityArea.GetGlobalCityArea();
            ChangeLocationPopupVM objVM = new ChangeLocationPopupVM();
            objVM.IsLocationChanged = (UrlLocation.CityId > 0 && UrlLocation.CityId != objLocation.CityId);
            objVM.CurrentLocation = objLocation;
            objVM.UrlLocation = UrlLocation;

            return objVM;
        }
    }
}