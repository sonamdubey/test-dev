using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.Security;
using System.Xml;
using Bikewale.Common;
using System.Text.RegularExpressions;
using Bikewale.CoreDAL;
using System.Data.Common;
using MySql.CoreDAL;

namespace Bikewale.Controls
{
    [ParseChildren(true, "Repeaters")]
    public class RepeaterPager : UserControl
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
        public int SerialNo = 0;

        private int _pageCount = 1, _curPageIndex = 1, _pagerPageSize = 5, _recordCount = 0;

        public int totalPages = 1;

        private DbCommand _cmdParamQ = null;
        private DbCommand _cmdParamR = null;


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

        protected override void OnInit(EventArgs e)
        {
            InitializeComponents();
        }

        void InitializeComponents()
        {
            HttpContext.Current.Trace.Warn("RepeaterPager : InitializeComponents started.");
            this.Load += new EventHandler(this.Page_Load);

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


            this.CreateNavigation();

            this.BindRepeater();

        }

        // Override the CreateChildControls function. Handle all the Child Repeaters Here.
        protected override void CreateChildControls()
        {
            pnlGrid.Controls.Add(rpt);
        }


        void CreateNavigation()
        {
            //get the total record count
            RecordCount = GetRecordCount();

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

        // This function will bind Repeater with the
        // query provided.
        public void BindRepeater()
        {
            this.SerialNo = (this.CurrentPageIndex - 1) * this.PageSize;

            string sql = "";
            //CommonOpn objCom = new CommonOpn();

            //Database db = new Database();

            int startIndex = (this.CurrentPageIndex - 1) * this.PageSize + 1;
            int endIndex = this.CurrentPageIndex * this.PageSize;

            //form the query. Only fetch the desired rows. 

            //sql = " Select * From (Select Row_Number() Over (Order By " + OrderByClause + ") AS RowN, "
            //    + " " + SelectClause + " From " + FromClause + " "
            //    + (WhereClause != "" ? " Where " + WhereClause + " " : "")
            //    + " ) AS TopRecords Where "
            //    + " RowN >= " + startIndex + " AND RowN <= " + endIndex + " ";
                 
            sql = string.Format(@"
                                    drop temporary table if exists tbl_bikes;
                                    create temporary table tbl_bikes 
                                    select 
                                    {0} 
                                    from {1}  
                                    {2} 
                                    order by {3};

                                    set @rownumber := 0;  

                                    select * from (select *,@rownumber:=@rownumber+1  as rown  from tbl_bikes tb) as c
                                    where  rown >= {4} and rown <= {5};

                                    drop temporary table if exists tbl_bikes;
                    
                                ", SelectClause, FromClause, !String.IsNullOrEmpty(WhereClause) ? " where  " + WhereClause : string.Empty, OrderByClause, startIndex, endIndex);

                                        
            
            
            Trace.Warn("Fetch the desired rows : " + sql);
            Trace.Warn("Current Page Index : " + CurrentPageIndex);
            try
            {
                DataSet ds = new DataSet();
                if (sql != "")
                {
                    Trace.Warn("Binding Sql : ");

                    CmdParamQ.CommandText = sql;
                    ds = MySqlDatabase.SelectAdapterQuery(CmdParamQ, ConnectionType.ReadOnly);

                    rpt.DataSource = ds;
                    rpt.DataBind();
                    Trace.Warn("PageSize: " + PageSize);
                }
                Trace.Warn("Binding Complete...");
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message + err.Source);
                ErrorClass.LogError(err, Request.ServerVariables["URL"]);
                
            }
        }


        int GetRecordCount()
        {
            int count = 0;

            try
            {
                if (RecordCountQuery != "")
                {
                    CmdParamR.CommandText = RecordCountQuery;

                    Trace.Warn("RecordCountQuery: " + RecordCountQuery);
                    using (IDataReader dr  = MySqlDatabase.SelectQuery(CmdParamR,ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            if (dr.Read())
                            {
                                count = Convert.ToInt32(dr[0]);
                            }
                            dr.Close();
                        } 
                    }                   
                }
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message + err.Source);
                ErrorClass.LogError(err, Request.ServerVariables["URL"]);
                
            }
            
            return count;
        }
    }
}