IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CheckCarwaleLogin]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CheckCarwaleLogin]
GO

	
--THIS PROCEDURE VALIDATES THE USER NAME AND PASSWORD, for the carwale customers and return the id, passwd, name
--and isemailverified

CREATE PROCEDURE [dbo].[CheckCarwaleLogin]
	@LoginId		VARCHAR(50), 
	@UserId		NUMERIC	OUTPUT,	--THIS IS THE ID OF THE USER
	@PassWd		VARCHAR(20)	OUTPUT, 
	@Name			VARCHAR(100)	OUTPUT,
	@IsEmailVerified		BIT OUTPUT
 AS
	
BEGIN
	
	--CHECK THE USER NAME
	SELECT
		@UserId	 = ID,  
		@PassWd 	= Password, 
		@Name 	= Name, 
		@IsEmailVerified = isEmailVerified 
	FROM 
		Customers 
	WHERE 
		Email = @LoginId AND
		IsFake = 0
		
	--CHECK WHETHER ANY USER EXSITS OR NOT
	IF (@@ROWCOUNT = 0)
	BEGIN
		SET @USERID = -1
	END
	
END
