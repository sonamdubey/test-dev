using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Carwale.Entity;

/// <summary>
/// Interface for Thread related details.
/// </summary>
/// 
namespace Carwale.Interfaces.Forums
{
    public interface IThread
    {
        string CreateNewThread(string customerId, string messageText, int alertType, string forumId, string title, int IsModerated, string remoteAddr, string clientIP);
        bool chkStickyThreads(string threadId, string customerId);
        bool CloseThread(string threadId, string customerId);
        bool DeleteThreadFromDB(string threadId, string customerId);
        bool UpdateStats( string ForumId, string SubCategoryId);
        ThreadBasicInfo GetAllForums(string threadId);
        ThreadBasicInfo GetForumForUserPostEdit(string threadId);
        DataSet GetThreadDetails(int threadId, int startIndex, int endIndex);
        bool GetModeratorLoginStatus(string customerId);
        string InsertStickyThreads(int id, int threadId, int strCat, int customerId);
        void DeleteStickyThreads(int threadId, int customerId);
        DataSet FillCategories();
    } // interface
}// namespace