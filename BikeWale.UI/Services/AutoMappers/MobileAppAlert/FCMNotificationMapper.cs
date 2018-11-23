
using AutoMapper;
using Bikewale.DTO.App.AppAlert;
using Bikewale.Entities.MobileAppAlert;
namespace Bikewale.Service.AutoMappers.MobileAppAlert
{
    public class FCMNotificationMapper
    {
        /// <summary>
        /// Created By : Sushil Kumar on 13th Dec 2016 
        /// To resolve app fcm input
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        internal static AppFCMInput Convert(AppIMEIDetailsInput input)
        {
           return Mapper.Map<AppIMEIDetailsInput, AppFCMInput>(input);
        }
    }
}