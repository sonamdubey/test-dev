﻿using Bikewale.Entities.BikeData;
using Bikewale.Entities.BikeData.NewLaunched;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.BikeData.NewLaunched;
using Bikewale.Interfaces.BikeData.UpComing;
using Bikewale.Interfaces.CMS;
using Bikewale.Utility;
using System;
using System.Collections.Generic;

namespace Bikewale.Models
{
    /// <summary>
    /// Created by  :   Sumit Kate on 30 mar 2017
    /// Description :   NewLaunchedIndex page Model
    /// </summary>
    public class NewLaunchedIndexModel
    {
        private readonly INewBikeLaunchesBL _newLaunches = null;
        private readonly IBikeMakesCacheRepository _objMakeCache = null;
        private readonly IUpcoming _upcoming = null;
        private readonly InputFilter _filter = null;
        private readonly PQSourceEnum _pqSource;
        private readonly ushort? _pageNumber;
        private uint _totalPagesCount;
        private readonly ICMSCacheContent _objArticles = null;
        public int PageSize { get; set; }
        public string BaseUrl { get; set; }
        public ushort MakeTopCount { get; set; }
        public NewLaunchedIndexModel(INewBikeLaunchesBL newLaunches, IBikeMakesCacheRepository objMakeCache, IUpcoming upcoming, InputFilter filter, PQSourceEnum pqSource, ushort? pageNumber, ICMSCacheContent objArticles)
        {
            _newLaunches = newLaunches;
            _objMakeCache = objMakeCache;
            _upcoming = upcoming;
            _filter = filter;
            _pageNumber = pageNumber;
            _pqSource = pqSource;
            _objArticles = objArticles;
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

                _totalPagesCount = (uint)(objVM.NewLaunched.Bikes.TotalCount / PageSize);

                if ((objVM.NewLaunched.Bikes.TotalCount % PageSize) > 0)
                    _totalPagesCount += 1;

                BindUpcoming(objVM);
                if (objVM.NewLaunched != null)
                {
                    CreatePager(objVM.NewLaunched, objVM.PageMetaTags);
                }
                CreateMeta(objVM.PageMetaTags);
                BindCMSContent(objVM);
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
                int pages = (int)_totalPagesCount;
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
        private void CreateMeta(PageMetaTags objMeta)
        {
            try
            {
                objMeta.Description = string.Format("Check out the latest bikes in India. Explore the bikes launched in {0}. Know more about prices, mileage,colors, specifications, and dealers of new bikes launches in {0}.", DateTime.Today.Year);
                objMeta.Title = string.Format("New Bike Launches in {0} | Latest Bikes in India - BikeWale", DateTime.Today.Year);

                if (_pageNumber > 1)
                {
                    objMeta.Description = string.Format("Page {0} of {1} - {2}", _pageNumber, _totalPagesCount, objMeta.Description);
                    objMeta.Title = string.Format("Page {0} of {1} - {2}", _pageNumber, _totalPagesCount, objMeta.Title);
                }

                objMeta.Keywords = string.Format("new bikes {0}, new bike launches in {1}, just launched bikes, new bike arrivals, bikes just got launched", DateTime.Today.AddDays(-1).Year, DateTime.Today.Year);
                objMeta.CanonicalUrl = string.Format("{0}/new-bike-launches/", Bikewale.Utility.BWConfiguration.Instance.BwHostUrlForJs);
                objMeta.AlternateUrl = string.Format("{0}/m/new-bike-launches/", Bikewale.Utility.BWConfiguration.Instance.BwHostUrlForJs);
            }
            catch (Exception ex)
            {
                Notifications.ErrorClass objErr = new Notifications.ErrorClass(ex, "NewLaunchedIndexModel.CreateMeta()");
            }
        }

        /// <summary>
        /// Created By :Snehal Dange on 30th Oct 2017
        /// Description: New launched page news widget
        /// </summary>
        /// <param name="objUpcoming"></param>
        private void BindCMSContent(NewLaunchedIndexVM objNewLaunches)
        {
            try
            {
                if (objNewLaunches != null)
                {

                    IList<int> modelIdList = new List<int>();
                    foreach (var obj in objNewLaunches.NewLaunched.Bikes.Bikes)
                    {
                        modelIdList.Add(obj.Model.ModelId);
                    }
                    string modelId = string.Join(",", modelIdList);
                    objNewLaunches.News = new RecentNews(5, 0, modelId, _objArticles).GetData();
                }
            }
            catch (Exception ex)
            {
               Notifications.ErrorClass objErr = new Notifications.ErrorClass(ex, "NewLaunchedIndexModel.BindCMSContent()");
            }

        }
    }
}