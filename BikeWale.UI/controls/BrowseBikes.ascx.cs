using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Bikewale.Common;
using System.Data;
using Bikewale.Memcache;

namespace Bikewale.Controls
{
    /// <summary>
    ///     Created By : Ashish G. Kamble on 1/9/2012
    ///     Class will provide search for the selected new bike
    /// </summary>
    public class BrowseBikes : System.Web.UI.UserControl
    {
        protected DropDownList ddlMake, ddlModel, ddlVersion;
        protected Button btn_browseBikes;
        protected Literal ltrTitel;
        protected HtmlInputHidden hdn_SelectedModel, hdn_SelectedVersion;
    

        protected string drpMake_Id = String.Empty, drpModel_Id = String.Empty, drpVersion_Id = String.Empty,
                         hdn_SelectedModel_Id = String.Empty, hdn_SelectedVersion_Id = String.Empty, btn_browseBikes_Id = String.Empty;

        private bool _version = true;

        public bool VersionRequired
        {
            get { return _version ; }
            set { _version = value; }
        }

        private string _caption="New Bikes";

        public string Caption
        {
            get { return _caption; }
            set { _caption = value; }
        }

        
        
        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
            btn_browseBikes.Click += new EventHandler(BrowseBikes_Click);
          
        }     

        protected void Page_Load(object sender, EventArgs e)
        {
            drpMake_Id = ddlMake.ClientID.ToString();
            drpModel_Id = ddlModel.ClientID.ToString();
            drpVersion_Id = ddlVersion.ClientID.ToString();
            hdn_SelectedModel_Id = hdn_SelectedModel.ClientID.ToString();
            hdn_SelectedVersion_Id = hdn_SelectedVersion.ClientID.ToString();
            btn_browseBikes_Id = btn_browseBikes.ClientID.ToString();

            if (!IsPostBack)
            {
                ltrTitel.Text = Caption;
                FillMakes();    
            }
        }

        protected void FillMakes()
        {
            DataSet ds = null;
            BikeMakes objBm = new BikeMakes();
            ds = objBm.GetNewBikeMakes();

            ddlMake.DataSource = ds.Tables[0];
            ddlMake.DataTextField = "Name";
            ddlMake.DataValueField = "Value";
            ddlMake.DataBind();
            /*MakeModelVersion mmv = new MakeModelVersion();
            DataTable dt = mmv.GetMakes("NEW");

            ddlMake.DataSource = dt;
            ddlMake.DataTextField = "Text";
            ddlMake.DataValueField = "Value";
            ddlMake.DataBind();
            */


           ddlMake.Items.Insert(0,new ListItem("--Select Make--", "0"));
        }   // end of fill makes

        /// <summary>
        ///     On click of the button redirect user to new bikes page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BrowseBikes_Click(object sender, EventArgs e)
        {
            string modelId = String.Empty, versionId = String.Empty, model = String.Empty, version = String.Empty;
            
            string makeId = ddlMake.SelectedValue.Split('_')[0];
            
            if (!String.IsNullOrEmpty(hdn_SelectedModel.Value))
            {
                modelId = hdn_SelectedModel.Value.Split('|')[0];
                modelId = modelId.Split('_')[0];
                //modelId = ddlModel.SelectedValue.Split('_')[0];
            }
            else {
                modelId = "0";
            }
            if (!String.IsNullOrEmpty(hdn_SelectedVersion.Value))
            {
                versionId = hdn_SelectedVersion.Value.Split('|')[0];                
            }
            else {
                versionId = "0";
            }
    
            string redirectURL = GetRedirectURL(makeId, modelId, versionId);

            if (!String.IsNullOrEmpty(redirectURL))
            {
               Response.Redirect(redirectURL);
            }            
        }

        /// <summary>
        ///     Function to form the url to redirect on the makes, models or versions page
        /// </summary>
        /// <returns></returns>
        protected string GetRedirectURL(string makeId, string modelId, string versionId)
        {
            string redirectURL = String.Empty;
            MakeModelVersion mmv = new MakeModelVersion();
            //MakeModelVersion mm = new MakeModelVersion();
            if (makeId != "0" && modelId != "0" && versionId != "0")
            {
                mmv.GetVersionDetails(versionId);
                //mm.GetModelDetails(modelId);
                //redirectURL += "/" + UrlRewrite.FormatSpecial(mmv.Make.Replace(" ", "").ToLower()) + "-bikes/" + UrlRewrite.FormatSpecial(mmv.Model.Replace(" ", "").ToLower()) + "/" + UrlRewrite.FormatSpecial(mmv.Version.Replace(" ", "").ToLower()) + "-specs-" + versionId + ".html";
                //redirectURL += "/" + mmv.MakeMappingName + "-bikes/" + mmv.ModelMappingName + "/" + UrlRewrite.FormatSpecial(mmv.Version.Replace(" ", "").ToLower()) + "-specs-" + versionId + ".html";

                redirectURL += "/" + mmv.MakeMappingName + "-bikes/" + mmv.ModelMappingName + "/";
                //Trace.Warn("mmv model mapping name : ", mmv.ModelMappingName);
            }
            else if (makeId != "0" && modelId != "0")
            {
                mmv.GetModelDetails(modelId);
                //redirectURL += "/" + UrlRewrite.FormatSpecial(mmv.Make.Replace(" ", "").ToLower()) + "-bikes/" + UrlRewrite.FormatSpecial( mmv.Model.Replace(" ", "").ToLower()) + "/";            
                redirectURL += "/" + mmv.MakeMappingName + "-bikes/" + mmv.ModelMappingName + "/"; 
            }
            else if (makeId != "0")
            {
                mmv.GetMakeDetails(makeId);
                redirectURL += "/" + mmv.MakeMappingName + "-bikes/";                
            }
            
            return redirectURL;
        }   // End of GetRedirectURL function
    
    }   // End of class
}   // End of namespace