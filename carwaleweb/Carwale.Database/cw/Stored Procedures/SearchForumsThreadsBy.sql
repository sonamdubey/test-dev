IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[SearchForumsThreadsBy]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[SearchForumsThreadsBy]
GO

	-- =============================================      
-- Author:  <Ravi Koshal>
-- Create date: <09/13/2013>      
-- Description: <Check for sticky threads.> 
-- =============================================      
CREATE procedure [cw].[SearchForumsThreadsBy]      -- execute cw.Forums_GetAllThreadDetails 770
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
 INTO #TempForumsThreadsBy
 FROM (
SELECT F.ID AS TopicId, 
IsNull(Views,0) AS Reads, 
F.Url AS Url, 
FC.Url AS ForumUrl,
FC.Name ForumCategory, 
FC.Id ForumCategoryId, 
F.Topic,
F.StartDateTime, 
IsNull(C.Name, 'anonymous') AS CustomerName, 
IsNull(CP.Name, 'anonymous') AS LastPostBy,
IsNull(F.Posts, 0) AS Replies,
FT.MsgDateTime AS LastPostTime, 
IsNull(C.Id, '0') StartedById, 
IsNull(CP.Id,'0') LastPostedById, 
ISNULL(UP.HandleName ,'anonymous')  AS HandleName,
ISNULL(UPF.HandleName,'anonymous')  AS PostHandleName,
ROW_NUMBER() OVER(ORDER BY FT.MsgDateTime desc) AS RowNumber
FROM Forums F
LEFT JOIN ForumThreads  FT ON FT.ID = F.LastPostId
LEFT JOIN Customers  CP ON CP.ID = FT.CustomerId
LEFT JOIN Customers C ON C.ID = F.CustomerId
LEFT JOIN ForumSubCategories FC ON FC.ID = F.ForumSubCategoryId
LEFT JOIN UserProfile UP ON UP.UserId = C.Id
LEFT JOIN UserProfile UPF ON UPF.UserId = CP.Id
WHERE 
F.CustomerId = @CustomerId AND FT.IsModerated = 1 AND F.IsActive = 1 AND FT.IsActive = 1 ) AS T 

select count(*) as Total from #TempForumsThreadsBy

select * from #TempForumsThreadsBy
where RowNumber BETWEEN @StartIndex AND @EndIndex

drop table #TempForumsThreadsBy			

END 


