using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using Bikewale.Common;
using Bikewale.Controls;
using Bikewale.DAL.BikeData;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.PriceQuote;
using Bikewale.Entities.SEO;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Microsoft.Practices.Unity;
using MySql.CoreDAL;
using System;
using System.Data;
using System.Data.Common;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Bikewale.New
{
    /// <summary>
    /// Modified by: Sangram Nandkhile on 24 Nov 2016
    /// Summary: Added makename and helper function
    /// </summary>
    public class UpcomingBikesList : Page
    {
        protected Repeater rptLaunches;
        protected RepeaterPager rpgUpcomingBikes;
        public int currentYear = DateTime.Now.Year;
        protected string PageNumber = string.Empty, SelectClause = string.Empty, FromClause = string.Empty, WhereClause = string.Empty,
            OrderByClause = string.Empty, BaseUrl = string.Empty, RecordCntQry = string.Empty, prevUrl = string.Empty, nextUrl = string.Empty,
            pageTitle = string.Empty, makeMaskingName = string.Empty, makeName = string.Empty;
        protected UpcomingBikeSearch UpcomingBikeSearch;
        protected Bikewale.Controls.NewBikeLaunches ctrl_NewBikeLaunches;
        protected DropDownList drpSort;
        protected HtmlGenericControl alertObj;
        protected bool isMake = false;
        protected PageMetaTags meta = null;
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
            string makeId = string.Empty, sort = string.Empty;
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
                    if (Bikewale.Common.CommonOpn.CheckId(Request.QueryString["pn"]) == true)
                        PageNumber = Request.QueryString["pn"];
                }
                if (!String.IsNullOrEmpty(Request.QueryString["sort"]))
                {
                    sort = Request.QueryString["sort"].ToString();
                    UpcomingBikeSearch.Sort = sort;
                }
                if (!String.IsNullOrEmpty(Request.QueryString["make"]))
                {
                    makeMaskingName = Request.QueryString["make"];
                    MakeMaskingResponse objMakeResponse = null;
                    try
                    {
                        using (IUnityContainer containerInner = new UnityContainer())
                        {
                            containerInner.RegisterType<IBikeMakesCacheRepository, BikeMakesCacheRepository>()
                                  .RegisterType<ICacheManager, MemcacheManager>()
                                  .RegisterType<IBikeMakes<BikeMakeEntity, int>, BikeMakesRepository<BikeMakeEntity, int>>()
                                 ;
                            var objCache = containerInner.Resolve<IBikeMakesCacheRepository>();
                            objMakeResponse = objCache.GetMakeMaskingResponse(makeMaskingName);
                        }
                    }
                    catch (Exception ex)
                    {
                        ErrorClass.LogError(ex, Request.ServerVariables["URL"] + "ParseQueryString");

                        UrlRewrite.Return404();
                    }
                    finally
                    {
                        if (objMakeResponse != null)
                        {
                            if (objMakeResponse.StatusCode == 200)
                            {
                                makeId = Convert.ToString(objMakeResponse.MakeId);
                                isMake = true;
                                var objMake = new MakeHelper().GetMakeNameByMakeId(Convert.ToUInt16(makeId));
                                if (objMake != null)
                                {
                                    makeName = objMake.MakeName;
                                }
                            }
                            else if (objMakeResponse.StatusCode == 301)
                            {
                                CommonOpn.RedirectPermanent(Request.RawUrl.Replace(makeMaskingName, objMakeResponse.MaskingName));
                            }
                            else
                            {
                                UrlRewrite.Return404();
                            }
                        }
                        else
                        {
                            UrlRewrite.Return404();
                        }
                    }

                    if (isMake)
                    {
                        UpcomingBikeSearch.MakeId = makeId;
                        FetchUpcomingBikes(makeId, makeMaskingName, sort);
                    }
                    else
                    {
                        UrlRewrite.Return404();
                    }
                }
                else
                {
                    FillRepeaters(sort);
                }
            }
            if (String.IsNullOrEmpty(makeId) && String.IsNullOrEmpty(sort))
                CreatePrevNextUrl();
            CreateMetaTags();
        }   // End of Page Load

        /// <summary>
        /// Written By : Sangram Nandkhile on 23 Nov 2016
        /// Summary : To create meta and title
        /// </summary>
        private void CreateMetaTags()
        {
            meta = new PageMetaTags();
            if (isMake)
            {
                meta.Title = string.Format("Upcoming {1} Bikes in India - Expected {1} Bike New Launches in {0}", currentYear, makeName);
                meta.Description = string.Format("Check out upcoming {1} bikes in {0} in India. From small to super-luxury, from announced to highly speculated models, from near future to end of year, know about every upcoming bike launch in India this year.", currentYear, makeName);
                meta.Keywords = string.Format("Upcoming {1} bikes, new upcoming {1} launches, upcoming {1} bike launches, upcoming {1} models, future {1} bikes, future {1} bike launches, {0} {1} bikes, speculated {1} launches, futuristic {1} models", currentYear, makeName);
                meta.CanonicalUrl = string.Format("https://www.bikewale.com/{0}-bikes/upcoming/", makeMaskingName);
                meta.AlternateUrl = string.Format("https://www.bikewale.com/m/{0}-bikes/upcoming/", makeMaskingName);
                pageTitle = string.Format("Upcoming {0} Bikes in India", makeName);

            }
            else
            {
                meta.Title = string.Format("Upcoming Bikes in India - Expected Launches in {0}", currentYear);
                meta.Description = string.Format("Find out upcoming new bikes in {0} in India. From small to super-luxury, from announced to highly speculated models, from near future to end of year, know about every upcoming bike launch in India this year.", currentYear);
                meta.Keywords = string.Format("Upcoming bikes, new upcoming launches, upcoming bike launches, upcoming models, future bikes, future bike launches, {0} bikes, speculated launches, futuristic models", currentYear);
                meta.CanonicalUrl = "https://www.bikewale.com/upcoming-bikes/";
                meta.AlternateUrl = "https://www.bikewale.com/m/upcoming-bikes/";
                pageTitle = "Upcoming Bikes in India";
            }
        }

        private void CreatePrevNextUrl()
        {
            string mainUrl = isMake ? string.Format("https://www.bikewale.com/{0}-bikes/upcoming/page/", makeMaskingName) : "https://www.bikewale.com/upcoming-bikes/page/";
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
        }

        #region Fetch Upcoming Bikes (Based on Make)
        /// <summary>
        /// PopulateWhere to fetch data on Upcoming bikes based on Make
        /// </summary>
        /// <param name="makeId">Bike Make Id</param>
        /// <param name="mkMaskingName">Bike Make Name</param>
        /// <param name="sort">Criteria on which the data is sorted.</param>
        private void FetchUpcomingBikes(string makeId, string mkMaskingName, string sort)
        {
            DbCommand cmd = DbFactory.GetDBCommand();
            SelectClause = " mo.makename, mo.makemaskingname as makemaskingname , mo.name as modelname,mo.maskingname as modelmaskingname, ecl.expectedlaunch, ecl.estimatedpricemin, ecl.estimatedpricemax, ecl.hosturl, ecl.largepicimagepath, csy.smalldescription as description, ecl.originalimagepath ";
            FromClause = @" expectedbikelaunches ecl 
                            left join bikesynopsis csy  on ecl.bikemodelid = csy.modelid and csy.isactive = 1 
                            inner join bikemodels mo   on ecl.bikemodelid = mo.id ";
            // inner join bikemakes mk   on mk.id = mo.bikemakeid ";
            WhereClause = " mo.futuristic = 1 and ecl.islaunched = 0  and ecl.isdeleted = 0 and mo.IsDeleted = 0 ";
            if (makeId != string.Empty)
            {
                WhereClause += "and mo.bikemakeid = @makeid ";
            }
            OrderByClause = GetSortCriteria(sort);
            if (sort != string.Empty)
                BaseUrl = "/" + mkMaskingName + "-bikes/upcoming/sort/" + sort + "/";
            else
                BaseUrl = "/" + mkMaskingName + "-bikes/upcoming/";
            cmd.Parameters.Add(DbFactory.GetDbParam("@makeid", DbType.Int32, makeId));
            RecordCntQry = " select count(*) from " + FromClause + " where " + WhereClause;
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
                DbCommand cmd = DbFactory.GetDBCommand();

                SelectClause = " mk.name makename,mk.maskingname as makemaskingname , mo.name as modelname,mo.maskingname as modelmaskingname, ecl.expectedlaunch, ecl.estimatedpricemin, ecl.estimatedpricemax, ecl.hosturl, ecl.largepicimagepath, csy.smalldescription as description, ecl.originalimagepath ";
                FromClause = @" expectedbikelaunches ecl
                            left join bikesynopsis csy  on ecl.bikemodelid = csy.modelid and csy.isactive = 1 
                            inner join bikemodels mo   on ecl.bikemodelid = mo.id 
                            inner join bikemakes mk   on mk.id = mo.bikemakeid ";
                WhereClause = " mo.futuristic = 1 and ecl.islaunched = 0  and ecl.isdeleted = 0 ";
                OrderByClause = GetSortCriteria(sort);
                if (sort != string.Empty)
                    BaseUrl = "/upcoming-bikes/sort/" + sort + "/";
                else
                    BaseUrl = "/upcoming-bikes/";

                RecordCntQry = " select count(*) from " + FromClause + " where " + WhereClause;

                BindData(cmd);
            }
            catch (Exception err)
            {
                ErrorClass.LogError(err, Request.ServerVariables["URL"]);

            } // catch Exception
        }
        #endregion

        #region BindData
        /// <summary>
        /// PopulateWhere to bind the data retrieved to the repeater
        /// </summary>
        /// <param name="cmd">Sql Command Object</param>
        void BindData(DbCommand cmd)
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
                rpgUpcomingBikes.CmdParamR = cmd;	//pass the parameter values for the record count                
                //initialize the grid, and this will also bind the repeater
                rpgUpcomingBikes.InitializeGrid();
                recordCount = rpgUpcomingBikes.RecordCount;
                if (recordCount == 0)
                {
                    alertObj.Visible = true;
                    alertObj.InnerText = "BikeWale doesn't have information for upcoming bikes from this manufacturer at this point. Please check again later.";
                }
                else
                {
                    alertObj.Visible = false;
                }
            }
            catch (Exception err)
            {
                ErrorClass.LogError(err, Request.ServerVariables["URL"]);

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
                    orderByClause = " ecl.estimatedpricemin asc ";
                    break;
                case "2":
                    orderByClause = " ecl.estimatedpricemin desc ";
                    break;
                case "3":
                    orderByClause = " ecl.launchdate asc ";
                    break;
                case "4":
                    orderByClause = " ecl.launchdate desc ";
                    break;
                default:
                    orderByClause = " ecl.launchdate ";
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
                formattedPrice = Bikewale.Common.CommonOpn.FormatNumeric(price);
            }
            return formattedPrice;
        }   // End of GetFormattedPrice function


    }   // End of class
}   // End of namespace