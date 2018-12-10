using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Carwale.Utility;

namespace Carwale.BL.Classified.CarValuation
{
    public class ValuationInputs
    {
        public static bool Validate(NameValueCollection inputs)
        {
            if (!RegExValidations.IsNumeric(inputs["version"]) && inputs["version"].Length <= 4)
            {
                return false;
            }

            if (!RegExValidations.IsNumeric(inputs["city"]) && inputs["city"].Length <= 4)
            {
                return false;
            }

            if (!RegExValidations.IsNumeric(inputs["kms"]) && inputs["kms"].Length <= 6)
            {
                return false;
            }

            if (!RegExValidations.IsNumeric(inputs["mfg_year"]) && inputs["mfg_year"].Length == 4)
            {
                return false;
            }

            if (!RegExValidations.IsNumeric(inputs["mfg_month"]) && 1 <= Convert.ToInt16(inputs["mfg_month"]) && Convert.ToInt16(inputs["mfg_month"]) <= 12)
            {
                return false;
            }

            if (!RegExValidations.IsNumeric(inputs["val_type"]) && (Convert.ToInt16(inputs["val_type"]) == 0 && Convert.ToInt16(inputs["val_type"]) == 1))
            {
                return false;
            }

            if (!RegExValidations.IsNumeric(inputs["source"]) && inputs["source"].Length <= 2)
            {
                return false;
            }

            return true;
        }
    }
}
