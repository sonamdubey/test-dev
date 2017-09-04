
namespace Bikewale.Entities.UserReviews
{
    /// <summary>
    /// Creayed by Sajal Gupta on 04-09-2017
    /// Description : This entity holds submit data of form rating reviews form
    /// </summary>
    public class ReviewSubmitData
    {
        public string ReviewDescription { get; set; }
        public string ReviewTitle { get; set; }
        public string ReviewQuestion { get; set; }
        public string ReviewTips { get; set; }
        public string EncodedId { get; set; }
        public string EmailId { get; set; }
        public string UserName { get; set; }
        public string MakeName { get; set; }
        public string ModelName { get; set; }
        public uint ReviewId { get; set; }
        public string EncodedString { get; set; }
        public bool? IsDesktop { get; set; }
        public string Mileage { get; set; }        
    }
}
