using Bikewale.BAL.EditCMS;
using Bikewale.BAL.Pager;
using Bikewale.Cache.CMS;
using Bikewale.Cache.Core;
using Bikewale.Common;
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bikewale.BindViewModels.Webforms.EditCMS
{
    /// <summary>
    /// Modified By : Sajal Gupta on 30-01-2017
    /// Description : Common logic to bind features listing page 
    /// </summary>
    public class FeaturesListing
    {
        protected int curPageNo = 1;
        public string prevPageUrl = String.Empty, nextPageUrl = String.Empty;
        private const int _pageSize = 10, _pagerSlotSize = 5;
        public bool isContentFound = false;
        public int startIndex = 0, endIndex = 0;
        public int totalrecords;
        public IList<ArticleSummary> articlesList;
        private ICMSCacheContent _cache;
        private IPager _objPager = null;
        public bool IsPageNotFound = true;

        public FeaturesListing()
        {
            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<IArticles, Articles>()
                        .RegisterType<ICMSCacheContent, CMSCacheRepository>()
                        .RegisterType<ICacheManager, MemcacheManager>()
                        .RegisterType<IPager, Pager>();

                _cache = container.Resolve<ICMSCacheContent>();
                _objPager = container.Resolve<IPager>();
            }

            ProcessQueryString();

        }

        private void ProcessQueryString()
        {
            try
            {
                var request = HttpContext.Current.Request;

                if (!string.IsNullOrEmpty(request.QueryString["pn"]))
                {
                    Int32.TryParse(request.QueryString["pn"], out curPageNo);
                }
                IsPageNotFound = false;
            }
            catch (Exception err)
            {
                ErrorClass.LogError(err, "Bikewale.BindViewModels.Webforms.EditCMS.FeaturesListing.ProcessQueryString");
            }
        }

        /// <summary>
        /// Written By : Ashwini Todkar on 30 Sept 2014
        /// Summary    : PopulateWhere to get features list from web api asynchronously
        /// </summary>
        public void GetFeaturesList()
        {
            try
            {
                int _startIndex = 0, _endIndex = 0;

                _objPager.GetStartEndIndex(_pageSize, curPageNo, out _startIndex, out _endIndex);

                List<EnumCMSContentType> categorList = new List<EnumCMSContentType>();
                categorList.Add(EnumCMSContentType.Features);
                categorList.Add(EnumCMSContentType.SpecialFeature);
                string _featuresCategoryId = CommonApiOpn.GetContentTypesString(categorList);

                var _objFeaturesList = _cache.GetArticlesByCategoryList(_featuresCategoryId, _startIndex, _endIndex, 0, 0);

                if (_objFeaturesList != null && _objFeaturesList.Articles.Count > 0)
                {

                    int _totalPages = _objPager.GetTotalPages(Convert.ToInt32(_objFeaturesList.RecordCount), _pageSize);
                    articlesList = _objFeaturesList.Articles;
                    totalrecords = Convert.ToInt32(_objFeaturesList.RecordCount);
                    isContentFound = true;
                }

            }
            catch (Exception err)
            {
                ErrorClass.LogError(err, "Bikewale.BindViewModels.Webforms.EditCMS.FeaturesListing.GetFeaturesList");
            }
        }

        /// <summary>
        ///  Created by : Aditi Srivastava on 17 Nov 2016
        /// Summary     : Create pagination
        /// </summary>
        /// <param name="_ctrlPager"></param>
        public void BindLinkPager(LinkPagerControl _ctrlPager)
        {
            _objPager.GetStartEndIndex(_pageSize, curPageNo, out startIndex, out endIndex);
            PagerOutputEntity _pagerOutput = null;
            PagerEntity _pagerEntity = null;
            int recordCount = totalrecords;
            string _baseUrl = RemoveTrailingPage(HttpContext.Current.Request.RawUrl.ToLower());

            try
            {
                GetStartEndIndex(_pageSize, curPageNo, out startIndex, out endIndex, recordCount);

                _pagerEntity = new PagerEntity();
                _pagerEntity.BaseUrl = _baseUrl;
                _pagerEntity.PageNo = curPageNo; //Current page number
                _pagerEntity.PagerSlotSize = _pagerSlotSize; // 5 links on a page
                _pagerEntity.PageUrlType = "page/";
                _pagerEntity.TotalResults = (int)recordCount; //total News count
                _pagerEntity.PageSize = _pageSize;  //No. of news to be displayed on a page
                _pagerOutput = _objPager.GetPager<PagerOutputEntity>(_pagerEntity);

                // for RepeaterPager

                _ctrlPager.PagerOutput = _pagerOutput;
                _ctrlPager.CurrentPageNo = curPageNo;
                _ctrlPager.TotalPages = _objPager.GetTotalPages((int)recordCount, _pageSize);
                _ctrlPager.BindPagerList();

                //For SEO

                prevPageUrl = String.IsNullOrEmpty(_pagerOutput.PreviousPageUrl) ? string.Empty : "https://www.bikewale.com" + _pagerOutput.PreviousPageUrl;
                nextPageUrl = String.IsNullOrEmpty(_pagerOutput.NextPageUrl) ? string.Empty : "https://www.bikewale.com" + _pagerOutput.NextPageUrl;
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.BindViewModels.Webforms.EditCMS.FeaturesListing.BindLinkPager");
            }
        }
        /// <summary>
        /// Created By : Aditi Srivastava on 17 Nov 2016
        /// Summary    : Set current page's start and end index of articles
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="currentPageNo"></param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <param name="totalCount"></param>
        public void GetStartEndIndex(int pageSize, int currentPageNo, out int startIndex, out int endIndex, int totalCount)
        {
            startIndex = 0;
            endIndex = 0;
            endIndex = currentPageNo * pageSize;
            startIndex = (endIndex - pageSize) + 1;
            if (totalCount < endIndex)
                endIndex = totalCount;
        }
        /// <summary>
        /// Created By : Aditi Srivastava on 17 Nov 2016
        /// Summary    : Remove trailing page from link
        /// </summary>
        /// <param name="rawUrl"></param>
        /// <returns></returns>
        public string RemoveTrailingPage(string rawUrl)
        {
            string retUrl = rawUrl;
            if (rawUrl.Contains("/page/"))
            {
                string[] urlArray = rawUrl.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                retUrl = string.Format("/{0}/", string.Join("/", urlArray.Take(urlArray.Length - 2).ToArray()));
            }
            return retUrl;
        }
    }
}