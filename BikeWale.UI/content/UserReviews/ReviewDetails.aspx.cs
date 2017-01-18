using Bikewale.BindViewModels.Webforms.UserReviews;
using Bikewale.Common;
using Bikewale.Controls;
using Bikewale.Entities.Location;
using Bikewale.Entities.SEO;
using Bikewale.Entities.UserReviews;
using MySql.CoreDAL;
using System;
using System.Data;
using System.Data.Common;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Bikewale.Content
{
    public class ReviewDetails : System.Web.UI.Page
    {
        // User control to show comments on the review
        protected DiscussIt ucDiscuss;
        protected bool IsNew = false, IsUsed = false;
        protected ReviewDetailsEntity objReview;
        // String variables
        public string reviewId = "";
        public string lastUpdatedOn = "";
        public string _title = "", entryDate = "", pros = "", cons = "", comments = "", Prev = "Previous Review", Next = "Next Review",
                    customerId = "-1", totalComments = "0", logoURL = "",
                    reviewerEmail = "", reviewerId = "-1", reviewerName = "", handleName = "", HostUrl = "";
        public string isOwned = "", isNewlyPurchased = "", familiarity = "", mileage = "";

        public HtmlTableRow trVerReviewed;

        // double variables
        public double overallR = 0, liked = 0, disliked = 0, viewed = 0, styleR = 0, comfortR = 0,
                      performanceR = 0, valueR = 0, fuelEconomyR = 0;

        // Bool variables
        public bool bikewaleRecommends = false;
        public bool userLoggedIn = false;
        protected bool isModerator = false;
        protected MostPopularBikesMin ctrlPopularBikes;
        protected UserReviewSimilarBike ctrlUserReviewSimilarBike;
        protected NewUserReviewsList ctrlUserReviews;
        public Repeater rptMoreUserReviews;
        public PageMetaTags pageMetas;
        protected uint cityId;
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
            // Modified By :Lucky Rathore on 12 July 2016.
            Form.Action = Request.RawUrl;
            //code for device detection added by Ashwini Todkar
            // Modified By :Ashish Kamble on 5 Feb 2016
            string originalUrl = Request.ServerVariables["HTTP_X_ORIGINAL_URL"];
            if (String.IsNullOrEmpty(originalUrl))
                originalUrl = Request.ServerVariables["URL"];

            DeviceDetection dd = new DeviceDetection(originalUrl);
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

                cityId = Convert.ToUInt32(Bikewale.Utility.BWConfiguration.Instance.DefaultCity);

                BindUserReviewsDetails objBike = new BindUserReviewsDetails();
                objReview = new ReviewDetailsEntity();
                bool IsAlreadyViewed = AlreadyViewed(reviewId);
                objReview = objBike.GetDetails(reviewId, IsAlreadyViewed);
                BikeName = string.Format("{0} {1} {2}", objReview.BikeEntity.MakeEntity.MakeName, objReview.BikeEntity.ModelEntity.ModelName, objReview.BikeEntity.VersionEntity.VersionName);
                URV += reviewId + ",";
                if (reviewerId == CurrentUser.Id)
                    userLoggedIn = true;
                ucDiscuss.Type = "review";

                GetMoreReviews();
                GoogleKeywords();
            }


            if (string.IsNullOrEmpty(BikeName))
            {
                Response.Redirect(CommonOpn.AppPath + "pageNotFound.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                this.Page.Visible = false;
            }

            logoURL = objReview.BikeEntity.VersionEntity.VersionId + "b.jpg";
            BindControls();
            CreatMetas();
        }//pageload
        /// <summary>
        /// Created By :- Subodh Jain 17 Jan 2017
        /// Summary :- Bind metas
        /// </summary>
        private void CreatMetas()
        {
            pageMetas = new PageMetaTags();
            pageMetas.Title = string.Format("{0} - A Review on {1} by {2}, {1}", _title, BikeName, objReview.ReviewEntity.WrittenBy);
            pageMetas.Description = string.Format("{0} User Review - A review/feedback on {1} by {2}. Find out what {2} has to say about {1}.", BikeMake, BikeName, objReview.ReviewEntity.WrittenBy);
            pageMetas.Keywords = string.Format("{0} review, {0} user review, car review, owner feedback, consumer review", BikeName);
            pageMetas.AlternateUrl = string.Format("{0}/m/{1}-bikes/{2}/user-reviews/{3}.html", Bikewale.Utility.BWConfiguration.Instance.BwHostUrl, MakeMaskingName, ModelMaskingName, reviewId);
            pageMetas.CanonicalUrl = string.Format("{0}/{1}-bikes/{2}/user-reviews/{3}.html", Bikewale.Utility.BWConfiguration.Instance.BwHostUrl, MakeMaskingName, ModelMaskingName, reviewId);

        }
        /// <summary>
        /// Created By :- Subodh Jain 17 Jan 2017
        /// Summary :- Bind Controls
        /// </summary>
        /// <param name="reviewId"></param>
        /// <param name="isAlreadyViewed"></param>
        private void BindControls()
        {
            try
            {
                GlobalCityAreaEntity currentCityArea = Bikewale.Utility.GlobalCityArea.GetGlobalCityArea();
                ctrlPopularBikes.totalCount = 4;
                ctrlPopularBikes.CityId = Convert.ToInt32(currentCityArea.CityId);
                ctrlPopularBikes.cityName = currentCityArea.City;
                ctrlPopularBikes.MakeId = Convert.ToInt32(objReview.BikeEntity.MakeEntity.MakeId);
                ctrlPopularBikes.makeMasking = objReview.BikeEntity.MakeEntity.MaskingName;
                ctrlPopularBikes.makeName = objReview.BikeEntity.MakeEntity.MakeName;

                ctrlUserReviewSimilarBike.ModelId = Convert.ToUInt16(objReview.BikeEntity.ModelEntity.ModelId);
                ctrlUserReviewSimilarBike.TopCount = 4;

                ctrlUserReviews.ReviewCount = 3;
                ctrlUserReviews.PageNo = 1;
                ctrlUserReviews.PageSize = 4;
                ctrlUserReviews.ModelId = objReview.BikeEntity.ModelEntity.ModelId;
                ctrlUserReviews.Filter = Entities.UserReviews.FilterBy.MostRecent;
                ctrlUserReviews.MakeName = objReview.BikeEntity.MakeEntity.MakeName;
                ctrlUserReviews.ModelName = objReview.BikeEntity.ModelEntity.ModelName;
                ctrlUserReviews.WidgetHeading = string.Format("More {0} {1} User reviews", objReview.BikeEntity.MakeEntity.MakeName, objReview.BikeEntity.ModelEntity.ModelName);
                ctrlUserReviews.ReviewId = Convert.ToInt32(reviewId);

            }
            catch (Exception err)
            {

                ErrorClass objErr = new ErrorClass(err, "ListReviews.BindControls");
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

            sql = @" select top 1 cr1.id nextreview, cr2.id previousreview
                from customerreviews cr  
                left join customerreviews cr1   on cr.modelid = cr1.modelid and cr1.id > @v_reviewid and cr1.isverified = 1 and cr1.isactive = 1
                left join customerreviews cr2  on cr.modelid = cr2.modelid and cr2.id < @v_reviewid and cr2.isverified = 1 and cr2.isactive = 1
                where cr.modelid = @v_modelid order by cr2.id desc ";

            try
            {

                using (DbCommand cmd = DbFactory.GetDBCommand(sql))
                {
                    cmd.Parameters.Add(DbFactory.GetDbParam("@v_modelid", DbType.Int32, ModelId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("@v_reviewid", DbType.Int32, reviewId));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            while (dr.Read())
                            {
                                nextId = dr["NextReview"].ToString();
                                prevId = dr["PreviousReview"].ToString();
                            }
                            dr.Close();
                        }
                    }
                }

                //Prev = prevId == "" ? "Previous Review" : "<a href=\"/research/" + UrlRewrite.FormatSpecial(BikeMake) + "-bikes/" + UrlRewrite.FormatSpecial(BikeModel) + "/userreviews/" + prevId + ".html\">Previous Review</a>";
                //Next = nextId == "" ? "Next Review" : "<a href=\"/research/" + UrlRewrite.FormatSpecial(BikeMake) + "-bikes/" + UrlRewrite.FormatSpecial(BikeModel) + "/userreviews/" + nextId + ".html\">Next Review</a>";

                Prev = prevId == "" ? "Previous Review" : "<a href=\"/research/" + MakeMaskingName + "-bikes/" + ModelMaskingName + "/userreviews/" + prevId + ".html\">Previous Review</a>";
                Next = nextId == "" ? "Next Review" : "<a href=\"/research/" + MakeMaskingName + "-bikes/" + ModelMaskingName + "/userreviews/" + nextId + ".html\">Next Review</a>";

                Trace.Warn("Prev : " + Prev + " : Next : " + Next);
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            } // catch Exception
        }

        // send mail to the reviewer
        void SendMail()
        {
            string url = "https://www.bikewale.com/content/userreviews/reviewdetails.aspx?rid=" + reviewId + "#comments";

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
            uint _modelId = 0, _versionId = 0;
            if (VersionId != "-1")
            {
                sql = @" select distinct cm.name as make, se.name as subsegment, bo.name bikebodystyle
                   from bikemodels as cmo, bikemakes as cm, bikebodystyles bo,
                    (bikeversions ve   left join bikesubsegments se   on se.id = ve.subsegmentid )
                    where cm.id=cmo.bikemakeid and cmo.id=ve.bikemodelid and bo.id=ve.bodystyleid
                    and ve.id = @v_versionid";
            }
            else
            {
                sql = @" select distinct cm.name as make, se.name as subsegment, bo.name bikebodystyle 
                     from bikemodels as cmo, bikemakes as cm, bikebodystyles bo, 
                     (bikeversions ve   left join bikesubsegments se   on se.id = ve.subsegmentid ) 
                     where cm.id=cmo.bikemakeid and cmo.id=ve.bikemodelid and bo.id=ve.bodystyleid 
                     and ve.bikemodelid = @v_modelid";
            }
            try
            {
                if (!string.IsNullOrEmpty(ModelId) && !string.IsNullOrEmpty(VersionId))
                {
                    uint.TryParse(VersionId, out _versionId);
                    uint.TryParse(ModelId, out _modelId);
                }

                using (DbCommand cmd = DbFactory.GetDBCommand(sql))
                {
                    cmd.Parameters.Add(DbFactory.GetDbParam("@v_modelid", DbType.Int32, _modelId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("@v_versionid", DbType.Int32, _versionId));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null && dr.Read())
                        {

                            oem = dr["Make"].ToString().Replace(" ", "").Replace("/", "").Replace("-", "");
                            bodyType = dr["BikeBodyStyle"].ToString().Replace(" ", "").Replace("/", "").Replace("-", "");
                            subSegment = dr["SubSegment"].ToString().Replace(" ", "").Replace("/", "").Replace("-", "");
                            dr.Close();
                        }

                    }
                }
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
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
                sql = @" select cr.id as reviewid, cu.name as customername, cu.id as customerid,
                        cr.title, cr.entrydatetime, liked, overallr 
                        from  customers as cu , customerreviews as cr  
                        where cu.id = cr.customerid and cr.isactive=1 and 
                        cr.isverified=1 and cr.modelid = @v_modelid and cr.id <> @v_reviewid
                        order by liked desc 
                        limit 5";

                DbParameter[] param = new[] { DbFactory.GetDbParam("par_modelid", DbType.Int32,ModelId ),
                    DbFactory.GetDbParam("par_reviewid", DbType.Int32,reviewId )};


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


    }//class
}//namespace