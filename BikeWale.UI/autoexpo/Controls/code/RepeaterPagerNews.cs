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
using Carwale.CMS;
using Carwale.CMS.Entities;
using Carwale.CMS.DAL.AutoExpo;
using System.Collections.Generic;

namespace AutoExpo
{	
	[ParseChildren( true, "Repeaters" )]
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
		private string _baseUrl = ""; // variable used bt property Query.
		public string baseUrlForPs = "";
		private string cssClassName = "rptNavDiv";
		private string nextText = "&raquo;";
		private string prevText = "&laquo;";
		
		public int SerialNo = 0;

        private int _pageCount = 1, _curPageIndex = 1, _pagerPageSize = 5, _recordCount = 0, _pageSize = 0;	
	
		private int totalPages = 1;
		
		/******************************************************************************************/
			//Properties
		/******************************************************************************************/
		public int PageCount
		{
			get{return _pageCount;}
			set{_pageCount = value;}
		}
		
		public int CurrentPageIndex
		{
			get{return _curPageIndex;}
			set{_curPageIndex = value;}
		}
		
		public int RecordCount
		{
			get { return _recordCount; }
			set { _recordCount = value; }
		} // RecordCount
		
		public int PageSize
		{
            get { return _pageSize; }
            set { _pageSize = value; }
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
		
		protected override void OnInit( EventArgs e )
		{
			InitializeComponents();
		}
		
		void InitializeComponents()
		{
			HttpContext.Current.Trace.Warn("RepeaterPager : InitializeComponents started.");
			this.Load += new EventHandler( this.Page_Load );
			
			// Child Repeater Controls Assignent.
			try
			{
				foreach ( Repeater rptr in Repeaters )
				{
					rpt = rptr;
				}
			}
			catch( Exception ex )
			{
				Trace.Warn( ex.Message );
				throw new Exception( "Extended Repeater Control: Only Repeaters can be placed inside this 'Extended Repeater Control'." );
			}
			
		}
		
		// This property will be used as default property by ParseChildren Attribute. All the Repeaters 
		// placed within pager control will be assiged to this ArrayList.
		public ArrayList Repeaters
		{
			get{return repeaters;}
		}
			
		void Page_Load( object sender, EventArgs e )
		{
						
		} // Page_Load

				
		// This function will initialize grid properties.
		public void InitializeGrid(int sortBy)
		{
			baseUrlForPs = BaseUrl;

			if(baseUrlForPs.IndexOf("&pn=") != -1)
				baseUrlForPs = Regex.Replace(baseUrlForPs, "(&pn=[0-9]*)", "");

                this.BindRepeater(sortBy);
			this.CreateNavigation();
			
			
			
		}

        public void InitializeGrid(ContentFilters filters)
        {
            baseUrlForPs = BaseUrl;
            if (baseUrlForPs.IndexOf("&pn=") != -1)
                baseUrlForPs = Regex.Replace(baseUrlForPs, "(&pn=[0-9]*)", "");
            this.BindRepeater(filters);
            this.CreateNavigation();
        }
		
		// Override the CreateChildControls function. Handle all the Child Repeaters Here.
		protected override void CreateChildControls()
		{
			pnlGrid.Controls.Add( rpt );
		}
			 
				
		void CreateNavigation()
		{
			//get the total record count
			
			totalPages = (int)Math.Ceiling((double)RecordCount / (double)PageSize);
            Trace.Warn("total pages = " + totalPages);
			PageCount = totalPages;
			
			bool firstPage = false, lastPage = false;
			
			string url = BaseUrl;

            if (url.IndexOf("page=") != -1)
            {
                url = Regex.Replace(url, "(page=[0-9]*)", "");
                Trace.Warn("url = " + url);
            }
			
						
			//now check whetner the current page is the first page or the last page
			if(PageCount > 1)
			{
				//find the total number of slots
				int totalSlots = (int)Math.Ceiling((double)PageCount / (double)PagerPageSize);
				int curSlot = ((int)Math.Floor((double)(CurrentPageIndex - 1) / (double)PagerPageSize)) + 1;
			
				Trace.Warn("totalSlots : " + totalSlots.ToString() + " : curSlot : " + curSlot.ToString());	
						
				if(CurrentPageIndex == 1)
					firstPage = true;
				else if(CurrentPageIndex == PageCount)
					lastPage = true;
				
				string navUrls = "";
				
				//add the navigations for first, previous, next and last
				if(firstPage == false)
				{					
					navUrls += "<span class='pg'><a href='" + url + "page/1/'>First</a></span><span class='pg'><a href='" + url + "page/" + (CurrentPageIndex - 1) + "/'>Previous</a></span>";
				}
				else
				{					
					navUrls += "<span class='pgEnd'>First</span><span class='pgEnd'>Previous</span>";
				}
				
				divFirstNav.InnerHtml = navUrls;
				
				navUrls = "";
								
				int startIndex = (curSlot - 1)*PagerPageSize + 1;
				int endIndex = curSlot*PagerPageSize;
				
				if( CurrentPageIndex > 5 )
				{
					// The change would have to happen only after the 3rd page onwards.
					startIndex = CurrentPageIndex - 4; // Start at the current page -4
					endIndex = CurrentPageIndex + 4; // end at the current page + 4 					
				}// condition is true only for PagerPageSize = 5
				
				endIndex = endIndex <= PageCount ? endIndex : PageCount;
				
				//if(curSlot > 1)
					//navUrls += "<span class='pg'><a href='" + url + "&pn=" + (startIndex - 1).ToString() + "'>" + PreviousPagerText + "</a></span>";
					
				for(int i = startIndex; i <= endIndex; i++)
				{
					if(CurrentPageIndex != i)
						navUrls += "<span class='pg'><a href='" + url + "page/" + i.ToString() + "/'>" + i.ToString() + "</a></span>";
					else
						navUrls += "<span class='pgSel'>" + i.ToString() + "</span>";
				}
				
				//if(curSlot < totalSlots)
					//navUrls += "<span class='pg'><a href='" + url + "&pn=" + (endIndex + 1).ToString() + "'>" + NextPagerText + "</a></span>";
				
				divPages.InnerHtml = navUrls;
								
				navUrls = "";
				if(lastPage == false)
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
                divFirstNav.InnerHtml = "";
                divLastNav.InnerHtml = "";
				//No need to create any navigation
			}		
		}
				
		// This function will bind Repeater with the
		// query provided.
		public void BindRepeater(int sortBy)
		{						
			this.SerialNo = (this.CurrentPageIndex - 1) * this.PageSize;
			
			CommonOpn objCom = new CommonOpn();		
						
			Database db = new Database();			
			
			int startIndex = (this.CurrentPageIndex - 1) * this.PageSize + 1;
			int endIndex = this.CurrentPageIndex * this.PageSize;
			
			try
			{
                ICMSContent content = CMSFactory.GetInstance(EnumCMSContentType.AutoExpo);
                var year = new ContentFilters();
                year.EventYear = DateTime.Now.Year;
                List<GenericEntitiyContentList> newsList = new List<GenericEntitiyContentList>();
                int recordCount = 0;
                if (sortBy == 1)
                {

                    newsList = (List<GenericEntitiyContentList>)content.GetContentList<GenericEntitiyContentList>(startIndex, endIndex, out recordCount, OrderBy.Date);
                }
                else if (sortBy == 2)
                {
                    newsList = (List<GenericEntitiyContentList>)content.GetContentList<GenericEntitiyContentList>(startIndex, endIndex, out recordCount, OrderBy.Views);
                }
                RecordCount = recordCount;
                rpt.DataSource = newsList;
                rpt.DataBind();
				Trace.Warn( "Binding Complete..." );
			}
			catch(Exception err)
			{
				Trace.Warn(err.Message + err.Source);
				ErrorClass objErr = new ErrorClass(err,Request.ServerVariables["URL"]);
				objErr.SendMail();
			}
		}

        

        public void BindRepeater(ContentFilters filters)
        {
            this.SerialNo = (this.CurrentPageIndex - 1) * this.PageSize;

            CommonOpn objCom = new CommonOpn();

            Database db = new Database();

            int startIndex = (this.CurrentPageIndex - 1) * this.PageSize + 1;
            int endIndex = this.CurrentPageIndex * this.PageSize;

            try
            {
                Trace.Warn("filters = " + filters.MakeId.ToString());
                ICMSContent content = CMSFactory.GetInstance(EnumCMSContentType.AutoExpo);
                List<GenericEntitiyContentList> newsList = new List<GenericEntitiyContentList>();
                int recordCount = 0;
                newsList = (List<GenericEntitiyContentList>)content.GetFilteredContent<GenericEntitiyContentList, ContentFilters>(startIndex, endIndex, out recordCount, filters);
                RecordCount = recordCount;
                Trace.Warn("Record Count = " + recordCount);
                rpt.DataSource = newsList;
                rpt.DataBind();
                Trace.Warn("Binding Complete..." + newsList.Count);
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message + err.Source);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }
	}
}