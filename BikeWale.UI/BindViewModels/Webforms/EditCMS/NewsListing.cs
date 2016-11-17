using Bikewale.BAL.EditCMS;
using Bikewale.Cache.CMS;
using Bikewale.Cache.Core;
using Bikewale.Entities.CMS;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Entities.Pager;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.EditCMS;
using Bikewale.Interfaces.Pager;
using Bikewale.Mobile.Controls;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Web;

namespace Bikewale.BindViewModels.Webforms.EditCMS
{
    /// <summary>
    /// Modified By : Sushil Kumar on 10th Nov 2016
    /// Description : Common logic to bind news listing page 
    /// </summary>
    public class NewsListing
    {

        private int _pageNumber = 1, _pageSize = 10, _pagerSlotSize = 5;
        private ICMSCacheContent _objNewsCache = null;
        private IPager _objPager = null;
        public IEnumerable<ArticleSummary> objNewsList = null;
        public int StartIndex, EndIndex;
        public uint TotalArticles;

        public string prevUrl = string.Empty, nextUrl = string.Empty;

        /// <summary>
        /// Constructor to initialize and resolve unity containers
        /// </summary>
        public NewsListing()
        {
            using (IUnityContainer container = new UnityContainer())
            {

                container.RegisterType<IArticles, Articles>()
                        .RegisterType<ICMSCacheContent, CMSCacheRepository>()
                        .RegisterType<ICacheManager, MemcacheManager>()
                        .RegisterType<IPager, Bikewale.BAL.Pager.Pager>();

                _objNewsCache = container.Resolve<ICMSCacheContent>();
                _objPager = container.Resolve<IPager>();
            }
            ProcessQueryString();
        }

        /// <summary>
        /// Modified By : Sushil Kumar on 10th Nov 2016
        /// Description : To process query string for page number
        /// </summary>
        public void ProcessQueryString()
        {
            var request = HttpContext.Current.Request;
            if (request != null && !string.IsNullOrEmpty(request["pn"]))
            {
                string pageNo = request.QueryString["pn"];
                if (!string.IsNullOrEmpty(pageNo))
                {
                    int.TryParse(pageNo, out _pageNumber);
                }
            }

        }

        /// <summary>
        /// Created BY : Sushil Kumar on 28th July 2016
        /// Description : To Load news list
        /// </summary>
        public void FetchNewsList(LinkPagerControl ctrlPager)
        {
            try
            {

                int _startIndex = 0, _endIndex = 0;
                _objPager.GetStartEndIndex(_pageSize, _pageNumber, out _startIndex, out _endIndex);

                List<EnumCMSContentType> categorList = new List<EnumCMSContentType>();
                categorList.Add(EnumCMSContentType.AutoExpo2016);
                categorList.Add(EnumCMSContentType.News);
                categorList.Add(EnumCMSContentType.Features);
                categorList.Add(EnumCMSContentType.RoadTest);
                categorList.Add(EnumCMSContentType.ComparisonTests);
                categorList.Add(EnumCMSContentType.SpecialFeature);
                string contentTypeList = Bikewale.Utility.CommonApiOpn.GetContentTypesString(categorList);

                categorList.Clear();
                categorList = null;

                CMSContent objNews = _objNewsCache.GetArticlesByCategoryList(contentTypeList, _startIndex, _endIndex, 0, 0);

                if (objNews != null && objNews.RecordCount > 0)
                {
                    objNewsList = objNews.Articles;
                    TotalArticles = objNews.RecordCount;
                    StartIndex = _startIndex;
                    EndIndex = _endIndex > objNews.RecordCount ? Convert.ToInt32(objNews.RecordCount) : _endIndex;
                    BindLinkPager(ctrlPager, _objPager, Convert.ToInt32(objNews.RecordCount));
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"] + " : Bikewale.News.NewsListing.FetchNewsList");
                objErr.SendMail();

            }
        }


        /// <summary>
        /// Created By : Sushil Kumar on 10th Nov 2016
        /// Description : Bind pages for news list
        /// </summary>
        /// <param name="ctrlPager"></param>
        /// <param name="objPager"></param>
        /// <param name="recordCount"></param>
        private void BindLinkPager(LinkPagerControl ctrlPager, IPager objPager, int recordCount)
        {
            PagerOutputEntity _pagerOutput = null;
            PagerEntity _pagerEntity = null;

            try
            {
                _pagerEntity = new PagerEntity();
                _pagerEntity.BaseUrl = "/news/";
                _pagerEntity.PageNo = _pageNumber; //Current page number
                _pagerEntity.PagerSlotSize = _pagerSlotSize; // 5 links on a page
                _pagerEntity.PageUrlType = "page/";
                _pagerEntity.TotalResults = recordCount; //total News count
                _pagerEntity.PageSize = _pageSize;        //No. of news to be displayed on a page

                _pagerOutput = objPager.GetPager<PagerOutputEntity>(_pagerEntity);

                // for RepeaterPager
                ctrlPager.PagerOutput = _pagerOutput;
                ctrlPager.CurrentPageNo = _pageNumber;
                ctrlPager.TotalPages = objPager.GetTotalPages(recordCount, _pageSize);
                ctrlPager.BindPagerList();

                //For SEO
                CreatePrevNextUrl(ctrlPager.TotalPages);
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "Exception : Bikewale.News.NewsListing.BindLinkPager");
                objErr.SendMail();
            }
        }

        /// <summary>
        /// Written By : Ashwini Todkar on 24 Sept 2014
        /// PopulateWhere to get relative next and previous page url links for SEO 
        /// </summary>
        /// <param name="totalPages"></param>
        private void CreatePrevNextUrl(int totalPages)
        {
            string _mainUrl = "https://www.bikewale.com/news/page/";
            string prevPageNumber = string.Empty, nextPageNumber = string.Empty;

            if (_pageNumber == 1)    //if page is first page
            {
                nextPageNumber = "2";
                nextUrl = _mainUrl + nextPageNumber + "/";
            }
            else if (_pageNumber == totalPages)    //if page is last page
            {
                prevPageNumber = (_pageNumber - 1).ToString();
                prevUrl = _mainUrl + prevPageNumber + "/";
            }
            else
            {          //for middle pages
                prevPageNumber = (_pageNumber - 1).ToString();
                prevUrl = _mainUrl + prevPageNumber + "/";
                nextPageNumber = (_pageNumber + 1).ToString();
                nextUrl = _mainUrl + nextPageNumber + "/";
            }
        }

        /// <summary>
        /// Created By : Sushil Kumar on 26th July 2016
        ///  Description  : Function to show category type based on categoryId
        /// </summary>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public string GetContentCategory(string contentType)
        {
            string _category = string.Empty;
            EnumCMSContentType _contentType = default(EnumCMSContentType);
            try
            {
                if (!string.IsNullOrEmpty(contentType) && Enum.TryParse<EnumCMSContentType>(contentType, true, out _contentType))
                {
                    switch (_contentType)
                    {
                        case EnumCMSContentType.AutoExpo2016:
                        case EnumCMSContentType.News:
                            _category = "NEWS";
                            break;
                        case EnumCMSContentType.Features:
                            _category = "FEATURES";
                            break;
                        case EnumCMSContentType.ComparisonTests:
                        case EnumCMSContentType.RoadTest:
                            _category = "EXPERT REVIEWS";
                            break;
                        case EnumCMSContentType.TipsAndAdvices:
                            _category = "BIKE CARE";
                            break;
                        default:
                            break;
                    }
                }

            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "Exception : Desktop.News.Default.GetContentCategory");
                objErr.SendMail();
            }
            return _category;
        }

    }
}