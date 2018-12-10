IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[OPRCHECKLOGINTEST_22082016]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[OPRCHECKLOGINTEST_22082016]
GO

	

--THIS PROCEDURE VALIDATES THE USER NAME AND PASSWORD, AND RETURN THE USER ROLE AND THE USER ID
--IN CASE NO SUCH USER NAME AND PASSWORD EXISTS THEN IT RETURNS -1 FOR USERID AND BLANK FOR ROLE
--Modified by : Sachin bharti on 27th Nov 2013 (Added Hashing)
CREATE PROCEDURE [dbo].[OPRCHECKLOGINTEST_22082016]
	@LOGINID				VARCHAR(30), 
	@USERNAME				VARCHAR(50) = NULL OUTPUT ,
	@PASSWD					VARCHAR(20)	= NULL OUTPUT ,
	@USERROLE				VARCHAR(1000) = NULL	OUTPUT,	--THIE IS THE ROLE OF THE USER
	@USERID					NUMERIC = NULL	OUTPUT,	--THIS IS THE ID OF THE USER,
	@IsOutsideAccess		NUMERIC = 0	OUTPUT,	--IS OUTSIDE ACCESS ALLOWED
	@CLExtension			NUMERIC = -1	OUTPUT,	--THIS IS clextension map of the user
	@DCRMExtension			NUMERIC = NULL OUTPUT,
	@CLExtensionType		NUMERIC = NULL OUTPUT,
	@CLOfficeType			NUMERIC = NULL OUTPUT,
	@CorrectHashedPwd		VARCHAR(70) = NULL OUTPUT,-- Added By Amit Kumar On 11th feb 2013
	@Salt					VARCHAR(10) = NULL OUTPUT,
	@DrishtiLogin			NUMERIC = NULL OUTPUT, -- Added By Amit Kumar On 8th jan 2014
	@IsCarTrade				BIT = NULL OUTPUT
 AS
	
BEGIN
	--CHECK FOR THE ADMIN. IF THE USER NAME IS ADMIN THEN SELECT USER FRM ADMIN TABLE
	IF(UPPER(@LOGINID) = 'ADMIN' )
	BEGIN
		--CHECK THE USER NAME FOR THE ADMIN
		SELECT  @PASSWD = PASSWORD,@USERID = ID, @USERROLE = TASKIDS, 
		@USERNAME = USERNAME,@CorrectHashedPwd = PasswordHash,@Salt = HashSalt ,@IsCarTrade = IsCarTrade
		FROM OPRUSERS WITH(NOLOCK)WHERE LOGINID = @LOGINID AND IsActive = 1
		
		
		--CHECK WHETHER ANY USER EXSITS OR NOT
		IF (@@ROWCOUNT = 0)
		BEGIN
			SET @USERID = -1
			SET @USERROLE = ''		
		END
	END
	ELSE
	BEGIN
		--CHECK THE USER NAME FOR OTHER THAN ADMIN
		SELECT   @PASSWD = PASSWORD,@USERID = ID, @USERROLE = TASKIDS,
		@USERNAME = USERNAME, @IsOutsideAccess = IsOutsideAccess, 
		@CorrectHashedPwd = PasswordHash ,@Salt = HashSalt,@IsCarTrade = IsCarTrade
		FROM OPRUSERS WITH(NOLOCK) WHERE LOGINID = @LOGINID AND IsActive = 1
		
		--CHECK WHETHER ANY USER EXSITS OR NOT
		IF (@@ROWCOUNT = 0)
		BEGIN
			SET @USERID = -1
			SET @USERROLE = ''		
		END
		
	END
	
	SET @CLExtension = -1
	--fetch the clextension mapping for the user
	if @USERID <> -1
	BEGIN
		Select @CLExtension = Extension, @CLExtensionType = DialerType, @CLOfficeType = OfficeId,@DrishtiLogin=DrishtiLoginId 
		From CL_ExtensionMap WITH(NOLOCK)
		 Where UserId = @USERID
		Select @DCRMExtension = Id 
		FROM DCRM_UserExtensions WITH(NOLOCK)  WHERE UserId = @USERID
	END

	PRINT @UserID
END



-------------------------------------------------------------------------------------
