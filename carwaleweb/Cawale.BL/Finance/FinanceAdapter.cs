using Carwale.DTOs.Finance;
using Carwale.Entity.Finance;
using Carwale.Entity.Enum;
using Carwale.Notifications;
using Carwale.Interfaces.Finance;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Carwale.DAL.Finance;

namespace Carwale.BL.Finance
{
    public class FinanceAdapter : IFinanceAdapter
    {
        private IUnityContainer _container;

        public FinanceAdapter(IUnityContainer container)
        {
            _container = container;
        }

        public ClientResponseDto Get(FinanceLead inputs)
        {
            try
            {                        
                Clients client = inputs.ClientId;              
                IFinance<FinanceLead, ClientResponseDto> _iFinanceBL = GetBL(client);

                if (_iFinanceBL != null) return _iFinanceBL.SaveLead(inputs);
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "FinanceAdapter.Get() inputs=" + JsonConvert.SerializeObject(inputs));
                objErr.SendMail();
            }
            return default(ClientResponseDto);
        }

        public IFinance<FinanceLead, ClientResponseDto> GetBL(Clients client)
        {
            switch (client)
            {
                case Clients.HDFC: return _container.Resolve<IFinance<FinanceLead, ClientResponseDto>>("Hdfc");
                case Clients.Axis: return _container.Resolve<IFinance<FinanceLead, ClientResponseDto>>("Axis");
                default: return null;
            }
        }
    }
}
