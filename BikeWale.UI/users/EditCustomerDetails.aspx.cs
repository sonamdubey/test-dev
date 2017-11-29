using Bikewale.BAL.MobileVerification;
using Bikewale.Common;
using MySql.CoreDAL;
/*******************************************************************************************************
IN THIS CLASS THE NEW MEMBEERS WHO HAVE REQUESTED FOR REGISTRATION ARE SHOWN
*******************************************************************************************************/
using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
//using Ajax;

namespace Bikewale.MyBikeWale
{
    public class EditCustomerDetails : Page
    {
        protected HtmlGenericControl spnError, divNote;
        protected DropDownList drpState, drpCity, drpPinCode;
        protected TextBox txtName, txtMobile;
        //txtAddress, txtStdCode1,txtPhone1; 

        protected Label lblEmail;
        protected CheckBox chkNewsLetter;
        protected HtmlInputCheckBox chkDOB;

        protected Button btnSave;

        public string customerId, cityId = "", areaId = "", pinId = "";
        public CustomerDetails cd;

        public bool NewCustomer
        {
            get { return Convert.ToBoolean(ViewState["NewCustomer"]); }
            set { ViewState["NewCustomer"] = value; }
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
            base.Load += new EventHandler(Page_Load);
            btnSave.Click += new EventHandler(btnSave_Click);
        }

        void Page_Load(object Sender, EventArgs e)
        {
            //register the ajax library and emits corresponding javascript code
            //for this page
            //Ajax.Utility.RegisterTypeForAjax(typeof(AjaxFunctions));

            // check for login.
            if (CurrentUser.Id == "-1")
                Response.Redirect("/users/login.aspx?returnUrl=/users/EditCustomerDetails.aspx");

            customerId = CurrentUser.Id;

            if (Request["returnUrl"] != null)
                divNote.Visible = true;
            else
                divNote.Visible = false;

            if (!IsPostBack)
            {
                NewCustomer = false;

                FillStates();
                ShowCustomerDetails();

                Trace.Warn("fill state");
            }
            else
            {
                //in case of post back update contents of the city and the area drop down
                //AjaxFunctions aj = new AjaxFunctions();

                ////update the contents for city
                //aj.UpdateContents(drpCity, CityContents, SelectedCity);
                CommonOpn opn = new CommonOpn();
                opn.UpdateContents(drpCity, CityContents, SelectedCity);
            }

        } // Page_Load

        //show the customer details
        void ShowCustomerDetails()
        {
            cd = new CustomerDetails(customerId);
            NewCustomer = !cd.IsVerified;

            //Code Added on 29 Oct 2009 By Sentil
            string phone = cd.Phone1;
            string[] str_PhoneCode = phone.Split('-');
            Trace.Warn(str_PhoneCode.Length.ToString());

            if (cd.Exists == true)
            {
                txtName.Text = cd.Name;
                txtMobile.Text = cd.Mobile;
                lblEmail.Text = cd.Email;

                string stateId = cd.StateId;
                cityId = cd.CityId;
                areaId = cd.AreaId;


                if (stateId != "" && stateId != "-1" && stateId != "0")
                {
                    drpState.SelectedIndex = drpState.Items.IndexOf(drpState.Items.FindByValue(stateId));

                    StateCity objCity = new StateCity();
                    DataTable dtCities = objCity.GetCities(stateId, "ALL");

                    if (dtCities != null && dtCities.Rows.Count > 0)
                    {
                        drpCity.DataSource = dtCities;
                        drpCity.DataTextField = "Text";
                        drpCity.DataValueField = "Value";
                        drpCity.DataBind();
                        drpCity.Items.Insert(0, new ListItem("Select City", "0"));
                        drpCity.SelectedValue = cityId;
                    }
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
            string sql = "";
            CommonOpn op = new CommonOpn();

            sql = " select Id, Name from states where isdeleted = 0 order by name ";
            try
            {
                op.FillDropDown(sql, drpState, "Name", "ID");
                drpState.Items.Insert(0, new ListItem("Select State", "0"));
            }
            catch (SqlException err)
            {
                Trace.Warn(err.Message);
                ErrorClass.LogError(err, Request.ServerVariables["URL"]);
                
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message);
                ErrorClass.LogError(err, Request.ServerVariables["URL"]);
                
            }
        }


        void btnSave_Click(object sender, EventArgs e)
        {
            Page.Validate();
            if (!IsValid)
                return;

            if (SaveCustomerDetails())
            {

                if (Request["returnUrl"] != null && Request["returnUrl"].ToString() != "")
                    Response.Redirect(Request["returnUrl"].ToString());
                else
                    Response.Redirect("/users/MyContactDetails.aspx");

                spnError.InnerHtml = "Your Details has been updated successfully.";
            }
            else
                spnError.InnerHtml = "Your Details could not be updated. Please try again.";
        }

        bool SaveCustomerDetails()
        {
            bool returnVal = false;
            CommonOpn op = new CommonOpn();

            string stateId = "0";

            if (drpState.SelectedIndex > -1)
                stateId = drpState.SelectedItem.Value;

            try
            {


                using (DbCommand cmd = DbFactory.GetDBCommand("updatecustomerdetails"))
                {
                    MobileVerification objMV = new MobileVerification();
                    bool _isverified = objMV.IsMobileVerified(txtMobile.Text.Trim(), lblEmail.Text.Trim());

                    cmd.CommandType = CommandType.StoredProcedure;

                    //prm = cmd.Parameters.Add("@customerid", SqlDbType.BigInt);
                    //prm.Value = customerId;

                    //prm = cmd.Parameters.Add("@name", SqlDbType.VarChar, 100);
                    //prm.Value = txtName.Text.Trim();

                    //prm = cmd.Parameters.Add("@email", SqlDbType.VarChar, 100);
                    //prm.Value = lblEmail.Text.Trim();

                    ////prm = cmd.Parameters.Add("@Address", SqlDbType.VarChar, 250);
                    ////prm.Value = address;

                    //prm = cmd.Parameters.Add("@cityid", SqlDbType.BigInt);
                    //prm.Value = SelectedCity;

                    //prm = cmd.Parameters.Add("@areaid", SqlDbType.BigInt);
                    //prm.Value = 0;
                    //prm = cmd.Parameters.Add("@mobile", SqlDbType.VarChar, 50);
                    //prm.Value = txtMobile.Text.Trim();

                    //prm = cmd.Parameters.Add("@ReceiveNewsletters", SqlDbType.Bit);
                    //prm.Value = chkNewsLetter.Checked;
                    //

                    //prm = cmd.Parameters.Add("@isverified", SqlDbType.Bit);
                    ////prm.Value = true;
                    //prm.Value = ;


                    cmd.Parameters.Add(DbFactory.GetDbParam("par_name", DbType.String, 100, txtName.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_email", DbType.String, 100, lblEmail.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_mobile", DbType.String, 50, txtMobile.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_customerid", DbType.Int64, customerId));

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbType.Int32, SelectedCity));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_areaid", DbType.Byte, 0));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_receivenewsletters", DbType.Int32, chkNewsLetter.Checked));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_isverified", DbType.Boolean, _isverified));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_phone1", DbType.String, 50, Convert.DBNull));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_address", DbType.String, 100, Convert.DBNull));
                    //Bikewale.Notifications.// LogLiveSps.LogSpInGrayLog(cmd);
                    MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.MasterDatabase);

                    returnVal = true;
                }
            }
            catch (SqlException err)
            {
                ErrorClass.LogError(err, Request.ServerVariables["URL"]);
                
                returnVal = false;
            } // catch SqlException
            catch (Exception err)
            {
                ErrorClass.LogError(err, Request.ServerVariables["URL"]);
                
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