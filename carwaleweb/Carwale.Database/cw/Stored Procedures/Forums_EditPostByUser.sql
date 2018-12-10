IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[Forums_EditPostByUser]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[Forums_EditPostByUser]
GO

	-- =============================================      
-- Author:  <Ravi Koshal>
-- Create date: <09/17/2013>      
-- Description: <Delete a post and update post count for the customer accordingly in user profile table.> 
-- =============================================      
CREATE procedure [cw].[Forums_EditPostByUser]      -- execute cw.Forums_DeletePost 1
 -- Add the parameters for the stored procedure here      
@Message VARCHAR(MAX),
@UpdatedBy NUMERIC(18,0),
@PostId NUMERIC(18,0)
 
AS      
BEGIN      
 -- SET NOCOUNT ON added to prevent extra result sets from      
 -- interfering with SELECT statements.      
SET NOCOUNT ON;
UPDATE ForumThreads 
SET 
Message = @Message,LastUpdatedTime = GETDATE(), UpdatedBy = @UpdatedBy
WHERE ID = @PostId
END 
       
