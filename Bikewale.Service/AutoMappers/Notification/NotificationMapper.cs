
namespace Bikewale.Service.AutoMappers.Notification
{
    public class NotificationMapper
    {
       
        internal static Entities.Upcoming.UpcomingNotificationEntity Convert(DTO.Upcoming.UpcomingNotificationDTO objNotif)
        {
            AutoMapper.Mapper.CreateMap<DTO.Upcoming.UpcomingNotificationDTO, Entities.Upcoming.UpcomingNotificationEntity>();
            return AutoMapper.Mapper.Map<DTO.Upcoming.UpcomingNotificationDTO, Entities.Upcoming.UpcomingNotificationEntity>(objNotif); 
        }
    }
}