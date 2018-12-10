IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[GetForumSubCategoryDetails_05082013]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[GetForumSubCategoryDetails_05082013]
GO

	---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
-- =============================================
-- Author:		<Reshma Shetty>
-- Create date: <23/08/2012>
-- Description:	<Returns the details required for a particular forum subcategory page
--               and the sticky threads at the top>
--               EXEC cw.GetForumSubCategoryDetails 8,1,5
-- =============================================

-- =============================================
-- MOdified By:		<Ravi Koshal>
-- Create date: <27/06/2013>
-- Description:	<IsModerated check included while retrival of data from forumthreads>
-- =============================================
CREATE  PROCEDURE [cw].[GetForumSubCategoryDetails_05082013] 
	-- Add the parameters for the stored procedure here
	@ForumId INT,
	@StartIndex INT,
	@EndIndex INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	
	SELECT Count(ID) as total  FROM Forums WHERE IsActive = 1 AND ForumSubCategoryId = @ForumId 

    -- Insert statements for procedure here
	SELECT FST.ID as ID,
		 FST.ThreadId as TopicId, FST.CatID, 
		 IsNull(Views,0) AS Reads, 
		 F.Topic, IsNull(C.Name, 'anonymous') AS CustomerName, F.StartDateTime, 
		 IsNull(F.Posts, 0) AS Replies, 
		 FT.MsgDateTime AS LastPostTime, IsNull(CP.Name, 'anonymous') AS LastPostBy, 
		 IsNull(C.Id, '0') StartedById, IsNull(CP.Id,'0') LastPostedById, 
		 ISNULL((SELECT HandleName FROM UserProfile UP WHERE UP.UserId = C.id),'anonymous')  AS HandleName,
		 ISNULL((SELECT HandleName FROM UserProfile UP WHERE UP.UserId = CP.Id),'anonymous')  AS PostHandleName, 
		 0 AS Row_No
	FROM Forum_StickyThreads FST 
		 INNER JOIN Forums AS F ON FST.ThreadId = F.ID
		 LEFT JOIN ForumThreads AS FT ON FT.ID = F.LastPostId 
		 LEFT JOIN Customers AS CP ON CP.ID = FT.CustomerId 
		 LEFT JOIN Customers C ON C.ID = F.CustomerId  
	WHERE (FST.CatID = 2 OR(FST.CatID = 1 AND F.ForumSubCategoryId = @ForumId ))AND F.IsActive = 1
		AND F.IsModerated = 1 AND FT.IsModerated = 1 
    UNION 
	SELECT * 
	FROM(SELECT 0 AS ID,F.ID AS TopicId,0 AS CatID, IsNull(Views,0) AS Reads, 
				 F.Topic, IsNull(C.Name, 'anonymous') AS CustomerName, F.StartDateTime, 
				 IsNull(F.Posts, 0) AS Replies, 
				 FT.MsgDateTime AS LastPostTime, IsNull(CP.Name, 'anonymous') AS LastPostBy, 
				 IsNull(C.Id, '0') StartedById, IsNull(CP.Id,'0') LastPostedById, 
				 ISNULL((SELECT HandleName FROM UserProfile UP WHERE UP.UserId = C.id),'anonymous')  AS HandleName,
				 ISNULL((SELECT HandleName FROM UserProfile UP WHERE UP.UserId = CP.Id),'anonymous')  AS PostHandleName,
				 ROW_NUMBER() OVER(ORDER BY FT.MsgDateTime DESC) AS Row_No
		 FROM Forums AS F LEFT JOIN ForumThreads AS FT ON FT.ID = F.LastPostId 
				 LEFT JOIN Customers AS CP ON CP.ID = FT.CustomerId 
				 LEFT JOIN Customers C ON C.ID = F.CustomerId 
		 WHERE F.ForumSubCategoryId = @ForumId AND F.IsActive = 1 
			AND F.IsModerated = 1 AND FT.IsModerated = 1
		 
		   ) AS A
	 WHERE Row_No between @StartIndex and @EndIndex
	 ORDER BY Row_No
	 
	 SELECT Name, Description FROM ForumSubCategories WHERE IsActive = 1 AND ID = @ForumId
END
