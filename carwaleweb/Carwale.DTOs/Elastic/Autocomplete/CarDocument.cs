using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.Elastic
{
    [Serializable]
    public class CarDocument
    {
        public ESCarDocument doc { get; set; }
        public ESOperation action { get; set; }
    }
}
