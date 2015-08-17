using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Bikewale.Common;
using Bikewale.Controls;
using Bikewale.Forums.Common;

namespace Bikewale.Content
{
    public class ReviewDetails : System.Web.UI.Page
    {
        // User control to show comments on the review
        protected DiscussIt ucDiscuss;
        protected bool IsNew = false, IsUsed = false;

        // String variables
        public string reviewId = "";
        public string lastUpdatedOn = "";
        public string _title = "", entryDate = "", pros = "", cons = "", comments = "", Prev = "Previous Review", Next = "Next Review",
                    customerId = "-1", totalComments = "0", logoURL = "",
                    reviewerEmail = "", reviewerId = "-1", reviewerName = "", handleName = "",HostUrl="";
        public string isOwned = "", isNewlyPurchased = "", familiarity = "", mileage = "";

        public HtmlTableRow trVerReviewed;

        // double variables
        public double overallR = 0, liked = 0, disliked = 0, viewed = 0, styleR = 0, comfortR = 0,
                      performanceR = 0, valueR = 0, fuelEconomyR = 0;

        // Bool variables
        public bool bikewaleRecommends = false;
        public bool userLoggedIn = false;
        protected bool isModerator = false;

        public Repeater rptMoreUserReviews;

        public string BikeName
        {
            get
            {
                if (ViewState["BikeName"] != null)
                    return ViewState["BikeName"].ToString();
                else
                    return "";
            }
            set { ViewState["BikeName"] = value; }
        }
        public string LargePic
        {
            get
            {
                if (ViewState["LargePic"] != null)
                    return ViewState["LargePic"].ToString();
                else
                    return "";
            }
            set { ViewState["LargePic"] = value; }
        }

        public string OriginalImagePath
        {
            get
            {
                if (ViewState["OriginalImagePath"] != null)
                    return ViewState["OriginalImagePath"].ToString();
                else
                    return "";
            }
            set { ViewState["OriginalImagePath"] = value; }
        }


        public string BikeMake
        {
            get
            {
                if (ViewState["BikeMake"] != null)
                    return ViewState["BikeMake"].ToString();
                else
                    return "";
            }
            set { ViewState["BikeMake"] = value; }
        }

        public string BikeModel
        {
            get
            {
                if (ViewState["BikeModel"] != null)
                    return ViewState["BikeModel"].ToString();
                else
                    return "";
            }
            set { ViewState["BikeModel"] = value; }
        }

        public string ModelMaskingName
        {
            get
            {
                if (ViewState["ModelMaskingName"] != null)
                    return ViewState["ModelMaskingName"].ToString();
                else
                    return "";
            }
            set { ViewState["ModelMaskingName"] = value; }
        }

        public string MakeMaskingName
        {
            get
            {
                if (ViewState["MakeMaskingName"] != null)
                    return ViewState["MakeMaskingName"].ToString();
                else
                    return "";
            }
            set { ViewState["MakeMaskingName"] = value; }
        }
        public string BikeVersion
        {
            get
            {
                if (ViewState["BikeVersion"] != null)
                    return ViewState["BikeVersion"].ToString();
                else
                    return "";
            }
            set { ViewState["BikeVersion"] = value; }
        }

        public string ModelId
        {
            get
            {
                if (ViewState["ModelId"] != null)
                    return ViewState["ModelId"].ToString();
                else
                    return "-1";
            }
            set { ViewState["ModelId"] = value; }
        }

        public string VersionId
        {
            get
            {
                if (ViewState["VersionId"] != null)
                    return ViewState["VersionId"].ToString();
                else
                    return "-1";
            }
            set { ViewState["VersionId"] = value; }
        }

        public string BackUrl
        {
            get
            {
                if (ViewState["BackUrl"] != null)
                    return ViewState["BackUrl"].ToString();
                else
                    return "";
            }
            set { ViewState["BackUrl"] = value; }
        }

        public string BackUrlRev
        {
            get
            {
                if (ViewState["BackUrlRev"] != null)
                    return ViewState["BackUrlRev"].ToString();
                else
                    return "";
            }
            set { ViewState["BackUrlRev"] = value; }
        }


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

        protected override void OnInit(EventArgs e)
        {
            base.Load += new EventHandler(Page_Load);
        }
        private void Page_Load(object sender, EventArgs e)
        {
            //code for device detection added by Ashwini Todkar
            DeviceDetection dd = new DeviceDetection(Request.ServerVariables["HTTP_X_REWRITE_URL"].ToString());
            dd.DetectDevice();

            //also get the forumId
            if (Request["rid"] != null && Request.QueryString["rid"] != "")
            {
                reviewId = Request.QueryString["rid"];

                //verify the id as passed in the url
                if (CommonOpn.CheckId(reviewId) == false)
                {
                    //redirect to the error page
                    Response.Redirect(CommonOpn.AppPath + "pageNotFound.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                    return;
                }
            }
            else
            {
                Response.Redirect(CommonOpn.AppPath + "pageNotFound.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                this.Page.Visible = false;
            }

            if (!IsPostBack)
            {
                customerId = CurrentUser.Id;
                ForumsCommon fc = new ForumsCommon();
                isModerator = fc.GetModeratorLoginStatus(CurrentUser.Id);
                Trace.Warn("moderate");
                GetDetails();
                //GetNextPreviousReview();
               
                //MakeModelVersion objBike = new MakeModelVersion();
                ModelVersionDescription objBike = new ModelVersionDescription();
                objBike.GetDetailsByModel(ModelId);
                ModelStartPrice = objBike.ModelBasePrice;
                Trace.Warn("ModelStartPrice " + ModelStartPrice + " ," + ModelId);
                
                ucDiscuss.ThreadId = GetThreadIdForReview(reviewId);
                ucDiscuss.Type = "review";

                GetMoreReviews();

                GoogleKeywords();
            }


            if (BikeName == "")
            {
                Response.Redirect(CommonOpn.AppPath + "pageNotFound.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                this.Page.Visible = false;
            }

            logoURL = VersionId + "b.jpg";											
        }//pageload

        private string GetThreadIdForReview(string review_Id)
        {
            string returnVal = "-1";
            string sql = "SELECT ThreadId FROM Forum_ArticleAssociation With(NoLock) WHERE ArticleType = 3 AND ArticleId = @ArticleId";

            SqlCommand cmd = new SqlCommand(sql);
            cmd.Parameters.Add("@ArticleId", SqlDbType.BigInt).Value = (review_Id != "" ? review_Id : "-1");

            SqlDataReader dr = null;
            Database db = new Database();
            try
            {
                dr = db.SelectQry(cmd);

                if (dr.Read())
                {
                    returnVal = dr[0].ToString();
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
                returnVal = "-1";
            }
            finally
            {
                dr.Close();
                db.CloseConnection();
            }

            return returnVal;
        }

        void GetDetails()
        {
            string sql = "";

            SqlDataReader dr = null;
            Database db = new Database();

            try
            {
                if (AlreadyViewed(reviewId) == false)
                {
                    //update the viewd count by 1
                    sql = " UPDATE CustomerReviews SET Viewed = IsNull(Viewed, 0) + 1 WHERE ID = @ID";

                    SqlCommand cmd = new SqlCommand(sql);
                    cmd.Parameters.Add("@ID", SqlDbType.BigInt).Value = (reviewId != "" ? reviewId : "-1");

                    db.UpdateQry(cmd);

                    //add this to the cookie
                    URV += reviewId + ",";

                    Trace.Warn(URV);
                }

                SqlCommand cmd1 = new SqlCommand("GetCustomerReviewInfo");
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.Add("@ReviewId", SqlDbType.BigInt).Value = (reviewId != "" ? reviewId : "-1");

                Trace.Warn("reviewId " + reviewId);

                dr = db.SelectQry(cmd1);

                if (dr.Read())
                {
                    BikeMake = dr["Make"].ToString();
                    BikeModel = dr["Model"].ToString();
                    ModelMaskingName = dr["ModelMaskingName"].ToString();
                    //ModelMaskingName = dr["ModelMaskingName"].ToString();
                    BikeVersion = dr["Version"].ToString();
                    BikeName = (BikeMake + " " + BikeModel + " " + BikeVersion).Trim();
                    MakeMaskingName = dr["MakeMaskingName"].ToString();
                    ModelId = dr["ModelId"].ToString();
                    VersionId = dr["VersionId"].ToString();
                    LargePic = dr["LargePic"].ToString();

                    _title = dr["Title"].ToString();
                    reviewerId = dr["CustomerId"].ToString();

                    reviewerName = dr["CustomerName"].ToString();
                    handleName = "";
                    handleName = dr["HandleName"].ToString();
                    reviewerEmail = dr["CustomerEmail"].ToString();
                    entryDate = Convert.ToDateTime(dr["EntryDateTime"]).ToString("dd MMMM, yyyy");
                    pros = dr["Pros"].ToString();
                    cons = dr["Cons"].ToString();
                    comments = dr["Comments"].ToString();
                    overallR = Convert.ToDouble(dr["OverallR"]);
                    liked = Convert.ToDouble(dr["Liked"]);
                    disliked = Convert.ToDouble(dr["Disliked"]);
                    viewed = Convert.ToDouble(dr["Viewed"]);
                    styleR = Convert.ToDouble(dr["StyleR"]);
                    comfortR = Convert.ToDouble(dr["ComfortR"]);
                    performanceR = Convert.ToDouble(dr["PerformanceR"]);
                    valueR = Convert.ToDouble(dr["ValueR"]);
                    fuelEconomyR = Convert.ToDouble(dr["FuelEconomyR"]);

                    totalComments = dr["TotalComments"].ToString();
                    logoURL = dr["LogoUrl"].ToString();
                    lastUpdatedOn = dr["LastUpdatedOn"].ToString();
                    bikewaleRecommends = Convert.ToBoolean(dr["BikewaleRecommended"]);

                    isOwned = dr["IsOwned"].ToString();
                    isNewlyPurchased = dr["IsNewlyPurchased"].ToString();
                    familiarity = dr["Familiarity"].ToString();
                    mileage = dr["Mileage"].ToString();
                    HostUrl = dr["HostURL"].ToString();
                    IsNew = Convert.ToBoolean(dr["New"]);
                    IsUsed = Convert.ToBoolean(dr["Used"]);
                    OriginalImagePath = dr["OriginalImagePath"].ToString();
                    Trace.Warn("IsNew : "+IsNew+" "+"IsUsed : "+IsUsed);
                    if (reviewerId == CurrentUser.Id)
                        userLoggedIn = true;
                }
            }
            catch (Exception err)
            {
                Trace.Warn("object not defined : "+err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            } // catch Exception
            finally
            {
                dr.Close();
                db.CloseConnection();
            }
        }


        public string GetComments(string value)
        {
            if (value != "")
                return value.Replace("\n", "<br>");
            else
                return "";
        }

        void GetNextPreviousReview()
        {
            string sql = "";
            string prevId = "", nextId = "";

            sql = " SELECT Top 1 Cr1.Id NextReview, Cr2.ID PreviousReview "
                + " FROM CustomerReviews Cr With(NoLock) "
                + " LEFT JOIN CustomerReviews Cr1 With(NoLock) ON Cr.ModelId = Cr1.ModelId AND Cr1.ID > @ReviewId AND Cr1.IsVerified = 1 AND Cr1.IsActive = 1 "
                + " LEFT JOIN CustomerReviews Cr2 With(NoLock) ON Cr.ModelId = Cr2.ModelId AND Cr2.ID < @ReviewId AND Cr2.IsVerified = 1 AND Cr2.IsActive = 1 "
                + " WHERE Cr.ModelId = @ModelId ORDER BY Cr2.ID Desc ";

            Trace.Warn(sql);

            SqlCommand cmd = new SqlCommand(sql);
            cmd.Parameters.Add("@ModelId", SqlDbType.BigInt).Value = (ModelId != "" ? ModelId : "-1");
            cmd.Parameters.Add("@ReviewId", SqlDbType.BigInt).Value = (reviewId != "" ? reviewId : "-1");

            SqlDataReader dr = null;
            Database db = new Database();

            try
            {
                dr = db.SelectQry(cmd);

                while (dr.Read())
                {
                    nextId = dr["NextReview"].ToString();
                    prevId = dr["PreviousReview"].ToString();
                }

                //Prev = prevId == "" ? "Previous Review" : "<a href=\"/research/" + UrlRewrite.FormatSpecial(BikeMake) + "-bikes/" + UrlRewrite.FormatSpecial(BikeModel) + "/userreviews/" + prevId + ".html\">Previous Review</a>";
                //Next = nextId == "" ? "Next Review" : "<a href=\"/research/" + UrlRewrite.FormatSpecial(BikeMake) + "-bikes/" + UrlRewrite.FormatSpecial(BikeModel) + "/userreviews/" + nextId + ".html\">Next Review</a>";

                Prev = prevId == "" ? "Previous Review" : "<a href=\"/research/" + MakeMaskingName + "-bikes/" + ModelMaskingName + "/userreviews/" + prevId + ".html\">Previous Review</a>";
                Next = nextId == "" ? "Next Review" : "<a href=\"/research/" +MakeMaskingName + "-bikes/" + ModelMaskingName + "/userreviews/" + nextId + ".html\">Next Review</a>";

                Trace.Warn("Prev : " + Prev + " : Next : " + Next);
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            } // catch Exception
            finally
            {
                dr.Close();
                db.CloseConnection();
            }
        }

        // send mail to the reviewer
        void SendMail()
        {
            string url = "http://www.bikewale.com/content/userreviews/reviewdetails.aspx?rid=" + reviewId + "#comments";

            try
            {
                Trace.Warn("reviewerId : " + reviewerId);

                if (reviewerId != CurrentUser.Id)
                {
                    string name = reviewerName;
                    string email = reviewerEmail;

                    Bikewale.Common.Mails.CustomerReviewComment(email, name, _title, url);
                }
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

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

        protected void GoogleKeywords()
        {
            string sql = "";
            SqlDataReader dr = null;

            if (VersionId != "-1")
            {
                sql = " SELECT DISTINCT CM.Name AS Make, Se.Name AS SubSegment, Bo.Name BikeBodyStyle "
                    + " FROM BikeModels AS CMO, BikeMakes AS CM, BikeBodyStyles Bo, "
                    + " (BikeVersions Ve With(NoLock) LEFT JOIN BikeSubSegments Se With(NoLock) ON Se.Id = Ve.SubSegmentId ) "
                    + " WHERE CM.ID=CMO.BikeMakeId AND CMO.ID=Ve.BikeModelId AND Bo.Id=Ve.BodyStyleId "
                    + " AND Ve.Id = @Id";
            }
            else
            {
                sql = " SELECT DISTINCT CM.Name AS Make, Se.Name AS SubSegment, Bo.Name BikeBodyStyle "
                    + " FROM BikeModels AS CMO, BikeMakes AS CM, BikeBodyStyles Bo, "
                    + " (BikeVersions Ve With(NoLock) LEFT JOIN BikeSubSegments Se With(NoLock) ON Se.Id = Ve.SubSegmentId ) "
                    + " WHERE CM.ID=CMO.BikeMakeId AND CMO.ID=Ve.BikeModelId AND Bo.Id=Ve.BodyStyleId "
                    + " AND Ve.BikeModelId = @BikeModelId";
            }

            SqlCommand cmd = new SqlCommand(sql);
            cmd.Parameters.Add("@BikeModelId", SqlDbType.BigInt).Value = (ModelId != "" ? ModelId : "-1");
            cmd.Parameters.Add("@Id", SqlDbType.BigInt).Value = (VersionId != "" ? VersionId : "-1");

            Database db = new Database();
            try
            {
                dr = db.SelectQry(cmd);
                if (dr.Read())
                {

                    oem = dr["Make"].ToString().Replace(" ", "").Replace("/", "").Replace("-", "");
                    bodyType = dr["BikeBodyStyle"].ToString().Replace(" ", "").Replace("/", "").Replace("-", "");
                    subSegment = dr["SubSegment"].ToString().Replace(" ", "").Replace("/", "").Replace("-", "");
                }

                Trace.Warn(sql);
                Trace.Warn("oem : " + oem);
                Trace.Warn(" bodyType : " + bodyType);
                Trace.Warn(" subSegment : " + subSegment);                
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                if(dr != null)
                    dr.Close();

                db.CloseConnection();
            }
        }

        public string PurchasedAs()
        {
            string returnVal = "";

            if (isOwned == "No")
            {
                returnVal = "Not Purchased";
            }
            else if (isNewlyPurchased == "Yes")
            {
                returnVal = "New";
            }
            else if (isNewlyPurchased == "No")
            {
                returnVal = "Used";
            }
            else
            {
                returnVal = "&nbsp;";
            }

            return returnVal;
        }

        public string GetFuelEconomy()
        {
            string returnVal = "";

            if (mileage.ToString() != "0")
            {
                returnVal = mileage.ToString() + " kpl";
            }

            return returnVal;
        }

        void GetMoreReviews()
        {
            string sql = "";
            CommonOpn op = new CommonOpn();

            try
            {
                sql = " Select Top 5 CR.ID AS ReviewId, ISNULL(UP.HandleName, CU.Name) AS CustomerName, CU.ID AS CustomerId, "
                    + " CR.Title, CR.EntryDateTime, Liked, OverallR "

                    + " From  Customers AS CU With(NoLock) left Join UserProfile UP With(NoLock) ON UP.UserId = CU.Id, CustomerReviews AS CR With(NoLock) "

                    + " Where CU.ID = CR.CustomerId AND CR.IsActive=1 AND "
                    + " CR.IsVerified=1 AND CR.ModelId = @ModelId AND CR.Id <> @ReviewId"
                    + " Order BY Liked DESC ";

                SqlParameter[] param = 
				{					
					new SqlParameter("@ModelId", ModelId),
					new SqlParameter("@ReviewId", reviewId)
				};

                op.BindRepeaterReader(sql, rptMoreUserReviews, param);
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            } // catch Exception
        }

        string _modelStartPrice = "";
        public string ModelStartPrice
        {
            get { return _modelStartPrice; }
            set { _modelStartPrice = value; }
        }

        //public bool IsNew
        //{
        //    get
        //    {
        //        if (ViewState["IsNew"] != null)
        //            return Convert.ToBoolean(ViewState["IsNew"]);
        //        else
        //            return false;
        //    }
        //}

        //public bool IsUsed
        //{
        //    get
        //    {
        //        if (ViewState["IsNew"] != null)
        //            return Convert.ToBoolean(ViewState["IsUsed"]);
        //        else
        //            return false;
        //    }
        //}

    }//class
}//namespace