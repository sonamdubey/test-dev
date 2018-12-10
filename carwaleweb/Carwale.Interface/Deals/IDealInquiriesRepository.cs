using Carwale.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Interfaces.Deals
{
       public interface IDealInquiriesRepository
    {
           bool PushMultipleLeads(DealsInquiryDetail dealsInquiry);
    }
}
