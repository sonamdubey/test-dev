using Carwale.Entity.Classified.Leads;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Interfaces.Classified.Leads
{
    public interface ISellerCacheRepository
    {
        Seller GetIndividualSeller(int inquiryId);
    }
}
