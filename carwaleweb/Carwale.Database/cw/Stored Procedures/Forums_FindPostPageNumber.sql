IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[Forums_FindPostPageNumber]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[Forums_FindPostPageNumber]
GO

	-- =============================================      
-- Author:  <Ravi Koshal>
-- Create date: <09/13/2013>      
-- Description: <Check for sticky threads.> 
-- =============================================      
CREATE procedure [cw].[Forums_FindPostPageNumber]      -- execute cw.Forums_CheckStickyThreads 1278
 -- Add the parameters for the stored procedure here      
 @ForumThreadId NUMERIC(18,0),
 @ThreadId NUMERIC(18,0)
 
AS      
BEGIN      
 -- SET NOCOUNT ON added to prevent extra result sets from      
 -- interfering with SELECT statements.      
 SET NOCOUNT ON;
 SELECT Count(ID) AS OldPosts 
 FROM ForumThreads 
 WHERE IsActive=1 AND IsModerated = 1 AND ForumId = @ThreadId AND MsgDateTime < (SELECT MsgDateTime FROM ForumThreads WHERE Id = @ForumThreadId AND IsModerated = 1)

 
END 

