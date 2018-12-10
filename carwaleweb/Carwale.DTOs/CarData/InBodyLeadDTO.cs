using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.CarData
{
    public class InBodyLeadDTO
    {
        public string MakeName { get; set; }
        public int ModelId { get; set; }
        public string ModelName { get; set; }
        public int VersionId { get; set; }
        public int MakeId { get; set; }
        public string PopupTitle { get; set; }
        public string DealerMobileNo { get; set; }
        public int DealerId { get; set; }
        public string PopupText { get; set; }
        public string ButtonText { get; set; }
        public int LeadPanel { get; set; }
        public int LeadBusinessType { get; set; }
        public int ActualDealerId { get; set; }
    }
}
