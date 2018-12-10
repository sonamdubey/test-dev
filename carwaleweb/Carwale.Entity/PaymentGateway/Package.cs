using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.PaymentGateway
{

    public class Package
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Validity { get; set; }
        public int Amount { get; set; }
        public string Description { get; set; }

    }

}
