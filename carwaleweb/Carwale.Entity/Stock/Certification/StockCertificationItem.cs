using FluentValidation.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Stock.Certification
{
    [JsonObject, Serializable, Validator(typeof(StockCertificationItemValidator))]
    public class StockCertificationItem
    {
        public int? CarItemId { get; set; }

        [JsonIgnore]
        public string Name { get; set; }

        public float? Score { get; set; }

        public int? ScoreColorId { get; set; }

        [JsonIgnore]
        public string ScoreColor { get; set; }

        public string Condition { get; set; }

        public List<StockCertificationSubItem> SubItems { get; set; }
    }
}
