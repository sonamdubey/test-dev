using Carwale.Entity.Author;
using Carwale.Entity.CMS;
using Carwale.Interfaces.Author;
using Carwale.Notifications;
using Carwale.Service;
using Carwale.Utility;
using MobileWeb.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MobileWeb.Authors
{
    public class AuthorDetails : System.Web.UI.Page
    {
        protected string authorName = "", profileImage = "", emailId = "", fullDescription = "",hostUrl = "",imageName = "";
        protected string facebookProfile = "", googlePlusProfile = "", linkedInProfile = "", twitterProfile = "";
        protected Repeater rptExptReviews, rptArticles, rptOtherAuthors, rptAllOtherAuthors;
        int authorId;

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
        }

        void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Request.QueryString["authorId"] != null && Request.QueryString["authorId"] != "" && CommonOpn.CheckId(Request.QueryString["authorId"]))
                {
                    Trace.Warn("authorid : " + Request.QueryString["authorId"].ToString());
                    authorId = Convert.ToInt32(Request.QueryString["authorId"].ToString());
                    GetAuthorDetails(authorId);
                    GetExpertReviewsByAuthor(authorId);
                    GetArticlesByAuthor(authorId);
                    GetOtherAuthors(authorId);
                }
            }
        }

        /// <summary>
        /// Created BY : Supriya Khartode on 31/7/2014
        /// Desc : Get the details of author
        /// </summary>
        private void GetAuthorDetails(int authorId)
        {
            IAuthorRepository authorContainer = UnityBootstrapper.Resolve<IAuthorRepository>();
            try
            {
                AuthorEntity authorDetails = authorContainer.GetAuthorDetails(authorId);
                authorName = authorDetails.AuthorName;
                profileImage = authorDetails.ProfileImage;
                emailId = authorDetails.EmailId;
                fullDescription = authorDetails.FullDescription;
                facebookProfile = authorDetails.FacebookProfile;
                googlePlusProfile = authorDetails.GooglePlusProfile;
                linkedInProfile = authorDetails.LinkedInProfile;
                twitterProfile = authorDetails.TwitterProfile;
                hostUrl = authorDetails.HostUrl;
                imageName = authorDetails.ImageName;
            }
            catch(Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "AuthorDetails.GetAuthorDetails()");
                objErr.LogException();
            }
        }

        /// <summary>
        /// Created BY : Supriya Khartode on 31/7/2014
        /// Desc : Get the expert reviews according to authorid passed
        /// </summary>
        private void GetExpertReviewsByAuthor(int authorId)
        {
            IAuthorRepository authorContainer = UnityBootstrapper.Resolve<IAuthorRepository>();
            try
            {
                List<ExpertReviews> expertReviews = authorContainer.GetExpertReviewsByAuthor(authorId,CMSAppId.Carwale);
                rptExptReviews.DataSource = expertReviews;
                rptExptReviews.DataBind();
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "AuthorDetails.GetExpertReviews()");
                objErr.LogException();
            }
        }

        /// <summary>
        /// Created BY : Supriya Khartode on 31/7/2014
        /// Desc : Get the news according to authorid passed
        /// </summary>
        private void GetArticlesByAuthor(int authorId)
        {
            IAuthorRepository authorContainer = UnityBootstrapper.Resolve<IAuthorRepository>();
            try
            {
                List<NewsEntity> news = authorContainer.GetNewsByAuthor(authorId,CMSAppId.Carwale);
                rptArticles.DataSource = news;
                rptArticles.DataBind();
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "AuthorDetails.GetArticlesByAuthor()");
                objErr.LogException();
            }
        }

        /// <summary>
        /// Created BY : Supriya Khartode on 31/7/2014
        /// Desc : Get the list of all authors
        /// </summary>
        private void GetOtherAuthors(int authorId)
        {
            IAuthorRepository authorContainer = UnityBootstrapper.Resolve<IAuthorRepository>();
            try
            {
                List<AuthorList> otherAuthors = authorContainer.GetAllOtherAuthors(authorId);
                rptOtherAuthors.DataSource = otherAuthors.Take(4);
                rptOtherAuthors.DataBind();

                rptAllOtherAuthors.DataSource = otherAuthors.Skip(4).Take(otherAuthors.Count);
                rptAllOtherAuthors.DataBind();
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "AuthorDetails.GetOtherAthours()");
                objErr.LogException();
            }
        }

        //Form Url for road test and comparison test
        protected string FormURL(int categoryId, string makeName, string maskingName, string url, int basicId)
        {
            string finalUrl = string.Empty;
            if (categoryId == 8)
            {
                finalUrl = Format.RemoveSpecialCharacters(makeName) + "-cars/" + maskingName + "/expert-reviews-" + basicId + "/";
            }
            else if (categoryId == 2)
            {
                finalUrl = "expert-reviews/" + url + "-" + basicId + "/";
            }
            return finalUrl.ToString();
        }
    }
}