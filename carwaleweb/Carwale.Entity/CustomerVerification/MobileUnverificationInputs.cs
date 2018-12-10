using FluentValidation.Attributes;
using System.Collections.Generic;

namespace Carwale.Entity.CustomerVerification
{
    [Validator(typeof(MobileUnverificationInputsValidator))]
    public class MobileUnverificationInputs
    {
        public IEnumerable<string> MobileNos { get; set; }
    }
}
