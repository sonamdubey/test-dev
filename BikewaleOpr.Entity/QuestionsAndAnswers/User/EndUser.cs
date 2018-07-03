using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikewaleOpr.Entity.QnA.User
{
    /// <summary>
    /// Created by: Dhruv Joshi
    /// Dated: 8th June 2018
    /// Description: End Uesr, ie, the one who asks question
    /// </summary>
    
    public class EndUser: BaseUser 
    {
        public string Email { get; set; }
    }
}
