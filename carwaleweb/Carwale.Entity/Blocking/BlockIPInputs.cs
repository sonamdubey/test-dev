using FluentValidation.Attributes;
using System.Collections.Generic;

namespace Carwale.Entity.Blocking
{
    [Validator(typeof(BlockIPInputsValidator))]
    public class BlockIPInputs
    {
        public IEnumerable<string> IpAddresses { get; set; }
        public string Reason { get; set; }
    }
}
