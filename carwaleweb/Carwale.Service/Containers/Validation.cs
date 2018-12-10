using Carwale.Interfaces.Validations;
using Carwale.Service.Adapters.Validations;
using Microsoft.Practices.Unity;

namespace Carwale.Service.Containers
{
    public static class Validation
    {
        public static void RegisterTypes(UnityContainer container)
        {
            container.RegisterType<IValidateMmv, ValidateMmv>()
                .RegisterType<IValidateLocation, ValidateLocation>();
        }
    }
}
