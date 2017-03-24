using Bikewale.Entities;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;

namespace Bikewale.Models
{
    /// <summary>
    /// Created by: Sangram Nandkhile on 24-Mar-2017
    /// Summary:  Model for homepage
    /// </summary>
    public class HomePageModel
    {
        #region Variables for dependency injection
        private readonly IBikeMakes<BikeMakeEntity, int> _bikeMakes = null;
        #endregion

        #region Page level variables
        public ushort TopCount { get; private set; }
        public string redirectUrl;
        public StatusCodes status;
        #endregion

        public HomePageModel(ushort topCount, IBikeMakes<BikeMakeEntity, int> bikeMakes)
        {
            TopCount = topCount;
            _bikeMakes = bikeMakes;
        }


        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <returns>
        /// Created by : Sangram Nandkhile on 24-Mar-2017 
        /// </returns>
        public HomePageVM GetData()
        {
            HomePageVM objVM = new HomePageVM();
            objVM.AdTags = new AdTags();
            objVM.PageMetaTags = new PageMetaTags();
            objVM.Brands = new BrandWidgetModel(TopCount, _bikeMakes).GetData(Entities.BikeData.EnumBikeType.New);
            return objVM;
        }
    }
}