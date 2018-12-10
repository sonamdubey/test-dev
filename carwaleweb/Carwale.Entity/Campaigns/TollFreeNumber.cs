using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Campaigns
{
    [Serializable]
    public class TollFreeNumber
    {
        public int Id { get; set; }
        public string Number { get; set; }
    }
}
