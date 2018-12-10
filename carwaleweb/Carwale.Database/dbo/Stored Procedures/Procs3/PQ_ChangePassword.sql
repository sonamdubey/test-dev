IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[PQ_ChangePassword]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[PQ_ChangePassword]
GO

	
CREATE PROCEDURE [dbo].[PQ_ChangePassword]
	@Id					NUMERIC,
	@ExistingPassword	VARCHAR(50),
	@LoginPassword		VARCHAR(50),
	@Status				BIT OUTPUT
 AS
	
BEGIN

	SELECT Id FROM PQ_Users WHERE Id = @Id AND LoginPassword = @ExistingPassword
	IF @@ROWCOUNT = 1
		BEGIN
			UPDATE PQ_Users SET LoginPassword = @LoginPassword
				WHERE Id = @Id
				
				SET @Status = 1
		END

	ELSE
		SET @Status = 0
END



