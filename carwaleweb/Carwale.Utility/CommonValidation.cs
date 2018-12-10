using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Utility
{
    public static class CommonValidation
    {
        public static bool IsValidBudget(string budget)
        {
            if (!string.IsNullOrWhiteSpace(budget))
            {
                int value = 0;
                int.TryParse(budget, out value);
                if (value > 0)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
