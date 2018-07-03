using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikewaleOpr.Entity.QnA.Answer
{
    /// <summary>
    /// Created by: Dhruv Joshi
    /// Dated: 8th June 2018
    /// Description: Contains basic answer text
    /// </summary>
        
    public class Answer
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
    }
}
