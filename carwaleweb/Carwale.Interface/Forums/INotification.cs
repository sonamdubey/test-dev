using System;
using System.Collections.Generic;
using System.Web;
using System.Data;

/// <summary>
/// interface for notification mails sent to the user.
/// </summary>
/// 
namespace Carwale.Interfaces.Forums
{
    public interface INotification
    {
        DataSet GetSubscribers(string discussionUrl, string threadId, string threadName, string handleName, string customerId, string eMail);

    }//interface

}//namespace