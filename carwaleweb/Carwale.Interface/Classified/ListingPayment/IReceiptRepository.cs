using Carwale.Entity.Classified.ListingPayment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Interfaces.Classified.ListingPayment
{
    public interface IReceiptRepository
    {
        bool Insert(Receipt receipt);
        List<Receipt> GetForInquiry(int inquiryId);
    }
}
