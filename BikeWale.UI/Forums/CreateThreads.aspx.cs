using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Bikewale.Common;
using Bikewale.Forums.Common;
using Bikewale.Controls;

namespace Bikewale.Forums
{
    public class CreateThreads : System.Web.UI.Page
    {
        protected HtmlGenericControl divForum;
        protected Label lblMessage, lblCaptcha;
        protected Button butSave;
        protected TextBox txtTopic, txtEmail, txtCaptcha;
        protected RichTextEditor rteNT;
        protected HtmlTableRow trCustomer, trCaptcha, trAlert;
        protected CheckBox chkEmailAlert;

        public string forumId = "", customerId = "";

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

        protected override void OnInit(EventArgs e)
        {
            base.Load += new EventHandler(Page_Load);
            butSave.Click += new EventHandler(butSave_Click);
        }
        private void Page_Load(object sender, EventArgs e)
        {
            customerId = CurrentUser.Id;
            // Modified By :Lucky Rathore on 12 July 2016.
            Form.Action = Request.RawUrl;
            //if the customer is already registered then dont ask information for the 
            //customer name and location, else ask the name, email and the location
            if (customerId != "-1")
            {
                trCustomer.Visible = false;
                trCaptcha.Visible = false;
                trAlert.Visible = true;
            }
            else
            {
                Response.Redirect("/users/login.aspx?returnUrl=" + HttpUtility.UrlEncode(Request.ServerVariables["HTTP_X_ORIGINAL_URL"]));
            }

            //also get the forumId
            if (Request["forum"] != null && Request.QueryString["forum"] != "")
            {
                forumId = Request.QueryString["forum"];

                //verify the id as passed in the url
                if (CommonOpn.CheckId(forumId) == false)
                {
                    //redirect to the default page
                    Response.Redirect("default.aspx");
                    return;
                }
            }
            else
            {
                //redirect to the default page
                Response.Redirect("default.aspx");
                return;
            }
            
            if (!IsPostBack)
            {              
                ForumName = GetForum();
                UserTracking ut = new UserTracking();
                ut.SaveActivity(CurrentUser.Id.ToString(), "3", forumId, "-1");               
            }
        }//pageload

        void butSave_Click(object Sender, EventArgs e)
        {
            bool noSpam = false; // if true, it ensures that there is no spamming attempt

            // check if user is logged in. 
            // If he is logged in then there is no chance of spamming.
            if (!trCaptcha.Visible)
            {
                noSpam = true;
            }
            else
            {
                // or else try matching captcha image text and user input
                if (Request.Cookies["CaptchaImageText"] != null)
                {
                    if (txtCaptcha.Text.Trim() == Request.Cookies["CaptchaImageText"].Value)
                    {
                        noSpam = true;

                        // now expire the cookie
                        Response.Cookies["CaptchaImageText"].Expires = DateTime.Now.AddDays(-1);
                    }
                }
            }

            if (noSpam)
            {
                //now check whether the customer is in the banned list or not.
                if (ForumsCommon.IsUserBanned(customerId) == false)
                {
                    string threadId = SaveDetails();
                    string msgPage = "/forums/viewthread-" + threadId+".html";

                    lblCaptcha.Text = "";

                    if (threadId != "-1")
                    {
                        Response.Redirect(msgPage);
                    }
                    else
                        lblMessage.Text = "ERROR:Your entry could not be submitted. Please try again.";
                }
                else
                {
                    lblMessage.Text = "Your 'BikeWale Forum' membership has been suspended. You cannot post in BikeWale Forums anymore. If it looks like a mistake to you, please write to banwari@bikewale.com.<br><br>";
                }
            }
            else
            {
                txtCaptcha.Text = "";
                lblCaptcha.Text = "Invalid code. Please try again.";
            }
        }

        public string GetForum()
        {
            string forum = "";
            Database db = new Database();
            SqlDataReader dr = null;

            string sql = "";

            sql = " SELECT Name, Description FROM ForumSubCategories With(NoLock) WHERE IsActive = 1 AND ID = @ForumId ";
            try
            {
                SqlCommand cmd = new SqlCommand(sql);

                cmd.Parameters.Add("@ForumId", SqlDbType.BigInt).Value = forumId;

                dr = db.SelectQry(cmd);

                if (dr.Read())
                {
                    forum = dr["Name"].ToString();
                    ForumDescription = dr["Description"].ToString();
                }

            }
            catch (SqlException err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            } // catch Exception
            finally
            {
                if(dr != null)
                    dr.Close();

                db.CloseConnection();
            }

            return forum;
        }

        string SaveDetails()
        {
            string threadId = "-1";

            int alertType = 0;

            if (trAlert.Visible)
                if (chkEmailAlert.Checked) alertType = 2; // instant email subscription
                else alertType = 1; // normal subscription
            else alertType = 0; // do not subscribe at all

            threadId = ForumsCommon.CreateNewThread(
                            customerId,
                            SanitizeHTML.ToSafeHtml(rteNT.Text.Trim()),
                            alertType,
                            forumId,
                            txtTopic.Text.Trim());

            return threadId;
        }
    }//class
}//namespace