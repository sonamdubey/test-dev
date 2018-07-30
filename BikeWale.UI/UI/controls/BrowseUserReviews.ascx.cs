using Bikewale.Common;
using Bikewale.Entities.BikeData;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Bikewale.Controls
{
    public class BrowseUserReviews : System.Web.UI.UserControl
    {
        protected DropDownList drpRevMake, drpRevModel;
        protected HtmlInputHidden hdn_drpModel;
        protected string drpRevMake_Id, drpRevModel_Id, drpRevModel_Name, hdn_drpModel_Id, makeName, modelName;

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            drpRevMake_Id = drpRevMake.ClientID.ToString();
            Trace.Warn("----" + drpRevMake_Id);
            drpRevModel_Id = drpRevModel.ClientID.ToString();
            hdn_drpModel_Id = hdn_drpModel.ClientID.ToString();
            drpRevModel_Name = drpRevModel.ClientID.ToString().Replace("_", "$");

            if (!IsPostBack)
            {
                FillMakes();
            }
        }

        /// <summary>
        ///     Function to fill the makes drop down list
        /// </summary>
        protected void FillMakes()
        {
            MakeModelVersion mmv = new MakeModelVersion();
            //DataTable dt = mmv.GetMakes("USERREVIEW");

            //if (dt != null)
            //{
            //    drpRevMake.DataSource = dt;
            //    drpRevMake.DataTextField = "Text";
            //    drpRevMake.DataValueField = "Value";
            //    drpRevMake.DataBind();

            //    drpRevMake.Items.Insert(0,new ListItem("--Select Make--","0"));
            //}

            mmv.GetMakes(EnumBikeType.UserReviews, ref drpRevMake);
        }
    }
}