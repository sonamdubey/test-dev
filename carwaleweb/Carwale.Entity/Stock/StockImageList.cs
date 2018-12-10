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
    [Serializable, JsonObject, Validator(typeof(StockImagesValidator))]
    public class StockImageList
    {
        public int? StockId { get; set; }

        public int? SellerType { get; set; }

        public int? SourceId { get; set; }

        public List<StockImage> StockImages { get; set; }
    }
}
