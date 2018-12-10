using FluentValidation.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Stock.Certification
{
    [JsonObject, Serializable, Validator(typeof(StockCertificationSubItemValidator))]
    public class StockCertificationSubItem
    {
        public string Name { get; set; }
        public string Condition { get; set; }
        public int? LegendId { get; set; }
        public List<StockCertificationSubItemDetail> Details { get; set; }
    }
}
