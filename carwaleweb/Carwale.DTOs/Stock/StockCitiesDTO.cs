using Carwale.DTOs.Stock.Validators;
using FluentValidation.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.Stock
{
    [Validator(typeof(StockCitiesValidator))]
    public class StockCitiesDTO
    {
        public List<int> CityIds { get; set; }
    }
}
