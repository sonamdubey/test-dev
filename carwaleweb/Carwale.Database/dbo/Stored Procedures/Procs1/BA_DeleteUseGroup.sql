IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BA_DeleteUseGroup]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BA_DeleteUseGroup]
GO

	-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[BA_DeleteUseGroup]
	@GroupId INT
	
AS
BEGIN
	SET NOCOUNT OFF;
	DECLARE @Status BIT = 0

	--Update the Group table --Set IsActive = 0 
	UPDATE [dbo].[BA_Groups]
			SET [IsActive] = 0 
			 WHERE ID = @GroupId 

--Update the GroupDetails table --Set IsActive = 0 
	UPDATE [dbo].[BA_GroupDetails]
			SET [IsActive] = 0 
			 WHERE GroupId = @GroupId 
  
SET @Status = @@ROWCOUNT --Set the status value

--Return the Status
SELECT  @Status AS Status


END
