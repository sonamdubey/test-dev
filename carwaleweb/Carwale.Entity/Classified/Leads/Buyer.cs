using FluentValidation.Attributes;
using System;
using System.Globalization;

namespace Carwale.Entity.Classified.Leads
{
    [Validator(typeof(BuyerValidator))]
    public class Buyer
    {
        private string _name;

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    if(string.IsNullOrWhiteSpace(value)) //if name contains only white spaces then don't trim, so that it will fail further validation
                    {
                        _name = value;
                    }
                    else
                    {
                        _name = (String.Compare(value, "unknown", StringComparison.OrdinalIgnoreCase) == 0) ? null : CultureInfo.CurrentCulture.TextInfo.ToTitleCase(value.ToLower().Trim());
                    }
                }
            }
        }
        public string Mobile { get; set; }

        private string _email;
        public string Email
        {
            get
            {
                return _email;
            }
            set
            {
                _email = value.ToLower();
            }
        }
    }
}
