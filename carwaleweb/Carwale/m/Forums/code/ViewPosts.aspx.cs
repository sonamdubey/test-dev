using System;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using MobileWeb.Common;
using MobileWeb.DataLayer;
using System.Data;

namespace MobileWeb.Forums
{
	public class ViewPosts : Page
	{
		protected string forumId, threadId, pageNo, threadCount = "0", topic, forumSubCatName = "", forumSubCatId = "", currPage = "1",threadUrl = "",postUrl = "";
		protected int pageSize = 10, totalPages = 0;
        protected string hdnPosts = "";
        protected Repeater rptPosts;
        private bool CheckCount = false;// This is used to check if the count for a thread is 0 and the user is accordingly redirected to forums default page.

		protected override void OnInit( EventArgs e )
		{
			InitializeComponent();
		}
		
		void InitializeComponent()
		{
			base.Load += new EventHandler( Page_Load );
		}

		void Page_Load( object Sender, EventArgs e )
		{
			if (!Page.IsPostBack)
			{
				if(Request["thread"] != null && Request.QueryString["thread"] != "")
				{
					threadId = Request.QueryString["thread"];
					
					//verify the id as passed in the url
					if(CommonOpn.CheckId(threadId) == false)
					{
						//redirect to the default page
						Response.Redirect("~/m/forums/");
						return;
					}
				}
				else
				{
					//redirect to the default page
                    Response.Redirect("~/m/forums/");
					return;
				}
                if (Request.QueryString["pg"] != null && Request.QueryString["pg"].ToString() != "")
                {
                    Trace.Warn("pg : " + Request.QueryString["pg"].ToString());
                    currPage = Request.QueryString["pg"].ToString();
                }
				
				UpdateThreadViews();
				GetThreadDetails();
				GetPostCount();
                GetPageWisePosts();
                
                if (CheckCount)
                {
                    Response.Redirect("/m/forums/" + threadUrl + "/", false);
                    Response.StatusCode = 301;
                    Response.End();
                }
			}	
		}
		
		private void UpdateThreadViews()
		{
			Forum obj = new Forum();
			obj.UpdateThreadViews(threadId);
		}
		
		private void GetThreadDetails()
		{
			IDataReader dr = null;
			Forum obj = new Forum();
			try
			{
				obj.GetReader = true;
				obj.GetThreadDetails(threadId);
				dr = obj.drReader;
				if (dr.Read())
				{
					topic = dr["Topic"].ToString();	
					forumSubCatName = dr["Name"].ToString();
					forumSubCatId = dr["ID"].ToString();
                    postUrl = dr["PostUrl"].ToString();
                    threadUrl = dr["ThreadUrl"].ToString();
				}
			}
			catch(Exception err)
			{
				ErrorClass objErr = new ErrorClass(err,Request.ServerVariables["URL"]);
				objErr.SendMail();
			}
			finally
			{
				obj.drReader.Close();
			}
		}
	
        private void GetPageWisePosts()
        {
            Forum obj = new Forum();
            obj.GetRepeater = true;
            obj.Rpt = rptPosts;
            obj.GetPagewisePosts(threadId,pageSize.ToString(), currPage);
            if (rptPosts.Items.Count == 0)
                CheckCount = true;
        }
		
		private void GetPostCount()
		{
			IDataReader dr = null;
			Forum obj = new Forum();
			try
			{
				obj.GetReader = true;
				obj.GetThreadPostCount(threadId);
				dr = obj.drReader;
				if (dr.Read())
				{
					int totalPosts = 0;
					totalPosts = Convert.ToInt32(dr[0].ToString());
					totalPages = totalPosts / pageSize;
					if (totalPosts % pageSize != 0)
						totalPages++;
				}
			}
			catch(Exception err)
			{
				ErrorClass objErr = new ErrorClass(err,Request.ServerVariables["URL"]);
				objErr.SendMail();
			}
			finally
			{
				obj.drReader.Close();
			}
		}

        public string GetDateTime(string value)
        {
            if (value != "")
                return Convert.ToDateTime(value).ToString("dd-MMM, yyyy hh:mm tt");
            else
                return "N/A";
        }

        public string GetUserTitle(string role, string posts, string bannedCust)
        {
            string title = "";

            if (bannedCust != "-1")
            {
                title = "[BANNED]";
            }
            else if (role != "")
            {
                title = "Moderator";
            }
            else
            {
                int noOfPosts = int.Parse(posts);

                if (noOfPosts < 26)
                    title = "New Arrival";
                else if (noOfPosts < 51)
                    title = "Driven";
                else if (noOfPosts < 101)
                    title = "Road-tested";
                else if (noOfPosts < 251)
                    title = "Long-termer";
                else if (noOfPosts < 501)
                    title = "Beloved";
                else if (noOfPosts < 1001)
                    title = "Best-seller";
                else title = "Legend";
            }

            return title;
        }

        public string GetMessage(string value)
        {
            string post = value;
            string quoteStart = "<div class='quote'>Posted by <b>";
            string quotePostedByEnd = "</b><br>";
            string quoteEnd = "</div>";

            // Identify and replace quotes
            if (post.ToLower().IndexOf("[^^quote=") >= 0)
            {
                Trace.Warn("Quote Found");
                post = post.Replace("[^^quote=", quoteStart);
                post = post.Replace("[^^/quote^^]", quoteEnd);
                post = post.Replace("^^]", quotePostedByEnd);
            }

            if (Request["h"] != null && Request["h"] != "")
            {
                post = post.ToLower().Replace(Request["h"], "<span style='background:#FFFF00;color:#000000;'>" + Request["h"] + "</span>");
            }

            post = post.Replace("<p>&nbsp;</p>", ""); // remove empty paragraphs.
            Trace.Warn(post);
            post = CommonOpn.RemoveAnchorTag(post);
            return post;
        }

        protected string CheckOwnerForVisibility(string _postedById)
        {
            if (_postedById == CurrentUser.Id)
                return "style='display:none;'";
            else
                return "style='display:display;'";
        }
	}
}		