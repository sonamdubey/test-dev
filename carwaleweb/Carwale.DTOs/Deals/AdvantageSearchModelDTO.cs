using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.Deals
{
    public class AdvantageSearchModelDTO
    {
        public AdvantageSearchFilterDTO Filters { get; set; }
        public List<City> Cities { get; set; }
        public bool IsAdvantageCity { get; set; }
        public City SelectedCity { get; set; }

    }
}


