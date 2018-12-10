IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[ShowReportAbuseReport]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[ShowReportAbuseReport]
GO

	-- =============================================      
-- Author:  <Ravi Koshal>
-- Create date: <09/13/2013>      
-- Description: <Check for sticky threads.> 
-- =============================================      
CREATE procedure [cw].[ShowReportAbuseReport]      -- execute cw.Forums_GetAllThreadDetails 770
 -- Add the parameters for the stored procedure here      

AS      
BEGIN      
 -- SET NOCOUNT ON added to prevent extra result sets from      
 -- interfering with SELECT statements.      
 SET NOCOUNT ON;
 
SELECT RA.ID as RID, RA.PostId as ID, RA.CustomerId as CustomerId, RA.ThreadId as ThreadId,F.Topic as Thread, FT.Message as Post, C1.Name as Customer, RA.Comment as Comment, RA.CreateDate as Date1 
FROM Forum_ReportAbuse RA 
LEFT JOIN Customers C1 ON C1.ID = RA.CustomerId 
LEFT JOIN ForumThreads FT ON FT.ID = RA.PostId 
LEFT JOIN Forums F on F.ID = RA.ThreadId 
WHERE RA.Status = 0 
ORDER BY RA.CreateDate DESC
 
END 
       


