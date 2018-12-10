IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[Forums_DeleteThread]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[Forums_DeleteThread]
GO

	-- =============================================      
-- Author:  <Ravi Koshal>
-- Create date: <09/17/2013>      
-- Description: <Delete a post and update post count for the customer accordingly in user profile table.> 
-- =============================================      
CREATE procedure [cw].[Forums_DeleteThread]      -- execute cw.Forums_DeletePost 1
 -- Add the parameters for the stored procedure here      
@ThreadId NUMERIC(18,0)
 
AS      
BEGIN      
 -- SET NOCOUNT ON added to prevent extra result sets from      
 -- interfering with SELECT statements.      
SET NOCOUNT ON;
UPDATE Forums SET IsActive=0 WHERE ID = @ThreadId
END 



       
