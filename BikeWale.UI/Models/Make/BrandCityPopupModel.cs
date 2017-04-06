
using Bikewale.Entities.BikeData;
using System.Web;
namespace Bikewale.Models.Make
{
    /// <summary>
    /// Created by: Sangram Nandkhile on 27-Mar-2017
    /// Brand and city change popup for example. Dealers in city page.
    /// </summary>
    public class BrandCityPopupModel
    {
        private readonly EnumBikeType _pageType;
        private readonly uint _makeId, _cityId;
        public BrandCityPopupModel(EnumBikeType PageType, uint makeId, uint cityId)
        {
            _pageType = PageType;
            _makeId = makeId;
            _cityId = cityId;
        }

        public BrandCityPopupVM GetData()
        {
            BrandCityPopupVM objData = new BrandCityPopupVM();
            objData.PageType = _pageType;
            objData.CityId = _cityId;
            objData.MakeId = _makeId;
            DetectOperaBrowser(objData);
            return objData;
        }

        private void DetectOperaBrowser(BrandCityPopupVM objData)
        {
            System.Web.HttpBrowserCapabilities browserDetection = HttpContext.Current.Request.Browser;
            if (browserDetection != null)
            {
                string browserType = browserDetection.Type;

                if (!string.IsNullOrEmpty(browserType) && browserType.ToLower().Contains("mini"))
                    objData.IsOperaBrowser = true;
            }
        }
    }
}