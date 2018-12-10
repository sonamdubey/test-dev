IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[SearchKeyWordFromDB]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[SearchKeyWordFromDB]
GO

	-- This procedure is to enter forum searches

-- Created By : Ravi Koshal on 30-12-2013
-- Description : This SP is used to retrieve result set from DB for forums search if Lucene Index search fails.

CREATE  PROCEDURE [cw].[SearchKeyWordFromDB]
	@SearchTerm		VARCHAR(500),
	@StartIndex	    INT,
	@EndIndex		INT
 AS

BEGIN
SELECT FU.ID AS TopicId,
FU.Topic As Topic,
FU.StartDateTime AS StartDateTime,
FU.URL As URL,
FU.LastPostId,
FU.CustomerId As StartedById,
FU.Posts As Replies,
FU.Views As Reads,
FS.URL As ForumUrl,
UP.HandleName As HandleName, 
FT.MsgDateTime As LastPostTime, 
UF.UserId As LastPostedById , 
UF.HandleName As PostHandleName , 
FS.Name As ForumCategory
FROM Forums FU 
INNER JOIN ForumSubCategories FS ON FS.ID = FU.ForumSubCategoryId
INNER JOIN UserProfile UP ON UP.UserId = FU.CustomerId
INNER JOIN ForumThreads FT ON FT.ID = FU.LastPostId
INNER JOIN UserProfile UF ON UF.UserId = FT.CustomerId
WHERE Message LIKE '%' + @SearchTerm + '%' AND FU.IsActive=1 and FU.IsModerated=1
END

