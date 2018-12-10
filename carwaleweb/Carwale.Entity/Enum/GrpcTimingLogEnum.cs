using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Enum
{
    public enum GrpcTimingLogEnum
    {
        T101, // No Channel Available
        T102, // Channel is not Idle/Ready
        T103, // Disposing Channels
        T104, // RPC exception occurred, retrying with another Channel
        T105, // Log Function call Timing if greater than GrpcCallTimeLimitCheckValue from config
        T106
    }
}
