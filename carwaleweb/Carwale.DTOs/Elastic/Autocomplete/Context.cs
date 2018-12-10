using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.Elastic
{
    [Serializable]
    public class Context
    {
        public List<string> types { get; set; }
    }
}
