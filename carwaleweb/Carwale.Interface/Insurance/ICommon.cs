using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Interfaces.Insurance
{
    public interface ICommon
    {
        double GetInsurancePremium(string carVersionId, string strCityId, int year);
    }
}
