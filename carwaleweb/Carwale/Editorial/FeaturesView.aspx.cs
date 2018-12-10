using AutoMapper;
using Carwale.DTOs.CMS.Articles;
using Carwale.Entity;
using Carwale.Entity.CMS;
using Carwale.Entity.CMS.Photos;
using Carwale.Entity.CMS.URIs;
using Carwale.Interfaces.CMS.Articles;
using Carwale.Interfaces.CMS.Photos;
using Carwale.Service;
using Carwale.UI.Common;
using Carwale.UI.Controls;
using System;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Linq;
using Microsoft.Practices.Unity;
using Carwale.Entity.Common;
using Carwale.Interfaces.CarData;
using Carwale.Utility;

namespace Carwale.UI.Editorial
{
    public class FeaturesView : System.Web.UI.Page
    {
        protected Repeater  rptFeatures;
        protected DataList dlstPhoto;
        protected Label lblHeading, lblAuthor, lblDate, lblDetails;
        protected string views = string.Empty;
        protected HtmlGenericControl topNav, bottomNav;
        protected string Url = string.Empty, PageId = "1", BasicId = string.Empty, Str = string.Empty, ArticleTitle, authorName, authorMaskingName;
        protected bool ShowGallery = false, IsPhotoGalleryPage = false;
        //protected int StrCount = 0;
        public string nextUrl = string.Empty, prevUrl = string.Empty, altURL = string.Empty;
        protected string canonicalUrl = string.Empty;
        protected DateTime displayDate;

        public PlaceHolder headMetaTags = null;
        protected SubNavigation SubNavigation;
        protected List<string> listPageTitles = new List<string>();
        public ArticlePageDetails featuresDetail;
        protected List<int> latestRelatedArticleIds;
        protected CarRightWidget carRightWidget = new CarRightWidget();
        private ICMSContent _cmsCacheRepository;
        private ICarModels _carModelBL;
        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
            using (IUnityContainer container = UnityBootstrapper.Resolver.GetContainer())
            {
                _cmsCacheRepository = container.Resolve<ICMSContent>();
                _carModelBL = container.Resolve<ICarModels>();
            }
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
            
            FillFeaturesDetail();
            ShowNavigation();
            FormCanonical();
            FillCarWidgetData();
        }


        protected void FillFeaturesDetail()
        {
            bool foundData = false;
            int Id = Convert.ToInt32(BasicId);
            
            featuresDetail = FeaturesAsyncCall(Id);

            latestRelatedArticleIds = LatestRelatedArticles(Id);

            if (featuresDetail != null)
            {
                for (int i = 0; i < featuresDetail.PageList.Count; i++)
                {
                    listPageTitles.Add(featuresDetail.PageList[i].PageName);
                }
                CarComparePageTitle = featuresDetail.Title;
                CarComparePageDesc = featuresDetail.Description;
                featuresDetail.Description = Format.GetPlainTextFromHTML(featuresDetail.Description);
                ArticleTitle = featuresDetail.Title;
                displayDate = featuresDetail.DisplayDate;
                views = featuresDetail.Views.ToString();
                Url = featuresDetail.ArticleUrl;
                ShowGallery = featuresDetail.ShowGallery;
                authorMaskingName = featuresDetail.AuthorMaskingName;
                authorName = featuresDetail.AuthorName;
                rptFeatures.DataSource = featuresDetail.PageList;
                rptFeatures.DataBind();

                if (ShowGallery)
                {
                    LoadPhotos();
                    listPageTitles.Add("Photos");
                }

                foundData = true;
            }

            if (!foundData)
            {
                Response.RedirectPermanent("/features/");
            }
        }


        protected ArticlePageDetails FeaturesAsyncCall(int id)
        {
            var articleURI = new ArticleContentURI()
            {
                BasicId = (ulong)id
            };
            var content = _cmsCacheRepository.GetContentPages(articleURI);
            if (content != null)
                return Mapper.Map<Carwale.Entity.CMS.Articles.ArticlePageDetails, ArticlePageDetails>(content);
            return new ArticlePageDetails();
        }

        protected List<int> LatestRelatedArticles(int id)
        {
            List<int> latestArticlesIds = new List<int>();
            var latestArticles = _cmsCacheRepository.GetTopArticlesByBasicId(id);
            if (latestArticles != null)
                latestArticlesIds = latestArticles.Select(x => x.BasicId).Take(2).ToList();
            return latestArticlesIds; 
        }

        private void ShowNavigation()
        {
            SubNavigation.subNavOnCarCompare = true;
            SubNavigation.PageId = "11";
            SubNavigation.rptPages.DataSource = listPageTitles;
            SubNavigation.rptPages.DataBind();
            SubNavigation.PQPageId = 50;
            SubNavigation.Category = "FeaturesPage";
        }


        private void LoadPhotos()
        {
            int basicId = Convert.ToInt32(BasicId);

            var photosList = GetPhotoAsync(basicId);

            if (photosList != null)
            {
                dlstPhoto.DataSource = photosList;
                dlstPhoto.DataBind();
            }
        }

        protected List<ModelImage> GetPhotoAsync(int basicId)
        {
            var articleURI = new ArticlePhotoUri()
            {
                basicId = (ulong)basicId
            };

            var cmsCacheRepository = UnityBootstrapper.Resolve<IPhotos>();
            return Mapper.Map<List<Carwale.Entity.CMS.Photos.ModelImage>, List<ModelImage>>(cmsCacheRepository.GetArticlePhotos(articleURI));
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

      
        protected string FormCanonical()
        {
            canonicalUrl = "https://www.carwale.com" + Url;
            return canonicalUrl;
        }

        protected void FillCarWidgetData()
        {
            Pagination page = new Pagination() { PageNo = 1, PageSize = 5 };
            carRightWidget.PopularModels = _carModelBL.GetTopSellingCarModels(page);
            carRightWidget.UpcomingCars = _carModelBL.GetUpcomingCarModels(page);
        }
    }
}