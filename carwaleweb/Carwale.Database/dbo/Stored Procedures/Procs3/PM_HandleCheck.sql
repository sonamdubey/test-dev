IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[PM_HandleCheck]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[PM_HandleCheck]
GO

	CREATE PROCEDURE [dbo].[PM_HandleCheck]
(
	@UserName AS VARCHAR(500),
	@UserID AS NUMERIC(18,0),
	@HandleName AS VARCHAR(500) OUT
)
AS
BEGIN

	SELECT @HandleName = HandleName FROM UserProfile 
	WHERE 
		REPLACE( HandleName, '.', '') = REPLACE( @UserName, '.', '')
		
END
