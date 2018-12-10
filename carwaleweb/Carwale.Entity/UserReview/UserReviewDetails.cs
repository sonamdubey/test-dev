using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.UserReview
{
   public class UserReviewDetails
    {
       public int Id { get; set; }
       public string Title { get; set; }
       public string Description { get; set; }
       public int ExteriorStyle { get; set; }
       public int Comfort { get; set; }
       public int Performance { get; set; }
       public int FuelEconomy { get; set; }
       public int ValueForMoney { get; set; }
    }
}
