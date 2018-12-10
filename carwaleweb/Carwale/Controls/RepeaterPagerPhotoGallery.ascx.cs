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
using Carwale.UI.Common;
using System.Text.RegularExpressions;
using Carwale.Notifications;
using Carwale.DAL.CoreDAL;
using Carwale.Entity.CMS.Photos;

namespace Carwale.UI.Controls
{
    [ParseChildren(true, "Repeaters")]
    public class RepeaterPagerPhotoGallery : UserControl
    {
        /******************************************************************************************/
        //ASP Controls
        /******************************************************************************************/
        protected DropDownList cmbPageSize, cmbPageSize1;
        protected Panel pnlGrid;
        protected Label lblRecords, lblRecordsFooter;
        private Repeater rpt = new Repeater();
        RepeaterNewPagerPosition position = RepeaterNewPagerPosition.TopBottom;

        /******************************************************************************************/
        //Html Controls
        /******************************************************************************************/
        protected HtmlGenericControl divPages1, divFirstNav1, divLastNav1, div_nec/* Not enaugh cars */;
        protected HtmlTableRow TopPager, BottomPager;

        /******************************************************************************************/
        //Variables
        /******************************************************************************************/
        private ArrayList repeaters = new ArrayList(); // variable used bt property Repeaters.

        // string variables
        private string _selectClause = "", _fromClause = "", _whereClause = "", _orderByClause = "", _groupByClause = "",
                        recCountQry = "", _baseUrl = ""; // variable used bt property Query.		

        // public string variables
        public string stateName = "State"; // will be assigned if showing results are below 20
        public string baseUrlForPs = "", strOrder = "";
        private string cssClassName = "rptNavDiv";
        private string nextText = "&raquo;";
        private string prevText = "&laquo;";
        private string resultName = "Records";
        
        // int variables
        public int SerialNo = 0;
        private int _pageCount = 1, _curPageIndex = 1, _recordCount = 0;
        private int totalPages = 1;

        // bool variables
        private bool _showHeadersVisible = true;

        /******************************************************************************************/
        //Properties
        /******************************************************************************************/
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

        SqlCommand _CmdParamQry;
        public SqlCommand CmdParamQry //command variable to store the parameters for query
        {
            get { return _CmdParamQry; }
            set { _CmdParamQry = value; }
        } // CmdParam

        SqlCommand _CmdParamCountQry;
        public SqlCommand CmdParamCountQry //command variable to store the parameters for query
        {
            get { return _CmdParamCountQry; }
            set { _CmdParamCountQry = value; }
        }

        public int PageSize
        {
            get { return 6; }
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
        int _pagerPageSize = 5;
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

        string _cityId = string.Empty;
        public string CityId
        {
            get { return _cityId; }
            set { _cityId = value; }
        }

        public CMSImage PhotosList = new CMSImage();

        protected override void OnInit(EventArgs e)
        {
            InitializeComponents();
        }

        void InitializeComponents()
        {
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
            baseUrlForPs = Regex.Replace(baseUrlForPs, "&?(ps|sc|so)=[0-9]*", "");

            this.CreateNavigation();
            this.BindRepeater();
            this.ShowHeaders(RecordCount);
        }

        // Override the CreateChildControls function. Handle all the Child Repeaters Here.
        protected override void CreateChildControls()
        {
            pnlGrid.Controls.Add(rpt);
        }

        // This function will bind Repeater with the
        // query provided.
        public void BindRepeater()
        {
            try
            {
                    rpt.DataSource = PhotosList.Images;
                    rpt.DataBind();
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message + err.Source);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

        // Function to create paging navigation, so that user can browse across the pages.
        void CreateNavigation()
        {
            //get the total record count
            RecordCount = (int)PhotosList.RecordCount;

            totalPages = (int)Math.Ceiling((double)RecordCount / (double)PageSize);
            PageCount = totalPages;

            bool firstPage = false, lastPage = false;

            string url = BaseUrl;

            /*if(url.IndexOf("&ps=") != -1)
                url = Regex.Replace(url, "(&ps=[0-9]*)", "");*/

            if (url.IndexOf("&pn=") != -1)
                url = Regex.Replace(url, "(&pn=[0-9]*)", "");

            // now check whetner the current page is the first page or the last page
            // PagerPageSize = Number of Pager numbers to be shown at one time
            // PageCount = Total number of Pages.
            // totalSlots = ( in this case ) no of sets of n pages. Here n = 5; total pages = 8 
            //				so totalSlots = total no of pages / PagerPageSize = 8 / 5 ~~ 2 (rounded value)

            if (PageCount > 1)
            {
                //find the total number of slots
                int totalSlots = (int)Math.Ceiling((double)PageCount / (double)PagerPageSize);
                int curSlot = ((int)Math.Floor((double)(CurrentPageIndex - 1) / (double)PagerPageSize)) + 1;

                HttpContext.Current.Trace.Warn("totalSlots : " + totalSlots.ToString() + " : curSlot : " + curSlot.ToString());

                if (CurrentPageIndex == 1)
                    firstPage = true;
                else if (CurrentPageIndex == PageCount)
                    lastPage = true;

                string navUrls = "";

                //add the navigations for first, previous, next and last
                if (firstPage == false)
                {
                    navUrls += "<a rel='nofollow' href='" + url + "&pn=" + (CurrentPageIndex - 1) + "'><span class='pg'><strong><</strong></span></a>";
                }
                else
                {
                    navUrls += "<span class='pgEnd'><strong><</strong></span>";
                }

                divFirstNav1.InnerHtml = navUrls;

                navUrls = "";

                int startIndex = (curSlot - 1) * PagerPageSize + 1;
                int endIndex = curSlot * PagerPageSize;


                //if(curSlot > 1)
                //navUrls += "<span class='pg'><a href='" + url + "&pn=" + (startIndex - 1).ToString() + "'>" + PreviousPagerText + "</a></span>";

                // Show the current page in the middle of the pager display.	
                if (CurrentPageIndex > 3)
                {
                    if ((CurrentPageIndex + 2) <= PageCount)
                    {
                        Trace.Warn("Here");
                        // The change would have to happen only after the 3rd page onwards.
                        startIndex = CurrentPageIndex - 2; // Start at the current page -2                        
                        endIndex = CurrentPageIndex + 2; // end at the current page + 2 					
                    }
                }// condition is true only for PagerPageSize = 5

                endIndex = endIndex <= PageCount ? endIndex : PageCount; // if end page number exceeds page count, set it to page count
                Trace.Warn("PageCount: " + PageCount.ToString());
                Trace.Warn("CurrentPageIndex: " + CurrentPageIndex.ToString());
                Trace.Warn("startIndex = " + startIndex.ToString()); Trace.Warn("endIndex = " + endIndex.ToString());
                for (int i = startIndex; i <= endIndex; i++)
                {
                    if (CurrentPageIndex != i)
                        navUrls += "<a rel='nofollow' href='" + url + "&pn=" + i.ToString() + "'><span class='pg'>" + i.ToString() + "</span></a>";
                    else
                        navUrls += "<span class='pgSel'>" + i.ToString() + "</span>";
                }
                //if(curSlot < totalSlots)
                //navUrls += "<span class='pg'><a href='" + url + "&pn=" + (endIndex + 1).ToString() + "'>" + NextPagerText + "</a></span>";

                divPages1.InnerHtml = navUrls;

                navUrls = "";
                if (lastPage == false)
                {
                    navUrls += "<a rel='nofollow' href='" + url + "&pn=" + (CurrentPageIndex + 1) + "'><span class='pg'><strong>></strong></span></a>";
                }
                else
                {
                    navUrls += "<span class='pgEnd'><strong>></strong></span>";
                }

                divLastNav1.InnerHtml = navUrls;
            }
            else
            {
                divPages1.InnerHtml = "";
                //No need to create any navigation
            }
        }

        private void ShowHeaders(int totalRecords)
        {
            if (ShowHeadersVisible == true)
            {
                if (totalRecords == 0)
                {
                    ShowHeadersVisible = false;
                    lblRecords.Text = "";
                    lblRecordsFooter.Text = "";
                }
                else
                {
                    int showingFrom = CurrentPageIndex > 1 ? ((CurrentPageIndex - 1) * PageSize) + 1 : 1;
                    int showingTo = totalRecords < (CurrentPageIndex * PageSize) ? totalRecords : (CurrentPageIndex * PageSize);

                    lblRecords.Text = "Showing <span class='price2'>" + showingFrom + "-" + showingTo + "</span> of <span class='price2'>" + totalRecords + "</span> Cars";
                    lblRecordsFooter.Text = lblRecords.Text;
                }
            }
        }
    }// class
} // namespace