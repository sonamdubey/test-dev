IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[UpdateContentViewCount]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[UpdateContentViewCount]
GO

	
-- =============================================            
-- Author:  <Ravi Koshal>            
-- Create date: <15/04/2014>            
-- Description: <Updates the view count of the contentId passed in argument.>
-- =============================================            
CREATE PROCEDURE [cw].[UpdateContentViewCount] -- EXEC [cw].[UpdateContentViewCount] 
	-- Add the parameters for the stored procedure here            
	@ContentId NUMERIC(18, 0) = 0
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from            
	-- interfering with SELECT statements.            
	SET NOCOUNT ON;
		UPDATE Con_EditCms_Basic
		SET [Views] = [Views] + 1
		WHERE Id = @ContentId
END