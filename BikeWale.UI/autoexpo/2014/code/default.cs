using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Bikewale.Common;
using System.Configuration;
using System.Text.RegularExpressions;
using Carwale.CMS;
using Carwale.CMS.Entities;
using System.Collections.Generic;
using System.ComponentModel;
using Carwale.CMS.DAL.AutoExpo;

namespace AutoExpo
{
    public class DefaultClass : Page
    {
        protected RepeaterPagerNews rpgNews;
        
        protected string[] vidArr = new string[3], imgArr = new string[8], vidIdArr = new string[3], vidUrl = new string[3];
        protected DropDownList ddlMake, ddlModel, ddlDays;
        protected Repeater rptNews;
        protected HtmlInputButton FilterData;
        private string pageNumber = string.Empty;
        protected HtmlAnchor PopularNews, LatestNews,viewAll;
        protected HtmlGenericControl LatestNewsSpan,PopularNewsSpan;
        protected HtmlGenericControl divGallery, video1, video2, image1, image2;
        protected string NewsTitle = string.Empty, NewsBreadCrumb = string.Empty, BaseUrl = string.Empty;
        protected HtmlInputHidden hdn_ddlMake, hdn_ddlModel;

        string tempMake = "", tempModel = "";
        
        /// <summary>
        /// cookie for panel selction.
        /// </summary>
        public string PanelSelectionCook
        {
            get
            {
                if (HttpContext.Current.Request.Cookies["Panel"] != null && HttpContext.Current.Request.Cookies["Panel"].Value.ToString() != "")
                {
                    return HttpContext.Current.Request.Cookies["Panel"].Value.ToString();
                }
                else return "1";
            }
            set
            {

                HttpCookie objCookie;
                objCookie = new HttpCookie("Panel");
                objCookie.Value = value;
                HttpContext.Current.Response.Cookies.Add(objCookie);
            }
            
        }

        /// <summary>
        /// Gets the selected make from the drop down.
        /// </summary>
        public string MakeDetail
        {
            get
            {
                if (Request.Form["ddlMake"] != null && Request.Form["ddlMake"].ToString() != "")
                    return Request.Form["hdn_ddlMake"].ToString();
                else
                    return "";
            }
        }

        /// <summary>
        /// Gets the selected model from the dropdown.
        /// </summary>
        public string ModelDetail
        {
            get
            {
                if (Request.Form["ddlModel"] != null && Request.Form["ddlModel"].ToString() != "")
                    return Request.Form["hdn_ddlModel"].ToString();
                else
                    return "";
            }


        }

        /// <summary>
        /// Cookie to store the selected model in the filter.
        /// </summary>
		public static string ModelCook
		{
			get
			{
				if( HttpContext.Current.Request.Cookies["Model"] != null && HttpContext.Current.Request.Cookies["Model"].Value.ToString() != "" )
				{
					return HttpContext.Current.Request.Cookies["Model"].Value.ToString();
				}
				else return "-1";
			}
			set
			{
                
				HttpCookie objCookie;
				objCookie = new HttpCookie("Model");
				objCookie.Value = value;
				HttpContext.Current.Response.Cookies.Add(objCookie);
			}
		}

        /// <summary>
        /// Cookie to store the selected make in the filter.
        /// </summary>
        public static string MakeCook
		{
			get
			{
				if( HttpContext.Current.Request.Cookies["Make"] != null && HttpContext.Current.Request.Cookies["Make"].Value.ToString() != "" )
				{
					return HttpContext.Current.Request.Cookies["Make"].Value.ToString();
				}
				else return "-1";
			}
			set
			{
				HttpCookie objCookie;
				objCookie = new HttpCookie("Make");
				objCookie.Value = value;
				HttpContext.Current.Response.Cookies.Add(objCookie);
			}
		}

        /// <summary>
        /// Gets the Selected Date Filter Value.
        /// </summary>
        public string EventDate
        {
            get
            {
                if (Request.Form["ddlDays"] != null && Request.Form["ddlDays"].ToString() != "")
                    return (Request.Form["hdn_ddlDays"].ToString());
                else
                    return "";
            }
        }

        protected override void OnInit(EventArgs e)
        {
            rptNews = (Repeater)rpgNews.FindControl("rptNews");
            InitializeComponent();
        }

        void InitializeComponent()
        {
            base.Load += new EventHandler(this.Page_Load);
            FilterData.ServerClick += new EventHandler(FilterData_ServerClick);
            PopularNews.ServerClick +=new EventHandler(PopularNews_ServerClick);
            LatestNews.ServerClick += new EventHandler(LatestNews_ServerClick);
            viewAll.ServerClick += new EventHandler(ViewAll_ServerClick);
        }

        /// <summary>
        /// Page Load Method.
        /// </summary>
        /// <param name="Sender"></param>
        /// <param name="e"></param>
        void Page_Load(object Sender, EventArgs e)
        {
            CommonOpn op = new CommonOpn();
           // PanelSelectionCook = "1";
            BindImageGalleryData();
            
            if (!IsPostBack)
            {
                if (MakeCook != "-1")
                    FillFilters();
                else
                    FillFiltersDefault();
                if (Request["pn"] != null && Request.QueryString["pn"] != "")
                {
                    if (CommonOpn.CheckId(Request.QueryString["pn"]) == true)
                        pageNumber = Request.QueryString["pn"];
                    Trace.Warn("Page Number : "+ pageNumber);
                    if (Convert.ToInt32(pageNumber) > 0)
                    {
                        Trace.Warn("Panel Selection = " + PanelSelectionCook + "page Numebr = " + pageNumber);
                        if (PanelSelectionCook == "1")
                        {
                            LatestNewsPanel();
                        }
                        else if (PanelSelectionCook == "2")
                        {
                            PopularNewsPanel();
                        }
                    }

                }
                if(Convert.ToInt32(Request["pn"]) < 1 )
                {
                    ddlMake.SelectedValue = "-1";
                    ddlModel.SelectedValue = "-1";
                    FillNews(1);
                }
                //FillFilters();
                if (string.IsNullOrEmpty(tempMake))
                    FillNews(1);
            }
        }

        /// <summary>
        /// Makes a panel selection active or inactive.
        /// </summary>
        void LatestNewsPanel()
        {

            PopularNews.Attributes["Class"] = "selection";
            LatestNews.Attributes["Class"] = "active selection";
            PopularNewsSpan.Attributes["Class"] = "ae-sprite";
            LatestNewsSpan.Attributes["Class"] = "ae-sprite tail-bottom";


        }

        /// <summary>
        /// Latest News Panel Server Click.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void LatestNews_ServerClick(object sender, EventArgs e)
        {
            PanelSelectionCook = "1";
            LatestNewsPanel();
            FillFiltersDefault();
            FillNews(1);
            ResetFilters();
        }

        /// <summary>
        /// Makes the Popular News panel active and the rest inactive.
        /// </summary>
        void PopularNewsPanel()
        {
            LatestNews.Attributes["Class"] = "selection";
            PopularNews.Attributes["Class"] = "active selection";
            LatestNewsSpan.Attributes["Class"] = "ae-sprite";
            PopularNewsSpan.Attributes["Class"] = "ae-sprite tail-bottom";

        }

        /// <summary>
        /// Popular News Panel Server Click.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void PopularNews_ServerClick(object sender, EventArgs e)
        {
            PanelSelectionCook = "2";
            PopularNewsPanel();
            FillFiltersDefault();
            FillNews(2);
            ResetFilters();
        }

        /// <summary>
        /// Filters out the data depending on the filters selected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void FilterData_ServerClick(object sender, EventArgs e)
        {
            ContentFilters filterData = new ContentFilters();
            if (!string.IsNullOrEmpty(MakeDetail)) filterData.MakeId = Convert.ToInt32(MakeDetail);
            if (!string.IsNullOrEmpty(ModelDetail))
            {
                filterData.ModelId = Convert.ToInt32(ModelDetail);
                ddlModel.SelectedValue = ModelDetail;
            }
            if(EventDate == "1" ) filterData.ByDate = Convert.ToDateTime("2014-2-7");
            if(EventDate == "2" ) filterData.ByDate = Convert.ToDateTime("2014-2-8");
            if(EventDate == "3" ) filterData.ByDate = Convert.ToDateTime("2014-2-9");
            if(EventDate == "4" ) filterData.ByDate = Convert.ToDateTime("2014-2-10");
            if(EventDate == "5" ) filterData.ByDate = Convert.ToDateTime("2014-2-11");
            FillFilteredNews(1, filterData);

        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 23rd Jan 2014
        /// Summary : to get all news
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ViewAll_ServerClick(object sender, EventArgs e)
        {
            if (PanelSelectionCook == "1")
            {
                LatestNewsPanel();  
            }
            else if (PanelSelectionCook == "2")
            {
                PopularNewsPanel();
            }
            FillFiltersDefault();
            FillNews(Convert.ToInt32(PanelSelectionCook));
            ResetFilters();
        }

        /// <summary>
        /// Finds Substring for youtube thumbnail
        /// </summary>
        /// <param name="str"></param>
        /// <param name="strFrom"></param>
        /// <param name="strTo"></param>
        /// <returns></returns>
        protected string FindSubString(string str, string strFrom, string strTo)
        {
            int pFrom = str.IndexOf(strFrom) + strFrom.Length; ;
            int pTo = str.IndexOf(strTo);
            return str.Substring(pFrom, pTo - pFrom);
        }

        /// <summary>
        /// Binds the Image galley.
        /// </summary>
        void BindImageGalleryData()
        {
            DataSet ds = new DataSet();
            Database db = new Database();
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "cw.AutoExpo_ImageGallery_V2";
                ds = db.SelectAdaptQry(cmd);


                if (ds.Tables[1].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {

                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            vidUrl[i] = ds.Tables[0].Rows[i]["videoUrl"].ToString();
                            vidIdArr[i] = ds.Tables[0].Rows[i]["videoId"].ToString();//FindSubString(ds.Tables[0].Rows[i]["videoUrl"].ToString(), "/embed/", "?");
                            vidArr[i] = "http://www.youtube.com/embed/" + vidIdArr[i] + "?rel=0&amp;wmode=transparent";

                        }
                    }
                    //show all images
                    for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                    {
                        Trace.Warn(" http://" + ds.Tables[1].Rows[i]["hosturl"].ToString() + ds.Tables[1].Rows[i]["imagepathlarge"].ToString());
                        imgArr[i] = "http://" + ds.Tables[1].Rows[i]["hosturl"].ToString() + ds.Tables[1].Rows[i]["imagepathlarge"].ToString();

                    }


                    if (!string.IsNullOrEmpty(vidUrl[0]))
                    {
                        Trace.Warn("a");
                        video1.Visible = true;
                        image1.Visible = false;
                    }
                    else
                    {
                        Trace.Warn("b");
                        video1.Visible = false;
                        image1.Visible = true;
                    }


                    if (!string.IsNullOrEmpty(vidUrl[1]))
                    {
                        video2.Visible = true;
                        image2.Visible = false;
                    }
                    else
                    {
                        video2.Visible = false;
                        image2.Visible = true;
                    }

                }
                else
                {
                    //hide div
                    divGallery.Visible = false;

                }


            }
            catch (SqlException ex)
            {
                Trace.Warn(ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                Trace.Warn(ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

        void FillFiltersDefault()
        {
            PopulateFilters filterData = new PopulateFilters();
            DataSet ds = filterData.GetMakeFilterData();
            ddlMake.DataSource = ds.Tables[0];
            ddlMake.DataTextField = "Name";
            ddlMake.DataValueField = "ID";
            ddlMake.DataBind();
            ddlMake.Items.Insert(0, new ListItem("Make", "-1"));
        }

        /// <summary>
        /// The method populates the filters on the landing page.
        /// </summary>
        private void FillFilters()
        {
            PopulateFilters filterData = new PopulateFilters();
            DataSet ds = filterData.GetMakeFilterData();
            ddlMake.DataSource = ds.Tables[0];
            ddlMake.DataTextField = "Name";
            ddlMake.DataValueField = "ID";
            ddlMake.DataBind();
            ddlMake.Items.Insert(0, new ListItem("Make", "-1"));
            Trace.Warn("modeldetail = " + ModelDetail);

            if (!string.IsNullOrEmpty(MakeDetail))
            {
                tempMake = MakeDetail;
                tempModel = ModelDetail;
                MakeCook = MakeDetail;
                ModelCook = ModelDetail;
            }
            else
            {
                tempModel = ModelCook;
                tempMake = MakeCook;
            }
            if (tempMake != "" )
            {

                ddlMake.SelectedValue = tempMake;
                FillModels(Convert.ToInt32(tempMake));
                ddlModel.SelectedValue = tempModel;
                ContentFilters filtersData = new ContentFilters();
                Trace.Warn("hidden 1 = " + tempMake + "hidden 2 = " + tempModel);
                filtersData.MakeId = Convert.ToInt32(tempMake);
                filtersData.ModelId = Convert.ToInt32(tempModel);

                FillFilteredNews(1,filtersData);
            }
            
        }

        void FillModels(int makeId)
        {
            PopulateFilters filterData = new PopulateFilters();
            DataSet ds = filterData.GetModelFilterData(makeId);
            DataTable dt = ds.Tables[0];
            ddlModel.DataSource = dt;
            ddlModel.DataTextField = "Name";
            ddlModel.DataValueField = "ID";
            ddlModel.DataBind();
            ddlModel.Items.Insert(0, new ListItem("Model", "-1"));
        }

        /// <summary>
        /// The method populates news.
        /// sortby = 1 : Date
        /// sortby = 2 : Views
        /// </summary>
        private void FillNews(int sortBy)
        {
            if (pageNumber != string.Empty)
                rpgNews.CurrentPageIndex = Convert.ToInt32(pageNumber);

            //form the base url. 
            string qs = Request.ServerVariables["QUERY_STRING"];
            Trace.Warn(qs);
            BaseUrl = "/autoexpo/2014/";
            rpgNews.BaseUrl = BaseUrl;

            rpgNews.PageSize = 10;
            if (sortBy == 1)
            {

                rpgNews.InitializeGrid(1);
            }
            else if (sortBy == 2)
            {
                rpgNews.InitializeGrid(2);
            }
            NewsTitle = "Auto Expo Updates";
            NewsBreadCrumb = "<li class=\"current\">&rsaquo; <strong>Bike News</strong></li>";
           
            Trace.Warn("ramwur = " + BaseUrl + "2" + Request.Url + "3" + Request.UrlReferrer);
        }

        /// <summary>
        /// this method  gets the filtered news data.
        /// </summary>
        /// <param name="sortBy"></param>
        /// <param name="filters"></param>
        void FillFilteredNews(int sortBy, ContentFilters filters)
        {
                            
            if (string.IsNullOrEmpty(pageNumber))
                rpgNews.CurrentPageIndex = 1;
            else
                rpgNews.CurrentPageIndex = Convert.ToInt32(pageNumber);
            //form the base url. 
            string qs = Request.ServerVariables["QUERY_STRING"];
            Trace.Warn(qs);
            BaseUrl = "/autoexpo/2014/";
            rpgNews.BaseUrl = BaseUrl;

            rpgNews.PageSize = 10;
            rpgNews.InitializeGrid(filters);
            NewsTitle = "Auto Expo Updates";
            NewsBreadCrumb = "<li class=\"current\">&rsaquo; <strong>Bike News</strong></li>";
            
            Trace.Warn("ramwur = " + BaseUrl + "2" + Request.Url + "3" + Request.UrlReferrer);
        }
        
        /// <summary>
        /// Created By : Sadhana Upadhyay on 24th Jan 2014
        /// Summary : To reset filter Fields
        /// </summary>
        private void ResetFilters()
        {
            ddlModel.SelectedValue = "-1";
            ddlMake.SelectedValue = "-1";
            ddlDays.SelectedValue = "-1";
            hdn_ddlMake.Value = "-1";
            hdn_ddlModel.Value = "-1";            
        }
    } 
} 