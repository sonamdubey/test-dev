using Bikewale.DAL.CoreDAL;
using Bikewale.Notifications;
using BikewaleOpr.Entity;
using BikewaleOpr.Interface.AdSlot;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;

namespace BikewaleOpr.DALs.AdSlot
{
    /// <summary>
    /// Created by : Ashutosh Sharma on 30 Oct 2017
    /// Description : Provide methods to get data for ad slots.
    /// </summary>
    public class AdSlot : IAdSlotRepository
    {
        /// <summary>
        /// Created by : Ashutosh Sharma on 30 Oct 2017
        /// Description : DAL method to get ad slots details.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<AdSlotEntity> GetAdSlots()
        {
            IEnumerable<AdSlotEntity> objAdSlotList = null;
            try
            {
                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {
                    objAdSlotList = connection.Query<AdSlotEntity>("getadslots", commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, "BikewaleOpr.DAL.AdSlot.GetAdSlots");
            }
            return objAdSlotList;
        }

        /// <summary>
        /// Created by : Ashutosh Sharma on 30 Oct 2017
        /// Description : DAL method to change status of 
        /// </summary>
        /// <param name="AdId"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public bool ChangeStatus(uint AdId, int UserId)
        {
            int rowsAffected = 0;
            try
            {
                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {
                    var param = new DynamicParameters();
                    param.Add("par_adid", AdId);
                    param.Add("par_userid", UserId);
                    rowsAffected = connection.Execute("changeadslotstatus",param: param, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, string.Format("BikewaleOpr.DAL.AdSlot.ChangeStatus_{0}_{1}", AdId, UserId));
            }
            return rowsAffected > 0;
        }
    }
}
