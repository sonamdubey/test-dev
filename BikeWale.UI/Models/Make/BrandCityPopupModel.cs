
using Bikewale.Entities.BikeData;
namespace Bikewale.Models.Make
{
    /// <summary>
    /// Created by: Sangram Nandkhile on 27-Mar-2017
    /// Brand and city change popup for example. Dealers in city page.
    /// </summary>
    public class BrandCityPopupModel
    {
        private readonly EnumBikeType _pageType;
        public BrandCityPopupModel(EnumBikeType PageType)
        {
            _pageType = PageType;
        }

        public BrandCityPopupVM GetData()
        {
            BrandCityPopupVM objData = new BrandCityPopupVM();
            objData.PageType = _pageType;
            return objData;
        }
    }
}