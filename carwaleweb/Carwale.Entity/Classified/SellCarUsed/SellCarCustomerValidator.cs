using Carwale.Utility;
using FluentValidation;
using FluentValidation.Results;

namespace Carwale.Entity.Classified.SellCarUsed
{
    public class SellCarCustomerValidator : AbstractValidator<SellCarCustomer>
    {
        public override ValidationResult Validate(SellCarCustomer customer)
        {
            return customer == null
                ? new ValidationResult(new[] { new ValidationFailure("customerdetails", "customer details can not be null") })
                : base.Validate(customer);
        }
        public SellCarCustomerValidator()
        {
            RuleFor(x => x.Name).NotEmpty().Length(2, 100).Matches(RegExValidations.NameRegex);
            RuleFor(x => x.Mobile).NotEmpty().Matches(RegExValidations.MobileRegex);
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.CityId).GreaterThan(0);
        }
    }    
}