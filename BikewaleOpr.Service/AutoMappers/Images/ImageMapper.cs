using BikewaleOpr.DTO.AWS;
using BikewaleOpr.DTO.Images;

namespace BikewaleOpr.Service.AutoMappers.Images
{
    /// <summary>
    /// Created by  :   Sumit Kate on 15 Nov 2016
    /// Description :   Image Entity Automapper Class
    /// It contains methods to convert entity to DTO and visa-versa
    /// </summary>
    public class ImageMapper
    {
        internal static Entities.Images.Image Convert(ImageDTO objImage)
        {
            AutoMapper.Mapper.CreateMap<ImageDTO, Entities.Images.Image>();
            return AutoMapper.Mapper.Map<ImageDTO, Entities.Images.Image>(objImage);
        }

        internal static ImageTokenDTO Convert(Entities.Images.ImageToken token)
        {
            AutoMapper.Mapper.CreateMap<Entities.Images.ImageToken, ImageTokenDTO>();
            AutoMapper.Mapper.CreateMap<Entities.AWS.Token, Token>();
            return AutoMapper.Mapper.Map<Entities.Images.ImageToken, ImageTokenDTO>(token);
        }

        internal static Entities.Images.ImageToken Convert(ImageTokenDTO dto)
        {
            AutoMapper.Mapper.CreateMap<ImageTokenDTO, Entities.Images.ImageToken>();
            AutoMapper.Mapper.CreateMap<Token, Entities.AWS.Token>();
            return AutoMapper.Mapper.Map<ImageTokenDTO, Entities.Images.ImageToken>(dto);
        }
    }
}