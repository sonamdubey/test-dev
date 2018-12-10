/*******************************************************************************************************
IN THIS CLASS THE NEW MEMBEERS WHO HAVE REQUESTED FOR REGISTRATION ARE SHOWN
*******************************************************************************************************/
using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Carwale.UI.Common;
using Carwale.UI.Controls;
using Carwale.Notifications;
using Carwale.DAL.Forums;
using Carwale.BL.Forums;
using Carwale.Cache.Forums;
using AEPLCore.Cache;


namespace Carwale.UI.Forums
{
    public class ViewForums : Page
    {
        #region Global Variables
        protected HtmlGenericControl divForum, divStrip, divStripTop;
        protected Label lblMessage;
        protected Repeater rptForums, rptStickyForums;
        private int maxNoLinks = 15;	//number of links in the strip
        private int pageCount = 15;	//number of threads on each page
        protected int pageNo = 1;	//default page number is 1
        private int total = 0;
        public string forumId = "", inboxTotal = "";
        public string forumurl = "-1";
        public string lastPageNo = string.Empty, nextUrl = string.Empty, prevUrl = string.Empty;
        DataSet dsViews = new DataSet();
        #endregion

        #region Properties

        public string ForumName
        {
            get
            {
                if (ViewState["ForumName"] != null)
                    return ViewState["ForumName"].ToString();
                else
                    return "";
            }
            set { ViewState["ForumName"] = value; }
        }

        public string ForumDescription
        {
            get
            {
                if (ViewState["ForumDescription"] != null)
                    return ViewState["ForumDescription"].ToString();
                else
                    return "";
            }
            set { ViewState["ForumDescription"] = value; }
        }
        #endregion

        #region On Init

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        #endregion

        #region Initialize Component

        void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
        }
        #endregion

        #region PageLoad

        void Page_Load(object Sender, EventArgs e)
        {
            UserBusinessLogic userDetails = new UserBusinessLogic();
            string loginCookie = HttpContext.Current.Request.Cookies["ForumLastLogin"] == null ? null : HttpContext.Current.Request.Cookies["ForumLastLogin"].ToString();
            userDetails.ManageLastLogin(CurrentUser.Id, loginCookie);
            if (Request["forum"] != null && Request.QueryString["forum"] != "")//also get the forumId
            {            
                forumId = Request.QueryString["forum"];
                if (CommonOpn.CheckId(forumId) == false)//verify the id as passed in the url
                {
                    RedirectToDefault();//redirect to the default page
                    return;
                }
            }
            else
            {
                RedirectToDefault();//redirect to the default page
                return;
            }   
            if (Convert.ToInt32(forumId) < 1)
            {
                RedirectToDefault();
            }
            else
            {
                /*	Code added by Ashish Ambokar ends here */
                if (Request["page"] != null && CommonOpn.CheckId(Request["page"]))
                {
                    pageNo = Convert.ToInt32(Request["page"]);
                }
                if (!IsPostBack)
                {
                    Trace.Warn("reached before repeaters");
                    ForumName = FillRepeaters();
                    Trace.Warn("reached after repeaters");
                    DeviceDetection dd = new DeviceDetection("/forums/" + forumurl + "/");
                    dd.DetectDevice();
                    PrevNextUrl();
                    if (ForumName == "")
                    {
                        RedirectToDefault();
                    }
                    SaveUserActivity saveAct = new SaveUserActivity();
                    saveAct.saveActivity(CurrentUser.Id, "2", forumId, "-1", CurrentUser.CWC);
                }
            }
        } // Page_Load

        #endregion

        #region Get Last Post


        public string GetLastPost(string title, string name, string date, string id, string posts, string startedById, string url)
        {
            PostsBusinessLogic postData = new PostsBusinessLogic();
            return (postData.GetLastPost(title, name, date, id, posts, startedById, url));
        }
        #endregion

        #region Get Last Post Thread

        public string GetLastPostThread(string name, string date, string lastPostById)
        {
            PostDAL postData = new PostDAL();
            return (postData.GetLastPostThread(name, date, lastPostById));
        }
        #endregion

        #region Fill Repeaters
        /// <summary>
        /// FillRepeaters() retrieves all the subthreads of the Forum threads 
        /// Modified By: Prashant Vishe 
        /// On: 9 Sept 2012
        /// </summary
        /// <returns>name of the forum </returns>
        public string FillRepeaters()
        {
            string forum = "";
            int startIndex = -1, endIndex = -1;
            if (Convert.ToInt32(forumId) < 1)
            {
                RedirectToDefault();
            }
            else
            {
                if (pageNo < 1)
                {
                    pageNo = 1;
                }
                if (pageNo == 1)
                {
                    startIndex = 1;
                    endIndex = pageCount;
                }
                else
                {
                    startIndex = (((pageNo - 1) * pageCount) + 1);
                    endIndex = (((pageNo - 1) * pageCount) + pageCount);
                }
                var forumCache = new ForumsCache();
                using (DataSet ds = forumCache.GetForumDetails(Convert.ToInt32(forumId), startIndex, endIndex, pageNo))
                {      
                        if (ds != null && ds.Tables[0].Rows.Count > 0)
                        {
                            total = Convert.ToInt32(ds.Tables[2].Rows[0]["total"]);
                            rptForums.DataSource = ds.Tables[0];
                            rptForums.DataBind();
                        }

                        if (ds != null && ds.Tables[1].Rows.Count > 0)
                        {
                            forum = (ds.Tables[1].Rows[0]["Name"]).ToString();
                            ForumDescription = (ds.Tables[1].Rows[0]["Description"]).ToString();
                            forumurl = (ds.Tables[1].Rows[0]["Url"]).ToString();
                        }                    
                }
            }

            if (rptForums.Items.Count == 0)
            {
                divForum.Visible = false;
                lblMessage.Text = "<p>No threads in this forum. <a href='/forums/CreateNewThread.aspx?forum=" + forumId + "'>Be the first to create one.</a></p>";
            }
            else
            {
                divForum.Visible = true;
                AddStrip();// add pages strip.

            }

            return forum;
        }

        #endregion

        #region Add Strip

        /// <summary>
        ///AddStrip() makes the strip, based on the total posts, the current page, and the page count
        /// </summary>
        void AddStrip()
        {
            //make the strip, based on the total posts, the current page, and the page count
            pageCount = pageCount <= 0 ? 1 : pageCount;
            int pages = (int)Math.Ceiling(Convert.ToDouble(total) / Convert.ToDouble(pageCount));
            if (Convert.ToInt32(forumId) < 1)
            {
                RedirectToDefault();
            }
            else
            {
                string pageUrl = "/forums/" + forumurl;
                if (pages > 1)
                {
                    divStrip.Visible = true;
                    divStripTop.Visible = true;
                    //get the slot number
                    int slotNo = (int)Math.Ceiling(Convert.ToDouble(pageNo) / Convert.ToDouble(maxNoLinks));
                    int startIndex = (slotNo - 1) * maxNoLinks + 1;
                    int endIndex = (slotNo - 1) * maxNoLinks + maxNoLinks;
                    endIndex = endIndex < pages ? endIndex : pages;
                    lastPageNo = endIndex.ToString();
                    divStrip.InnerHtml = "Pages : ";
                    if (startIndex > maxNoLinks)
                        divStrip.InnerHtml += "<span class='ac'><a href='" + pageUrl + "-p"
                                            + (startIndex - 1).ToString() + "/'>«</a></span>";
                    for (int i = startIndex; i <= endIndex; i++)
                    {
                        if (i != pageNo)
                            divStrip.InnerHtml += "<span class='ac'><a href='" + pageUrl + "-p" + i.ToString() + "/'>"
                                                + i.ToString() + "</a></span>";
                        else
                            divStrip.InnerHtml += "<span class='iac'>" + i.ToString() + "</span>";
                    }
                    if (endIndex < pages)
                        divStrip.InnerHtml += "<span class='ac'><a href='" + pageUrl + "-p"
                                            + (endIndex + 1).ToString() + "/'>»</a></span>";
                    // now make an exact replica of footer strip.						
                    divStripTop.InnerHtml = divStrip.InnerHtml;
                }
            }
        }
        #endregion

        #region Prev Next URL

        /// <summary>
        /// Written By:Prashant Vishe On 21 June 2013
        /// Function is used to Create Next and Prev url 
        /// </summary>
        void PrevNextUrl()
        {
            string prevPageNumber = string.Empty, nextPageNumber = string.Empty;
            string mainUrl = "https://www.carwale.com/forums/";

            if (!String.IsNullOrEmpty(lastPageNo))
            {
                if ((String.IsNullOrEmpty(pageNo.ToString()) || pageNo == 1)) //first page
                {
                    nextPageNumber = "2";
                    nextUrl = mainUrl + forumurl + "-p" + nextPageNumber + "/";
                }
                else if ((pageNo == int.Parse(lastPageNo)) && (!String.IsNullOrEmpty(pageNo.ToString())))  //last page
                {
                    prevPageNumber = (pageNo - 1).ToString();
                    prevUrl = mainUrl + forumurl + "-p" + prevPageNumber + "/";
                }
                else
                { //middle pages
                    if (!String.IsNullOrEmpty(pageNo.ToString()))
                    {
                        prevPageNumber = (pageNo - 1).ToString();
                        prevUrl = mainUrl + forumurl + "-p" + prevPageNumber + "/";
                        nextPageNumber = (pageNo + 1).ToString();
                        nextUrl = mainUrl + forumurl + "-p" + nextPageNumber + "/";
                    }
                }
            }

        }
        #endregion

        #region Redirect To Default Page

        private void RedirectToDefault()
        {
            Response.Redirect("/forums/default.aspx");
        }
        #endregion

    } // class
} // namespace