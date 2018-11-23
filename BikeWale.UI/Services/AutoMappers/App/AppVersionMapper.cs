using AutoMapper;

namespace Bikewale.Service.AutoMappers.App
{
    /// <summary>
    /// Author      :   Sumit Kate
    /// Created on  :   07 Dec 2015
    /// Description :   Automapper config for AppVersion Entity to DTO
    /// </summary>
    public class AppVersionMapper
    {
        internal static DTO.App.AppVersion Convert(Entities.App.AppVersion entity)
        {
       
            return Mapper.Map<Entities.App.AppVersion, DTO.App.AppVersion>(entity);
        }
    }
}