using BikewaleOpr.Common;
using BikewaleOpr.Entities;
using BikeWaleOpr.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace BikewaleOpr.NewBikeBooking
{
    public class ManageBikeAvailabilityByColorAndCost : System.Web.UI.Page
    {
        protected Repeater rptModelColor, rptVersionColor, rptColor, rptVColor;
        protected HtmlGenericControl spnError;
        protected HtmlInputHidden hdnVersionColor, hdnColorDayObject;
        protected Button  btnUpdateVersionColorAvailability;
        protected int modelColorCount = 0;
        protected int versionCount = 0;
        protected int dealerId = 0, versionId = 0;
        protected string versionAvailabilityDays = String.Empty;
        protected int modelId = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            //queryString Parse
            dealerId = Convert.ToInt32(Request.QueryString["dealerId"]);
            versionId = Convert.ToInt32(Request.QueryString["versionId"]);

            versionAvailabilityDays = Convert.ToString(Request.QueryString["versionAvailDays"]);

            ManageBikeAvailbilityByColor objModel = new ManageBikeAvailbilityByColor();

            modelId = objModel.GetModelIdForVersion(Convert.ToInt32(versionId));

            if (modelId > 0 && versionId > 0 &&  dealerId > 0)
            {
                rptColor.DataSource = objModel.FetchVersionColorsWithAvailability(versionId, dealerId);
                rptColor.DataBind();
            }
            

        }

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
            btnUpdateVersionColorAvailability.Click += new EventHandler(btnUpdateVersionColor_Click);
            rptColor.ItemDataBound += new RepeaterItemEventHandler(rptColor_ItemDataBound);
        }

        private void rptColor_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            RepeaterItem item = e.Item;
            if ((item.ItemType == ListItemType.Item) ||
                (item.ItemType == ListItemType.AlternatingItem))
            {
                rptVColor = (Repeater)item.FindControl("rptVColor");
                string modelColorId = ((HiddenField)item.FindControl("hdnModelColorId")).Value;
                IEnumerable<ModelColorBase> modelColors = (new ManageModelColor()).FetchModelColors(modelId);
                rptVColor.DataSource = (from color in modelColors
                                        where color.Id == Convert.ToUInt32(modelColorId)
                                        select color).FirstOrDefault().ColorCodes;
                rptVColor.DataBind();
            }
        }

        //private void rptModelColor_ItemDataBound(object sender, RepeaterItemEventArgs e)
        //{
        //    RepeaterItem item = e.Item;
        //    if ((item.ItemType == ListItemType.Item) ||
        //        (item.ItemType == ListItemType.AlternatingItem))
        //    {
        //        rptColor = (Repeater)item.FindControl("rptColorCode");
        //        string modelColorId = ((HiddenField)item.FindControl("hdnModelColorId")).Value;
        //        Button btnDelete = (Button)item.FindControl("btnDelete");
        //        btnDelete.Click += new EventHandler(btnDelete_Click);
        //        IEnumerable<ModelColorBase> modelColors = (new ManageModelColor()).FetchModelColors(Convert.ToInt32(ModelId));
        //        rptColor.DataSource = (from color in modelColors
        //                               where color.Id == Convert.ToUInt32(modelColorId)
        //                               select color).FirstOrDefault().ColorCodes;
        //        rptColor.DataBind();
        //    }
        //}

        private void btnUpdateVersionColor_Click(object sender, EventArgs e)
        {
            string hdnColorDays = hdnColorDayObject.Value;
            string[] versionColors = null;
            string bikeVersionId = string.Empty;
            bool isUpdated = false;
            ManageBikeAvailbilityByColor objManageModelColorByColor = null;
            try
            {
                if (!String.IsNullOrEmpty(hdnColorDays))
                {
                    objManageModelColorByColor = new ManageBikeAvailbilityByColor();
                    versionColors = hdnColorDays.Split(',');

                    if (versionColors != null && versionColors.Length > 0)
                        {
                            for (int i = 0; i < versionColors.Length; i++)
                            {
                                string[] str = versionColors[i].Split('_');
                                if(str!=null && str.Length > 0)
                                {
                                   isUpdated = objManageModelColorByColor.UpdateBikeAvailabilityByColor(
                                   new VersionColorWithAvailability()
                                   {
                                       ModelColorID = Convert.ToUInt32(str[0]),
                                       NoOfDays = Convert.ToString(str[1])
                                   }, Convert.ToInt32(versionId),
                                   Convert.ToInt32(CurrentUser.Id), Convert.ToInt32(dealerId));
                                }
                    
                            }
                        }
                }
                if (isUpdated)
                {
                    spnError.InnerHtml = "<b>Saved.</b>";
                }
                else
                {
                    spnError.InnerHtml = "<b>Not Updated.</b>";
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
               // spnError.InnerHtml = "<b>Error occured while saving the version colors.</b>";
            }

        }

        private void rptVersionColor_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            RepeaterItem item = e.Item;
            if ((item.ItemType == ListItemType.Item) ||
                (item.ItemType == ListItemType.AlternatingItem))
            {
                rptColor = (Repeater)item.FindControl("rptColor");
                rptColor.ItemDataBound += new RepeaterItemEventHandler(rptColor_ItemDataBound);
                rptColor.DataSource = (new ManageBikeAvailbilityByColor()).FetchVersionColorsWithAvailability(versionId,dealerId);
                rptColor.DataBind();
            }
        }
    }
}