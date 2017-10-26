using Bikewale.Entities.Finance.CapitalFirst;
using Bikewale.Utility;
using Consumer;
using MySql.CoreDAL;
using System;
using System.Data;
using System.Data.Common;

namespace Bikewale.RabbitMq.CapitalFirstLeadConsumer
{
    internal class LeadConsumerRepository : IDisposable
    {
        public CapitalFirstLeadEntity GetLeadDetails(string ctLeadId)
        {
            CapitalFirstLeadEntity lead = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandText = "getcapitalfirstleaddetails";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_ctleadid", DbType.Int32, ctLeadId));
                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.MasterDatabase))
                    {
                        lead = new CapitalFirstLeadEntity();
                        if (dr != null)
                        {
                            while (dr.Read())
                            {
                                lead.AgentName = Convert.ToString(dr["AgentName"]);
                                lead.AgentNumber = Convert.ToString(dr["AgentNumber"]);
                                lead.BikeName = Convert.ToString(dr["BikeName"]);
                                lead.CtLeadId = ctLeadId;
                                lead.EmailId = Convert.ToString(dr["EmailId"]);
                                lead.Exshowroom = SqlReaderConvertor.ToUInt32(dr["Exshowroom"]);
                                lead.Rto = SqlReaderConvertor.ToUInt32(dr["RTO"]);
                                lead.Insurance = SqlReaderConvertor.ToUInt32(dr["Insurance"]);
                                lead.FirstName = Convert.ToString(dr["FirstName"]);
                                lead.LastName = Convert.ToString(dr["LastName"]);
                                lead.MobileNo = Convert.ToString(dr["MobileNo"]);
                                lead.EmailId = Convert.ToString(dr["EmailId"]);
                                lead.VoucherNumber = Convert.ToString(dr["voucherNumber"]);
                                lead.VoucherExpiryDate = SqlReaderConvertor.ToDateTime(dr["VoucherExpiryDate"]);

                            }
                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog(String.Format("Error in GetLeadDetails({0}) : Msg : {1}", ctLeadId, ex.Message));
            }
            return lead;
        }

        public bool UpdateCustomerNotified(string ctLeadId)
        {
            bool isSuccess = false;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandText = "updatecapitalfirstvouchernotified";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_ctleadid", DbType.Int32, ctLeadId));
                    MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.MasterDatabase);
                    isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog(String.Format("Error in UpdateCustomerNotified({0}) : Msg : {1}", ctLeadId, ex.Message));
            }
            return isSuccess;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
        #endregion
    }
}
