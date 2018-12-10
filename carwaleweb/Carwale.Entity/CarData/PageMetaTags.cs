using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.CarData
{
    /// <summary>
    /// Created By : Shalini Nair
    /// </summary>
    [Serializable]
    public class PageMetaTags
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Keywords { get; set; }
        public string Heading { get; set; }
        public string Summary { get; set; }
        public string Canonical { get; set; }
        public string DeeplinkAlternativesAndroid { get; set; }
        public string DeeplinkAlternatives { get; set; }
        public string PrevUrl { get; set; }
        public string NextUrl { get; set; }
        public string Amphtml { get; set; }
        public string Alternate { get; set; }
        public int Id { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
