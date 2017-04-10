using Bikewale.Entities.BikeData;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.BikeData.NewLaunched;
using Bikewale.Interfaces.BikeData.UpComing;
using Bikewale.Notifications;
using System;

namespace Bikewale.Models
{
    /// <summary>
    /// Created by: Sangram Nandkhile on 07-Apr-2017
    /// summary: Upcoming bikes page model
    /// </summary>
    public class UpcomingPageModel
    {
        #region Private variables

        private IUpcoming _upcoming = null;
        private readonly INewBikeLaunchesBL _newLaunches = null;
        private uint _topbrandCount;
        private EnumUpcomingBikesFilter filter = EnumUpcomingBikesFilter.Default;

        #endregion

        #region Constructor

        public UpcomingPageModel(uint topbrandCount, IUpcoming upcoming, INewBikeLaunchesBL newLaunches)
        {
            _upcoming = upcoming;
            _topbrandCount = topbrandCount;
            _newLaunches = newLaunches;
        }
        #endregion

        #region Public members

        public UpcomingBikesListInputEntity Filters { get; set; }
        public EnumUpcomingBikesFilter SortBy { get; set; }

        #endregion

        #region Functions

        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <returns>
        /// Created by : Sangram Nandkhile on 07-Apr-2017 
        /// </returns>
        public UpcomingPageVM GetData()
        {
            UpcomingPageVM objUpcoming = new UpcomingPageVM();
            try
            {
                BindPageMetaTags(objUpcoming.PageMetaTags);
                var upcomingBikes = _upcoming.GetModels(Filters, SortBy);
                objUpcoming.Brands = _upcoming.BindUpcomingMakes(_topbrandCount);
                objUpcoming.NewLaunches = new NewLaunchedWidgetModel(9, _newLaunches).GetData();
                //objUpcoming.NewLaunches.PageCatId = 1;
                objUpcoming.NewLaunches.PQSourceId = (uint)PQSourceEnum.Desktop_UpcomiingBikes_NewLaunchesWidget;
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Models.UpcomingPageModel.GetData");
            }
            return objUpcoming;
        }

        /// <summary>
        /// Binds the page meta tags.
        /// </summary>
        /// <param name="pageMetaTags">The page meta tags.</param>
        private void BindPageMetaTags(PageMetaTags pageMetaTags)
        {
            try
            {
                int currentYear = DateTime.Now.Year;
                string nextYear = DateTime.Now.AddYears(1).Year.ToString().Substring(2);
                string year = string.Format("{0}-{1}", currentYear, nextYear);
                pageMetaTags.CanonicalUrl = "https://www.bikewale.com/upcoming-bikes/";
                pageMetaTags.AlternateUrl = "https://www.bikewale.com/m/upcoming-bikes/";
                pageMetaTags.Keywords = "Upcoming bikes, expected launch, new bikes, upcoming scooter, upcoming, to be released bikes, bikes to be launched";
                pageMetaTags.Description = string.Format("Find a list of upcoming bikes in India in {0}. Get details on expected launch date, prices for bikes expected to launch in {1}.", year, currentYear);
                pageMetaTags.Title = string.Format("Upcoming Bikes in India | Expected Launches in {0} - BikeWale", currentYear);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Models.UpcomingPageModel.BindPageMetaTags");
            }
        }

        #endregion
    }
}
