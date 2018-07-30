using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.DTO.QuestionAndAnswers
{
    /// <summary>
    /// Created By : Deepak Israni on 11 June 2018
    /// Description: Tag DTO to store information about tags.
    /// </summary>
    public class Tag
    {
        [JsonProperty("id")]
        public uint Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
