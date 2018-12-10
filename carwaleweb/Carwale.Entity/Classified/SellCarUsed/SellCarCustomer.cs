
using FluentValidation.Attributes;
using Newtonsoft.Json;
using System;

namespace Carwale.Entity.Classified.SellCarUsed
{
    [JsonObject, Serializable, Validator(typeof(SellCarCustomerValidator))]
    public class SellCarCustomer
    {
        [JsonIgnore]
        public int Id { get; set; }
        [JsonIgnore]
        public int TempInquiryId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public int CityId { get; set; }
    }
}