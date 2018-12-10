using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Carwale.UI.Common;
using Carwale.Entity.CMS;
using Carwale.Notifications;
using Carwale.DAL.CoreDAL;
using System.Threading.Tasks;
using System.Net.Http;
using System.Configuration;
using System.Net.Http.Headers;
using System.Collections.Generic;
using Carwale.UI.Controls;
using Carwale.Entity.CMS.Photos;
using Carwale.DTOs.CMS.Articles;
using Carwale.Interfaces.CMS.Articles;
using Carwale.Service;
using AutoMapper;
using Carwale.Entity.CMS.URIs;
using Carwale.Interfaces.CMS.Photos;

namespace Carwale.UI.Editorial
{
    public class TipsAndAdviceView : System.Web.UI.Page
    {
        protected Repeater rptTips;
        protected DataList dlstPhoto;
        protected Label lblHeading, lblAuthor, lblDate, lblDetails;
        protected HtmlGenericControl topNav, bottomNav;
        protected string Url = string.Empty, PageId = "1", BasicId = string.Empty, Str = string.Empty, ArticleTitle, nextUrl = string.Empty, prevUrl = string.Empty, ApplicationId = string.Empty;
        protected bool ShowGallery = false, IsPhotoGalleryPage = false;
        protected int StrCount = 0, count = 0;
        protected string canonicalUrl = string.Empty, authorMaskingName, views, authorName;
        protected string altURL = string.Empty;
        public PlaceHolder headMetaTags = null;
        protected List<string> listPageTitles = new List<string>();
        protected DateTime displayDate;
        protected SubNavigation SubNavigation;
        protected ArticlePageDetails tipsDetail;


        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
        }

        void Page_Load(object Sender, EventArgs e)
        {
           
            DeviceDetection dd = new DeviceDetection(Request.ServerVariables["HTTP_X_REWRITE_URL"].ToString());
            dd.DetectDevice();

            if (Request["pageId"] != null && Request.QueryString["pageId"].ToString() != "")
            {
                PageId = Request.QueryString["pageId"].ToString();

                if (CommonOpn.CheckId(PageId) == false)
                {
                    return;
                }
            }

            if (Request.QueryString["id"] != null && Request.QueryString["id"].ToString() != "" && CommonOpn.CheckId(Request.QueryString["id"]))
            {
                BasicId = Request.QueryString["id"].ToString();
            }
            else
            {
                BasicId = "0";
            }


            FillTipsDetail();
            ShowNavigation();
            FormCanonical();
            FormAltUrl();
            
        }


        private void ShowNavigation()
        {
            SubNavigation.subNavOnCarCompare = true;
            SubNavigation.PageId = "11";
            SubNavigation.rptPages.DataSource = listPageTitles;
            SubNavigation.rptPages.DataBind();
            SubNavigation.PQPageId = 52;
            SubNavigation.Category = "TipsAndAdvicePage";
        }

        protected void FillTipsDetail()
        {
            bool foundData = false;
            int Id = Convert.ToInt32(BasicId);
            var objTipsDetail = TipsDetailAsyncCall(Id);

            tipsDetail = objTipsDetail;
           
            if (tipsDetail != null)
            {
                for (int i = 0; i < tipsDetail.PageList.Count; i++)
                {
                    listPageTitles.Add(tipsDetail.PageList[i].PageName);
                }
                CarComparePageTitle = tipsDetail.Title;
                CarComparePageDesc = tipsDetail.Description;
                ArticleTitle = tipsDetail.Title;
                displayDate = tipsDetail.DisplayDate;
                views = tipsDetail.Views.ToString();
                Url = tipsDetail.ArticleUrl;
                ShowGallery = tipsDetail.ShowGallery;
                authorMaskingName = tipsDetail.AuthorMaskingName;
                authorName = tipsDetail.AuthorName;
                rptTips.DataSource = tipsDetail.PageList;
                rptTips.DataBind();

                if (ShowGallery)
                {
                    LoadPhotos();
                    listPageTitles.Add("Photos");
                }

                foundData = true;
            }

            if (!foundData)
            {
                Response.RedirectPermanent("/tipsadvice/");
            }
        }

        protected ArticlePageDetails TipsDetailAsyncCall(int id)
        {
            ArticlePageDetails objDetail = null;
            var bl = UnityBootstrapper.Resolve<ICMSContent>();
            objDetail = Mapper.Map<Carwale.Entity.CMS.Articles.ArticlePageDetails, Carwale.DTOs.CMS.Articles.ArticlePageDetails>(bl.GetContentPages(new Entity.CMS.URIs.ArticleContentURI() { BasicId = (ulong)id }));

            //using (HttpClient client = new HttpClient())
            //{
            //    //sets the base URI for HTTP requests
            //    string _cwHostUrl = "https://" + ConfigurationManager.AppSettings["HostUrl"];
            //    client.BaseAddress = new Uri(_cwHostUrl);

            //    //sets the Accept header to "application/json", which tells the server to send data in JSON format.
            //    client.DefaultRequestHeaders.Accept.Clear();
            //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //    // Send HTTP GET requests 
            //    HttpResponseMessage response = await client.GetAsync("/webapi/article/ContentPageDetail/?basicid=" + id).ConfigureAwait(false);

            //    response.EnsureSuccessStatusCode();    // Throw if not a success code.

            //    if (response.IsSuccessStatusCode) //success status 200 above Status
            //    {
            //        if (response.StatusCode == System.Net.HttpStatusCode.OK)
            //        {
            //            objDetail = await response.Content.ReadAsAsync<ArticlePageDetails>();
            //        }
            //    }
            //}
            return objDetail;
        }

        private void LoadPhotos()
        {
            int basicId = Convert.ToInt32(BasicId);

            var objList = GetPhotoAsync(basicId);
            List<ModelImage> photoList = objList;

            dlstPhoto.DataSource = photoList;
            dlstPhoto.DataBind();

        }

        protected List<ModelImage> GetPhotoAsync(int basicId)
        {
            List<ModelImage> objDetail = new List<ModelImage>();
            try
            {
                UnityBootstrapper.Resolve<IPhotos>().GetArticlePhotos(new ArticlePhotoUri() { basicId = (ulong)basicId });
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return objDetail;
        }

        protected string FormCanonical()
        {
            canonicalUrl = "https://www.carwale.com" + Url;
            return canonicalUrl;
        }
        protected string FormAltUrl()
        {
            altURL = "https://www.carwale.com/m/" + Url;
            return altURL;
        }

        protected string GetImagePath(string articleId, string photoId, string imgSize, string hostUrl)
        {
            return ImagingFunctions.GetImagePath("/ec/", hostUrl) + articleId + "/img/" + imgSize + "/" + photoId + ".jpg";
        }

        public string CarComparePageTitle
        {
            get
            {
                if (ViewState["CarComparePageTitle"] != null)
                    return ViewState["CarComparePageTitle"].ToString();
                else
                    return "";
            }
            set { ViewState["CarComparePageTitle"] = value; }
        }

        public string CarComparePageDesc
        {
            get
            {
                if (ViewState["CarComparePageDesc"] != null)
                    return ViewState["CarComparePageDesc"].ToString();
                else
                    return "";
            }
            set { ViewState["CarComparePageDesc"] = value; }
        }

       
        public string oem
        {
            get
            {
                if (ViewState["oem"] != null)
                    return ViewState["oem"].ToString();
                else
                    return "";
            }
            set { ViewState["oem"] = value; }
        }

        public string bodyType
        {
            get
            {
                if (ViewState["bodyType"] != null)
                    return ViewState["bodyType"].ToString();
                else
                    return "";
            }
            set { ViewState["bodyType"] = value; }
        }

        public string subSegment
        {
            get
            {
                if (ViewState["subSegment"] != null)
                    return ViewState["subSegment"].ToString();
                else
                    return "";
            }
            set { ViewState["subSegment"] = value; }
        }

    }
}