
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
namespace Bikewale.BindViewModels.Webforms.ServiceCenter
{
    public class BikeCareModels
    {
        private IPager objPager = null;
        private const int _pageSize = 10;
        private int _pageNo = 1;
        public int startIndex = 0, endIndex = 0;
        private const int _pagerSlotSize = 5;
        public string prevUrl = string.Empty, nextUrl = string.Empty, title = string.Empty, description = string.Empty, keywords = string.Empty;
        public int makeId, modelId;
        public uint totalrecords;
        private ICMSCacheContent _cache;
        public CMSContent objArticleList = null;
        public BikeCareModels()
        {
            using (IUnityContainer container = new UnityContainer())
            {

                container.RegisterType<IArticles, Articles>()
                              .RegisterType<ICMSCacheContent, CMSCacheRepository>()
                              .RegisterType<ICacheManager, MemcacheManager>();
                _cache = container.Resolve<ICMSCacheContent>();

            }


            ProcessQueryString();
            BikeCare();
            CreateMetas();
        }
        private void CreateMetas()
        {
            title = string.Format("Bike Care | Maintenance Tips from Bike Experts - BikeWale");
            description = string.Format("BikeWale brings you maintenance tips from the bike experts to help you keep your bike in good shape. Read through these maintenance tips to learn more about your bike maintenance");
            keywords = string.Format("Bike maintenance, bike common issues, bike common problems, Maintaining bikes, bike care");

        }
        private void BikeCare()
        {

            try
            {

                IPager objPager = GetPager();


                objPager.GetStartEndIndex(_pageSize, _pageNo, out startIndex, out endIndex);

                List<EnumCMSContentType> categorList = new List<EnumCMSContentType>();
                categorList.Add(EnumCMSContentType.TipsAndAdvices);
                string _contentType = CommonApiOpn.GetContentTypesString(categorList);



                using (IUnityContainer container = new UnityContainer())
                {

                    objArticleList = _cache.GetArticlesByCategoryList(_contentType, startIndex, endIndex, makeId, modelId);
                    totalrecords = objArticleList.RecordCount;
                }



            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BindMaintainanceTipsControl.MaintainanceTips");
                objErr.SendMail();
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

        public void BindLinkPager(LinkPagerControl _ctrlPager)
        {
            PagerOutputEntity _pagerOutput = null;
            PagerEntity _pagerEntity = null;
            int recordCount = Convert.ToInt32(totalrecords);
            string _baseUrl = RemoveTrailingPage(HttpContext.Current.Request.RawUrl.ToLower());
            IPager objPager = GetPager();
            try
            {
                GetStartEndIndex(_pageSize, _pageNo, out startIndex, out endIndex, recordCount);

                _pagerEntity = new PagerEntity();
                _pagerEntity.BaseUrl = string.Format("{0}?pn", _baseUrl);
                _pagerEntity.PageNo = _pageNo; //Current page number
                _pagerEntity.PagerSlotSize = _pagerSlotSize; // 5 links on a page
                _pagerEntity.TotalResults = (int)recordCount; //total News count
                _pagerEntity.PageSize = _pageSize;  //No. of news to be displayed on a page
                _pagerOutput = objPager.GetTipsAndAdvicePager<PagerOutputEntity>(_pagerEntity);

                // for RepeaterPager

                _ctrlPager.PagerOutput = _pagerOutput;
                _ctrlPager.CurrentPageNo = _pageNo;
                _ctrlPager.TotalPages = objPager.GetTotalPages((int)recordCount, _pageSize);
                _ctrlPager.BindPagerList();

                //For SEO

                prevUrl = String.IsNullOrEmpty(_pagerOutput.PreviousPageUrl) ? string.Empty : "http://www.bikewale.com" + _pagerOutput.PreviousPageUrl;
                nextUrl = String.IsNullOrEmpty(_pagerOutput.NextPageUrl) ? string.Empty : "http://www.bikewale.com" + _pagerOutput.NextPageUrl;
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

        private void ProcessQueryString()
        {
            HttpContext page = HttpContext.Current;

            try
            {

                if (!String.IsNullOrEmpty(page.Request.QueryString["pn"]))
                {
                    int result;
                    int.TryParse(page.Request.QueryString["pn"], out result);
                    _pageNo = result;
                }

            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, page.Request.ServerVariables["URL"] + "ParseQueryString");
                objErr.SendMail();
            }


        }

        public string RemoveTrailingPage(string rawUrl)
        {
            string retUrl = rawUrl;
            if (rawUrl.Contains("?pn="))
            {
                string[] urlArray = rawUrl.Split(new string[] { "?" }, StringSplitOptions.RemoveEmptyEntries);
                retUrl = string.Format("{0}", string.Join("/", urlArray.Take(urlArray.Length - 1).ToArray()));
            }
            return retUrl;
        }
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