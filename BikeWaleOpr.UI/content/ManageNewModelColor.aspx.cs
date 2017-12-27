using BikewaleOpr.Cache;
using BikewaleOpr.Common;
using BikewaleOpr.Entities;
using BikeWaleOpr.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace BikewaleOpr.content
{
    public class ManageNewModelColor : System.Web.UI.Page
    {
        protected string modelColorId;
        protected string ModelColorName;
        protected string modelId;
        protected Repeater rptHexCode;
        protected Button btnSave, btnUpdate;
        protected TextBox txtNewHexCode;
        protected HtmlGenericControl spnError;

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }
        void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
            btnSave.Click += new EventHandler(btnSave_Cilck);
            btnUpdate.Click += new EventHandler(btnUpdate_Click);            
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            ManageModelColor objManageModelColor = null;
            string hexCode = String.Empty;
            bool isActive = false;
            int colorId = 0;
            bool isSaved = false;
            try
            {
                ProcessQueryString();
                if (!String.IsNullOrEmpty(modelId))
                {
                    objManageModelColor = new ManageModelColor();
                    for (int i = 0; i < rptHexCode.Items.Count; i++)
                    {
                        TextBox txtHexCode = (TextBox)rptHexCode.Items[i].FindControl("txtHexCode");
                        CheckBox chkActive = (CheckBox)rptHexCode.Items[i].FindControl("chkActive");
                        HiddenField hdnColorID = (HiddenField)rptHexCode.Items[i].FindControl("hdnColorID");

                        hexCode = txtHexCode.Text.Trim();
                        colorId = Convert.ToInt32(hdnColorID.Value);
                        isActive = chkActive.Checked;
                        isSaved = objManageModelColor.UpdateColorCode(colorId, hexCode, Convert.ToInt32(CurrentUser.Id), isActive);
                    }
                    if (rptHexCode.Items.Count > 0)
                    {
                        MemCachedUtil.Remove(string.Format("BW_ModelPhotosColorWise_{0}", modelId));
                    }
                }
                BindControls();
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, Request.ServerVariables["URL"]);
                
                spnError.InnerHtml = "<b>Error occured while saving the new color hex code.</b>";
            }
            if (isSaved)
            {
                spnError.InnerHtml = "<b>Saved.</b>";
            }
            else
            {
                spnError.InnerHtml = "<b>Not Updated.</b>";
            }
        }

        private void btnSave_Cilck(object sender, EventArgs e)
        {
            ManageModelColor objManageModelColor = null;
            string hexCode = String.Empty;
            bool isSaved = false;
            try
            {
                ProcessQueryString();
                if (!String.IsNullOrEmpty(modelId))
                {
                    hexCode = txtNewHexCode.Text.Trim();
                    if (!String.IsNullOrEmpty(hexCode))
                    {
                        objManageModelColor = new ManageModelColor();
                        isSaved = objManageModelColor.AddColorCode(Convert.ToInt32(modelColorId), hexCode, Convert.ToInt32(CurrentUser.Id));
                        txtNewHexCode.Text = "";
                    }
                }
                BindControls();
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, Request.ServerVariables["URL"]);
                
                spnError.InnerHtml = "<b>Error occured while saving the new color hex code.</b>";
            }
            if (isSaved)
            {
                spnError.InnerHtml = "<b>Saved.</b>";
            }
            else
            {
                spnError.InnerHtml = "<b>Not Updated.</b>";
            }            
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            ProcessQueryString();
            BindControls();
            spnError.InnerText = string.Empty;
        }

        private void ProcessQueryString()
        {
            modelColorId = Request.QueryString["modelColorId"];
            modelId = Request.QueryString["modelId"];
        }

        private void BindControls()
        {
            try
            {
                ManageModelColor objManageModelColor = null;
                IEnumerable<ModelColorBase> modelColors = null;
                if (!String.IsNullOrEmpty(modelColorId) && !String.IsNullOrEmpty(modelId))
                {
                    objManageModelColor = new ManageModelColor();
                    modelColors = objManageModelColor.FetchModelColors(Convert.ToInt32(modelId));

                    if (modelColors != null && modelColors.Any())
                    {
                        ModelColorBase modelColor = (from color in modelColors
                                                     where color.Id == Convert.ToInt32(modelColorId)
                                                     select color).FirstOrDefault();

                        ModelColorName = modelColor.Name;
                        rptHexCode.DataSource = modelColor.ColorCodes;
                        rptHexCode.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, Request.ServerVariables["URL"]);
                
            }
        }
    }
}