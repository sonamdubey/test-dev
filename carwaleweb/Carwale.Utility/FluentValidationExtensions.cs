using FluentValidation.Validators;

namespace FluentValidation
{
    public static class FluentValidationExtensions
    {
        public static IRuleBuilderOptions<T, TProperty> IsValidInteger<T, TProperty>(this IRuleBuilder<T, TProperty> ruleBuilder)
        {
            return ruleBuilder.SetValidator(new IntegerValidator<TProperty>());
        }
        public static IRuleBuilder<T, TProperty> StrictlyBetween<T, TProperty>(this IRuleBuilder<T, TProperty> ruleBuilder, int from, int to)
        {
            return ruleBuilder.SetValidator(new RangeValidator<TProperty>(from, to));
        }
    }
    public class IntegerValidator<T> : PropertyValidator
    {
        public IntegerValidator() : base("The value provided is not a valid integer") { }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            int parsedInteger;
            return int.TryParse(context.PropertyValue as string, out parsedInteger);
        }
    }
    public class RangeValidator<T> : PropertyValidator
    {
        private int _from = 0;
        private int _to = 0;
        public RangeValidator(int from, int to)
            : base("The value is outside range, should be in between " + from + " and " + to)
        {
            _from = from;
            _to = to;
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            int parsedInteger;
            return (int.TryParse(context.PropertyValue as string, out parsedInteger) && parsedInteger >= _from && parsedInteger <= _to);
        }
    }
}
