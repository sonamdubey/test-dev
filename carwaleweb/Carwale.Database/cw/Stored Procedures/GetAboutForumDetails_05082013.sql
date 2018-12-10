IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[GetAboutForumDetails_05082013]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[GetAboutForumDetails_05082013]
GO

	-- =============================================
-- Author:		<Reshma Shetty>
-- Create date: <17/08/2012>
-- Description:	<Returns all the info required in the About Carwale Forums space>
---modifed by Prashant Vishe on 22/8/2012
-- =============================================

-- =============================================
-- Modified By:		<Ravi Koshal>
-- Modification date: <6/27/2013>
-- Description:	<IsModerated check included>
---
-- =============================================
CREATE  PROCEDURE [cw].[GetAboutForumDetails_05082013]
	-- Add the parameters for the stored procedure here
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Returns the Handle Names of the active members
	SELECT DISTINCT UP.HandleName 
	FROM ForumUserTracking FUT WITH(NOLOCK)
		INNER JOIN  UserProfile UP WITH(NOLOCK) ON FUT.UserID = UP.UserId
	WHERE DATEDIFF(MINUTE, FUT.ActivityDateTime, getdate()) < 60 AND FUT.UserId <> -1
	
	
	-- Returns top forum contributors @PostType=1 or if NO Parameter is passed 
    -- Returns top forum contributors for current month top @PostType=2 
	--SELECT TOP 5 Posts, CustomerId, ISNULL(U.HandleName,'anonymous') AS Name 
	--FROM AP_ForumTopPosts AF WITH(NOLOCK) 
	--	INNER JOIN UserProfile U WITH(NOLOCK) ON U.UserId = AF.CustomerId
	--WHERE PostType = 1 ORDER BY Posts DESC
	
	SELECT TOP 5 Posts, CustomerId, ISNULL(U.HandleName,'anonymous') AS Name 
	FROM AP_ForumTopPosts AF WITH(NOLOCK)		
		INNER JOIN Customers Cs WITH(NOLOCK) ON Cs.Id = AF.CustomerId
		INNER JOIN UserProfile U WITH(NOLOCK) ON U.UserId = AF.CustomerId
	WHERE PostType = 1 AND Cs.IsFake = 0 ORDER BY Posts DESC
	
	--SELECT TOP 5 Posts, CustomerId, ISNULL(U.HandleName,'anonymous') AS Name 
	--FROM AP_ForumTopPosts AF WITH(NOLOCK) 
	--	INNER JOIN UserProfile U WITH(NOLOCK) ON U.UserId = AF.CustomerId
	--WHERE PostType = 2 ORDER BY Posts DESC
	
	SELECT TOP 5 Posts, CustomerId, ISNULL(U.HandleName,'anonymous') AS Name 
	FROM AP_ForumTopPosts AF WITH(NOLOCK)		
		INNER JOIN Customers Cs WITH(NOLOCK) ON Cs.Id = AF.CustomerId
		INNER JOIN UserProfile U WITH(NOLOCK) ON U.UserId = AF.CustomerId
	WHERE PostType = 2 AND Cs.IsFake = 0 ORDER BY Posts DESC
	
	-- Returns the number of discussions,contributors and posts
	SELECT COUNT(F.Id) AS Discussions, 
		(SELECT COUNT(Id) FROM Customers WITH(NOLOCK) WHERE Id IN (SELECT CustomerId FROM ForumThreads WITH(NOLOCK) Where IsModerated = 1 )  ) AS Contributors,
		(SELECT COUNT(Id) FROM ForumThreads WITH(NOLOCK) Where IsModerated = 1 ) AS Posts
	FROM Forums F WITH(NOLOCK)
	
	
	--Returns details of each forum category
	
	
	
	SELECT FSC.ID, FSC.ForumCategoryId,FC.Name AS ForumCategory, FC.Description AS FC_Description, 
		FSC.Name AS ForumSubCategory, FSC.Description AS FSC_Description, F.ID AS TopicId, F.Topic, 
		IsNull(C.Name, 'anonymous') AS CustomerName, F.StartDateTime, IsNull(CT.Name, 'anonymous') LastPostBy, 
		IsNull(CT.Id, '0') LastPostById, FT.MsgDateTime AS LastPostDate, FSC.Threads AS Threads, 
		FSC.Posts AS Posts ,ISNULL(UP.HandleName,'anonymous')  AS HandleName
	FROM ForumSubCategories AS FSC WITH(NOLOCK)
		LEFT JOIN ForumCategories AS FC WITH(NOLOCK) ON FC.id=FSC.ForumCategoryId  
		LEFT JOIN ForumThreads AS FT WITH(NOLOCK) ON FT.ID = FSC.LastPostId 
		LEFT JOIN Forums AS F WITH(NOLOCK) ON F.ID = FT.ForumId 
		LEFT JOIN Customers AS C WITH(NOLOCK) ON C.ID = F.CustomerId 
		LEFT JOIN Customers AS CT WITH(NOLOCK) ON CT.ID = FT.CustomerId 
		LEFT JOIN  UserProfile UP WITH(NOLOCK) ON UP.UserId = CT.Id
	WHERE FSC.IsActive = 1 AND FC.IsActive=1 AND F.IsActive = 1 AND CT.IsFake = 0 
	AND FT.IsActive=1 AND FT.IsModerated = 1  -- Avishkar 24/5/2013 Added 
	ORDER BY FC.Name,FSC.Name ,FT.MsgDateTime DESC
	
	SELECT ID, Name, Description FROM ForumCategories WHERE IsActive = 1 ORDER BY Name
	
END
