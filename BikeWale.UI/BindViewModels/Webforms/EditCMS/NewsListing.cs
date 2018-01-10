using System;
using System.Collections.Generic;
using System.Web;
using Bikewale.BAL.EditCMS;
using Bikewale.Cache.CMS;
using Bikewale.Cache.Core;
using Bikewale.Common;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.CMS;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Entities.Pager;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.EditCMS;
using Bikewale.Interfaces.Pager;
using Bikewale.Mobile.Controls;
using Bikewale.Utility;
using Microsoft.Practices.Unity;

namespace Bikewale.BindViewModels.Webforms.EditCMS
{
    /// <summary>
    /// Modified By : Sushil Kumar on 10th Nov 2016
    /// Description : Common logic to bind news listing page 
    /// </summary>
    public class NewsListing
    {

        private int _pageNumber = 1, _pageSize = 10, _pagerSlotSize = 5;
        public uint MakeId, ModelId;
        private ICMSCacheContent _objNewsCache = null;
        private IPager _objPager = null;
        public IEnumerable<ArticleSummary> objNewsList = null;
        public int StartIndex, EndIndex;
        public uint TotalArticles;
        public bool IsMake301, IsModel301, IsPageNotFound;
        public string prevUrl = string.Empty, nextUrl = string.Empty, make, model;
        private readonly MakeHelper makeHelper = null;
        private readonly ModelHelper modelHelper = null;
        private string RedirectUrl = String.Empty;
        public string PageTitle = String.Empty, Description, Keywords, Canonical, PrevUrl, NextUrl, Alternate, PageH1, PageH2;
        public BikeMakeEntityBase objMake;
        public BikeModelEntityBase objModel;
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
            makeHelper = new MakeHelper();
            modelHelper = new ModelHelper();
            ProcessQueryString();
        }

        /// <summary>
        /// Modified By : Sushil Kumar on 10th Nov 2016
        /// Description : To process query string for page number
        /// </summary>
        public void ProcessQueryString()
        {
            var request = HttpContext.Current.Request;
            var queryString = request != null ? request.QueryString : null;

            if (queryString != null)
            {

                if (!string.IsNullOrEmpty(queryString["pn"]))
                {
                    string pageNo = queryString["pn"];
                    if (!string.IsNullOrEmpty(pageNo))
                    {
                        int.TryParse(pageNo, out _pageNumber);
                    }
                }
                make = queryString["make"];
                model = queryString["model"];

                ProcessMakeMaskingName(request, make);
                ProcessModelMaskingName(request, make, model);
                HandleRedirection();
            }
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 02 Jan 2017
        /// Description :   Handles 301 Redirections for make and model specific news urls
        /// </summary>
        private void HandleRedirection()
        {
            if (IsMake301 || IsModel301)
            {
                CommonOpn.RedirectPermanent(RedirectUrl);
            }
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 02 jan 2017
        /// Description :   Processes model masking name
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        private void ProcessModelMaskingName(HttpRequest request, string make, string model)
        {
            ModelMaskingResponse modelResponse = null;
            if (!String.IsNullOrEmpty(model))
            {
                modelResponse = modelHelper.GetModelDataByMasking(make, model);
            }
            if (modelResponse != null)
            {
                if (modelResponse.StatusCode == 200)
                {
                    ModelId = modelResponse.ModelId;
                    objModel = modelHelper.GetModelDataById(ModelId);
                }
                else if (modelResponse.StatusCode == 301)
                {
                    RedirectUrl = request.RawUrl.Replace(model, modelResponse.MaskingName);
                    IsModel301 = true;
                }
                else
                {
                    IsPageNotFound = true;
                }
            }
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 02 Jan 2017
        /// Description :   Processes Make masking name
        /// </summary>
        /// <param name="request"></param>
        /// <param name="make"></param>
        private void ProcessMakeMaskingName(HttpRequest request, string make)
        {
            MakeMaskingResponse makeResponse = null;
            if (!String.IsNullOrEmpty(make))
            {
                makeResponse = makeHelper.GetMakeByMaskingName(make);
            }
            if (makeResponse != null)
            {
                if (makeResponse.StatusCode == 200)
                {
                    MakeId = makeResponse.MakeId;
                    objMake = makeHelper.GetMakeNameByMakeId(MakeId);
                }
                else if (makeResponse.StatusCode == 301)
                {
                    RedirectUrl = request.RawUrl.Replace(make, makeResponse.MaskingName);
                    IsMake301 = true;
                }
                else
                {
                    IsPageNotFound = true;
                }
            }
        }

        /// <summary>
        /// Created BY : Sushil Kumar on 28th July 2016
        /// Description : To Load news list
        /// Modified by :   Sumit Kate on 02 Jan 2017
        /// Description :   Pass Make and Model to get news lists
        /// Modified by :   Sumit Kate on 05 Jan 2017
        /// Description :   Get only News for make and model specific news
        /// </summary>
        public void FetchNewsList(LinkPagerControl ctrlPager, bool isMobile = false)
        {
            try
            {

                int _startIndex = 0, _endIndex = 0;
                _objPager.GetStartEndIndex(_pageSize, _pageNumber, out _startIndex, out _endIndex);

                List<EnumCMSContentType> categorList = new List<EnumCMSContentType>();
                categorList.Add(EnumCMSContentType.News);
                if (MakeId == 0 && ModelId == 0)
                {
                    categorList.Add(EnumCMSContentType.AutoExpo2016);
                    categorList.Add(EnumCMSContentType.Features);
                    categorList.Add(EnumCMSContentType.RoadTest);
                    categorList.Add(EnumCMSContentType.ComparisonTests);
                    categorList.Add(EnumCMSContentType.SpecialFeature);
                    categorList.Add(EnumCMSContentType.TipsAndAdvices);
                }
                string contentTypeList = Bikewale.Utility.CommonApiOpn.GetContentTypesString(categorList);

                categorList.Clear();
                categorList = null;

                CMSContent objNews = _objNewsCache.GetArticlesByCategoryList(contentTypeList, _startIndex, _endIndex, (int)MakeId, (int)ModelId);

                if (objNews != null && objNews.RecordCount > 0)
                {
                    objNewsList = objNews.Articles;
                    TotalArticles = objNews.RecordCount;
                    StartIndex = _startIndex;
                    EndIndex = _endIndex > objNews.RecordCount ? Convert.ToInt32(objNews.RecordCount) : _endIndex;
                    BindLinkPager(ctrlPager, _objPager, Convert.ToInt32(objNews.RecordCount), isMobile);
                    PageMetas();
                }
                else
                {
                    IsPageNotFound = true;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "NewsListing.FetchNewsList");
            }
        }


        /// <summary>
        /// Created By : Sushil Kumar on 10th Nov 2016
        /// Description : Bind pages for news list
        /// Modified by :   Sumit Kate on 02 Jan 2017
        /// Description :   Format URLs for Mobile site as well
        /// </summary>
        /// <param name="ctrlPager"></param>
        /// <param name="objPager"></param>
        /// <param name="recordCount"></param>
        private void BindLinkPager(LinkPagerControl ctrlPager, IPager objPager, int recordCount, bool isMobile = false)
        {
            PagerOutputEntity _pagerOutput = null;
            PagerEntity _pagerEntity = null;

            try
            {
                _pagerEntity = new PagerEntity();
                _pagerEntity.BaseUrl = (isMobile ? "/m" : "") + UrlFormatter.FormatNewsUrl(make, model);
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
                CreatePrevNextUrl(ctrlPager.TotalPages, _pagerEntity.BaseUrl);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.News.NewsListing.BindLinkPager");
                
            }
        }

        /// <summary>
        /// Written By : Ashwini Todkar on 24 Sept 2014
        /// PopulateWhere to get relative next and previous page url links for SEO 
        /// Modified by :   Sumit Kate on 02 Jan 2017
        /// Description :   Create next and prev page urls as per the page(make/model/generic)
        /// </summary>
        /// <param name="totalPages"></param>
        private void CreatePrevNextUrl(int totalPages, string baseUrl)
        {
            string _mainUrl = String.Format("{0}{1}page/", BWConfiguration.Instance.BwHostUrlForJs, baseUrl);
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
                        case EnumCMSContentType.SpecialFeature:
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
                ErrorClass.LogError(ex, "Exception : Desktop.News.Default.GetContentCategory");
                
            }
            return _category;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 02 Jan 2017
        /// Description :   Build Page metas like canonical/alternate/page title/description/keywords
        /// </summary>
        private void PageMetas()
        {
            Canonical = String.Format("{0}{1}{2}", BWConfiguration.Instance.BwHostUrlForJs, UrlFormatter.FormatNewsUrl(make, model), (_pageNumber > 1 ? String.Format("page/{0}/", _pageNumber) : ""));
            Alternate = String.Format("{0}/m{1}{2}", BWConfiguration.Instance.BwHostUrlForJs, UrlFormatter.FormatNewsUrl(make, model), (_pageNumber > 1 ? String.Format("page/{0}/", _pageNumber) : ""));
            if (ModelId > 0)
            {
                PageTitle = String.Format("Latest News about {0} {1} | {0} {1} News - BikeWale", objMake.MakeName, objModel.ModelName);
                Description = String.Format("Read the latest news about {0} {1} bikes exclusively on BikeWale. Know more about {1}.", objMake.MakeName, objModel.ModelName);
                PageH1 = String.Format("{0} {1} Bikes News", objMake.MakeName, objModel.ModelName);
                PageH2 = String.Format("Latest {0} {1} Bikes News and Views", objMake.MakeName, objModel.ModelName);
            }
            else if (MakeId > 0)
            {
                PageTitle = String.Format("Latest News about {0} Bikes | {0} Bikes News - BikeWale", objMake.MakeName);
                Description = String.Format("Read the latest news about popular and upcoming {0} bikes exclusively on BikeWale. Know more about {0} bikes.", objMake.MakeName);
                PageH1 = String.Format("{0} Bikes News", objMake.MakeName);
                PageH2 = String.Format("Latest {0} Bikes News and Views", objMake.MakeName);
            }
            else
            {
                PageTitle = "Bike News - Latest Indian Bike News & Views | BikeWale";
                Description = "Latest news updates on Indian bikes industry, expert views and interviews exclusively on BikeWale.";
                Keywords = "news, bike news, auto news, latest bike news, indian bike news, bike news of india";
                PageH1 = String.Format("Bike News");
                PageH2 = String.Format("Latest Indian Bikes News and Views");
            }
        }

    }
}