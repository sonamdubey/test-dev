IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BA_RenameUserGroup]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BA_RenameUserGroup]
GO

	-- =============================================
-- Author:		Ranjeet Kumar
-- Create date: 07-06-14
-- Description:	Rename the User Group
-- =============================================
CREATE PROCEDURE [dbo].[BA_RenameUserGroup]
@GroupId INT,
@NewName VARCHAR(50)
AS
BEGIN
DECLARE @Status TINYINT = 0
	SET NOCOUNT OFF;
	UPDATE BA_Groups SET GroupName = @NewName, ModifyDate = GETDATE() WHERE ID = @GroupId
	SET @Status = @@ROWCOUNT 

	SELECT @Status AS Status --return Status
END
