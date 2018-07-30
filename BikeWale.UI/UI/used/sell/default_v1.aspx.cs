using Bikewale.Common;
using Bikewale.Controls;
using Bikewale.CV;
using Bikewale.Entities.BikeData;
using Enyim.Caching;
using MySql.CoreDAL;
using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Bikewale.Used
{
    /// <summary>
    ///     Created By : Ashish G. Kamble on 21/8/2012
    /// </summary>
    public class AboutBike : Page
    {
        protected DropDownList drpMake, drpModel, drpVersion, drpOwner;
        protected Repeater rptColors;
        protected TextBox txtKms, txtRegNo, txtRegAt, txtPrice, txtComments, txtColor, txtWaranties, txtModifications;
        protected Button btnContinue, btnUpdate;
        protected RadioButton btnTaxI, btnTaxC, rdoThirdParty, rdoComprehensive, rdoNoInsurance;
        protected HtmlGenericControl selectedColor, div_AboutYou;
        protected CheckBox chkTerms;
        protected TextBox txtName, txtEmail, txtMobile; // user's name, email-id and mobile
        protected DropDownList drpStates, drpCities; // user's state and city
        protected Int16 statusId = 4;

        // Html Controls
        protected HtmlGenericControl msgYourBike, msgOwner, msgBikeColor, msgKms, msgRegNo, msgRegAt,
                                     msgLifeTax, msgBikeIns, msgPrice, msgName, msgEmail, msgMobile,
                                     msgMakeYear, msgCity, msgTerms, div_sellBike, div_NotAuthorised,
                                     msgValidTill, div_FakeCustomer;

        protected HtmlInputHidden hdn_drpSelectedVersion, hdn_drpSelectedCity;
        protected HtmlSelect calMakeYear_cmbMonth, calValidTill_cmbMonth;

        //Other Controls
        protected DateControl calMakeYear, calValidTill;

        // variables
        protected bool askLogin = false;
        protected string inquiryId = "-1";
        protected string customerId = string.Empty;

        bool _isMemcachedUsed;
        protected static MemcachedClient _mc = null;

        public AboutBike()
        {
            _isMemcachedUsed = bool.Parse(ConfigurationManager.AppSettings.Get("IsMemcachedUsed"));
            if (_mc == null)
            {
                InitializeMemcached();
            }
        }

        #region Initialize Memcache
        private void InitializeMemcached()
        {
            _mc = new MemcachedClient("memcached");
        }
        #endregion

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
            this.btnContinue.Click += new EventHandler(btnContinue_Click);
            //this.btnUpdate.Click += new EventHandler(btnContinue_Click);
        }

        void Page_Load(object Sender, EventArgs e)
        {
            div_NotAuthorised.Visible = false;
            div_FakeCustomer.Visible = false;

            if (!String.IsNullOrEmpty(Request.QueryString["id"]))
            {
                if (CurrentUser.Id != "-1")
                {
                    inquiryId = Request.QueryString["id"];
                }
                else
                {
                    Response.Redirect("/users/login.aspx?ReturnUrl=/used/sell/default.aspx?id=" + Request.QueryString["id"]);
                }
            }

            if (!IsPostBack)
            {
                Trace.Warn("Valid Email" + txtEmail.Text.Trim().ToLower());
                if (inquiryId != "-1")
                    PrefillData();

                FillStates();
                FillMakes();
            }
        }   // End of page_load

        /// <summary>
        ///     Function to prefill the data if inquiry id is passed
        /// </summary>
        protected void PrefillData()
        {
            DataTable dt = GetUserData();

            if (dt != null && dt.Rows.Count > 0)
            {
                //div_AboutYou.Visible = false;
                statusId = Convert.ToInt16(dt.Rows[0]["StatusId"]);
                drpMake.SelectedValue = dt.Rows[0]["MakeId"].ToString() + "_" + dt.Rows[0]["MakeMaskingName"].ToString();

                FillModels(dt.Rows[0]["MakeId"].ToString());
                drpModel.SelectedValue = dt.Rows[0]["ModelId"].ToString();
                drpModel.Enabled = true;

                FillVesions(dt.Rows[0]["ModelId"].ToString());
                drpVersion.SelectedValue = dt.Rows[0]["VersionId"].ToString();
                drpVersion.Enabled = true;
                hdn_drpSelectedVersion.Value = drpVersion.SelectedValue + "|" + drpVersion.SelectedItem.Text;
                Trace.Warn("hdn : ", hdn_drpSelectedVersion.Value);

                calMakeYear.Value = String.IsNullOrEmpty(dt.Rows[0]["MakeYear"].ToString()) ? DateTime.Now : Convert.ToDateTime(dt.Rows[0]["MakeYear"].ToString());

                drpOwner.SelectedValue = dt.Rows[0]["Owner"].ToString();
                txtColor.Text = dt.Rows[0]["Color"].ToString();
                txtKms.Text = dt.Rows[0]["Kilometers"].ToString();

                drpStates.SelectedValue = dt.Rows[0]["StateId"].ToString();

                FillCities(dt.Rows[0]["StateId"].ToString());
                drpCities.SelectedValue = dt.Rows[0]["CityId"].ToString();
                drpCities.Enabled = true;
                hdn_drpSelectedCity.Value = drpCities.SelectedValue + "|" + drpCities.SelectedItem.Text;
                Trace.Warn("hdn city : ", hdn_drpSelectedCity.Value);

                txtRegNo.Text = dt.Rows[0]["BikeRegNo"].ToString();
                txtRegAt.Text = dt.Rows[0]["RegistrationPlace"].ToString();

                if (dt.Rows[0]["InsuranceType"].ToString() == "No Insurance")
                {
                    rdoNoInsurance.Checked = true;
                }
                else if (dt.Rows[0]["InsuranceType"].ToString() == "Third Party")
                {
                    rdoThirdParty.Checked = true;
                    calValidTill.Value = Convert.ToDateTime(dt.Rows[0]["InsuranceExpiryDate"].ToString());
                }
                else if (dt.Rows[0]["InsuranceType"].ToString() == "Comprehensive")
                {
                    rdoComprehensive.Checked = true;
                    calValidTill.Value = Convert.ToDateTime(dt.Rows[0]["InsuranceExpiryDate"].ToString());
                }

                if (dt.Rows[0]["LifetimeTax"].ToString() == "Individual")
                {
                    btnTaxI.Checked = true;
                }
                else if (dt.Rows[0]["LifetimeTax"].ToString() == "Corporate")
                {
                    btnTaxC.Checked = true;
                }

                txtPrice.Text = dt.Rows[0]["Price"].ToString();
                txtComments.Text = dt.Rows[0]["Comments"].ToString();

                txtEmail.Text = dt.Rows[0]["CustomerEmail"].ToString().ToLower();
                txtMobile.Text = dt.Rows[0]["CustomerMobile"].ToString();
                txtName.Text = dt.Rows[0]["CustomerName"].ToString();
                chkTerms.Checked = true;
            }
            else
            {
                div_NotAuthorised.Visible = true;
                div_sellBike.Attributes.Add("class", "hide");
            }
        }   // End of prefill data

        private DataTable GetUserData()
        {

            DataTable dt = null;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "getclassifiedindividualsellinquiriesdetails_sp";

                    //cmd.Parameters.Add("@InquiryId", SqlDbType.VarChar, 10).Value = inquiryId;
                    //cmd.Parameters.Add("@RequestType", SqlDbType.VarChar, 20).Value = "DETAILS";
                    //cmd.Parameters.Add("@CustomerId", SqlDbType.BigInt).Value = CurrentUser.Id;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_inquiryid", DbType.String, 10, inquiryId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_requesttype", DbType.String, 20, "DETAILS"));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_customerid", DbType.Int64, CurrentUser.Id));

                    using (DataSet ds = MySqlDatabase.SelectAdapterQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
                        {
                            dt = ds.Tables[0];
                        }
                    }


                }
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Trace.Warn("GetUserData sql ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("GetUserData ex: " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

            return dt;
        }   // End of GetUserData function

        /// <summary>
        ///     Function to fill states dropdownlist
        /// </summary>
        protected void FillStates()
        {
            Trace.Warn("inside fill states");
            StateCity obj = new StateCity();
            DataTable dt = obj.GetStates();

            if (dt != null)
            {
                drpStates.DataSource = dt;
                drpStates.DataTextField = "Text";
                drpStates.DataValueField = "Value";
                drpStates.DataBind();
            }

            drpStates.Items.Insert(0, new ListItem("--Select State--", "0"));
        }   // end of fillstates method

        /// <summary>
        /// Function to fill cities dropdownlist for selected state
        /// </summary>
        /// <param name="stateId"></param>
        protected void FillCities(string stateId)
        {
            Trace.Warn("inside fill cities");
            StateCity obj = new StateCity();
            DataTable dt = obj.GetCities(stateId, "ALL");

            if (dt != null)
            {
                drpCities.DataSource = dt;
                drpCities.DataTextField = "Text";
                drpCities.DataValueField = "Value";
                drpCities.DataBind();
            }

            drpCities.Items.Insert(0, new ListItem("--Select City--", "0"));


            //using (SqlCommand cmd = new SqlCommand("GetCitiesWithMappingName"))
            //{       
            //    Database db = null;
            //    cmd.CommandType = CommandType.StoredProcedure;
            //    cmd.Parameters.Add("@RequestType", SqlDbType.VarChar, 20).Value = "ALL";
            //    cmd.Parameters.Add("@StateId", SqlDbType.BigInt).Value = stateId;

            //    SqlDataReader dr = null;
            //    try
            //    {
            //        db = new Database();

            //        dr = db.SelectQry(cmd);

            //        drpCities.DataTextField = "Text";
            //        drpCities.DataValueField = "Value";
            //        drpCities.DataSource = dr;
            //        drpCities.DataBind();

            //        drpCities.Items.Insert(0, new ListItem("--Select City--", "0"));
            //    }
            //    catch (Exception ex)
            //    {
            //        Trace.Warn(ex.Message);
            //        ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
            //        objErr.SendMail();
            //    }
            //    finally
            //    {
            //        if (dr != null)
            //        {
            //            dr.Close();
            //        }
            //        db.CloseConnection();
            //    }
            //}
        }   // end of fillstates method

        /// <summary>
        ///     Function to fill the makes dropdownlist
        /// </summary>
        protected void FillMakes()
        {
            MakeModelVersion mmv = new MakeModelVersion();

            //DataTable dt = mmv.GetMakes("USED");

            //if (dt != null)
            //{
            //    drpMake.DataSource = dt;
            //    drpMake.DataTextField = "Text";
            //    drpMake.DataValueField = "Value";
            //    drpMake.DataBind();
            //}

            //drpMake.Items.Insert(0, new ListItem("--Select Make--", "0"));

            mmv.GetMakes(EnumBikeType.Used, ref drpMake);
        }

        /// <summary>
        ///     Function to fill the models dropdownlist
        /// </summary>
        protected void FillModels(string makeId)
        {
            MakeModelVersion mmv = new MakeModelVersion();

            DataTable dt = mmv.GetModels(makeId, "USED");

            if (dt != null)
            {
                drpModel.DataSource = dt;
                drpModel.DataTextField = "Text";
                drpModel.DataValueField = "Value";
                drpModel.DataBind();
            }

            drpModel.Items.Insert(0, new ListItem("--Select Model--", "0"));
        }

        /// <summary>
        ///     Function to fill the versions dropdownlist
        /// </summary>
        protected void FillVesions(string modelId)
        {
            MakeModelVersion mmv = new MakeModelVersion();

            DataTable dt = mmv.GetVersions(modelId, "USED");

            if (dt != null)
            {
                drpVersion.DataSource = dt;
                drpVersion.DataTextField = "Text";
                drpVersion.DataValueField = "Value";
                drpVersion.DataBind();
            }

            drpVersion.Items.Insert(0, new ListItem("--Select Version--", "0"));
        }

        void btnContinue_Click(object Sender, EventArgs e)
        {
            Trace.Warn("btncontinue clicked !!");

            Trace.Warn("ValidateBikeUserDetails() : ", ValidateBikeUserDetails().ToString());
            if (ValidateBikeUserDetails())
            {
                SaveSellBikeInfo();

                if (inquiryId != "-1")
                {
                    // Do mobile verification of the customer
                    CustomerVerification objCV = new CustomerVerification();

                    bool isVerified = objCV.IsMobileVerified(txtName.Text.Trim(), txtEmail.Text.Trim(), txtMobile.Text.Trim());
                    Trace.Warn("isVerified : ", isVerified.ToString());

                    // Assign customer information to cookies for further use
                    CookiesCustomers.Mobile = txtMobile.Text.Trim();
                    CookiesCustomers.Email = txtEmail.Text.Trim();
                    CookiesCustomers.CustomerName = txtName.Text.Trim();
                    CookiesCustomers.CityId = SelectedCityId;
                    CookiesCustomers.SellInquiryId = inquiryId;
                    CookiesCustomers.CustomerId = customerId;

                    if (isVerified)
                    {
                        SellBikeCommon common = new SellBikeCommon();
                        RegisterCustomer objRC = new RegisterCustomer();

                        // Update customer mobile number into customers table.
                        objRC.UpdateCustomerMobile(txtMobile.Text.Trim(), txtEmail.Text.Trim(), txtName.Text.Trim());

                        // Update statusId = 1; in SellIndividualInquiries 
                        common.UpdateIsVerifiedCustomer(inquiryId);

                        //this line invalidate memcache to get updated used bike count from live listing
                        _mc.Remove("BW_ModelWiseUsedBikesCount");

                        Response.Redirect("uploadbasic.aspx");
                    }
                    else
                    {
                        Response.Redirect("verify.aspx");
                    }
                }
                else
                {
                    //Response.Redirect("default.aspx");
                    div_FakeCustomer.Visible = true;
                    div_sellBike.Attributes.Add("class", "hide");
                }
            }
        }

        /// <summary>
        ///     Function to save the customer bike basic info and customer info into database
        ///     Function will set inquiryId for the customer
        ///     Modified By : Sadhana Upadhyay on 2nd April 2014
        ///     Summary : To capture Client IP.
        ///     Modified by :   Sumit Kate on 22 Sep 2016
        ///     Description :   Corrected insurance expiry date parsing
        /// </summary>
        protected void SaveSellBikeInfo()
        {

            try
            {
                RegisterCustomer objCust = new RegisterCustomer();
                customerId = objCust.RegisterUser(txtName.Text.Trim(), txtEmail.Text.Trim(), txtMobile.Text.Trim(), "", "", SelectedCityId);

                Trace.Warn("Customer id ", customerId);

                DateTime _bikemkyear = new DateTime(), _insexpiry = new DateTime();
                DateTime.TryParse(calMakeYear.Value.ToString(), out _bikemkyear);
                DateTime.TryParse(InsExp, out _insexpiry);

                if (!objCust.IsFakeCustomer(Convert.ToInt32(customerId)))
                {
                    using (DbCommand cmd = DbFactory.GetDBCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "saveclassifiedindividualsellbikeinquiries_sp";

                        // Bikewale.Notifications.// LogLiveSps.LogSpInGrayLog(cmd);
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_bikeversionid", DbType.Int32, SelectedVersion));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_bikemakeyear", DbType.DateTime, _bikemkyear));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_bikeowner", DbType.Byte, drpOwner.SelectedValue));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_bikecolor", DbType.String, 100, txtColor.Text.Trim()));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_kilometers", DbType.Int32, txtKms.Text.Trim()));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbType.Int16, SelectedCityId));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_registrationno", DbType.String, 50, txtRegNo.Text.Trim()));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_registrationplace", DbType.String, 50, txtRegAt.Text.Trim()));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_lifetimetax", DbType.String, 20, GetLifetimeTax));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_insurancetype", DbType.String, 20, BikeInsurance));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_insuranceexpirydate", DbType.DateTime, BikeInsurance == "No Insurance" ? Convert.DBNull : _insexpiry));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_expectedprice", DbType.Int64, txtPrice.Text.Trim()));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_comments", DbType.String, 250, txtComments.Text.Trim() == "Your Comments" ? Convert.DBNull : txtComments.Text.Trim()));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_modifications", DbType.String, 250, String.IsNullOrEmpty(txtModifications.Text.Trim()) ? Convert.DBNull : txtModifications.Text.Trim()));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_warranties", DbType.String, 250, String.IsNullOrEmpty(txtWaranties.Text.Trim()) ? Convert.DBNull : txtWaranties.Text.Trim()));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_sourceid", DbType.Byte, ConfigurationManager.AppSettings["sourceId"].ToString()));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_name", DbType.String, 50, txtName.Text.Trim()));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_email", DbType.String, 100, txtEmail.Text.Trim().ToLower()));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_mobile", DbType.String, 20, txtMobile.Text.Trim()));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_inquiryid", DbType.Int64, ParameterDirection.InputOutput));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_customerid", DbType.Int64, customerId));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_clientip", DbType.String, 40, CommonOpn.GetClientIP()));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_statusid", DbType.Byte, statusId));
                        cmd.Parameters["par_inquiryid"].Value = inquiryId;

                        MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.MasterDatabase);

                        inquiryId = cmd.Parameters["par_inquiryid"].Value.ToString();

                    }
                }
                else
                {
                    Trace.Warn("Not Authorized");
                }
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Trace.Warn("SaveSellBikeInfo sql ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("SaveSellBikeInfo ex: " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }


        /// <summary>
        /// Created by  :   Bikewale Dev
        /// Description :   Validate the input by the user - Bike and User Details
        /// Modified by :   Sumit Kate on 22 Sep 2016
        /// Description :   Enabled Make Year and Insurance valid date server side validation
        /// </summary>
        /// <returns></returns>
        bool ValidateBikeUserDetails()
        {
            bool isError = false;

            if (String.IsNullOrEmpty(hdn_drpSelectedVersion.Value))
            {
                isError = true;
                msgYourBike.InnerText = "Required!";
            }
            else { msgYourBike.InnerText = string.Empty; }

            //Make Year
            DateTime makeYear = calMakeYear.Value;
            DateTime dt = DateTime.Now;
            if (DateTime.Compare(makeYear, dt) > 0)
            {
                msgMakeYear.InnerText = "Enter Correct Make Year.";
            }
            else { msgMakeYear.InnerText = string.Empty; }

            // Owner			
            if (drpOwner.SelectedIndex == 0)
            {
                isError = true;
                msgOwner.InnerText = "Required!";
            }
            else { msgOwner.InnerText = string.Empty; }


            // Color
            if (txtColor.Text.Trim() == string.Empty)
            {
                isError = true;
                msgBikeColor.InnerText = "Required!";
            }
            else { msgBikeColor.InnerText = string.Empty; }


            // Kilometers
            if (String.IsNullOrEmpty(txtKms.Text.Trim()))
            {
                isError = true;
                msgKms.InnerText = "Required!";
            }
            else if (!Regex.IsMatch(txtKms.Text.Trim(), @"^[0-9]+$"))
            {
                isError = true;
                msgKms.InnerText = "Invalid kilometers. It should be numeric only!";
            }
            else { msgKms.InnerText = string.Empty; }


            // Registration Number
            if (txtRegNo.Text.Trim() == string.Empty)
            {
                isError = true;
                msgRegNo.InnerText = "Required!";
            }
            else { msgRegNo.InnerText = string.Empty; }


            // Registration @
            if (txtRegAt.Text.Trim() == string.Empty)
            {
                isError = true;
                msgRegAt.InnerText = "Required!";
            }
            else { msgRegAt.InnerText = string.Empty; }


            // Tax
            if (!btnTaxI.Checked && !btnTaxC.Checked)
            {
                isError = true;
                msgLifeTax.InnerText = "Required!";
            }
            else { msgLifeTax.InnerText = string.Empty; }


            // Insurance
            if (!rdoThirdParty.Checked && !rdoComprehensive.Checked && !rdoNoInsurance.Checked)
            {
                isError = true;
                msgBikeIns.InnerText = "Required!";
            }
            else { msgBikeIns.InnerText = string.Empty; }

            //Insurance Valid Till
            if (!rdoNoInsurance.Checked && (rdoThirdParty.Checked || rdoComprehensive.Checked))
            {
                DateTime validTill = calValidTill.Value;
                if (DateTime.Compare(calMakeYear.Value, validTill) > 0)
                {
                    msgValidTill.InnerText = "Enter Correct Insurance Validity Date.";
                }
                else { msgValidTill.InnerText = string.Empty; }
            }
            else { msgValidTill.InnerText = string.Empty; }

            // Price
            if (txtPrice.Text.Trim() == string.Empty)
            {
                isError = true;
                msgPrice.InnerText = "Required!";
            }
            else if (!Regex.IsMatch(txtPrice.Text.Trim(), @"^[0-9]+$"))
            {
                isError = true;
                msgPrice.InnerText = "Invalid Price! It should be numberic only!";
            }
            else { msgPrice.InnerText = string.Empty; }


            // Validate Name
            if (txtName.Text.Trim() == string.Empty)
            {
                isError = true;
                msgName.InnerText = "Required!";
            }
            else { msgName.InnerText = string.Empty; }


            // Validate Email
            if (string.IsNullOrEmpty(txtEmail.Text.Trim()))
            {
                isError = true;
                msgEmail.InnerText = "Required!";
            }
            else { msgEmail.InnerText = string.Empty; }


            // Validate Mobile Number
            if (txtMobile.Text.Trim() == string.Empty)
            {
                isError = true;
                msgMobile.InnerText = "Required!";
            }
            else { msgMobile.InnerText = string.Empty; }

            // Validate City
            if (SelectedCityId == "-1" || SelectedCityId == "")
            {
                isError = true;
                msgCity.InnerText = "Required!";
            }
            else { msgCity.InnerText = string.Empty; }

            // Terms &  Conditions
            if (!chkTerms.Checked)
            {
                isError = true;
                msgTerms.InnerText = "You must agree to the terms & conditions before poceeding!";
            }
            else { msgTerms.InnerText = string.Empty; }

            return !isError;
        }

        public string SelectedModel
        {
            get
            {
                if (Request.Form["drpModel"] != null && Request.Form["drpModel"].ToString() != "")
                    return Request.Form["drpModel"].ToString();
                else
                    return "-1";
            }
        }

        public string ModelContents
        {
            get
            {
                if (Request.Form["hdn_drpModel"] != null && Request.Form["hdn_drpModel"].ToString() != "")
                    return Request.Form["hdn_drpModel"].ToString();
                else
                    return "";
            }
        }

        public string ModelName
        {
            get
            {
                if (Request.Form["hdn_drpModelName"] != null && Request.Form["hdn_drpModelName"].ToString() != "")
                    return Request.Form["hdn_drpModelName"].ToString();
                else
                    return "";
            }
        }

        public string SelectedVersion
        {
            get
            {
                if (!String.IsNullOrEmpty(hdn_drpSelectedVersion.Value))
                    return hdn_drpSelectedVersion.Value.Split('|')[0];
                else
                    return "-1";
            }
        }

        public string VersionContents
        {
            get
            {
                if (Request.Form["hdn_drpVersion"] != null && Request.Form["hdn_drpVersion"].ToString() != "")
                    return Request.Form["hdn_drpVersion"].ToString();
                else
                    return "";
            }
        }

        public string VersionName
        {
            get
            {
                if (Request.Form["hdn_drpVersionName"] != null && Request.Form["hdn_drpVersionName"].ToString() != "")
                    return Request.Form["hdn_drpVersionName"].ToString();
                else
                    return "";
            }
        }

        public string BikeColor
        {
            get
            {
                /*if(Request.Form["hdnBikeColor"] != null && Request.Form["hdnBikeColor"].ToString() != "")
                {
                    string[] arrColor = Request.Form["hdnBikeColor"].ToString().Split('|');
                    return arrColor[0];
                }
                else
                    return "";*/

                return txtColor.Text.Trim();
            }
        }

        public string BikeColorCode
        {
            get
            {
                /*if(Request.Form["hdnBikeColor"] != null && Request.Form["hdnBikeColor"].ToString() != "")
                {
                    string[] arrColor = Request.Form["hdnBikeColor"].ToString().Split('|');
                    return arrColor[1];
                }
                else*/

                return "";
            }
        }

        public string SelectedCityId
        {
            get
            {
                if (!String.IsNullOrEmpty(hdn_drpSelectedCity.Value))
                    return hdn_drpSelectedCity.Value.Split('|')[0].ToString();
                else
                    return "-1";
            }
        }

        public string SelectedStateId
        {
            get
            {
                if (Request.Form["drpStates"] != null && Request.Form["drpStates"].ToString() != "")
                    return Request.Form["drpStates"].ToString();
                else
                    return "-1";
            }
        }

        public string CityContents
        {
            get
            {
                if (Request.Form["hdn_drpCities"] != null && Request.Form["hdn_drpCities"].ToString() != "")
                    return Request.Form["hdn_drpCities"].ToString();
                else
                    return "";
            }
        }

        public string CityName
        {
            get
            {
                if (Request.Form["hdn_drpCity"] != null && Request.Form["hdn_drpCity"].ToString() != "")
                    return Request.Form["hdn_drpCity"].ToString();
                else
                    return "";
            }
        }

        public string BikeInsurance
        {
            get
            {
                string ins = "";

                if (rdoThirdParty.Checked)
                {
                    ins = rdoThirdParty.Text;
                }
                else if (rdoComprehensive.Checked)
                {
                    ins = rdoComprehensive.Text;
                }
                else if (rdoNoInsurance.Checked)
                {
                    ins = rdoNoInsurance.Text;
                }

                return ins;
            }
        }

        public string GetLifetimeTax
        {
            get
            {
                string tax = "";

                if (btnTaxI.Checked == true)
                {
                    tax = btnTaxI.Text;
                }
                else if (btnTaxC.Checked == true)
                {
                    tax = btnTaxC.Text;
                }

                return tax;
            }
        }

        public string InsExp
        {
            get
            {
                string ins = "";

                if (!rdoNoInsurance.Checked)
                {
                    ins = calValidTill.Value.ToString();
                }

                return ins;
            }
        }

    }   // End of class
}   // End of namespace