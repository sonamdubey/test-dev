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
using Microsoft.Practices.Unity;
using System;
using System.Configuration;
using System.Web.UI.WebControls;

namespace Bikewale.Mobile.News
{
    /// <summary>
    /// Created By : Ashwini Todkar on 21 May 2014
    /// Modified By : Ashwini Todkar on 1 Oct 2014
    /// </summary>
    public partial class Default : System.Web.UI.Page
    {
        protected Repeater rptNews;
        protected int curPageNo = 1, totalPages = 0;
        protected string prevPageUrl = String.Empty, nextPageUrl = String.Empty;
        protected ListPagerControl listPager;
        private const int _pageSize = 10;

        static bool _useGrpc = Convert.ToBoolean(ConfigurationManager.AppSettings["UseGrpc"]);

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!String.IsNullOrEmpty(Request.QueryString["pn"]))
                    if (!Int32.TryParse(Request.QueryString["pn"], out curPageNo))
                        curPageNo = 1;

                IPager objPager = GetPager();
                int _startIndex = 0, _endIndex = 0;
                objPager.GetStartEndIndex(_pageSize, curPageNo, out _startIndex, out _endIndex);
                // string contentTypeList = CommonApiOpn.GetContentTypesString(new List<EnumCMSContentType>() { EnumCMSContentType.News, EnumCMSContentType.AutoExpo2016 });

                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IArticles, Articles>()
                           .RegisterType<ICMSCacheContent, CMSCacheRepository>()
                           .RegisterType<ICacheManager, MemcacheManager>();
                    ICMSCacheContent _cache = container.Resolve<ICMSCacheContent>();

                    CMSContent objNews = _cache.GetArticlesByCategoryList(Convert.ToString((int)EnumCMSContentType.News), _startIndex, _endIndex, 0, 0);

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
                _pagerEntity.BaseUrl = "/m/news/";
                _pagerEntity.PageNo = curPageNo; //Current page number
                _pagerEntity.PagerSlotSize = objPager.GetTotalPages(recordCount, _pageSize); // 5 links on a page
                _pagerEntity.PageUrlType = "page/";
                _pagerEntity.TotalResults = recordCount; //total News count
                _pagerEntity.PageSize = _pageSize;        //No. of news to be displayed on a page

                _pagerOutput = objPager.GetPager<PagerOutputEntity>(_pagerEntity);

                // for RepeaterPager
                listPager.PagerOutput = _pagerOutput;
                listPager.CurrentPageNo = curPageNo;
                listPager.TotalPages = objPager.GetTotalPages(recordCount, _pageSize);
                listPager.BindPageNumbers();

                //For SEO
                //get next and prev page links for SEO
                prevPageUrl = _pagerOutput.PreviousPageUrl;
                nextPageUrl = _pagerOutput.NextPageUrl;
            }
            catch (Exception ex)
            {
                Trace.Warn(ex.Message);
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

        //PopulateWhere to create Pager instance
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
    }
}