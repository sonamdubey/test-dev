using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.DTO
{
    public class ReviewBase
    {
        public int ReviewId { get; set; }
        public string ReviewTitle { get; set; }        
        public DateTime ReviewDate { get; set; }
        public string WrittenBy { get; set; }
    }
}
