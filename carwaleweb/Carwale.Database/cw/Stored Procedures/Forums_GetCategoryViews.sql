IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[Forums_GetCategoryViews]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[Forums_GetCategoryViews]
GO

	-- =============================================      
-- Author:  <Ravi Koshal>
-- Create date: <09/13/2013>      
-- Description: <Check for sticky threads.> 
-- =============================================      
CREATE procedure [cw].[Forums_GetCategoryViews]      -- execute cw.Forums_GetAllThreadDetails 770
 -- Add the parameters for the stored procedure here      
AS      
BEGIN      
 -- SET NOCOUNT ON added to prevent extra result sets from      
 -- interfering with SELECT statements.      
SET NOCOUNT ON;
SELECT	CategoryId, COUNT(SessionID) AS NoOfViews FROM	ForumUserTracking
WHERE	ActivityId IN (2,3,4,5) AND CategoryId <> -1 AND DATEDIFF(MINUTE, ActivityDateTime, getdate()) < 60
GROUP BY CategoryId
END 
       

