using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Carwale.UI.Common;
using Carwale.UI.Controls;
using Carwale.Notifications;
using CarwaleAjax;
using Carwale.BL.Forums;
using Carwale.DAL.Forums;
using Carwale.Entity;
using Carwale.Cache.Forums;
using RabbitMqPublishing;
using System.Collections.Specialized;
using MySql.Data.MySqlClient;
using Carwale.Utility;
using AEPLCore.Queue;
using System.Configuration;
using Google.Protobuf;

namespace Carwale.UI.Forums
{
    public class ViewThread : Page
    {
        #region Global Variables
        protected HtmlGenericControl divThread, divStrip, divStripTop, divNoReplyMessage, divReplyLink, divQuickReply;
        protected Label lblMessage, lblBrk, lblCaptcha;
        protected Repeater rptThreads;
        protected LinkButton lnkDeletePost, lnkDeleteThread, lnkCloseThread, lnkMergePost;
        protected Panel pnlModeratorTools;
        protected TextBox txtCaptcha;
        protected RichTextEditor rteQR;
        protected HtmlTableRow trCaptcha;
        protected Button butSave;
        protected CheckBox chkEmailAlert;
        protected HyperLink hyplnk, hyplnkRemoveSticky, hyplnkCreateSticky;
        protected string postedByUserIds = "", concatenatedPostIds = "";
        public string threadId = "", customerId = "", inboxTotal = "";
        public int serial = 0;
        public bool modLogin = false, flagLogin = false;
        private int IsModerated = 1;
        private bool CheckCount = false;// This is used to check if the count for a thread is 0 and the user is accordingly redirected to forums default page.
        private int maxNoLinks = 25;	// number of links in the strip
        private int postsPerPage = 10;	// number of posts on each page
        protected int pageNo = 1;		// default page number is 1
        protected string lastPageNo = string.Empty, nextUrl = string.Empty, prevUrl = string.Empty;
        protected int totalNoOfPages;
        private int total = 0;
        ForumsCache forumsCache = null;
        #endregion

        #region Properties
        public string ThreadUrl
        {
            get
            {
                if (ViewState["ThreadUrl"] != null)
                    return ViewState["ThreadUrl"].ToString();
                else
                    return "";
            }
            set
            {
                ViewState["ThreadUrl"] = value;
            }
        }

        public string ForumUrl
        {
            get
            {
                if (ViewState["ForumUrl"] != null)
                    return ViewState["ForumUrl"].ToString();
                else
                    return "";
            }
            set { ViewState["ForumUrl"] = value; }
        }



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

        public string ForumId
        {
            get
            {
                if (ViewState["ForumId"] != null)
                    return ViewState["ForumId"].ToString();
                else
                    return "";
            }
            set { ViewState["ForumId"] = value; }
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

        public string ThreadName
        {
            get
            {
                if (ViewState["ThreadName"] != null)
                    return ViewState["ThreadName"].ToString();
                else
                    return "";
            }
            set { ViewState["ThreadName"] = value; }
        }

        public string StartedBy
        {
            get
            {
                if (ViewState["StartedBy"] != null)
                    return ViewState["StartedBy"].ToString();
                else
                    return "";
            }
            set { ViewState["StartedBy"] = value; }
        }

        public string StartedOn
        {
            get
            {
                if (ViewState["StartedOn"] != null && ViewState["StartedOn"].ToString() != "")
                    return Convert.ToDateTime(ViewState["StartedOn"]).ToString("dd-MMM,yyyy HH:mm");
                else
                    return "";
            }
            set { ViewState["StartedOn"] = value; }
        }
        #endregion

        #region OnInit

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }
        #endregion

        #region Inititalize Component
        void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
            lnkDeletePost.Click += new EventHandler(DeletePost);
            lnkDeleteThread.Click += new EventHandler(DeleteThread);
            lnkCloseThread.Click += new EventHandler(CloseThread);
            lnkMergePost.Click += new EventHandler(MergePost);
            butSave.Click += new EventHandler(butSave_Click);
        }
        #endregion

        #region Page Load

        void Page_Load(object Sender, EventArgs e)
        {
            /*
               Code created By : Supriya Khartode
               Created Date : 11/12/2013
               Note : This is the code used for device detection to integrate mobile website with desktop website
               */

            DeviceDetection dd = new DeviceDetection(Request.ServerVariables["HTTP_X_REWRITE_URL"].ToString());
            dd.DetectDevice();
            ForumsSecurityChecks forumsecuritycheck = new ForumsSecurityChecks();
            ForumsCommon.ManageLastLogin();
            forumsCache = new ForumsCache();
            UserDAL userDetails = new UserDAL();
            UserBusinessLogic userBL = new UserBusinessLogic();
            string loginCookie = HttpContext.Current.Request.Cookies["ForumLastLogin"] == null ? null : HttpContext.Current.Request.Cookies["ForumLastLogin"].ToString();
            userBL.ManageLastLogin(CurrentUser.Id, loginCookie);
            customerId = CurrentUser.Id;
            if (customerId != "-1") // show quick reply box if user is logged in.
                divQuickReply.Visible = true;
            if (customerId != "-1")
                flagLogin = true;
            else
                flagLogin = false;
            modLogin = GetModeratorLoginStatus(customerId);//check if moderator is logged-in?
            if (modLogin == true)
                pnlModeratorTools.Visible = true;
            if (Request["thread"] != null && Request.QueryString["thread"] != "")//also get the threadId
            {
                threadId = Request.QueryString["thread"];
                if (CommonOpn.CheckId(threadId) == false)//verify the id as passed in the url
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
            if (Request["page"] != null && CommonOpn.CheckId(Request["page"]))
            {
                pageNo = Convert.ToInt32(Request["page"]);
            }
            if (Request["post"] != null && CommonOpn.CheckId(Request["post"]))
            {
                int post = Convert.ToInt32(Request["post"]);
                FindPost(post);// find out post no and redirect user accordingly.
            }
            Trace.Warn("reached 1---check handle ");
            if (userDetails.CheckUserHandle(CurrentUser.Id) != false)//Check the handle. if it does not exist then redirect the user to change the handle page
            {
                Trace.Warn("reached 2---check handle ");
                divQuickReply.Visible = false;
            }
            Trace.Warn("reached 2---check handle ");
            Ajax.Utility.RegisterTypeForAjax(typeof(AjaxForum));
            if (!IsPostBack)
            {
                if (!GetForum())
                {
                    RedirectToDefault();
                }
                if (customerId != "")
                {
                    bool Stickyflag = chkStickyThreads(threadId, customerId);
                    if (Stickyflag == true)
                    {
                        hyplnkRemoveSticky.Visible = true;
                        hyplnkRemoveSticky.Text = "Remove Sticky";
                        hyplnkCreateSticky.Visible = false;
                    }
                    else
                    {
                        hyplnkCreateSticky.Visible = true;
                        hyplnkCreateSticky.Text = "Create Sticky";
                        hyplnkRemoveSticky.Visible = false;
                    }
                    AjaxForum aj = new AjaxForum();
                    bool flag = aj.GetSubscribeLink(threadId, customerId);
                    if (flag == true)
                    {
                        hyplnk.Visible = false;
                        lblBrk.Visible = false;
                    }
                    else
                    {
                        hyplnk.Visible = true;
                        hyplnk.Text = "<B>Subscribe to this thread</B>";
                        lblBrk.Visible = true;
                    }
                }
                else
                {
                    hyplnk.Visible = false;
                    lblBrk.Visible = false;
                }
                FillRepeaters();
                PrevNextUrl();
                SaveUserActivity saveAct = new SaveUserActivity();
                saveAct.saveActivity(CurrentUser.Id, "4", ForumId, threadId, CurrentUser.CWC);
            }
            if (Request.Cookies["Forum_MultiQuotes"] == null)
            {
                HttpCookie objCookie;
                objCookie = new HttpCookie("Forum_MultiQuotes");
                objCookie.Value = "";
                objCookie.HttpOnly = false;
                HttpContext.Current.Response.Cookies.Add(objCookie);
            }
            bool checkforum = forumsecuritycheck.Captchacheck(customerId);
            if (!checkforum)
            {
                trCaptcha.Visible = true;
            }
            else
            {
                trCaptcha.Visible = false;
            }
        } // Page_Load
        #endregion

        #region Check for Sticky
        protected bool chkStickyThreads(string threadId, string customerId)
        {
            ThreadsDAL threadDetails = new ThreadsDAL();
            return threadDetails.chkStickyThreads(threadId, customerId);
        }
        #endregion

        #region Delete Post
        /// <summary>
        /// Renders a post inactive
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void DeletePost(object sender, EventArgs e)
        {
            if (Request.Form["__EVENTARGUMENT"] != null && Request.Form["__EVENTARGUMENT"].ToString() != "")
            {
                PostDAL postDetails = new PostDAL();
                bool postDeleteResponse = postDetails.DeletePost(Request.Form["__EVENTARGUMENT"].ToString(), threadId, CurrentUser.Id);
                forumsCache.InvalidatePaginationCache(string.Format("Forums-Thread-{0}-Page", threadId), pageNo);
                FillRepeaters();
            }
        }
        #endregion

        #region Merge Post
        void MergePost(object sender, EventArgs e)
        {
            try
            {
                Label lblId;
                string strMergeId, strMessage, segments = "";
                for (int i = 0; i < rptThreads.Items.Count; i++)
                {
                    HtmlInputCheckBox chkIds = (HtmlInputCheckBox)rptThreads.Items[i].FindControl("chkItem");
                    lblId = (Label)rptThreads.Items[i].FindControl("lblId");
                    if (chkIds.Checked)
                    {
                        segments += lblId.Text + ","; //All the messages r appended
                    }
                }

                strMergeId = segments.Split(',')[0].Trim();
                segments = segments.Substring(0, segments.LastIndexOf(',')).ToString();
                if (CheckCustomerId(segments) == true)
                {
                    strMessage = AppendMessagesForMerge(segments);
                    PostDAL postDetails = new PostDAL();
                    postDetails.MergePost(strMessage.Trim(), strMergeId);
                    postDetails.MergePost(segments);
                    UpdateStats(threadId, ForumId);
                    FillRepeaters();
                }
                else
                {
                    lblMessage.Text = "Cannot Merge Posts of different Customers";
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            } // catch Exception	
        }
        #endregion

        #region Check Customer Id
        /// <summary>
        /// Check if selected posts are from same customerid
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        protected bool CheckCustomerId(string ids)
        {
            Trace.Warn(ids);
            UserDAL userdetails = new UserDAL();
            return userdetails.CheckCustomerIds(ids);
        }
        #endregion

        #region Message Append For Merge
        /// <summary>
        /// Appends message for merging.
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        protected string AppendMessagesForMerge(string ids)
        {
            PostDAL postDetails = new PostDAL();
            return postDetails.AppendMessagesForMerge(ids);
        }
        #endregion

        #region Update Forum Stats.
        /// <summary>
        /// Update Forum stats
        /// </summary>
        /// <param name="ForumId"></param>
        /// <param name="SubCategoryId"></param>
        /// <returns></returns>
        protected bool UpdateStats(string ForumId, string SubCategoryId)
        {
            ThreadsDAL threadDetails = new ThreadsDAL();
            return (threadDetails.UpdateStats(ForumId, SubCategoryId));
        }
        #endregion

        #region Delete Thread
        /// <summary>
        /// Deletes a thread, essentially making it inactive.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void DeleteThread(object sender, EventArgs e)
        {
            ThreadsBL threadDetails = new ThreadsBL();
            bool deletesuccess = threadDetails.DeleteThread(threadId, CurrentUser.Id);
            forumsCache.InvalidatePaginationCache(string.Format("Forums-Id-{0}-Page", ForumId), 1);
            RedirectToDefault();
        }

        #endregion

        #region Close Thread
        /// <summary>
        /// This closes a thread from any further replies.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CloseThread(object sender, EventArgs e)
        {
            ThreadsDAL threadDetails = new ThreadsDAL();
            bool closesuccess = threadDetails.CloseThread(threadId, CurrentUser.Id);
            GetForum();
        }
        #endregion

        #region Get Forum
        /// <summary>
        /// Gets all the details for a forum.
        /// </summary>
        /// <returns></returns>

        public bool GetForum()
        {
            bool retVal = true;
            bool replyStatus = true;
            
            try
            {
                ThreadBasicInfo ThreadInfo = forumsCache.GetAllForums(threadId);

                PublishManager rmq = new PublishManager();
                var payload = new NewCarConsumers.ThreadId
                {
                    Id = Convert.ToInt32(threadId)
                };

                var message = new QueueMessage
                {
                    ModuleName = (ConfigurationManager.AppSettings["NewCarConsumerModuleName"] ?? string.Empty),
                    FunctionName = "ForumThreadViewCount",
                    Payload = payload.ToByteString(),
                    InputParameterName = "ThreadId",
                };
                rmq.PublishMessageToQueue((ConfigurationManager.AppSettings["ThreadViewCountQueue"] ?? "FORUMTHREADVIEWCOUNTMYSQL").ToUpper(), message);

                ForumId = ThreadInfo.ForumId;
                ForumName = ThreadInfo.ForumName;
                ForumDescription = ThreadInfo.ForumDescription;
                ThreadName = ThreadInfo.ThreadName;
                StartedOn = ThreadInfo.StartedOn;
                replyStatus = ThreadInfo.replyStatus;
                ThreadUrl = ThreadInfo.ThreadUrl;
                ForumUrl = ThreadInfo.ForumUrl;
                if (!replyStatus)
                {
                    divNoReplyMessage.InnerHtml = "<b>This discussion is closed for new replies.</b>";
                    divReplyLink.Visible = false;
                    if (!modLogin) divQuickReply.Visible = false; // hide quick reply box as well.
                }
            }
            catch (MySqlException err)
            {
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"] + " Error from query 3");
                objErr.SendMail();
                retVal = false;
            } // catch Exception
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
                retVal = false;
            }

            return retVal;
        }

        #endregion

        #region Fill Page Repeaters

        void FillRepeaters()
        {
            DataSet ds = new DataSet();
            try
            {
                int startIndex = -1, endIndex = -1;
                PrepareStartEndIndex(out startIndex, out endIndex);
                ds = forumsCache.GetThreadDetails(Convert.ToInt32(threadId), startIndex, endIndex,pageNo);
                rptThreads.DataSource = ds.Tables[0];
                if (ds.Tables[0].Rows.Count > 0)
                {
                    rptThreads.DataSource = ds.Tables[0];
                    rptThreads.DataBind();
                    total = Convert.ToInt32(ds.Tables[1].Rows[0][0]);
                }
                if (rptThreads.Items.Count == 0)
                {
                    CheckCount = true;
                    divThread.Visible = false;
                }
                else
                {
                    divThread.Visible = true;
                    AddStrip();//add pages strip.
                }
            }

            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"] + " Erro from query 5");
                objErr.SendMail();
            } // catch Exception

            if (CheckCount)
            {
                Response.Redirect("/forums/" + ForumUrl + "/", false);
                Response.StatusCode = 301;
                Response.End();
            }

        }

        #endregion

        #region Prepare Start and End Index to populate threads
        /// <summary>
        /// This function is called to prepare the start and end index for fetching the news.Serial keeps track of the post numbers. 
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        void PrepareStartEndIndex(out int startIndex, out int endIndex)
        {
            if (pageNo <= 0)
            {
                pageNo = 1;
                startIndex = 1;
                endIndex = 10;
                serial = 0;
            }
            if (pageNo == 1)
            {
                startIndex = 1;
                endIndex = 10;
                serial = 0;
            }
            else
            {
                startIndex = (pageNo - 1) * postsPerPage + 1;
                endIndex = startIndex + 9;
                serial = (pageNo - 1) * postsPerPage;
            }
        }


        #endregion

        #region Get The Thread Title

        public string GetTitle(string value)
        {
            string[] words = value.Split(' ');
            string retVal = "";
            for (int i = 0; i < words.Length; i++)
            {
                if (i == 0)
                {
                    retVal = "<span>" + words[i] + "</span>";
                }
                else
                    retVal += " " + words[i];
            }
            return retVal;
        }

        #endregion

        #region Get message in Quote.

        public string GetMessage(string value)
        {
            string post = value;
            string quoteStart = "<div class='quote'>Posted by <b>";
            string quotePostedByEnd = "</b><br>";
            string quoteEnd = "</div>";
            // Identify and replace quotes
            if (post.ToLower().IndexOf("[^^quote=") >= 0)
            {
                post = post.Replace("[^^quote=", quoteStart);
                post = post.Replace("[^^/quote^^]", quoteEnd);
                post = post.Replace("^^]", quotePostedByEnd);
            }
            if (Request["h"] != null && Request["h"] != "")
            {
                post = post.ToLower().Replace(Request["h"], "<span style='background:#FFFF00;color:#000000;'>" + Request["h"] + "</span>");
            }
            //add smileys
            post = post.Replace(":)", "<img class='inobg' src='" + ImagingFunctions.GetRootImagePath() + "/images/forums/smileys/smile.gif' border='0' align='absmiddle' />");
            post = post.Replace(":(", "<img class='inobg' src='" + ImagingFunctions.GetRootImagePath() + "/images/forums/smileys/sad.gif' border='0' align='absmiddle' />");
            post = post.Replace(":D", "<img class='inobg' src='" + ImagingFunctions.GetRootImagePath() + "/images/forums/smileys/biggrin.gif' border='0' align='absmiddle' />");
            post = post.Replace(":-)", "<img class='inobg' src='" + ImagingFunctions.GetRootImagePath() + "/images/forums/smileys/smile.gif' border='0' align='absmiddle' />");
            post = post.Replace(":-(", "<img class='inobg' src='" + ImagingFunctions.GetRootImagePath() + "/images/forums/smileys/sad.gif' border='0' align='absmiddle' />");
            post = post.Replace(":-D", "<img class='inobg' src='" + ImagingFunctions.GetRootImagePath() + "/images/forums/smileys/biggrin.gif' border='0' align='absmiddle' />");
            post = post.Replace("<p>&nbsp;</p>", ""); // remove empty paragraphs.
            return post;
        }

        #endregion

        #region Get Signature

        public string GetSignature(string value)
        {
            if (value != "")
                return "<img src=\"" + ImagingFunctions.GetRootImagePath() + "/images/forums/line.jpg\" align=\"absmiddle\" /><BR><span style=\"color: #777777\"> " + value + "</span>";
            else
                return "";
        }

        #endregion

        #region Get Date Time

        public string GetDateTime(string value)
        {
            if (value != "")
                return Convert.ToDateTime(value).ToString("dd-MMM, yyyy hh:mm tt");
            else
                return "N/A";
        }

        #endregion

        #region Moderator Login Status
        /// <summary>
        /// Gets the moderator login status.
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        bool GetModeratorLoginStatus(string customerId)
        {
            ForumsCache threadDetails = new ForumsCache();
            return (threadDetails.IsModerator(customerId));
        }

        #endregion

        #region Is Post Editable
        /// <summary>
        /// Lets us know if the post is editable.
        /// </summary>
        /// <param name="msgDateTime"></param>
        /// <param name="postedBy"></param>
        /// <returns></returns>
        public bool IsPostEditable(string msgDateTime, string postedBy)
        {
            bool logStatus = false;
            if (CurrentUser.Id == postedBy && Convert.ToDateTime(msgDateTime).AddMinutes(10) >= DateTime.Now)
                logStatus = true;
            else
                logStatus = false;

            return logStatus;
        }
        #endregion

        #region Add Page Strip
        /// <summary>
        /// This adds the paging strip at the bottom of the thread details.
        /// </summary>
        void AddStrip()
        {
            //make the strip, based on the total questions, the current page, and the page count
            postsPerPage = postsPerPage <= 0 ? 1 : postsPerPage;
            int pages = (int)Math.Ceiling(Convert.ToDouble(total) / Convert.ToDouble(postsPerPage));
            string pageUrl = "/forums/" + threadId + "-" + ThreadUrl;
            if (pages > 1)
            {
                divStrip.Visible = true;
                divStripTop.Visible = true;
                int slotNo = (int)Math.Ceiling(Convert.ToDouble(pageNo) / Convert.ToDouble(maxNoLinks));//get the slot number
                int startIndex = (slotNo - 1) * maxNoLinks + 1;
                int endIndex = (slotNo - 1) * maxNoLinks + maxNoLinks;
                endIndex = endIndex < pages ? endIndex : pages;
                lastPageNo = endIndex.ToString();
                totalNoOfPages = pages;
                divStrip.InnerHtml = "Pages : ";
                if (startIndex > maxNoLinks)
                    divStrip.InnerHtml += "<span class='ac'><a href='" + pageUrl + "-p"
                                        + (startIndex - 1).ToString() + ".html'>«</a></span>";
                for (int i = startIndex; i <= endIndex; i++)
                {
                    if (i != pageNo)
                        divStrip.InnerHtml += "<span class='ac'><a href='" + pageUrl + "-p" + i.ToString() + ".html'>"
                                            + i.ToString() + "</a></span>";
                    else
                        divStrip.InnerHtml += "<span class='iac'>" + i.ToString() + "</span>";
                }
                if (endIndex < pages)
                    divStrip.InnerHtml += "<span class='ac'><a href='" + pageUrl + "-p"
                                        + (endIndex + 1).ToString() + ".html'>»</a></span>";
                // now make an exact replica of footer strip.						
                divStripTop.InnerHtml = divStrip.InnerHtml;
            }
        }
        #endregion

        #region Get User Title (Banned or not)
        /// <summary>
        /// Gets the title for a user (whether banned or not.)
        /// </summary>
        /// <param name="role"></param>
        /// <param name="posts"></param>
        /// <param name="bannedCust"></param>
        /// <returns></returns>
        public string GetUserTitle(string role, string posts, string bannedCust)
        {
            string title = "";
            if (bannedCust != "-1")
            {
                title = "[BANNED]";
            }
            else if (role != "")
            {
                title = "<div><div style=\"height: 16px;margin: auto;width: 16px;float:left;\" title='" + role + "' ><img src='" + ImagingFunctions.GetRootImagePath() + "/images/forums/moderatorforum.gif' /></div><div style='float:left;margin-left:5px;width:100px;'>Moderator</div><div style='clear:both;'></div></div>";
            }
            else
            {
                int noOfPosts = int.Parse(posts);

                if (noOfPosts < 26)
                    title = "New Arrival<br>";
                else if (noOfPosts < 51)
                    title = "Driven<br>";
                else if (noOfPosts < 101)
                    title = "Road-tested<br>";
                else if (noOfPosts < 251)
                    title = "Long-termer<br>";
                else if (noOfPosts < 501)
                    title = "Beloved<br>";
                else if (noOfPosts < 1001)
                    title = "Best-seller<br>";
                else title = "Legend<br>";
            }

            return title;
        }
        #endregion

        #region Send Private Message
        /// <summary>
        /// Send a private message to a member
        /// </summary>
        /// <param name="postedById"></param>
        /// <param name="handle"></param>
        /// <returns></returns>
        public string SendPrivateMessage(string postedById, string handle)
        {
            string retLink = "";

            if (customerId != "" && customerId != "-1" && postedById != CurrentUser.Id && postedById != "0")
            {
                retLink = "<a id='#' target='_blank' href='/community/pms/compose.aspx?username=" + handle + "'><img align='absmiddle' title='Send a private message to this member' style='cursor:pointer' border='0' src='" + ImagingFunctions.GetRootImagePath() + "/images/icons/sendmessage.gif'"
                        + " alt='PM' ></img></a>";
            }

            return retLink;
        }
        #endregion

        #region Find Post
        /// <summary>
        /// Function to figure out a post's page number.
        /// </summary>
        /// <param name="post"></param>
        void FindPost(int post)
        {
            int oldPosts = 0;
            try
            {
                oldPosts = forumsCache.FindPost(post, Convert.ToInt32(threadId));
                pageNo = (oldPosts - (oldPosts % postsPerPage)) / postsPerPage + 1;
                forumsCache.InvalidatePaginationCache(string.Format("Forums-Thread-{0}-Page", threadId), pageNo);
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"] + " Error from query 6");
                objErr.SendMail();
            } // catch Exception
            if (pageNo > 1)

                Response.Redirect("viewthread-" + threadId + "-p" + pageNo + ".html#post" + post);
            else
                Response.Redirect("viewthread-" + threadId + ".html#post" + post);
        }
        #endregion

        #region Quick Reply

        // Quick Reply
        void butSave_Click(object Sender, EventArgs e)
        {
            ForumsSecurityChecks forumsecuritycheck = new ForumsSecurityChecks();
            PostDAL postDetails = new PostDAL();

            UserDAL userdetails = new UserDAL();
            bool noSpam = false;
            int ForumCustomerCheck = forumsecuritycheck.CheckForumPostEligibility(customerId);
            bool CheckForModeration = forumsecuritycheck.CheckForModeration(customerId);
            if (ForumCustomerCheck != 0 && ForumCustomerCheck != -2)
            {
                if (ForumCustomerCheck == 1)
                {
                    lblMessage.Text = "The forum requires you to wait for 5 minutes before you post again. This restriction is lifted once you have 50 posts. Inconvenience is deeply regretted. ";
                    txtCaptcha.Text = "";
                }
                else
                {
                    if (!trCaptcha.Visible)
                    {
                        noSpam = true;
                    }
                    else
                    {
                        // or else try matching captcha image text and user input
                        if (Request.Cookies["CaptchaImageText"] != null)
                        {
                            if (txtCaptcha.Text.Trim() == CarwaleSecurity.Decrypt(Request.Cookies["CaptchaImageText"].Value, true))
                            {
                                noSpam = true;
                                Response.Cookies["CaptchaImageText"].Expires = DateTime.Now.AddDays(-1);// now expire the cookie
                            }
                        }
                    }
                    if (flagLogin == true)
                    {
                        if (noSpam)
                        {
                            if (userdetails.IsUserBanned(customerId) == false)//check whether the user is in the banned list or not
                            {
                                int alertType = 0;
                                if (chkEmailAlert.Checked)
                                    alertType = 2; // instant email subscription
                                else
                                    alertType = 1; // normal subscription
                                bool CheckForValidHyperlinks = SanitizeHTML.VerifyAllHyperlinks(rteQR.Text.Trim());
                                if(CheckForValidHyperlinks)
                                {
                                    if (CheckForModeration) // Indicates that posts need to be moderated
                                    {
                                        IsModerated = 0;
                                    }
                                    // post save with 100 alert type. It means no action is to be taken on alerts. Leave them intact.
                                    string remoteAddr = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"] == null ? null : HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                                    string clientIp = HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"] == null ? null : HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"];
                                    string postId = postDetails.SavePost(CurrentUser.Id, SanitizeHTML.ToSafeHtml(rteQR.Text), alertType, threadId, IsModerated, remoteAddr, clientIp);
                                    if (IsModerated == 0) // Indicates that posts need to be moderated
                                    {
                                        Response.Redirect("/forums/PostResponse.aspx");
                                    }

                                    CreateNVC publishToRMQ = new CreateNVC();
                                    publishToRMQ.UpdateLuceneIndex(threadId, "1");
                                    string discussionUrl = "/Forums/viewthread.aspx?thread=" + threadId + "&post=" + postId;
                                    if (postId != "-1")
                                    {
                                        NotificationsBusinessLogic notifyUsers = new NotificationsBusinessLogic();
                                        UserBusinessLogic cm = new UserBusinessLogic();
                                        var handleData = cm.GetExistingHandleDetails(Convert.ToInt32(CurrentUser.Id));
                                        notifyUsers.NotifySubscribers(discussionUrl, threadId, this.ThreadName, (handleData != null ? handleData.HandleName : string.Empty), CurrentUser.Id, CurrentUser.Email);//send mails to all the participants
                                        //redirect it to message page
                                        Response.Redirect(discussionUrl);
                                    }
                                    else
                                        lblMessage.Text = "Some error occured. Post could not be submitted. Please try again.";
                                }
                                else
                                {
                                    lblMessage.Text = "ERROR:Your post could not be submitted. One or more of your hyperlinks is/are not valid";
                                    txtCaptcha.Text = "";   
                                }
                            }
                            else
                            {
                                lblMessage.Text = "Your 'CarWale Forum' membership has been suspended. You cannot post in CarWale Forums anymore. If it looks like a mistake to you, please write to banwari@carwale.com.<br><br>";
                            }
                        }
                        else
                        {
                            txtCaptcha.Text = "";
                            lblCaptcha.Text = "Invalid code. Please try again.";
                        }
                    }
                    else
                    {
                        Response.Redirect("/Forums/notauthorized.aspx?returnUrl=" + HttpUtility.UrlEncode(Request.ServerVariables["HTTP_X_REWRITE_URL"]));
                    }
                }
            }
            // Ask user to verify email first and only then can he post in the forum.
            else if (ForumCustomerCheck == 0)
            {
                lblMessage.Text = "A verification email has been sent to your registered email address. Please verify your email address in order to continue posting in the forum.";
                Mails.CustomerRegistration(customerId);
            }
            else if (ForumCustomerCheck == -2)
            {
                lblMessage.Text = "Your 'CarWale Forum' membership has been suspended. You cannot post in CarWale Forums anymore. If it looks like a mistake to you, please write to banwari@carwale.com.<br><br>";
            }
        }

        #endregion

        #region Concatenate Posted By User Ids

        protected string ConcatenatePostedByUserIds(string _postedbyid)
        {
            if (postedByUserIds == "")
            {
                postedByUserIds = "," + _postedbyid + ",";
            }
            else
            {
                if (postedByUserIds.IndexOf("," + _postedbyid + ",") == -1)
                    postedByUserIds = postedByUserIds + _postedbyid + ",";
            }
            return "";
        }
        #endregion

        #region Get Posted By User Ids
        protected string GetPostedByUserIds()
        {
            return postedByUserIds;
        }
        #endregion

        #region Concatenate Post Ids
        protected string ConcatenatePostIds(string _postid)
        {
            if (concatenatedPostIds == "")
            {
                concatenatedPostIds = _postid;
            }
            else
            {
                concatenatedPostIds = concatenatedPostIds + "," + _postid;
            }
            return "";
        }
        #endregion

        #region Check Owner For Visibility

        protected string CheckOwnerForVisibility(string _postedById)
        {
            if (_postedById == CurrentUser.Id)
                return "style='display:none;'";
            else
                return "style='display:display;'";
        }
        #endregion

        #region Set Previous and next Url for pages.
        /// <summary>
        /// written By:Prashant Vishe On 21 June 2013
        /// Function is used to create Prev and next url.
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
                    nextUrl = mainUrl + threadId + "-" + ThreadUrl + "-p" + nextPageNumber + ".html";
                }
                else if ((pageNo == int.Parse(lastPageNo)) && (!String.IsNullOrEmpty(pageNo.ToString())))  //last page
                {
                    prevPageNumber = (pageNo - 1).ToString();
                    prevUrl = mainUrl + threadId + "-" + ThreadUrl + "-p" + prevPageNumber + ".html";
                }
                else
                { //middle pages
                    if (!String.IsNullOrEmpty(pageNo.ToString()))
                    {
                        prevPageNumber = (pageNo - 1).ToString();
                        prevUrl = mainUrl + threadId + "-" + ThreadUrl + "-p" + prevPageNumber + ".html";
                        nextPageNumber = (pageNo + 1).ToString();
                        nextUrl = mainUrl + threadId + "-" + ThreadUrl + "-p" + nextPageNumber + ".html";
                    }
                }
            }
        }
        #endregion

        #region Redirect To Default Page
        /// <summary>
        /// Redirects to the default page.
        /// </summary>
        private void RedirectToDefault()
        {
            Response.Redirect("default.aspx");
        }
        #endregion

    } // class
} // namespace