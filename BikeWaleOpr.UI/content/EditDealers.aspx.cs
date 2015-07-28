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
    public partial class EditDealers : Page
    {
        protected DropDownList drpMake, drpState, drpCity;
        protected Button btnFind;
        protected TextBox txtName, txtAddress, txtPincode, txtContact, txtFax, txtEmail, txtWebsite, txtWorkingHours;
        protected Label lblMessage;
        protected CheckBox cbxIsActive, cbxIsNcd;
        protected Repeater MyRepeater;
        protected HtmlInputHidden hdn_drpCity;

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
            btnFind.Click += new EventHandler(btnFind_Click);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            CommonOpn op = new CommonOpn();
            if (!IsPostBack)
            {
                Ajax.Utility.RegisterTypeForAjax(typeof(AjaxFunctions));
                FillMakes();
                FillStates();
            }
            else {
                AjaxFunctions aj = new AjaxFunctions();
                aj.UpdateContents(drpCity, hdn_drpCity.Value, Request.Form["drpCity"]);               
            }            
        }

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

        void btnFind_Click(object Sender, EventArgs e)
        {           
            string sql = " SELECT"
                       + " Id,MakeId,CityId,Name,Address,Pincode,ContactNo,"
                       + " FaxNo,EMailId,WebSite,WorkingHours,LastUpdated,IsActive,IsNCD"
                       + " FROM Dealer_NewBike where MakeId='" + drpMake.SelectedItem.Value + "' and CityId='" + drpCity.SelectedItem.Value + "'";
                       
            SqlConnection con;
            Database db = new Database();
            CommonOpn op = new CommonOpn();

            string conStr = db.GetConString();
            con = new SqlConnection(conStr);
            try
            {
      
                SqlDataAdapter da = new SqlDataAdapter(sql, con);
                DataSet ds = new DataSet();
                da.Fill(ds);
                MyRepeater.DataSource = ds;
                MyRepeater.DataBind();
                if (ds.Tables[0].Rows.Count == 0)
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('No record found');", true);
                }
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message + err.Source);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }

        }
    }
}