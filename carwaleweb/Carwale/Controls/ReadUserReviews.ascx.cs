using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Carwale.UI.Common;
using System.Data.Common;
using Carwale.DAL.CoreDAL.MySql;

namespace Carwale.UI.Controls
{
    public class ReadUserReviews : UserControl
    {
        protected HtmlGenericControl ulUserReview;
        protected DropDownList drpRevMake, drpRevModel;
        protected HtmlInputHidden hdn_drpModel;
        protected string drpRevMake_Id, drpRevModel_Id, drpRevModel_Name, hdn_drpModel_Id, makeName, modelName;
        protected bool _VerticalDisplay = true;

        protected override void OnInit(EventArgs e)
        {
            InitializeComponents();
        }

        void InitializeComponents()
        {
            this.Load += new EventHandler(this.Page_Load);
        }

        public bool VerticalDisplay
        {
            get
            {
                return _VerticalDisplay;
            }
            set
            {
                _VerticalDisplay = value;
            }
        }

        public string ModelContents
        {
            get
            {
                if (hdn_drpModel != null)
                    return hdn_drpModel.Value;
                else
                    return "";
            }
        }

        public string SelectedModel
        {
            get
            {
                if (Request.Form[drpRevModel_Name] != null && Request.Form[drpRevModel_Name].ToString() != "")
                    return Request.Form[drpRevModel_Name].ToString();
                else
                    return "-1";
            }
        }

        void Page_Load(object sender, EventArgs e)
        {
            drpRevMake_Id = drpRevMake.ClientID.ToString();
            drpRevModel_Id = drpRevModel.ClientID.ToString();
            hdn_drpModel_Id = hdn_drpModel.ClientID.ToString();
            drpRevModel_Name = drpRevModel.ClientID.ToString().Replace("_", "$");

            if (VerticalDisplay == false)
            {
                ulUserReview.Attributes.Add("class", "ur-hor");
            }
            if (!IsPostBack)
            {
                ReviewMakes();
            }

        } // Page_Load	

        private void ReviewMakes()
        {
            CommonOpn op = new CommonOpn();
            DataSet ds = new DataSet();
            using (DbCommand cmd = DbFactory.GetDBCommand("GetCarMakes_v16_11_7"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(DbFactory.GetDbParam("v_MakeCond", DbType.String, 10, "All"));
                ds = MySqlDatabase.SelectAdapterQuery(cmd, DbConnections.CarDataMySqlReadConnection);
            }
            if (ds != null && ds.Tables.Count > 0)
                op.FillDropDownMySql(ds.Tables[0], drpRevMake, "MakeName", "MakeId", "Make"); 
        }
    }//class
}//namespace