IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CheckAffiliateMemberLogin]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CheckAffiliateMemberLogin]
GO

	
--THIS PROCEDURE VALIDATES THE USER NAME AND PASSWORD, for the carwale customers and return the id, passwd, name
--and isemailverified

CREATE PROCEDURE [dbo].[CheckAffiliateMemberLogin]
	@LoginId		VARCHAR(50), 
	@UserId		NUMERIC	OUTPUT,	--THIS IS THE ID OF THE USER
	@PassWd		VARCHAR(50)	OUTPUT, 
	@Name			VARCHAR(100)	OUTPUT,
	@Email			VARCHAR(100)	OUTPUT
 AS
	
	BEGIN
	
		--CHECK THE USER NAME
		SELECT
			@UserId	 = ID,  
			@PassWd 	= Passwd, 
			@Name 	= ContactPerson, 
			@Email = ContactEmail 
		FROM 
			AffiliateMembers 
		WHERE 
			LoginId = @LoginId  AND
			IsActive = 1
		
		--CHECK WHETHER ANY USER EXSITS OR NOT
		
		IF (@@ROWCOUNT = 0)

			BEGIN
				SET @USERID = -1
			END
	
	END
