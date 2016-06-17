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
using System.Text.RegularExpressions;
using Bikewale.Common;
using System.Data.Common;
using Bikewale.Notifications.CoreDAL;

namespace Bikewale.Controls
{	
	[ParseChildren( true, "Repeaters" )]
	public class RepeaterPagerUsed : UserControl
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
		protected HtmlGenericControl divPages1, divFirstNav1, divLastNav1, div_nec/* Not enaugh bikes */;
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
			get{return _pageCount;}
			set{_pageCount = value;}
		}
		
		public bool ShowHeadersVisible
		{
			get{return _showHeadersVisible;}
			set{_showHeadersVisible = value;}
		}
		
		
		public int CurrentPageIndex
		{
			get{return _curPageIndex;}
			set{_curPageIndex = value;}
		}
		
		// This property will be used as container for 
		// DatGrid binding sql string.
		public string SelectClause
		{
			get{return _selectClause;}
			set{_selectClause = value;}
		}
		
		public string FromClause
		{
			get{return _fromClause;}
			set{_fromClause = value;}
		}
		
		public string WhereClause
		{
			get{return _whereClause;}
			set{_whereClause = value;}
		}
		
		public string OrderByClause
		{
			get{return _orderByClause;}
			set{_orderByClause = value;}
		}
		
		public string GroupByClause
		{
			get{return _groupByClause;}
			set{_groupByClause = value;}
		}

        // The term to be used for base url to create pagination
        private string _DefaultURL = string.Empty;
        public string DefaultURL
        {
            get { return _DefaultURL; }
            set { _DefaultURL = value; }
        }

		// This property will be used as container for 
		// the query for counting the total number of records
		public string RecordCountQuery
		{
			get{return recCountQry;}
			set{recCountQry = value;}
		} // Query
		
				
		public int RecordCount
		{
			get { return _recordCount; }
			set { _recordCount = value; }
		} // RecordCount

        DbCommand _CmdParamQry;
		public DbCommand CmdParamQry //command variable to store the parameters for query
		{
			get { return _CmdParamQry; }
			set { _CmdParamQry = value; }
		} // CmdParam

        DbCommand _CmdParamCountQry;
        public DbCommand CmdParamCountQry //command variable to store the parameters for query
		{
			get { return _CmdParamCountQry; }
			set { _CmdParamCountQry = value; }
		} 
				
		public int PageSize
		{
			get { return 20; }
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
			set { _pagerPageSize = value;}
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

        public string City { get; set; }
        public string PrevIndex { get; set; }
        public string NextIndex { get; set; }
		
		protected override void OnInit( EventArgs e )
		{
			InitializeComponents();
		}
		
		void InitializeComponents()
		{			
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
			pnlGrid.Controls.Add( rpt );
		}
				
		// This function will bind Repeater with the
		// query provided.
		public void BindRepeater()
		{						
			this.SerialNo = (this.CurrentPageIndex - 1) * this.PageSize;
			
			string sql = string.Empty;
			
			CommonOpn objCom = new CommonOpn();															
			
			int startIndex = (this.CurrentPageIndex - 1) * this.PageSize + 1;
			int endIndex = this.CurrentPageIndex * this.PageSize;						
									
            //sql = " Select * From (Select Row_Number() Over (Order By " + OrderByClause + ") AS RowN, "
            //    + " " + SelectClause + " From " + FromClause + " "
            //    + (WhereClause != "" ? " Where " + WhereClause + " " : "")
            //    + " ) AS TopRecords Where " 
            //    + " RowN >= " + startIndex + " AND RowN <= " + endIndex + " ";

            sql = string.Format(@"set @row_number:=0;
                                select * from 
                                (
                                    select {0},@row_number:= @row_number+1 as rown
                                     from {1}
                                     {2}  
                                    order by  {3}
                                ) as t
                                where rown between {4} and {5};
                                ", SelectClause, FromClause, (!string.IsNullOrEmpty(WhereClause)) ? string.Format(" Where {0} ", WhereClause) : string.Empty, OrderByClause, startIndex, endIndex);
 
			try
			{
				if ( sql != "" )
				{
					// Assign CommandText to the SqlCommand
					CmdParamQry.CommandText = sql;

					
					// Execute Sql command and bind data with the repeater.
                    rpt.DataSource = MySqlDatabase.SelectAdapterQuery(CmdParamQry);
					rpt.DataBind();					
				}				
			}
			catch(Exception err)
			{
				Trace.Warn(err.Message + err.Source);
				ErrorClass objErr = new ErrorClass(err,Request.ServerVariables["URL"]);
				objErr.SendMail();
			}
		}
		
		// Total number of records matched users criteria	
		int GetRecordCount()
		{
            int count = 0;

            try
            {
                if (RecordCountQuery != "")
                {
                    CmdParamCountQry.CommandText = RecordCountQuery;

                    using (IDataReader dr = MySqlDatabase.SelectQuery(CmdParamCountQry))
                    {
                        if (dr != null && dr.Read())
                        {
                            count = Convert.ToInt32(dr[0]);
                            dr.Close();
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
		
		// Function to create paging navigation, so that user can browse across the pages.
		void CreateNavigation()
		{
			//get the total record count
			RecordCount = GetRecordCount();
			
			totalPages = (int)Math.Ceiling((double)RecordCount / (double)PageSize);
			PageCount = totalPages;
			
			bool firstPage = false, lastPage = false;
			
            ///Commented By : Ashwini Todkar on 15 April 2014 - Not more required
            //StateCity city = new StateCity();                    
			//string url = " ";//"/used-cars-in-" + city.GetCityDetails(CityId).City;
			
			/*if(url.IndexOf("&ps=") != -1)
				url = Regex.Replace(url, "(&ps=[0-9]*)", "");*/
			
			//if(url.IndexOf("&pn=") != -1)
			//	url = Regex.Replace(url, "(&pn=[0-9]*)", "");
						
			// now check whetner the current page is the first page or the last page
			// PagerPageSize = Number of Pager numbers to be shown at one time
			// PageCount = Total number of Pages.
			// totalSlots = ( in this case ) no of sets of n pages. Here n = 5; total pages = 8 
			//				so totalSlots = total no of pages / PagerPageSize = 8 / 5 ~~ 2 (rounded value)
			
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

                PrevIndex = Convert.ToString(CurrentPageIndex - 1);
                NextIndex = Convert.ToString(CurrentPageIndex + 1);

				//add the navigations for first, previous, next and last
				if(firstPage == false)
				{              
                    navUrls += "<a navid='" + (CurrentPageIndex - 1) + "' href='"+  (!String.IsNullOrEmpty(DefaultURL)? DefaultURL : "")+ "page-" + (CurrentPageIndex - 1) + "/" + "'><span class='pgSel'>Previous</span></a>";
				}
				else
				{					
					navUrls += "<span class='pgEnd'>Previous</span>";
                    PrevIndex = "";
				}
								
				divFirstNav1.InnerHtml = navUrls;
				
				navUrls = "";
								
				int startIndex = (curSlot - 1)*PagerPageSize + 1;
				int endIndex = curSlot*PagerPageSize;
				
				
				Trace.Warn("startIndex = " + startIndex.ToString());
                Trace.Warn("endIndex = " + endIndex.ToString());
				//if(curSlot > 1)
					//navUrls += "<span class='pg'><a href='" + url + "&pn=" + (startIndex - 1).ToString() + "'>" + PreviousPagerText + "</a></span>";
				
				// Show the current page in the middle of the pager display.	
				if( CurrentPageIndex > 3 )
				{
					// The change would have to happen only after the 3rd page onwards.
					startIndex = CurrentPageIndex - 2; // Start at the current page -2
					endIndex = CurrentPageIndex + 2; // end at the current page + 2 					
				}// condition is true only for PagerPageSize = 5
				
				endIndex = endIndex <= PageCount ? endIndex : PageCount; // if end page number exceeds page count, set it to page count
				
				for(int i = startIndex; i <= endIndex; i++)
				{
					if(CurrentPageIndex != i)
                        navUrls += "<a navid='" + i.ToString() + "' href='" + (!String.IsNullOrEmpty(DefaultURL)? DefaultURL : "") + "page-" + i.ToString() + "/" + "'><span class='pg'>" + i.ToString() + "</span></a>";
					else
                        navUrls += "<span navid='" + i.ToString() + "' class='pgSel'>" + i.ToString() + "</span>";
				}
				//if(curSlot < totalSlots)
					//navUrls += "<span class='pg'><a href='" + url + "&pn=" + (endIndex + 1).ToString() + "'>" + NextPagerText + "</a></span>";
								
				divPages1.InnerHtml = navUrls;
				
				navUrls = "";
				if(lastPage == false)
				{
                    navUrls += "<a navid='" + (CurrentPageIndex + 1) + "' href='" + (!String.IsNullOrEmpty(DefaultURL) ? DefaultURL : "") + "page-" + (CurrentPageIndex + 1) + "/" + "'><span class='pgSel'>Next</span></a>";                                                            
				}
				else
				{
					navUrls += "<span class='pgEnd'>Next</span>";                    
                    NextIndex = "";
				}
												
				divLastNav1.InnerHtml = navUrls;
			}
			else
			{				
				divPages1.InnerHtml = "";
				//No need to create any navigation
			}		
		}		
		
		private void ShowHeaders( int totalRecords )
		{
			if(ShowHeadersVisible == true)
			{
				if( totalRecords == 0 )
				{
					ShowHeadersVisible = false;
					lblRecords.Text = "";
					lblRecordsFooter.Text = "";
				}
				else
				{
					int showingFrom = CurrentPageIndex > 1 ? ((CurrentPageIndex-1) * PageSize) + 1  : 1;
					int showingTo = totalRecords < (CurrentPageIndex * PageSize) ? totalRecords : (CurrentPageIndex * PageSize);
					
					lblRecords.Text = "Showing <span class='price2'>" + showingFrom +"-"+  showingTo + "</span> of <span class='price2'>" + totalRecords +"</span> Bikes";
					lblRecordsFooter.Text = lblRecords.Text;
				}								
			}				
		}
		
		/*void GetEntireStateCount()
		{
			SqlDataReader dr = null;
			Database db = new Database();
			
			try
			{					
				string sql = " Select COUNT(profileid) StateCount, StateId, StateName From LiveListings LL "
							+ " Where LL.StateId IN(SELECT Top 1 StateId FROM LiveListings WHERE CityId = @CityId) "
							+ " GROUP BY StateId, StateName";
				Trace.Warn(sql + "    cityId : " + CityId);			
				SqlCommand cmd = new SqlCommand(sql);
				cmd.Parameters.Add("@CityId", SqlDbType.BigInt).Value = CityId;
				
				dr = db.SelectQry(cmd);
				
				if(dr.Read())
				{
					stateCount = dr[0].ToString();
					stateId = dr[1].ToString();
					stateName = dr[2].ToString();
					
					// If matching record count is between 1 to 10. Start showing user a message
					// "Not enough bikes? If you increase the Kms Around in the left, you may get more bikes."
					if(Convert.ToInt32(stateCount) > 0 ){
						div_nec.Visible = true;							
					}
				}
			}
			catch(Exception err)
			{
				Trace.Warn(err.Message + err.Source);
				ErrorClass objErr = new ErrorClass(err,Request.ServerVariables["URL"]);
				objErr.SendMail();
			}
			finally
			{
				dr.Close();
				db.CloseConnection();
			}					
		}*/
	}// class
} // namespace