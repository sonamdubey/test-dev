using System;
using System.Collections.Generic;
using System.Web;
using System.Data;

/// <summary>
/// Interface for Post related functions.
/// </summary>
/// 
namespace Carwale.Interfaces.Forums
{
    public interface IPost
    {
        bool DeletePost(string postId, string threadId, string customerId);
        bool EditPostByUser(int postId, string message, string customerId);
        int FindPost(int post,int threadId);
        string AppendMessagesForMerge(string ids);
        void MergePost(string strMessage, string strMergeId);
        void MergePost(string mergeId);
        string SavePost(string customerId, string messageText, int alertType, string threadId, int IsModerated, string remoteAddr, string clientIp);
        string GetLastPostThread(string name, string date, string lastPostById);
        string FillExistingPost(string postId);
        bool SaveToPostThanks(string _customerId, string _postId);
        void MovePost(string topic, int subCategoryId, int forumId);
        string SplitPostSaveData(int sId, int subCategory, string topic, int StrCustId, DateTime StrdateTime);
        DataSet ShowPreviousPosts(string threadId);
    }// Interface
}// namespace