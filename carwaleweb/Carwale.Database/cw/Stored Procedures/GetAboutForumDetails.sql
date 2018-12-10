IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[GetAboutForumDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[GetAboutForumDetails]
GO

	-- =============================================
-- Author:		<Reshma Shetty>
-- Create date: <17/08/2012>
-- Description:	<Returns all the info required in the About Carwale Forums space>
---modifed by Prashant Vishe on 22/8/2012
-- =============================================

-- =============================================
-- Modified By:		<Ravi Koshal>
-- Modification date: <2/19/2014>
-- Description:	<The conditions in the second last query were added to the join. Which was  not done previously.>
---
-- =============================================
-- Modified By:		amit verma
-- Modification date: <1/1/20124
-- Description:	Optimized forumthread customer count query
-- Modified by : Kundan Added WITH(NOLOCK) on 21/12/2015
-- Modified by : kundan on 23/12/2015 -- commented  Returns the number of discussions,contributors and posts query  
									  -- and written the same query with variable

 
CREATE  PROCEDURE [cw].[GetAboutForumDetails]   -- execute cw.GetAboutForumDetails 
	-- Add the parameters for the stored procedure here
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	 DECLARE @Discussions int ,
			 @Contributors int,
	         @Posts int  

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
	
	-- Get Top 5 Contributors
	SELECT TOP 5 Posts, CustomerId, ISNULL(U.HandleName,'anonymous') AS Name 
	FROM AP_ForumTopPosts AF WITH(NOLOCK)		
		INNER JOIN Customers Cs WITH(NOLOCK) ON Cs.Id = AF.CustomerId AND Cs.IsFake = 0
		INNER JOIN UserProfile U WITH(NOLOCK) ON U.UserId = AF.CustomerId
	WHERE PostType = 1  ORDER BY Posts DESC
	
	--SELECT TOP 5 Posts, CustomerId, ISNULL(U.HandleName,'anonymous') AS Name 
	--FROM AP_ForumTopPosts AF WITH(NOLOCK) 
	--	INNER JOIN UserProfile U WITH(NOLOCK) ON U.UserId = AF.CustomerId
	--WHERE PostType = 2 ORDER BY Posts DESC
	
	-- Get Top 5 Monthly Contributors
	SELECT TOP 5 Posts, CustomerId, ISNULL(U.HandleName,'anonymous') AS Name 
	FROM AP_ForumTopPosts AF WITH(NOLOCK)		
		INNER JOIN Customers Cs WITH(NOLOCK) ON Cs.Id = AF.CustomerId AND Cs.IsFake = 0
		INNER JOIN UserProfile U WITH(NOLOCK) ON U.UserId = AF.CustomerId
	WHERE PostType = 2  ORDER BY Posts DESC
	
	-- Returns the number of discussions,contributors and posts 
	---- Commented by kundan on 23/12/2015
	------------------------------------------------------------------------------------------------------
	/*SELECT COUNT(F.Id) AS Discussions,
		--modified by amit on 1/1/2014 start
		(
			SELECT COUNT(DISTINCT CustomerId) FROM ForumThreads WITH(NOLOCK)
			Where IsModerated = 1
		) AS Contributors,
		--(SELECT COUNT(Id) FROM Customers WITH(NOLOCK) WHERE Id IN (SELECT CustomerId FROM ForumThreads WITH(NOLOCK) Where IsModerated = 1 )  ) AS Contributors,
		--modified by amit on 1/1/2014 end
		( 
			SELECT COUNT(Id) FROM ForumThreads WITH(NOLOCK) 
			Where IsModerated = 1 
		) AS Posts
	FROM Forums F WITH(NOLOCK)
	*/
	------------------------------------------------------------------------------------------------------
	--  Modified by kundan on 23/12/2015 : Replaced Commented query part  /* ---*/ with below block 
	----------------------------------------------------------------------------------------------------------
			SET @Discussions= (
									SELECT COUNT(F.Id) AS Discussions FROM Forums F WITH(NOLOCK) 
							  )

			SET @Contributors =	(
									SELECT COUNT(DISTINCT CustomerId) FROM ForumThreads WITH(NOLOCK)
									Where IsModerated = 1
							    )

			       SET @Posts = (
									SELECT COUNT(Id) FROM ForumThreads WITH(NOLOCK) 
									Where IsModerated = 1 
				        		)
	  SELECT @Discussions as Discussions, @Contributors  As Contributors, @Posts As Posts
----------------------------------------------------------------------------------------------------------
	--Returns details of each forum category
	
	
	
SELECT FSC.ID, 
       FSC.ForumCategoryId,
       FC.Name AS ForumCategory, 
       FC.Description AS FC_Description, 
               FSC.Name AS ForumSubCategory, 
               FSC.Description AS FSC_Description,
                F.ID AS TopicId, 
                F.Topic, 
               IsNull(C.Name, 'anonymous') AS CustomerName,
                F.StartDateTime, IsNull(CT.Name, 'anonymous') LastPostBy, 
               IsNull(CT.Id, '0') LastPostById, 
               FT.MsgDateTime AS LastPostDate, 
               FSC.Threads AS Threads, 
               FSC.Posts AS Posts ,
               ISNULL(UP.HandleName,'anonymous')  AS HandleName,
               F.Url AS Url,
               
               FSC.Url AS SubUrl
       FROM ForumSubCategories AS FSC WITH(NOLOCK)
               LEFT JOIN ForumCategories AS FC WITH(NOLOCK) ON FC.id=FSC.ForumCategoryId  AND FC.IsActive=1
               LEFT JOIN ForumThreads AS FT WITH(NOLOCK) ON FT.ID = FSC.LastPostId AND FT.IsActive=1 AND FT.IsModerated = 1
               LEFT JOIN Forums AS F WITH(NOLOCK) ON F.ID = FT.ForumId AND F.IsActive = 1
               LEFT JOIN Customers AS C WITH(NOLOCK) ON C.ID = F.CustomerId 
               LEFT JOIN Customers AS CT WITH(NOLOCK) ON CT.ID = FT.CustomerId AND CT.IsFake = 0 
               LEFT JOIN  UserProfile UP WITH(NOLOCK) ON UP.UserId = CT.Id
       WHERE FSC.IsActive = 1 
       --AND FC.IsActive=1 AND F.IsActive = 1 AND CT.IsFake = 0 
       ---AND FT.IsActive=1 AND FT.IsModerated = 1  
       ORDER BY FC.Name,FSC.Name ,FT.MsgDateTime DESC
	
	SELECT ID, Name, Description 
	FROM ForumCategories WITH(NOLOCK) -- added WITH(NOLOCK) By kundan on 21/12/2015
	WHERE IsActive = 1 ORDER BY Name
	
END