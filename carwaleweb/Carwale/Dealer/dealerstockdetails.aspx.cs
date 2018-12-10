using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Carwale.UI.Controls;
using Carwale.UI.Common;
using Carwale.Utility;
using System.Data.Common;
using Carwale.DAL.CoreDAL.MySql;

namespace Carwale.UI.Dealer
{
    public class DealerStockDetails : System.Web.UI.Page
    {
        protected RepeaterPagerDealerStock rpgListings;
        protected Repeater rptListings;

        // html controls
        //protected HtmlGenericControl res_msg;

        // protected variables
        protected string QsSortCriteria = string.Empty, QsSortOrder = string.Empty, PageNumber = string.Empty, MakeIds = string.Empty;


        protected override void OnInit(EventArgs e)
        {
            rptListings = (Repeater)rpgListings.FindControl("rptListings");
            InitializeComponent();
        }

        void InitializeComponent()
        {
            base.Load += new EventHandler(this.Page_Load);
        }

        void Page_Load(object Sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string query_string = Request.ServerVariables["QUERY_STRING"];
                NameValueCollection qsCollection = Request.QueryString;
                if (!String.IsNullOrEmpty(qsCollection.Get("mId")) && !String.IsNullOrEmpty(qsCollection.Get("dId")))
                {
                    string makeId = qsCollection.Get("mId");
                    string dealerId = qsCollection.Get("dId");
                    string sortCriteria = qsCollection.Get("sc");
                    string sortOrder = qsCollection.Get("so");
                    // Class to process parameters selected by the user		
                    //ParseSearchCriterias objPC = new ParseSearchCriterias(qsCollection);

                    if (CommonOpn.CheckId(dealerId))
                    {
                        DbCommand sqlCmdParams = DbFactory.GetDBCommand();
                        sqlCmdParams.Parameters.Add(DbFactory.GetDbParam("@DealerId", DbType.Int64, dealerId));
                        Trace.Warn("MakeId:" + makeId);
                        string[] makes = makeId.Split(',');
                        Trace.Warn("Makes:" + makes);
                        int count = makes.Length - 1;
                        for (int i = 0; i < makes.Length; i++)
                        {
                            if (CommonOpn.CheckId(makes[i]))
                            {
                                if (int.Parse(makes[i]) > 0)
                                {
                                    MakeIds = MakeIds + makes[i] + ",";
                                }
                            }
                        }
                        if (!string.IsNullOrEmpty(MakeIds))
                        {
                            sqlCmdParams.Parameters.Add(DbFactory.GetDbParam("@MakeId", DbType.String, 100, MakeIds));
                        }
                        Trace.Warn("makeIds;" + MakeIds);

                        rpgListings.BaseUrl = "/dealer/dealerstockdetails.aspx?" + query_string;
                        rpgListings.SelectClause = GetSelectClause();
                        rpgListings.FromClause = GetFromClause();
                        rpgListings.WhereClause = GetWhereClause(MakeIds);
                        rpgListings.OrderByClause = GetOrderByClause(sortCriteria, sortOrder);
                        rpgListings.RecordCountQuery = GetRecordCountQry(MakeIds);
                        rpgListings.CmdParamQry = sqlCmdParams;
                        rpgListings.CmdParamCountQry = DbFactory.CloneDBCommand(sqlCmdParams);


                        string pageNumber = qsCollection.Get("pn");
                        if (!String.IsNullOrEmpty(pageNumber) && CommonOpn.IsNumeric(pageNumber))
                            rpgListings.CurrentPageIndex = int.Parse(pageNumber);
                        else
                            rpgListings.CurrentPageIndex = 1;

                        QsSortCriteria = sortCriteria;
                        QsSortOrder = sortOrder;

                        rpgListings.InitializeGrid();//initialize the grid, and this will also bind the repeater

                        if (rpgListings.RecordCount == 0)
                        {
                            rpgListings.Visible = false;
                            //res_msg.Visible = true;
                        }
                    }
                }
            }
        }

        private string GetRecordCountQry(string MakeIds)
        {
            return " Select Count(distinct ProfileId) From " + GetFromClause() + " Where " + GetWhereClause(MakeIds);
        }


        private string GetOrderByClause(string sortCriteria, string sortOrder)
        {
            string retVal = "";

            switch (sortCriteria)
            {
                case "0":
                    retVal = "( MakeName + ' ' + ModelName + ' ' + VersionName ) " + (sortOrder == "1" ? "DESC" : "ASC");
                    break;

                case "1":
                    retVal = "MakeYear " + (sortOrder == "1" ? "DESC" : "ASC");
                    break;

                case "2":
                    retVal = "Kilometers " + (sortOrder == "1" ? "DESC" : "ASC");
                    break;

                case "3":
                    retVal = "Price " + (sortOrder == "1" ? "DESC" : "ASC");
                    break;

                default:
                    retVal = "MakeYear DESC";
                    break;
            }

            return retVal;
        }

        private string GetWhereClause(string MakeIds)
        {
            string whereClause = "LL.SellerType = 1 AND DealerId = @DealerId";
            if (!string.IsNullOrEmpty(MakeIds))
            {
                whereClause += " And Find_in_set(MakeId,@MakeId) ";
            }
            return whereClause;
        }

		//Modified By : Sadhana Upadhyay on 13 Apr 2015 Added nolock in sql query
        private string GetFromClause()
        {
            return "livelistings AS LL Inner Join cwmasterdb.carmodels CMO on LL.ModelId=CMO.ID";
        }

        private string GetSelectClause()
        {
            return "LL.ProfileId,  concat( MakeName , ' ' , ModelName , ' ' , VersionName ) Car, DATE_FORMAT(MakeYear, '%d %b %Y') MakeYear,LL.MakeName,LL.ModelName,LL.CityName, LL.Price, LL.Color, LL.Kilometers, LL.MakeYear AS MakeYr,LL.CertificationId,LL.PhotoCount,CMO.MaskingName,LL.HostURL,LL.OriginalImgPath";
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
                sortImage = (QsSortOrder == "1" ? "<img src='https://img.carwale.com/used/sorting-down.png' border='0' />" :
                                "<img src='https://img.carwale.com/used/sorting-up.png' border='0' />");
            }

            return sortImage;
        }


        public string GetCarImages(string hostURL, string size, string originalImgPath)
        {
            string str = "";

            if (originalImgPath != "")
                str = "<img class='imgborder' src='" +ImageSizes.CreateImageUrl(hostURL,size,originalImgPath)+ "' border='0'></img>";
            else
                str = "<img src='" + ImagingFunctions.GetRootImagePath() + "/images/dealer/nocarimg.gif' border='0'></img>";

            return str;
        }
    } // class
} // namespace