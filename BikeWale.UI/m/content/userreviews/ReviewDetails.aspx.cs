using Bikewale.BAL.UserReviews;
using Bikewale.Common;
using Bikewale.Entities.UserReviews;
using Bikewale.Interfaces.UserReviews;
using Microsoft.Practices.Unity;
using MySql.CoreDAL;
using System;
using System.Data;
using System.Data.Common;
using System.Web;

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
        public static string URV
        {
            get
            {
                string val = "";

                if (HttpContext.Current.Request.Cookies["URV"] != null &&
                    HttpContext.Current.Request.Cookies["URV"].Value.ToString() != "")
                {
                    val = HttpContext.Current.Request.Cookies["URV"].Value.ToString();
                }
                else
                    val = "";

                return val;
            }
            set
            {
                HttpCookie objCookie;
                objCookie = new HttpCookie("URV");
                objCookie.Value = value;
                HttpContext.Current.Response.Cookies.Add(objCookie);
            }
        }

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
            GetDetails();
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

            if (objReview.PrevReviewId != 0)
                prevPageUrl = "/m/" + objReview.BikeEntity.MakeEntity.MaskingName + "-bikes/" + objReview.BikeEntity.ModelEntity.MaskingName + "/user-reviews/" + objReview.PrevReviewId.ToString() + ".html";
        }

        private void RegistorContainer()
        {
            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<IUserReviews, UserReviews>();

                objUserReviews = container.Resolve<IUserReviews>();
            }
        }
        /// <summary>
        ///  To check if review has already been viewed previously
        /// </summary>
        /// <param name="reviewId"></param>
        /// <returns></returns>
        bool AlreadyViewed(string reviewId)
        {
            //check whether this review has already been viewed
            string viewedList = URV;
            bool viewed = false;

            if (viewedList != "")
            {
                string[] lists = viewedList.Split(',');
                for (int i = 0; i < lists.Length; i++)
                {
                    if (reviewId == lists[i])
                    {
                        viewed = true;
                        break;
                    }
                }
            }

            return viewed;
        }

        /// <summary>
        /// Updated the Review Views by 1 and makes entry to cookie so subsequent entries will not be made
        /// </summary>
        void GetDetails()
        {
            try
            {
                if (reviewId > 0)
                {
                    using (DbCommand cmd = DbFactory.GetDBCommand())
                    {
                        if (AlreadyViewed(reviewId.ToString()) == false)
                        {
                            //update the viewd count by 1
                            string sql = " update customerreviews set viewed = ifnull(viewed, 0) + 1 where id = @v_reviewid";
                            cmd.CommandText = sql;
                            cmd.Parameters.Add(DbFactory.GetDbParam("@v_reviewid", DbType.Int32, reviewId));
                            MySqlDatabase.UpdateQuery(cmd, ConnectionType.MasterDatabase);
                            URV += reviewId + ",";
                        }

                    }
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            } // catch Exception
        }

    }
}