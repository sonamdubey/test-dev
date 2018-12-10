IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[Forums_DeletePost]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[Forums_DeletePost]
GO

	-- =============================================      
-- Author:  <Ravi Koshal>
-- Create date: <09/17/2013>      
-- Description: <Delete a post and update post count for the customer accordingly in user profile table.> 
-- =============================================      
CREATE procedure [cw].[Forums_DeletePost]      -- execute cw.orums_DeletePost 1278,775
 -- Add the parameters for the stored procedure here      
 @ThreadId NUMERIC(18,0)
 
AS      
BEGIN      
 -- SET NOCOUNT ON added to prevent extra result sets from      
 -- interfering with SELECT statements.      
 SET NOCOUNT ON;
DECLARE @UserId NUMERIC (18,0)
UPDATE ForumThreads SET IsActive=0 WHERE ID = @ThreadId
SELECT @UserId = CustomerId FROM ForumThreads WHERE ID = @ThreadId
EXECUTE cw.Forums_UpdatePostCount @UserId
END 
       
