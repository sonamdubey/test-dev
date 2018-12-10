using Carwale.DTOs.Finance;
using Carwale.Entity.Finance;
using Carwale.Entity.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Interfaces.Finance
{
    public interface IFinanceAdapter
    {
        ClientResponseDto Get(FinanceLead inputs);
        IFinance<FinanceLead, ClientResponseDto> GetBL(Clients client);
    }
}
