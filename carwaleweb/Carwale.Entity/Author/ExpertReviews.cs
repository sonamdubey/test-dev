using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Author
{
    [Serializable]
    public class ExpertReviews
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public string MaskingName { get; set; }
        public string MakeName { get; set; }
        public int CategoryId { get; set; }
        public int BasicId { get; set; }
    }
}
