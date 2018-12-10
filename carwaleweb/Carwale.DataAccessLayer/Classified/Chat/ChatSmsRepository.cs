using Carwale.Entity.Classified.Chat;
using Carwale.Interfaces.Classified.Chat;
using Dapper;
using System.Configuration;
using System.Data;
using System.Linq;

namespace Carwale.DAL.Classified.Chat
{
    public class ChatSmsRepository : RepositoryBase, IChatSmsRepository
    {
        private readonly string _usedChatSmsFallBackTime = ConfigurationManager.AppSettings["UsedChatSmsFallTimeInMinutes"];

        public bool shouldMessageBeSent(string from, string to)
        {
            string subQuery = "select 1 from chatfallbacksmsentries where Sender = @v_from and Receiver = @v_to and entrydatetime > date_add(now(), interval - @v_fallbackTime minute) order by entryDateTime desc limit 1";
            string query = $"select not exists({ subQuery });";
            using (var con = ClassifiedMySqlReadConnection)
            {
                return con.Query<bool>(query, new {
                                            v_fallbackTime = _usedChatSmsFallBackTime,          //this must be same as applozic sms fallback time
                                            v_from = from,
                                            v_to = to
                                        }, 
                                        commandType: CommandType.Text).FirstOrDefault();
            }
        }

        public void InsertChatSmsDetails(ChatSmsPayload chatSmsPayload, bool isBuyerToSellerChat)
        {
            using (var con = ClassifiedMySqlMasterConnection)
            {
                con.Execute("insert into chatfallbacksmsentries (sender, receiver, typeid, messagesenttime) values (@v_from, @v_to, @v_typeid, @v_messagesenttime); ",
                                new
                                {
                                    v_from = chatSmsPayload.From,
                                    v_to = chatSmsPayload.To,
                                    v_typeid = isBuyerToSellerChat,
                                    v_messagesenttime = chatSmsPayload.MessageSentTime
                                },
                        commandType: CommandType.Text);
            }
        }
    }
}
