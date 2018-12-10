using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.PriceQuote
{
    public class PQUserInfoTrackEntity
    {
        public ulong PQId { get; set; }
        public string ClientIp { get; set; }
        public string AspSessoinId { get; set; }
        public string EntryDate { get; set; }
        public string CWCookievalue { get; set; }
    }
}
