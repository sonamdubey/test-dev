using BikewaleOpr.BAL.ContractCampaign;
using BikewaleOpr.common.ContractCampaignAPI;
using BikewaleOpr.Common;
using BikewaleOpr.CommuteDistance;
using BikewaleOpr.Entity.ContractCampaign;
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
    /// </summary>
    public class ManageDealers : System.Web.UI.Page
    {
        #region variable

        protected int dealerId, contractId, campaignId, currentUserId;
        protected string dealerName, oldMaskingNumber, dealerMobile, reqFormMaskingNumber, reqFormRadius;
        protected Button btnUpdate;
        protected ManageDealerCampaign dealerCampaign;
        protected TextBox txtdealerRadius, txtDealerEmail, txtMaskingNumber, txtCampaignName;
        protected string startDate, endDate;
        public Label lblGreenMessage, lblErrorSummary;
        public HtmlGenericControl textArea;
        public bool isCampaignPresent;
        public DropDownList ddlMaskingNumber;
        public HiddenField hdnOldMaskingNumber;
        protected CommuteDistanceBL objCommuteDistanceBL;
        #endregion

        #region events

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
            btnUpdate.Click += new EventHandler(InserOrUpdateDealerCampaign);
            dealerCampaign = new ManageDealerCampaign();

        }

        /// <summary>
        /// Modified by :   Sumit Kate on 18 Apr 2016
        /// Description :   Save the Areas to Dealer Commute Distance mapping
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InserOrUpdateDealerCampaign(object sender, EventArgs e)
        {
            try
            {
                // Update campaign
                if (isCampaignPresent)
                {
                    dealerCampaign.UpdateBWDealerCampaign(
                        true,
                        campaignId,
                        currentUserId,
                        dealerId,
                        contractId,
                        Convert.ToInt16(reqFormRadius),
                        reqFormMaskingNumber,
                        txtCampaignName.Text.Trim(),
                        txtDealerEmail.Text.Trim(),
                        false);

                    lblGreenMessage.Text = "Selected campaign has been Updated !";

                }
                else // Insert new campaign
                {
                    campaignId = dealerCampaign.InsertBWDealerCampaign(
                         true,
                         currentUserId,
                         dealerId,
                         contractId,
                         Convert.ToInt16(reqFormRadius),
                         reqFormMaskingNumber,
                         txtCampaignName.Text.Trim(),
                         txtDealerEmail.Text.Trim(),
                         false);
                    lblGreenMessage.Text = "New campaign has been added !";
                    isCampaignPresent = true;

                }

                InsertUpdateContractCampaign();

                ClearForm(Page.Form.Controls, true);
                objCommuteDistanceBL = new CommuteDistanceBL();
                objCommuteDistanceBL.DealerID = Convert.ToUInt16(dealerId);
                objCommuteDistanceBL.LeadServingDistance = Convert.ToUInt16(reqFormRadius);
                PageAsyncTask asynTask = new PageAsyncTask(objCommuteDistanceBL.OnBegin, objCommuteDistanceBL.OnEnd, null, null);
                RegisterAsyncTask(asynTask);
                ExecuteRegisteredAsyncTasks();

            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "InserOrUpdateDealerCampaign");
                objErr.SendMail();
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
            ccInputs.SellerMobileMaskingId = default(int);

            CwWebserviceAPI CWWebservice = new CwWebserviceAPI();
            try
            {
                if (IsProd)
                {
                    if (isMaskingChanged)
                    {
                        // Release previous number and add new number
                        CWWebservice.ReleaseMaskingNumber(Convert.ToUInt32(dealerId), currentUserId, reqFormMaskingNumber);
                    }

                    //callApp.pushDataToKnowlarity(false, "-1", dealerMobile, string.Empty, reqFormMaskingNumber);
                    CWWebservice.AddCampaignContractData(ccInputs);

                    if (!CWWebservice.IsCCMapped(Convert.ToUInt32(dealerId), Convert.ToUInt32(contractId), Convert.ToUInt32(campaignId)))
                    {
                        lblGreenMessage.Text = string.Empty;
                        lblErrorSummary.Text = "DataSync : Campaign Contract Updation failed for Carwale";
                    }
                }

            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "UpdateContractCampaign");
                objErr.SendMail();
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
                    }
                }
            }
            if (Request.Form["txtMaskingNumber"] != null)
                reqFormMaskingNumber = Request.Form["txtMaskingNumber"] as string;
            if (Request.Form["txtdealerRadius"] != null)
                reqFormRadius = Request.Form["txtdealerRadius"] as string;
            SetPageVariables();
        }

        /// <summary>
        /// Created By : Sangram Nandkhile on 05-Apr-2016
        /// Description : To Load masking numbers dropdown
        /// </summary>
        private void LoadMaskingNumbers()
        {
            try
            {
                IEnumerable<MaskingNumber> numbersList = null;
                using (IUnityContainer container = new UnityContainer())
                {

                    container.RegisterType<IContractCampaign, ContractCampaign>();
                    IContractCampaign objCC = container.Resolve<IContractCampaign>();

                    numbersList = objCC.GetAllMaskingNumbers(Convert.ToUInt32(dealerId));

                    if (numbersList != null && numbersList.Count() > 0)
                    {
                        ddlMaskingNumber.DataSource = numbersList;
                        ddlMaskingNumber.DataTextField = "Number";
                        ddlMaskingNumber.DataValueField = "IsAssigned";
                        ddlMaskingNumber.DataBind();
                        //ddlMaskingNumber.Items.Insert(0, item);
                    }

                }


            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"] + "BikewaleOpr.Campaign.ManageDealers.LoadMaskingNumbers");
                objErr.SendMail();
            }
        }

        #endregion

        #region functions

        /// <summary>
        /// Created By : Sangram Nandkhile on 21st March 2016.
        /// Description : To fetch current campaign details
        /// Updated by: Sangram
        /// Summary:    Added dealerMobile,hdnOldMaskingNumber,oldMaskingNumber
        /// </summary>
        private void FetchDealeCampaign()
        {
            try
            {
                DataTable dtCampaign = dealerCampaign.FetchBWDealerCampaign(campaignId);
                if (dtCampaign != null && dtCampaign.Rows.Count > 0)
                {
                    txtdealerRadius.Text = dtCampaign.Rows[0]["DealerLeadServingRadius"].ToString();
                    if (!String.IsNullOrEmpty(Convert.ToString(dtCampaign.Rows[0]["Number"])))
                    {
                        txtMaskingNumber.Text = Convert.ToString(dtCampaign.Rows[0]["Number"]);
                        oldMaskingNumber = txtMaskingNumber.Text;
                        hdnOldMaskingNumber.Value = txtMaskingNumber.Text;
                    }
                    txtCampaignName.Text = Convert.ToString(dtCampaign.Rows[0]["DealerName"]);
                    oldMaskingNumber = txtMaskingNumber.Text;
                    txtDealerEmail.Text = dtCampaign.Rows[0]["DealerEmailId"].ToString().Trim();
                    dealerMobile = dtCampaign.Rows[0]["dealerMobile"].ToString();
                }
                else
                {
                    //Response.Redirect("../pagenotfound.aspx");
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"] + "BikewaleOpr.Campaign.ManageDealers.FetchDealeCampaign");
                objErr.SendMail();
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
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"] + "BikewaleOpr.Campaign.ManageDealers.SetPageVariables");
                objErr.SendMail();
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
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"] + "BikewaleOpr.Campaign.ManageDealers.ParseQueryString");
                objErr.SendMail();
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