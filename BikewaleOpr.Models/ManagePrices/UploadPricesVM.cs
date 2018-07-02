namespace BikewaleOpr.Models.ManagePrices
{
    /// <summary>
    /// Created By : Prabhu Puredla on 22 May 2018
    /// Description : ViewModel for landing page of Bulk Price upload. 
    /// </summary>
    public class UploadPricesVM
    {
        public MakesAndStatesVM MakesAndStatesListVM { get; set; }
        public uint MakeId { get; set; }
        public string MakeName { set; get; }
    }
}
