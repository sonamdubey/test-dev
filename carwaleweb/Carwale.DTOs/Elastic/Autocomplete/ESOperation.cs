using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.Elastic
{
    [Serializable]
    public enum ESOperation
    {
        CREATE = 1,
        UPDATE = 2,
        DELETE = 3
    }
}
