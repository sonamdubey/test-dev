using System;
using System.Collections.Generic;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for IForumThreadDetails
/// </summary>
/// 
namespace Carwale.Interfaces.Forums
{
    public interface IForum
    {
        DataSet GetForumDetails(int forumId, int startIndex, int endIndex);
        DataSet GetAllForums();
        List<string> GetActiveMember();
        DataSet GetForumReviewCount(int ReviewId,int startIndex,int endIndex);

    }
}