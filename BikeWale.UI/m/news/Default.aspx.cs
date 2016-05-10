using Bikewale.BAL.GrpcFiles;
using Bikewale.BAL.Pager;
using Bikewale.Common;
using Bikewale.Entities.CMS;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Entities.Pager;
using Bikewale.Interfaces.Pager;
using Bikewale.Mobile.Controls;
using Bikewale.Utility;
using Grpc.CMS;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
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

                GetNews();
            }
        }

        //private void GetNewsList()
        //{
        //    int recordCount = 0;

        //    using (IUnityContainer container = new UnityContainer())
        //    {
        //        ContentFilter filter = new ContentFilter();

        //        container.RegisterType<ICMSContentRepository<CMSContentListEntity, CMSPageDetailsEntity>, CMSData<CMSContentListEntity, CMSPageDetailsEntity>>();
        //        ICMSContentRepository<CMSContentListEntity, CMSPageDetailsEntity> repository = container.Resolve<ICMSContentRepository<CMSContentListEntity, CMSPageDetailsEntity>>(new ResolverOverride[] { new ParameterOverride("contentType", EnumCMSContentType.News) });

        //        container.RegisterType<IPager, Pager>();
        //        IPager objPager = container.Resolve<IPager>();

        //        int startIndex = 0, endIndex = 0, pageSize = 10;
        //        objPager.GetStartEndIndex(pageSize, curPageNo, out startIndex, out endIndex);

        //        //get page wise news list
        //        IList<CMSContentListEntity> objContentList = repository.GetContentList(startIndex, endIndex , out recordCount, filter);

        //        totalPages = objPager.GetTotalPages(recordCount, pageSize);

        //        //if current page number exceeded the total pages count i.e. the page is not available 
        //        if (curPageNo > 0 && curPageNo <= totalPages)
        //        {
        //            if (objContentList.Count > 0)
        //            {
        //                rptNews.DataSource = objContentList;
        //                rptNews.DataBind();
        //            }

        //            PagerEntity pagerEntity = new PagerEntity();
        //            pagerEntity.BaseUrl = "/m/news/";
        //            pagerEntity.PageNo = curPageNo;
        //            pagerEntity.PagerSlotSize = totalPages;
        //            pagerEntity.PageUrlType = "page/";
        //            pagerEntity.TotalResults = recordCount;
        //            pagerEntity.PageSize = pageSize;

        //            PagerOutputEntity pagerOutput = objPager.GetPager<PagerOutputEntity>(pagerEntity);

        //            //get next and prev page links for SEO 
        //            listPager.PagerOutput = pagerOutput;
        //            listPager.TotalPages = totalPages;
        //            listPager.CurrentPageNo = curPageNo;

        //            //get next and prev page links for SEO
        //            prevPageUrl = pagerOutput.PreviousPageUrl;
        //            nextPageUrl = pagerOutput.NextPageUrl;
        //        }
        //        else
        //        {
        //            Response.Redirect("/m/pagenotfound.aspx", false);
        //            HttpContext.Current.ApplicationInstance.CompleteRequest();
        //            this.Page.Visible = false;
        //        }
        //    }
        // }

        private void BindNews(CMSContent data)
        {

            rptNews.DataSource = data.Articles;
            rptNews.DataBind();
        }


        private void GetNews()
        {
            try
            {
                if (_useGrpc)
                {
                    // get pager instance
                    IPager objPager = GetPager();

                    int _startIndex = 0, _endIndex = 0;
                    objPager.GetStartEndIndex(_pageSize, curPageNo, out _startIndex, out _endIndex);

                    string contentTypeList = CommonApiOpn.GetContentTypesString(new List<EnumCMSContentType>() { EnumCMSContentType.News, EnumCMSContentType.AutoExpo2016 });

                    var _objGrpcArticle = GrpcMethods.GetArticleListByCategory(contentTypeList, (uint)_startIndex, (uint)_endIndex);

                    if (_objGrpcArticle != null && _objGrpcArticle.RecordCount > 0)
                    {
                        BindNews(GrpcToBikeWaleConvert.ConvertFromGrpcToBikeWale(_objGrpcArticle));
                        BindListPager(objPager, Convert.ToInt32(_objGrpcArticle.RecordCount));
                    }
                    else
                    {
                        GetNewsOldWay();
                    }

                }
                else
                {
                    GetNewsOldWay();
                }
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

        /// <summary>
        /// Written By : Ashwini Todkar on 1 Oct 2014
        /// Summary    : method to fetch news list and total record count from carwale api
        /// </summary>

        private async void GetNewsOldWay()
        {
            try
            {
                using (var client = new HttpClient())
                {

                    //sets the base URI for HTTP requests
                    string _cwHostUrl = ConfigurationManager.AppSettings["cwApiHostUrl"];
                    client.BaseAddress = new Uri(_cwHostUrl);

                    //sets the Accept header to "application/json", which tells the server to send data in JSON format.
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    // get pager instance
                    IPager objPager = GetPager();

                    int _startIndex = 0, _endIndex = 0;
                    objPager.GetStartEndIndex(_pageSize, curPageNo, out _startIndex, out _endIndex);
                    List<EnumCMSContentType> categorList = new List<EnumCMSContentType>();
                    categorList.Add(EnumCMSContentType.News);
                    categorList.Add(EnumCMSContentType.AutoExpo2016);
                    string contentTypeList = CommonApiOpn.GetContentTypesString(categorList);
                    // Send HTTP GET requests 
                    HttpResponseMessage response = await client.GetAsync("webapi/article/listbycategory/?applicationid=" + ConfigurationManager.AppSettings["applicationId"] + "&categoryidlist=" + contentTypeList + "&startindex=" + _startIndex + "&endindex=" + _endIndex);

                    response.EnsureSuccessStatusCode();    // Throw if not a success code.

                    if (response.IsSuccessStatusCode) //success status 200 above Status
                    {
                        CMSContent _objArticleList = await response.Content.ReadAsAsync<CMSContent>();

                        BindNews(_objArticleList);
                        BindListPager(objPager, Convert.ToInt32(_objArticleList.RecordCount));
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                Trace.Warn(ex.Message);
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

        /// <summary>
        /// Written By : Ashwini Todkar on 24 Sept 2014
        /// PopulateWhere to bind link pager control 
        /// </summary>
        /// <param name="objPager"> Pager instance </param>
        /// <param name="recordCount"> total news available</param>
        private void BindListPager(IPager objPager, int recordCount)
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