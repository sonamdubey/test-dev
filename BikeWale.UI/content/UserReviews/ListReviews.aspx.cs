using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Bikewale.Common;
using Bikewale.Memcache;
using Microsoft.Practices.Unity;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.BikeData;
using Bikewale.Cache.Core;
using Bikewale.DAL.BikeData;
using Bikewale.Cache.BikeData;

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

        protected DropDownList drpSort, drpVersions;
        protected MakeModelVersion objBike;
        protected HtmlGenericControl spnComments;

        protected HtmlForm frmMain;

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
        private void Page_Load(object sender, EventArgs e)
        {
            //code for device detection added by Ashwini Todkar
            DeviceDetection dd = new DeviceDetection(Request.ServerVariables["HTTP_X_REWRITE_URL"].ToString());
            dd.DetectDevice();


            //also get the forumId
            if (Request["bikem"] != null && Request.QueryString["bikem"] != "")
            {
                ModelMaskingResponse objResponse = null;

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

               
                //ModelMapping mm = new ModelMapping();
                //modelId = mm.GetModelId(Request.QueryString["bikem"]);
                //Trace.Warn( "Model Name : ",Request.QueryString["bikem"]);
                //Trace.Warn("model name id :", modelId);
                //verify the id as passed in the url
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
                Trace.Warn("modelId " + versionId);
               
                //verify the id as passed in the url
                if (CommonOpn.CheckId(versionId) == false)
                {
                    //redirect to the error page                 

                    Response.Redirect(CommonOpn.AppPath + "pageNotFound.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                }
            }
            Trace.Warn("start post back ");
           
            if (!IsPostBack)
            {
                //objBike = new MakeModelVersion();
                
              //  ModelStartPrice = objBike.GetModelStartingPrice(modelId);
                ModelVersionDescription objBike; 
                if (modelId != "")
                {
                    objBike = new ModelVersionDescription();
                    objBike.GetDetailsByModel(modelId);
                    ModelStartPrice = objBike.ModelBasePrice;
                    
                    BikeName = objBike.BikeName;
                    LargePic = objBike.LargePic;

                    RatingOverall = objBike.ModelRatingOverall;
                    RatingLooks = objBike.ModelRatingLooks;
                    RatingPerformance = objBike.ModelRatingPerformance;
                    RatingComfort = objBike.ModelRatingComfort;
                    RatingFuelEconomy = objBike.ModelRatingFuelEconomy;
                    RatingValueForMoney = objBike.ModelRatingValueForMoney;

                    MakeName = objBike.MakeName;
                    ModelName = objBike.ModelName;
                    ModelMaskingName = objBike.ModelMaskingName;
                    MakeMaskingName = objBike.MakeMaskingName;
                    HostUrl = objBike.HostUrl;
                    IsNew = objBike.IsNew;
                    IsUsed = objBike.IsUsed;
                    OriginalImagePath = objBike.OriginalImagePath;
                    Trace.Warn("MakeName : " + MakeName + "ModelName : " + ModelName + " LargePic : " + LargePic);
                }
                else
                {
                    objBike = new ModelVersionDescription();
                    objBike.GetDetailsByVersion(versionId);
                    ModelStartPrice = objBike.ModelBasePrice;
                    BikeName = objBike.BikeName;
                    LargePic = objBike.LargePic;
                    RatingOverall = objBike.ModelRatingOverall;
                    RatingLooks = objBike.ModelRatingLooks;
                    RatingPerformance = objBike.ModelRatingPerformance;
                    RatingComfort = objBike.ModelRatingComfort;
                    RatingFuelEconomy = objBike.ModelRatingFuelEconomy;
                    RatingValueForMoney = objBike.ModelRatingValueForMoney;

                    MakeName = objBike.MakeName;
                    ModelName = objBike.ModelName;
                    ModelMaskingName = objBike.ModelMaskingName;
                    MakeMaskingName = objBike.MakeMaskingName;
                    HostUrl = objBike.HostUrl;
                    IsNew = objBike.IsNew;
                    IsUsed = objBike.IsUsed;
                    OriginalImagePath = objBike.OriginalImagePath;
                    Trace.Warn("MakeName : " + MakeName + "ModelName : " + ModelName + " LargePic : " + LargePic);
                }

                Trace.Warn("RatingOverall : " + RatingOverall);

                // This static function bind all versions having reviews count greter then zero
                // for selected model.
                FillControls.FillReviewedVersions(drpVersions, modelId);

                FillRepeaters();

                // Rewrite form action property with the rewritten url 
                // just to make url consistent
                // onpostback select first page


              //  frmMain.Action = "/content/" + UrlRewrite.FormatSpecial(MakeName) + "-bikes/" + UrlRewrite.FormatSpecial(ModelName) + "/userreviews-p1/";
            }

            GoogleKeywords();
        }//pageload


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

            Database db = new Database();

            string selectClause = "", fromClause = "", whereClause = "", orderByClause = "", recordCntQry = "";
            versionId = versionId != "" ? versionId : drpVersions.SelectedItem.Value;
            Trace.Warn("versionId :: " + versionId);
            try
            {
                if (versionId == "0" || versionId == "")
                {
                    selectClause = " CR.ID AS ReviewId, CU.Name AS CustomerName, CU.ID AS CustomerId, ISNULL(UP.HandleName, '') As HandleName, CR.StyleR, "
                                + " CR.ComfortR, CR.PerformanceR, CR.ValueR, CR.FuelEconomyR, CR.OverallR, CR.Pros, "
                                + " CR.Cons, Substring(CR.Comments,0,Cast(Floor(LEN(CR.Comments)*0.15) AS INT)) AS SubComments, "
                                + " CR.Title, CR.EntryDateTime, CR.Liked, CR.Disliked, CR.Viewed, ISNULL(Fm.Posts, 0) Comments, Fso.ThreadId ";

                    fromClause = " Customers AS CU With(NoLock) LEFT JOIN UserProfile UP With(NoLock) ON UP.UserId = CU.ID, CustomerReviews AS CR With(NoLock) LEFT JOIN Forum_ArticleAssociation Fso With(NoLock) ON CR.ID = Fso.ArticleId "
                                + " LEFT JOIN Forums Fm With(NoLock) ON Fso.ThreadId = Fm.ID ";

                    whereClause = " CU.ID = CR.CustomerId AND CR.IsActive=1 AND CR.IsVerified=1 AND CR.ModelId = @ModelId ";

                    orderByClause = SortingCriteria + " DESC";

                    recordCntQry = " Select Count(*) From " + fromClause + " Where " + whereClause;
                }
                else
                {
                    if (!IsPostBack)
                    {
                        drpVersions.SelectedIndex = drpVersions.Items.IndexOf(drpVersions.Items.FindByValue(versionId));
                    }

                    selectClause = " CR.ID AS ReviewId, CU.Name AS CustomerName, CU.ID AS CustomerId, ISNULL(UP.HandleName, '') As HandleName, CR.StyleR, "
                                 + " CR.ComfortR, CR.PerformanceR, CR.ValueR, CR.FuelEconomyR, CR.OverallR, CR.Pros, "
                                 + " CR.Cons, Substring(CR.Comments,0,Cast(Floor(LEN(CR.Comments)*0.15) AS INT)) AS SubComments, "
                                 + " CR.Title, CR.EntryDateTime, CR.Liked, CR.Disliked, CR.Viewed, ISNULL(Fm.Posts, 0) Comments, Fso.ThreadId ";

                    fromClause = " Customers AS CU With(NoLock) LEFT JOIN UserProfile UP With(NoLock) ON UP.UserId = CU.ID, CustomerReviews AS CR With(NoLock) LEFT JOIN Forum_ArticleAssociation Fso With(NoLock) ON CR.ID = Fso.ArticleId "
                                + " LEFT JOIN Forums Fm With(NoLock) ON Fso.ThreadId = Fm.ID ";

                    whereClause = " CU.ID = CR.CustomerId AND CR.IsActive=1 AND CR.IsVerified=1 AND CR.VersionId = @VersionId ";

                    orderByClause = SortingCriteria + " DESC";

                    recordCntQry = " Select Count(*) From " + fromClause + " Where " + whereClause;
                }



                SqlParameter[] param = 
				{
					new SqlParameter("@ModelId", modelId),
					new SqlParameter("@VersionId", versionId)
				};

                Trace.Warn("pageNumber :  : : " + pageNumber);
                if (pageNumber != "")
                    rpgReviews.CurrentPageIndex = Convert.ToInt32(pageNumber);

                Trace.Warn("Testing");
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
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
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
            SqlDataReader dr = null;

            if (versionId != "")
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
            cmd.Parameters.Add("@Id", SqlDbType.BigInt).Value = versionId != "" ? versionId : "-1";
            cmd.Parameters.Add("@BikeModelId", SqlDbType.BigInt).Value = modelId != "" ? modelId : "-1";

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
            set{ ViewState["MakeMaskingName"] = value; }          
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