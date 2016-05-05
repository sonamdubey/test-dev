using System;
using System.Collections.Specialized;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Bikewale.Controls;

///
/// Created by umesh
///Used Bike Search Result
///
namespace Bikewale.Used
{
    public class SearchResult : Page
    {
        protected Repeater rptStockListings;
        protected RepeaterPagerUsed rpgListings;
        protected HtmlGenericControl res_msg;

        protected string QsSortCriteria = string.Empty, QsSortOrder = string.Empty, sortValue = String.Empty; 

        protected override void OnInit(EventArgs e)
        {
            rptStockListings = (Repeater)rpgListings.FindControl("rptStockListings");
            base.Load += Page_Load;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGrid();
                // Next and prev page index will be add into the headers.
                // Page index will give crawler next and previous page links.
                if (!String.IsNullOrEmpty(Request.Headers["pageIndex"]) && Request.Headers["pageIndex"].ToString() == "-1")
                {
                    Response.Headers.Add("pageIndex", (rpgListings.PrevIndex + "," + rpgListings.NextIndex));
                }
            }
        }//pageload

        /// <summary>
        /// Bind repeater with search criteria given by user
        /// Modified By : Ashwini Todkar on 17 April 2014
        ///             : Added condition to check if current page index greater than available records
        /// </summary>
        private void BindGrid()
        {
            // Get the complete query string.
            string query_string = Request.ServerVariables["QUERY_STRING"];
            Trace.Warn("query_string ", query_string);
          
            ClassifiedCookies.UsedSearchQueryString = query_string;
            Trace.Warn("search result ClassifiedCookies.UsedSearchQueryString : " + ClassifiedCookies.UsedSearchQueryString + " : end");

            // Assign above query string values in the name value collection object.
            NameValueCollection qsCollection = HttpUtility.ParseQueryString(query_string);

            // Format Sql Query
            ParseSearchCriteria objPC = new ParseSearchCriteria(qsCollection);

            Trace.Warn("query_string base url : " + query_string);

            rpgListings.BaseUrl = query_string;
            rpgListings.CityId = Request.QueryString["city"];
            rpgListings.DefaultURL = qsCollection.Get("pageUrl");
            rpgListings.SelectClause = objPC.GetSelectClause();
            rpgListings.FromClause = objPC.GetFromClause();
            rpgListings.WhereClause = objPC.GetWhereClause();
            rpgListings.OrderByClause = objPC.GetOrderByClause();
            rpgListings.RecordCountQuery = objPC.GetRecordCountQry();
            rpgListings.CmdParamQry = objPC.sqlCmdParams;
            rpgListings.CmdParamCountQry = objPC.sqlCmdParams;

            Trace.Warn("CurrentPageIndexx : ", objPC.GetCurrentPageIndex.ToString());
            if (objPC.GetCurrentPageIndex > 1)
                rpgListings.CurrentPageIndex = objPC.GetCurrentPageIndex;

            if (!String.IsNullOrEmpty(qsCollection.Get("so")) && qsCollection.Get("so") != "-1")
                sortValue = "sc=" + qsCollection.Get("sc") + "&so=" + qsCollection.Get("so"); 

            QsSortCriteria = qsCollection.Get("sc");
            QsSortOrder = qsCollection.Get("so");

            Trace.Warn("init start" + QsSortCriteria + " QsSortOrder :  " + QsSortOrder + " : objPC.GetOrderByClause() : " + objPC.GetOrderByClause());

            rpgListings.InitializeGrid(); //initialize the grid, and this will also bind the repeater

            //check for no records found
            if (rpgListings.RecordCount == 0)
            {
                rpgListings.Visible = false;
                res_msg.Visible = true;
            }
            else
            {
                //check for if page index greater than total record count
                if (((rpgListings.CurrentPageIndex - 1) * rpgListings.PageSize) + 1 > rpgListings.RecordCount)
                {
                    rpgListings.Visible = false;
                    res_msg.Visible = true;
                }
            }
        }

        //Commented By : Ashwini Todkar on 17 April 2014 not more required
        //public string SortColumnBy(string sortOrder, string sortCriteria)
        //{
        //    string url = "?";

        //    if (QsSortCriteria != string.Empty && QsSortOrder != string.Empty)
        //    {
        //        if (QsSortCriteria != sortCriteria)
        //            url += "sc=" + sortCriteria + "&so=0";
        //        else
        //            url += "sc=" + sortCriteria + "&so=" + (QsSortOrder == "1" ? "0" : "1");
        //    }
        //    else
        //        url += "sc=" + sortCriteria + "&so=" + sortOrder;

        //    return url;
        //}

        //public string GetSortImage(string sortCriteria)
        //{
        //    string sortImage = string.Empty;

        //    if (sortCriteria == QsSortCriteria)
        //    {
        //        sortImage = (QsSortOrder == "1" ? "<img src='http://imgd3.aeplcdn.com/0x0/bw/static/design15/old-images/d/sorting-down.png' border='0' />" :
        //                        "<img src='http://imgd4.aeplcdn.com/0x0/bw/static/design15/old-images/d/sorting-up.png' border='0' />");
        //    }

        //    return sortImage;
        //}

        //public string GetPathForImage(string HostUrl, string DirPath, string imgName)
        //{
        //    //return ImagingFunctions.GetImagePath("/tc/") + ValidateUser.GetBranchId() + "/" + imgName;
        //    // new code added by surendra ,date-8th Aug, because we need to make carwale & Tc imgae common         
        //    return HostUrl + DirPath + imgName;
        //}

        
        //public string IsPhotosAvailable(string photoCount)
        //{
        //    int _photoCount;
        //    string photo_str = string.Empty;
        //    string photoImg = string.Empty;

        //    if (!String.IsNullOrEmpty(photoCount))
        //    {
        //        _photoCount = Convert.ToInt32(photoCount);

        //        if (_photoCount > 0)
        //        {

        //            photo_str = _photoCount == 1 ? _photoCount + " photo available" : _photoCount + " photos available";
        //            photoImg = "<img title='" + photo_str + "' src='http://www.carwale.com/images/icons/camera.gif' border='0'/>";
        //        }
        //    }
        //    return photoImg;
        //}

        /// <summary>
        /// Written By : Ashwini Todkar on 17 April 2014
        /// Summary    : PopulateWhere to get bike fuel type text
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetFuelType(string id)
        {
            string fuelVal = "-";
            switch (id)
            {
                case "1":
                    fuelVal = "Petrol";
                    break;
                case "2":
                    fuelVal = "Diesel";
                    break;
                case "3":
                    fuelVal = "CNG";
                    break;
                case "4":
                    fuelVal = "LPG";
                    break;
                case "5":
                    fuelVal = "Electric";
                    break;
            }
            return fuelVal;
        }

        /// <summary>
        /// Written By : Ashwini Todkar on 17 April 2014
        /// Summary    : PopulateWhere to get transmission type text
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetTransmissionText(string id) 
        {
            string transVal = "-";   
            switch (id) 
            {
                case "1":
                    transVal = "Automatic";
                    break;
                case "2":
                    transVal = "Manual";
                    break;
            }

            return transVal;
        }

        //Commented by : Ashwini Todkar on 16 April 2014 - Not more required
        //public string IsPhotosAvailable(int photoCount)
        //{
        //    if (photoCount > 0)
        //    {
        //        string photo_str = photoCount == 1 ? photoCount + " photo available" : photoCount + " photos available";
        //        return " <img title='" + photo_str + "' src='http://www.carwale.com/images/icons/camera.gif' border='0'/>";
        //    }
        //    else
        //        return "";
        //}

    }//class
}//namesapce