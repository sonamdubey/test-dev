using BikewaleOpr.Common;
using BikewaleOpr.Entity;
using BikeWaleOpr.Common;
using System;
using System.Linq;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace BikewaleOpr.Campaign
{
    /// <summary>
    /// Created By :    Sangram Nandkhile on 22 March 2016.
    /// Description :   To manage campaigns for contract
    /// </summary>
    public class MapCampaign : System.Web.UI.Page
    {
        #region Variables

        protected Repeater rptCampaigns;
        protected string startDate = string.Empty, endDate = string.Empty, packageName = string.Empty;
        protected int contractId, packageId, contractStatus, dealerId;
        protected Panel pnlExisting, pnlNew;
        protected HtmlInputHidden hdn_CampaignId;
        protected HtmlInputHidden hdnCurrentMaskingNumber, hdnCurrentUserMobileNumber, hdnCurrentDealerType, hdnCurrentNcdBrandId, hdnCurrentMaskingNumberId, hdnCurrentDealerId, hdnCurrentCampaignId;
        protected Button btnProceed, btnResumeAndMap, btnMapCampaign;
        protected string dealerName = string.Empty, CampaignId = string.Empty, dealerNumber = string.Empty;
        protected ManageDealerCampaign dealerCampaign;
        protected DealerContractEntity dealerContract;
        #endregion

        #region Events

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (ParseQueryString())
            {
                SaveContractDetails(dealerContract);
                LoadCampaignForDealer();
            }
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 12 July 2016
        /// Description :   Saves the Contract Details into mapping table
        /// </summary>
        /// <param name="dealerContract"></param>
        private void SaveContractDetails(DealerContractEntity dealerContract)
        {
            try
            {
                if (dealerContract != null)
                {
                    dealerCampaign.SaveDealerContract(dealerContract);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, String.Format("BikewaleOpr.Campaign.MapCampaign.SaveContractDetails({0})", Newtonsoft.Json.JsonConvert.SerializeObject(dealerContract)));
                objErr.SendMail();
            }
        }

        /// <summary>
        /// Created By : Sangram Nandkhile on 22-March-2016.
        /// Description : Load campaigns for contract
        /// Modified by :   Sumit Kate on 12 July 2016
        /// Description :   Changed the Method Name. It shows the dealer campaigns
        /// </summary>
        private void LoadCampaignForDealer()
        {
            try
            {
                DealerCampaignBase campaigns = dealerCampaign.FetchBWCampaigns(dealerId);
                if (campaigns != null)
                {

                    dealerName = campaigns.DealerName;
                    dealerNumber = campaigns.DealerNumber;
                    if (campaigns.DealerCampaigns != null && campaigns.DealerCampaigns.Count() > 0)
                    {
                        rptCampaigns.DataSource = campaigns.DealerCampaigns;
                        rptCampaigns.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"] + "BikewaleOpr.Campaign.MapCampaign.LoadCampaignForContract");
                objErr.SendMail();
            }
        }


        #endregion

        #region Functions

        /// <summary>
        /// Created By : Sangram Nandkhile on 21st March 2016.
        /// Description : Parses query string to fetch campaign id, dealerid and dealerName
        /// </summary>
        private bool ParseQueryString()
        {
            bool isValid = true;
            try
            {
                if (!string.IsNullOrEmpty(Request.QueryString["dealerId"]))
                {
                    dealerId = Convert.ToInt32(Request.QueryString["dealerId"]);
                }
                else
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

                if (!string.IsNullOrEmpty(Request.QueryString["packageId"]))
                {
                    packageId = Convert.ToInt32(Request.QueryString["packageId"]);
                }


                if (!string.IsNullOrEmpty(Request.QueryString["packageName"]))
                {
                    packageName = Convert.ToString(Request.QueryString["packageName"]);
                }

                if (!string.IsNullOrEmpty(Request.QueryString["startDate"]))
                {
                    startDate = Convert.ToString(Request.QueryString["startDate"]);
                }

                if (!string.IsNullOrEmpty(Request.QueryString["endDate"]))
                {
                    endDate = Convert.ToString(Request.QueryString["endDate"]);
                }

                if (!string.IsNullOrEmpty(Request.QueryString["contractStatus"]))
                {
                    contractStatus = Convert.ToInt32(Request.QueryString["contractStatus"]);
                }

                if (isValid)
                {
                    if (contractId > 0 && contractStatus > 0)
                    {
                        dealerContract = new DealerContractEntity();
                        dealerContract.ContractId = contractId;
                        dealerContract.ContractStatus = contractStatus;
                        dealerContract.DealerId = dealerId;
                        dealerContract.EndDate = Convert.ToDateTime(endDate);
                        dealerContract.StartDate = Convert.ToDateTime(startDate);
                        dealerContract.PackageId = packageId;
                        dealerContract.PackageName = packageName;
                    }
                }
                dealerCampaign = new ManageDealerCampaign();
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"] + "BikewaleOpr.Campaign.MapCampaign.ParseQueryString");
                objErr.SendMail();
            }
            return isValid;
        }
        #endregion
    }
}