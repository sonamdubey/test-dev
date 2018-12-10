IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AxisBank_CheckUserLogin]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AxisBank_CheckUserLogin]
GO

	--THIS PROCEDURE VALIDATES THE USER NAME AND PASSWORD, for the carwale customers and return the id, passwd, name
--and isemailverified
-- Modified By : Ashish G. Kamble on 22 Apr 2013
-- Added PassswordSalt and PasswordHash instead of actual password
-- exec [dbo].[AxisBank_CheckUserLogin] 'akon12',0,'','','','','',0
CREATE PROCEDURE [dbo].[AxisBank_CheckUserLogin]
	@LoginId		VarChar(50),	
	@UserId			numeric(18,0) OUTPUT,
	@EmailId  varchar(100) OUTPUT,
	@PasswordSalt	VARCHAR(10) OUTPUT,
	@PasswordHash	VARCHAR (64) OUTPUT,
	@FirstName			VARCHAR(50)	OUTPUT,
	@LastName			VARCHAR(50)	OUTPUT,
	@IsVerified	BIT OUTPUT
 AS
	
BEGIN
	
	--CHECK THE USER NAME
	SELECT
		@UserId=UserId,
		@EmailId	 = Email,  
		@PasswordSalt = PasswordSalt,
		@PasswordHash = PasswordHash,
		@FirstName 	= FirstName, 
		@LastName	= LastName,
		@IsVerified = IsVerified 
	FROM 
		AxisBank_Users
	WHERE 
		LoginId = @LoginId AND
		IsActive = 1
		
	--CHECK WHETHER ANY USER EXSITS OR NOT
	IF (@@ROWCOUNT = 0)
	BEGIN
		SET @USERID = -1
	END

END
