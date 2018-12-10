using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Classified
{
    public class MyPaymentsEntity
    {
        public int Id { get; set; }
        public int ActualAmount { get; set; }
        public string Package { get; set; }
        public int Invoice { get; set; }
        public string EntryDate { get; set; }
    }
}
