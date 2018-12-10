using Carwale.Entity.Author;
using Carwale.Entity.CMS;
using Carwale.Interfaces.Author;
using Carwale.Notifications;
using Carwale.Service;
using Carwale.UI.Common;
using Carwale.UI.Controls;
using System;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace Carwale.UI.Authors
{
    public class AuthorDetails : System.Web.UI.Page
    {
        private int authorId = -1;
        protected string authorName = "", designation = "", profileImage = "", fullDescription = "", altURL = "";
        protected string emailId = "", facebookProfile = "", googlePlusProfile = "", linkedInProfile = "", twitterProfile = "", maskingName = "", imageName = "", hostUrl = "";
        protected Repeater rptExpertReview, rptNewsList, rptOtherAuthors;
        protected NewsRightWidget ctrlNewsRightWidget;
        protected PopularVideoWidget ctrlPopularVideoWidget;


        protected override void OnInit(EventArgs e)
        {

            InitializeComponent();
        }

        void InitializeComponent()
        {
            base.Load += new EventHandler(this.Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                if (Request.QueryString["authorId"] != null && Request.QueryString["authorId"] != string.Empty && CommonOpn.CheckId(Request.QueryString["authorId"]) == true)
                {
                    DeviceDetection dd = new DeviceDetection(Request.ServerVariables["HTTP_X_REWRITE_URL"].ToString());
                    dd.DetectDevice();
                    authorId = Convert.ToInt16(Request.QueryString["authorId"]);
                    FillAuthorDetails();
                    FillExpertReview(authorId);
                    FillNewsPanel(authorId);
                    FillOtherAuthorsPanel(authorId);
                    FillRightPanel();
                    altURL = "/authors/" + maskingName;
                }
                else
                {
                    Response.Redirect("/authors/");
                }
            }
        }

        /// <summary>
        /// Populates the author details
        /// </summary>
        private void FillAuthorDetails()
        {
            IAuthorRepository authorContainer = UnityBootstrapper.Resolve<IAuthorRepository>();

            try
            {
                AuthorEntity authorDetails = authorContainer.GetAuthorDetails(authorId);
                authorName = authorDetails.AuthorName;
                designation = authorDetails.Designation;
                profileImage = authorDetails.ProfileImage;
                fullDescription = authorDetails.FullDescription;
                emailId = authorDetails.EmailId;
                facebookProfile = authorDetails.FacebookProfile;
                googlePlusProfile = authorDetails.GooglePlusProfile;
                linkedInProfile = authorDetails.LinkedInProfile;
                twitterProfile = authorDetails.TwitterProfile;
                maskingName = authorDetails.MaskingName;
                imageName = authorDetails.ImageName;
                hostUrl = authorDetails.HostUrl;
            }

            catch (SqlException err)
            {
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

        /// <summary>
        /// This function creates the URL for expert review list specific to the author's page.
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="makeName"></param>
        /// <param name="maskingName"></param>
        /// <param name="url"></param>
        /// <param name="basicId"></param>
        /// <returns></returns>
        protected string FormURL(string categoryId, string makeName, string maskingName, string url, string basicId)
        {
            string finalUrl = string.Empty;
            if (categoryId == "8")
            {
                finalUrl = UrlRewrite.FormatSpecial(makeName) + "-cars/" + maskingName + "/expert-reviews-" + basicId + "/";
            }
            else if (categoryId == "2")
            {
                finalUrl = "expert-reviews/" + url + "-" + basicId + "/";
            }
            return finalUrl.ToString();
        }


        /// <summary>
        /// Populates the expert reviews panel 
        /// </summary>
        /// <param name="authorId"></param>
        private void FillExpertReview(int authorId)
        {
            try
            {
                IAuthorRepository authorContainer = UnityBootstrapper.Resolve<IAuthorRepository>();
                rptExpertReview.DataSource = authorContainer.GetExpertReviewsByAuthor(authorId, CMSAppId.Carwale);
                rptExpertReview.DataBind();
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }


        /// <summary>
        /// Populates the news panel 
        /// </summary>
        /// <param name="authorId"></param>
        private void FillNewsPanel(int authorId)
        {
            try
            {
                IAuthorRepository authorContainer = UnityBootstrapper.Resolve<IAuthorRepository>();
                rptNewsList.DataSource = authorContainer.GetNewsByAuthor(authorId, CMSAppId.Carwale);
                rptNewsList.DataBind();
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }


        /// <summary>
        /// Populates the Other Authors Panel 
        /// </summary>
        /// <param name="authorId"></param>
        private void FillOtherAuthorsPanel(int authorId)
        {
            try
            {
                IAuthorRepository authorContainer = UnityBootstrapper.Resolve<IAuthorRepository>();
                rptOtherAuthors.DataSource = authorContainer.GetAllOtherAuthors(authorId);
                rptOtherAuthors.DataBind();
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }


        /// <summary>
        ///Populates the right panel of popular,recent news and videos 
        /// </summary>
        private void FillRightPanel()
        {
            try
            {
                ctrlNewsRightWidget.NumberofRecords = 5;
                ctrlNewsRightWidget.CategoryId = 1;
                ctrlNewsRightWidget.BasicId = 1;
                ctrlNewsRightWidget.PopulateNewsWidget();
                ctrlPopularVideoWidget.Tag = "";
                ctrlPopularVideoWidget.populateVideoWidget();
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }
    }
}