IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AW_ChangePassword]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AW_ChangePassword]
GO

	
CREATE PROCEDURE [dbo].[AW_ChangePassword]
	@Id					NUMERIC,
	@ExistingPassword	VARCHAR(50),
	@LoginPassword		VARCHAR(50),
	@Status				BIT OUTPUT
 AS
	
BEGIN

	SELECT Id FROM TrilogyLogin WHERE Id = @Id AND Password = @ExistingPassword
	IF @@ROWCOUNT = 1
		BEGIN
			UPDATE TrilogyLogin SET Password = @LoginPassword
				WHERE Id = @Id
				
				SET @Status = 1
		END

	ELSE
		SET @Status = 0
END



