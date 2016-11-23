
namespace Bikewale.Service.AutoMappers.Images
{
    /// <summary>
    /// Created by  :   Sumit Kate on 15 Nov 2016
    /// Description :   Image Entity Automapper Class
    /// It contains methods to convert entity to DTO and visa-versa
    /// </summary>
    public class ImageMapper
    {
        internal static Entities.Images.Image Convert(DTO.Images.ImageDTO objImage)
        {
            AutoMapper.Mapper.CreateMap<DTO.Images.ImageDTO, Entities.Images.Image>();
            return AutoMapper.Mapper.Map<DTO.Images.ImageDTO, Entities.Images.Image>(objImage);
        }

        internal static DTO.Images.ImageTokenDTO Convert(Entities.Images.ImageToken token)
        {
            AutoMapper.Mapper.CreateMap<Entities.Images.ImageToken, DTO.Images.ImageTokenDTO>();
            AutoMapper.Mapper.CreateMap<Entities.AWS.Token, DTO.AWS.Token>();
            return AutoMapper.Mapper.Map<Entities.Images.ImageToken, DTO.Images.ImageTokenDTO>(token);
        }

        internal static Entities.Images.ImageToken Convert(DTO.Images.ImageTokenDTO dto)
        {
            AutoMapper.Mapper.CreateMap<DTO.Images.ImageTokenDTO, Entities.Images.ImageToken>();
            AutoMapper.Mapper.CreateMap<DTO.AWS.Token, Entities.AWS.Token>();
            return AutoMapper.Mapper.Map<DTO.Images.ImageTokenDTO, Entities.Images.ImageToken>(dto);
        }
    }
}