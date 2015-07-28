using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bikewale.Common;
using Microsoft.Practices.Unity;
using Bikewale.Interfaces.UserReviews;
using Bikewale.BAL.UserReviews;
using Bikewale.Entities.UserReviews;

namespace Bikewale.Mobile.Content
{
    /// <summary>
    /// Created By : Ashwini Todkar on 9 May 2014
    /// Summary    : Class to manage bike reviews
    /// </summary>
    public class Default : System.Web.UI.Page
    {
        protected DropDownList ddlMake;
        protected LinkButton btnSubmit;
        protected Repeater rptMostReviewed, rptMostRead, rptMostHelpful, rptMostRecent, rptMostRated;
        private   List<ReviewTaggedBikeEntity> objMostReviewed = null;
        private IUserReviews objUserReviews = null;

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindMake();
                RegisterContainer();
                GetMostReviewedBikeList();
                GetMostReadReviews();
                GetMostHelpfulReviews();
                GetMostRecentReviews();
                GetMostRatedReviews();
            }
        }

        private void GetMostRecentReviews()
        {
            List<ReviewsListEntity> objMostRecent = objUserReviews.GetMostRecentReviews(10);
            rptMostRecent.DataSource = objMostRecent;
            rptMostRecent.DataBind();
        }

        private void GetMostHelpfulReviews()
        {
            List<ReviewsListEntity> objMostHelpful = objUserReviews.GetMostHelpfulReviews(10);
            rptMostHelpful.DataSource = objMostHelpful;
            rptMostHelpful.DataBind();
        }

        private void GetMostRatedReviews()
        {
            List<ReviewsListEntity> objMostRated = objUserReviews.GetMostRatedReviews(10);
            rptMostRated.DataSource = objMostRated;
            rptMostRated.DataBind();
        }

        private void GetMostReadReviews()
        {
            List<ReviewsListEntity> objMostRead = objUserReviews.GetMostReadReviews(10);
            rptMostRead.DataSource = objMostRead;
            rptMostRead.DataBind();
        }

        private void RegisterContainer()
        {
 	        using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<IUserReviews, UserReviews>();

                objUserReviews = container.Resolve<IUserReviews>();
            }
        }

        private void GetMostReviewedBikeList()
        {
            objMostReviewed = objUserReviews.GetMostReviewedBikesList(10);
            rptMostReviewed.DataSource = objMostReviewed;
            rptMostReviewed.DataBind();
        }

        private void BindMake()
        {
            try
            {
                MakeModelVersion mmv = new MakeModelVersion();

                ddlMake.DataSource = mmv.GetMakes("USERREVIEW");
                ddlMake.DataValueField = "Value";
                ddlMake.DataTextField = "Text";
                ddlMake.DataBind();
                ddlMake.Items.Insert(0, (new ListItem("--Select Make--", "0")));
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }
    }
}