using System;
using System.Runtime.Serialization;

namespace BikewaleOpr.Entities
{
    /// Created by: Aditi Srivastava on 01 Sep 2016
    /// Summary: To save and update dealer offers
    [Serializable, DataContract]
    public class DealerOffersEntity
    {
        [DataMember]
        public uint OfferId {get;set;} 
        [DataMember]
        public uint UserId {get;set;} 
        [DataMember]
        public uint OfferCategoryId {get;set;}
        [DataMember]
        public string OfferText {get;set;}
        [DataMember]
        public uint OfferValue {get;set;}
        [DataMember]
        public DateTime OfferValidTill {get;set;}
        [DataMember]
        public bool IsPriceImpact {get;set;}
        [DataMember]
        public string Terms { get; set; }
    }
}
