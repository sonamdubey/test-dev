using AutoMapper;
using Carwale.DTOs.CMS.Articles;
using Carwale.Entity.CMS.URIs;
using Carwale.Interfaces.CMS.Articles;
using Carwale.Service;
using Carwale.UI.Common;
using Carwale.Utility;
using System;

namespace Carwale.UI.Editorial
{
    public class PitStopDetails : System.Web.UI.Page
    {

        public string FbCommentCount = "0", title = "", url = "", authorName = "", authorMaskingName, content = "", displayDate = "", views = "0", HostUrl = string.Empty, MainImgCaption = string.Empty;
        public bool IsPreview = false;
        protected string CustomerId = string.Empty, PageNumber = string.Empty;
        protected string tag = string.Empty, prevNewsTitle = string.Empty, prevNewsUrl = string.Empty, nextNewsTitle = string.Empty, nextNewsUrl = string.Empty, prevId, nextId;
        protected string OriginalImgUrl = string.Empty;
        public string ImagePath = string.Empty;
        public ArticleDetails pitStopDetails;
        //protected HtmlGenericControl prevArticle, nextArticle;

        protected ulong pitstopId;

        protected override void OnInit(EventArgs e)
        {

            InitializeComponent();
        }


        void InitializeComponent()
        {
            base.Load += new EventHandler(this.Page_Load);
        }


        void Page_Load(object Sender, EventArgs e)
        {
            DeviceDetection dd = new DeviceDetection(Request.ServerVariables["HTTP_X_REWRITE_URL"].ToString());
            dd.DetectDevice();

            if (Request["pn"] != null && Request.QueryString["pn"] != "")
            {
                if (CommonOpn.CheckId(Request.QueryString["pn"]) == true)
                    PageNumber = Request.QueryString["pn"];
            }

            string newsTitle = string.Empty;

            if (Request["prw"] != null && Request.QueryString["prw"] != string.Empty)
            {
                IsPreview = Request.QueryString["prw"] == "1" ? true : false;
            }

            if (Request["id"] != null && Request.QueryString["id"] != string.Empty && Request.QueryString["id"] != "0")
            {
                if (CommonOpn.CheckId(Request.QueryString["id"]) == true)
                    ulong.TryParse(Request.QueryString["id"], out pitstopId);

                
                FillPitStopDetails();                

                if (CheckTitleTamper())
                {
                    RedirectToListPage();
                }               
            }
            else
            {
                RedirectToListPage();
            }

            ImagePath = ImageSizes.CreateImageUrl(HostUrl, ImageSizes._110X61, OriginalImgUrl);

        }


        private bool CheckTitleTamper()
        {
            return url != Request.RawUrl;
        }

        private void FillPitStopDetails()
        {
            string prevNewsUrlSub = string.Empty, nextNewsUrlSub = string.Empty;


            var objPitStopDetail = GetPitStopDetailAsync(pitstopId);// Bind  data

            pitStopDetails = objPitStopDetail;


            bool isPreview = IsPreview ? false : true;
            if (pitStopDetails != null)
            {

                // Get pitstop Details
                tag = string.Join(",", pitStopDetails.TagsList.ToArray());
                authorName = pitStopDetails.AuthorName;
                authorMaskingName = pitStopDetails.AuthorMaskingName;
                displayDate = pitStopDetails.DisplayDate.ToString("f");
                title = pitStopDetails.Title;
                url = pitStopDetails.ArticleUrl;
                views = pitStopDetails.Views.ToString();
                content = pitStopDetails.Content;
                HostUrl = pitStopDetails.HostUrl;
                OriginalImgUrl = pitStopDetails.OriginalImgUrl;
                MainImgCaption = pitStopDetails.MainImgCaption;
                FbCommentCount = pitStopDetails.FacebookCommentCount.ToString();

                // Next, Previous new articles
                nextId = pitStopDetails.NextArticle.BasicId.ToString();
                prevId = pitStopDetails.PrevArticle.BasicId.ToString();
                nextNewsUrlSub = pitStopDetails.NextArticle.ArticleUrl;
                prevNewsUrlSub = pitStopDetails.PrevArticle.ArticleUrl;
                nextNewsTitle = pitStopDetails.NextArticle.Title;
                prevNewsTitle = pitStopDetails.PrevArticle.Title;
                prevNewsUrl = prevNewsUrlSub;
                nextNewsUrl = nextNewsUrlSub;
            }

            else
            {
                RedirectToListPage();
            }
        }


        protected ArticleDetails GetPitStopDetailAsync(ulong basicId)
        {
            var articleURI = new ArticleContentURI()
            {
                BasicId = basicId
            };

            var cmsCacheRepository = UnityBootstrapper.Resolve<ICMSContent>();
            return Mapper.Map<Carwale.Entity.CMS.Articles.ArticleDetails, ArticleDetails>(cmsCacheRepository.GetContentDetails(articleURI));                                   
        }

        void RedirectToListPage()
        {
            Response.RedirectPermanent("/pitstop/");
        }
    } // class
} // namespace

