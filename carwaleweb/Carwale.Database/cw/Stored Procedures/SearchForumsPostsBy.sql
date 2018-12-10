IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[SearchForumsPostsBy]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[SearchForumsPostsBy]
GO

	-- =============================================      
-- Author:  <Ravi Koshal>
-- Create date: <09/13/2013>      
-- Description: <Check for sticky threads.> 
-- =============================================      
CREATE procedure [cw].[SearchForumsPostsBy]      -- execute cw.Forums_GetAllThreadDetails 770
 -- Add the parameters for the stored procedure here      
@CustomerId NUMERIC(18,0),
@StartIndex INTEGER,
@EndIndex INTEGER
AS      
BEGIN      
 -- SET NOCOUNT ON added to prevent extra result sets from      
 -- interfering with SELECT statements.      
 SET NOCOUNT ON;

 SELECT * 
  INTO #TempForumsPostsBy

 FROM (
    SELECT F.ID AS TopicId,
    IsNull(Views,0) AS Reads,
    F.Url AS Url, FC.Url AS ForumUrl,
	F.Topic,
	F.StartDateTime,
	FC.Name ForumCategory, 
	FC.Id ForumCategoryId, 
	Message, 
	FT.Id AS PostId,  
    IsNull(F.Posts, 0) AS Replies,
	ROW_NUMBER() OVER(ORDER BY FT.MsgDateTime desc) AS RowNumber
    FROM ForumThreads FT WITH(NOLOCK)	
    LEFT JOIN Forums AS F WITH(NOLOCK) ON F.ID = FT.ForumId
	LEFT JOIN ForumSubCategories FC WITH(NOLOCK) ON FC.ID = F.ForumSubCategoryId
    WHERE FT.customerId=@CustomerId AND FT.IsActive=1 AND F.IsActive=1 AND FT.IsModerated = 1) AS T 
	
	select count(*) as Total from #TempForumsPostsBy

select * from #TempForumsPostsBy
where RowNumber BETWEEN @StartIndex AND @EndIndex

drop table #TempForumsPostsBy
END 
