using AEPLCore.Cache;
using Carwale.DAL.ChatManagement;
using Carwale.Entity.ChatManagement;

namespace Carwale.Cache.ChatManagement
{
    public class ChatManagementCache
    {
        public static ChatResponse GetChatFlags(int pageId)
        {
            CacheManager cacheProvider = new CacheManager();
            ChatManagementRepository chatManagementRepo = new ChatManagementRepository();

            return cacheProvider.GetFromCache<ChatResponse>("chat-management-v1-pageid-" + pageId,
                            CacheRefreshTime.OneDayExpire(),
                                () => chatManagementRepo.GetChatManagementFlag(pageId));
        }
    }
}
