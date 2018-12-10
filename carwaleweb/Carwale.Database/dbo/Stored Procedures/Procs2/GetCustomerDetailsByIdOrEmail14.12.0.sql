IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetCustomerDetailsByIdOrEmail14]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetCustomerDetailsByIdOrEmail14]
GO

	-- =============================================
-- Author:		amit vema
-- Create date: 12 june 2014
-- Description:	get customer details by email id
--modified by rohan sapkal on 17-05-2015 , Fetches Oauth, SETs Oauth if null
-- =============================================

CREATE PROCEDURE [dbo].[GetCustomerDetailsByIdOrEmail14.12.0]
	@CustomerId		NUMERIC = NULL,	
	@LoginId		VARCHAR(50) = NULL,
	@NewOAuth	VARCHAR(50)=NULL, 
	@UserId		NUMERIC	OUTPUT,	--THIS IS THE CustomerId OF THE USER
	@Email		VARCHAR(50)	OUTPUT,	--THIS IS THE LoginId OF THE USER
	@PassWd		VARCHAR(20)	OUTPUT,
	@PwdHashSaltStr VARCHAR(100)	OUTPUT,
	@Name			VARCHAR(100)	OUTPUT,
	@IsEmailVerified		BIT OUTPUT,
	@Mobile VARCHAR(20)	OUTPUT,
	@OAuth VARCHAR(50) OUTPUT
 AS
	
BEGIN
	
	--CHECK THE USER NAME
	IF(@CustomerId IS NULL AND @LoginId IS NULL)
		SET @USERID = -1
	ELSE
		SELECT TOP 1
			@UserId	 = ID,  
			@PassWd 	= Password,
			@Name 	= Name, 
			@IsEmailVerified = isEmailVerified,
			@Email = email,
			@PwdHashSaltStr = PwdSaltHashStr,
			@Mobile = Mobile,
			@OAuth = OAuth 			 
		FROM 
			Customers WITH(NOLOCK)
		WHERE 
			(Id = @CustomerId OR Email = @LoginId)
			AND IsFake = 0
		
	--CHECK WHETHER ANY USER EXSITS OR NOT
	IF (@@ROWCOUNT = 0)
	BEGIN
		SET @USERID = -1
	END
	ELSE IF(@OAuth is NULL AND @NewOAuth IS NOT NULL)
	BEGIN
	UPDATE Customers SET OAuth=@NewOAuth WHERE Id=@UserId
	SET @OAuth=@NewOAuth
	END
	
END

/****** Object:  StoredProcedure [dbo].[OAuthCheck]    Script Date: 19-02-2015 16:23:36 ******/
SET ANSI_NULLS ON
