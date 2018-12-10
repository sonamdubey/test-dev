using FluentValidation;
using FluentValidation.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Stock
{
    [Serializable, JsonObject, Validator(typeof(StockImageValidator))]
    public class StockImage
    {
        public int? Id { get; set; }

        public string Url { get; set; }

        public string AltText { get; set; }

        public string Title { get; set; }

        public bool? DefaultImg { get; set; }
    }
}
