using BikeWaleOpr.Common;
using MySql.CoreDAL;
using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

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
            string sql = "select Name, Id from states where isdeleted = 0 order by name";

            CommonOpn op = new CommonOpn();

            try
            {
                op.FillDropDown(sql, drpState, "Name", "Id");

                drpState.Items.Insert(0, new ListItem("--Select State--", "-1"));
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message);
                ErrorClass.LogError(err, Request.ServerVariables["URL"]);
                
            } // catch Exception
        }

        //Fill Bike Makes
        void FillMakes()
        {
            string sql = "";

            sql = " SELECT Id, name as MakeName from bikemakes where isdeleted = 0 order by makename";

            CommonOpn op = new CommonOpn();

            try
            {
                op.FillDropDown(sql, drpMake, "MakeName", "Id");
                drpMake.Items.Insert(0, new ListItem("--Select--", "-1"));
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message + err.Source);
                ErrorClass.LogError(err, Request.ServerVariables["URL"]);
                
            }
        }

        void FillCities(string StateId)
        {
            string sql = "select Name, Id from cities where isdeleted = 0 and stateid = '" + StateId + "' order by name";
            CommonOpn op = new CommonOpn();

            try
            {
                op.FillDropDown(sql, drpCity, "Name", "ID");

                drpCity.Items.Insert(0, new ListItem("--Select City--", "-1"));
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message);
                ErrorClass.LogError(err, Request.ServerVariables["URL"]);
                
            } // catch Exception
        }

        void LoadDealers()
        {
            String URLData = Request.QueryString["id"];
            if (URLData != null)
            {
                btnSave.Text = "Update Details";

                CommonOpn op = new CommonOpn();

                lbl.Text = "Edit Dealer Details";
                string sql = @"select ct.stateid,dl.makeid,dl.cityid,dl.name,dl.address,dl.pincode,dl.contactno,dl.faxno,
                                dl.emailid,dl.website,dl.workinghours,if(dl.isncd,1,0) isncd,if(dl.isactive,1,0) isactive 
                                from dealer_newbike dl, cities ct where 
                                dl.id = '" + URLData + "' and ct.id = dl.cityid";
                try
                {
                    using (DbCommand cmd = DbFactory.GetDBCommand(sql))
                    {
                        using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                        {
                            if (dr != null)
                            {
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
                        }
                    }
                }

                catch (Exception ex)
                {
                    Response.Write(ex.Message);
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

            CommonOpn op = new CommonOpn();
            string URLData;


            if (Request.QueryString["id"] == null)
            {
                URLData = "-1";
            }

            else
            {
                URLData = Request.QueryString["id"];
            }

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("adddealers"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_id", DbType.String, 50, URLData));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makeid", DbType.Int64, drpMake.SelectedItem.Value));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbType.Int64, SelectedCity));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_name", DbType.String, 100, txtName.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_address", DbType.String, 1000, txtAddress.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_pincode", DbType.String, 50, txtPincode.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_contactno", DbType.String, 200, txtContact.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_faxno", DbType.String, 50, txtFax.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_emailid", DbType.String, 100, txtEmail.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_website", DbType.String, 100, txtWebsite.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_workinghours", DbType.String, 50, txtWorkingHours.Text));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_lastupdated", DbType.DateTime, DateTime.Now));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_isncd", DbType.Int16, isNcd));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_isactive", DbType.Int16, isActive));

                    //run the command
                    MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.MasterDatabase);

                    isCompleted = true;
                }
            }
            catch (SqlException err)
            {
                errM = err.Message;
                //catch the sql exception. if it is equal to 2627, then say that it is for duplicate entry 
                ErrorClass.LogError(err, "Vspl.Masters.NCDCommon");
                
                isCompleted = false;
                //throw;
            } // catch SqlException
            catch (Exception err)
            {
                errM += err.Message;
                ErrorClass.LogError(err, "Vspl.Masters.NCDCommon");
                

                isCompleted = false;
                //throw;
            } // catch Exception

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
