/*THIS CLASS IS FOR ADDING, EDITING AND DELETING MarketS FROM AND TO THE MarketS TABLE INTO THE DATABASE
*/

using System;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using Carwale.Notifications;
using Carwale.UI.Common;
using Carwale.DAL.CoreDAL;
using Carwale.DAL.Customers;
using Carwale.Interfaces;
using Carwale.Service;
using Carwale.Entity;
using Carwale.Utility;

namespace Carwale.UI.Users
{
    public class VerifyEmail : Page
    {
        protected HtmlGenericControl spnError;

        override protected void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.Load += new System.EventHandler(this.Page_Load);
        }

        private void Page_Load(object sender, System.EventArgs e)
        {
            if (!IsPostBack)
            {
                string userId = CurrentUser.Id;
                bool showPageNotFound = false;


                // verify that pasted code has some length i.e. is not empty.
                if (string.IsNullOrEmpty(Request.QueryString["verify"]))
                {
                    showPageNotFound = true;
                }



                //  string userIdDecripted = CarwaleSecurity.DecodeVerificationCode(Request["verify"].ToString().Replace(' ','+' ));

                string userIdDecripted = Utils.Utils.DecryptTripleDES(Request["verify"].ToString());
                HttpContext.Current.Trace.Warn("Code to be unwound : " + Request["verify"].ToString());
                HttpContext.Current.Trace.Warn("Decryped staging test : " + userIdDecripted);
                try
                {
                    if (showPageNotFound == false && userIdDecripted != "-1")
                    {
                        try
                        {

                            ICustomerRepository<Customer, CustomerOnRegister> customerRepo = UnityBootstrapper.Resolve<ICustomerRepository<Customer, CustomerOnRegister>>();
                            customerRepo.UpdateEmailVerfication(true,CustomParser.parseIntObject(userIdDecripted));
                            if (userId == "-1") // Login user if not logged in
                            {
                                CustomerDetails cd = new CustomerDetails(userIdDecripted);

                                // end current session.
                                CurrentUser.EndSession();

                                // create fresh session.
                                CurrentUser.StartSession(cd.Name, userIdDecripted, cd.Email, true);
                            }
                        }
                        catch (Exception err)
                        {
                            Trace.Warn(err.Message);
                            ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                            objErr.SendMail();
                        }

                        showPageNotFound = false;

                        spnError.InnerText = "Congratulations! Account Verified Successfully.";
                    }
                }
                catch (Exception err)
                {
                    showPageNotFound = true;

                    Trace.Warn(err.Message);
                    ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                    objErr.SendMail();
                }

                if (showPageNotFound) UrlRewrite.Return404();
                else Response.Redirect(CommonOpn.AppPath + "MyCarwale/", false);
            }
        }
    }//class
}//namespace	
