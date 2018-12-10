/*******************************************************************************************************
IN THIS CLASS THE NEW MEMBEERS WHO HAVE REQUESTED FOR REGISTRATION ARE SHOWN
*******************************************************************************************************/
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;
using Carwale.Notifications;
using Carwale.UI.Common;
using CarwaleAjax;
using Carwale.Entity.Forums;
using Carwale.BL.Forums;

namespace Carwale.UI.Users
{
    public class EditUserHandle : Page
    {
        protected TextBox txtHandle;
        protected Label lblMessage;
        protected Button btnSave;
        protected HtmlGenericControl spnHandle;
        protected string sysUserName = "";
        public UserProfile result;
        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
            btnSave.Click += new EventHandler(btnSave_Click);
        }

        void Page_Load(object Sender, EventArgs e)
        {
            lblMessage.Text = "";
            spnHandle.InnerText = "";
            //Ajax 	
            Ajax.Utility.RegisterTypeForAjax(typeof(AjaxForum));

            if (!IsPostBack)
            {
                if (CurrentUser.Id == "-1")
                {
                    Response.Redirect("Login.aspx?ReturnUrl=/users/EditUserHandle.aspx");
                }
                else
                {
                    result = new UserProfile();
                    UserBusinessLogic userDetails = new UserBusinessLogic();
                    result = userDetails.GetExistingHandleDetails(Convert.ToInt32(CurrentUser.Id));
                    if(result != null)
                    {
                        txtHandle.Text = result.HandleName = !string.IsNullOrEmpty(result.HandleName) ? result.HandleName : string.Empty;
                        if (result.IsUpdated.ToString() == "True")
                    {
                        txtHandle.Enabled = false;
                        if (txtHandle.Text != null && txtHandle.Text != "") ReturnUrl();
                    }
                    else
                    {
                        if (result.HandleName != null && result.HandleName.ToString() != "")
                        {
                            sysUserName = "<p class='subTitle'>Temporary user name. Created by system. Please update it.</p>";
                        }
                    }
                   }
                }

            }
        }

        void btnSave_Click(object Sender, EventArgs e)
        {
            //To check the validation for Handle name from server side
            HandleChk();
            if (CurrentUser.Id != "-1")
            {
                InsertHandle();
            }
            else
            {
                Response.Redirect("/Login.aspx?ReturnUrl=/users/EditUserHandle.aspx");
            }
        }

        void InsertHandle()
        {
            bool result;
            try
            {
                UserBusinessLogic userDetails = new UserBusinessLogic();
                result = userDetails.InsertHandle(Convert.ToInt32(CurrentUser.Id), txtHandle.Text.Trim(), true);
                if (result)
                {
                    ReturnUrl();
                }
                else
                {
                    spnHandle.InnerHtml = "<span class='errMes'>user name is already used.</span>";
                    return;
                }
            }
            catch (Exception err)
            {
                ExceptionHandler objErr = new ExceptionHandler(err, "InsertHandle()");
                objErr.LogException();
            }
        }

        void HandleChk()
        {
            string chkReg = "";

            //To Validate the Handle Value with Regular expressions and Should not start with a Dot 
            chkReg = ValidateHandle(txtHandle.Text.ToString());

            Trace.Warn("False : " + chkReg);

            if (chkReg == "False")
            {
                lblMessage.Text = "User name should be 3-20 characters in length. Use A-Z, 0-9 and dot (.) to form a name.";
                return;
            }

        }

        //Validation for Handle
        public string ValidateHandle(string handle)
        {
            string reghandle = @"^[a-z_0-9_.]*$";
            string val = "False";

            if (Regex.IsMatch(handle.ToLower(), reghandle))
            {
                val = "True";
            }
            if (handle == string.Empty)
            {
                val = "True";
            }
            if (handle.Substring(0, 1) == ".")
            {
                val = "True";
            }
            if (handle.Length < 3 || handle.Length > 15)
            {
                val = "True";
            }

            return val;
        }

        void ReturnUrl()
        {
            if ((Request["ReturnUrl"] != null) && (Request.QueryString["ReturnUrl"] != ""))
            {
                string returnUrl = Request.QueryString["ReturnUrl"];

                //validating return url
                if (ScreenInput.IsValidRedirectUrl(returnUrl) == true)
                {
                    Response.Redirect(returnUrl, false);
                }
                else
                {
                    Response.Redirect("/users/EditUserHandle.aspx", false);
                }
            }
            else
            {
                Response.Redirect("/mycarwale/MyContactDetails.aspx", false);
            }
        }
    }
}