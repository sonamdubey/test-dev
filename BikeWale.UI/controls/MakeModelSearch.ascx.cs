using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bikewale.Common;
using Bikewale.Memcache;

namespace Bikewale.Controls
{
    public class MakeModelSearch : System.Web.UI.UserControl
    {
        protected DropDownList drpMake, drpModel;
        private DataSet _makeContents = null;
        private string _makeId = "-1", _modelId = "-1";

        private string _requestType;

        public string RequestType
        {
            get { return _requestType; }
            set { _requestType = value; }
        }          

        protected override void OnInit(EventArgs e)
        {
            base.Load += new EventHandler(Page_Load);
        }
        private void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadMakes();
                AutoFill();
            }
        }
        void LoadMakes()
        {
            //DataTable dtMakeContents;
            //if (MakeContents == null)
            //{
            //    dsMakeContents = BWCommon.GetStaticMakes();
            //}
            //else
            //{
            //    dsMakeContents = MakeContents;
            //}

            DataTable dt = null;
            MakeModelVersion MMV = new MakeModelVersion();

            dt = MMV.GetMakes(RequestType);
            drpMake.DataSource = dt;
            drpMake.DataTextField = "Text";
            drpMake.DataValueField = "Value";
            drpMake.DataBind();
            drpMake.Items.Insert(0, new ListItem("--Select Makes--", "0"));

            //MakeModelVersion mmv = new MakeModelVersion();
            //dtMakeContents = mmv.GetMakes(RequestType);
            //Trace.Warn("dtMakeContents.Rows.Count " + dtMakeContents.Rows.Count);
            //if (dtMakeContents != null && dtMakeContents.Rows.Count > 0)
            //{
            //    drpMake.DataSource = dtMakeContents;
            //    drpMake.DataTextField = "Text";
            //    drpMake.DataValueField = "Value";
            //    drpMake.DataBind();
            //    drpMake.Items.Insert(0, new ListItem("--Select Makes--", "0"));
            //}
        }

        void AutoFill()
        {
            MakeModelVersion mmv = new MakeModelVersion();
            try
            {
                HttpContext.Current.Trace.Warn("AUTO FILL");
                if (MakeId != "" && MakeId != "-1")
                {
                    drpMake.SelectedIndex = drpMake.Items.IndexOf(drpMake.Items.FindByValue(MakeId + '_' + Request.QueryString["make"].ToString()));
                    Trace.Warn("drpMake.SelectedIndex " + drpMake.SelectedIndex);
                    drpModel.Enabled = true;
                    drpModel.DataSource = mmv.GetModelsWithMappingName(MakeId, "ROADTEST");
                    drpModel.DataTextField = "Text";
                    drpModel.DataValueField = "Value";
                    drpModel.DataBind();
                    ListItem item = new ListItem("All Models", "0");
                    drpModel.Items.Insert(0, item);

                    Trace.Warn("----ModelId : " + ModelId);
                    if (ModelId != "" && ModelId != "-1")
                    {
                        Trace.Warn("Filling versions...");
                        Trace.Warn("selected drp value : " + drpModel.SelectedValue);
                        drpModel.SelectedIndex = drpModel.Items.IndexOf(drpModel.Items.FindByValue(ModelId + '_' + Request.QueryString["model"].ToString()));
                    }
                }
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message + err.Source);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
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

        public string ModelId
        {
            get
            {
                return _modelId;
            }
            set
            {
                _modelId = value;
            }
        }
    }
}