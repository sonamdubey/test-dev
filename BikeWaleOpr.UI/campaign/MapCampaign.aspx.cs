using BikewaleOpr.Common;
using BikeWaleOpr.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
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
        protected string dealerId = string.Empty;
        protected int contractId;
        protected Panel pnlExisting, pnlNew;
        protected HtmlInputHidden hdn_CampaignId;
        protected HtmlInputHidden hdnCurrentMaskingNumber, hdnCurrentUserMobileNumber, hdnCurrentDealerType, hdnCurrentNcdBrandId, hdnCurrentMaskingNumberId, hdnCurrentDealerId, hdnCurrentCampaignId;
        protected Button btnProceed, btnResumeAndMap;
        protected string dealerName = string.Empty, CampaignId = string.Empty;
        protected ManageDealerCampaign dealerCampaign;
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
            ParseQueryString();
            if (!IsPostBack)
            {
                LoadCampaignForContract();
            }
        }

        /// <summary>
        /// Created By : Sangram Nandkhile on 22-March-2016.
        /// Description : Load campaigns for contract
        /// </summary>
        private void LoadCampaignForContract()
        {
            try
            {
                dealerCampaign = new ManageDealerCampaign();
                DataTable dtCampaigns = dealerCampaign.FetchBWCampaigns(contractId);
                if (dtCampaigns != null && dtCampaigns.Rows.Count > 0)
                {
                    dealerId = dtCampaigns.Rows[0]["DealerId"].ToString();
                    dealerName = dtCampaigns.Rows[0]["Organization"].ToString();
                    rptCampaigns.DataSource = dtCampaigns;
                    rptCampaigns.DataBind();
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
        private void ParseQueryString()
        {
            try
            {
                if (Request.QueryString["contractid"] == null)
                {
                    //page not found
                    Response.Redirect("../pagenotfound.aspx");
                }
                if (!string.IsNullOrEmpty(Request.QueryString["contractid"]))
                {
                    contractId = Convert.ToInt32(Request.QueryString["contractid"]);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"] + "BikewaleOpr.Campaign.MapCampaign.ParseQueryString");
                objErr.SendMail();
            }
        }

        protected string GetVisibility(string leadPanel, string maskingNum)
        {
            if (maskingNum == "")
                return "display:block;";
            else
                return "display: none;";
        }

        #endregion


    }
}