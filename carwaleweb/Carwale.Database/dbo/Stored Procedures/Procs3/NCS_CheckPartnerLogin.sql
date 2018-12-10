IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[NCS_CheckPartnerLogin]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[NCS_CheckPartnerLogin]
GO

	CREATE PROCEDURE [dbo].[NCS_CheckPartnerLogin]
	@LoginId		VARCHAR(50), 
	@Make		INT, 
	@UserId		NUMERIC	OUTPUT,	--THIS IS THE ID OF THE USER
	@PassWd		VARCHAR(20)	OUTPUT, 
	@Name			VARCHAR(100)	OUTPUT,
	@MakeId INT OUTPUT
 AS
	
BEGIN

	--CHECK FOR THE ADMIN. IF THE USER NAME IS ADMIN THEN SELECT USER FRM ADMIN TABLE
	IF(UPPER(@LoginId) = 'CARWALE' )
	BEGIN
		--CHECK THE USER NAME FOR THE ADMIN
		SELECT @PassWd = Password, @UserId = ID, @Name 	= Name FROM NCS_PartnerLogin WHERE 
		LoginID = @LoginId AND isActive = 1
		
		SET @MakeId = @Make
		
	--CHECK WHETHER ANY USER EXSITS OR NOT
	IF (@@ROWCOUNT = 0)
		BEGIN
			SET @USERID = -1
		END	
	
	END
	ELSE
	BEGIN
		--CHECK THE USER NAME
	SELECT
		@UserId	 = ID,  
		@PassWd 	= Password, 
		@Name 	= Name,
		@MakeId = MakeId
		
	FROM 
		NCS_PartnerLogin
	WHERE 
		LoginID 	= @LoginId AND MakeId =@Make AND isActive = 1
		
	--CHECK WHETHER ANY USER EXSITS OR NOT
	IF (@@ROWCOUNT = 0)
		BEGIN
			SET @USERID = -1
		END	
	END

END