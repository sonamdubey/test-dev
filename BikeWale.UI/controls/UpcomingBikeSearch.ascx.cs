using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Bikewale.Common;
using System.Data.Common;
using Bikewale.CoreDAL;
using MySql.CoreDAL;
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
            string sql = string.Empty;

            sql = @" select distinct concat(ma.id,'_',ma.maskingname) as id, name as name from expectedbikelaunches ecl 
                inner join bikemakes ma   on ma.id = ecl.bikemakeid
                where ecl.islaunched = 0 and ma.isdeleted = 0 and ( ma.futuristic = 1 or ma.new = 1)";              

            try
            {

                using (DbCommand cmd = DbFactory.GetDBCommand(sql))
                {
                    cmd.CommandText = sql;
                    cmd.CommandType = CommandType.Text;

                    using (DataSet dsMakeContents = MySqlDatabase.SelectAdapterQuery(cmd, ConnectionType.ReadOnly))
                    {
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
                }
                
                
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, Request.ServerVariables["URL"]);
                
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