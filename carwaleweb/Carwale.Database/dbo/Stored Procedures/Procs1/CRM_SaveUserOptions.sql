IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_SaveUserOptions]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_SaveUserOptions]
GO

	


CREATE PROCEDURE [dbo].[CRM_SaveUserOptions]

	@UserId				Numeric,
	@Type				Bit
 AS
	
BEGIN
		UPDATE CRM_UserOptions SET IsVerificationScript = @Type
		WHERE UserId = @UserId
		
		IF @@ROWCOUNT = 0
			BEGIN
				INSERT INTO CRM_UserOptions(UserId, IsVerificationScript)
					VALUES(@UserId, @Type)
			END
END















