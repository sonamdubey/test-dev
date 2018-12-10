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
using System.Text.RegularExpressions;
using Carwale.Notifications;
using Carwale.UI.Common;
using Carwale.DAL.CoreDAL;
using System.Data.Common;
using Carwale.DAL.CoreDAL.MySql;

namespace Carwale.UI.Controls
{
    public enum RepeaterNewPagerPosition
    {
        Top, Bottom, TopBottom, None
    }

    [ParseChildren(true, "Repeaters")]
    public class RepeaterPagerAdvanced : UserControl
    {
        protected DropDownList cmbPageSize, cmbPageSize1;
        protected Panel pnlGrid;
        protected HtmlGenericControl divPages, divPages1, divFirstNav, divFirstNav1, divLastNav, divLastNav1;
        protected Label lblPages, lblRecords;
        protected HtmlTableRow TopPager, BottomPager;

        private ArrayList repeaters = new ArrayList(); // variable used bt property Repeaters.
        private string _selectClause = "", _fromClause = "", _whereClause = "", _orderByClause = "", _groupByClause = "",
                        recCountQry = "", _baseUrl = ""; // variable used bt property Query.
        public string baseUrlForPs = "";
        private string cssClassName = "rptNavDiv";
        private string nextText = "&raquo;";
        private string prevText = "&laquo;";
        private string resultName = "Records";
        private string spName = string.Empty;
        private DbCommand _mysqlCmd = null;

        private Repeater rpt = new Repeater();
        RepeaterNewPagerPosition position = RepeaterNewPagerPosition.TopBottom;

        public int SerialNo = 0;

        private int _pageCount = 1, _curPageIndex = 1, _pagerPageSize = 5, _recordCount = 0;
        private bool _showHeadersVisible = true;

        private int totalPages = 1;

        private SqlParameter[] _SParams = null;

        public DbCommand MySqlCmd
        {
            get { return _mysqlCmd; }
            set { _mysqlCmd = value; }
        }

        public int PageCount
        {
            get { return _pageCount; }
            set { _pageCount = value; }
        }

        public bool ShowHeadersVisible
        {
            get { return _showHeadersVisible; }
            set { _showHeadersVisible = value; }
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

        // This property hold all the sqlParameters
        public SqlParameter[] SParams
        {
            get
            {
                return _SParams;
            }
            set
            {
                _SParams = value;
            }
        } // SParams


        public int PageSize
        {
            get { return int.Parse(cmbPageSize.SelectedValue); }
            set
            {
                Trace.Warn("Updated PageSize : " + value);
                cmbPageSize.SelectedIndex = cmbPageSize.Items.IndexOf(cmbPageSize.Items.FindByValue(value.ToString()));
                cmbPageSize1.SelectedIndex = cmbPageSize1.Items.IndexOf(cmbPageSize1.Items.FindByValue(value.ToString()));
                Trace.Warn("Updated PageSize : " + value);
            }
        }

        ///	<summary>
        ///	This Property tells the Control that how to render Pager-Strip. Three possible Positions are: 
        /// 1. Top 	2. Bottom	3. TopBottom
        public RepeaterNewPagerPosition PagerPosition
        {
            get { return position; }
            set { position = value; }
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
                TopPager.Attributes["class"] = value;
                BottomPager.Attributes["class"] = value;
                cssClassName = value;
            }
        } // RecordCount

        // This property holds the value of Pager PageSize, i.e. How many pages are to be listed in the pager at a time.
        public int PagerPageSize
        {
            get { return _pagerPageSize; }
            set { _pagerPageSize = value; }
        } // RecordCount


        // The term to be used for results. Default term is 'Records'.
        public string ResultName
        {
            set { resultName = value; }
            get { return resultName; }
        }

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
            if (baseUrlForPs.IndexOf("&ps=") != -1)
                baseUrlForPs = Regex.Replace(baseUrlForPs, "(&ps=[0-9]*)", "");

            if (baseUrlForPs.IndexOf("&pn=") != -1)
                baseUrlForPs = Regex.Replace(baseUrlForPs, "(&pn=[0-9]*)", "");


            this.CreateNavigation();

            this.ApplyPosition();
            this.BindRepeater();
            this.ShowHeaders(RecordCount);
        }

        // This function will find Positions for pager-strips.
        private void ApplyPosition()
        {
            switch (this.PagerPosition)
            {
                case RepeaterNewPagerPosition.Top:
                    BottomPager.Visible = false;
                    break;
                case RepeaterNewPagerPosition.Bottom:
                    TopPager.Visible = false;
                    break;
                case RepeaterNewPagerPosition.None:
                    TopPager.Visible = false;
                    BottomPager.Visible = false;
                    break;
                default:
                    TopPager.Visible = true;
                    BottomPager.Visible = true;
                    break;

            }
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

            if (url.IndexOf("&ps=") != -1)
                url = Regex.Replace(url, "(&ps=[0-9]*)", "");

            url += "&ps=" + PageSize;

            if (url.IndexOf("&pn=") != -1)
                url = Regex.Replace(url, "(&pn=[0-9]*)", "");

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
                    navUrls += "<span class='pg'><a href='" + url + "&pn=1'>First</a></span>";
                    navUrls += "<span class='pg'><a href='" + url + "&pn=" + (CurrentPageIndex - 1) + "'>Previous</a></span>";
                }
                else
                {
                    navUrls += "<span class='pgSel'>First</span>";
                    navUrls += "<span class='pgSel'>Previous</span>";
                }

                divFirstNav.InnerHtml = navUrls;
                divFirstNav1.InnerHtml = navUrls;

                navUrls = "";

                int startIndex = (curSlot - 1) * PagerPageSize + 1;
                int endIndex = curSlot * PagerPageSize;
                endIndex = endIndex <= PageCount ? endIndex : PageCount;

                if (curSlot > 1)
                    navUrls += "<span class='pg'><a href='" + url + "&pn=" + (startIndex - 1).ToString() + "'>" + PreviousPagerText + "</a></span>";

                for (int i = startIndex; i <= endIndex; i++)
                {
                    if (CurrentPageIndex != i)
                        navUrls += "<span class='pg'><a href='" + url + "&pn=" + i.ToString() + "'>" + i.ToString() + "</a></span>";
                    else
                        navUrls += "<span class='pgSel'>" + i.ToString() + "</span>";
                }

                if (curSlot < totalSlots)
                    navUrls += "<span class='pg'><a href='" + url + "&pn=" + (endIndex + 1).ToString() + "'>" + NextPagerText + "</a></span>";

                divPages.InnerHtml = navUrls;
                divPages1.InnerHtml = navUrls;

                navUrls = "";
                if (lastPage == false)
                {
                    navUrls += "<span class='pg'><a href='" + url + "&pn=" + (CurrentPageIndex + 1) + "'>Next</a></span>";
                    navUrls += "<span class='pg'><a href='" + url + "&pn=" + PageCount + "'>Last</a></span>";
                }
                else
                {
                    navUrls += "<span class='pgSel'>Next</span>";
                    navUrls += "<span class='pgSel'>Last</span>";
                }


                divLastNav.InnerHtml = navUrls;
                divLastNav1.InnerHtml = navUrls;
            }
            else
            {
                divPages.InnerHtml = "";
                divPages1.InnerHtml = "";
                //No need to create any navigation
            }

        }


        // This function will bind Repeater with the
        // query provided.
        public void BindRepeater()
        {
            this.SerialNo = (this.CurrentPageIndex - 1) * this.PageSize;    
            CommonOpn objCom = new CommonOpn();      
            int startIndex = (this.CurrentPageIndex - 1) * this.PageSize + 1;
            int endIndex = this.CurrentPageIndex * this.PageSize;       
            try
            {
                DataSet ds = new DataSet();
                DbCommand cmd = MySqlCmd;
                cmd.Parameters.Add(DbFactory.GetDbParam("v_StartIndex", DbType.Int32, startIndex));
                cmd.Parameters.Add(DbFactory.GetDbParam("v_EndIndex", DbType.Int32, endIndex));
                ds = MySqlDatabase.SelectAdapterQuery(cmd, DbConnections.CarDataMySqlReadConnection);
                rpt.DataSource = ds;
                rpt.DataBind();
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message + err.Source);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }


        int GetRecordCount()
        {
            int count = 0;
            try
            {
                if (!string.IsNullOrEmpty(RecordCountQuery))
                {
                    using (DbCommand cmd = DbFactory.GetDBCommand(RecordCountQuery))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, DbConnections.CarDataMySqlReadConnection))
                        {
                            if (dr.Read())
                            {
                                count = Convert.ToInt32(dr[0]);
                            }
                        }
                    }
                }
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message + err.Source);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

            return count;
        }


        private void ShowHeaders(int totalRecords)
        {
            if (ShowHeadersVisible == true)
            {
                lblRecords.Text = "Total <span>"
                            + totalRecords + "</span> " + this.ResultName + " Found.";
                lblPages.Text = "Showing <span>"
                            + (CurrentPageIndex).ToString()
                            + "</span> of Total <span>"
                            + PageCount.ToString()
                            + "</span> Pages.";
            }
            else
            {
                lblRecords.Text = "";
                lblPages.Text = "";
            }

        }


    }
}