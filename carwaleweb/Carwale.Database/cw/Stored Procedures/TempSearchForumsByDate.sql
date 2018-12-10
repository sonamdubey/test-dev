IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[TempSearchForumsByDate]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[TempSearchForumsByDate]
GO

	-- =============================================      
-- Author:  <Ravi Koshal>
-- Create date: <09/13/2013>      
-- Description: <Check for sticky threads.> 
-- =============================================      
CREATE procedure [cw].[TempSearchForumsByDate]      -- execute cw.TempSearchForumsByDate 770 , 1, 100
 -- Add the parameters for the stored procedure here      
@DateToCheck DateTime,
@StartIndex INTEGER,
@EndIndex INTEGER
AS      
BEGIN      
 -- SET NOCOUNT ON added to prevent extra result sets from      
 -- interfering with SELECT statements.      
 SET NOCOUNT ON;
-- WITH T1 AS 
-- (
-- SELECT F.ID AS TopicId, 
--IsNull(Views,0) AS Reads, 
--F.Url AS Url, 
--FC.Url AS ForumUrl,
--F.Topic, 
--IsNull(C.Name, 'anonymous') AS CustomerName, 
--IsNull(CP.Name, 'anonymous') AS LastPostBy,
--IsNull(F.Posts, 0) AS Replies,
--FT.MsgDateTime AS LastPostTime, 
--IsNull(C.Id, '0') StartedById, 
--IsNull(CP.Id,'0') LastPostedById, 
--ISNULL(UP.HandleName ,'anonymous')  AS HandleName,
--ISNULL(UPF.HandleName,'anonymous')  AS PostHandleName,
--ROW_NUMBER() OVER(ORDER BY FT.MsgDateTime desc) AS RowNumber
--FROM Forums F
--LEFT JOIN ForumThreads  FT ON FT.ID = F.LastPostId
--LEFT JOIN Customers  CP ON CP.ID = FT.CustomerId
--LEFT JOIN Customers C ON C.ID = F.CustomerId
--LEFT JOIN ForumSubCategories FC ON FC.ID = F.ForumSubCategoryId
--LEFT JOIN UserProfile UP ON UP.UserId = C.Id
--LEFT JOIN UserProfile UPF ON UPF.UserId = CP.Id
--WHERE 
--FT.MsgDateTime >= @DateToCheck AND FT.IsModerated = 1 AND F.IsActive = 1 AND FT.IsActive = 1 
-- )

-- SELECT COUNT(*) FROM T1 AS Total

-- SELECT COUNT(*) as Total
--FROM Forums F
--LEFT JOIN ForumThreads  FT ON FT.ID = F.LastPostId
--LEFT JOIN Customers  CP ON CP.ID = FT.CustomerId
--LEFT JOIN Customers C ON C.ID = F.CustomerId
--LEFT JOIN ForumSubCategories FC ON FC.ID = F.ForumSubCategoryId
--LEFT JOIN UserProfile UP ON UP.UserId = C.Id
--LEFT JOIN UserProfile UPF ON UPF.UserId = CP.Id
--WHERE 
--FT.MsgDateTime >= @DateToCheck AND FT.IsModerated = 1 AND F.IsActive = 1 AND FT.IsActive = 1 

  SELECT *  
  INTO #TempForums
  FROM (
SELECT F.ID AS TopicId, 
IsNull(Views,0) AS Reads, 
F.Url AS Url, 
FC.Url AS ForumUrl,
F.Topic, 
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
FT.MsgDateTime >= @DateToCheck AND FT.IsModerated = 1 AND F.IsActive = 1 AND FT.IsActive = 1 ) AS T 
--WHERE  T.RowNumber BETWEEN @StartIndex
--			AND @EndIndex

    select count(*) as Total from #TempForums

	select *   from #TempForums
	where RowNumber BETWEEN @StartIndex AND @EndIndex

	drop table #TempForums		 

END 
