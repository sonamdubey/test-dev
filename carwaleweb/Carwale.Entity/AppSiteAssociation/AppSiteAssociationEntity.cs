using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
/// <summary>
/// Author: Ajay Singh(on 16 feb 2016)
/// Description : For IOS purpose
/// </summary>

namespace Carwale.Entity.AppSiteAssociation
{
    public class AppSiteAssociationEntity
    {
        [JsonProperty("applinks")]
        public AppSiteDetailAppLinks AppLinks { get; set; }
    }

    public class AppSiteDetailAppLinks
    {
        [JsonProperty("apps")]
        public List<string> Apps { get; set; }
        [JsonProperty("details")]
        public List<AppSiteDetail> Details { get; set; }
    }

    public class AppSiteDetail
    {
        [JsonProperty("appID")]
        public string AppID { get; set; }
        [JsonProperty("paths")]
        public List<string> Paths { get; set; }
     
    }

}
