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
    [Validator(typeof(StockDeleteValidator))]
    public class StockDelete
    {
        public int SellerId { get; set; }

        public int SellerType { get; set; }

        public int SourceId { get; set; }

        public List<int> Ids { get; set; }      
    }
}
