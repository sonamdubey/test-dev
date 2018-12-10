using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Interfaces.Logs
{
    public interface ILoggingRepository
    {
        void LogRequestBody(string jsonData, string requestMethod);
    }
}
