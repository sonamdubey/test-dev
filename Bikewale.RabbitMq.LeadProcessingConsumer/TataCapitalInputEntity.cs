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
}
