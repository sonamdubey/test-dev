using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bikewale.Common;
using System.Data.SqlClient;

namespace Bikewale
{
    public class Default : Page
    {
        protected DropDownList  ddlUsedCities;

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DeviceDetection dd = new DeviceDetection();
                dd.DetectDevice();

                BindUsedBikeCities();
            }
        }

        private void BindUsedBikeCities()
        {
            //DataTable dt = null;

            //StateCity obj = new StateCity();
            //dt = obj.GetCities("used");

            //if (dt.Rows.Count > 0)
            //{
            //    ddlUsedCities.DataTextField = "Text";
            //    ddlUsedCities.DataValueField = "Value";
            //    ddlUsedCities.DataSource = dt;
            //    ddlUsedCities.DataBind();

            //    //also prepend the list with the values of top cites
            //    ddlUsedCities.Items.Insert(0, new ListItem("------------", "-1"));
            //    ddlUsedCities.Items.Insert(0, new ListItem("Hyderabad", "105"));
            //    ddlUsedCities.Items.Insert(0, new ListItem("Pune", "12"));
            //    ddlUsedCities.Items.Insert(0, new ListItem("Ahmedabad", "128"));
            //    ddlUsedCities.Items.Insert(0, new ListItem("Kolkata", "198"));
            //    ddlUsedCities.Items.Insert(0, new ListItem("Chennai", "176"));
            //    ddlUsedCities.Items.Insert(0, new ListItem("Bangalore", "2"));
            //    ddlUsedCities.Items.Insert(0, new ListItem("New Delhi", "10"));
            //    ddlUsedCities.Items.Insert(0, new ListItem("Mumbai", "1"));
            //    ddlUsedCities.Items.Insert(0, new ListItem("------------", "-1"));

            //    ddlUsedCities.Items.Insert(0, new ListItem("--Select City--", "-1")); 
            //}

            using (SqlCommand cmd = new SqlCommand("GetCitiesWithMappingName"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@RequestType", SqlDbType.VarChar, 20).Value = "USED";
                Database db = new Database();
                SqlDataReader dr = null;
                try
                {
                    dr = db.SelectQry(cmd);
                    ddlUsedCities.DataTextField = "Text";
                    ddlUsedCities.DataValueField = "Value";
                    ddlUsedCities.DataSource = dr;
                    ddlUsedCities.DataBind();

                    // adding by default main cities at top
                    ddlUsedCities.Items.Insert(0, new ListItem("-----------", "-1"));
                    ddlUsedCities.Items.Insert(0, new ListItem("Hyderabad", "105_hyderabad"));
                    ddlUsedCities.Items.Insert(0, new ListItem("Pune", "12_pune"));
                    ddlUsedCities.Items.Insert(0, new ListItem("Ahmedabad", "128_ahmedabad"));
                    ddlUsedCities.Items.Insert(0, new ListItem("Kolkata", "198_kolkata"));
                    ddlUsedCities.Items.Insert(0, new ListItem("Chennai", "176_chennai"));
                    ddlUsedCities.Items.Insert(0, new ListItem("Bangalore", "2_bangalore"));
                    ddlUsedCities.Items.Insert(0, new ListItem("New Delhi", "10_newdelhi"));
                    ddlUsedCities.Items.Insert(0, new ListItem("Mumbai", "1_mumbai"));
                    ddlUsedCities.Items.Insert(0, new ListItem("-----------", "-1"));
                    ddlUsedCities.Items.Insert(0, new ListItem("--Select City--", "0"));

                    // drpCity.Items.FindByValue("-1").Enabled = false;
                }
                catch (Exception ex)
                {
                    Trace.Warn(ex.Message);
                    ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                    objErr.SendMail();
                }
                finally
                {
                    if (dr != null)
                    {
                        dr.Close();
                    }
                    db.CloseConnection();
                }
            }


        }
    }
}