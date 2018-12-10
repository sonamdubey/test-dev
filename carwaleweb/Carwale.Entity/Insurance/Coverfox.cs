using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Insurance.Coverfox
{
    #region BaseResponse
    public class Base
    {
        [JsonProperty("errorCode")]
        public int? ErrorCode { get; set; }

        [JsonProperty("errorMessage")]
        public string ErrorMessage { get; set; }

        [JsonProperty("errorItems")]
        public object[] ErrorItems { get; set; }

        [JsonProperty("success")]
        public bool Success { get; set; }
    }
    #endregion

    #region Model
    public class ModelsResponse : Base
    {
        [JsonProperty("data")]
        public ModelWrapper Data { get; set; }
    }

    public class ModelWrapper
    {
        [JsonProperty("models")]
        public List<Model> Models { get; set; }
    }
    public class Model
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
    #endregion

    #region Version
    public class VersionsResponse : Base
    {
        [JsonProperty("data")]
        public VersionWrapper Data { get; set; }
    }

    public class VersionWrapper
    {
        [JsonProperty("variants")]
        public List<Version> Versions { get; set; }
    }

    public class Version
    {
        [JsonProperty("fuel_type")]
        public string FuelType { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
    #endregion

    #region Cities
    public class RTOsResponse : Base
    {
        [JsonProperty("data")]
        public RTOsWrapper Data { get; set; }
    }

    public class RTOsWrapper
    {
        [JsonProperty("rtos")]
        public List<RTO> RTOs { get; set; }
    }

    public class RTO
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
    #endregion

    #region Quote
    public class TokenResponse
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }
    }

    public class LeadResponse
    {
        [JsonProperty("success")]
        public bool Success { get; set; }
    }

    public class QuoteResponse : Base
    {
        [JsonProperty("data")]
        public QuoteWrapper Data { get; set; }
    }

    public class QuoteWrapper
    {
        [JsonProperty("quoteId")]
        public string QuoteId { get; set; }

        [JsonProperty("premiums")]
        public List<Quote> Quotes { get; set; }
    }

    public class Quote : IComparable<Quote>
    {
        [JsonProperty("insurerId")]
        public int Id { get; set; }

        [JsonProperty("insurerName")]
        public string Name { get; set; }

        [JsonProperty("final_premium")]
        public double Premium { get; set; }

        int IComparable<Quote>.CompareTo(Quote other)
        {
            if (other.Premium > this.Premium)
                return -1;
            else if (other.Premium == this.Premium)
                return 0;
            else
                return 1;
        }
    }
    #endregion
}
