using BikewaleOpr.BAL.ContractCampaign;
using BikewaleOpr.Cache.Campaigns;
using BikewaleOpr.common.ContractCampaignAPI;
using BikewaleOpr.Common;
using BikewaleOpr.DALs.ContractCampaign;
using BikewaleOpr.Entities;
using BikewaleOpr.Entities.ContractCampaign;
using BikewaleOpr.Interface.ContractCampaign;
using BikeWaleOpr.Common;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace BikewaleOpr.Campaign
{
    /// <summary>
    /// Created by: Sangram Nandkhile on 25 Mar 2016
    /// Desc:       To manage dealer campaigs with add/update/delete options
    /// Modified By : Sushil Kumar on 29th Nov 2016
    /// Description : Added dailylimit textbox 
    /// Modified by :   Sumit Kate on 12 May 2017
    /// Description :   removed lead serving radius property and related code
    /// </summary>
    public class ManageDealers : System.Web.UI.Page
    {
        #region variable
        protected int dealerId, contractId, campaignId, currentUserId;
        protected string dealerName, oldMaskingNumber, dealerMobile, reqFormMaskingNumber, reqLeadsLimit;
        protected Button btnUpdate;
        protected ManageDealerCampaign dealerCampaign;
        protected TextBox txtdealerRadius, txtDealerEmail, txtMaskingNumber, txtCampaignName, txtLeadsLimit, txtDealerNumber;
        protected string startDate, endDate;
        protected Label lblGreenMessage, lblErrorSummary;
        protected HtmlGenericControl textArea;
        protected bool isCampaignPresent;
        protected DropDownList ddlMaskingNumber, ddlCallToAction;
        protected HiddenField hdnOldMaskingNumber;
        protected CheckBox chkUseDefaultCallToAction;
        private const int DEFAULT_CALL_TO_ACTION = 1;
        private IDealerCampaignRepository campaignRepository;
        private IContractCampaign objCC;
        private DealerCampaignEntity campaign;
        protected bool useDefaultCallToAction = true;
        #region Unit Container
        private readonly IUnityContainer container = new UnityContainer();
        #endregion

        #endregion

        #region events

        /// <summary>
        /// Modified by :   Sumit Kate on 29 Dec 2016
        /// Description :   Call registerTypes
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
            btnUpdate.Click += new EventHandler(InserOrUpdateDealerCampaign);
            dealerCampaign = new ManageDealerCampaign();
            RegisterTypes();
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 29 Dec 2016
        /// Description :   Register Types in Unity Container and initilize the references
        /// </summary>
        private void RegisterTypes()
        {
            try
            {
                using (container)
                {
                    container.RegisterType<IContractCampaign, ContractCampaign>();
                    container.RegisterType<IDealerCampaignRepository, DealerCampaignRepository>();

                    campaignRepository = container.Resolve<IDealerCampaignRepository>();
                    objCC = container.Resolve<IContractCampaign>();
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "RegisterTypes");
            }
        }

        /// <summary>
        /// Modified by :   Sumit Kate on 18 Apr 2016
        /// Description :   Save the Areas to Dealer Commute Distance mapping
        /// Modified By : Sushil Kumar on 29th Nov 2016
        /// Description : Added dailylimit textbox to update daily limit lead for the dealer campaign
        /// Modified by :   Sumit Kate on 29 Dec 2016
        /// Description :   Use New DAL and pass the call to action value
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InserOrUpdateDealerCampaign(object sender, EventArgs e)
        {
            try
            {
                // Update campaign
                bool isActive = true;
                int leadLimit = !String.IsNullOrEmpty(reqLeadsLimit) ? Convert.ToInt32(reqLeadsLimit) : 0;
                UInt16 callToAction = (UInt16)(chkUseDefaultCallToAction.Checked ? DEFAULT_CALL_TO_ACTION : Convert.ToUInt16(ddlCallToAction.SelectedValue));
                string campaignName = txtCampaignName.Text.Trim();
                string dealerEmail = txtDealerEmail.Text.Trim();
                if (isCampaignPresent)
                {
                    campaignRepository.UpdateBWDealerCampaign(
                        isActive,
                        campaignId,
                        currentUserId,
                        dealerId,
                        contractId,
                        reqFormMaskingNumber,
                        campaignName,
                        dealerEmail,
                        leadLimit,
                        callToAction
                        );

                    lblGreenMessage.Text = "Selected campaign has been Updated !";

                }
                else // Insert new campaign
                {
                    campaignId = campaignRepository.InsertBWDealerCampaign(
                         isActive,
                         currentUserId,
                         dealerId,
                         contractId,
                         reqFormMaskingNumber,
                         campaignName,
                         dealerEmail,
                         leadLimit,
                         callToAction
                         );
                    lblGreenMessage.Text = "New campaign has been added !";
                    isCampaignPresent = true;

                }

                Dealers.ClearDealerBikes(dealerId);
                InsertUpdateContractCampaign();

                ClearForm(Page.Form.Controls, true);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "InserOrUpdateDealerCampaign");
                
            }
        }

        /// <summary>
        /// Created By  : Sushil Kumar on 14th July 2016
        /// Description : API Call to update contract campaign  to carwale db 
        /// </summary>
        /// <returns></returns>
        private bool InsertUpdateContractCampaign()
        {
            bool isMaskingChanged = hdnOldMaskingNumber.Value == reqFormMaskingNumber ? false : true;
            bool IsProd = Convert.ToBoolean(ConfigurationManager.AppSettings["isProduction"]);
            oldMaskingNumber = hdnOldMaskingNumber.Value;
            DataTable dtCampaign = dealerCampaign.FetchBWDealerCampaign(campaignId);
            if (dtCampaign != null && dtCampaign.Rows.Count > 0)
            {
                dealerMobile = dtCampaign.Rows[0]["dealerMobile"].ToString();
            }


            ContractCampaignInputEntity ccInputs = new ContractCampaignInputEntity();
            ccInputs.ConsumerId = dealerId;
            ccInputs.DealerType = 2;
            ccInputs.LeadCampaignId = campaignId;
            ccInputs.LastUpdatedBy = currentUserId;
            ccInputs.OldMaskingNumber = oldMaskingNumber;
            ccInputs.MaskingNumber = reqFormMaskingNumber;
            ccInputs.NCDBranchId = -1;
            ccInputs.ProductTypeId = 3;
            ccInputs.Mobile = dealerMobile;
            ccInputs.SellerMobileMaskingId = -1;

            CwWebserviceAPI CWWebservice = new CwWebserviceAPI();
            try
            {
                if (IsProd)
                {
                    if (isMaskingChanged)
                    {
                        // Release previous number and add new number
                        CWWebservice.ReleaseMaskingNumber(Convert.ToUInt32(dealerId), currentUserId, oldMaskingNumber);
                    }

                    //callApp.pushDataToKnowlarity(false, "-1", dealerMobile, string.Empty, reqFormMaskingNumber);
                    if (!String.IsNullOrEmpty(ccInputs.MaskingNumber))
                    {
                        CWWebservice.AddCampaignContractData(ccInputs);
                    }

                    if (!CWWebservice.IsCCMapped(Convert.ToUInt32(dealerId), Convert.ToUInt32(contractId), Convert.ToUInt32(campaignId)))
                    {
                        lblGreenMessage.Text = string.Empty;
                        lblErrorSummary.Text = "DataSync : Campaign Contract Updation failed for Carwale";
                    }
                }

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "UpdateContractCampaign");
                
            }
            return false;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (ParseQueryString())
            {
                if (!IsPostBack)
                {
                    LoadMaskingNumbers();
                    if (isCampaignPresent)
                    {
                        FetchDealeCampaign();
                    }
                    else
                    {
                        txtCampaignName.Text = dealerName;
                        txtDealerNumber.Text = dealerMobile;
                    }
                    LoadDealerCallToAction();
                }
            }
            if (Request.Form["txtMaskingNumber"] != null)
                reqFormMaskingNumber = Request.Form["txtMaskingNumber"] as string;
            if (Request.Form["txtLeadsLimit"] != null)
                reqLeadsLimit = Request.Form["txtLeadsLimit"] as string;
            SetPageVariables();
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 29 Dec 2016
        /// Description :   Load Dealer CallToAction dropdown
        /// </summary>
        private void LoadDealerCallToAction()
        {
            ddlCallToAction.DataSource = campaignRepository.FetchDealerCallToActions();
            ddlCallToAction.DataTextField = "Display";
            ddlCallToAction.DataValueField = "Id";
            if (campaign != null)
            {
                ddlCallToAction.SelectedValue = Convert.ToString(campaign.CallToAction);
            }
            ddlCallToAction.CssClass = string.Format("margin-left10 {0}", useDefaultCallToAction ? "hide" : "");
            ddlCallToAction.DataBind();
        }

        /// <summary>
        /// Created By : Sangram Nandkhile on 05-Apr-2016
        /// Description : To Load masking numbers dropdown
        /// Created By : Sushil Kumar on 21st March 2016
        /// Description : Add default text for 
        /// </summary>
        private void LoadMaskingNumbers()
        {
            try
            {
                IEnumerable<MaskingNumber> numbersList = null;
                if (objCC != null)
                {
                    numbersList = objCC.GetAllMaskingNumbers(Convert.ToUInt32(dealerId));

                    if (numbersList != null && numbersList.Any())
                    {
                        ddlMaskingNumber.DataSource = numbersList;
                        ddlMaskingNumber.DataTextField = "Number";
                        ddlMaskingNumber.DataValueField = "IsAssigned";
                        ddlMaskingNumber.DataBind();
                    }

                    ddlMaskingNumber.Items.Insert(0, new ListItem("Select Masking Number", ""));

                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, Request.ServerVariables["URL"] + "BikewaleOpr.Campaign.ManageDealers.LoadMaskingNumbers");
                
            }
        }

        #endregion

        #region functions

        /// <summary>
        /// Created By : Sangram Nandkhile on 21st March 2016.
        /// Description : To fetch current campaign details
        /// Updated by: Sangram
        /// Summary:    Added dealerMobile,hdnOldMaskingNumber,oldMaskingNumber
        /// Modified By : Sushil Kumar on 29th Nov 2016
        /// Description : Added dailylimit textbox to update daily limit lead for the dealer campaign
        /// </summary>
        private void FetchDealeCampaign()
        {
            try
            {
                if (campaignRepository != null)
                {
                    campaign = campaignRepository.FetchBWDealerCampaign((uint)campaignId);
                    if (campaign != null)
                    {
                        if (!String.IsNullOrEmpty(campaign.MaskingNumber))
                        {
                            txtMaskingNumber.Text = campaign.MaskingNumber;
                            oldMaskingNumber = txtMaskingNumber.Text;
                            hdnOldMaskingNumber.Value = txtMaskingNumber.Text;
                        }
                        txtDealerNumber.Text = campaign.DealerMobile;
                        txtCampaignName.Text = campaign.CampaignName;
                        oldMaskingNumber = txtMaskingNumber.Text;
                        txtDealerEmail.Text = campaign.EmailId;
                        dealerMobile = campaign.DealerMobile;
                        txtLeadsLimit.Text = Convert.ToString(campaign.DailyLeadLimit);
                        useDefaultCallToAction = campaign.CallToAction == DEFAULT_CALL_TO_ACTION ? true : false;
                        chkUseDefaultCallToAction.Checked = useDefaultCallToAction;
                    }
                }


            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, Request.ServerVariables["URL"] + "BikewaleOpr.Campaign.ManageDealers.FetchDealeCampaign");
                
            }
        }

        /// <summary>
        /// Created By :   Sangram Nandkhile on 21st March 2016.
        /// Description :  Sets basic variable for get, set, update functions
        /// </summary>
        private void SetPageVariables()
        {
            try
            {
                currentUserId = Convert.ToInt32(CurrentUser.Id);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, Request.ServerVariables["URL"] + "BikewaleOpr.Campaign.ManageDealers.SetPageVariables");
                
            }
        }

        /// <summary>
        /// Created By : Sangram Nandkhile on 21st March 2016.
        /// Description : Parses query string to fetch campaign id, dealerid and dealerName
        /// </summary>
        private bool ParseQueryString()
        {
            bool isValid = true;
            try
            {
                if (Request.QueryString["contractid"] == null || Request.QueryString["dealerid"] == null || Request.QueryString["dealername"] == null)
                {
                    isValid = false;
                    if (!isValid)
                    {
                        Response.Redirect("../pagenotfound.aspx", false);
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                        this.Page.Visible = false;
                    }
                }
                if (!string.IsNullOrEmpty(Request.QueryString["contractid"]))
                {
                    contractId = Convert.ToInt32(Request.QueryString["contractid"]);
                }
                if (!string.IsNullOrEmpty(Request.QueryString["dealerid"]))
                {
                    dealerId = Convert.ToInt32(Request.QueryString["dealerid"]);
                }
                if (!string.IsNullOrEmpty(Request.QueryString["dealername"]))
                {
                    dealerName = Request.QueryString["dealername"];
                }
                if (!string.IsNullOrEmpty(Request.QueryString["campaignid"]))
                {
                    campaignId = Convert.ToInt32(Request.QueryString["campaignid"]);
                    isCampaignPresent = true;
                }
                if (!string.IsNullOrEmpty(Request.QueryString["no"]))
                {
                    dealerMobile = Request.QueryString["no"];
                }

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, Request.ServerVariables["URL"] + "BikewaleOpr.Campaign.ManageDealers.ParseQueryString");
                
            }
            return isValid;
        }

        /// <summary>
        /// Created by  : Sangram Nandkhile on 14-March-2016
        /// Description : Resets all the Textboxes
        /// </summary>
        public void ClearForm(ControlCollection controls, bool? clearLabels)
        {
            bool toClearLabel = clearLabels == null ? false : true;
            foreach (Control c in controls)
            {
                if (c.GetType() == typeof(System.Web.UI.WebControls.TextBox))
                {
                    System.Web.UI.WebControls.TextBox t = (System.Web.UI.WebControls.TextBox)c;
                    t.Text = String.Empty;
                }
                if (c.Controls.Count > 0) ClearForm(c.Controls, clearLabels);
            }
        }
        #endregion
    }
}