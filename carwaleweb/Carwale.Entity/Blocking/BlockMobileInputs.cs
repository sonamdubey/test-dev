using FluentValidation.Attributes;
using System.Collections.Generic;

namespace Carwale.Entity.Blocking
{
    [Validator(typeof(BlockMobileInputsValidator))]
    public class BlockMobileInputs
    {
        public IEnumerable<string> MobileNos { get; set; }
        public string Reason { get; set; }
    }
}
