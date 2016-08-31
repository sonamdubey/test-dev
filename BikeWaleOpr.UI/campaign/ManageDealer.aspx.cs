using BikewaleOpr.DALs.ManufactureCampaign;
using BikewaleOpr.Entities;
using BikewaleOpr.Entity;
using BikewaleOpr.Entity.ManufacturerCampaign;
using BikewaleOpr.Interface.ManufacturerCampaign;
using BikeWaleOpr.Common;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BikewaleOpr.Campaign
{
    /// <summary>
    /// Created by: Sajal Gupta on 30/08/2016
    /// Description : To manage dealer campaigns with add/update options
    /// </summary>
    public class ManageDealer : System.Web.UI.Page
    {
        protected bool isEdit = true;
        protected Button btnUpdate;
        protected TextBox campaignDescription, txtMaskingNumber, textBox1, textBox2, textBox3, textBox4;
        protected CheckBox isActive, CheckBox1, CheckBox2, CheckBox3, CheckBox4;
        protected HiddenField Hiddenfield1, Hiddenfield2, Hiddenfield3, Hiddenfield4, hdnOldMaskingNumber;
        protected int dealerId, userId;
        protected int campaignId = 0;
        protected Label lblGreenMessage;
        protected string manufacturerName;
        protected string BwOprHostUrl = ConfigurationManager.AppSettings["BwOprHostUrlForJs"];

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["campaignid"] == null)
            {
                isEdit = false;                
            }
            else
            {
                campaignId = Convert.ToInt32(Request.QueryString["campaignid"]);
                if (!IsPostBack)
                {
                    ShowData(campaignId); 
                }
            }
            dealerId = Convert.ToInt32(Request.QueryString["dealerid"]);
            userId = Convert.ToInt32(CurrentUser.Id);

            manufacturerName = Request.QueryString["manufactureName"];
        }

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
            btnUpdate.Click += new EventHandler(InserOrUpdateDealerCampaign);
           // dealerCampaign = new ManageDealerCampaign();

        }
    
        /// <summary>
        /// Created by : SajalGupta on 30/08/2016
        /// Description : This function binds the data to the textboxes;
        /// </summary>
        /// <param name="campaignId"></param>
        private void ShowData(int campaignId)
        {
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IManufacturerCampaignRepository, ManufacturerCampaign>();
                    IManufacturerCampaignRepository objMfgCampaign = container.Resolve<IManufacturerCampaignRepository>();
                    List<BikewaleOpr.Entity.ManufacturerCampaign.ManufacturerCampaignEntity> dataReader = objMfgCampaign.FetchCampaignDetails(campaignId);

                    int count = 0;
                    int pageId, isDefault;
                    string templateHtml;
                    foreach(var campaignDetails in dataReader)
                    {
                        if(count == 0)
                        {
                            campaignDescription.Text = campaignDetails.CampaignDescription;
                            txtMaskingNumber.Text = campaignDetails.CampaignMaskingNumber;
                            if(campaignDetails.IsActive == 1)
                            {
                                isActive.Checked = true;
                            }
                            else
                            {
                                isActive.Checked = false;
                            }
                        }

                        pageId = campaignDetails.PageId;
                        isDefault = campaignDetails.IsDefault;
                        templateHtml = campaignDetails.TemplateHtml;

                        HiddenField control2 = FindControl("Hiddenfield" + pageId) as HiddenField;
                        control2.Value = campaignDetails.TemplateId.ToString();

                        if(isDefault == 1)
                        {
                            CheckBox control = FindControl("CheckBox"+pageId) as CheckBox;
                            control.Checked = true;

                            TextBox control1 = FindControl("TextBox" + pageId) as TextBox;
                            control1.Enabled = false;
                        }
                        else
                        {
                            CheckBox control = FindControl("CheckBox" + pageId) as CheckBox;
                            control.Checked = false;

                            TextBox control1 = FindControl("TextBox" + pageId) as TextBox;
                            control1.Text = campaignDetails.TemplateHtml;
                            control1.Enabled = true;
                        }                        
                        count++;
                    }                    
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "ShowData");
                objErr.SendMail();
            }

        }

        /// <summary>
        /// Created By : Sajal Gupta on 30/08/2016
        /// Description : This function update or inserts manufacturer campaign data. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InserOrUpdateDealerCampaign(object sender, EventArgs e)
        {
            string templateHtml1, templateHtml2, templateHtml3, templateHtml4;
            int templateId1 = 0, templateId2 = 0, templateId3 = 0, templateId4 = 0;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IManufacturerCampaignRepository, ManufacturerCampaign>();
                    IManufacturerCampaignRepository objMfgCampaign = container.Resolve<IManufacturerCampaignRepository>();
                    if (!isEdit)
                    {
                        campaignId = objMfgCampaign.InsertBWDealerCampaign(campaignDescription.Text.Trim(), (isActive.Checked ? 1 : 0), hdnOldMaskingNumber.Value, dealerId, userId);
                    
                        if(!CheckBox1.Checked)
                        {
                            templateHtml1 = textBox1.Text.Trim();
                        }
                        else
                        {
                            templateHtml1 = null;
                            templateId1 = 1;
                        }
                        if (!CheckBox2.Checked)
                        {
                            templateHtml2 = textBox2.Text.Trim();
                        }
                        else
                        {
                            templateHtml2 = null;
                            templateId2 = 2;
                        }
                        if (!CheckBox3.Checked)
                        {
                            templateHtml3 = textBox3.Text.Trim();                            
                        }
                        else
                        {
                            templateHtml3 = null;
                            templateId3 = 3;
                        }
                        if (!CheckBox4.Checked)
                        {
                            templateHtml4 = textBox4.Text.Trim();
                        }
                        else
                        {
                            templateHtml4 = null;
                            templateId4 = 4;
                        }

                        objMfgCampaign.SaveManufacturerCampaignTemplate(templateHtml1, templateId1, templateHtml2, templateId2, templateHtml3, templateId3, templateHtml4, templateId4, userId, campaignId);

                    }
                    else
                    {
                        if (!CheckBox1.Checked)
                        {
                            templateHtml1 = textBox1.Text.Trim();
                            templateId1 = (Convert.ToInt32(Hiddenfield1.Value) != 1) ? Convert.ToInt32(Hiddenfield1.Value) : 0;
                        }
                        else
                        {
                            templateHtml1 = null;
                            templateId1 = 1;
                        }
                        if (!CheckBox2.Checked)
                        {
                            templateHtml2 = textBox2.Text.Trim();
                            templateId2 = (Convert.ToInt32(Hiddenfield2.Value) != 2) ? Convert.ToInt32(Hiddenfield2.Value) : 0;
                        }
                        else
                        {
                            templateHtml2 = null;
                            templateId2 = 2;
                        }
                        if (!CheckBox3.Checked)
                        {
                            templateHtml3 = textBox3.Text.Trim();
                            templateId3 = (Convert.ToInt32(Hiddenfield3.Value) != 3) ? Convert.ToInt32(Hiddenfield3.Value) : 0;
                        }
                        else
                        {
                            templateHtml3 = null;
                            templateId3 = 3;
                        }
                        if (!CheckBox4.Checked)
                        {
                            templateHtml4 = textBox4.Text.Trim();
                            templateId4 = (Convert.ToInt32(Hiddenfield4.Value) != 4) ? Convert.ToInt32(Hiddenfield4.Value) : 0;
                        }
                        else
                        {
                            templateHtml4 = null;
                            templateId4 = 4;
                        }
                        objMfgCampaign.UpdateBWDealerCampaign(campaignDescription.Text.Trim(), (isActive.Checked ? 1 : 0), hdnOldMaskingNumber.Value, dealerId, userId, campaignId, templateHtml1, templateId1, templateHtml2, templateId2, templateHtml3, templateId3, templateHtml4, templateId4);
                        ShowData(campaignId);                       
                    }

                    lblGreenMessage.Text = "Data entered succesfully";
                }
                
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "InserOrUpdateDealerCampaign");
                objErr.SendMail();
            }

        }

        
    }
}