using Carwale.Entity.Classified.UsedLeads;
using System.Collections.Generic;

namespace Carwale.Interfaces.Classified.Leads
{
    public interface IUsedLeadsRepository
    {
        DealerLeadsCount GetDealerLeadsCount(int dealerId, int month, int year);
    }
}