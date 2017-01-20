using Bikewale.BindViewModels.Webforms.UserReviews;
using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using Bikewale.Common;
using Bikewale.Controls;
using Bikewale.DAL.BikeData;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Location;
using Bikewale.Entities.SEO;
using Bikewale.Entities.UserReviews;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Utility;
using Microsoft.Practices.Unity;
using MySql.CoreDAL;
using System;
using System.Data;
using System.Data.Common;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Bikewale.Content
{
    public class ListReviews : System.Web.UI.Page
    {
        protected string oem = "", bodyType = "", subSegment = "";
        protected string pageNumber = "";
        public string modelId = "", versionId = "";
        protected RepeaterPagerReviews rpgReviews;
        protected Repeater rptReviews;

        public int totalReviewCount = 0;

        protected UserReviewSimilarBike ctrlUserReviewSimilarBike;
        protected DropDownList drpSort, drpVersions;
        protected MakeModelVersion objBike;
        protected HtmlGenericControl spnComments;
        protected MostPopularBikesMin ctrlPopularBikes;
        protected HtmlForm frmMain;
        protected ReviewDetailsEntity objReview;
        protected uint MakeId;
        public uint cityid;
        public PageMetaTags pageMetas;
        public string SortingCriteria
        {
            get
            {
                string sortCrit = "Liked";

                string sortCriteriaVal = IsPostBack ? drpSort.SelectedItem.Value : ShortCriteria;

                if (!IsPostBack)
                    drpSort.SelectedIndex = drpSort.Items.IndexOf(drpSort.Items.FindByValue(sortCriteriaVal));

                switch (drpSort.SelectedItem.Value.ToString())
                {
                    case "1":	//most helpful
                        sortCrit = "Liked";
                        break;

                    case "2":	//most read
                        sortCrit = "Viewed";
                        break;

                    case "3":	//most recent
                        sortCrit = "EntryDateTime";
                        break;

                    case "4":	//most rated
                        sortCrit = "OverallR";
                        break;

                    default:
                        sortCrit = "Liked";
                        break;
                }

                return sortCrit;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.Load += new EventHandler(Page_Load);
            rptReviews = (Repeater)rpgReviews.FindControl("rptReviews");
            drpSort.SelectedIndexChanged += new EventHandler(drpSort_Change);
            drpVersions.SelectedIndexChanged += new EventHandler(drpVersions_Change);
        }
        /// <summary>
        /// Modified By :- Subodh Jain 16 Jain 2017
        /// Summary :- Created layerd architecture for data retrival
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_Load(object sender, EventArgs e)
        {
            // Modified By :Lucky Rathore on 12 July 2016.
            Form.Action = Request.RawUrl;
            //code for device detection added by Ashwini Todkar
            // Modified By :Ashish Kamble on 5 Feb 2016
            Form.Action = Request.RawUrl;
            string originalUrl = Request.ServerVariables["HTTP_X_ORIGINAL_URL"];
            if (String.IsNullOrEmpty(originalUrl))
                originalUrl = Request.ServerVariables["URL"];

            DeviceDetection dd = new DeviceDetection(originalUrl);
            dd.DetectDevice();
            ModelMaskingResponse objResponse = null;

            //also get the forumId
            if (Request["bikem"] != null && Request.QueryString["bikem"] != "")
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IBikeMaskingCacheRepository<BikeModelEntity, int>, BikeModelMaskingCache<BikeModelEntity, int>>()
                             .RegisterType<ICacheManager, MemcacheManager>()
                             .RegisterType<IBikeModelsRepository<BikeModelEntity, int>, BikeModelsRepository<BikeModelEntity, int>>();
                    var objCache = container.Resolve<IBikeMaskingCacheRepository<BikeModelEntity, int>>();

                    objResponse = objCache.GetModelMaskingResponse(Request.QueryString["bikem"]);

                    if (objResponse != null && objResponse.StatusCode == 200)
                    {
                        modelId = objResponse.ModelId.ToString();
                    }
                    else
                    {
                        if (objResponse.StatusCode == 301)
                        {
                            //redirect permanent to new page 
                            CommonOpn.RedirectPermanent(Request.RawUrl.Replace(Request.QueryString["bikem"], objResponse.MaskingName));

                        }
                        else
                        {
                            Response.Redirect(CommonOpn.AppPath + "pageNotFound.aspx", false);
                            HttpContext.Current.ApplicationInstance.CompleteRequest();
                            this.Page.Visible = false;
                        }
                    }
                }
                if (CommonOpn.CheckId(modelId) == false)
                {
                    //redirect to the error page
                    Response.Redirect(CommonOpn.AppPath + "pageNotFound.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                }
            }

            //also get the forumId
            if (Request["pn"] != null && Request.QueryString["pn"] != "")
            {
                pageNumber = Request.QueryString["pn"];

                //verify the id as passed in the url
                if (CommonOpn.CheckId(pageNumber) == false)
                {
                    //redirect to the error page
                    Response.Redirect(CommonOpn.AppPath + "pageNotFound.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                }
            }

            //also get the forumId
            if (Request["version"] != null && Request.QueryString["version"] != "")
            {
                versionId = Request.QueryString["version"];


                //verify the id as passed in the url
                if (CommonOpn.CheckId(versionId) == false)
                {
                    //redirect to the error page                 

                    Response.Redirect(CommonOpn.AppPath + "pageNotFound.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                }
            }


            if (objResponse != null && objResponse.StatusCode != 404)
            {

                if (!IsPostBack)
                {
                    cityid = Convert.ToUInt32(Bikewale.Utility.BWConfiguration.Instance.DefaultCity);

                    BindUserReviews objBike = new BindUserReviews();
                    objReview = new ReviewDetailsEntity();
                    if (modelId != "")
                    {

                        objReview = objBike.GetDetailsByModel(Convert.ToInt32(modelId), cityid);

                    }
                    else
                    {
                        objReview = objBike.GetDetailsByVersion(Convert.ToInt32(versionId), cityid);
                    }

                    if (objReview != null)
                    {
                        ModelStartPrice = objReview.ModelBasePrice;


                        LargePic = objReview.LargePicUrl;

                        if (objReview.ReviewRatingEntity != null)
                        {
                            RatingOverall = objReview.ReviewRatingEntity.OverAllRating;
                            RatingLooks = objReview.ReviewRatingEntity.ModelRatingLooks;
                            RatingPerformance = objReview.ReviewRatingEntity.PerformanceRating;
                            RatingComfort = objReview.ReviewRatingEntity.ComfortRating;
                            RatingFuelEconomy = objReview.ReviewRatingEntity.FuelEconomyRating;
                            RatingValueForMoney = objReview.ReviewRatingEntity.ValueRating;
                        }
                        if (objReview.BikeEntity != null && objReview.BikeEntity.MakeEntity != null)
                        {
                            MakeId = Convert.ToUInt32(objReview.BikeEntity.MakeEntity.MakeId);
                            MakeName = objReview.BikeEntity.MakeEntity.MakeName;
                            MakeMaskingName = objReview.BikeEntity.MakeEntity.MaskingName;
                        }
                        if (objReview.BikeEntity != null && objReview.BikeEntity.ModelEntity != null)
                        {
                            ModelName = objReview.BikeEntity.ModelEntity.ModelName;
                            ModelMaskingName = objReview.BikeEntity.ModelEntity.MaskingName;
                            ModelReviewCount = Convert.ToInt32(objReview.BikeEntity.ReviewsCount);
                        }
                        BikeName = string.Format("{0} {1}", MakeName, ModelName);
                        HostUrl = objReview.HostUrl;
                        IsNew = objReview.New;
                        IsUsed = objReview.Used;
                        OriginalImagePath = objReview.OriginalImagePath;
                        Futuristic = objReview.IsFuturistic;
                        if (objReview.ModelSpecs != null)
                        {

                            displacement = Convert.ToDouble(objReview.ModelSpecs.Displacement);
                            maxPower = Convert.ToDouble(objReview.ModelSpecs.MaxPower);
                            fuelEfficency = Convert.ToDouble(objReview.ModelSpecs.FuelEfficiencyOverall);
                            kerbWeight = Convert.ToDouble(objReview.ModelSpecs.KerbWeight);

                        }

                    }
                    if (Futuristic)
                    {
                        Response.Redirect(CommonOpn.AppPath + "pageNotFound.aspx", false);
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                        this.Page.Visible = false;
                    }
                    FillControls.FillReviewedVersions(drpVersions, modelId);

                    FillRepeaters();
                }

                GoogleKeywords();
                BindControls();
                CreatMetas();
            }
        }//pageload
        /// <summary>
        /// Created By :- Subodh Jain 17 Jan 2017
        /// Summary :- Bind metas
        /// </summary>
        private void CreatMetas()
        {
            pageMetas = new PageMetaTags();
            pageMetas.Title = string.Format("User Reviews: {0} | Bikes Reviews.", BikeName);
            pageMetas.Description = string.Format("{0} User Reviews - Read first-hand reviews of actual {0} owners. Find out what buyers of {0} have to say about the bike.", BikeName);
            pageMetas.Keywords = string.Format("{0} reviews, {0} Users Reviews, {0} customer reviews, {0} customer feedback, {0} owner feedback, user bike reviews, owner feedback, consumer feedback, buyer reviews", BikeName);
            pageMetas.AlternateUrl = !string.IsNullOrEmpty(pageNumber) ? string.Format("{0}/m/{1}-bikes/{2}/user-reviews-p{3}/", Bikewale.Utility.BWConfiguration.Instance.BwHostUrl, MakeMaskingName, ModelMaskingName, pageNumber) : string.Format("{0}/m/{1}-bikes/{2}/user-reviews/", Bikewale.Utility.BWConfiguration.Instance.BwHostUrl, MakeMaskingName, ModelMaskingName);
            pageMetas.CanonicalUrl = !string.IsNullOrEmpty(pageNumber) ? string.Format("{0}/{1}-bikes/{2}/user-reviews-p{3}/", Bikewale.Utility.BWConfiguration.Instance.BwHostUrl, MakeMaskingName, ModelMaskingName, pageNumber) : string.Format("{0}/{1}-bikes/{2}/user-reviews/", Bikewale.Utility.BWConfiguration.Instance.BwHostUrl, MakeMaskingName, ModelMaskingName);

        }
        /// <summary>
        /// Created By :-Subodh Jain 17 Jan 2017
        /// Summary :- To bind wigets
        /// </summary>
        private void BindControls()
        {
            try
            {
                GlobalCityAreaEntity currentCityArea = GlobalCityArea.GetGlobalCityArea();
                ctrlPopularBikes.totalCount = 4;
                ctrlPopularBikes.CityId = Convert.ToInt32(currentCityArea.CityId);
                ctrlPopularBikes.cityName = currentCityArea.City;
                ctrlPopularBikes.MakeId = Convert.ToInt32(MakeId);
                ctrlPopularBikes.makeMasking = MakeMaskingName;
                ctrlPopularBikes.makeName = MakeName;

                ctrlUserReviewSimilarBike.ModelId = Convert.ToUInt16(modelId);
                ctrlUserReviewSimilarBike.TopCount = 4;


            }
            catch (Exception err)
            {

                ErrorClass objErr = new ErrorClass(err, "ListReviews.BindControls");
            }
        }
        void drpSort_Change(object sender, EventArgs e)
        {
            pageNumber = "1";
            ShortCriteria = drpSort.SelectedItem.Value;
            FillRepeaters();
        }

        void drpVersions_Change(object sender, EventArgs e)
        {
            pageNumber = "1";
            versionId = drpVersions.SelectedItem.Value;
            FillRepeaters();
        }

        void FillRepeaters()
        {
            CommonOpn op = new CommonOpn();

            string selectClause = "", fromClause = "", whereClause = "", orderByClause = "", recordCntQry = "";
            versionId = versionId != "" ? versionId : drpVersions.SelectedItem.Value;
            Trace.Warn("versionId :: " + versionId);
            try
            {
                if (versionId == "0" || versionId == "")
                {
                    selectClause = @" cr.id as reviewid, cu.name as customername, cu.id as customerid, '' as handlename, cr.styler, 
                                                             cr.comfortr, cr.performancer, cr.valuer, cr.fueleconomyr, cr.overallr, cr.pros, 
                                                             cr.cons,striphtml(cr.comments) as subcomments, 
                                                             cr.title, cr.entrydatetime, cr.liked, cr.disliked, cr.viewed, '' comments, 0 as threadid ";

                    fromClause = @" customers as cu inner join customerreviews as cr on cu.id = cr.customerid ";

                    whereClause = " cu.id = cr.customerid and cr.isactive=1 and cr.isverified=1 and cr.modelid = @v_modelid ";

                    orderByClause = SortingCriteria + " desc";

                    recordCntQry = string.Format(" select count(*) from {0} where {1}", fromClause, whereClause);

                }
                else
                {
                    if (!IsPostBack)
                    {
                        drpVersions.SelectedIndex = drpVersions.Items.IndexOf(drpVersions.Items.FindByValue(versionId));
                    }

                    selectClause = @" cr.id as reviewid, cu.name as customername, cu.id as customerid, '' as handlename, cr.styler, 
                                                              cr.comfortr, cr.performancer, cr.valuer, cr.fueleconomyr, cr.overallr, cr.pros, 
                                                              cr.cons,  striphtml(cr.comments) as subcomments, 
                                                              cr.title, cr.entrydatetime, cr.liked, cr.disliked, cr.viewed, '' comments, 0 as threadid ";

                    fromClause = @" customers as cu   
                                                                inner join customerreviews as cr on cu.id = cr.customerid ";

                    whereClause = " cu.id = cr.customerid and cr.isactive=1 and cr.isverified=1 and cr.versionid = @v_versionid ";

                    orderByClause = SortingCriteria + " desc";

                    recordCntQry = string.Format(" select count(*) from {0} where {1}", fromClause, whereClause);
                }



                DbParameter[] param = new[] {   DbFactory.GetDbParam("@v_modelid", DbType.Int32,modelId ),
                                                            DbFactory.GetDbParam("@v_versionid", DbType.Int32,versionId)
                                                        };



                if (pageNumber != "")
                    rpgReviews.CurrentPageIndex = Convert.ToInt32(pageNumber);
                //rpgReviews.BaseUrl = "/" + UrlRewrite.FormatSpecial(MakeName) + "-bikes/" + UrlRewrite.FormatSpecial(ModelName) + "/user-reviews/";
                rpgReviews.BaseUrl = "/" + MakeMaskingName + "-bikes/" + ModelMaskingName + "/user-reviews/";
                rpgReviews.SelectClause = selectClause;
                rpgReviews.FromClause = fromClause;
                rpgReviews.WhereClause = whereClause;
                rpgReviews.OrderByClause = orderByClause;
                rpgReviews.RecordCountQuery = recordCntQry;
                rpgReviews.SParams = param;
                rpgReviews.VersionId = versionId != "0" && versionId != "" ? versionId : "";

                rpgReviews.InitializeGrid();//initialize the grid, and this will also bind the repeater

                totalReviewCount = rpgReviews.RecordCount;


            }
            catch (Exception err)
            {

                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);

            } // catch Exception
        }

        public string HandleName(string customerName, string handleName)
        {
            string ret;
            if (handleName == "")
            {
                ret = customerName;
            }
            else
            {
                ret = "<a href=\"/community/members/" + handleName + ".html\">" + handleName + "</a>";
            }
            return ret;
        }

        public string RemoveHtmlTags(string text)
        {
            String strOutput = Regex.Replace(text, "<(.|\n)+?>", string.Empty);
            return strOutput;
        }

        public string GetVersionName(string value)
        {
            return (value == "" ? "" : "<span class='verName'>Version : " + value + "</span><br>");
        }

        public string GetComments(string value)
        {
            if (value != "")
                return RemoveHtmlTags(value.Substring(0, value.LastIndexOf(' ') == -1 ? value.Length : value.LastIndexOf(' ')).Replace("\n", "<br>"));
            else
                return "";
        }

        protected void GoogleKeywords()
        {
            string sql = "";
            uint _modelId = 0, _versionId = 0;
            if (versionId != "-1")
            {
                sql = @" select distinct cmo.makename as make, se.name as subsegment, bo.name bikebodystyle
                   from bikemodels as cmo, bikebodystyles bo,
                    (bikeversions ve   left join bikesubsegments se   on se.id = ve.subsegmentid )
                    where cmo.id=ve.bikemodelid and bo.id=ve.bodystyleid
                    and ve.id = @v_versionid";
            }
            else
            {
                sql = @" select distinct cmo.makename as make, se.name as subsegment, bo.name bikebodystyle 
                     from bikemodels as cmo, bikebodystyles bo, 
                     (bikeversions ve   left join bikesubsegments se   on se.id = ve.subsegmentid ) 
                     where cmo.id=ve.bikemodelid and bo.id=ve.bodystyleid 
                     and ve.bikemodelid = @v_modelid";
            }
            try
            {
                if (!string.IsNullOrEmpty(modelId) && !string.IsNullOrEmpty(versionId))
                {
                    uint.TryParse(versionId, out _versionId);
                    uint.TryParse(modelId, out _modelId);
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

        /* All the properties used to maintain ViewState */
        public string MakeName
        {
            get
            {
                if (ViewState["MakeName"] != null)
                    return ViewState["MakeName"].ToString();
                else
                    return "-1";
            }
            set { ViewState["MakeName"] = value; }
        }
        public double displacement
        {
            get
            {
                if (ViewState["displacement"] != null)
                    return Convert.ToDouble(ViewState["displacement"]);
                else
                    return -1;
            }
            set { ViewState["displacement"] = value; }
        }
        public double maxPower
        {
            get
            {
                if (ViewState["maxPower"] != null)
                    return Convert.ToDouble(ViewState["maxPower"]);
                else
                    return -1;
            }
            set { ViewState["maxPower"] = value; }
        }
        public double fuelEfficency
        {
            get
            {
                if (ViewState["fuelEfficency"] != null)
                    return Convert.ToDouble(ViewState["fuelEfficency"]);
                else
                    return -1;
            }
            set { ViewState["fuelEfficency"] = value; }
        }
        public double kerbWeight
        {
            get
            {
                if (ViewState["kerbWeight"] != null)
                    return Convert.ToDouble(ViewState["kerbWeight"]);
                else
                    return -1;
            }
            set { ViewState["kerbWeight"] = value; }
        }
        public bool IsDiscountinous
        {
            get
            {
                if (ViewState["IsDiscountinous"] != null)
                    return Convert.ToBoolean(ViewState["IsDiscountinous"]);
                else
                    return false;
            }
            set { ViewState["IsDiscountinous"] = value; }
        }
        public bool Futuristic
        {
            get
            {
                if (ViewState["Futuristic"] != null)
                    return Convert.ToBoolean(ViewState["Futuristic"]);
                else
                    return false;
            }
            set { ViewState["Futuristic"] = value; }
        }
        public string ModelName
        {
            get
            {
                if (ViewState["ModelName"] != null)
                    return ViewState["ModelName"].ToString();
                else
                    return "-1";
            }
            set { ViewState["ModelName"] = value; }
        }

        public string ModelMaskingName
        {
            get
            {
                if (ViewState["ModelMaskingName"] != null)
                    return ViewState["ModelMaskingName"].ToString();
                else
                    return "-1";
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
                    return "-1";
            }
            set { ViewState["MakeMaskingName"] = value; }
        }

        public string ModelIdVer
        {
            get
            {
                if (ViewState["ModelIdVer"] != null)
                    return ViewState["ModelIdVer"].ToString();
                else
                    return "-1";
            }
            set { ViewState["ModelIdVer"] = value; }
        }

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

        public double RatingOverall
        {
            get
            {
                if (ViewState["RatingOverall"] != null)
                    return Convert.ToDouble(ViewState["RatingOverall"]);
                else
                    return 0;
            }
            set { ViewState["RatingOverall"] = value; }
        }

        public double RatingLooks
        {
            get
            {
                if (ViewState["RatingLooks"] != null)
                    return Convert.ToDouble(ViewState["RatingLooks"]);
                else
                    return 0;
            }
            set { ViewState["RatingLooks"] = value; }
        }

        public double RatingPerformance
        {
            get
            {
                if (ViewState["RatingPerformance"] != null)
                    return Convert.ToDouble(ViewState["RatingPerformance"]);
                else
                    return 0;
            }
            set { ViewState["RatingPerformance"] = value; }
        }

        public double RatingComfort
        {
            get
            {
                if (ViewState["RatingComfort"] != null)
                    return Convert.ToDouble(ViewState["RatingComfort"]);
                else
                    return 0;
            }
            set { ViewState["RatingComfort"] = value; }
        }

        public double RatingFuelEconomy
        {
            get
            {
                if (ViewState["RatingFuelEconomy"] != null)
                    return Convert.ToDouble(ViewState["RatingFuelEconomy"]);
                else
                    return 0;
            }
            set { ViewState["RatingFuelEconomy"] = value; }
        }

        public double RatingValueForMoney
        {
            get
            {
                if (ViewState["RatingValueForMoney"] != null)
                    return Convert.ToDouble(ViewState["RatingValueForMoney"]);
                else
                    return 0;
            }
            set { ViewState["RatingValueForMoney"] = value; }
        }

        public string ModelStartPrice
        {
            get
            {
                if (ViewState["ModelStartPrice"] != null)
                    return ViewState["ModelStartPrice"].ToString();
                else
                    return "";
            }
            set { ViewState["ModelStartPrice"] = value; }
        }

        public string HostUrl
        {
            get
            {
                if (ViewState["HostUrl"] != null)
                    return ViewState["HostUrl"].ToString();
                else
                    return "";
            }
            set { ViewState["HostUrl"] = value; }
        }

        //Added By : Suresh Prajapati on 20 Aug 2014
        public bool IsNew
        {
            get
            {
                if (ViewState["IsNew"] != null)
                    return Convert.ToBoolean(ViewState["IsNew"]);
                else
                    return false;
            }
            set { ViewState["IsNew"] = value; }
        }

        //Added By : Suresh Prajapati on 20 Aug 2014
        public bool IsUsed
        {
            get
            {
                if (ViewState["IsUsed"] != null)
                    return Convert.ToBoolean(ViewState["IsUsed"]);
                else
                    return false;
            }
            set { ViewState["IsUsed"] = value; }
        }
        public int ModelReviewCount
        {
            get
            {
                if (ViewState["ModelReviewCount"] != null)
                    return Convert.ToInt32(ViewState["ModelReviewCount"]);
                else
                    return -1;
            }
            set { ViewState["ModelReviewCount"] = value; }
        }


        public string ShortCriteria
        {
            get
            {
                string val = "";	//default false

                if (Request.Cookies["_ShortCriteria"] != null && Request.Cookies["_ShortCriteria"].Value.ToString() != "")
                {
                    val = Request.Cookies["_ShortCriteria"].Value.ToString();
                }
                else
                {
                    val = "";
                }

                return val;
            }
            set
            {
                HttpCookie objCookie;
                objCookie = new HttpCookie("_ShortCriteria");
                objCookie.Value = value;
                objCookie.Expires = DateTime.Now.AddHours(2);
                HttpContext.Current.Response.Cookies.Add(objCookie);
            }
        }

    }//class
}//namespace