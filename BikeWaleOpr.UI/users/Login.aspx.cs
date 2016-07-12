using BikeWaleOpr.Common;
using Carwale.WebServices.OprAuthentication;
using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace BikeWaleOpr.users
{
    /// <summary>
    /// Written By : Ashwini Todkar on 17 Dec 2013
    /// Summary    : This class handle Bikewaleopr user Authentication 
    /// </summary>
    public class Login : Page
    {
        protected TextBox txtLoginid, txtPasswd;
        protected Button btnLogin;
        protected HtmlGenericControl spnErrorPwd;
        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
            btnLogin.Click += new EventHandler(btnLogin_click);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnLogin_click(object sender, EventArgs e)
        {
            Page.Validate();
            if (!Page.IsValid)
            {
                return;
            }

            OprAuthentication objOA = null;
            UserBasicInfo objBasicInfo = null;

            string userName = txtLoginid.Text.Trim();
            string password = txtPasswd.Text.Trim();

            objOA = new OprAuthentication();
            objBasicInfo = objOA.AuthenticateUser(userName, password);

            if (!string.IsNullOrEmpty(objBasicInfo.UserId) && objBasicInfo.UserId != "-1")
            {
                Trace.Warn((CurrentUser.Id != "-1").ToString());

                if (objBasicInfo.IsAuthenticated)
                {
                    BikeWaleAuthentication.CreateAuthCookies(objBasicInfo.UserId, objBasicInfo.Name);
                    Response.Redirect("/default.aspx");
                }
                else
                {
                    spnErrorPwd.InnerText = "Wrong loginid or password.";
                }
            }
            else
                spnErrorPwd.InnerText = "Wrong loginid or password.";
        }
        // End of IsValidPassword 
    }//Login
}//