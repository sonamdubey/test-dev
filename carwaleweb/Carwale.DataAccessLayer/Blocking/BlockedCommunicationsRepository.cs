using Carwale.Entity.Blocking;
using Carwale.Interfaces.Blocking;
using Dapper;
using System.Data;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Carwale.DAL.Blocking
{
    public class BlockedCommunicationsRepository: RepositoryBase, IBlockedCommunicationsRepository
    {
        public BlockedCommunicationRequest BlockCommunication(BlockedCommunicationRequest communicationRequest)
        {
            if (communicationRequest != null && communicationRequest.Communications != null)
            {
                communicationRequest.IsAllSuccess = true;
                using (var con = ClassifiedMySqlMasterConnection)
                {
                    foreach (var communication in communicationRequest.Communications)
                    {
                        var param = new DynamicParameters();
                        param.Add("v_value", communication.Value);
                        param.Add("v_type", communication.Type);
                        param.Add("v_module", communication.Module);
                        param.Add("v_reason", communication.Reason);
                        param.Add("v_actionby", communication.ActionBy);
                        param.Add("v_id", dbType: DbType.Int32, direction: ParameterDirection.Output);

                        con.Execute("cwmasterdb.blockcommunication", param, commandType: CommandType.StoredProcedure);
                        int id = param.Get<int>("v_id");
                        communication.IsBlocked = id != 0;
                        if(id == 0)
                        {
                            communicationRequest.IsAllSuccess = false;
                        }
                    }
                }
            }
            return communicationRequest;
        }

        public bool IsCommunicationBlocked(BlockedCommunication communication)
        {
            if (communication != null)
            {
                var param = new DynamicParameters();
                param.Add("v_value", communication.Value);
                param.Add("v_type", communication.Type);
                param.Add("v_module", communication.Module);
                param.Add("v_isBlocked", dbType: DbType.Int16, direction: ParameterDirection.Output);
                using (var con = ClassifiedMySqlReadConnection)
                {
                    con.Execute("cwmasterdb.iscommunicationblocked_v1", param, commandType: CommandType.StoredProcedure);
                    return param.Get<short>("v_isBlocked") == 1;
                } 
            }
            return false;
        }


        public BlockedCommunicationRequest UnblockCommunication(BlockedCommunicationRequest communicationRequest)
        {
            if (communicationRequest != null && communicationRequest.Communications != null)
            {
                communicationRequest.IsAllSuccess = true;
                using (var con = ClassifiedMySqlMasterConnection)
                {
                    foreach (var communication in communicationRequest.Communications)
                    {
                        var param = new DynamicParameters();
                        param.Add("v_value", communication.Value);
                        param.Add("v_type", communication.Type);
                        param.Add("v_module", communication.Module);
                        param.Add("v_actionby", communication.ActionBy);
                        param.Add("v_isDeleted", dbType: DbType.Int16, direction: ParameterDirection.Output);
                        con.Execute("cwmasterdb.unblockcommunication", param, commandType: CommandType.StoredProcedure);
                        int deletedStatus = param.Get<short>("v_isDeleted");
                        communication.IsBlocked = deletedStatus == 0;
                        if(deletedStatus != 1)
                        {
                            communicationRequest.IsAllSuccess = false;
                        }
                    }
                } 
            }
            return communicationRequest;
        }
    }
}
