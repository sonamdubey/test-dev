using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.ImageUpload
{
    public class Token
    {
        public string Key { get; set; }
        public string URI { get; set; }
        public string AccessKeyId { get; set; }
        public string Policy { get; set; }
        public string Signature { get; set; }
        public string DatetTmeISO { get; set; }
        public string DateTimeISOLong { get; set; }
    }

}
