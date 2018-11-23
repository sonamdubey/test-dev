using Bikewale.DTO.PriceQuote;
using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.Dealer;
using Bikewale.Entities.MaskingNumber;
using Bikewale.Interfaces.MaskingNumber;
using Bikewale.Notifications;
using MySql.CoreDAL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Web;

namespace Bikewale.DAL.MaskingNumber
{
    public class MaskingNumberDl : IMaskingNumberDl
    {
        /// <summary>
        /// Author  : Kartik Rathod on 19 nov 2019
        /// Desc    : save ds es and masking number
        /// </summary>
        /// <param name="objLead"></param>
        /// <returns>Lead Id</returns>
        public uint SaveMaskingNumberLead(MaskingNumberLeadEntity objLead)
        {
            uint leadId = 0;
            try
            {
                if (objLead.VersionId > 0 && objLead.DealerId > 0)
                {
                    using (DbCommand cmd = DbFactory.GetDBCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "savemaskingnumberlead";

                        cmd.Parameters.Add(DbFactory.GetDbParam("par_dealerid", DbType.UInt32, objLead.DealerId));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_customerid", DbType.UInt32, objLead.CustomerId));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_customername", DbType.String,50, objLead.CustomerName));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_customeremail", DbType.String,50,objLead.CustomerEmail));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_customermobile", DbType.String,15, objLead.CustomerMobile));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_leadsourceid", DbType.Int16, (short)LeadSourceEnum.MaskingNumber));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_campaignId", DbType.UInt32, objLead.CampaignId));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_spamscore", DbType.Double, objLead.SpamScore));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_isaccepted", DbType.Boolean,  objLead.IsAccepted));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_overallscore", DbType.Int16, objLead.OverallSpamScore));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbType.UInt16,  objLead.CityId));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_versionid", DbType.UInt32, objLead.VersionId));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_pqguid", DbType.String, 50,objLead.PqGuId ));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_sourceid", DbType.Int16, (short)PQSources.MaskingNumber));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_comments", DbType.String, 500, objLead.Comments));

                        cmd.Parameters.Add(DbFactory.GetDbParam("par_leadId", DbType.UInt32, ParameterDirection.Output));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_dealertypeId", DbType.UInt16, ParameterDirection.Output));
                        
                        MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.MasterDatabase);

                        leadId = Convert.ToUInt32(cmd.Parameters["par_leadId"].Value);
                        objLead.LeadTypeId = Convert.ToUInt16(cmd.Parameters["par_dealertypeId"].Value) == 7 ? LeadTypeEnum.ES : LeadTypeEnum.DS;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Exception : Bikewale.BAL.MaskingNumber.MaskingNumberDl.SaveMaskingNumberLead() obj - {0}", JsonConvert.SerializeObject(objLead)));
            }
            return leadId;
        }
    }
}