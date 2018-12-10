IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[Forums_GetThreadViews]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[Forums_GetThreadViews]
GO

	-- =============================================      
-- Author:  <Ravi Koshal>
-- Create date: <09/13/2013>      
-- Description: <Check for sticky threads.> 
-- =============================================      
CREATE procedure [cw].[Forums_GetThreadViews]      -- execute cw.Forums_GetAllThreadDetails 770
 -- Add the parameters for the stored procedure here      
@ThreadIds VARCHAR(MAX)
AS      
BEGIN      
 -- SET NOCOUNT ON added to prevent extra result sets from      
 -- interfering with SELECT statements.      
SET NOCOUNT ON;
SELECT	ThreadId, COUNT(SessionID) AS NoOfViews 
FROM ForumUserTracking
WHERE	ActivityId IN (4,5) AND ThreadId IN (select listmember from  [dbo].[fnSplitCSVValues](@ThreadIds)) AND DATEDIFF(MINUTE, ActivityDateTime, getdate()) < 60
GROUP BY ThreadId
END 
   

