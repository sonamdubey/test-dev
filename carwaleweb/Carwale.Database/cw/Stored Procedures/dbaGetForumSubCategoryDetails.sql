IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[dbaGetForumSubCategoryDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[dbaGetForumSubCategoryDetails]
GO

	---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
-- =============================================
-- Author:		<Reshma Shetty>
-- Create date: <23/08/2012>
-- Description:	<Returns the details required for a particular forum subcategory page
--               and the sticky threads at the top>
--               EXEC cw.GetForumSubCategoryDetails 1,1,5
-- =============================================

-- =============================================
-- MOdified By:		<Ravi Koshal>
-- Create date: <27/06/2013>
-- Description:	Url Field value retireved
-- =============================================
-- =============================================
-- MOdified By:		<Reshma Shetty>
-- Create date: <27/01/2014>
-- Description:	Added WITH(NOLOCK) in the queries
-- =============================================
-- MOdified By:		Avishkar  27/06/2014 
-- =============================================
CREATE  PROCEDURE [cw].[dbaGetForumSubCategoryDetails] 
	-- Add the parameters for the stored procedure here
	@ForumId INT,
	@StartIndex INT,
	@EndIndex INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	
	SELECT Count(ID) as total  FROM Forums WITH(NOLOCK) WHERE IsActive = 1 AND ForumSubCategoryId = @ForumId 

    -- Insert statements for procedure here
	SELECT FST.ID as ID,
		 FST.ThreadId as TopicId, FST.CatID, 
		 IsNull(Views,0) AS Reads, 
		 F.Topic, IsNull(C.Name, 'anonymous') AS CustomerName, F.StartDateTime, 
		 IsNull(F.Posts, 0) AS Replies,
		 F.Url As Url, 
		 FT.MsgDateTime AS LastPostTime, IsNull(CP.Name, 'anonymous') AS LastPostBy, 
		 IsNull(C.Id, '0') StartedById, IsNull(CP.Id,'0') LastPostedById, 
		 (SELECT ISNULL(HandleName,'anonymous') HandleName FROM UserProfile UP WHERE UP.UserId = C.id) HandleName ,
		 (SELECT ISNULL(HandleName,'anonymous') PostHandleName FROM UserProfile UP WHERE UP.UserId = CP.id) PostHandleName  ,
		 0 AS Row_No
	FROM Forum_StickyThreads FST WITH(NOLOCK)
		 INNER JOIN Forums AS F WITH(NOLOCK) ON FST.ThreadId = F.ID
		 LEFT JOIN ForumThreads AS FT WITH(NOLOCK) ON FT.ID = F.LastPostId 
		 LEFT JOIN Customers AS CP WITH(NOLOCK) ON CP.ID = FT.CustomerId 
		 LEFT JOIN Customers C WITH(NOLOCK) ON C.ID = F.CustomerId  
	WHERE (FST.CatID = 2 OR(FST.CatID = 1 AND F.ForumSubCategoryId = @ForumId ))AND F.IsActive = 1
		AND F.IsModerated = 1 AND FT.IsModerated = 1 AND FT.IsActive = 1
    UNION 
	SELECT * 
	FROM(SELECT 0 AS ID,F.ID AS TopicId,0 AS CatID, IsNull(Views,0) AS Reads, 
				 F.Topic, IsNull(C.Name, 'anonymous') AS CustomerName, F.StartDateTime, 
				 IsNull(F.Posts, 0) AS Replies, F.Url AS Url,
				 FT.MsgDateTime AS LastPostTime, IsNull(CP.Name, 'anonymous') AS LastPostBy, 
				 IsNull(C.Id, '0') StartedById, IsNull(CP.Id,'0') LastPostedById, 
				 (SELECT ISNULL(HandleName,'anonymous') HandleName FROM UserProfile UP WHERE UP.UserId = C.id) HandleName  ,
				 (SELECT ISNULL(HandleName,'anonymous') PostHandleName FROM UserProfile UP WHERE UP.UserId = CP.id) PostHandleName ,
				 ROW_NUMBER() OVER(ORDER BY FT.MsgDateTime DESC) AS Row_No
		 FROM Forums AS F WITH(NOLOCK) 
				LEFT JOIN ForumThreads AS FT WITH(NOLOCK) ON FT.ID = F.LastPostId 
				 LEFT JOIN Customers AS CP WITH(NOLOCK) ON CP.ID = FT.CustomerId 
				 LEFT JOIN Customers C WITH(NOLOCK) ON C.ID = F.CustomerId 
		 WHERE F.ForumSubCategoryId = @ForumId AND F.IsActive = 1 
			AND F.IsModerated = 1 AND FT.IsModerated = 1 AND FT.IsActive = 1
		 
		   ) AS A
	 WHERE Row_No between @StartIndex and @EndIndex
	 ORDER BY Row_No
	 
	 SELECT Name, Description,Url FROM ForumSubCategories WITH(NOLOCK) WHERE IsActive = 1 AND ID = @ForumId
END

