using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Carwale.DTOs.Stock.Validators
{
    public class StockCitiesValidator: AbstractValidator<StockCitiesDTO>
    {
        public StockCitiesValidator()
        {
            RuleFor(x => x.CityIds).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty()
                .Must<StockCitiesDTO, List<int>>(cityIds => cityIds.Count < 51).WithMessage("City Ids must not contain more then 50 cityids.")
                .Must<StockCitiesDTO, List<int>>(cityIds => cityIds.All(cityId => cityId > 0)).WithMessage("City Ids must be positive");
        }
    }
}
