
using Bikewale.Entities;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Pages;
using Bikewale.Entities.Schema;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.BikeData.NewLaunched;
using Bikewale.Interfaces.BikeData.UpComing;
using Bikewale.Interfaces.Dealer;
using Bikewale.Utility;
using System;
using System.Collections.Generic;
namespace Bikewale.Models
{
    public class DealerShowroomIndexPage
    {


        private readonly INewBikeLaunchesBL _newLaunches = null;
        private readonly IUpcoming _upcoming = null;
        private readonly IBikeMakesCacheRepository _bikeMakes = null;
        private readonly IBikeModelsCacheRepository<int> _objBestBikes = null;
        protected string makeMaskingName = string.Empty, redirectUrl = string.Empty;
        public uint MakeId { get; set; }
        public ushort TopCount { get; set; }
        public StatusCodes status { get; set; }
        public bool IsMobile { get; set; }

        public DealerShowroomIndexPage(IBikeModelsCacheRepository<int> objBestBikes, IDealerCacheRepository objDealerCache, IBikeMakesCacheRepository bikeMakes, IUpcoming upcoming, INewBikeLaunchesBL newLaunches, ushort topCount)
        {
            _newLaunches = newLaunches;
            _upcoming = upcoming;
            _bikeMakes = bikeMakes;
            _objBestBikes = objBestBikes;
            TopCount = topCount;
        }
        /// <summary>
        /// Created By :- Subodh Jain 23 March 2017
        /// Summary :- Dealer Locator view model data fetching
        /// </summary>
        /// <returns></returns>
        public IndexVM GetData()
        {
            IndexVM objDealerVM = new IndexVM();
            try
            {

                objDealerVM.MakesList = BindMakeList();
                objDealerVM.objUpcomingBikes = BindUpCompingBikesWidget();
                objDealerVM.BestBikes = new BestBikeWidgetModel(null, _objBestBikes).GetData();
                objDealerVM.NewLaunchedBikes = BindNewLaunchesBikes();
                objDealerVM.Brands = new BrandWidgetModel(TopCount, _bikeMakes).GetData(Entities.BikeData.EnumBikeType.Dealer);
                BindPageMetas(objDealerVM.PageMetaTags);
                objDealerVM.Page = GAPages.Dealer_Locator_Page;
                SetBreadcrumList(objDealerVM);
                SetPageJSONLDSchema(objDealerVM);
            }
            catch (Exception ex)
            {

                Bikewale.Notifications.ErrorClass.LogError(ex, "DealerShowroomIndexPage.GetData()");
            }
            return objDealerVM;
        }


        /// <summary>
        /// Created By :- Subodh Jain 29 March 2017
        /// Summary :- Bind MakeList
        /// </summary>
        /// <returns></returns>
        private IEnumerable<BikeMakeEntityBase> BindMakeList()
        {
            IEnumerable<BikeMakeEntityBase> _makes;
            _makes = _bikeMakes.GetMakesByType(EnumBikeType.Dealer);
            return _makes;
        }

        /// <summary>
        /// Created By:- Subodh Jain 23 March 2017
        /// Summary:- Fetching data about dealers of other brands
        /// </summary>
        /// <returns></returns>
        private void BindPageMetas(PageMetaTags objPage)
        {

            try
            {
                objPage.Title = string.Format("New Bike Showroom in India | Find Authorized Bike Dealers - BikeWale");
                objPage.Keywords = string.Format("new bike dealers, new bike showrooms, bike dealers, bike showrooms, showrooms, dealerships");
                objPage.Description = string.Format("Locate new bike showrooms and authorized bike dealers in India. Find new bike dealer information for more than 200 cities in India.");

            }
            catch (Exception ex)
            {

                Bikewale.Notifications.ErrorClass.LogError(ex, "DealerShowroomIndiaPage.BindPageMetas()");
            }
        }
        /// <summary>
        /// Created By:- Subodh Jain 23 March 2017
        /// Summary:- Fetching data about New Bike Launches
        /// </summary>
        /// <returns></returns>
        private NewLaunchedWidgetVM BindNewLaunchesBikes()
        {
            NewLaunchedWidgetVM NewLaunchedbikes = null;
            try
            {
                NewLaunchedWidgetModel objNewLaunched = new NewLaunchedWidgetModel(9, _newLaunches);
                NewLaunchedbikes = objNewLaunched.GetData();
            }
            catch (Exception ex)
            {

                Bikewale.Notifications.ErrorClass.LogError(ex, "DealerShowroomIndiaPage.BindNewLaunchesBikes()");
            }
            return NewLaunchedbikes;

        }

        /// <summary>
        /// Created By:- Subodh Jain 23 March 2017
        /// Summary:- Binding data for upcoming bike widget
        /// </summary>
        /// <returns></returns>
        private UpcomingBikesWidgetVM BindUpCompingBikesWidget()
        {
            UpcomingBikesWidgetVM objUpcomingBikes = null;
            try
            {
                UpcomingBikesWidget objUpcoming = new UpcomingBikesWidget(_upcoming);

                objUpcoming.Filters = new Bikewale.Entities.BikeData.UpcomingBikesListInputEntity()
                {
                    PageSize = 9,
                    PageNo = 1
                };
                objUpcoming.SortBy = Bikewale.Entities.BikeData.EnumUpcomingBikesFilter.Default;
                objUpcomingBikes = objUpcoming.GetData();
            }
            catch (Exception ex)
            {

                Bikewale.Notifications.ErrorClass.LogError(ex, "DealerShowroomIndiaPage.BindUpCompingBikesWidget()");
            }
            return objUpcomingBikes;
        }

        /// <summary>
        /// Created by : Snehal Dange on 12th Jan 2017
        /// Desc : Added breadcrum and webpage schema
        /// </summary>
        /// <param name="objPageMeta"></param>
        private void SetPageJSONLDSchema(IndexVM objPageMeta)
        {
            //set webpage schema 
            WebPage webpage = SchemaHelper.GetWebpageSchema(objPageMeta.PageMetaTags, objPageMeta.BreadcrumbList);

            if (webpage != null)
            {
                objPageMeta.PageMetaTags.SchemaJSON = SchemaHelper.JsonSerialize(webpage);
            }
        }

        /// <summary>
        /// Created by : Snehal Dange on 12th Dec 2017
        /// Desc : Added breadcrumb for dealerpage
        /// </summary>
        /// <param name="objData"></param>
        private void SetBreadcrumList(IndexVM objData)
        {
            try
            {
                IList<BreadcrumbListItem> BreadCrumbs = new List<BreadcrumbListItem>();
                string url = string.Format("{0}/", Utility.BWConfiguration.Instance.BwHostUrl);
                ushort position = 1;
                if (IsMobile)
                {
                    url += "m/";
                }

                BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, url, "Home"));
                url = string.Format("{0}new-bikes-in-india/", url);
                BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, url, "New Bikes"));
                BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position, null, "Dealer Locator"));


                objData.BreadcrumbList.BreadcrumListItem = BreadCrumbs;
            }
            catch (Exception ex)
            {

                Bikewale.Notifications.ErrorClass.LogError(ex, "DealerShowroomIndiaPage.SetBreadcrumList()");
            }

        }
    }
}