using AutoMapper;
using Bikewale.DAL.Location;
using Bikewale.DTO.State;
using Bikewale.Entities.Location;
using Bikewale.Interfaces.Location;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.State;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace Bikewale.Service.Controllers.State
{
    /// <summary>
    /// To Get List of States
    /// Author : Sushil Kumar
    /// Created On : 24th August 2015
    /// </summary>
    public class StateListController : ApiController
    {
        #region State's List
        /// <summary>
        ///  To get List of States currently Bikewale is Serving
        /// </summary>
        /// <returns>State's List</returns>
        [ResponseType(typeof(StateList))]
        public HttpResponseMessage Get()
        {
            List<StateEntityBase> objStateList = null;
            StateList objDTOStateList = null;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    IState statesRepository = null;

                    container.RegisterType<IState, StateRepository>();
                    statesRepository = container.Resolve<IState>();

                    objStateList = statesRepository.GetStates();

                    if (objStateList != null && objStateList.Count > 0)
                    {
                        objDTOStateList = new StateList();
                        objDTOStateList.State = StateEntityToDTO.ConvertStateEntityBase(objStateList);

                        return Request.CreateResponse(HttpStatusCode.OK, objDTOStateList);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.NoContent, "No Data Found");
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.State.StateListController");
                objErr.SendMail();
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "OOps ! Some error occured.");
            }
        }   // Get State List 
        #endregion

    }
}
