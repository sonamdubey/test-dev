using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Blocking
{
    public class BlockedCommunicationRequest
    {
        public IEnumerable<BlockedCommunication> Communications { get;set;}
        public bool IsAllSuccess { get; set; }
    }
}
