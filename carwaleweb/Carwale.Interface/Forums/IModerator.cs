using System;
using System.Collections.Generic;
using System.Web;
using System.Data;

/// <summary>
/// Interface For Moderator Actions.
/// </summary>
/// 
namespace Carwale.Interfaces.Forums
{
    public interface IModerator
    {
        void LogModAction(string modId, string threadId, string actionType, string forumId);
        DataSet ShowRepotAbuseReport(); 
    }//interface
}//namesapce