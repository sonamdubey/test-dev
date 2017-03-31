using Bikewale.Common;
using Bikewale.Entities;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.BikeData.NewLaunched;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.BikeData.NewLaunched;
using Bikewale.Interfaces.BikeData.UpComing;
using Bikewale.Models;
using Bikewale.Utility;
using System;
using System.Linq;
using System.Web;
namespace Bikewale.Models
{
    /// <summary>
    /// Created by  :   Sumit Kate on 30 Mar 2017
    /// Description :   NewLaunched Make Page Model
    /// </summary>
    public class NewLaunchedMakePageModel
    {
        private readonly INewBikeLaunchesBL _newLaunches = null;
        private readonly IBikeMakesCacheRepository<int> _objMakeCache = null;
        private readonly IUpcoming _upcoming = null;
        private InputFilter _filter;
        private readonly PQSourceEnum _pqSource;
        private readonly ushort? _pageNumber;
        private readonly string _makeMaskingName;

        public int PageSize { get; set; }
        public string BaseUrl { get; set; }
        public ushort MakeTopCount { get; set; }
        public StatusCodes Status { get; private set; }
        public String RedirectUrl { get; private set; }
        public uint MakeId { get; private set; }

        /// <summary>
        /// Created by  :   Sumit Kate on 30 Mar 2017
        /// Description :   Constructor to intialize member variables
        /// </summary>
        /// <param name="makeMaskingName"></param>
        /// <param name="newLaunches"></param>
        /// <param name="objMakeCache"></param>
        /// <param name="upcoming"></param>
        /// <param name="pqSource"></param>
        /// <param name="pageNumber"></param>
        public NewLaunchedMakePageModel(string makeMaskingName, INewBikeLaunchesBL newLaunches, IBikeMakesCacheRepository<int> objMakeCache, IUpcoming upcoming, PQSourceEnum pqSource, ushort? pageNumber)
        {
            _newLaunches = newLaunches;
            _objMakeCache = objMakeCache;
            _upcoming = upcoming;
            _pageNumber = pageNumber;
            _pqSource = pqSource;
            _makeMaskingName = makeMaskingName;
            ProcessQueryString();
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 30 Mar 2017
        /// Description :   Returns View model for New launch make page
        /// </summary>
        /// <returns></returns>
        public NewLaunchedMakeVM GetData()
        {
            NewLaunchedMakeVM objVM = null;
            try
            {
                objVM = new NewLaunchedMakeVM();
                _filter = new InputFilter()
                {
                    PageNo = (int)(_pageNumber.HasValue ? _pageNumber : 1),
                    CityId = GlobalCityArea.GetGlobalCityArea().CityId,
                    Make = MakeId,
                    PageSize = PageSize
                };
                objVM.NewLaunched = (new NewLaunchesBikesModel(_newLaunches, _filter, _pqSource)).GetData();

                if (objVM.NewLaunched != null && objVM.NewLaunched.HasBikes)
                {
                    objVM.NewLaunched.Makes = null;
                    objVM.CityId = GlobalCityArea.GetGlobalCityArea().CityId;
                    objVM.Brands = (new BrandWidgetModel(MakeTopCount, _newLaunches)).GetData(EnumBikeType.NewLaunched);
                    objVM.Make = objVM.NewLaunched.Bikes.Bikes.FirstOrDefault().Make;
                    objVM.Page_H1 = String.Format("New {0} Bike Launches", objVM.Make.MakeName);
                    objVM.NewLaunched.Page_H2 = string.Format("Latest {0} bikes in India", objVM.Make.MakeName);
                    BindUpcoming(objVM);
                    if (objVM.NewLaunched != null)
                    {
                        CreatePager(objVM.NewLaunched, objVM.PageMetaTags);
                    }
                    CreateMeta(objVM);
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "NewLaunchedMakePageModel.GetData()");
            }
            return objVM;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 30 Mar 2017
        /// Description :   Binds upcoming model
        /// </summary>
        /// <param name="objVM"></param>
        private void BindUpcoming(NewLaunchedMakeVM objVM)
        {
            try
            {
                UpcomingBikesWidget upcomingModel = new UpcomingBikesWidget(_upcoming);
                upcomingModel.SortBy = EnumUpcomingBikesFilter.Default;
                upcomingModel.Filters = new UpcomingBikesListInputEntity() { StartIndex = 1, EndIndex = 9, MakeId = (int)MakeId };
                objVM.Upcoming = upcomingModel.GetData();
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "NewLaunchedMakePageModel.BindUpcoming()");
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
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "NewLaunchedMakePageModel.CreatePager()");
            }
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 30 Mar 2017
        /// Description :   Creates meta for new launches landing page
        /// </summary>
        /// <param name="objVM"></param>
        private void CreateMeta(NewLaunchedMakeVM objVM)
        {
            try
            {
                objVM.PageMetaTags.Description = string.Format("Check out the latest {0} bikes in India. Know more about prices, mileage, colors, specifications, and dealers of recently launched {0} bikes.", objVM.Make.MakeName.ToLower());
                objVM.PageMetaTags.Title = string.Format("{0} Bike Launches | Latest {0} Bikes in India- BikeWale", objVM.Make.MakeName);
                objVM.PageMetaTags.Keywords = string.Format("new {2} bikes {0}, new {2} bike launches in {1}, just launched {2} bikes, new {2} bike arrivals, {2} bikes just got launched", DateTime.Today.AddDays(-1).Year, DateTime.Today.Year, objVM.Make.MakeName.ToLower());
                objVM.PageMetaTags.CanonicalUrl = string.Format("{0}/new-{1}-bike-launches/", Bikewale.Utility.BWConfiguration.Instance.BwHostUrlForJs, objVM.Make.MaskingName);
                objVM.PageMetaTags.AlternateUrl = string.Format("{0}/m/new-{1}-bike-launches/", Bikewale.Utility.BWConfiguration.Instance.BwHostUrlForJs, objVM.Make.MaskingName);
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "NewLaunchedMakePageModel.CreateMeta()");
            }
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 30 Mar 2017
        /// Description :   ProcessQueryString to process make masking name
        /// </summary>
        private void ProcessQueryString()
        {
            String rawUrl = HttpContext.Current.Request.RawUrl;
            MakeMaskingResponse objMakeResponse = null;
            try
            {
                objMakeResponse = new MakeHelper().GetMakeByMaskingName(_makeMaskingName);
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, String.Format("NewLaunchedMakePageModel.ProcessQueryString({0})", _makeMaskingName));
                Status = StatusCodes.ContentNotFound;
            }
            finally
            {
                if (objMakeResponse != null)
                {
                    if (objMakeResponse.StatusCode == 200)
                    {
                        MakeId = objMakeResponse.MakeId;
                        Status = StatusCodes.ContentFound;
                    }
                    else if (objMakeResponse.StatusCode == 301)
                    {
                        rawUrl = rawUrl.Replace(_makeMaskingName, objMakeResponse.MaskingName);
                        Status = StatusCodes.RedirectPermanent;
                    }
                    else
                    {
                        Status = StatusCodes.ContentNotFound;
                    }
                    RedirectUrl = rawUrl;
                }
                else
                {
                    Status = StatusCodes.ContentNotFound;
                }
            }
        }
    }
}