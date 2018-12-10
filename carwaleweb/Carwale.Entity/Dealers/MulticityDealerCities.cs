using FluentValidation.Attributes;
using System.Collections.Generic;

namespace Carwale.Entity.Dealers
{
    [Validator(typeof(MulticityDealerCitiesValidator))]
    public class MulticityDealerCities
    {
        public IEnumerable<int> CityIds { get; set; }
    }
}
