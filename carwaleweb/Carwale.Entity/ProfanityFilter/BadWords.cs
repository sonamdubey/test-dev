using FluentValidation.Attributes;
using System.Collections.Generic;

namespace Carwale.Entity.ProfanityFilter
{
    [Validator(typeof(BadWordsValidator))]
    public class BadWords
    {
        public IEnumerable<string> Words { get; set; }
    }
}
