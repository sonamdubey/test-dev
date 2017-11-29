using AutoMapper;

namespace Bikewale.Service.AutoMappers.AppDeepLinking
{
    /// <summary>
    /// Created By : Lucky Rathore 
    /// Created on : 10 March 2016
    /// Description : Mapper to map Deeplinking entity to DTO
    /// </summary>
    public class DeepLinkingMapper
    {
        /// <summary>
        /// Created By : Lucky Rathore
        /// Created On : 10 March 2016
        /// Description : Mapper to map Deeplinking entity to DTO
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        internal static DTO.AppDeepLinking.DeepLinking Convert(Entities.AppDeepLinking.DeepLinkingEntity entity)
        {
            Mapper.CreateMap<Entities.AppDeepLinking.DeepLinkingEntity, DTO.AppDeepLinking.DeepLinking>();
            return Mapper.Map<Entities.AppDeepLinking.DeepLinkingEntity, DTO.AppDeepLinking.DeepLinking>(entity);
        }
    }
}