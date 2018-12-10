IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[PQ_SaveNewUser]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[PQ_SaveNewUser]
GO

	
CREATE PROCEDURE [dbo].[PQ_SaveNewUser]
	@BankId			SMALLINT,
	@UserName		VARCHAR(100),
	@LoginId		VARCHAR(50),
	@LoginPassword	VARCHAR(50),
	@UserType		SMALLINT,
	@CityId			NUMERIC,
	@Status         BIT OUTPUT
 AS
	
BEGIN
	SELECT Id FROM PQ_Users 
	WHERE LoginId = @LoginId AND BankId = @BankId AND Status = 1

	IF @@ROWCOUNT = 0
		BEGIN
			INSERT INTO PQ_Users(BankId, UserName, LoginId, LoginPassword, UserType, CityId, Status)
				VALUES(@BankId, @UserName, @LoginId, @LoginPassword, @UserType, @CityId, 1)

			SET @Status = 1
		END

	ELSE
		
		SET @Status = 0
END


