using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carwale.UI.Common;
using System.Web.UI.HtmlControls;
using Carwale.Entity.CMS;
using Carwale.Notifications;
using Carwale.UI.Controls;
using Carwale.DAL.CoreDAL;
using System.Threading.Tasks;
using System.Net.Http;
using System.Configuration;
using System.Net.Http.Headers;
using Carwale.DTOs.CMS.Articles;
using Carwale.Interfaces.CMS.Articles;
using Carwale.Service;
using Carwale.Entity.CMS.URIs;
using AutoMapper;

namespace Carwale.UI.Editorial
{
    public class TipsAndAdvicesDefault : System.Web.UI.Page
    {        
        protected Repeater rptTipsAdvices, rptUCL;
        protected DropDownList ddlUsedCarLocations, ddlMake;
        protected string canonicalUrl = string.Empty;
        protected string altURL = string.Empty;

        protected string  BaseUrl = string.Empty;

        public string nextUrl = string.Empty, prevUrl = string.Empty;

        public PlaceHolder headMetaTags = null;

        protected string pageNumber = "1";
        // private string pageNumber { get; set; }

        public string subCategoryId { get; set; }
        public string subCategoryName { get; set; }
        protected CommonPager pagerDetails;
        protected int totalPages;

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            DeviceDetection dd = new DeviceDetection(Request.ServerVariables["HTTP_X_REWRITE_URL"].ToString());
            dd.DetectDevice();

            if (Request["pn"] != null && Request.QueryString["pn"] != "")
            {
                if (CommonOpn.CheckId(Request.QueryString["pn"]) == true)
                    pageNumber = Request.QueryString["pn"];
                Trace.Warn("pn: " + Request.QueryString["pn"]);
            }
            
            subCategoryId = Request.QueryString["catid"];

            if (String.IsNullOrEmpty(subCategoryId))
                UrlRewrite.Return404();


            FillTipsList();

            CreatePrevNextUrl();
            FormCanonical();
            FormAltUrl();

            if (nextUrl != "")
            {
                HtmlLink linkNext = new HtmlLink();
                linkNext.Href = nextUrl;
                linkNext.Attributes.Add("rel", "next");
                headMetaTags.Controls.Add(linkNext);
            }

            if (prevUrl != "")
            {
                HtmlLink linkPrev = new HtmlLink();
                linkPrev.Href = prevUrl;
                linkPrev.Attributes.Add("rel", "prev");
                headMetaTags.Controls.Add(linkPrev);
            }
        }

        protected void FillTipsList()
        {
            pagerDetails.PageSize = 10;
            int startIndex = (Convert.ToInt32(pageNumber) - 1) * pagerDetails.PageSize + 1;
            int endIndex = (Convert.ToInt32(pageNumber)) * pagerDetails.PageSize;

            var objList = GetTipsAsync(startIndex, endIndex);

            CMSContent articleList = objList;

            rptTipsAdvices.DataSource = articleList.Articles;
            rptTipsAdvices.DataBind();

            subCategoryName = articleList.Articles[0].SubCategory;

            BaseUrl = "/tipsadvice/" + subCategoryName.ToLower().Replace(" ", "-") + "/";
            pagerDetails.BaseUrl = BaseUrl;
            pagerDetails.PageNo = Convert.ToInt32(pageNumber);
            pagerDetails.PageSlotSize = 10;

            pagerDetails.PageUrlType = "page/";
            pagerDetails.TotalResults = Convert.ToInt32(articleList.RecordCount);
            pagerDetails.GetPagerForCMS();

            totalPages = pagerDetails.TotalResults / pagerDetails.PageSlotSize;
            if (pagerDetails.TotalResults % pagerDetails.PageSlotSize != 0)
                totalPages++;
        }

        protected CMSContent GetTipsAsync(int startIndex, int endIndex)
        {
            CMSContent objTask = new CMSContent();
            try
            {
                ushort subCat = 0;
                ushort.TryParse(subCategoryId, out subCat);
                var bl = UnityBootstrapper.Resolve<ICMSContent>();
                ArticleBySubCatURI queryString=new ArticleBySubCatURI(){
                StartIndex=(uint)startIndex,
                EndIndex = (uint)endIndex,
                CategoryIdList = "5",
                SubCategoryId = subCat,
                ApplicationId = Convert.ToUInt16(CMSAppId.Carwale)
                };
                objTask = Mapper.Map<Carwale.Entity.CMS.Articles.CMSContent, Carwale.DTOs.CMS.Articles.CMSContent>(bl.GetContentListBySubCategory(queryString)); 
                //using (var client = new HttpClient())
                //{
                //    HttpResponseMessage _response = new HttpResponseMessage();
                //    //sets the base URI for HTTP requests
                //    string _cwHostUrl = "https://" + ConfigurationManager.AppSettings["HostUrl"];
                //    client.BaseAddress = new Uri(_cwHostUrl);

                //    client.DefaultRequestHeaders.Accept.Clear();

                //    //sets the Accept header to "application/json", which tells the server to send data in JSON format.
                //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //    //HTTP GET

                //    _response = await client.GetAsync("/webapi/article/ListBySubCategory/?applicationid=" + appId + "&categoryidlist=" + categoryid + "&subcategoryid=" + subCategoryId + "&startindex=" + startIndex + "&endindex=" + endIndex).ConfigureAwait(false);

                //    _response.EnsureSuccessStatusCode(); //Throw if not a success code.

                //    if (_response.IsSuccessStatusCode)
                //    {
                //        if (_response.StatusCode == System.Net.HttpStatusCode.OK) //Check 200 OK Status
                //            objTask = await _response.Content.ReadAsAsync<CMSContent>();

                //    }
                //}
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn(ex.Message);
            }

            return objTask;
        }


    
        /// <summary>
        /// written By:Prashant Vishe On 21 June 2013
        /// Function is Used to Create next and Prev url.
        /// </summary>
        private void CreatePrevNextUrl()
        {
           
            string mainUrl = "https://www.carwale.com/tipsadvice/" + subCategoryName.ToLower().Replace(" ", "-") + "/";
            string prevPageNumber = string.Empty, nextPageNumber = string.Empty;
            
            if (pageNumber == string.Empty || pageNumber == "1")    //if page is first page
            {
                if (pagerDetails.TotalResults > pagerDetails.PageSize)
                {
                    nextPageNumber = "2";
                    nextUrl = mainUrl + "page" + "/" + nextPageNumber + "/";
                }
            }
            else if (int.Parse(pageNumber) == totalPages)    //if page is last page
            {
                prevPageNumber = (int.Parse(pageNumber) - 1).ToString();
                prevUrl = mainUrl + "page" + "/" + prevPageNumber + "/";
            }
            else
            {          //for middle pages
                prevPageNumber = (int.Parse(pageNumber) - 1).ToString();
                prevUrl = mainUrl + "page" + "/" + prevPageNumber + "/";
                nextPageNumber = (int.Parse(pageNumber) + 1).ToString();
                nextUrl = mainUrl + "page" + "/" + nextPageNumber + "/";
            }
        }
        protected string FormCanonical()
        {
            if (pageNumber == "" || pageNumber == "1")
                canonicalUrl = "https://www.carwale.com/tipsadvice/" + subCategoryName.ToLower().Replace(" ", "-") + "/";
            else
                canonicalUrl = "https://www.carwale.com/tipsadvice/" + subCategoryName.ToLower().Replace(" ", "-") + "/" + "page" + "/" + pageNumber + "/";
            return canonicalUrl;
        }
        protected string FormAltUrl()
        {
            if (pageNumber == "" || pageNumber == "1")
                altURL = "https://www.carwale.com/m/tipsadvice/" + subCategoryName.ToLower().Replace(" ", "-") + "/";
            else
                altURL = "https://www.carwale.com/m/tipsadvice/" + subCategoryName.ToLower().Replace(" ", "-") + "/" + "page" + "/" + pageNumber + "/";
            return altURL;
        }
    }
}