using FluentValidation.Attributes;
using System;

namespace Carwale.Entity.Stock
{
    public class StockSortScore
    {
        public double SortScore { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
