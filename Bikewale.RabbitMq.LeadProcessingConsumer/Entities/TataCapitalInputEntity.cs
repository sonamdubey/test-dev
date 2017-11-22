namespace Bikewale.RabbitMq.LeadProcessingConsumer
{
    /// <summary>
    /// Created by : Sangram Nandkhile on 15 Jun 2017
    /// Entiry for tata capital
    /// </summary>
    public class TataCapitalInputEntity
    {
        private string _fname = string.Empty;
        private string _lname = string.Empty;
        private string _resEmailId = string.Empty;
        private string _resMobNo = string.Empty;
        private string _resCity = string.Empty;
        public string fname { get { return _fname; } set { _fname = value; } }
        public string lname { get { return _lname; } set { _lname = value; } }
        public string resEmailId { get { return _resEmailId; } set { _resEmailId = value; } }
        public string resMobNo { get { return _resMobNo; } set { _resMobNo = value; } }
        public string resCity { get { return _resCity; } set { _resCity = value; } }

        public string source { get { return "Bikewale"; } }
        public string password { get { return "Bikewale@123"; } }
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