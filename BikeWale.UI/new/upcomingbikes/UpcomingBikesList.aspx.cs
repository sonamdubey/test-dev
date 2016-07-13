using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Bikewale.Common;
using Bikewale.Controls;
using System.Data;
using System.Data.SqlClient;
using Bikewale.Memcache;
using Bikewale.Entities.PriceQuote;

namespace Bikewale.New
{    
    public class UpcomingBikesList : Page
    {
        protected Repeater rptLaunches;
        protected RepeaterPager rpgUpcomingBikes;
        public int serial = 0;
        protected string PageNumber = string.Empty, SelectClause = string.Empty, FromClause = string.Empty, WhereClause = string.Empty,
            OrderByClause = string.Empty, BaseUrl = string.Empty, RecordCntQry = string.Empty, prevUrl = string.Empty,nextUrl = string.Empty;
        protected UpcomingBikeSearch UpcomingBikeSearch;
        protected Bikewale.Controls.NewBikeLaunches ctrl_NewBikeLaunches;
        protected DropDownList drpSort;
        protected HtmlGenericControl alertObj;

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
            rptLaunches = (Repeater)rpgUpcomingBikes.FindControl("rptLaunches");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string makeId = string.Empty, makeName = string.Empty, sort = string.Empty;
            alertObj.Visible = false;
            // Modified By :Lucky Rathore on 12 July 2016.
            Form.Action = Request.RawUrl;
            //code for device detection added by Ashwini Todkar
            // Modified By :Ashish Kamble on 5 Feb 2016
            string originalUrl = Request.ServerVariables["HTTP_X_ORIGINAL_URL"];
            if (String.IsNullOrEmpty(originalUrl))
                originalUrl = Request.ServerVariables["URL"];

            DeviceDetection dd = new DeviceDetection(originalUrl);
            dd.DetectDevice();

            ctrl_NewBikeLaunches.PQSourceId = (int)PQSourceEnum.Desktop_Upcoming_NewLaunches;

            if (!IsPostBack)
            {              
                if (Request["pn"] != null && Request.QueryString["pn"] != "")
                {
                    if (CommonOpn.CheckId(Request.QueryString["pn"]) == true)
                        PageNumber = Request.QueryString["pn"];
                    Trace.Warn("pn: " + Request.QueryString["pn"]);
                }
                if (!String.IsNullOrEmpty(Request.QueryString["sort"]))
                {
                    sort = Request.QueryString["sort"].ToString();
                    UpcomingBikeSearch.Sort = sort;
                }
                if (!String.IsNullOrEmpty(Request.QueryString["make"]))
                {
                    makeId = MakeMapping.GetMakeId(Request.QueryString["make"]);
                    if (!string.IsNullOrEmpty(makeId))
                    {
                        makeName = Request.QueryString["make"].ToString();
                        UpcomingBikeSearch.MakeId = makeId;
                        FetchUpcomingBikes(makeId, makeName, sort);
                    }
                    else
                    {
                        Response.Redirect("/pagenotfound.aspx",false);
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                        this.Page.Visible = false;
                    }
                }
                else
                {
                    FillRepeaters(sort);
                }
            }
            if (String.IsNullOrEmpty(makeId) && String.IsNullOrEmpty(sort))
                CreatePrevNextUrl();
        }   // End of Page Load

        private void CreatePrevNextUrl()
        {
            string mainUrl = "http://www.bikewale.com/upcoming-bikes/page/";
            string prevPageNumber = string.Empty, nextPageNumber = string.Empty;

            if (PageNumber == string.Empty || PageNumber == "1")    //if page is first page
            {
                nextPageNumber = "2";
                nextUrl = mainUrl + nextPageNumber + "/";
            }
            else if (int.Parse(PageNumber) == rpgUpcomingBikes.totalPages)    //if page is last page
            {
                prevPageNumber = (int.Parse(PageNumber) - 1).ToString();
                prevUrl = mainUrl + prevPageNumber + "/";
            }
            else
            {          //for middle pages
                prevPageNumber = (int.Parse(PageNumber) - 1).ToString();
                prevUrl = mainUrl + prevPageNumber + "/";
                nextPageNumber = (int.Parse(PageNumber) + 1).ToString();
                nextUrl = mainUrl + nextPageNumber + "/";
            }
            //Trace.Warn("-----previous page url is :  " + prevUrl);
            //Trace.Warn("-----Next page url is :  " + nextUrl);
        }

        #region Fetch Upcoming Bikes (Based on Make)
        /// <summary>
        /// PopulateWhere to fetch data on Upcoming bikes based on Make
        /// </summary>
        /// <param name="makeId">Bike Make Id</param>
        /// <param name="makeName">Bike Make Name</param>
        /// <param name="sort">Criteria on which the data is sorted.</param>
        private void FetchUpcomingBikes(string makeId, string makeName, string sort)
        {
            SqlCommand cmd = new SqlCommand();
            SelectClause = " MK.Name MakeName,MK.MaskingName AS MakeMaskingName , Mo.Name AS ModelName,Mo.MaskingName as ModelMaskingName, ECL.ExpectedLaunch, ECL.EstimatedPriceMin, ECL.EstimatedPriceMax, ECL.HostURL, ECL.LargePicImagePath, Csy.SmallDescription AS Description, ECL.OriginalImagePath ";
            FromClause = " ExpectedBikeLaunches ECL With(NoLock) "
                + " LEFT JOIN BikeSynopsis Csy With(NoLock) ON ECL.BikeModelId = Csy.ModelId AND Csy.IsActive = 1 "
                + " INNER JOIN BikeModels Mo With(NoLock) ON ECL.BikeModelId = Mo.ID "
                + " INNER JOIN BikeMakes MK With(NoLock) ON MK.ID = Mo.BikeMakeId ";
            WhereClause = " Mo.Futuristic = 1 AND ECL.isLaunched = 0  AND ECL.IsDeleted = 0 ";
            if (makeId != string.Empty)
            {
                WhereClause += "AND MK.ID = @MakeId ";
            }
            OrderByClause = GetSortCriteria(sort);
            if (sort != string.Empty)
                BaseUrl = "/" + makeName + "-bikes/upcoming/sort/" + sort + "/";
            else
                BaseUrl = "/" + makeName + "-bikes/upcoming/";
            //if(PageNumber != string.Empty)
                //BaseUrl = "/" + makeName + "-bikes/upcoming/page/" + PageNumber + "/";

            cmd.Parameters.Add("@MakeId", SqlDbType.Int).Value = makeId;

            RecordCntQry = " Select Count(*) From " + FromClause + " Where " + WhereClause;

            BindData(cmd);
        }
        #endregion

        #region Fill Repeater
        /// <summary>
        /// PopulateWhere to fetch data on Upcoming bikes
        /// </summary>
        /// <param name="sort">Criteria on which the data is sorted.</param>
        void FillRepeaters(string sort)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();

                SelectClause = " MK.Name MakeName, MK.MaskingName AS MakeMaskingName, Mo.Name AS ModelName,Mo.MaskingName as ModelMaskingName, ECL.ExpectedLaunch, ECL.EstimatedPriceMin, ECL.EstimatedPriceMax, ECL.HostURL, ECL.LargePicImagePath, Csy.SmallDescription AS Description, ECL.OriginalImagePath ";
                FromClause = " ExpectedBikeLaunches ECL With(NoLock) "
                    + " LEFT JOIN BikeSynopsis Csy With(NoLock) ON ECL.BikeModelId = Csy.ModelId AND Csy.IsActive = 1 "
                    + " INNER JOIN BikeModels Mo With(NoLock) ON ECL.BikeModelId = Mo.ID "
                    + " INNER JOIN BikeMakes MK With(NoLock) ON MK.ID = Mo.BikeMakeId ";
                WhereClause = " Mo.Futuristic = 1 AND ECL.isLaunched = 0 AND ECL.IsDeleted = 0 ";
                OrderByClause = GetSortCriteria(sort);
                if (sort != string.Empty)
                    BaseUrl = "/upcoming-bikes/sort/" + sort + "/";
                else
                    BaseUrl = "/upcoming-bikes/";

                RecordCntQry = " Select Count(*) From " + FromClause + " Where " + WhereClause;

                BindData(cmd);
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            } // catch Exception
        }
        #endregion

        #region BindData
        /// <summary>
        /// PopulateWhere to bind the data retrieved to the repeater
        /// </summary>
        /// <param name="cmd">Sql Command Object</param>
        void BindData(SqlCommand cmd)
        {
            try
            {
                if (PageNumber != string.Empty)
                    rpgUpcomingBikes.CurrentPageIndex = Convert.ToInt32(PageNumber);

                //form the base url. 
                string qs = Request.ServerVariables["QUERY_STRING"];
                int recordCount;

                rpgUpcomingBikes.BaseUrl = BaseUrl;

                rpgUpcomingBikes.SelectClause = SelectClause;
                rpgUpcomingBikes.FromClause = FromClause;
                rpgUpcomingBikes.WhereClause = WhereClause;
                rpgUpcomingBikes.OrderByClause = OrderByClause;
                rpgUpcomingBikes.RecordCountQuery = RecordCntQry;
                rpgUpcomingBikes.CmdParamQ = cmd;	//pass the parameter values for the query
                rpgUpcomingBikes.CmdParamR = cmd.Clone();	//pass the parameter values for the record count                
                //initialize the grid, and this will also bind the repeater
                rpgUpcomingBikes.InitializeGrid();
                recordCount = rpgUpcomingBikes.RecordCount;
                Trace.Warn("recordCount: " + recordCount.ToString());
                if (recordCount == 0)
                {
                    alertObj.Visible = true;
                    alertObj.InnerText = "BikeWale doesn't have information for upcoming bikes from this manufacturer at this point. Please check again later.";
                }
                else
                {
                    alertObj.Visible = false;
                }
                Trace.Warn("BaseURL" + BaseUrl);
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }
        #endregion

        #region Sort Criteria
        /// <summary>
        /// PopulateWhere to sort Data retrieved
        /// </summary>
        /// <param name="sort">Criteria on which the data will be sorted</param>
        /// <returns></returns>
        private string GetSortCriteria(string sort)
        {
            string orderByClause = string.Empty;
            switch (sort)
            {
                case "1":
                    orderByClause = " ECL.EstimatedPriceMin ASC ";
                    break;
                case "2":
                    orderByClause = " ECL.EstimatedPriceMin DESC ";
                    break;
                case "3":
                    orderByClause = " ECL.LaunchDate ASC ";
                    break;
                case "4":
                    orderByClause = " ECL.LaunchDate DESC ";
                    break;
                default:
                    orderByClause = " ECL.LaunchDate ";
                    break;
            }
            return orderByClause;
        }
        #endregion

        /// <summary>
        ///     Written By : Ashish G. Kamble on 28/9/2012
        ///     Function will return the formatted price
        /// </summary>
        /// <param name="price"></param>
        /// <returns></returns>
        protected string GetFormattedPrice(string price)
        {
            string formattedPrice = string.Empty;

            if (String.IsNullOrEmpty(price))
            {
                formattedPrice = "N/A";
            }
            else
            { 
                //formattedPrice = price.Replace(".00", "");
                formattedPrice = CommonOpn.FormatNumeric(price);
            }
            return formattedPrice;
        }   // End of GetFormattedPrice function

    
    }   // End of class
}   // End of namespace