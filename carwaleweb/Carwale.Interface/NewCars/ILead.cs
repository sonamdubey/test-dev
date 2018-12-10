using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Interfaces.NewCars
{
    public interface ILead
    {
        bool RepushLead(List<string> LeadIds);
    }
}
