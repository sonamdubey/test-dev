using Carwale.Entity.SEO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity
{
    [Serializable]
    public class MetaTags
    {
        public string Title { get; set; }
        public string Heading { get; set; }
        public string Keywords { get; set; }
        public string Description { get; set; }
        public Annotations Annotation { get; set; }
    }
}
