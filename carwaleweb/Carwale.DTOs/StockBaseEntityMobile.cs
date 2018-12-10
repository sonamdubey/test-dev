using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs
{
    public class StockBaseEntityMobile
    {
        public string ProfileId { get; set; }
        public string CarName { get; set; }
        public string Url { get; set; }
        public string FrontImagePath { get; set; }
        public string Price { get; set; }
        public string Km { get; set; }
        public string MakeYear { get; set; }
        public bool IsPremium { get; set; }
    }
}
