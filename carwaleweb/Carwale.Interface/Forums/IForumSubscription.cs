using System;
using System.Collections.Generic;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for IForumSubscription
/// </summary>
/// 
namespace Carwale.Interfaces.Forums
{
    public interface IForumSubscription
    {
        void ManageSubscriptions(string actionType, int emailSubscriptionId, int customerId, int forumThreadId);
        DataSet ShowSubscriptions(string customerId);
        bool AddSubscription(string custID, string subId);
    }//interface
}//namespace