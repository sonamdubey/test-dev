using Carwale.Entity.ChatManagement;
using Carwale.Notifications;
using Carwale.Notifications.Logs;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Carwale.DAL.ChatManagement
{
    public class ChatManagementRepository : RepositoryBase
    {
        public ChatResponse GetChatManagementFlag(int pageId)
        {
            var chatResponse = new ChatResponse();
            try
            {
                var param = new DynamicParameters();
                param.Add("v_pageId", pageId);

                using (var con = NewCarMySqlReadConnection)
                {
                    chatResponse = con.Query<ChatResponse>("GetChatFlags", param, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    LogLiveSps.LogSpInGrayLog("GetChatSFlags");
                    return chatResponse;
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "ChatManagementRepository.GetChatManagementFlag");
                objErr.LogException();
                return null;
            }
        }
    }
}
