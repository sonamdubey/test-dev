using AutoMapper;
using Carwale.Adapters.Mappers;

namespace Carwale.Adapters
{
    public static class AdapterMappers
    {
        public static void CreateMaps(IConfiguration config)
        {
            config.AddProfile<VerNamProfile>();
        }
    }
}
