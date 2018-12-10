IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BA_CreateUserGroup]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BA_CreateUserGroup]
GO

	-- =============================================
-- Author:		Ranjeet kumar
-- Create date: 28-may-14
-- Description:	Create group for the User
-- =============================================
CREATE PROCEDURE [dbo].[BA_CreateUserGroup]
@BrokerId INT,
@GroupName VARCHAR(100),
@CreatedOn DATETIME

AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @GroupId INT = -1
	DECLARE @Status TINYINT = 0 --To return status
--Check Same Name Exist for this Broker or Not
IF (SELECT COUNT(BG.ID) FROM [dbo].[BA_Groups] AS BG WITH (NOLOCK) WHERE BG.GroupName = @GroupName AND BG.BrokerId = @BrokerId ) = 0 
	BEGIN
		INSERT INTO [dbo].[BA_Groups]
			   ([BrokerId]
			   ,[GroupName]
			   ,[IsActive]
			   ,[CreatedOn]
			   ,[ModifyDate]
			   ,[DeletedOn])
		 VALUES
				(@BrokerId
			   ,@GroupName
			   ,1
			   ,@CreatedOn
			   ,NULL
			   ,NULL)

		--Get the inserted Id 
		SELECT @GroupId = SCOPE_IDENTITY()

		IF @GroupId <> -1
			SET @Status  = 1 --If Success

END --END IF

ELSE
	SET @Status  = 2 --Group Name Already Exists

SELECT @Status AS Status,@GroupId AS Id  ---Return status AND Id
 

END
