using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity
{
    [Serializable]
    public class Customer : CustomersBasicInfo
    {
        public string FacebookId { get; set; }
        public string GoogleId { get; set; }
        public string CustomerId { get; set; }
        public string Password { get; set; }
        public string SecurityKey { get; set; }
        public string PasswordSalt { get; set; }
        public string PasswordHash { get; set; }
        public string OAuth { get; set; }
        public DateTime RegistrationDateTime { get; set; }
        public int SourceId { get; set; }
        public bool ReceiveNewsletters { get; set; }
        public bool IsEmailVerified { get; set; }
        public bool IsVerified { get; set; }
        public bool IsFake { get; set; }
        public bool IsApproved { get; set; }
        public bool openUserVerified { get; set; }


        public string PasswordSaltHashStr { get; set; }
        public string StatusOnRegister { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
    }

    [Serializable]
    public class CustomerLocation
    {
        public int StateId { get; set; }
        public int CityId { get; set; }
        public string ZoneId { get; set; }
        public string State { get; set; }
        public string City { get; set; }
    }
    [JsonObject]
    [Serializable]
    public class CustomersBasicInfo : CustomerLocation
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
    }

    [Serializable]
    public class CustomerOnRegister
    {
        public string CustomerId { get; set; }
        public string OAuth { get; set; }
        public string StatusOnRegister { get; set; }
        public bool IsApproved { get; set; }
    }

    [Serializable]
    public class CustomerRememberMe
    {
        public string CustomerId { get; set; }
        public string Identifier { get; set; }
        public string AccessToken { get; set; }
        public string DateCreated { get; set; }
        public string DateUpdated { get; set; }
        public string IsActive { get; set; }
        public string IPAddress { get; set; }
        public string UserAgent { get; set; }
        public string SessionCount { get; set; }
        public string IsHacked { get; set; }
    }
}
