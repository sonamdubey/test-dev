using Bikewale.Entities;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.CMS;
using Bikewale.Entities.EditorialWidgets;
using Bikewale.Entities.Location;
using Bikewale.Entities.Pager;
using Bikewale.Entities.Schema;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.BikeData.UpComing;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.Pager;
using Bikewale.Models.EditorialPages;
using Bikewale.Notifications;
using Bikewale.Utility;
using System;
using System.Collections.Generic;
using System.Web;

namespace Bikewale.Models
{
    /// <summary>
    /// Created by : Aditi Srivastava on 31 Mar 2017
    /// Summary    : Model to fetch data for Bike care listing page
    /// </summary>
    public class BikeCareIndexPage : EditorialBasePage
    {
        #region Variables for dependency injection
        private readonly ICMSCacheContent _cmsCache = null;
        private readonly IPager _pager;
        private readonly IUpcoming _upcoming = null;
        private readonly IBikeModels<BikeModelEntity, int> _bikeModels = null;
        private readonly IBikeMakesCacheRepository _bikeMakesCacheRepository = null;
        private readonly IBikeModelsCacheRepository<int> _models = null;
        private readonly IBikeSeries _series;
        #endregion

        #region Page level variables
        private const int pageSize = 10, pagerSlotSize = 5;
        private uint CityId;
        private int curPageNo = 1;
        private uint _totalPagesCount;
        public string redirectUrl;
        public StatusCodes status;
        private GlobalCityAreaEntity currentCityArea;
        #endregion

        #region Public members
        public bool IsMobile { get; set; }
        #endregion

        #region Constructor
        public BikeCareIndexPage(ICMSCacheContent cmsCache, IPager pager, IUpcoming upcoming, IBikeModels<BikeModelEntity, int> bikeModels, IBikeMakesCacheRepository bikeMakesCacheRepository, IBikeModelsCacheRepository<int> models, IBikeSeries series)
            : base(bikeMakesCacheRepository, models, bikeModels, upcoming, series)
        {
            _cmsCache = cmsCache;
            _pager = pager;
            _upcoming = upcoming;
            _bikeModels = bikeModels;
            _bikeMakesCacheRepository = bikeMakesCacheRepository;
            _models = models;
            _series = series;
            ProcessQueryString();
        }
        #endregion

        #region Functions

        /// <summary>
        /// Created by : Aditi Srivastava on 3 Apr 2017
        /// Summary    : Process query string
        /// </summary>
        private void ProcessQueryString()
        {
            var request = HttpContext.Current.Request;
            var queryString = request != null ? request.QueryString : null;

            if (queryString != null)
            {

                if (!string.IsNullOrEmpty(queryString["pn"]))
                {
                    string _pageNo = queryString["pn"];
                    if (!string.IsNullOrEmpty(_pageNo))
                    {
                        int.TryParse(_pageNo, out curPageNo);
                    }
                }
            }
            currentCityArea = GlobalCityArea.GetGlobalCityArea();
            if (currentCityArea != null)
                CityId = currentCityArea.CityId;
        }

        /// <summary>
        /// Created by : Aditi Srivastava on 31 Mar 2017
        /// Summary    : Populate page view model 
        /// Modified by : Snehal Dange on 29th Nov 2017
        /// Descritpion : Added ga for page
        /// </summary>
        public BikeCareIndexPageVM GetData(int widgetTopCount)
        {
            BikeCareIndexPageVM objData = new BikeCareIndexPageVM();
            try
            {
                int _startIndex = 0, _endIndex = 0;
                _pager.GetStartEndIndex(pageSize, curPageNo, out _startIndex, out _endIndex);

                objData.StartIndex = _startIndex;
                objData.EndIndex = _endIndex;

                IList<EnumCMSContentType> categorList = new List<EnumCMSContentType>();
                categorList.Add(EnumCMSContentType.TipsAndAdvices);
                string _contentType = CommonApiOpn.GetContentTypesString(categorList);


                objData.Articles = _cmsCache.GetArticlesByCategoryList(_contentType, _startIndex, _endIndex, 0, 0);

                _totalPagesCount = (uint)_pager.GetTotalPages((int)objData.Articles.RecordCount, pageSize);

                if (objData.Articles != null && objData.Articles.RecordCount > 0)
                {
                    status = StatusCodes.ContentFound;
                    objData.StartIndex = _startIndex;
                    objData.EndIndex = _endIndex > objData.Articles.RecordCount ? Convert.ToInt32(objData.Articles.RecordCount) : _endIndex;
                    BindLinkPager(objData);
                    SetPageMetas(objData);
                    CreatePrevNextUrl(objData);
                    GetWidgetData(objData);
                    objData.Page = Entities.Pages.GAPages.Editorial_List_Page;
                }
                else
                    status = StatusCodes.ContentNotFound;
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Models.BikeCareIndexPage.GetData");
            }
            return objData;
        }

        /// <summary>
        /// Created by : Aditi Srivastava on 1 Apr 2017
        /// Summary    : Bind pager
        /// </summary>
        private void BindLinkPager(BikeCareIndexPageVM objData)
        {
            try
            {
                objData.PagerEntity = new PagerEntity();
                objData.PagerEntity.BaseUrl = (IsMobile ? "/m/bike-care/" : "/bike-care/");
                objData.PagerEntity.PageNo = curPageNo;
                objData.PagerEntity.PagerSlotSize = pagerSlotSize;
                objData.PagerEntity.PageUrlType = "page/";
                objData.PagerEntity.TotalResults = (int)objData.Articles.RecordCount;
                objData.PagerEntity.PageSize = pageSize;
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Models.BikeCareIndexPage.BindLinkPager");
            }
        }

        /// <summary>
        /// Created by : Aditi Srivastava on 1 Apr 2017
        /// Summary    : Set page metas
        /// </summary>
        private void SetPageMetas(BikeCareIndexPageVM objData)
        {
            try
            {
                objData.PageMetaTags.Title = "Bike Care | Maintenance Tips from Bike Experts - BikeWale";
                objData.PageMetaTags.Description = "BikeWale brings you maintenance tips from the bike experts to help you keep your bike in good shape. Read through these maintenance tips to learn more about your bike maintenance";
                objData.PageMetaTags.Keywords = "Bike maintenance, bike common issues, bike common problems, Maintaining bikes, bike care";
                objData.PageMetaTags.CanonicalUrl = string.Format("{0}/bike-care/{1}", BWConfiguration.Instance.BwHostUrl, (curPageNo > 1 ? string.Format("page/{0}/", curPageNo) : ""));
                objData.PageMetaTags.AlternateUrl = string.Format("{0}/m/bike-care/{1}", BWConfiguration.Instance.BwHostUrl, (curPageNo > 1 ? string.Format("page/{0}/", curPageNo) : ""));

                if (curPageNo > 1)
                {
                    objData.PageMetaTags.Description = string.Format("Page {0} of {1} - {2}", curPageNo, _totalPagesCount, objData.PageMetaTags.Description);
                    objData.PageMetaTags.Title = string.Format("Page {0} of {1} - {2}", curPageNo, _totalPagesCount, objData.PageMetaTags.Title);
                }
                SetBreadcrumList(objData);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Models.BikeCareIndexPage.SetPageMetas");
            }
        }

        /// <summary>
        /// Created by : Aditi Srivastava on 1 Apr 2017
        /// Summary    : Create prev and next page urls for SEO
        /// </summary>
        private void CreatePrevNextUrl(BikeCareIndexPageVM objData)
        {
            try
            {
                string _mainUrl = String.Format("{0}{1}page/", BWConfiguration.Instance.BwHostUrl, objData.PagerEntity.BaseUrl);
                string prevPageNumber = string.Empty, nextPageNumber = string.Empty;
                int totalPages = _pager.GetTotalPages((int)objData.Articles.RecordCount, pageSize);
                if (totalPages > 1)
                {
                    if (curPageNo == 1)
                    {
                        nextPageNumber = "2";
                        objData.PageMetaTags.NextPageUrl = string.Format("{0}{1}/", _mainUrl, nextPageNumber);
                    }
                    else if (curPageNo == totalPages)
                    {
                        prevPageNumber = Convert.ToString(curPageNo - 1);
                        objData.PageMetaTags.PreviousPageUrl = string.Format("{0}{1}/", _mainUrl, prevPageNumber);
                    }
                    else
                    {
                        prevPageNumber = Convert.ToString(curPageNo - 1);
                        objData.PageMetaTags.PreviousPageUrl = string.Format("{0}{1}/", _mainUrl, prevPageNumber);
                        nextPageNumber = Convert.ToString(curPageNo + 1);
                        objData.PageMetaTags.NextPageUrl = string.Format("{0}{1}/", _mainUrl, nextPageNumber);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Models.BikeCareIndexPage.CreatePrevNextUrl");
            }
        }

        /// <summary>
        /// Created by : Aditi Srivastava on 1 Apr 2017
        /// Summary    : Get data to populate widget view model
        /// Modified by Sajal Gupta on 5-12-2017
        /// Desc : Addded multi tabs widget
        /// Modified by : Sanskar Gupta on 22 Jan 2018
        /// Description : Added Newly Launched feature
        /// </summary>
        private void GetWidgetData(BikeCareIndexPageVM objData)
        {
            try
            {
                SetAdditionalVariables();
                objData.PageWidgets = base.GetEditorialWidgetData(EnumEditorialPageType.Listing);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Models.BikeCareIndexPage.GetWidgetData");
            }
        }



        /// <summary>
        /// Created By :Snehal Dange on 8th Nov 2017
        /// Description : Function to create page level schema for breadcrum
        /// </summary>
        private void SetBreadcrumList(BikeCareIndexPageVM objVM)
        {
            try
            {
                IList<BreadcrumbListItem> BreadCrumbs = new List<BreadcrumbListItem>();
                string bikeUrl;
                bikeUrl = string.Format("{0}/", Utility.BWConfiguration.Instance.BwHostUrl);
                ushort position = 1;
                if (IsMobile)
                {
                    bikeUrl += "m/";
                }

                BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, bikeUrl, "Home"));

                BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position, null, "Bike Care"));

                objVM.BreadcrumbList.BreadcrumListItem = BreadCrumbs;
            }
            catch (Exception ex)
            {

                Bikewale.Notifications.ErrorClass.LogError(ex, "Bikewale.Models.BikeCareIndexPage.SetBreadcrumList");
            }

        }


        /// <summary>
        /// Created by  :   Sumit Kate on 25 Apr 2018
        /// Description :   Set basic flags to get the editorial widgets
        /// </summary>
        /// <param name="objData"></param>
        private void SetAdditionalVariables()
        {
            EditorialWidgetEntity editorialWidgetData = new EditorialWidgetEntity
            {
                IsMobile = IsMobile,
                CityId = CityId
            };
            base.SetAdditionalData(editorialWidgetData);
        }
        #endregion
    }


}
