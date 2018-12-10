using Carwale.Entity.Classified.UsedLeads;

namespace Carwale.Interfaces.Classified.Leads
{
    public interface ILeadCacheRepository
    {
        DealerLeadsCount GetLeadsCountForCurrentMonth(int dealerId);
        DealerLeadsCount GetLeadsCountForLastMonth(int dealerId);
        DealerLeadsCount GetLeadsCountForSecondLastMonth(int dealerId);
    }
}
