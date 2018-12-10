IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[Forums_AppendMessagesForMerge]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[Forums_AppendMessagesForMerge]
GO

	-- =============================================      
-- Author:  <Ravi Koshal>
-- Create date: <09/13/2013>      
-- Description: <Check for sticky threads.> 
-- =============================================      
CREATE procedure [cw].[Forums_AppendMessagesForMerge]      -- execute cw.Forums_UpdateMergeIdInactive
 -- Add the parameters for the stored procedure here      
 @PostIds VARCHAR (MAX)
 
AS      
BEGIN      
 -- SET NOCOUNT ON added to prevent extra result sets from      
 -- interfering with SELECT statements.      
 SET NOCOUNT ON;
 SELECT Message FROM ForumThreads,[dbo].[fnSplitCSV](@PostIds)
 WHERE ID = ListMember

 END 

