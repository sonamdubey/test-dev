namespace BikewaleOpr.Entities.BikeData
{
    /// <summary>
    /// Modified by : Aditi  Srivastava on 24 May 2017
    /// Summary     : Added oldmaskingname property for mails on masking name change
    /// Modified by sajal Gupta on 20-11-2017
    /// Desc : Added MakeFooterAdded
    /// Modified by : Sanskar Gupta on 27 April 2018
    /// Description : Added property `OldMakeName`
    /// </summary>
    public class BikeMakeEntity : BikeMakeEntityBase
    {
        public bool Futuristic { get; set; }
        public bool New { get; set; }
        public bool Used { get; set; }
        public string CreatedOn { get; set; }
        public string UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public string OldMakeMasking { get; set; }
        public bool MakeFooterAdded { get; set; }
        public string OldMakeName { get; set; }
    }
}
