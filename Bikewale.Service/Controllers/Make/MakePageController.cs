using Bikewale.DTO.Make;
using Bikewale.DTO.Upcoming;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.UpcomingNotification;
using Bikewale.Interfaces.BikeData;
using Bikewale.Notifications;
using Bikewale.Notifications.MailTemplates;
using Bikewale.Service.AutoMappers.Make;
using Bikewale.Service.AutoMappers.UpcomingNotification;
using Bikewale.Service.Utilities;
using Bikewale.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;

namespace Bikewale.Service.Controllers.Make
{
    /// <summary>
    /// Make Page Controller
    /// Author  :   Sumit Kate
    /// Created :   03 Sept 2015
    /// Modified by :   Sumit Kate on 18 May 2016
    /// Description :   Extend from CompressionApiController instead of ApiController 
    /// Modified by: Dhruv Joshi on 6 Feb 2018 
    /// Description:  UpcomingNotification (post api) method
    /// </summary>
    public class MakePageController : CompressionApiController//ApiController
    {
        private readonly IBikeMakes<BikeMakeEntity, int> _makesRepository;
        private readonly IBikeModelsRepository<BikeModelEntity, int> _modelRepository = null;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="makesRepository"></param>
        /// <param name="modelRepository"></param>
        public MakePageController(IBikeMakes<BikeMakeEntity, int> makesRepository, IBikeModelsRepository<BikeModelEntity, int> modelRepository)
        {
            _makesRepository = makesRepository;
            _modelRepository = modelRepository;
        }

        /// <summary>
        /// Returns the Make page data which includes
        /// Make name, Description(small and large) and popular bikes
        /// </summary>
        /// <param name="makeId">make id</param>
        /// <param name="totalBikeCount">total popular bike count</param>
        /// <returns></returns>
        [ResponseType(typeof(MakePage))]
        public IHttpActionResult Get(int makeId)
        {
            BikeMakePageEntity entity = null;
            BikeDescriptionEntity description = null;
            IEnumerable<MostPopularBikesBase> objModelList = null;
            MakePage makePage = null;

            try
            {
                objModelList = _modelRepository.GetMostPopularBikesByMake(makeId);
                description = _makesRepository.GetMakeDescription(makeId);

                if (objModelList != null && objModelList.Any() && description != null)
                {
                    entity = new BikeMakePageEntity();
                    entity.Description = description;
                    entity.PopularBikes = objModelList;
                    makePage = MakePageEntityMapper.Convert(entity);

                    objModelList = null;

                    return Ok(makePage);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.Controllers.Make.MakePageController");
                return InternalServerError();
            }
            return NotFound();
        }
        [Route("api/notifyuser/")]
        public IHttpActionResult UpcomingNotification([FromBody]UpcomingNotificationDTO dtoNotif)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    dtoNotif.NotificationTypeId = (int)EnumNotifTypeId.Upcoming;
                    UpcomingNotificationEntity entitiyNotif = NotificationMapper.Convert(dtoNotif);
                    if (entitiyNotif != null)
                    {
                        _makesRepository.ProcessNotification(entitiyNotif);

                        ComposeEmailBase objNotify = new UpcomingBikesSubscription(entitiyNotif.BikeName);
                        objNotify.Send(entitiyNotif.EmailId, string.Format("You have subscribed to notifications for upcoming bike {0}", entitiyNotif.BikeName));

                        return Ok();
                    }
                    else
                    {
                        return InternalServerError();
                    }
                    
                }
                else
                {
                    return BadRequest();
                }
            }
            catch(Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.Controllers.Make.MakePageController");
                return InternalServerError();
            }
        }
    }
}
