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

namespace Bikewale.Controls
{	
	[ParseChildren( true, "Repeaters" )]
	public class RepeaterPagerAjax : UserControl
	{
		/******************************************************************************************/
			//ASP Controls
		/******************************************************************************************/		
		protected Panel pnlGrid;
		protected Label lblRecords, lblRecordsFooter;
		private Repeater rpt = new Repeater();
		protected Repeater rptFeaturedBike;
		RepeaterNewPagerPosition position = RepeaterNewPagerPosition.TopBottom;

		/******************************************************************************************/
			//Html Controls
		/******************************************************************************************/
		protected HtmlGenericControl divPages, divPages1, divFirstNav, divFirstNav1, divLastNav, divLastNav1;
		protected HtmlTableRow TopPager, BottomPager;

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
		private string resultName = "Records";
		public int SerialNo = 0;
		
		private int _pageCount = 1, _curPageIndex = 1, _pagerPageSize = 20, _recordCount = 0;		
		private bool _showHeadersVisible = true;
		
		private int totalPages = 1;
		
		private SqlCommand _cmdParamQ = null;
		private SqlCommand _cmdParamR = null;
		
		public string spotlightUrl = "";
        public string so = string.Empty, sc = string.Empty;

		protected override void OnInit( EventArgs e )
		{
            if (!String.IsNullOrEmpty(HttpContext.Current.Request.QueryString["scr"]) && !String.IsNullOrEmpty(HttpContext.Current.Request.QueryString["so"])) 
            {
                sc = HttpContext.Current.Request.QueryString["scr"];
                so = HttpContext.Current.Request.QueryString["so"];
            }

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
		public void InitializeGrid()
		{
			baseUrlForPs = BaseUrl;
			if(baseUrlForPs.IndexOf("&ps=") != -1)
				baseUrlForPs = Regex.Replace(baseUrlForPs, "(&ps=[0-9]*)", "");

			if(baseUrlForPs.IndexOf("&pn=") != -1)
				baseUrlForPs = Regex.Replace(baseUrlForPs, "(&pn=[0-9]*)", "");
				
		
			this.CreateNavigation();
			
			this.ApplyPosition();
			this.BindRepeater();
			this.ShowHeaders(RecordCount);
		}
		
		// This function will find Positions for pager-strips.
		private void ApplyPosition()
		{
			switch ( this.PagerPosition )
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
			pnlGrid.Controls.Add( rpt );
		}
			 
				
		void CreateNavigation()
		{
			//get the total record count
			RecordCount = GetRecordCount();
			
			totalPages = (int)Math.Ceiling((double)RecordCount / (double)PageSize);
			PageCount = totalPages;
			
			bool firstPage = false, lastPage = false;
			
			string url = BaseUrl;
			
			/*if(url.IndexOf("&ps=") != -1)
				url = Regex.Replace(url, "(&ps=[0-9]*)", "");*/

			//url += "&ps=" + PageSize;
			
			if(url.IndexOf("&pn=") != -1)
				url = Regex.Replace(url, "(&pn=[0-9]*)", "");
						
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
					navUrls += "<span class='pg'><a href='" + url + "&pn=" + (CurrentPageIndex - 1) + "'>Previous</a></span>";
				}
				else
				{					
					navUrls += "<span class='pgEnd'>Previous</span>";
				}
				
				divFirstNav.InnerHtml = navUrls;
				divFirstNav1.InnerHtml = navUrls;
				
				navUrls = "";
								
				int startIndex = (curSlot - 1)*PagerPageSize + 1;
				int endIndex = curSlot*PagerPageSize;
				endIndex = endIndex <= PageCount ? endIndex : PageCount;
				
				//if(curSlot > 1)
					//navUrls += "<span class='pg'><a href='" + url + "&pn=" + (startIndex - 1).ToString() + "'>" + PreviousPagerText + "</a></span>";
					
				for(int i = startIndex; i <= endIndex; i++)
				{
					if(CurrentPageIndex != i)
						navUrls += "<span class='pg'><a href='" + url + "&pn=" + i.ToString() + "'>" + i.ToString() + "</a></span>";
					else
						navUrls += "<span class='pgSel'>" + i.ToString() + "</span>";
				}
				
				//if(curSlot < totalSlots)
					//navUrls += "<span class='pg'><a href='" + url + "&pn=" + (endIndex + 1).ToString() + "'>" + NextPagerText + "</a></span>";
				
				divPages.InnerHtml = navUrls;
				divPages1.InnerHtml = navUrls;
				
				navUrls = "";
				if(lastPage == false)
				{
					navUrls += "<span class='pg'><a href='" + url + "&pn=" + (CurrentPageIndex + 1) + "'>Next</a></span>";						
				}
				else
				{
					navUrls += "<span class='pgEnd'>Next</span>";					
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
			
			string sql = "";
			CommonOpn objCom = new CommonOpn();		
						
			Database db = new Database();			
			
			
			int startIndex = (this.CurrentPageIndex - 1) * this.PageSize + 1;
			int endIndex = this.CurrentPageIndex * this.PageSize;

            Trace.Warn("start index", startIndex.ToString());
            Trace.Warn("end index", endIndex.ToString());

			//form the query. Only fetch the desired rows. 
			//sql = " Select * From (Select Top " + endIndex + " DENSE_RANK() Over (Order By " + OrderByClause + ") AS RowN, "
            
            /*sql = " Select * From (Select DENSE_RANK() Over (Order By " + OrderByClause + ") AS RowN, "
                + " " + SelectClause + " From " + FromClause + " "
                + (WhereClause != "" ? " Where " + WhereClause + " " : "")
                + " ) AS TopRecords Where " 
                + " RowN >= " + startIndex + " AND RowN <= " + endIndex + " ";*/

            // Code Commented By : Ashish G. Kamble on 28 Mar 2013
            //sql = " Select * From (Select DENSE_RANK() Over (Order By " + OrderByClause + ") AS RowN, * FROM ( SELECT "
            //    + " " + SelectClause + " From " + FromClause + " "
            //    + (WhereClause != "" ? " Where " + WhereClause + " " : "")
            //    + " )AS tbl ) AS TopRecords Where "
            //    + " RowN >= " + startIndex + " AND RowN <= " + endIndex + " ";

            //Trace.Warn("SelectClause : ", SelectClause);
            // Modified by : Suresh on 24 July 2014 
            // Summary : Changed query to sort records by price
            sql = " WITH CTE_BikeModels AS( "
                + " SELECT "
                + " DENSE_RANK() OVER( ORDER BY MO.MinPrice ) AS DenseRank, "
                + SelectClause
                + " FROM "
                + FromClause
                + (WhereClause != "" ? " Where " + WhereClause + " " : "")
                + " ) "
                + " SELECT * "
                + " FROM CTE_BikeModels WITH (NOLOCK) "
                + " WHERE DenseRank BETWEEN " + startIndex + " AND " + endIndex
                + " ORDER BY MinPrice ";               

			Trace.Warn("Fetch the desired rows : " + sql);			
			try
			{
				DataSet ds = new DataSet();
				if ( sql != "" )
				{
					Trace.Warn( "Binding Sql : ");
					
					CmdParamQ.CommandText = sql;
					ds = db.SelectAdaptQry( CmdParamQ );
					
					AddFeaturedBikeToDataSet(ds);
					
					rpt.DataSource = ds;
					rpt.DataBind();
					Trace.Warn( "PageSize: " + PageSize);	
				}
				Trace.Warn( "Binding Complete..." );
			}
			catch(Exception err)
			{
				Trace.Warn(err.Message + err.Source);
				ErrorClass objErr = new ErrorClass(err,Request.ServerVariables["URL"]);
				objErr.SendMail();
			}
		}
		
		
		int GetRecordCount()
		{
			int count = 0;
			SqlDataReader dr = null;
			Database db = new Database();

            try
            {
                if (RecordCountQuery != "")
                {
                    CmdParamR.CommandText = RecordCountQuery;

                    Trace.Warn("RecordCountQuery: " + RecordCountQuery);
                    dr = db.SelectQry(CmdParamR);

                    if (dr.Read())
                    {
                        count = Convert.ToInt32(dr[0]);
                    }

                }

            }
            catch (Exception err)
            {
                Trace.Warn(err.Message + err.Source);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                if(dr != null)
                    dr.Close();
                db.CloseConnection();
            }

			return count;
		}
		
		
		private void ShowHeaders( int totalRecords )
		{
            HttpContext.Current.Trace.Warn("total records : ", totalRecords.ToString());
            HttpContext.Current.Trace.Warn("CurrentPageIndex : ", CurrentPageIndex.ToString());
            HttpContext.Current.Trace.Warn("PageSize : ", PageSize.ToString());

			if(ShowHeadersVisible == true)
			{
				string showingFrom = CurrentPageIndex > 1 ? ( ((CurrentPageIndex-1) * PageSize) + 1 ).ToString() : "0";

				if( totalRecords > PageSize && totalRecords >= (CurrentPageIndex * PageSize))
				{
					lblRecords.Text = "Showing <span>" + (showingFrom == "0" ? (Convert.ToInt32(showingFrom)+1).ToString() : showingFrom) +"-"+ (CurrentPageIndex * PageSize).ToString()  + "</span> of " + totalRecords;
					lblRecordsFooter.Text = lblRecords.Text;
				}
				else if( totalRecords == 0 )
				{
					ShowHeadersVisible = false;
					lblRecords.Text = "";
					lblRecordsFooter.Text	 = "";			
				}
				else if( totalRecords < (CurrentPageIndex * PageSize) )
				{
					lblRecords.Text = "Showing <span>" + (showingFrom == "0" ? (Convert.ToInt32(showingFrom)+1).ToString() : showingFrom)  +"-" + totalRecords;
					lblRecordsFooter.Text = lblRecords.Text;
					Trace.Warn("CurrentPageIndex *: " + (CurrentPageIndex * PageSize).ToString() + ":totalRecords:" + totalRecords);																					
				}
				else
				{
					lblRecords.Text = "Showing <span> 0-"+ totalRecords  + "</span>";
					lblRecordsFooter.Text = lblRecords.Text;
				}	
			}
			else
			{
				lblRecords.Text = "";
				lblRecordsFooter.Text	 = "";			
			}
				
		}

        void AddFeaturedBikeToDataSet(DataSet ds)
        {
            string versions_str = string.Empty;

            if (ds.Tables[0].Rows.Count > 0)
            {
                // If versions available to show
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    versions_str += ds.Tables[0].Rows[i]["VersionId"].ToString() + ",";
                }

                versions_str = versions_str.Substring(0, versions_str.Length - 1);
                string featuredVersion = Bikewale.Common.FeaturedBike.GetFeaturedVersion(versions_str);
                Trace.Warn("AddFeaturedBikeToDataSet featuredVersion : " + featuredVersion);
                if (featuredVersion != "")
                {
                    Trace.Warn("binding featured versions data");
                    string featuredVerId = featuredVersion.Split('#')[0];
                    spotlightUrl = featuredVersion.Split('#')[1];
                    rptFeaturedBike.DataSource = Bikewale.Common.FeaturedBike.GetFeaturedBikeForNewSearch(featuredVerId);
                    rptFeaturedBike.DataBind();
                }
            }
        }
		
		public string GetMiledge(string mileage)
		{
			if( mileage != "" )
				return ", " + mileage + "kpl";
			else
				return "";
		}
		
		string modelHead = string.Empty;
		public string GetModelRow( string modelId, string modelCount, string bikeModel, string modelReviewRate, 
								   string minPrice, string maxPrice, string hostUrl,string originalImagePath, string modelReviewCount, string makeName, string modelName, string MakeMaskingName,string ModelMaskingName )
		{
			StringBuilder sb = new StringBuilder();
			
			if( modelHead == string.Empty )
			{
				sb.Append("<tr id='mod_"+ modelId +"' class='model-row fearured dt_body'>");
                sb.Append("<td><img class='img-border' alt='" + bikeModel + "' title='" + bikeModel + "' src='" + Bikewale.Utility.Image.GetPathToShowImages(originalImagePath, hostUrl, Bikewale.Utility.ImageSize._110x61) + "' /></td>");
                    sb.Append("<td><a class='href-title' href='" + (spotlightUrl == "" ? ("/" + MakeMaskingName + "-models/" + ModelMaskingName) : spotlightUrl) + "/'>" + bikeModel);
                    sb.Append("</a><p class='text-grey'><a id='" + modelId + "' class='text-grey viewVersions'><span id='modShowIcon' class='icon-sheet2 right-arrow2'></span><span id='modShow' class='show' >View " + (modelCount) + " versions.</span><span id='modHide' class='hide' >Hide versions.</span></a></p>");
					sb.Append("</td>");
					sb.Append("<td class='price2'>Rs."+ (Math.Round(Convert.ToDouble(minPrice)/100000,2)).ToString() + "-" + (Math.Round(Convert.ToDouble(maxPrice)/100000,2)).ToString() + " lac <p></p>&nbsp;</td>");
					sb.Append("<td align='center'>"+  CommonOpn.GetRateImage( Convert.ToDouble(modelReviewRate) ) +"<p><a title='"+ modelReviewCount +" Reviews of "+ bikeModel +"' href='/research/" + MakeMaskingName + "-bikes/" + ModelMaskingName + "/userreviews/' class='href-grey'>"+ modelReviewCount +" Reviews</a></p> <p class='text-grey mid-box'>sponsored</p></td>");
					sb.Append("<td>&nbsp;</td>");
				sb.Append("</tr>");
			}
			
			modelHead = modelId;
				
			return sb.ToString();
		}

        public string SortColumnBy(string sortOrder, string sortCriteria)
        {
            string url = string.Empty;

            if (sc != string.Empty && so != string.Empty)
            {
                if (sc != sortCriteria)
                    url = BaseUrlQS + "&scr=" + sortCriteria + "&so=0";
                else
                    url = BaseUrlQS + "&scr=" + sortCriteria + "&so=" + (so == "1" ? "0" : "1");
            }
            else
                url = BaseUrlQS + "&scr=" + sortCriteria + "&so=" + sortOrder;

            return url;
        }

        public string GetSortImage(string sortCriteria)
        {
            string sortImage = string.Empty;

            if (sortCriteria == sc)
            {
                sortImage = (so == "1" ? "<img src='http://img.carwale.com/used/sorting-down.png' border='0' />" :
                                "<img src='http://img.carwale.com/used/sorting-up.png' border='0' />");
            }

            return sortImage;
        }

        /*string _FeaturedVersion = string.Empty;
        public string FeaturedVersion
        {
            get{ return _FeaturedVersion; }
            set{ _FeaturedVersion = value; }
        }*/

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

        // This property hold all the CmdParam
        public SqlCommand CmdParamQ //command variable to store the parameters for query
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

        public SqlCommand CmdParamR	//command variable for the record count
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

        // The term to be used for base url, to this the pc for pagecount and pn for page numbers will be added
        string _BaseUrlQS = string.Empty;
        public string BaseUrlQS
        {
            get { return _BaseUrlQS; }
            set { _BaseUrlQS = value; }
        }
	}
}