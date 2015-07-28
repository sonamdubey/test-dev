using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Bikewale.Common;
namespace Bikewale.Controls
{
    public class UpcomingBikeSearch : System.Web.UI.UserControl
    {
        protected DropDownList drpMake;
        protected HtmlSelect drpUCSortList;
        private DataSet _makeContents = null;
        private string _makeId = "-1", _sort = string.Empty;
        
        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(this.Page_Load);
        }
	
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadMakes();
            }
        }

        void LoadMakes()
        {
            DataSet dsMakeContents = new DataSet();
            string sql = string.Empty;
            SqlCommand cmd = new SqlCommand();
            Database db = new Database();

            sql = " Select Distinct cast(Ma.ID as varchar) + '_' + Ma.MaskingName As Id, Name As Name From ExpectedBikeLaunches ECL With(NoLock) "
                + " Inner Join BikeMakes Ma With(NoLock) On Ma.ID = ECL.BikeMakeId "
                + " Where ECL.IsLaunched = 0 AND Ma.IsDeleted = 0 AND ( Ma.Futuristic = 1 OR Ma.New = 1)";

            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;

            try
            {
                dsMakeContents = db.SelectAdaptQry(cmd);
                if (dsMakeContents != null && dsMakeContents.Tables[0].Rows.Count > 0)
                {
                    drpMake.DataSource = dsMakeContents;
                    drpMake.DataTextField = "Name";
                    drpMake.DataValueField = "Id";
                    drpMake.DataBind();
                    drpMake.Items.Insert(0, new ListItem("--Select Makes--", "0"));
                }
                if (MakeId != "" && MakeId != "-1")
                {
                    drpMake.SelectedIndex = drpMake.Items.IndexOf(drpMake.Items.FindByValue(MakeId + '_' + Request.QueryString["make"]));
                }
                if (Sort != string.Empty)
                {
                    drpUCSortList.SelectedIndex = drpUCSortList.Items.IndexOf(drpUCSortList.Items.FindByValue(Sort));
                }
            }
            catch (SqlException ex)
            {
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                db.CloseConnection();
                cmd.Dispose();
            }
        }

        public DataSet MakeContents
        {
            get
            {
                return _makeContents;
            }
            set
            {
                _makeContents = value;
            }
        }

        public string MakeId
        {
            get
            {
                return _makeId;
            }
            set
            {
                _makeId = value;
            }
        }

        public string Sort
        {
            get
            {
                return _sort;
            }
            set
            {
                _sort = value;
            }
        }
    }
}