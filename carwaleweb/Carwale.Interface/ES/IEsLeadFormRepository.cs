using Carwale.Entity.ES;

namespace Carwale.Interfaces.ES
{
    public interface IEsLeadFormRepository
    {
        int SubmitEsLeadFormData(EsLeadFormResponse customerResponse);
    }
}
