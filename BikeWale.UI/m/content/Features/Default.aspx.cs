using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Practices.Unity;
using Bikewale.Entities.CMS;
using Bikewale.Interfaces.CMS;
using Bikewale.BAL.CMS;
using Bikewale.Interfaces.Pager;
using Bikewale.BAL.Pager;
using Bikewale.Entities.Pager;
using Bikewale.Common;
using Bikewale.Mobile.Controls;
using System.Net.Http;
using System.Configuration;
using System.Net.Http.Headers;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Utility;

namespace Bikewale.Mobile.Content
{
    /// <summary>
    /// Created By : Ashwini Todkar on 20 May 2014
    /// Modified By : Ashwini Todkar on 30 Sept 2014
    /// </summary>
    public class Features : System.Web.UI.Page
    {
        protected Repeater rptFeatures;
        protected ListPagerControl listPager;
        protected int curPageNo = 1;
        protected string prevPageUrl = String.Empty, nextPageUrl = String.Empty;
        private const int _pageSize = 10;
        private bool _isContentFound = true;

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!String.IsNullOrEmpty(Request.QueryString["pn"]))
                    Int32.TryParse(Request.QueryString["pn"], out curPageNo);
                     
                GetFeaturesList();
            }
        }
        /// <summary>
        /// Written By : Ashwini Todkar on 30 Sept 2014
        /// Summary    : PopulateWhere to get features list from web api asynchronously
        /// </summary>
        private async void GetFeaturesList()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    //uint _totalPages = 0;
                    //sets the base URI for HTTP requests
                    string _cwHostUrl = ConfigurationManager.AppSettings["cwApiHostUrl"];
                    client.BaseAddress = new Uri(_cwHostUrl);

                    //sets the Accept header to "application/json", which tells the server to send data in JSON format.
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    // get pager instance
                    IPager objPager = GetPager();

                    int _startIndex = 0, _endIndex = 0;// _featuresCategoryId = (int)EnumCMSContentType.Features;

                    objPager.GetStartEndIndex(_pageSize, curPageNo, out _startIndex, out _endIndex);

                    List<EnumCMSContentType> categorList = new List<EnumCMSContentType>();
                    categorList.Add(EnumCMSContentType.Features);
                    categorList.Add(EnumCMSContentType.SpecialFeature);
                    string _featuresCategoryId = CommonApiOpn.GetContentTypesString(categorList);

                    // Send HTTP GET requests 
                    HttpResponseMessage response = await client.GetAsync("webapi/article/listbycategory/?applicationid=" + ConfigurationManager.AppSettings["applicationId"] + "&categoryidlist=" + _featuresCategoryId + "&startindex=" + _startIndex + "&endindex=" + _endIndex);

                    response.EnsureSuccessStatusCode();    // Throw if not a success code.

                    if (response.IsSuccessStatusCode) //success status 200 above Status
                    {
                        var _objFeaturesList = await response.Content.ReadAsAsync<CMSContent>();

                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            int _totalPages = objPager.GetTotalPages(Convert.ToInt32(_objFeaturesList.RecordCount), _pageSize);

                            BindFeatures(_objFeaturesList);
                            BindLinkPager(objPager, Convert.ToInt32(_objFeaturesList.RecordCount), _totalPages);
                        }
                        else
                        {
                            _isContentFound = false;
                        }
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
            finally
            {
                if (!_isContentFound)
                {
                    Response.Redirect("/m/pagenotfound.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                }
            }
        }

        private void BindLinkPager(IPager objPager, int recordCount, int totalPages)
        {           
            PagerEntity pagerEntity = new PagerEntity();

            pagerEntity.BaseUrl = "/m/features/";
            pagerEntity.PageNo = curPageNo;
            pagerEntity.PagerSlotSize = totalPages;
            pagerEntity.PageUrlType = "page/";
            pagerEntity.TotalResults = recordCount;
            pagerEntity.PageSize = _pageSize;

            PagerOutputEntity pagerOutput = objPager.GetPager<PagerOutputEntity>(pagerEntity);

            listPager.PagerOutput = pagerOutput;
            listPager.TotalPages = totalPages;
            listPager.CurrentPageNo = curPageNo;
            listPager.BindPageNumbers();

            //get next and prev page links for SEO
            prevPageUrl = pagerOutput.PreviousPageUrl;
            nextPageUrl = pagerOutput.NextPageUrl;       
        }

        private void BindFeatures(CMSContent _objFeaturesList)
        {
            rptFeatures.DataSource = _objFeaturesList.Articles;
            rptFeatures.DataBind();
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

        //private void GetFeatures()
        //{
        //    try
        //    {
        //        using (IUnityContainer container = new UnityContainer())
        //        {
        //            ContentFilter filter = new ContentFilter();

        //            container.RegisterType<ICMSContentRepository<CMSContentListEntity, CMSPageDetailsEntity>, CMSData<CMSContentListEntity, CMSPageDetailsEntity>>();
        //            ICMSContentRepository<CMSContentListEntity, CMSPageDetailsEntity> repository = container.Resolve<ICMSContentRepository<CMSContentListEntity, CMSPageDetailsEntity>>(new ResolverOverride[] { new ParameterOverride("contentType", EnumCMSContentType.Features) });

        //            container.RegisterType<IPager, Pager>();
        //            IPager objPager = container.Resolve<IPager>();

        //            int startIndex = 0, endIndex = 0, pageSize = 10, recordCount = 0;
        //            objPager.GetStartEndIndex(pageSize, curPageNo, out startIndex, out endIndex);

        //            //get page wise features list
        //            IList<CMSContentListEntity> t = repository.GetContentList(startIndex, endIndex, out recordCount, filter);

        //            if (t.Count > 0)
        //            {
        //                rptFeatures.DataSource = t;
        //                rptFeatures.DataBind();
        //            }

        //            totalPages = objPager.GetTotalPages(recordCount, pageSize);

        //            //if current page number exceeded the total pages count i.e. the page is not available 
        //            if (curPageNo > totalPages)
        //            {
        //                Response.Redirect("/m/pagenotfound.aspx", false);
        //                HttpContext.Current.ApplicationInstance.CompleteRequest();
        //                this.Page.Visible = false;
        //            }
        //            else
        //            {
        //                PagerEntity pagerEntity = new PagerEntity();
        //                pagerEntity.BaseUrl = "/m/features/";
        //                pagerEntity.PageNo = curPageNo;
        //                pagerEntity.PagerSlotSize = totalPages;
        //                pagerEntity.PageUrlType = "page/";
        //                pagerEntity.TotalResults = recordCount;
        //                pagerEntity.PageSize = pageSize;

        //                PagerOutputEntity pagerOutput = objPager.GetPager<PagerOutputEntity>(pagerEntity);

        //                listPager.PagerOutput = pagerOutput;
        //                listPager.TotalPages = totalPages;
        //                listPager.CurrentPageNo = curPageNo;

        //                //get next and prev page links for SEO
        //                prevPageUrl = pagerOutput.PreviousPageUrl;
        //                nextPageUrl = pagerOutput.NextPageUrl;
        //            }
        //        }
        //    }
        //    catch (Exception err)
        //    {
        //        Trace.Warn(err.Message);
        //        ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
        //        objErr.SendMail();
        //    }
        //}
    }
}