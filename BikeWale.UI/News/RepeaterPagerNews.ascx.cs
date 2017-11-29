using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Bikewale.Common;
using System.Data.Common;

namespace Bikewale.News
{
    [ParseChildren(true, "Repeaters")]
    public class RepeaterPagerNews : UserControl
    {
        /******************************************************************************************/
        //ASP Controls
        /******************************************************************************************/
        protected Panel pnlGrid;

        private Repeater rpt = new Repeater();

        /******************************************************************************************/
        //Html Controls
        /******************************************************************************************/
        protected HtmlGenericControl divPages, divFirstNav, divLastNav;

        /******************************************************************************************/
        //Variables
        /******************************************************************************************/
        private ArrayList repeaters = new ArrayList(); // variable used bt property Repeaters.
        private string _selectClause = "", _fromClause = "", _whereClause = "", _orderByClause = "", _groupByClause = "",
                        recCountQry = "", _baseUrl = ""; // variable used bt property Query.
        public string baseUrlForPs = "", strOrder = "";
        private string cssClassName = "rptNavDiv";
        private string nextText = "&raquo;";
        private string prevText = "&laquo;";
        private bool _getTags = false;
        private bool _getSubCat = false;
        public int SerialNo = 0;

        private int _pageCount = 1, _curPageIndex = 1, _pagerPageSize = 5, _recordCount = 0;

        public int totalPages = 1;

        private DbCommand _cmdParamQ = null;
        private DbCommand _cmdParamR = null;

        private DataSet _dataSetTags = null;
        private DataSet _dataSetSubCat = null;


        /******************************************************************************************/
        //Properties
        /******************************************************************************************/
        public int PageCount
        {
            get { return _pageCount; }
            set { _pageCount = value; }
        }

        public int CurrentPageIndex
        {
            get { return _curPageIndex; }
            set { _curPageIndex = value; }
        }

        // This property will be used as container for 
        // DatGrid binding sql string.
        public string SelectClause
        {
            get { return _selectClause; }
            set { _selectClause = value; }
        }

        public string FromClause
        {
            get { return _fromClause; }
            set { _fromClause = value; }
        }

        public string WhereClause
        {
            get { return _whereClause; }
            set { _whereClause = value; }
        }

        public string OrderByClause
        {
            get { return _orderByClause; }
            set { _orderByClause = value; }
        }

        public string GroupByClause
        {
            get { return _groupByClause; }
            set { _groupByClause = value; }
        }

        // This property will be used as container for 
        // the query for counting the total number of records
        public string RecordCountQuery
        {
            get { return recCountQry; }
            set { recCountQry = value; }
        } // Query


        public int RecordCount
        {
            get { return _recordCount; }
            set { _recordCount = value; }
        } // RecordCount

        // This property hold all the CmdParam
        public DbCommand CmdParamQ //command variable to store the parameters for query
        {
            get
            {
                return _cmdParamQ;
            }
            set
            {
                _cmdParamQ = value;
            }
        } // CmdParam

        public DbCommand CmdParamR	//command variable for the record count
        {
            get
            {
                return _cmdParamR;
            }
            set
            {
                _cmdParamR = value;
            }
        } // CmdParam

        public DataSet DataSetTags
        {
            get { return _dataSetTags; }
            set { _dataSetTags = value; }
        }

        public DataSet DataSetSubCat
        {
            get { return _dataSetSubCat; }
            set { _dataSetSubCat = value; }
        }

        public bool GetTags
        {
            get { return _getTags; }
            set { _getTags = value; }
        }

        public bool GetSubCat
        {
            get { return _getSubCat; }
            set { _getSubCat = value; }
        }

        public int PageSize
        {
            get { return 10; }
        }


        public string NextPagerText
        {
            get { return nextText; }
            set { nextText = value; }
        }

        public string PreviousPagerText
        {
            get { return prevText; }
            set { prevText = value; }
        }

        /// Pager-Strip's css class name.
        public string CssClass
        {
            get { return cssClassName; }
            set
            {
                cssClassName = value;
            }
        } // RecordCount

        // This property holds the value of Pager PageSize, i.e. How many pages are to be listed in the pager at a time.
        public int PagerPageSize
        {
            get { return _pagerPageSize; }
            set { _pagerPageSize = value; }
        } // RecordCount


        // The term to be used for base url, to this the pc for pagecount and pn for page numbers will be added
        public string BaseUrl
        {
            get { return _baseUrl; }
            set { _baseUrl = value; }
        }

        //added by : Ashwini Todkar on 21 jan 2014
        DbCommand _CmdParamQry;
        public DbCommand CmdParamQry //command variable to store the parameters for query
        {
            get { return _CmdParamQry; }
            set { _CmdParamQry = value; }
        } // CmdParam

        protected override void OnInit(EventArgs e)
        {
            InitializeComponents();
        }

        void InitializeComponents()
        {
            HttpContext.Current.Trace.Warn("RepeaterPager : InitializeComponents started.");
            //this.Load += new EventHandler(this.Page_Load);
            base.Load += new EventHandler(Page_Load);

            // Child Repeater Controls Assignent.
            try
            {
                foreach (Repeater rptr in Repeaters)
                {
                    rpt = rptr;
                }
            }
            catch (Exception ex)
            {
                Trace.Warn(ex.Message);
                throw new Exception("Extended Repeater Control: Only Repeaters can be placed inside this 'Extended Repeater Control'.");
            }

        }

        // This property will be used as default property by ParseChildren Attribute. All the Repeaters 
        // placed within pager control will be assiged to this ArrayList.
        public ArrayList Repeaters
        {
            get { return repeaters; }
        }

        void Page_Load(object sender, EventArgs e)
        {

        } // Page_Load


        // This function will initialize grid properties.
        public void InitializeGrid()
        {
            baseUrlForPs = BaseUrl;

            if (baseUrlForPs.IndexOf("&pn=") != -1)
                baseUrlForPs = Regex.Replace(baseUrlForPs, "(&pn=[0-9]*)", "");

            BindData(GetRepeaterData());

            //this.CreateNavigation();
            //this.BindRepeater();
        }//InitializeGrid

        private void BindData(DataSet Data)
        {
            this.CreateNavigation(Convert.ToInt32(Data.Tables[1].Rows[0]["RecordCount"]));
            this.BindRepeater(Data.Tables[0]);
        }

        // Override the CreateChildControls function. Handle all the Child Repeaters Here.
        protected override void CreateChildControls()
        {
            pnlGrid.Controls.Add(rpt);
        }

        void CreateNavigation(int totalRecords)
        {
            //get the total record count
            RecordCount = totalRecords;

            totalPages = (int)Math.Ceiling((double)RecordCount / (double)PageSize);
            PageCount = totalPages;

            bool firstPage = false, lastPage = false;

            string url = BaseUrl;

            if (url.IndexOf("page=") != -1)
                url = Regex.Replace(url, "(page=[0-9]*)", "");

            //now check whetner the current page is the first page or the last page
            if (PageCount > 1)
            {
                //find the total number of slots
                int totalSlots = (int)Math.Ceiling((double)PageCount / (double)PagerPageSize);
                int curSlot = ((int)Math.Floor((double)(CurrentPageIndex - 1) / (double)PagerPageSize)) + 1;

                Trace.Warn("totalSlots : " + totalSlots.ToString() + " : curSlot : " + curSlot.ToString());

                if (CurrentPageIndex == 1)
                    firstPage = true;
                else if (CurrentPageIndex == PageCount)
                    lastPage = true;

                string navUrls = "";

                //add the navigations for first, previous, next and last
                if (firstPage == false)
                {
                    navUrls += "<span class='pg'><a href='" + url + "page/1/'>First</a></span><span class='pg'><a href='" + url + "page/" + (CurrentPageIndex - 1) + "/'>Previous</a></span>";
                }
                else
                {
                    navUrls += "<span class='pgEnd'>First</span><span class='pgEnd'>Previous</span>";
                }

                divFirstNav.InnerHtml = navUrls;

                navUrls = "";

                int startIndex = (curSlot - 1) * PagerPageSize + 1;
                int endIndex = curSlot * PagerPageSize;

                if (CurrentPageIndex > 5)
                {
                    // The change would have to happen only after the 3rd page onwards.
                    startIndex = CurrentPageIndex - 4; // Start at the current page -4
                    endIndex = CurrentPageIndex + 4; // end at the current page + 4 					
                }// condition is true only for PagerPageSize = 5

                endIndex = endIndex <= PageCount ? endIndex : PageCount;

                //if(curSlot > 1)
                //navUrls += "<span class='pg'><a href='" + url + "&pn=" + (startIndex - 1).ToString() + "'>" + PreviousPagerText + "</a></span>";

                for (int i = startIndex; i <= endIndex; i++)
                {
                    if (CurrentPageIndex != i)
                        navUrls += "<span class='pg'><a href='" + url + "page/" + i.ToString() + "/'>" + i.ToString() + "</a></span>";
                    else
                        navUrls += "<span class='pgSel'>" + i.ToString() + "</span>";
                }

                //if(curSlot < totalSlots)
                //navUrls += "<span class='pg'><a href='" + url + "&pn=" + (endIndex + 1).ToString() + "'>" + NextPagerText + "</a></span>";

                divPages.InnerHtml = navUrls;

                navUrls = "";
                if (lastPage == false)
                {
                    navUrls += "<span class='pg'><a href='" + url + "page/" + (CurrentPageIndex + 1) + "/'>Next</a></span><span class='pg'><a href='" + url + "page/" + PageCount + "/'>Last</a></span>";
                }
                else
                {
                    navUrls += "<span class='pgEnd'>Next</span><span class='pgEnd'>Last</span>";
                }


                divLastNav.InnerHtml = navUrls;
            }
            else
            {
                divPages.InnerHtml = "";
                //No need to create any navigation
            }
        }

        /// <summary>
        /// Written By : Ashwini Todkar on 21 Jan 2014
        /// Summary    : binds news repeater
        /// </summary>
        /// <param name="Data"></param>
        private void BindRepeater(DataTable Data)
        {
            try
            {
                if (Data.Rows.Count > 0)
                {
                    rpt.DataSource = Data;
                    rpt.DataBind();
                }
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message + err.Source);
                ErrorClass.LogError(err, Request.ServerVariables["URL"]);
                
            }
        }

        // This function will bind Repeater with the
        // query provided.
        public void BindRepeater()
        {
            throw new Exception("Method not used/commented");

            //this.SerialNo = (this.CurrentPageIndex - 1) * this.PageSize;

            //string sql = "";
            //CommonOpn objCom = new CommonOpn();

            //Database db = new Database();

            //int startIndex = (this.CurrentPageIndex - 1) * this.PageSize + 1;
            //int endIndex = this.CurrentPageIndex * this.PageSize;

            ////form the query. Only fetch the desired rows. 

            //sql = " Select * From (Select Top " + endIndex + " Row_Number() Over (Order By " + OrderByClause + ") AS RowN, "
            //    + " " + SelectClause + " From " + FromClause + " "
            //    + (WhereClause != "" ? " Where " + WhereClause + " " : "")
            //    + " ) AS TopRecords Where "
            //    + " RowN >= " + startIndex + " AND RowN <= " + endIndex + " ";

            //Trace.Warn("Fetch the desired rows : " + sql);
            //try
            //{
            //    DataSet ds = new DataSet();
            //    if (sql != "")
            //    {
            //        Trace.Warn("Binding Sql : " + sql);

            //        CmdParamQ.CommandText = sql;
            //        ds = db.SelectAdaptQry(CmdParamQ);
            //        Trace.Warn("data Row Count: " + ds.Tables[0].Rows.Count.ToString());

            //        Trace.Warn("GetTags : " + GetTags);
            //        /* if (GetTags)
            //         {
            //             DataView Dv = ds.Tables[0].DefaultView;
            //             DataTable dt = Dv.ToTable(true, "BasicId");

            //             Trace.Warn(" dt count " + dt.Rows.Count.ToString());

            //             DataRow[] distinctRows = dt.Select();
            //             string ids = string.Empty;
            //             foreach (DataRow drow in dt.Rows)
            //             {
            //                 ids += drow["BasicId"].ToString() + ",";
            //             }

            //             ids = ids.Substring(0, ids.Length - 1);

            //             Trace.Warn("Here: ");
            //             SqlCommand cmdTags = new SqlCommand();
            //             string tagsSql = " Select BT.BasicId, Tag, Slug From Con_EditCms_Tags T "
            //                            + " Left Join Con_EditCms_BasicTags BT On BT.TagId = T.ID "
            //                            + " Where BT.BasicId In ( " + db.GetInClauseValue(ids, "BasicIds", cmdTags) + " )";
            //             cmdTags.CommandText = tagsSql;

            //             Trace.Warn("tagsSql: " + tagsSql);
            //             Trace.Warn("ids: " + ids);

            //             DataSetTags = db.SelectAdaptQry(cmdTags);
            //         }
            //         Trace.Warn("GetSubCat : " + GetSubCat);
            //         if (GetSubCat)//fetch the subcategories for these basic ids
            //         {
            //             DataView Dv = ds.Tables[0].DefaultView;
            //             DataTable dt = Dv.ToTable(true, "BasicId");

            //             Trace.Warn(" dt count " + dt.Rows.Count.ToString());

            //             DataRow[] distinctRows = dt.Select();
            //             string ids = string.Empty;
            //             foreach (DataRow drow in dt.Rows)
            //             {
            //                 ids += drow["BasicId"].ToString() + ",";
            //             }

            //             ids = ids.Substring(0, ids.Length - 1);

            //             Trace.Warn("Here: ");
            //             SqlCommand cmdSubCat = new SqlCommand();
            //             string subCatSql = " Select BSC.BasicId, SC.Id, SC.Name From Con_EditCms_SubCategories SC "
            //                              + " Inner Join Con_EditCms_BasicSubCategories BSC On BSC.SubCategoryId = SC.Id "
            //                              + " Where BSC.BasicId In ( " + db.GetInClauseValue(ids, "BasicIds", cmdSubCat) + " ) ";
            //             cmdSubCat.CommandText = subCatSql;

            //             Trace.Warn("subCatSql: " + subCatSql);
            //             Trace.Warn("ids: " + ids);

            //             DataSetSubCat = db.SelectAdaptQry(cmdSubCat);
            //         }*/
            //        rpt.DataSource = ds;
            //        Trace.Warn("DataSource Set");
            //        rpt.DataBind();
            //        Trace.Warn("PageSize: " + PageSize);
            //    }
            //    Trace.Warn("Binding Complete...");
            //}
            //catch (Exception err)
            //{
            //    Trace.Warn(err.Message + err.Source);
            //    ErrorClass.LogError(err, Request.ServerVariables["URL"]);
            //    
            //}
        }

        //int GetRecordCount()
        //{
        //    int count = 0;
        //    SqlDataReader dr = null;
        //    Database db = new Database();

        //    Trace.Warn("RecordCountQuery", RecordCountQuery);
        //    try
        //    {
        //        if (RecordCountQuery != "")
        //        {
        //            CmdParamR.CommandText = RecordCountQuery;

        //            Trace.Warn("RecordCountQuery: " + RecordCountQuery);
        //            dr = db.SelectQry(CmdParamR);

        //            if (dr.Read())
        //            {
        //                count = Convert.ToInt32(dr[0]);
        //            }
        //        }

        //    }
        //    catch (Exception err)
        //    {
        //        Trace.Warn(err.Message + err.Source);
        //        ErrorClass.LogError(err, Request.ServerVariables["URL"]);
        //        
        //    }
        //    finally
        //    {
        //        if(dr != null)
        //            dr.Close();

        //        db.CloseConnection();
        //    }

        //    return count;
        //}

        /// <summary>
        /// Written By : Ashwini Todkar on 21 Jan 2014
        /// summary    : retrives news page detais like views,AuthorName,Description,DisplayDate,Title,Url,IsMainImage etc
        /// </summary>
        /// <returns></returns>
        private DataSet GetRepeaterData()
        {
            throw new Exception("Method not used/commented");

            //DataSet ds = new DataSet();

            //this.SerialNo = (this.CurrentPageIndex - 1) * this.PageSize;

            //string sql = string.Empty;

            //Database db = new Database();

            //int startIndex = (this.CurrentPageIndex - 1) * this.PageSize + 1;
            //int endIndex = this.CurrentPageIndex * this.PageSize;

            //Trace.Warn("CmdParamQry " + CmdParamQry);

            //try
            //{
            //    // Assign parameter(predefine) to the SqlCommand

            //    CmdParamQry.Parameters.Add("@StartIndex", SqlDbType.Int).Value = startIndex;
            //    CmdParamQry.Parameters.Add("@EndIndex", SqlDbType.Int).Value = endIndex;

            //    // Execute Sql command and bind data with the repeater.
            //    ds = db.SelectAdaptQry(CmdParamQry);

            //    Trace.Warn("rows count : " + ds.Tables[0].Rows.Count);
            //}
            //catch (Exception err)
            //{
            //    Trace.Warn(err.Message + err.Source);
            //    ErrorClass.LogError(err, Request.ServerVariables["URL"]);
            //    
            //}

            //return ds;
        }
    }//class
}//namespace