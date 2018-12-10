using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Carwale.Entity.Enum;
using FluentValidation;

namespace Carwale.Entity.Classified.SellCarUsed
{
    public class Listing
    {
        public Entity.Enum.SellerType SellerType { get; set; }
        public ClassifiedStockSource StockSource { get; set; }
        public List<ListingDetails> ListingDetails { get;set; }
    }

    public class ListingDetails 
    {
        public int InquiryId { get; set; }
        public ListingStatus Status { get; set; }
    }
}
