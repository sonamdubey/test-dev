using System;

namespace Bikewale.UserReviewsCommunication
{
    /// <summary>
    /// Created by : Aditi Srivastava on 15 Apr 2017
    /// Summary    : Entity for user review email
    /// </summary>
    public class UserReviewMailEntity
    {
        public uint ReviewId {get;set;}
        public string MakeName {get;set;}
        public string ModelName {get;set;}
        public uint CustomerId {get;set;}
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public DateTime EntryDate { get; set; }
        public string ReviewLink { get; set; }
    }
}
