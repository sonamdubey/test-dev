using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Collections.Specialized;
using System.Data;
using System.Text;
using Bikewale.Common;
using Bikewale.Controls;

namespace Bikewale.New
{
    public class search_result : System.Web.UI.Page
    {
        protected RepeaterPagerAjax rpgListings;
        protected Repeater rptListings;

        protected HtmlGenericControl res_msg;

        // protected variables
        protected string QsSortCriteria = string.Empty, QsSortOrder = string.Empty, PageNumber = string.Empty;

        protected override void OnInit(EventArgs e)
        {
            rptListings = (Repeater)rpgListings.FindControl("rptListings");
            InitializeComponent();
        }

        void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
        }

        void Page_Load(object Sender, EventArgs e)
        {
            Trace.Warn("HEre");
            string query_string = Request.ServerVariables["QUERY_STRING"];
            NameValueCollection qsCollection = Request.QueryString;
            // Class to process parameters selected by the user		
            ParseSearchCriterias objPC = new ParseSearchCriterias(qsCollection);

            bool isValid = objPC.ValidateParamIndex("8,2", "9");

            Trace.Warn("isValid: " + isValid.ToString());

            Trace.Warn("objPC.IsCriteriasParsed: " + objPC.IsCriteriasParsed.ToString());

            if (objPC.IsCriteriasParsed)
            {
                rpgListings.BaseUrl = "/new/search_result.aspx?" + query_string;
                Trace.Warn("query_string: " + query_string);
                rpgListings.SelectClause = objPC.GetSelectClause();
                //Trace.Warn("Select Query ...:", rpgListings.SelectClause);
                rpgListings.FromClause = objPC.GetFromClause();
                rpgListings.WhereClause = objPC.GetWhereClause();
                rpgListings.OrderByClause = objPC.GetOrderByClause();
                rpgListings.RecordCountQuery = objPC.GetRecordCountQry();
                rpgListings.CmdParamQ = objPC.sqlCmdParams;
                rpgListings.CmdParamR = objPC.sqlCmdParams.Clone();

                if (objPC.GetCurrentPageIndex > 1)
                    rpgListings.CurrentPageIndex = objPC.GetCurrentPageIndex;

                QsSortCriteria = qsCollection.Get("sc");
                QsSortOrder = qsCollection.Get("so");

                rpgListings.InitializeGrid();//initialize the grid, and this will also bind the repeater

                if (rpgListings.RecordCount == 0)
                {
                    rpgListings.Visible = false;
                    res_msg.Visible = true;
                }

                // Note: Page size will be 50 by default and it will be fixed, user cant chancg page size
            }
        } // Page_Load

        public string SortColumnBy(string sortOrder, string sortCriteria)
        {
            string url = string.Empty;

            if (QsSortCriteria != string.Empty && QsSortOrder != string.Empty)
            {
                if (QsSortCriteria != sortCriteria)
                    url = rpgListings.baseUrlForPs + "&sc=" + sortCriteria + "&so=0";
                else
                    url = rpgListings.baseUrlForPs + "&sc=" + sortCriteria + "&so=" + (QsSortOrder == "1" ? "0" : "1");
            }
            else
                url = rpgListings.baseUrlForPs + "&sc=" + sortCriteria + "&so=" + sortOrder;

            return url;
        }

        public string GetSortImage(string sortCriteria)
        {
            string sortImage = string.Empty;

            if (sortCriteria == QsSortCriteria)
            {
                sortImage = (QsSortOrder == "1" ? "<img src='http://imgd1.aeplcdn.com/0x0/bw/static/design15/old-images/d/sorting-down.png' border='0' />" :
                                "<img src='http://imgd2.aeplcdn.com/0x0/bw/static/design15/old-images/d/sorting-up.png' border='0' />");
            }

            return sortImage;
        }
        
        /// <summary>
        /// Function to prepare prepare link of matching versions
        /// </summary>
        /// <param name="modelRank"></param>
        /// <param name="modId">Model Id</param>
        /// <param name="modelCount">Model Count</param>
        /// <returns>String of prepare link of matching versions</returns>
        public string GetImageDetails(string modelRank, string modId, string modelCount)
        {
            string retVal = "";

            if (modelRank == "1")
            {
                //Setting model count to a variable
                int modCnt = Convert.ToInt32(modelCount);

                if (modCnt > 0)
                    retVal = "<span id='spnModel_" + modId + "' class='spnExpand' onclick=\"javascript:Expand('" + modId + "');\" ></span><span id='spnData_" + modId + "' >View " + (modCnt) + " matching versions.</span>"
                           + " <input type='hidden' id='hdnView_" + modId + "' value='" + modCnt + "'/>";
            }

            return retVal;
        }

        /// <summary>
        /// Function prepares row for dispalying models
        /// </summary>
        /// <param name="modelRank"></param>
        /// <param name="modelId">Model id</param>
        /// <param name="modelCount">Count of matching versions</param>
        /// <param name="bikeModel">Model name like Maruti-Suzuki Swift</param>
        /// <param name="modelReviewRate">Model review rating given by the users (out of 5)</param>
        /// <param name="minPrice">Minimum price of the Model</param>
        /// <param name="maxPrice">Maximum price of the model</param>
        /// <param name="originalImagePath">Thumb image of the model</param>
        /// <param name="modelReviewCount">Model review count</param>
        /// <param name="makeName">Name of the Make like Maruti-Suzuki</param>
        /// <param name="modelName">Name of the Model like Swift</param>
        /// <returns></returns>
        public string GetModelRow(string modelRank, string modelId, string modelCount, string bikeModel, string modelReviewRate,
                                   string minPrice, string maxPrice, string hostUrl, string originalImagePath, string modelReviewCount, string makeName, string modelMappingName,string makeMappingname)
        {
            StringBuilder sb = new StringBuilder();

            if (modelRank == "1")
            {

                string imgSrc = originalImagePath == "" ? "http://imgd1.aeplcdn.com/0x0/bw/static/design15/old-images/d/nobike.jpg" : MakeModelVersion.GetModelImage(hostUrl, originalImagePath, Bikewale.Utility.ImageSize._110x61);
                Trace.Warn("Bike Model :: ",bikeModel);
                Trace.Warn("Bike Model Name:::", modelMappingName);
                Trace.Warn("Bike Make Name:::", makeMappingname);
                Trace.Warn("imgSrc",imgSrc);
                sb.Append("<tr id='mod_" + modelId + "' class='model-row version-row dt_body'>");
                sb.Append("<td><img alt='" + bikeModel + "' title='" + bikeModel + "' src='" + imgSrc + "' /></td>");
                sb.Append("<td><a class='href-title' href='/" + makeMappingname + "-bikes/" + modelMappingName + "/'>" + bikeModel);
                sb.Append("</a><p class='text-grey'><a id='" + modelId + "' class='text-grey viewVersions'><span id='modShowIcon' class='icon-sheet right-arrow'></span><span id='modShow' class='show' title='Click to expand'>View " + (modelCount) + " matching versions.</span><span id='modHide' class='hide' title='Click to hide'>Hide versions.</span></a></p>");
                sb.Append("</td>");
                sb.Append("<td class='price2'>Starts At Rs. " + minPrice + "<p></p>&nbsp;</td>");
                //sb.Append("<td class='price2'>Rs." + (Math.Round(Convert.ToDouble(minPrice) / 100000, 2)).ToString() + "-" + (Math.Round(Convert.ToDouble(maxPrice) / 100000, 2)).ToString() + " lac <p></p>&nbsp;</td>");
                sb.Append("<td align='center'>" + CommonOpn.GetRateImage(Convert.ToDouble(modelReviewRate)) + "<p><a title='" + modelReviewCount + " Reviews of " + bikeModel + "' href='/" + makeMappingname + "-bikes/" + modelMappingName + "/user-reviews/' class='href-grey'>" + modelReviewCount + " Reviews</a></p></td>");
                sb.Append("<td>&nbsp;</td>");
                sb.Append("</tr>");
            }

            return sb.ToString();
        }

        /// <summary>
        /// Function to format mileage. if its not available return empty string else return mileage along with unit that is kpl
        /// </summary>
        /// <param name="mileage">mileage(fuel economy)</param>
        /// <returns>string</returns>
        public string GetMiledge(string mileage)
        {
            if (mileage != "")
                return ", " + mileage + "kpl";
            else
                return "";
        }

    }   // End of class
}   // End of namespace