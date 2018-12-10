using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Utility.Classified
{
    public static class StockValidations
    {
        public static bool IsProfileIdValid(string profileId)
        {
            if (!String.IsNullOrEmpty(profileId) && profileId.Length > 1)
            {
                char prefix = profileId[0];
                if (char.ToUpper(prefix) == 'D' || char.ToUpper(prefix) == 'S')
                {
                    string inquiryId = profileId.Substring(1);
                    int inqId;
                    return int.TryParse(inquiryId, out inqId) && inqId > 0;
                }
            }
            return false;
        }
    }
}
