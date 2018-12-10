/*******************************************************************************************************
IN THIS CLASS THE NEW MEMBEERS WHO HAVE REQUESTED FOR REGISTRATION ARE SHOWN
*******************************************************************************************************/
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Carwale.UI.Common;
using Carwale.Notifications;
using CarwaleAjax;
using Carwale.BL.Forums;
using System.Collections.Generic;
using Carwale.Interfaces;
using Carwale.BL.Customers;
using Carwale.Entity;
using Microsoft.Practices.Unity;
using Carwale.Service;
using Carwale.Interfaces.Geolocation;
using Carwale.DAL.Customers;

namespace Carwale.UI.MyCarwale
{
    public class EditCustomerDetails : Page
    {
        protected HtmlGenericControl spnError, divNote;
        protected DropDownList drpState, drpCity, drpPinCode;
        protected TextBox txtName, txtPhone1, txtMobile, txtAddress, txtStdCode1;

        protected Label lblEmail;
        protected CheckBox chkNewsLetter;
        protected HtmlInputCheckBox chkDOB;

        protected Button btnSave;
        private IGeoCitiesCacheRepository _geoCitiesCacheRepo;
        protected ICustomerBL<Customers, Customers> _customerBl;

        public string customerId, cityId = "", areaId = "", pinId = "";
        public CustomerDetails cd;
        protected HiddenField setToken;

        public bool NewCustomer
        {
            get { return Convert.ToBoolean(ViewState["NewCustomer"]); }
            set { ViewState["NewCustomer"] = value; }
        }

        public string TokenString
        {
            get {
                if (ViewState["tokenstring"] == null) ViewState["tokenstring"] = CustomerSecurity.getRandomString(10);
                    return ViewState["tokenstring"].ToString();   
            }
        }

        public string Comments
        {
            get
            {
                if (ViewState["Comments"] != null)
                    return ViewState["Comments"].ToString();
                else
                    return "";
            }
            set { ViewState["Comments"] = value; }
        }

        public string SelectedCity
        {
            get
            {
                if (Request.Form["drpCity"] != null)
                    return Request.Form["drpCity"].ToString();
                else
                    return "-1";
            }
        }

        public string CityContents
        {
            get
            {
                if (Request.Form["hdn_drpCity"] != null)
                    return Request.Form["hdn_drpCity"].ToString();
                else
                    return "";
            }
        }


        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            using (IUnityContainer container = UnityBootstrapper.Resolver.GetContainer())
            {
                _geoCitiesCacheRepo = container.Resolve<IGeoCitiesCacheRepository>();
            }
            base.Load += new EventHandler(Page_Load);
            btnSave.Click += new EventHandler(btnSave_Click);
        }

        void Page_Load(object Sender, EventArgs e)
        {
            //register the ajax library and emits corresponding javascript code
            //for this page
            Ajax.Utility.RegisterTypeForAjax(typeof(AjaxFunctions));

            // check for login.
            if (CurrentUser.Id == "-1")
                Response.Redirect("/users/login.aspx?returnUrl=/MyCarwale/EditCustomerDetails.aspx");

            customerId = CurrentUser.Id;

            if (Request["returnUrl"] != null)
                divNote.Visible = true;
            else
                divNote.Visible = false;

            if (!IsPostBack)
            {
                if(Request.HttpMethod == "POST") Response.Redirect("/mycarwale/MyContactDetails.aspx", true);
                setToken.Value = TokenString;
                NewCustomer = false;

                FillStates();

                ShowCustomerDetails();
            }
            else
            {
                if (setToken.Value != TokenString) Response.Redirect("/MyCarwale/EditCustomerDetails.aspx", true);
                //in case of post back update contents of the city and the area drop down
                AjaxFunctions aj = new AjaxFunctions();
                //update the contents for city
                aj.UpdateContents(drpCity, CityContents, SelectedCity);
            }

        } // Page_Load

        //show the customer details
        void ShowCustomerDetails()
        {
            cd = new CustomerDetails(customerId);
            NewCustomer = !cd.IsVerified;
            string phone = cd.Phone1;
            string[] str_PhoneCode = phone.Split('-');
            Trace.Warn(str_PhoneCode.Length.ToString());

            if (cd.Exists == true)
            {
                txtName.Text = cd.Name;
                if (str_PhoneCode.Length > 1)
                {
                    txtStdCode1.Text = str_PhoneCode[0];
                    txtPhone1.Text = str_PhoneCode[1];
                }
                else
                {
                    txtPhone1.Text = str_PhoneCode[0];
                }
                txtMobile.Text = cd.Mobile;
                txtAddress.Text = cd.Address;
                lblEmail.Text = cd.Email;
                string stateId = cd.StateId;
                cityId = cd.CityId;
                areaId = cd.AreaId;

                if (stateId != "" && stateId != "-1")
                {
                    drpState.SelectedIndex = drpState.Items.IndexOf(drpState.Items.FindByValue(stateId));           
                    drpCity.DataSource = _geoCitiesCacheRepo.GetCitiesByStateId(Convert.ToInt32(cd.StateId));
                    drpCity.DataTextField = "CityName";
                    drpCity.DataValueField = "CityId";
                    drpCity.DataBind();
                    drpCity.Items.Insert(0, new ListItem("Any", "0"));
                    Trace.Warn("cityId : " + cityId);
                    drpCity.SelectedIndex = drpCity.Items.IndexOf(drpCity.Items.FindByValue(cityId));
                }
                pinId = cd.PinCodeId;
                chkNewsLetter.Checked = cd.ReceiveNewsletters;
                Comments = cd.Comment;
            }
            else
            {
                spnError.InnerHtml = "No such customer exists";
                btnSave.Enabled = false;
            }
        }

        void FillStates()
        {
            List<Carwale.Entity.Geolocation.States> states = new List<Carwale.Entity.Geolocation.States>(); 
            UserBusinessLogic userDetails = new UserBusinessLogic();
            try
            {
                states = _geoCitiesCacheRepo.GetStates();
                drpState.DataSource = states;
                drpState.DataTextField = "StateName";
                drpState.DataValueField = "StateId";
                drpState.DataBind();
                drpState.Items.Insert(0, new ListItem("Select State", "-1"));
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }


        void btnSave_Click(object sender, EventArgs e)
        {
            Page.Validate();
            if (!IsValid)
                return;

            if (SaveCustomerDetails() == true)
            {

                if (Request["returnUrl"] != null && Request["returnUrl"].ToString() != "")
                    Response.Redirect(Request["returnUrl"].ToString());
                else
                    Response.Redirect("MyContactDetails.aspx");

                spnError.InnerHtml = "Your Details has been updated successfully.";
            }
            else
                spnError.InnerHtml = "Your Details could not be updated. Please try again.";
        }

        bool SaveCustomerDetails()
        {

            ICustomerBL<Customer, CustomerOnRegister> customerRepo = new CustomerActions<Customer, CustomerOnRegister>();
            Customer custDetails = new Customer();
            bool returnVal = false;
            string address = txtAddress.Text.Trim();
            if (address.Length > 250)
                address = address.Substring(0, 249);
            string stateId = "0";
            if (drpState.SelectedIndex > -1)
                stateId = drpState.SelectedItem.Value;
            string phoneNo = "";
            if (txtStdCode1.Text != "")
                phoneNo = txtStdCode1.Text.Trim() + "-" + txtPhone1.Text.Trim();
            else
                phoneNo = txtPhone1.Text.Trim();
            try
            {
                custDetails.CustomerId = customerId;
                custDetails.Name = txtName.Text.Trim();
                custDetails.Email = lblEmail.Text.Trim();
                custDetails.Address = address;
                custDetails.StateId = Convert.ToInt32(stateId);
                custDetails.CityId = Convert.ToInt32(SelectedCity);
                custDetails.Phone = phoneNo;
                custDetails.Mobile = txtMobile.Text.Trim();
                custDetails .ReceiveNewsletters = chkNewsLetter.Checked;
                custDetails.IsVerified = true;
                custDetails.IsFake = false;
               returnVal = customerRepo.UpdateCustomerDetails(custDetails);
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
                returnVal = false;
            } // catch Exception
            return returnVal;
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