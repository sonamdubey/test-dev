using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.CMS.URIs
{
    /// <summary>
    /// created by natesh on 13/11/14
    /// for querystring to get make details
    /// </summary>
    public class MakeDetailURI
    {
        [JsonProperty(PropertyName = "applicationid")]
        public ushort ApplicationId { get; set; }

        [JsonProperty(PropertyName = "categoryidlist")]
        public string CategoryIdList { get; set; }
    }


    /// <summary>
    /// created by natesh on 13/11/14
    /// for querystring to get model details
    /// </summary>
    public class ModelDetailURI : MakeDetailURI
    {
        [JsonProperty(PropertyName = "makeid")]
        public uint MakeId { get; set; }
    }
}
