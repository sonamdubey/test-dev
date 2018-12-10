IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[Forums_DisplaySearchResults]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[Forums_DisplaySearchResults]
GO

	-- =============================================      
-- Author:  <Ravi Koshal>
-- Create date: <09/13/2013>      
-- Description: <Check for sticky threads.> 
-- =============================================      
CREATE procedure [cw].[Forums_DisplaySearchResults]      -- execute cw.Forums_FillSearchResults 770
 -- Add the parameters for the stored procedure here      
 @SearchId NUMERIC(18,0),
 @SearchType VARCHAR(200),
 @PageNumber INT
AS      
BEGIN      
 -- SET NOCOUNT ON added to prevent extra result sets from      
 -- interfering with SELECT statements.      
SET NOCOUNT ON;
-- Get the count of posts for the searchId
SELECT Count(ForumThreadId) as total FROM ForumSearchResults WHERE SearchId = @SearchId
DECLARE @LastDate DateTime
-- Get the posts for the searchId to fill the search page repeater.
IF @SearchType = 'PostsBy'
BEGIN
	IF @PageNumber = 1
	BEGIN
		SELECT Top 15  F.ID AS TopicId, IsNull(Views,0) AS Reads, F.Url AS Url, FC.Url AS ForumUrl,
		F.Topic, FC.Name ForumCategory, FC.Id ForumCategoryId, Message, FT.Id PostId, 
		IsNull(F.Posts, 0) AS Replies 
		FROM 
		(((ForumSearchResults SR LEFT JOIN ForumThreads FT ON SR.ForumThreadId=FT.Id AND FT.IsActive = 1 AND FT.IsModerated = 1) 
		LEFT JOIN Forums AS F ON F.ID = FT.ForumId AND F.IsActive = 1 ) 
		LEFT JOIN ForumSubCategories FC ON FC.ID = F.ForumSubCategoryId )
		WHERE SR.SearchId = @SearchId 
	END
	ELSE
	BEGIN
		DECLARE @NoOfResults INT
		SET @NoOfResults = (@PageNumber - 1)*15
		 SELECT @LastDate=LastDate FROM (SELECT TOP 1 * FROM 
		(SELECT TOP (@NoOfResults) MsgDateTime AS LastDate
		From ForumThreads FT, ForumSearchResults SR
		WHERE FT.Id=SR.ForumThreadId AND FT.IsActive=1 AND SR.SearchId = @SearchId
		ORDER BY LastDate Desc) As LastDates ORDER BY LastDate ASC) As LastMsgDate
	END
END
ELSE
	IF @PageNumber = 1
	BEGIN
	 SELECT Top 15 F.ID AS TopicId, IsNull(Views,0) AS Reads, F.Url AS Url, FC.Url AS ForumUrl,
F.Topic, IsNull(C.Name, 'anonymous') AS CustomerName, IsNull(CP.Name, 'anonymous') AS LastPostBy,
F.StartDateTime, FC.Name ForumCategory, FC.Id ForumCategoryId,
IsNull(F.Posts, 0) AS Replies,
FT.MsgDateTime AS LastPostTime, IsNull(C.Id, '0') StartedById, IsNull(CP.Id,'0') LastPostedById,
ISNULL((SELECT HandleName FROM UserProfile UP WHERE UP.UserId = C.id),'anonymous')  AS HandleName,
ISNULL((SELECT HandleName FROM UserProfile UP WHERE UP.UserId = CP.Id),'anonymous')  AS PostHandleName
FROM (((((ForumSearchResults SR LEFT JOIN Forums F ON SR.ForumThreadId=F.Id AND F.IsActive = 1 )
LEFT JOIN ForumThreads AS FT ON FT.ID = F.LastPostId AND FT.IsActive = 1)
LEFT JOIN Customers AS CP ON CP.ID = FT.CustomerId)
LEFT JOIN Customers C ON C.ID = F.CustomerId )
LEFT JOIN ForumSubCategories FC ON FC.ID = F.ForumSubCategoryId )
WHERE SR.SearchId = @SearchId
	END

	ELSE
	BEGIN
		SET @NoOfResults = (@PageNumber - 1)*15
	 Select LastDate From (SELECT TOP 1 * FROM 
(SELECT TOP (@NoOfResults)
CONVERT(CHAR(1),SR.IsTitleMatch) + CONVERT(VARCHAR,MSGDATETIME,112) + REPLACE(CONVERT(VARCHAR,MSGDATETIME,108),':','') AS LastDate
FROM ((ForumSearchResults SR LEFT JOIN Forums F ON SR.ForumThreadId=F.Id AND F.IsActive = 1)
LEFT JOIN ForumThreads AS FT
ON FT.ID = F.LastPostId AND FT.IsActive = 1) WHERE SR.SearchId = @SearchId
Order By LastDate Desc) AS lastdates ORDER BY LastDate ASC)
AS LastMsgDate
	END

 
END 
