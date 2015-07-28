using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using BikeWaleOpr.Common;
using System.Configuration;

namespace BikeWaleOpr.Content
{
    public class AddDealers : Page
    {
        protected DropDownList drpMake, drpState, drpCity;
        protected Button btnSave;
        protected TextBox txtName, txtAddress, txtPincode, txtContact, txtFax, txtEmail, txtWebsite, txtWorkingHours;
        protected Label lblMessage;
        protected CheckBox cbxIsActive, cbxIsNcd;
        protected Label lbl;

        public string SelectedCity
        {
            get
            {
                if (Request.Form["drpCity"] != null && Request.Form["drpCity"].ToString() != "")
                    return Request.Form["drpCity"].ToString();
                else
                    return "-1";
            }
        }

        public string CityContents
        {
            get
            {
                if (Request.Form["hdn_drpCity"] != null && Request.Form["hdn_drpCity"].ToString() != "")
                    return Request.Form["hdn_drpCity"].ToString();
                else
                    return "";
            }
        }

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
            btnSave.Click += new EventHandler(btnSave_Click);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                Ajax.Utility.RegisterTypeForAjax(typeof(AjaxFunctions));               

                FillMakes();
                FillStates();
                LoadDealers();
            }
            
        }
        void btnSave_Click(object Sender, EventArgs e)
        {
            if (SaveData())
            {
                lblMessage.Text = "Data Saved Successfully";
                ClearText();
            }
            else
            {
                lblMessage.Text = "You are trying to put duplicate entry.";
            }
        }

        //Fill states
        void FillStates()
        {
            string sql = "SELECT Name, Id FROM States WHERE IsDeleted = 0 ORDER BY Name";

            CommonOpn op = new CommonOpn();

            try
            {
                op.FillDropDown(sql, drpState, "Name", "Id");

                drpState.Items.Insert(0, new ListItem("--Select State--", "-1"));
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            } // catch Exception
        }

        //Fill Bike Makes
        void FillMakes()
        {
            string sql = "";

            sql = " SELECT Id, Name AS MakeName FROM BikeMakes WHERE IsDeleted = 0 ORDER BY MakeName";

            CommonOpn op = new CommonOpn();

            try
            {
                op.FillDropDown(sql, drpMake, "MakeName", "Id");
                drpMake.Items.Insert(0, new ListItem("--Select--", "-1"));
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message + err.Source);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

        void FillCities(string StateId)
        {
            string sql = "SELECT Name, Id FROM Cities WHERE IsDeleted = 0 and StateId = '" + StateId + "' ORDER BY Name";
            CommonOpn op = new CommonOpn();

            try
            {
                op.FillDropDown(sql, drpCity, "Name", "ID");

                drpCity.Items.Insert(0, new ListItem("--Select City--", "-1"));
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            } // catch Exception
        }

        void LoadDealers()
        {
            String URLData = Request.QueryString["id"];
            if (URLData != null)
            {
                btnSave.Text = "Update Details";
                SqlConnection con;
                Database db = new Database();
                CommonOpn op = new CommonOpn();
                string conStr = db.GetConString();
                con = new SqlConnection(conStr);
                SqlDataReader dr = null;
                lbl.Text = "Edit Dealer Details";
                
                try
                {
                    SqlCommand cmd = new SqlCommand("select ct.StateId,dl.MakeId,dl.CityId,dl.Name,dl.Address,dl.Pincode,dl.ContactNo,dl.FaxNo,"
                    +"dl.EmailId,dl.WebSite,dl.WorkingHours,dl.IsNCD,dl.IsActive from Dealer_NewBike dl, Cities ct where "
                    +"dl.Id = '" + URLData + "' and ct.ID = dl.CityId",con);
                    con.Open();
                    dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        drpMake.SelectedValue = dr["MakeId"].ToString();
                        drpState.SelectedValue = dr["StateId"].ToString();
                        drpCity.Enabled = true;
                        FillCities(dr["StateId"].ToString());
                        drpCity.SelectedValue = dr["CityId"].ToString();
                        txtName.Text = dr["Name"].ToString();
                        txtAddress.Text = dr["Address"].ToString();
                        txtPincode.Text = dr["Pincode"].ToString();
                        txtContact.Text = dr["ContactNo"].ToString();
                        txtFax.Text = dr["FaxNo"].ToString();
                        txtEmail.Text = dr["EmailId"].ToString();
                        txtWebsite.Text = dr["WebSite"].ToString();
                        txtWorkingHours.Text = dr["WorkingHours"].ToString();
                        if (dr["IsNCD"].ToString() == "1")
                        {
                            cbxIsNcd.Checked = true;
                        }
                        if (dr["IsActive"].ToString() == "1")
                        {
                            cbxIsActive.Checked = true;
                        }
                    }
                }

                catch (Exception ex)
                {
                    Response.Write(ex.Message);
                }

                finally
                {
                    if (con.State == ConnectionState.Open)
                        con.Close();
                }
            }
            else
            {
                lbl.Text = "Add Dealer Details";
            }
        }

        bool SaveData()
        {
            int isNcd = 0;
            int isActive = 0;
            if (cbxIsActive.Checked == true)
                isActive = 1;
            if (cbxIsNcd.Checked == true)
                isNcd = 1;
            bool isCompleted = true;
            string errM = "";
            SqlConnection con;
            SqlCommand cmd;
            SqlParameter prm;
            Database db = new Database();
            CommonOpn op = new CommonOpn();
	    string URLData;

            string conStr = db.GetConString();

            con = new SqlConnection(conStr);

            if(Request.QueryString["id"] == null)
            {
                URLData = "-1";
            }
	    
	    else
            {
                URLData = Request.QueryString["id"];
            } 

            try
                {
                    cmd = new SqlCommand("AddDealers", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    prm = cmd.Parameters.Add("@Id", SqlDbType.VarChar, 50);
                    prm.Value = URLData;

                    prm = cmd.Parameters.Add("@MakeId", SqlDbType.BigInt);
                    prm.Value = drpMake.SelectedItem.Value;

                    prm = cmd.Parameters.Add("@CityId", SqlDbType.BigInt);
                    prm.Value = SelectedCity;

                    prm = cmd.Parameters.Add("@Name", SqlDbType.VarChar, 100);
                    prm.Value = txtName.Text.Trim();

                    prm = cmd.Parameters.Add("@Address", SqlDbType.VarChar, 1000);
                    prm.Value = txtAddress.Text.Trim();

                    prm = cmd.Parameters.Add("@Pincode", SqlDbType.VarChar, 50);
                    prm.Value = txtPincode.Text.Trim();

                    prm = cmd.Parameters.Add("@ContactNo", SqlDbType.VarChar, 200);
                    prm.Value = txtContact.Text.Trim();

                    prm = cmd.Parameters.Add("@FaxNo", SqlDbType.VarChar, 50);
                    prm.Value = txtFax.Text.Trim();

                    prm = cmd.Parameters.Add("@EMailId", SqlDbType.VarChar, 100);
                    prm.Value = txtEmail.Text.Trim();

                    prm = cmd.Parameters.Add("@WebSite", SqlDbType.VarChar, 100);
                    prm.Value = txtWebsite.Text.Trim();

                    prm = cmd.Parameters.Add("@WorkingHours", SqlDbType.VarChar, 50);
                    prm.Value = txtWorkingHours.Text;

                    prm = cmd.Parameters.Add("@LastUpdated", SqlDbType.DateTime);
                    prm.Value = DateTime.Now;

                    prm = cmd.Parameters.Add("@IsNCD", SqlDbType.SmallInt);
                    prm.Value = isNcd;

                    prm = cmd.Parameters.Add("@IsActive", SqlDbType.SmallInt);
                    prm.Value = isActive;

                    con.Open();
                    //run the command
                    cmd.ExecuteNonQuery();
                }
                catch (SqlException err)
                {
                    errM = err.Message;
                    //catch the sql exception. if it is equal to 2627, then say that it is for duplicate entry 
                    ErrorClass objErr = new ErrorClass(err, "Vspl.Masters.NCDCommon");
                    objErr.SendMail();
                    isCompleted = false;
                    //throw;
                } // catch SqlException
                catch (Exception err)
                {
                    errM += err.Message;
                    ErrorClass objErr = new ErrorClass(err, "Vspl.Masters.NCDCommon");
                    objErr.SendMail();

                    isCompleted = false;
                    //throw;
                } // catch Exception
                finally
                {
                    //close the connection	
                    if (con.State == ConnectionState.Open)
                    {
                        con.Close();
                    }
                }
                return isCompleted;
            }

        void ClearText()
        {
            txtName.Text = "";
            txtAddress.Text = "";
            txtPincode.Text = "";
            txtContact.Text = "";
            txtFax.Text = "";
            txtEmail.Text = "";
            txtWebsite.Text = "";
            FillStates();
        }

    }
}
