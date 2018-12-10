using System;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Carwale.Entity;
using Carwale.BL.Customers;
using Carwale.Interfaces;
using Carwale.Notifications;
using Carwale.UI.Common;
using Carwale.Entity.Enum;
using MySql.Data.MySqlClient;
//using Carwale.PriceQuote;

namespace Carwale.UI.Controls
{
    public class RegisterControl : UserControl
    {
        protected HtmlGenericControl spnError, errEmail;
        protected DropDownList cmbStates;
        protected Button btnRegister;
        protected TextBox txtName, txtEmail, txtPassword, txtPin,
                txtMobile, txtAddress, txtCity, txtArea, txtEmailConf;
        protected CheckBox chkNewsLetter;

        public bool showContactDetails = false;

        protected HtmlTableRow trContactDetails;

        private string redirectUrl = "";

        public string RedirectUrl
        {
            set { redirectUrl = value; }
        }

        public string Name
        {
            set { txtName.Text = value; }
        }

        public string EmailId
        {
            set { txtEmail.Text = value; }
        }

        public string EmailIdConf
        {
            set { txtEmailConf.Text = value; }
        }

        public string MobileNo
        {
            set { txtMobile.Text = value; }
        }

        public string StateId
        {
            get
            {
                if (ViewState["StateId"] != null && ViewState["StateId"].ToString() != "")
                    return ViewState["StateId"].ToString();
                else
                    return "-1";
            }
            set { ViewState["StateId"] = value; }
        }

        public string CityId
        {
            get
            {
                if (ViewState["CityId"] != null && ViewState["CityId"].ToString() != "")
                    return ViewState["CityId"].ToString();
                else
                    return "-1";
            }
            set { ViewState["CityId"] = value; }
        }

        public string City
        {
            get
            {
                if (ViewState["City"] != null && ViewState["City"].ToString() != "")
                    return ViewState["City"].ToString();
                else
                    return "";
            }
            set { ViewState["City"] = value; }
        }

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
            this.btnRegister.Click += new EventHandler(btnRegister_Click);
        }

        void Page_Load(object Sender, EventArgs e)
        {
            CommonOpn op = new CommonOpn();
            showContactDetails = op.GetNeedContactInformation();

            trContactDetails.Visible = showContactDetails;
        } // Page_Load

        void btnRegister_Click(object sender, EventArgs e)
        {
            if (CurrentUser.Id != "-1")
            {
                string returnUrl = "";
                if (Request["returnUrl"] != null && Request["returnUrl"] != "")
                    returnUrl += Request["returnUrl"];
                else if (redirectUrl != "")
                    returnUrl = redirectUrl;
                else
                    returnUrl += "/Users/registrationConfirmation.aspx?returnUrl=" + Request["returnUrl"];

                // redirect to the confirmation page.
                Response.Redirect(returnUrl);

                return;
            }

            string reEmail = @"^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,6}$";

            if (Regex.IsMatch(txtEmail.Text.ToLower(), reEmail))
            {
                RegisterUser();
            }
            else
            {
                errEmail.InnerText = "Invalid Email!";
            }
        } // btnSave_Click

        void RegisterUser()
        {
            CustomerOnRegister customer = new CustomerOnRegister();
            string val = string.Empty;
            try
            {

                val += "@Name=" + CommonOpn.ConvertToTitleCase(txtName.Text.Trim()) + ",";
                val += "@Email=" + txtEmail.Text.Trim() + ",";
                val += "@Mobile=" + (showContactDetails ? (String.IsNullOrEmpty(txtMobile.Text.Trim()) ? "" : txtMobile.Text.Trim()) : "") + ",";
                val += "@Password=" + txtPassword.Text.Trim() + ",";

                ICustomerBL<Customer, CustomerOnRegister> customerRepo = new CustomerActions<Customer, CustomerOnRegister>();
                customer = customerRepo.CreateCustomer(new Customer()
                {
                    Name = CommonOpn.ConvertToTitleCase(txtName.Text.Trim()),
                    Email = txtEmail.Text.Trim(),
                    Mobile = showContactDetails ? (String.IsNullOrEmpty(txtMobile.Text.Trim()) ? "" : txtMobile.Text.Trim()) : "",
                    Password = txtPassword.Text.Trim()
                });

            }
            catch (MySqlException err)
            {
                //catch the sql exception. if it is equal to 2627, then say that it is for duplicate entry 
                Trace.Warn(err.Message);
                Exception ex = new Exception(err.Message + "Values : " + val);
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();


            } // catch SqlException
            catch (Exception err)
            {
                Trace.Warn(err.Message);
                Exception ex = new Exception(err.Message + "Values : " + val);
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            } // catch Exception
            finally
            {
                // if its a fresh registration!
                if (customer.StatusOnRegister == "N")
                {
                    // login this user.
                    CurrentUser.StartSession(txtName.Text, customer.CustomerId, txtEmail.Text, false);

                    //also update the SourceId
                    SourceIdCommon.UpdateSourceId(EnumTableType.Customers, customer.CustomerId);


                    string returnUrl = "";

                    if (Request["returnUrl"] != null && Request["returnUrl"] != "")
                        returnUrl += Request["returnUrl"];
                    else if (redirectUrl != "")
                        returnUrl = redirectUrl;
                    else
                        returnUrl += "/Users/registrationConfirmation.aspx?returnUrl=" + Request["returnUrl"];

                    // redirect to the confirmation page.
                    Response.Redirect(returnUrl);
                }
                // Already registered.
                if (customer.StatusOnRegister == "O")
                {
                    spnError.InnerHtml = "<p>This email-id is already registered in Carwale. "
                                + "If you have forgotten your password, "
                                + "<a style=\"color:blue;\" target=\"_blank\" href=\"/users/forgotPassword.aspx?loginid="
                                + txtEmail.Text + "\">Click Here</a> to recover "
                                + "Or try with another email-id.</p>";
                }
            }
        }
        public int PrimaryPhone
        {
            get
            {
                int primary = 1;
                primary = txtMobile.Text.Trim() == "" ? 1 : 3;

                return primary;
            }
        }
    } // class
} // namespace