IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[WA_GetReviewComments]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[WA_GetReviewComments]
GO

	--Author: Rakesh Yadav  
--Date:28 Oct 2013  
--Desc:Fetch comments for reviews
--Modified By : Supriya on 1/7/2014 to add IsModerated filter 
--Approved by: Manish Chourasiya on 01-07-2014 5:50 pm , With (NoLock) is used, No missing index. 

CREATE PROCEDURE [dbo].[WA_GetReviewComments] @ReviewId INT
	,@StartIndex INT
	,@EndIndex INT
AS

BEGIN
	--Verify query before taking live there bug with this query showing 2 comments on reviews page (www.carwale.com/marutisuzuki-cars/800/userreviews) and 4 on reviewsDetails page (www.carwale.com/marutisuzuki-cars/800/userreviews/263.html )
	WITH CTE
	AS (
		SELECT ROW_NUMBER() OVER (
				ORDER BY FT.MsgDateTime
				) AS ROW_NO
			,FT.Message
			,ft.MsgDateTime
			,ISNULL(c.NAME, 'Anonymous') AS PostedBy
		FROM Forum_ArticleAssociation FA WITH (NOLOCK)
		INNER JOIN Forums F WITH (NOLOCK) ON F.ID = FA.ThreadId
		INNER JOIN ForumThreads FT WITH (NOLOCK) ON FT.ForumId = FA.ThreadId
		INNER JOIN Customers C WITH (NOLOCK) ON C.Id = FT.CustomerId
		WHERE ArticleType = 3
			AND ArticleId = @ReviewId
			AND FT.IsActive = 1
			AND FT.IsModerated = 1
		)
	SELECT *
	FROM CTE
	WHERE ROW_NO BETWEEN @StartIndex
			AND @EndIndex

	--Get Commens Count
	SELECT COUNT(FT.ID) AS CommentsCount
	FROM Forum_ArticleAssociation FA WITH (NOLOCK)
	INNER JOIN Forums F WITH (NOLOCK) ON F.ID = FA.ThreadId
	INNER JOIN ForumThreads FT WITH (NOLOCK) ON FT.ForumId = FA.ThreadId
	WHERE ArticleType = 3
		AND ArticleId = @ReviewId
		AND FT.IsActive = 1
		AND FT.IsModerated = 1
END

