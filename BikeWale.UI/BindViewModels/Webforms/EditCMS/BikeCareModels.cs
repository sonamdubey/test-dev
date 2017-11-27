
using Bikewale.BAL.EditCMS;
using Bikewale.BAL.Pager;
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
using Bikewale.Notifications;
using Bikewale.Utility;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace Bikewale.BindViewModels.Webforms.EditCMS
{
    /// <summary>
    /// Created By:- Subodh jain 11 Nov 2016
    /// Summary :- Bike Care Landing page
    /// </summary>
    public class BikeCareModels
    {
        private IPager objPager = null;
        private const int _pageSize = 10;
        private int _pageNo = 1;
        public int startIndex = 0, endIndex = 0;
        private const int _pagerSlotSize = 5;
        public string prevUrl = string.Empty, nextUrl = string.Empty, title = string.Empty, description = string.Empty, keywords = string.Empty;
        public int makeId, modelId;
        public uint totalRecords;
        private ICMSCacheContent _objBikeCareCache;
        public CMSContent objArticleList = null;
        HttpRequest page = HttpContext.Current.Request;
        public bool pageNotFound = false;
        // <summary>
        /// Created By:- Subodh jain 11 Nov 2016
        /// Summary :- Bike Care Landing page Constructor (resolving interface)
        /// </summary>
        public BikeCareModels()
        {
            using (IUnityContainer container = new UnityContainer())
            {

                container.RegisterType<IArticles, Articles>()
                              .RegisterType<ICMSCacheContent, CMSCacheRepository>()
                              .RegisterType<ICacheManager, MemcacheManager>();
                _objBikeCareCache = container.Resolve<ICMSCacheContent>();

            }


            ProcessQueryString();
            BikeCare();
            CreateMetas();

        }
        /// <summary>
        /// Created By:- Subodh jain 11 Nov 2016
        /// Summary :- Bike Care Landing page metas
        /// </summary>
        private void CreateMetas()
        {
            title = string.Format("Bike Care | Maintenance Tips from Bike Experts - BikeWale");
            description = string.Format("BikeWale brings you maintenance tips from the bike experts to help you keep your bike in good shape. Read through these maintenance tips to learn more about your bike maintenance");
            keywords = string.Format("Bike maintenance, bike common issues, bike common problems, Maintaining bikes, bike care");

        }

        /// <summary>
        /// Created By:- Subodh jain 11 Nov 2016
        /// Summary :- Bike Care Landing page Articles of tips And Advice
        /// </summary>
        private void BikeCare()
        {

            try
            {

                objPager = GetPager();


                objPager.GetStartEndIndex(_pageSize, _pageNo, out startIndex, out endIndex);

                List<EnumCMSContentType> categorList = new List<EnumCMSContentType>();
                categorList.Add(EnumCMSContentType.TipsAndAdvices);
                string _contentType = CommonApiOpn.GetContentTypesString(categorList);
                using (IUnityContainer container = new UnityContainer())
                {

                    objArticleList = _objBikeCareCache.GetArticlesByCategoryList(_contentType, startIndex, endIndex, makeId, modelId);
                    totalRecords = objArticleList.RecordCount;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikeCareModels.BikeCare");
            }

        }
        private IPager GetPager()
        {
            IPager _objPager = null;
            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<IPager, Pager>();
                _objPager = container.Resolve<IPager>();
            }
            return _objPager;
        }
        /// <summary>
        /// Created By:- Subodh jain 11 Nov 2016
        /// Summary :- Bike Care Landing page bind link pager
        /// </summary>
        public void BindLinkPager(LinkPagerControl _ctrlPager)
        {
            PagerOutputEntity _pagerOutput = null;
            PagerEntity _pagerEntity = null;
            int recordCount = Convert.ToInt32(totalRecords);
            string _baseUrl = RemoveTrailingPage(page.RawUrl.ToLower());

            try
            {
                GetStartEndIndex(_pageSize, _pageNo, out startIndex, out endIndex, recordCount);

                _pagerEntity = new PagerEntity();
                _pagerEntity.BaseUrl = _baseUrl;
                _pagerEntity.PageNo = _pageNo; //Current page number
                _pagerEntity.PagerSlotSize = _pagerSlotSize; // 5 links on a page
                _pagerEntity.PageUrlType = "page/";
                _pagerEntity.TotalResults = (int)recordCount; //total News count
                _pagerEntity.PageSize = _pageSize;  //No. of news to be displayed on a page
                _pagerOutput = objPager.GetPager<PagerOutputEntity>(_pagerEntity);

                // for RepeaterPager

                _ctrlPager.PagerOutput = _pagerOutput;
                _ctrlPager.CurrentPageNo = _pageNo;
                _ctrlPager.TotalPages = objPager.GetTotalPages((int)recordCount, _pageSize);
                _ctrlPager.BindPagerList();

                //For SEO

                prevUrl = String.IsNullOrEmpty(_pagerOutput.PreviousPageUrl) ? string.Empty : "https://www.bikewale.com" + _pagerOutput.PreviousPageUrl;
                nextUrl = String.IsNullOrEmpty(_pagerOutput.NextPageUrl) ? string.Empty : "https://www.bikewale.com" + _pagerOutput.NextPageUrl;
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikeCareModels.BindLinkPager");
            }
        }
        /// <summary>
        /// Created By:- Subodh jain 11 Nov 2016
        /// Summary :- Bike Care Landing page ProcessQueryString
        /// </summary>

        private void ProcessQueryString()
        {
            try
            {

                if (!String.IsNullOrEmpty(page.QueryString["pn"]))
                {

                    int.TryParse(page.QueryString["pn"], out _pageNo);

                }

            }
            catch (Exception ex)
            {
                pageNotFound = true;
                ErrorClass.LogError(ex, "BikeCareModels.ParseQueryString");
            }


        }

        /// <summary>
        /// Created By:- Subodh jain 11 Nov 2016
        /// Summary :- Bike Care Landing page remove trailing page from link
        /// </summary>
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
        /// <summary>
        /// Created By:- Subodh jain 11 Nov 2016
        /// Summary :- Bike Care Landing page Get Start and End index
        /// </summary>
        public void GetStartEndIndex(int pageSize, int currentPageNo, out int startIndex, out int endIndex, int totalCount)
        {
            startIndex = 0;
            endIndex = 0;
            endIndex = currentPageNo * pageSize;
            startIndex = (endIndex - pageSize) + 1;
            if (totalCount < endIndex)
                endIndex = totalCount;
        }


    }
}