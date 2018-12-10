IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[SearchForumsByDate]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[SearchForumsByDate]
GO

	-- =============================================      
-- Author:  <Ravi Koshal>
-- Create date: <09/13/2013>      
-- Description: <Check for sticky threads.> 
-- =============================================      
CREATE PROCEDURE [cw].[SearchForumsByDate]      -- execute cw.SearchForumsByDate 770 , 1, 10
 -- Add the parameters for the stored procedure here      
@DateToCheck DateTime,
@StartIndex INTEGER,
@EndIndex INTEGER
AS      
BEGIN      
 -- SET NOCOUNT ON added to prevent extra result sets from      
 -- interfering with SELECT statements.      
 SET NOCOUNT ON;
  SELECT *  
  INTO #TempForums
  FROM (
SELECT F.ID AS TopicId, 
IsNull(Views,0) AS Reads, 
F.Url AS Url, 
FC.Url AS ForumUrl,
F.Topic,
F.StartDateTime,
FC.Name AS ForumCategory, 
IsNull(C.Name, 'anonymous') AS CustomerName, 
IsNull(CP.Name, 'anonymous') AS LastPostBy,
IsNull(F.Posts, 0) AS Replies,
FT.MsgDateTime AS LastPostTime, 
IsNull(C.Id, '0') StartedById, 
IsNull(CP.Id,'0') LastPostedById, 
ISNULL(UP.HandleName ,'anonymous')  AS HandleName,
ISNULL(UPF.HandleName,'anonymous')  AS PostHandleName,
ROW_NUMBER() OVER(ORDER BY FT.MsgDateTime desc) AS RowNumber
FROM Forums F WITH(NOLOCK)
LEFT JOIN ForumThreads  FT WITH(NOLOCK) ON FT.ID = F.LastPostId
LEFT JOIN Customers  CP WITH(NOLOCK) ON CP.ID = FT.CustomerId
LEFT JOIN Customers C WITH(NOLOCK) ON C.ID = F.CustomerId
LEFT JOIN ForumSubCategories FC WITH(NOLOCK) ON FC.ID = F.ForumSubCategoryId
LEFT JOIN UserProfile UP WITH(NOLOCK) ON UP.UserId = C.Id
LEFT JOIN UserProfile UPF WITH(NOLOCK) ON UPF.UserId = CP.Id
WHERE 
FT.MsgDateTime >= @DateToCheck AND FT.IsModerated = 1 AND F.IsActive = 1 AND FT.IsActive = 1 ) AS T 

select count(*) as Total from #TempForums

select * from #TempForums
where RowNumber BETWEEN @StartIndex AND @EndIndex

drop table #TempForums
END 