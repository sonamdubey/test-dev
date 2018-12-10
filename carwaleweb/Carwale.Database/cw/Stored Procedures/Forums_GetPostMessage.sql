IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[Forums_GetPostMessage]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[Forums_GetPostMessage]
GO

	-- =============================================      
-- Author:  <Ravi Koshal>
-- Create date: <09/13/2013>      
-- Description: <Check for sticky threads.> 
-- =============================================      
CREATE procedure [cw].[Forums_GetPostMessage]      -- execute cw.Forums_GetAllThreadDetails 770
 -- Add the parameters for the stored procedure here      
@PostId NUMERIC(18,0),
@Message VARCHAR(MAX) OUTPUT

AS      
BEGIN      
 -- SET NOCOUNT ON added to prevent extra result sets from      
 -- interfering with SELECT statements.      
SET NOCOUNT ON;
SELECT @Message=Message FROM ForumThreads WHERE Id = @PostId
END 
       
