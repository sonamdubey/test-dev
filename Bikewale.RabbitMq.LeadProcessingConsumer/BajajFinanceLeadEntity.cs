
using System.Collections.Generic;
namespace Bikewale.RabbitMq.LeadProcessingConsumer
{
    public class BajajFinanceLeadEntity
    {

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobileNo { get; set; }
        public string EmailID { get; set; }

        public string ProductMake { get; set; }
        public string Model { get; set; }
        public string PinCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }

        public string LanNumber { get; set; }
        public string PreApprovedAmount { get; set; }
        public string DateOfBirth { get; set; }
        public string LikelyPurchaseDate { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }

        public string DealerCode { get { return "7704"; } }
        public string PrefLanguage { get { return "ENGLISH"; } }
        public string source { get { return "Web Sales"; } }
        public string PurchaseType { get { return "Finance"; } }
        public string assignto { get { return "Call Center"; } }
        public string subchannel { get { return "SC00058"; } }
        public string caseSourceFrom { get { return "Web"; } }
        public string Ext_UserID { get { return "Bikewale"; } }
        public string Ext_sysID { get { return "881"; } }
        public string icrm_user_id { get { return "2249"; } }
        public string AgentComment { get { return "LEAD"; } }
        public string ProductType { get { return "2W"; } }
    }


    public class BajajFinanceLeadInput
    {

        public string batch_id { get { return "1"; } }
        public string AuthKey { get { return "B40CD4625A4966EAFB451E256D483EA3_UAT"; } }
        public IEnumerable<BajajFinanceLeadEntity> leadData { get; set; }
    }

}
