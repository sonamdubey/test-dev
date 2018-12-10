IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetUserReviews_16_10_1]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetUserReviews_16_10_1]
GO

	-- =============================================
-- Author:		<jitendra singh>
-- Create date: <30 sep 2016>
-- Description:	<Fetch user reviews for models based on condition>
-- 14 oct 2016 : by jitendra change good/bad to goods/bads while fetching
-- =============================================
CREATE PROCEDURE [dbo].[GetUserReviews_16_10_1]
	@ModelId INT,
	@StartIndex INT,
	@EndIndex INT,
	@SortCriteria INT,
	@VersionId INT=NULL
AS

BEGIN

	SELECT 
	ReviewId,
	CustomerId,
	HandleName,
	CustomerName,
	EntryDateTime as ReviewDate,
	Pros as Goods,
	Cons as Bads,
	SubComments as Description,
	OverallR as ReviewRate,
	Comments,
	ThreadId,
	Title
	 FROM
	(
		SELECT 
		ROW_NUMBER() OVER
		(
		ORDER BY
		CASE WHEN @SortCriteria=1 THEN Liked END DESC,
		CASE WHEN @SortCriteria=2 THEN Viewed END DESC,
		CASE WHEN @SortCriteria=3 THEN EntryDateTime END DESC,
		CASE WHEN @SortCriteria=4 THEN OverallR END DESC
		) AS Row
		, CR.ID AS ReviewId, CU.Name AS CustomerName 
		, CU.ID AS CustomerId
		, ISNULL(UP.HandleName, '') As HandleName
		, CR.StyleR, CR.ComfortR, CR.PerformanceR
		, CR.ValueR, CR.FuelEconomyR, CR.OverallR, CR.Pros,CR.Cons
		, Substring(CR.Comments,0,Cast(Floor(LEN(CR.Comments)*0.15) AS INT)) AS SubComments
		, CR.Title, CR.EntryDateTime, CR.Liked, CR.Disliked
		, CR.Viewed, ISNULL(Fm.Posts, 0) Comments, Fso.ThreadId
		FROM
		CustomerReviews AS CR WITH(NOLOCK)
		LEFT JOIN Customers AS CU WITH(NOLOCK)ON CU.Id = CR.CustomerId
		LEFT JOIN UserProfile UP WITH(NOLOCK) ON UP.UserId = CU.ID		
		LEFT JOIN Forum_ArticleAssociation Fso WITH(NOLOCK) ON CR.ID = Fso.ArticleId
		LEFT JOIN Forums Fm WITH(NOLOCK) ON Fso.ThreadId = Fm.ID
		WHERE
		CU.ID = CR.CustomerId AND CR.IsActive=1 
		AND CR.IsVerified=1 AND CR.ModelId = @ModelId 
		AND (@VersionId IS NULL OR CR.VersionId = @VersionId)
	) AS Result WHERE Result.Row BETWEEN @StartIndex AND @EndIndex

END