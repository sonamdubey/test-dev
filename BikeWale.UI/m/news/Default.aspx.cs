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
using System.Configuration;
using System.Web.UI.WebControls;

namespace Bikewale.Mobile.News
{
    /// <summary>
    /// Created By : Ashwini Todkar on 21 May 2014
    /// Modified By : Ashwini Todkar on 1 Oct 2014
    /// </summary>
    public class Default : System.Web.UI.Page
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

        /// <summary>
        /// Modified By : Sushil Kumar on 26th July 2016
        /// Description : Added Features,expert reviews and autoexpo categories for multiple categories 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

                List<EnumCMSContentType> categorList = new List<EnumCMSContentType>();
                categorList.Add(EnumCMSContentType.AutoExpo2016);
                categorList.Add(EnumCMSContentType.News);
                categorList.Add(EnumCMSContentType.Features);
                categorList.Add(EnumCMSContentType.RoadTest);
                categorList.Add(EnumCMSContentType.ComparisonTests);
                string contentTypeList = CommonApiOpn.GetContentTypesString(categorList);

                categorList.Clear();
                categorList = null;

                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IArticles, Articles>()
                           .RegisterType<ICMSCacheContent, CMSCacheRepository>()
                           .RegisterType<ICacheManager, MemcacheManager>();
                    ICMSCacheContent _cache = container.Resolve<ICMSCacheContent>();

                    CMSContent objNews = _cache.GetArticlesByCategoryList(contentTypeList, _startIndex, _endIndex, 0, 0);

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

        /// <summary>
        /// Created By : Sushil Kumar on 26th July 2016
        ///  Description  : Function to show category type based on categoryId
        /// </summary>
        /// <param name="contentType"></param>
        /// <returns></returns>
        protected string GetContentCategory(string contentType)
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
                            _category = "NEWS";
                            break;
                        case EnumCMSContentType.News:
                            _category = "NEWS";
                            break;
                        case EnumCMSContentType.Features:
                            _category = "FEATURES";
                            break;
                        case EnumCMSContentType.ComparisonTests:
                            _category = "EXPERT REVIEWS";
                            break;
                        case EnumCMSContentType.RoadTest:
                            _category = "EXPERT REVIEWS";
                            break;
                        default:
                            break;
                    }
                }

            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "Exception : Mobile.News.Default.GetContentCategory");
                objErr.SendMail();
            }
            return _category;
        }
    }
}