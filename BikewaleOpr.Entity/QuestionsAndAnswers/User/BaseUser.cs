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
    /// Description: Parent class for User Details
    /// </summary>

    public class BaseUser
    {
        public uint Id { get; set; }
        public string Name { get; set; }
    }
}
