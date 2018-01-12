
namespace Bikewale.Entities.UserReviews
{
    /// <summary>
    /// Creayed by Sajal Gupta on 04-09-2017
    /// Description : This entity holds submit data of form rating reviews form
    /// Modified By :   Vishnu Teja Yalakuntla on 07 Sep 2017
    /// Description :   Added CustomerId
    /// Modified By : Sanskar Gupta 09/01/2018
    /// Description : Added the bool field called "fromParamterRatingPage" to know the page from which the request has been made.
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
        public ulong CustomerId { get; set; }
        public string EncodedString { get; set; }
        public bool? IsDesktop { get; set; }
        public uint Mileage { get; set; }

        public bool? fromParamterRatingPage { get; set; }
    }
}
