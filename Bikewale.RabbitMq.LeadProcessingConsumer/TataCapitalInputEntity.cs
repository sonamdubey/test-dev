using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.RabbitMq.LeadProcessingConsumer
{
    /// <summary>
    /// Created by : Sangram Nandkhile on 15 Jun 2017
    /// Entiry for tata capital
    /// </summary>
    public class TataCapitalInputEntity
    {
        public string source { get; set; } = "";
        public string password { get; set; } = "";
        public string fname { get; set; } = "";
        public string lname { get; set; } = "";
        public string resEmailId { get; set; } = "";
        public string resMobNo { get; set; } = "";
        public string resCity { get; set; } = "";
        public string sageProduct { get { return "Two Wheelers"; } }
        public string sagechannel { get { return "Bikewale"; } }
        public string leadType { get { return "Individual"; } }
        public string leadTag { get { return "WarmLead"; } }
        public string leadStage { get { return "NewLead"; } }
    }

    /*public string source { get; set; } = "";
        public string password { get; set; } = "";
        public string fname { get; set; } = "";
        public string mname { get; set; } = "";
        public string lname { get; set; } = "";
        public string title { get; set; } = "";
        public string resEmailId { get; set; } = "";
        public string gender { get; set; } = "";
        public string dob { get; set; } = "";
        public string resMobNo { get; set; } = "";
        public string resLandlineNo { get; set; } = "";
        public string resAddress1 { get; set; } = "";
        public string resAddress2 { get; set; } = "";
        public string resAddress3 { get; set; } = "";
        public string resCity { get; set; } = "";
        public string resPincode { get; set; } = "";
        public string resState { get; set; } = "";
        public string companyName { get; set; } = "";
        public string designation { get; set; } = "";
        public string officeEmailId { get; set; } = "";
        public string officeMobNo { get; set; } = "";
        public string leadDetails { get; set; } = "";
        public string salesOrg { get; set; } = "";
        public string sageProduct { get { return "Two Wheelers"; } }
        public string sagechannel { get { return "Bikewale"; } }
        public string leadType { get { return "Individual"; } }
        public string leadTag { get { return "WarmLead"; } }
        public string sageBranch { get; set; } = "";
        public string loanAmount { get; set; } = "";
        public string campaignId { get; set; } = "";
        public string tenure { get; set; } = "";
        public string motherMaidenName { get; set; } = "";
        public string maritalStatus { get; set; } = "";
        public string semCampaignName { get; set; } = "";
        public string semSource { get; set; } = "";
        public string semSiteId { get; set; } = "";
        public string semHeadLine { get; set; } = "";
        public string semCreativeId { get; set; } = "";
        public string semKeyword { get; set; } = "";
        public string pan { get; set; } = "";
        public string gclId { get; set; } = "";
        public string referralName { get; set; } = "";
        public string referralDob { get; set; } = "";
        public string referralMob { get; set; } = "";
        public string referralContractNo { get; set; } = "";
        public string referralEmpId { get; set; } = "";
        public string leadStage { get { return "NewLead"; } }
        public string companyCategory { get; set; } = "";
        public string monthlySalary { get; set; } = "";
        public string rejectionReason { get; set; } = "";
        public string sanctionedAmount { get; set; } = "";*/

}
