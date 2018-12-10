using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using Carwale.Entity.Customers;
using Carwale.Entity.Forums;
using Carwale.Entity.Geolocation;

/// <summary>
/// Interface for checl on user credentials.
/// </summary>
/// 
namespace Carwale.Interfaces.Forums
{
    public interface IUser
    {
        bool IsUserBanned(string customerId);
        bool CheckUserHandle(string userId);
        bool CheckCustomerIds(string ids);
        bool GetLoginStatus(string userId, string postId);
        DataTable LoadHandles(string postId);
        void SaveActivity(string userId, string activityId, string categoryId, string threadId, string sessionId);
        UserProfile GetProfileDetails(int Id);
        UserProfile GetExistingHandleDetails(int UserId);
        bool InsertHandle(int UserId, string HandleName, bool IsUpdated);
        bool InsertImages(string userId, UserProfile param);        
    }//interface
}//namespace