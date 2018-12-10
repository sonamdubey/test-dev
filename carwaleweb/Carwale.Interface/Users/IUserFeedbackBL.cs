using Carwale.Entity.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Interfaces.Users
{
    public interface IUserFeedbackBL
    {
        void ProcessFeedback(UserFeedback feedback);
    }
}