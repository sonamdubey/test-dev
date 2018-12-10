using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.XmlFeed
{
    public class url
    {
        public string loc { get; set; }
    }

    public class SociomanticProduct
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string Brand { get; set; }
        public DateTime Availablity { get; set; }
        public int SalePrice { get; set; }
        public int RegularPrice { get; set; }
        public string Currency { get; set; }
        public string Url { get; set; }
        public string ImageUrl { get; set; }
    }
}
