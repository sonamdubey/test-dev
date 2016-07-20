using Bikewale.BAL.EditCMS;
using Bikewale.Cache.CMS;
using Bikewale.Cache.Core;
using Bikewale.Common;
using Bikewale.Controls;
using Bikewale.Entities.CMS;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Entities.Pager;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.EditCMS;
using Bikewale.Interfaces.Pager;
using Microsoft.Practices.Unity;
using System;
using System.Web.UI.WebControls;

namespace Bikewale.News
{
    /// <summary>
    /// Modified By : Ashwini Todkar on 24 Sept 2014
    /// </summary>
    public class Default : System.Web.UI.Page
    {
        protected Repeater rptNews;
        protected LinkPagerControl linkPager;

        protected string prevUrl = string.Empty, nextUrl = string.Empty;

        //current page number 
        private int _pageNumber = 1;

        //No. of news to be displayed on a page
        private const int _pageSize = 10;

        private const int _pagerSlotSize = 10;

        protected override void OnInit(EventArgs e)
        {
            base.Load += new EventHandler(Page_Load);
        }

        private void Page_Load(object sender, EventArgs e)
        {
            //code for device detection added by Ashwini Todkar
            // Modified By :Ashish Kamble on 5 Feb 2016
            string originalUrl = Request.ServerVariables["HTTP_X_ORIGINAL_URL"];
            if (String.IsNullOrEmpty(originalUrl))
                originalUrl = Request.ServerVariables["URL"];

            DeviceDetection dd = new DeviceDetection(originalUrl);
            dd.DetectDevice();

            CommonOpn op = new CommonOpn();

            if (Request["pn"] != null && Request.QueryString["pn"] != "")
            {
                if (CommonOpn.CheckId(Request.QueryString["pn"]) == true)
                    _pageNumber = Convert.ToInt32(Request.QueryString["pn"]);
            }

            IPager objPager = GetPager();
            int _startIndex = 0, _endIndex = 0;
            objPager.GetStartEndIndex(_pageSize, _pageNumber, out _startIndex, out _endIndex);
            //string contentTypeList = CommonApiOpn.GetContentTypesString(new List<EnumCMSContentType>() { EnumCMSContentType.News, EnumCMSContentType.AutoExpo2016 });

            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<IArticles, Articles>()
                           .RegisterType<ICMSCacheContent, CMSCacheRepository>()
                           .RegisterType<ICacheManager, MemcacheManager>();
                ICMSCacheContent _cache = container.Resolve<ICMSCacheContent>();

                CMSContent objNews = _cache.GetArticlesByCategory(EnumCMSContentType.News, _startIndex, _endIndex, 0, 0);

                if (objNews != null)
                {
                    BindNews(objNews);
                    BindLinkPager(objPager, Convert.ToInt32(objNews.RecordCount));
                }
            }
        }

        private void BindNews(CMSContent data)
        {

            rptNews.DataSource = data.Articles;
            rptNews.DataBind();
        }

        /// <summary>
        /// Written By : Ashwini Todkar on 24 Sept 2014
        /// PopulateWhere to bind link pager control 
        /// </summary>
        /// <param name="objPager"> Pager instance </param>
        /// <param name="recordCount"> total news available</param>
        private void BindLinkPager(IPager objPager, int recordCount)
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
                linkPager.PagerOutput = _pagerOutput;
                linkPager.CurrentPageNo = _pageNumber;
                linkPager.TotalPages = objPager.GetTotalPages(recordCount, _pageSize);
                linkPager.BindPagerList();

                //For SEO
                CreatePrevNextUrl(linkPager.TotalPages);
            }
            catch (Exception ex)
            {
                Trace.Warn(ex.Message);
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
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
            string _mainUrl = "http://www.bikewale.com/news/page/";
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

        //PopulateWhere to create Pager instance
        private IPager GetPager()
        {
            IPager _objPager = null;
            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<IPager, Bikewale.BAL.Pager.Pager>();
                _objPager = container.Resolve<IPager>();
            }
            return _objPager;
        }


    }//End of Class
}//End of NameSpace