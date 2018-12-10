IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[OPRCHECKLOGINTEST]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[OPRCHECKLOGINTEST]
GO

	--THIS PROCEDURE VALIDATES THE USER NAME AND PASSWORD, AND RETURN THE USER ROLE AND THE USER ID
--IN CASE NO SUCH USER NAME AND PASSWORD EXISTS THEN IT RETURNS -1 FOR USERID AND BLANK FOR ROLE
--Modified by : Sachin bharti on 27th Nov 2013 (Added Hashing)
--Modified by : Kartik Rathod on 1 aug 2016,check for attendance status of employee at time of login (added @IsPresent)
CREATE PROCEDURE [dbo].[OPRCHECKLOGINTEST] --'satish'
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
	@IsCarTrade				BIT = NULL OUTPUT,
	@IsPresent				BIT = NULL OUTPUT
 AS
	
BEGIN
	--CHECK FOR THE ADMIN. IF THE USER NAME IS ADMIN THEN SELECT USER FRM ADMIN TABLE
	IF(UPPER(@LOGINID) = 'ADMIN' )
	BEGIN
		--CHECK THE USER NAME FOR THE ADMIN
		SELECT  @PASSWD = O.PASSWORD,@USERID = O.ID, @USERROLE = O.TASKIDS, 
		@USERNAME = O.USERNAME,@CorrectHashedPwd = O.PasswordHash,@Salt = O.HashSalt ,@IsCarTrade = ISNULL(O.IsCarTrade,0),@IsPresent = CASE WHEN AD.ID IS NULL AND AM.OprUserId IS NULL THEN 0 ELSE 1 END 
		FROM OPRUSERS O WITH(NOLOCK)
		LEFT JOIN HR_AttendanceDetails	AD WITH(NOLOCK)  ON  CONVERT(INT,[dbo].[udf_GetNumeric](O.EmployeeCode)) = CONVERT(INT,AD.EmployeeCode) AND CONVERT(DATE,AD.Date) = CONVERT(DATE,GETDATE()) AND AD.StatusId = 1
		LEFT JOIN  HR_TempAttendanceMarkLogs AM WITH(NOLOCK) ON O.Id = AM.OprUserId 
		WHERE O.LOGINID = @LOGINID AND O.IsActive = 1
		
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
		SELECT   @PASSWD = O.PASSWORD,@USERID = O.ID, @USERROLE = O.TASKIDS,
		@USERNAME = O.USERNAME, @IsOutsideAccess = O.IsOutsideAccess, 
		@CorrectHashedPwd = O.PasswordHash ,@Salt = O.HashSalt,@IsCarTrade =ISNULL(O.IsCarTrade,0), @IsPresent=  CASE WHEN AD.ID IS NULL AND AM.OprUserId IS NULL THEN 0 ELSE 1 END 
		FROM OPRUSERS O WITH(NOLOCK) 
		LEFT JOIN  HR_AttendanceDetails	AD  WITH(NOLOCK)  ON  CONVERT(INT,[dbo].[udf_GetNumeric](O.EmployeeCode))  = CONVERT(INT,AD.EmployeeCode) AND CONVERT(DATE,AD.Date) = CONVERT(DATE,GETDATE()) AND AD.StatusId = 1
		LEFT JOIN  HR_TempAttendanceMarkLogs AM WITH(NOLOCK) ON O.Id = AM.OprUserId 
		WHERE O.LOGINID = @LOGINID AND O.IsActive = 1
		
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

END
