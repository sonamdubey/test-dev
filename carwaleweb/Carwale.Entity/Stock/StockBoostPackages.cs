using FluentValidation.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Carwale.Entity.Stock
{
    [Validator(typeof(StockBoostPackagesValidator))]
    public class StockBoostPackages
    {
        public int? BoostPackageId { get; set; }
    }
}
