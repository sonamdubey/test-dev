using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity
{
    public class APIResponseEntity
    {
        public int StatusCode { get; set; }
        public ulong ResponseId { get; set; }
        public string ResponseText { get; set; }        
    }
}
