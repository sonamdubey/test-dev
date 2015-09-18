using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Bikewale.Common;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.IO;
using Bikewale.Memcache;
using System.Text.RegularExpressions;

namespace Bikewale.Used
{
    /// <summary>
    ///     Created By : Ashish G. Kamble on 9/8/2012
    /// </summary>
    public class Search : Page
    {
        protected Repeater rptMakes;
        protected DropDownList drpCity;
        protected HtmlGenericControl searchRes;
        protected string city = string.Empty, cityId = string.Empty, prevUrl = string.Empty, nextUrl = string.Empty, makeId = string.Empty,queryString = String.Empty;
        protected string makeMaskingName = String.Empty, modelMaskingName = String.Empty, cityMaskingName = String.Empty, make = String.Empty, model = String.Empty, modelId = String.Empty, pageNumber = String.Empty, nextPrevBaseUrl = String.Empty;
        protected string pageCanonical = String.Empty, pageKeywords = String.Empty, pageDescription = String.Empty, pageTitle = String.Empty;

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        protected void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //Get pageNumber to form next and prvious page urls
            if (Request["pn"] != null && Request.QueryString["pn"] != "")
            {
                if (CommonOpn.CheckId(Request.QueryString["pn"]) == true)
                    pageNumber = Request.QueryString["pn"];
                Trace.Warn("pn: " + Request.QueryString["pn"]);
            }

            if (!IsPostBack)
            {
                try
                {
                    BindCity();
                    BindMakeRepeater();

                    string _qs = String.Empty;
                    
                    //form query string to load result
                    processQueryString(ref _qs);

                    // Load search results according to query string.
                    LoadSearchResults(_qs);

                    //if search criteria contains city then set ddl to selected option
                    if(!String.IsNullOrEmpty(cityId))
                        drpCity.SelectedValue = cityId + '_' + cityMaskingName;
                }
                catch (Exception ex)
                {
                    Response.Redirect("/pagenotfound.aspx",false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;

                    Trace.Warn("search Page Load : ", ex.Message);
                    ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                    objErr.SendMail();
                }
            }
        }//End of PageLoad

        /// <summary>
        /// Written By : Ashwini Todkar on 17 April 2014
        /// Summary    : method to form query string
        /// </summary>
        /// <param name="_qs"> query string for loading the search result page</param>
        private void processQueryString(ref string _qs)
        {
            string _tempQS = string.Empty,_tempBudget = string.Empty,_pageNo = string.Empty;

            try
            {
                if (!String.IsNullOrEmpty(ClassifiedCookies.UsedSearchQueryString))
                {
                    _tempQS = ClassifiedCookies.UsedSearchQueryString;
                }

                Match matchBudget = Regex.Match(_tempQS, "budget=[0-9]");

                if (matchBudget.Success)
                {
                    _tempBudget = matchBudget.Value;
                }

                if (!String.IsNullOrEmpty(Request.QueryString["city"]))
                {
                    cityId = CitiMapping.GetCityId(Request.QueryString["city"]);
                    cityMaskingName = Request.QueryString["city"];
                }

                if (!String.IsNullOrEmpty(Request.QueryString["make"]))
                {
                    makeId = MakeMapping.GetMakeId(Request.QueryString["make"]);
                    makeMaskingName = Request.QueryString["make"];
                }

                _qs = ((!String.IsNullOrEmpty(cityId)) ? "city=" + cityId + "&dist=50" : "") + ((String.IsNullOrEmpty(makeId) ? "" : "&make=" + makeId));


                if (!String.IsNullOrEmpty(_tempBudget))
                {
                    _qs += "&" + _tempBudget;
                }
                //else if (!String.IsNullOrEmpty(Request.QueryString["budget"]))
               // {
                   // _qs += "&budget=" + Request.QueryString["budget"];
                //}

                if (!String.IsNullOrEmpty(Request.QueryString["pn"]))
                {
                    _qs += "&pn=" + Request.QueryString["pn"];
                }

                // Remove first & in the url
                if (_qs.IndexOf("&") == 0)
                {
                    _qs = _qs.Remove(0, 1);
                }

                queryString = _qs;
                Trace.Warn("query  string",queryString);


                // query string with page url for paging purpose on page load.
                string _url = HttpContext.Current.Request.ServerVariables["HTTP_X_REWRITE_URL"];
                string _finalUrl = string.Empty;

                if (_url.IndexOf("page") >= 0)
                {
                    _finalUrl = _url.Split(new string[] { "page" }, StringSplitOptions.None)[0];
                    Trace.Warn("final url", _url + "  " + _finalUrl);
                }
                else
                {
                    _finalUrl = _url;
                }

                _qs += "&pageUrl=" + _finalUrl;

                //Remove _USQueryString cookie if exists
                HttpCookie objCookie;
                objCookie = new HttpCookie("_USQueryString");
                objCookie.Value = "";
                objCookie.Expires = DateTime.Now.AddDays(-1);
                HttpContext.Current.Response.Cookies.Add(objCookie);

            }
            catch (Exception ex)
            {
                Trace.Warn("processQueryString : ", ex.Message);
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }//End of processQueryString

        //Commented By : Ashwini Todkar on 17 April 2014 - not more required
        //protected void DrpCity_Change(object sender, EventArgs e)
        //{
        //    Trace.Warn("City Index changes");

        //    Response.Redirect("/used-bikes-in-mumbai/");
        //}

        //protected void GetCityName()
        //{
        //    StateCity objCity = new StateCity();
            
        //    Cities obj = objCity.GetCityDetails(cityId);
        //    city = obj.City;

        //    Trace.Warn("city : " + obj.City);
        //}

        //protected void CreateHtmlSnapshot()
        //{
        //    // Get the complete query string.
        //    //string query_string = Request.QueryString["_escaped_fragment_"];
        //    string query_string = Request.QueryString.ToString().Replace("_escaped_fragment_=","");
            
        //    if (Request.QueryString.ToString().IndexOf("dist") <= 0)
        //    {
        //        query_string += "&dist=50";
        //        Trace.Warn("query : ", query_string);
        //    }

        //    Trace.Warn("query_string : " + query_string);

        //    // Removed _escaped_fragment_ from the query string to get actual query string.
        //    string qs = query_string.Replace("%3d", "=");


        //    LoadSearchResults(qs);
        //}


        
        /// <summary>
        /// Written By : Ashwini Todkar on 16 April 2014
        /// Summary    : Function will load the search result page on the current page.
        /// </summary>
        /// <param name="qs"></param>
        private void LoadSearchResults(string qs) 
        {
            string host = Request.ServerVariables["HTTP_HOST"].ToString();

            string url = "http://" + host + "/used/searchresult.aspx" + (String.IsNullOrEmpty(qs) ? "" : ("?" + qs));

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url.Replace("%3d", "="));
                   
                Trace.Warn("+++request", request.RequestUri.ToString());
                request.Headers.Add("pageIndex", "-1");
                    
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                StreamReader stream = new StreamReader(response.GetResponseStream());
                searchRes.InnerHtml = stream.ReadToEnd();

                string pageIndex = string.Empty;
                // Next and prev page index will be add into the headers.
                // Page index will give crawler next and previous page links.
                if (!String.IsNullOrEmpty(response.Headers["pageIndex"].ToString()))
                {
                    pageIndex = response.Headers["pageIndex"].ToString();
                    Trace.Warn("Page index on search page",pageIndex);
                    Trace.Warn("page number on search page ,", pageNumber);
                }
                Trace.Warn("GetLinkToPrevNextPages" + pageIndex);

                // Create title, description and meta keywords for the search page.
                GetMetaKeywords();

                // Get Links to previous and next pages in case of paging.
                GetLinkToPrevNextPages(pageIndex);                    
            }
            catch (Exception ex)
            {
                Trace.Warn("LoadSearchResults : ",ex.Message);
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

        /// <summary>
        /// Summary     : Function to get used bike makes
        /// Modified By : Ashwini Todkar on 17 April 2014
        /// </summary>
        private void BindMakeRepeater()
        {
            try
            {
                MakeModelVersion mmv = new MakeModelVersion();
                rptMakes.DataSource = mmv.GetMakes("USED");
                rptMakes.DataBind();
            }
            catch (Exception ex)
            {
                Trace.Warn(ex.Message);
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }
       
        /// <summary>
        /// Modified By : Ashwini Todkar on 14 April 2014
        /// Summary     : Function to get all used cities 
        /// </summary>
        private void BindCity()
        {
            try
            {
                StateCity objCity = new StateCity();
                DataTable dt = objCity.GetCitiesWithMappingName("USED");

                drpCity.DataTextField = "Text";
                drpCity.DataValueField = "Value";
                drpCity.DataSource = dt;
                drpCity.DataBind();

                // adding by default main cities at top
                drpCity.Items.Insert(0, new ListItem("-----------", "-1"));
                drpCity.Items.Insert(0, new ListItem("Hyderabad", "105_hyderabad"));
                drpCity.Items.Insert(0, new ListItem("Pune", "12_pune"));
                drpCity.Items.Insert(0, new ListItem("Ahmedabad", "128_ahmedabad"));
                drpCity.Items.Insert(0, new ListItem("Kolkata", "198_kolkata"));
                drpCity.Items.Insert(0, new ListItem("Chennai", "176_chennai"));
                drpCity.Items.Insert(0, new ListItem("Bangalore", "2_bangalore"));
                drpCity.Items.Insert(0, new ListItem("New Delhi", "10_newdelhi"));
                drpCity.Items.Insert(0, new ListItem("Mumbai", "1_mumbai"));
                drpCity.Items.Insert(0, new ListItem("-----------", "-1"));
                drpCity.Items.Insert(0, new ListItem("All India", "0"));

            }
            catch (Exception ex)
            {
                Trace.Warn(ex.Message);
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

        /// <summary>
        ///     Written By : Ashish G. Kamble on 1 Apr 2013
        ///     Summary : Function will create links for the crawler to next and prev pages.
        /// </summary>
        /// <param name="pageIndex"></param>
        protected void GetLinkToPrevNextPages(string pageIndex)
        {
            string nextPageNo = string.Empty, prevPageNo = string.Empty;

            if (pageIndex.IndexOf(",") >= 0)
            {
                prevPageNo = pageIndex.Split(',')[0];
                nextPageNo = pageIndex.Split(',')[1];
            }

            if (!String.IsNullOrEmpty(prevPageNo))
            {
                if (prevPageNo == "")
                {
                    nextUrl = nextPrevBaseUrl + nextPageNo + "/";
                }
                else
                {
                    prevUrl = nextPrevBaseUrl + prevPageNo + "/";
                }
            }

            if (!String.IsNullOrEmpty(nextPageNo))
            {
                nextUrl = nextPrevBaseUrl + nextPageNo + "/";
            }

            Trace.Warn("next prev url  : " + prevUrl);
            Trace.Warn("next prev url  : " + nextUrl);
        }

        /// <summary>
        ///   Written By : Ashwini Todkar on 22 Feb 2013
        ///   Summary : PopulateWhere to fetch meta info related to page.
        /// </summary>
        private void GetMetaKeywords()
        {
            MetaKeywordsUsed objKeywords = new MetaKeywordsUsed();

            objKeywords.GetMetaKeywordsSearchPage(makeId, modelId, cityId, makeMaskingName, modelMaskingName, cityMaskingName, pageNumber);

            pageTitle = objKeywords.PageTitle;
            pageCanonical = objKeywords.Canonical;
            pageKeywords = objKeywords.PageKeywords;
            pageDescription = objKeywords.PageDescription;

            nextPrevBaseUrl = objKeywords.BaseURL;
     
        }// End of GetMetaKeywords method

    }   // End of class
}   // End of namespace