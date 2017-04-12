using Bikewale.Entities.BikeData;
using Bikewale.Entities.BikeData.NewLaunched;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.BikeData.NewLaunched;
using Bikewale.Interfaces.BikeData.UpComing;
using Bikewale.Models;
using Bikewale.Utility;
using System;

namespace Bikewale.Models
{
    /// <summary>
    /// Created by  :   Sumit Kate on 30 mar 2017
    /// Description :   NewLaunchedIndex page Model
    /// </summary>
    public class NewLaunchedIndexModel
    {
        private readonly INewBikeLaunchesBL _newLaunches = null;
        private readonly IBikeMakesCacheRepository<int> _objMakeCache = null;
        private readonly IUpcoming _upcoming = null;
        private readonly InputFilter _filter = null;
        private readonly PQSourceEnum _pqSource;
        private readonly ushort? _pageNumber;
        public int PageSize { get; set; }
        public string BaseUrl { get; set; }
        public ushort MakeTopCount { get; set; }
        public NewLaunchedIndexModel(INewBikeLaunchesBL newLaunches, IBikeMakesCacheRepository<int> objMakeCache, IUpcoming upcoming, InputFilter filter, PQSourceEnum pqSource, ushort? pageNumber)
        {
            _newLaunches = newLaunches;
            _objMakeCache = objMakeCache;
            _upcoming = upcoming;
            _filter = filter;
            _pageNumber = pageNumber;
            _pqSource = pqSource;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 30 Mar 2017
        /// Description :   Returns theView model for New launch landing page
        /// </summary>
        /// <returns></returns>
        public NewLaunchedIndexVM GetData()
        {
            NewLaunchedIndexVM objVM = null;
            try
            {
                objVM = new NewLaunchedIndexVM();
                objVM.Page_H1 = string.Format("NEW BIKE LAUNCHES - {0}", DateTime.Today.Year);

                objVM.Brands = (new BrandWidgetModel(MakeTopCount, _newLaunches)).GetData(EnumBikeType.NewLaunched);
                objVM.NewLaunched = (new NewLaunchesBikesModel(_newLaunches, _filter, _pqSource)).GetData();
                BindUpcoming(objVM);
                if (objVM.NewLaunched != null)
                {
                    CreatePager(objVM.NewLaunched, objVM.PageMetaTags);
                }
                CreateMeta(objVM);
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "NewLaunchedIndexModel.GetData()");
            }
            return objVM;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 30 Mar 2017
        /// Description :   Binds upcoming model
        /// </summary>
        /// <param name="objVM"></param>
        private void BindUpcoming(NewLaunchedIndexVM objVM)
        {
            try
            {
                UpcomingBikesWidget upcomingModel = new UpcomingBikesWidget(_upcoming);
                upcomingModel.SortBy = EnumUpcomingBikesFilter.Default;
                upcomingModel.Filters = new UpcomingBikesListInputEntity() { PageNo = 1, PageSize = 9 };
                objVM.Upcoming = upcomingModel.GetData();
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "NewLaunchedIndexModel.BindUpcoming()");
            }
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 30 Mar 2017
        /// Description :   Binds Pager
        /// </summary>
        /// <param name="newLaunchesBikesVM"></param>
        /// <param name="objMeta"></param>
        private void CreatePager(NewLaunchesBikesVM newLaunchesBikesVM, PageMetaTags objMeta)
        {
            try
            {
                newLaunchesBikesVM.Pager = new Entities.Pager.PagerEntity()
                    {
                        PageNo = (int)(_pageNumber.HasValue ? _pageNumber : 1),
                        PageSize = PageSize,
                        PagerSlotSize = 5,
                        BaseUrl = BaseUrl,
                        PageUrlType = "page/",
                        TotalResults = (int)(newLaunchesBikesVM.Bikes != null ? newLaunchesBikesVM.Bikes.TotalCount : 0)
                    };
                int pages = (int)(newLaunchesBikesVM.Bikes.TotalCount / PageSize);

                if ((newLaunchesBikesVM.Bikes.TotalCount % PageSize) > 0)
                    pages += 1;
                string prevUrl = string.Empty, nextUrl = string.Empty;
                Paging.CreatePrevNextUrl(pages, BaseUrl, (int)newLaunchesBikesVM.Pager.PageNo, ref nextUrl, ref prevUrl);
                objMeta.NextPageUrl = nextUrl;
                objMeta.PreviousPageUrl = prevUrl;
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "NewLaunchedIndexModel.CreatePager()");
            }
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 30 Mar 2017
        /// Description :   Creates meta for new launches landing page
        /// </summary>
        /// <param name="objVM"></param>
        private void CreateMeta(NewLaunchedIndexVM objVM)
        {
            try
            {
                objVM.PageMetaTags.Description = string.Format("Check out the latest bikes in India. Explore the bikes launched in {0}. Know more about prices, mileage,colors, specifications, and dealers of new bikes launches in {0}.", DateTime.Today.Year);
                objVM.PageMetaTags.Title = string.Format("New Bike Launches in {0} | Latest Bikes in India - BikeWale", DateTime.Today.Year);
                objVM.PageMetaTags.Keywords = string.Format("new bikes {0}, new bike launches in {1}, just launched bikes, new bike arrivals, bikes just got launched", DateTime.Today.AddDays(-1).Year, DateTime.Today.Year);
                objVM.PageMetaTags.CanonicalUrl = string.Format("{0}/new-bike-launches/", Bikewale.Utility.BWConfiguration.Instance.BwHostUrlForJs);
                objVM.PageMetaTags.AlternateUrl = string.Format("{0}/m/new-bike-launches/", Bikewale.Utility.BWConfiguration.Instance.BwHostUrlForJs);
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "NewLaunchedIndexModel.CreateMeta()");
            }
        }

    }
}