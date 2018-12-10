using FluentValidation.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Stock.Certification
{
    [JsonObject, Serializable, Validator(typeof(StockCertificationSubItemDetailValidator))]
    public class StockCertificationSubItemDetail
    {
        public string Text { get; set; }
        public int? LegendId { get; set; }
    }
}
