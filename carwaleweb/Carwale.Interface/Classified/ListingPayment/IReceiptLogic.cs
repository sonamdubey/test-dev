using Carwale.Entity.Classified.ListingPayment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Interfaces.Classified.ListingPayment
{
    public interface IReceiptLogic
    {
        bool UploadPdf(Receipt receipt);
        bool SendNotification(Receipt receipt);
    }
}
