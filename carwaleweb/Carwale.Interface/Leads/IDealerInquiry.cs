using Carwale.Entity.Dealers;
using Carwale.Entity.Leads;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Interfaces.Leads
{
    public interface IDealerInquiry
    {
        ulong ProcessRequest(DealerInquiryDetails inquiriesDetails);
        List<ulong> DealerInquiries(DealerInquiry inquiries);
    }
}
