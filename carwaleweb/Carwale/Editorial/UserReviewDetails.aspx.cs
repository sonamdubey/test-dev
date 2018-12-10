using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Carwale.UI.Controls;
using Carwale.Notifications;
using Carwale.UI.Common;
using Carwale.DAL.Forums;
using Carwale.Utility;
using RabbitMqPublishing;
using System.Collections.Specialized;
using System.Data.Common;
using Carwale.DAL.CoreDAL.MySql;
using MySql.Data.MySqlClient;
using Carwale.Interfaces.CarData;
using Carwale.Service;
using Carwale.Entity.CarData;
using Carwale.Cache.Forums;
using Carwale.BL.CMS;
using Carwale.Interfaces.CMS;
using System.Collections.Generic;
using Carwale.Entity.Enum;
using Carwale.DTOs.CMS.ThreeSixtyView;

namespace Carwale.UI.Editorial
{
    public class ReadFullUserReview : Page
    {
        // User control to show comments on the review
        protected DiscussIt ucDiscuss;

        // String variables
        protected string targetKey = "", targetValue = "";
        public string reviewId = "";
        public string threadId = "-1", threadUrl = "-1";
        public string lastUpdatedOn = "";
        public DateTime entryDate;
        public string title = "",   pros = "", cons = "", comments = "", Prev = "Previous Review", Next = "Next Review",
                    customerId = "-1", totalComments = "0", logoURL = "",
                    reviewerEmail = "", reviewerId = "-1", reviewerName = "", handleName = "";
        public string isOwned = "", isNewlyPurchased = "", familiarity = "", mileage = "";
        protected string galleryHref = "";
        protected bool showSubNavigation = false;
        public HtmlTableRow trVerReviewed;
        protected SubNavigation SubNavigation;
        protected UsedCarsCount UsedCarsCount1;
        protected RoadTestSpc ucRoadTestSpc;
       // protected MakeModelVersion objCar;
        protected HiddenField hdnTest;
        // double variables
        public double overallR = 0, liked = 0, disliked = 0, viewed = 0, styleR = 0, comfortR = 0,
                      performanceR = 0, valueR = 0, fuelEconomyR = 0;

        // Bool variables
        public bool carwaleRecommends = false;
        public bool userLoggedIn = false;
        protected bool isModerator = false;
        protected int imageCount;
        protected int videoCount;
        protected CarModelDetails modelDetails;
        public Repeater rptMoreUserReviews;
        ICarModelCacheRepository modelCache = UnityBootstrapper.Resolve<ICarModelCacheRepository>();
        IVideosBL videosBL = UnityBootstrapper.Resolve<IVideosBL>();
        readonly ICarModels _carModelBl = UnityBootstrapper.Resolve<ICarModels>();
        protected Dictionary<string, object> param = new Dictionary<string,object>(); 

        public string CarName
        {
            get
            {
                if (ViewState["CarName"] != null)
                    return ViewState["CarName"].ToString();
                else
                    return "";
            }
            set { ViewState["CarName"] = value; }
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

        public string CarMake
        {
            get
            {
                if (ViewState["CarMake"] != null)
                    return ViewState["CarMake"].ToString();
                else
                    return "";
            }
            set { ViewState["CarMake"] = value; }
        }

        public string CarModel
        {
            get
            {
                if (ViewState["CarModel"] != null)
                    return ViewState["CarModel"].ToString();
                else
                    return "";
            }
            set { ViewState["CarModel"] = value; }
        }

        public string MaskingName
        {
            get
            {
                if (ViewState["MaskingName"] != null)
                    return ViewState["MaskingName"].ToString();
                else
                    return "";
            }
            set { ViewState["MaskingName"] = value; }
        }

        public string CarVersion
        {
            get
            {
                if (ViewState["CarVersion"] != null)
                    return ViewState["CarVersion"].ToString();
                else
                    return "";
            }
            set { ViewState["CarVersion"] = value; }
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
            InitializeComponent();
        }

        void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
        }

        void Page_Load(object Sender, EventArgs e)
        {
            /*
             Code created By : Supriya Khartode
             Created Date : 11/12/2013
             Note : This is the code used for device detection to integrate mobile website with desktop website
            */

            DeviceDetection dd = new DeviceDetection(Request.ServerVariables["HTTP_X_REWRITE_URL"].ToString());
            dd.DetectDevice();

            /*	Code added by Supriya Khartode ends here */
            //also get the forumId
            if (Request["rid"] != null && Request.QueryString["rid"] != "")
            {
                reviewId = Request.QueryString["rid"];

                //verify the id as passed in the url
                if (CommonOpn.CheckId(reviewId) == false)
                {
                    //redirect to the error page
                    UrlRewrite.Return404();
                    return;
                }
            }
            else
                UrlRewrite.Return404();

            string modelMaskingname = Request.QueryString["model"];
			string makeMaskingname = Request.QueryString["make"];
			
			if (!IsPostBack)
            {
                customerId = CurrentUser.Id;
                ForumsCache threadInfo = new ForumsCache();
                isModerator = threadInfo.IsModerator(CurrentUser.Id);

                GetDetails();
                var cmr = _carModelBl.FetchModelIdFromMaskingName(MaskingName, string.Empty);
				if (!cmr.IsValid)
				{
					UrlRewrite.Return404();
					return;
				}

				if (ModelId != "-1")
				{
					if (MaskingName != modelMaskingname || cmr.IsRedirect)
					{
						string modelUrl = MaskingName != modelMaskingname ? ManageCarUrl.CreateModelUrl(CarMake, MaskingName) : cmr.RedirectUrl;
						Response.RedirectPermanent(string.Format("{0}userreviews/{1}/", modelUrl, reviewId));
						return;
					}
				}
				else
				{
					string modelUrl = cmr.IsRedirect ? cmr.RedirectUrl : ManageCarUrl.CreateModelUrl(makeMaskingname, modelMaskingname);
					Response.RedirectPermanent(string.Format("{0}userreviews/", modelUrl));
					return;
				}
                modelDetails = modelCache.GetModelDetailsById(Convert.ToInt16(ModelId));

                if (modelDetails != null)
                {
                    ModelStartPrice = FormatPrice.FormatFullPrice(modelDetails.MinPrice.ToString(), modelDetails.MaxPrice.ToString());
                    imageCount = string.IsNullOrEmpty(modelDetails.OriginalImage) ? 0 : modelDetails.PhotoCount;
                    galleryHref = imageCount > 0 ? CMSCommon.GetImageUrl(modelDetails.MakeName, modelDetails.MaskingName) : "javascript:void(0);";
                    param.Add("modelId", modelDetails.ModelId);
                    param.Add("page", ContentPages.UserReviewDetailsPage);
                    param.Add("modelDetails", modelDetails);
                }
                GetThreadIdForReview(reviewId);
                ucDiscuss.ThreadId = threadId;
                ucDiscuss.ThreadUrl = threadUrl;
                ucDiscuss.Type = "review";

                GetMoreReviews();

                GoogleKeywords();

                ShowSubNavigation(modelDetails);

            }


            if (CarName == "")
                UrlRewrite.Return404();


            //logoURL = VersionId + "b.jpg";											
        } // Page_Load	

        /// <summary>
        /// Created By: Rakesh Yadav 29 Apr 2014
        /// Desc: Handle SubNavigation
        /// </summary>
        private void ShowSubNavigation(CarModelDetails objCar)
        {
            try
            {
                ucRoadTestSpc.MakeId = objCar.MakeId.ToString();
                ucRoadTestSpc.ModelId = objCar.ModelId.ToString();
                ucRoadTestSpc.Car = objCar.ModelName;
                ucRoadTestSpc.GetCount();
                ucRoadTestSpc.Visible = false;
                if (ucRoadTestSpc.ResultCount > 0)
                    SubNavigation.IsExpertReviewAvial = true;
                SubNavigation.TrackingFor360 = "user_review_detail";
                SubNavigation.is360Available = CMSCommon.IsThreeSixtyViewAvailable(objCar);
                if (SubNavigation.is360Available)
                    SubNavigation.Default360Category = CMSCommon.Get360DefaultCategory(AutoMapper.Mapper.Map<ThreeSixtyAvailabilityDTO>(objCar));
                SubNavigation.Make = CarMake;
                SubNavigation.MaskingName = MaskingName;
                SubNavigation.ModelName = CarModel;
                SubNavigation.ModelId = objCar.ModelId;
                SubNavigation.PQPageId = 53;
                SubNavigation.MakeId = objCar.MakeId.ToString();
                SubNavigation.Category = "UserReviewDetailsPage";
                SubNavigation.ImageCount = objCar.PhotoCount;
                var videos = videosBL.GetVideosByModelId(objCar.ModelId, Entity.CMS.CMSAppId.Carwale, 1, -1);
                videoCount = videos != null && videos.Count > 0 ? videos.Count : 0;
                SubNavigation.VideoCount = videoCount;
                if (objCar.New && !objCar.Futuristic)
                {
                    SubNavigation.IsMileageAvail = true;
                    showSubNavigation = true;
                }
                else
                {
                    SubNavigation.IsMileageAvail = false;
                    showSubNavigation = false;
                }

                SubNavigation.IsReviewsAvial = true;

                UsedCarsCount1.MakeId = objCar.MakeId.ToString();
                UsedCarsCount1.ModelId = objCar.ModelId.ToString();
                UsedCarsCount1.MakeName = objCar.MakeName;
                UsedCarsCount1.ModelName = objCar.ModelName;
                UsedCarsCount1.MaskingName = objCar.MaskingName;
                UsedCarsCount1.Visible = false;
                UsedCarsCount1.GetUsedCarsDetails();
                SubNavigation.IsUsedCarAvail = UsedCarsCount1.IsUsedCarAvial;//not sure
                SubNavigation.ShowColorsLink = CMSCommon.IsModelColorPhotosPresent(modelCache.GetModelColorsByModel(objCar.ModelId));
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }
        private void GetThreadIdForReview(string review_Id)
        {       
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("GetThreadIdForReview_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_ReviewId", DbType.Int64, review_Id != "" ? review_Id : "-1"));
                    using(IDataReader dr = MySqlDatabase.SelectQuery(cmd,DbConnections.ForumsMySqlReadConnection))
                    {
                        if (dr.Read())
                        {
                            threadId = dr["ThreadId"].ToString();
                            threadUrl = dr["Url"].ToString();
                        }
                    }
                }                  
            }
            catch (MySqlException err)
            {
                HttpContext.Current.Trace.Warn("GetThreadIdForReview EX : ", err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception err)
            {
                HttpContext.Current.Trace.Warn("GetThreadIdForReview EX : ", err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }         
        }

     
        void GetDetails()
        {      
            try
            {
                if (AlreadyViewed(reviewId) == false)
                {
                    string userReviewQueue = System.Configuration.ConfigurationManager.AppSettings["UserReviewViewsUpdaterQueue"];
                    RabbitMqPublish rmq = new RabbitMqPublish();
                    NameValueCollection nvc = new NameValueCollection();
                    nvc.Add("ReviewId", reviewId);
                    rmq.PublishToQueue(userReviewQueue, nvc);
                }
                using (DbCommand cmd = DbFactory.GetDBCommand("GetUserReviewDetailsById_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_ReviewId", DbType.Int64, reviewId != "" ? reviewId : "-1"));
                    using(IDataReader dr = MySqlDatabase.SelectQuery(cmd,DbConnections.CarDataMySqlReadConnection))
                    {
                        if (dr.Read())
                        {
                            CarMake = dr["Make"].ToString();
                            CarModel = dr["Model"].ToString();
                            MaskingName = dr["MaskingName"].ToString();
                            CarVersion = dr["Version"].ToString();
                            CarName = (CarMake + " " + CarModel + " " + CarVersion).Trim();

                            ModelId = dr["ModelId"].ToString();
                            VersionId = dr["VersionId"].ToString();
                            LargePic = dr["LargePic"].ToString();

                            title = dr["Title"].ToString();
                            reviewerId = dr["CustomerId"].ToString();

                            reviewerName = dr["CustomerName"].ToString();
                            handleName = "";
                            handleName = dr["HandleName"].ToString();
                            reviewerEmail = dr["CustomerEmail"].ToString();
                            entryDate = Convert.ToDateTime(dr["EntryDateTime"]);
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
                            logoURL = ImageSizes.CreateImageUrl(dr["HostURL"].ToString(), ImageSizes._210X118, dr["OriginalImgPath"].ToString());
                            lastUpdatedOn = dr["LastUpdatedOn"].ToString();
                            carwaleRecommends = Convert.ToBoolean(dr["CarwaleRecommended"]);

                            isOwned = dr["IsOwned"].ToString();
                            isNewlyPurchased = dr["IsNewlyPurchased"].ToString();
                            familiarity = dr["Familiarity"].ToString();
                            mileage = dr["Mileage"].ToString();

                            if (reviewerId == CurrentUser.Id)
                                userLoggedIn = true;
                        }
                    }
                }    
            }
            catch (MySqlException err)
            {
                HttpContext.Current.Trace.Warn("GetDetails Sql EX : ", err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            } // catch Exception
            catch (Exception err)
            {
                HttpContext.Current.Trace.Warn("GetDetails EX : ", err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            } // catch Exception          
        }


        public string GetComments(string value)
        {
            if (value != "")
                return value.Replace("\n", "<br>");
            else
                return "";
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
            try
            {
                using(DbCommand cmd = DbFactory.GetDBCommand("GetGoogleKeyWords_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_VersionId", DbType.Int32,!string.IsNullOrEmpty(VersionId)? VersionId : Convert.DBNull));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_ModelId", DbType.Int32, !string.IsNullOrEmpty(ModelId) ? ModelId : Convert.DBNull));
                    using(IDataReader dr = MySqlDatabase.SelectQuery(cmd,DbConnections.CarDataMySqlReadConnection))
                    {
                        if (dr.Read())
                        {
                            oem = dr["Make"].ToString().Replace(" ", "").Replace("/", "").Replace("-", "");
                            bodyType = dr["CarBodyStyle"].ToString().Replace(" ", "").Replace("/", "").Replace("-", "");
                            subSegment = dr["SubSegment"].ToString().Replace(" ", "").Replace("/", "").Replace("-", "");
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
            CommonOpn op = new CommonOpn();
            DataSet ds = new DataSet();
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("GetMoreReviewByModelIdandReviewId_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_ModelId", DbType.Int32, ModelId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_ReviewId", DbType.Int64, reviewId));
                    ds = MySqlDatabase.SelectAdapterQuery(cmd, DbConnections.CarDataMySqlReadConnection);
                }
                op.BindRepeaterReaderDataSet(ds, rptMoreUserReviews);
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

	} // class
} // namespace