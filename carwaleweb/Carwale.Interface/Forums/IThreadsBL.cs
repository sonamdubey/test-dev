using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Interfaces.Forums
{
    public interface IThreadsBL
    {
        bool DeleteThread(string threadId, string customerId);
    }
}
