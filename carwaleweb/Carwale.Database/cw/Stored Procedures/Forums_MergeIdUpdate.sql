IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[Forums_MergeIdUpdate]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[Forums_MergeIdUpdate]
GO

	-- =============================================      
-- Author:  <Ravi Koshal>
-- Create date: <09/13/2013>      
-- Description: <Check for sticky threads.> 
-- =============================================      
CREATE procedure [cw].[Forums_MergeIdUpdate]      -- execute cw.Forums_CheckStickyThreads 1278
 -- Add the parameters for the stored procedure here      
 @Message VARCHAR(MAX),
 @MergeId NUMERIC(18,0)

 
AS      
BEGIN      
 -- SET NOCOUNT ON added to prevent extra result sets from      
 -- interfering with SELECT statements.      
 SET NOCOUNT ON;
UPDATE ForumThreads SET Message = @Message WHERE ID = @MergeId 
 
END 





