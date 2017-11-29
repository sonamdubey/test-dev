using Newtonsoft.Json;

namespace BikewaleOpr.Entity
{
    public  class BannerDetails
    {
        
        [JsonProperty("horizontalposition")]
        public string HorizontalPosition { get; set; }
        [JsonProperty("verticalposition")]
        public string VerticalPosition { get; set; }
        [JsonProperty("backgroundcolor")]
        public string BackgroundColor { get; set; }
        
        public string HostUrl { get; set; }
        
        public string OriginalImagePath { get; set; }
        [JsonProperty("bannertitle")]
        public string BannerTitle { get; set; }
        [JsonProperty("buttonposition")]
        public string ButtonPosition { get; set; }
        [JsonProperty("buttontype")]
        public ushort ButtonType { get; set; }
        [JsonProperty("buttoncolor")]
        public string ButtonColor { get; set; }
        [JsonProperty("buttontext")]
        public string ButtonText { get; set; }

        [JsonProperty("target")]
        public ushort Target { get; set; }

        [JsonProperty("targethref")]
        public string TargetHref { get; set; }

        [JsonProperty("jumbotrondepth")]
        public string JumbotronDepth { get; set; }

        [JsonProperty("html")]
        public string HTML { get; set; }
        [JsonProperty("unmodifiedhtml")]
        public string UnModifiedHtml { get; set; }

        [JsonProperty("js")]
        public string JS { get; set; }
     

        [JsonProperty("css")]
        public string CSS { get; set; }
        [JsonProperty("unmodifiedcss")]
        public string UnModifiedCSS { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }
        [JsonProperty("action")]
        public string Action { get; set; }
        [JsonProperty("label")]
        public string Label { get; set; }

    }
}
