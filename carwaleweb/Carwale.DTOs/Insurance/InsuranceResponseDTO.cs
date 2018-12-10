using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.Insurance
{
    public class InsuranceResponseDTO
    {
        public string RedirectUrl { get; set; }
        public bool IsQuoteAvailable { get; set; }
        public long UniqueId { get; set; }
    }
}
