using Carwale.Entity.PaymentGateway;
using Carwale.Entity.Template;

namespace Carwale.Interfaces.Deals
{
    public interface IDealsNotification
    {
        void SendDealsMailToDealer(TemplateContent templateContent, bool isPaymentSucess, GatewayResponse pgResponse);
        void SendDealsMailToCustomer(TemplateContent templateContent, bool isPaymentSucess, GatewayResponse pgResponse);
    }
}
