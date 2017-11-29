using Bikewale.DAL.CoreDAL;
using Bikewale.Entities.Models;
using Bikewale.Interfaces.AdSlot;
using Bikewale.Notifications;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;

namespace Bikewale.DAL.AdSlot
{
    /// <summary>
    /// Created by : Ashutosh Sharma on 31 Oct 2017
    /// Description : Provide DAL methods for ad slots.
    /// </summary>
    public class AdSlot : IAdSlotRepository
    {
        /// <summary>
        /// Created by : Ashutosh Sharma on 31 Oct 2017
        /// Description : DAL method to get all ad slot status.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<AdSlotEntity> GetAdSlotStatus()
        {
            IEnumerable<AdSlotEntity> objAdSlotList = null;
            try
            {
                using (IDbConnection con = DatabaseHelper.GetReadonlyConnection())
                {
                    objAdSlotList = con.Query<AdSlotEntity>("getadslots", commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, "DAL.AdSlot.GetAdStatus");
            }
            return objAdSlotList;
        }
    }
}
