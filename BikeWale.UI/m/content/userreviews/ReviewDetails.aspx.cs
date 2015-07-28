using Bikewale.BAL.UserReviews;
using Bikewale.Common;
using Bikewale.Entities.UserReviews;
using Bikewale.Interfaces.UserReviews;
using Bikewale.Memcache;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Bikewale.Mobile.Content
{
    /// <summary>
    /// Created By  : Ashwini Todkar on 9th May 2014
    /// </summary>
    public class ReviewDetails : System.Web.UI.Page
    {
        protected String nextPageUrl = String.Empty, prevPageUrl = String.Empty;
        private IUserReviews objUserReviews = null;
        protected ReviewDetailsEntity objReview = null;
        private uint reviewId = 0;


        protected override void OnInit(EventArgs e)
        {
             this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    if (ProcessQS())
                    {
                        RegistorContainer();
                        GetReviewDetails();
                    }
                    else
                    {
                        Response.Redirect("/m/pagenotfound.aspx", false);
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                        this.Page.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

        private bool ProcessQS()
        {
            bool isSucess = true;

            if (!String.IsNullOrEmpty(Request.QueryString["rid"]))
            {
                if (!UInt32.TryParse(Request.QueryString["rid"], out reviewId))
                {
                    isSucess = false;
                }
            }
            else
            {
                isSucess = false;
            }
            return isSucess;
        }

        private void GetReviewDetails()
        {
            objReview = objUserReviews.GetReviewDetails(reviewId);

            if (objReview != null)
            {
                GetNextPrevLinks();
            }
            else
            {
                //Sets the page for redirect, but does not abort.
                Response.Redirect("/m/pagenotfound.aspx", false);
                // Causes ASP.NET to bypass all events and filtering in the HTTP pipeline
                // chain of execution and directly execute the EndRequest event.
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                // By setting this to false, we flag that a redirect is set,
                // and to not render the page contents.
                this.Page.Visible = false;            
            }
        }

        private void GetNextPrevLinks()
        {
            if (objReview.NextReviewId != 0)
                nextPageUrl = "/m/" + objReview.BikeEntity.MakeEntity.MaskingName + "-bikes/" + objReview.BikeEntity.ModelEntity.MaskingName + "/user-reviews/" + objReview.NextReviewId.ToString() + ".html";

            if (objReview.PrevReviewId != 0 )
                prevPageUrl ="/m/" + objReview.BikeEntity.MakeEntity.MaskingName + "-bikes/" + objReview.BikeEntity.ModelEntity.MaskingName + "/user-reviews/" +  objReview.PrevReviewId.ToString() + ".html";
        }

        private void RegistorContainer()
        {
            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<IUserReviews, UserReviews>();

                objUserReviews = container.Resolve<IUserReviews>();
            }
        }
    }
}