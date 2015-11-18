﻿using BikewaleOpr.Common;
using BikewaleOpr.Entities;
using BikeWaleOpr;
using BikeWaleOpr.Common;
using BikeWaleOpr.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace BikewaleOpr.content
{
    public class NewBikeModelColors_New : System.Web.UI.Page
    {
        protected HtmlGenericControl spnError;
        protected HtmlInputHidden hdnVersionColor, hdnHexCodes, hdnDeleteModelId;
        protected Button btnSave, btnShowColors, btnUpdateVersionColor, btnDelete;
        protected DataGrid dtgrdColors;
        protected DropDownList cmbMake, cmbModel;
        protected TextBox txtColor, txtCode;
        protected Repeater rptModelColor, rptVersionColor, rptColor;        
        protected int modelColorCount = 0;
        protected int versionCount = 0;
        public string SelectedModel
        {
            get
            {
                if (Request.Form["cmbModel"] != null)
                    return Request.Form["cmbModel"].ToString();
                else
                    return "-1";
            }
        }

        public int ModelId
        {
            get
            {
                if (!String.IsNullOrEmpty(SelectedModel))
                {
                    return Convert.ToInt32(SelectedModel);
                }
                return 0;
            }
        }

        public string ModelContents
        {
            get
            {
                if (Request.Form["hdn_cmbModel"] != null)
                    return Request.Form["hdn_cmbModel"].ToString();
                else
                    return "";
            }
        }

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
            btnShowColors.Click += new EventHandler(btnShowColors_Click);
            rptVersionColor.ItemDataBound += new RepeaterItemEventHandler(rptVersionColor_ItemDataBound);
            btnSave.Click += new EventHandler(btnSave_Click);
            btnUpdateVersionColor.Click += new EventHandler(btnUpdateVersionColor_Click);
            rptModelColor.ItemDataBound += new RepeaterItemEventHandler(rptModelColor_ItemDataBound);            
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            bool isSaved = false;
            ManageModelColor obj = null;
            try
            {
                obj = new ManageModelColor();

                isSaved = obj.DeleteModelColor(Convert.ToInt32(hdnDeleteModelId.Value), CurrentUser.UserName);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
                spnError.InnerHtml = "<b>Error occured while deleting.</b>";
            }
            if (isSaved)
            {
                spnError.InnerHtml = "<b>Deleted.</b>";
                BindModelColorRepeater();
                hdnDeleteModelId.Value = "";
            }
            else
            {
                spnError.InnerHtml = "<b>Not Deleted.</b>";
            }
        }        

        private void rptModelColor_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            RepeaterItem item = e.Item;
            if ((item.ItemType == ListItemType.Item) ||
                (item.ItemType == ListItemType.AlternatingItem))
            {
                rptColor = (Repeater)item.FindControl("rptColorCode");
                string modelColorId = ((HiddenField)item.FindControl("hdnModelColorId")).Value;
                Button btnDelete = (Button)item.FindControl("btnDelete");
                btnDelete.Click += new EventHandler(btnDelete_Click);
                IEnumerable<ModelColorBase> modelColors = (new ManageModelColor()).FetchModelColors(Convert.ToInt32(ModelId));
                rptColor.DataSource = (from color in modelColors
                                       where color.Id == Convert.ToUInt32(modelColorId)
                                       select color).FirstOrDefault().ColorCodes;
                rptColor.DataBind();
            }
        }

        private void btnUpdateVersionColor_Click(object sender, EventArgs e)
        {
            string strhdnVersionColor = hdnVersionColor.Value;
            string[] versionColors = null;
            string[] versionColor = null;
            string bikeVersionId = string.Empty;
            bool isUpdated = false;
            ManageModelColor objManageModelColor = null;
            try
            {
                if (!String.IsNullOrEmpty(strhdnVersionColor))
                {
                    objManageModelColor = new ManageModelColor();
                    versionColors = strhdnVersionColor.Split('|');
                    foreach (string strVersionColor in versionColors)
                    {
                        versionColor = strVersionColor.Split(':');

                        if (versionColor != null && versionColor.Length > 1)
                        {
                            bikeVersionId = versionColor[0];
                            foreach (string modelColor in versionColor[1].Split(','))
                            {
                                isUpdated = objManageModelColor.SaveVersionColor(
                                    new VersionColorBase()
                                    {
                                        ModelColorID = Convert.ToUInt32(modelColor.Split('_')[0]),
                                        IsActive = modelColor.Split('_')[1].Equals("1")
                                    }, Convert.ToInt32(bikeVersionId), CurrentUser.UserName);
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
                spnError.InnerHtml = "<b>Error occured while saving the version colors.</b>";
            }

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            bool isSaved = false;
            string strHexCode = String.Empty, colorName = String.Empty, hexCodes = String.Empty;
            ManageModelColor objManageModelColor = null;
            try
            {
                strHexCode = hdnHexCodes.Value.Trim();
                colorName = txtColor.Text.Trim();
                if (!String.IsNullOrEmpty(strHexCode))
                {
                    hexCodes = strHexCode;
                }                
                objManageModelColor = new ManageModelColor();
                if (!String.IsNullOrEmpty(hexCodes))
                {
                    isSaved = objManageModelColor.SaveModelColor(ModelId, colorName, CurrentUser.UserName, hexCodes);
                }
                if (isSaved)
                {
                    spnError.InnerHtml = "<b>Saved the new Model Color</b>";
                }
                else
                {
                    spnError.InnerHtml = "<b>No color saved.</b>";
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
                spnError.InnerHtml = "<b>Error occured while saving the model color</b>";
            }
            txtColor.Text = String.Empty;            
            BindModelColorRepeater();
        }

        private void rptVersionColor_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            RepeaterItem item = e.Item;
            if ((item.ItemType == ListItemType.Item) ||
                (item.ItemType == ListItemType.AlternatingItem))
            {
                rptColor = (Repeater)item.FindControl("rptColor");
                string versionId = ((HiddenField)item.FindControl("BikeVersionId")).Value;
                rptColor.DataSource = (new ManageModelColor()).FetchVersionColors(Convert.ToInt32(versionId));
                rptColor.DataBind();
            }
        }

        private void btnShowColors_Click(object sender, EventArgs e)
        {
            BindModelColorRepeater();
        }

        private void BindModelColorRepeater()
        {
            ManageModelColor objManageModelColor = null;
            IEnumerable<ModelColorBase> modelColors = null;
            IEnumerable<VersionEntityBase> versions = null;
            try
            {
                objManageModelColor = new ManageModelColor();                
                modelColors = objManageModelColor.FetchModelColors(ModelId);
                versions = objManageModelColor.FetchBikeVersion(ModelId);

                if (modelColors !=null && modelColors.Count() > 0)
                {
                    modelColorCount = modelColors.Count();
                    rptModelColor.DataSource = modelColors;
                    rptModelColor.DataBind();
                }
                if (versions!=null && versions.Count() > 0)
                {
                    versionCount = versions.Count();
                    rptVersionColor.DataSource = versions;
                    rptVersionColor.DataBind(); 
                }

            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Ajax.Utility.RegisterTypeForAjax(typeof(AjaxFunctions));
            CommonOpn op = new CommonOpn();
            string sql;

            if (!IsPostBack)
            {
                sql = "SELECT ID, Name FROM BikeMakes WHERE IsDeleted <> 1 ORDER BY NAME";
                op.FillDropDown(sql, cmbMake, "Name", "ID");
                ListItem item = new ListItem("--Select--", "0");
                cmbMake.Items.Insert(0, item);
            }
            else
            {
                AjaxFunctions aj = new AjaxFunctions();
                aj.UpdateContents(cmbModel, ModelContents, SelectedModel);
                BindModelColorRepeater();
            }
            spnError.InnerText = string.Empty;
        }
    }
}