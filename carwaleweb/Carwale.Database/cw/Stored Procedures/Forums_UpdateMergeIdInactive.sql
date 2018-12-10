IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[Forums_UpdateMergeIdInactive]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[Forums_UpdateMergeIdInactive]
GO

	-- =============================================      
-- Author:  <Ravi Koshal>
-- Create date: <09/13/2013>      
-- Description: <Check for sticky threads.> 
-- =============================================      
CREATE procedure [cw].[Forums_UpdateMergeIdInactive]      -- execute cw.Forums_Update
 -- Add the parameters for the stored procedure here      
 @ThreadId VARCHAR (MAX)
 
AS      
BEGIN      
 -- SET NOCOUNT ON added to prevent extra result sets from      
 -- interfering with SELECT statements.      
 SET NOCOUNT ON;
 
 
UPDATE ForumThreads SET IsActive=0 WHERE ID IN (@ThreadId)

 
END 


