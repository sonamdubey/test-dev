IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[PQ_DoLogin]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[PQ_DoLogin]
GO

	
CREATE PROCEDURE [dbo].[PQ_DoLogin]
	@LoginId		VARCHAR(50),
	@LoginPassword	VARCHAR(50),
	@BankId			SMALLINT,
	@LastLoginDate	DATETIME,
	@Id				NUMERIC OUTPUT,
	@UserName		VARCHAR(100) OUTPUT,
	@UserType		SMALLINT OUTPUT,
	@Status         BIT OUTPUT
 AS
	
BEGIN
	SELECT @Id = Id, @UserName = UserName, @UserType = UserType FROM PQ_Users 
	WHERE LoginId = @LoginId AND LoginPassword = @LoginPassword
			AND BankId = @BankId AND Status = 1

	IF @@ROWCOUNT = 1
		BEGIN
			UPDATE PQ_Users SET LastLoginDate = @LastLoginDate
			WHERE Id = @Id

			SET @Status = 1
		END
	ELSE
		
		SET @Status = 0
END


