using Carwale.BL.Videos;
using Carwale.Cache.CMS;
using AEPLCore.Cache;
using Carwale.Entity;
using Carwale.Entity.CMS;
using Carwale.Interfaces;
using Carwale.Interfaces.CMS;
using Carwale.Notifications;
using Carwale.Service;
using Microsoft.Practices.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Carwale.UI.Controls
{
    [ParseChildren(true, "Repeaters")]
    public class VideosPager : UserControl
    {
        /******************************************************************************************/
        //ASP Controls
        /******************************************************************************************/
        protected DropDownList cmbPageSize, cmbPageSize1;
        protected Panel pnlGrid;
        protected Label lblRecordsFooter;
        private Repeater rpt = new Repeater();
        RepeaterNewPagerPosition position = RepeaterNewPagerPosition.TopBottom;

        /******************************************************************************************/
        //Html Controls
        /******************************************************************************************/
        protected HtmlGenericControl divPages1, divFirstNav1, divLastNav1, div_nec/* Not enaugh cars */;
        protected HtmlTableRow BottomPager;

        /******************************************************************************************/
        //Variables
        /******************************************************************************************/
        private ArrayList repeaters = new ArrayList(); // variable used bt property Repeaters.

        // string variables
        //private string _selectClause = "", _fromClause = "", _whereClause = "", _orderByClause = "", _groupByClause = "",
        //                recCountQry = "", _baseUrl = ""; // variable used bt property Query.		

        // public string variables
        private string _baseUrl = "";
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



        public int RecordCount
        {
            get { return _recordCount; }
            set { _recordCount = value; }
        } // RecordCount



        public int PageSize
        {
            get { return 9; }
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
                //TopPager.Attributes["class"] = value;
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

        public string CatId
        {
            get;
            set;
        }
        public string MakeId
        {
            get;
            set;
        }

        public string ModelId
        {
            get;
            set;
        }

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

        //This property will be used as default property by ParseChildren Attribute. All the Repeaters 
        //placed within pager control will be assiged to this ArrayList.
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
            BindData();
        }


        private void BindData()
        {
            this.BindRepeater();
            this.CreateNavigation(recordCount());
            this.ShowHeaders(recordCount());
        }

        // Override the CreateChildControls function. Handle all the Child Repeaters Here.
        protected override void CreateChildControls()
        {
            pnlGrid.Controls.Add(rpt);
        }

        public void BindRepeater()
        {

            try
            {
                this.SerialNo = (this.CurrentPageIndex - 1) * this.PageSize;
                int startIndex = (this.CurrentPageIndex - 1) * this.PageSize + 1;
                int endIndex = this.CurrentPageIndex * this.PageSize;

                //-----------------Code added by Rohan Sapkal Purpose:Replace CarwaleVs logic by CarwalePro
                
                IVideosBL blObj = UnityBootstrapper.Resolve<IVideosBL>();

                List<Carwale.Entity.CMS.Video> videoList = blObj.GetNewModelsVideosBySubCategory(getCategory(CatId), CMSAppId.Carwale, startIndex, endIndex);
                rpt.DataSource = videoList;
                rpt.DataBind();
                //-----------------End of Code edited by Rohan Sapkal
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message + err.Source);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }


        int recordCount()
        {
            List<Carwale.Entity.CMS.Video> videoList;
            //-----------------Code edited by Rohan Sapkal Purpose:Replace CarwaleVs logic by CarwalePro
            try
            {                
                IVideosBL blObj = UnityBootstrapper.Resolve<IVideosBL>();
                videoList = blObj.GetNewModelsVideosBySubCategory(getCategory(CatId), CMSAppId.Carwale, 1, -1);

                return videoList != null ? videoList.Count : -1;
                //-----------------End of Code edited by Rohan Sapkal
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message + err.Source);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return -1;
        }


        // Function to create paging navigation, so that user can browse across the pages.
        void CreateNavigation(int recordCount)
        {
            //get the total record count
            RecordCount = recordCount;

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

                //Trace.Warn("totalSlots : " + totalSlots.ToString() + " : curSlot : " + curSlot.ToString());

                if (CurrentPageIndex == 1)
                    firstPage = true;
                else if (CurrentPageIndex == PageCount)
                    lastPage = true;

                string navUrls = "";

                //add the navigations for first, previous, next and last
                if (firstPage == false)
                {
                    navUrls += "<a href='" + url + "&pn=" + (CurrentPageIndex - 1) + "'><span class='pgSel'>Previous</span></a>";
                }
                else
                {
                    navUrls += "<span class='pgEnd'>Previous</span>";
                }

                divFirstNav1.InnerHtml = navUrls;

                navUrls = "";

                int startIndex = (curSlot - 1) * PagerPageSize + 1;
                int endIndex = curSlot * PagerPageSize;


                //Trace.Warn("startIndex = " + startIndex.ToString()); Trace.Warn("endIndex = " + endIndex.ToString());
                //if(curSlot > 1)
                //navUrls += "<span class='pg'><a href='" + url + "&pn=" + (startIndex - 1).ToString() + "'>" + PreviousPagerText + "</a></span>";

                // Show the current page in the middle of the pager display.	
                if (CurrentPageIndex > 3)
                {
                    // The change would have to happen only after the 3rd page onwards.
                    startIndex = CurrentPageIndex - 2; // Start at the current page -2
                    endIndex = CurrentPageIndex + 2; // end at the current page + 2 					
                }// condition is true only for PagerPageSize = 5

                endIndex = endIndex <= PageCount ? endIndex : PageCount; // if end page number exceeds page count, set it to page count

                for (int i = startIndex; i <= endIndex; i++)
                {
                    if (CurrentPageIndex != i)
                        navUrls += "<a href='" + url + "&pn=" + i.ToString() + "'><span class='pg'>" + i.ToString() + "</span></a>";
                    else
                        navUrls += "<span class='pgSel'>" + i.ToString() + "</span>";
                }
                //if(curSlot < totalSlots)
                //navUrls += "<span class='pg'><a href='" + url + "&pn=" + (endIndex + 1).ToString() + "'>" + NextPagerText + "</a></span>";

                divPages1.InnerHtml = navUrls;

                navUrls = "";
                if (lastPage == false)
                {
                    navUrls += "<a href='" + url + "&pn=" + (CurrentPageIndex + 1) + "'><span class='pgSel'>Next</span></a>";
                }
                else
                {
                    navUrls += "<span class='pgEnd'>Next</span>";
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
                    //lblRecords.Text = "";
                    lblRecordsFooter.Text = "";
                }
                else
                {
                    int showingFrom = CurrentPageIndex > 1 ? ((CurrentPageIndex - 1) * PageSize) + 1 : 1;
                    int showingTo = totalRecords < (CurrentPageIndex * PageSize) ? totalRecords : (CurrentPageIndex * PageSize);

                    //lblRecords.Text = "Showing <span class='price2'>" + showingFrom +"-"+  showingTo + "</span> of <span class='price2'>" + totalRecords +"</span> Cars";
                    lblRecordsFooter.Text = "Showing <span class='price2'>" + showingFrom + "-" + showingTo + "</span> of <span class='price2'>" + totalRecords + "</span> Videos"; ;
                }
            }
        }


        public static EnumVideoCategory getCategory(string catid)
        {
            switch (catid)
            {

                case "1": return EnumVideoCategory.MostPopular;
                case "2": return EnumVideoCategory.ExpertReviews;
                case "3": return EnumVideoCategory.InteriorShow;
                case "4": return EnumVideoCategory.Miscelleneous;
                case "5": return EnumVideoCategory.FeaturedAndLatest;
                default: return EnumVideoCategory.MostPopular;
            }
        }
    }// class
}//namespace