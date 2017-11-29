using Bikewale.DTO.State;
using Bikewale.Entities.Location;
using Bikewale.Interfaces.Location;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.State;
using System;
using System.Collections.Generic;
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

        private readonly IState _statesRepository = null;
        public StateListController(IState statesRepository)
        {
            _statesRepository = statesRepository;
        }


        #region State's List
        /// <summary>
        ///  To get List of States currently Bikewale is Serving
        /// </summary>
        /// <returns>State's List</returns>
        [ResponseType(typeof(StateList))]
        public IHttpActionResult Get()
        {
            List<StateEntityBase> objStateList = null;
            StateList objDTOStateList = null;
            try
            {
                objStateList = _statesRepository.GetStates();

                if (objStateList != null && objStateList.Count > 0)
                {
                    objDTOStateList = new StateList();
                    objDTOStateList.State = StateListMapper.Convert(objStateList);

                    objStateList.Clear();
                    objStateList = null;

                    return Ok(objDTOStateList);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.State.StateListController");
               
                return InternalServerError();
            }
            return NotFound();
        }   // Get State List 
        #endregion

    }
}
