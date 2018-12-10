IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertUsers]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertUsers]
GO

	
--THIS PROCEDURE IS FOR INSERTING AND UPDATING RECORDS FOR USERS TABLE
--Modifier: Sachin Bharti(31st Dec 2013) Added Hashing
--Modifier :Ajay Singh(on 7 jan 2015) added 3 more parameters

CREATE PROCEDURE [dbo].[InsertUsers]
	@Id				NUMERIC,		-- Id. Will be -1 if Its Insertion
	@loginid 		VARCHAR(30),		
	@PassWord		VARCHAR(20) = NULL,		
	@UserName		VARCHAR(50),
	@HashedPassword VARCHAR(70) = NULL,-- Added by amit kumar	
	@Address		VARCHAR(255),
	@PhoneNo		VARCHAR(20),
	@RoleIds		VARCHAR(500),
	@TaskIds		VARCHAR(1000),
	@IsOutsideAccess	BIT, 
	@status			INTEGER OUTPUT,
	@EmployeeCode	VARCHAR(15) = NULL,
	@Salt			VARCHAR(10) = NULL,
	--added by Ajay singh 3 parametrs
	@ReportingManager INT = NULL,
	@Designation      INT = NULL,
	@BusinessUnit     INT = NULL

	
 AS
	DECLARE @temp AS INTEGER
		
BEGIN
	SET @status = 0	
	IF @Id = -1 -- Insertion
		
		BEGIN
			SELECT ID FROM OPRUSERS WHERE LOGINID = @loginid
			IF @@ROWCOUNT = 0 
				BEGIN			
					INSERT INTO OPRUSERS
						(userName,Address, PhoneNo, LoginId , IsActive , RoleIds , TaskIds , IsOutsideAccess,PasswordHash ,EmployeeCode,HashSalt,ReportingManagerId,Designation,UserDeptId)--PassWord,
				 	VALUES 
						(@UserName,@Address, @PhoneNo, @loginid, 1, @RoleIds , @TaskIds , @IsOutsideAccess,@HashedPassword,@EmployeeCode,@Salt,@ReportingManager,@Designation,@BusinessUnit)--,@PassWord
		
					SET @status = 0
				END 
			ELSE 
					SET @status = -1
		END
	ELSE
		BEGIN
			SELECT ID FROM OPRUSERS WHERE LOGINID = @loginid 	AND ID <> @Id
			IF @@ROWCOUNT = 0 
				BEGIN		
					UPDATE	OPRUSERS SET 
							userName	           =    @UserName
						,	Address		           =	@Address
						,	PhoneNo		           = 	@PhoneNo
						,	LoginId		           =	@loginid
						--,	PassWord	           =	NULL
						,	RoleIds		           =	@RoleIds
						,	TaskIds		           =	@TaskIds
						,	IsOutsideAccess		   =	@IsOutsideAccess
						,	PasswordHash		   =	@HashedPassword
						,	EmployeeCode		   =	@EmployeeCode
						,	HashSalt	           =	@Salt
						,   ReportingManagerId     =    @ReportingManager
						,   Designation            =    @Designation
						,   UserDeptId             =    @BusinessUnit
					WHERE
						ID = @Id
					SET @status = 0
				END 
			ELSE 
				SET @status = -1
			
		END
	
END


