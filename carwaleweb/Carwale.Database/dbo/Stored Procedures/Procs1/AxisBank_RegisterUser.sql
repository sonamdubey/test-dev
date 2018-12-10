IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AxisBank_RegisterUser]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AxisBank_RegisterUser]
GO

	-- Written By : Akansha
-- Summary : Proc to register Users with Axis Bank.
--			 Proc checks if customer is already registered or not.
--			 For new reigstered customer @RegistrationStatus is 'N' and for old return 'O'. Its case sensitive.
--			 Proc also creates security key for the UserId.
-- exec AxisBank_RegisterUser 'James Bond','james@bond.com','abcd','efdg','2005-10-21 00:00:00.000','12434567890','7','','0','0'
CREATE PROCEDURE [dbo].[AxisBank_RegisterUser]
	@LoginId			Varchar(50),
	@FirstName			VARCHAR(50),
	@LastName			VARCHAR(50),
	@Email			VARCHAR(100),
	@PasswordSalt	VARCHAR(10), 
	@PasswordHash	VARCHAR(64),
	@CreatedOn	DATETIME,	
	@SecurityKey		VARCHAR(50),
	@UserId		NUMERIC OUTPUT,
	@RegistrationStatus	CHAR(1) OUTPUT,
	@PasswordExpiry		DateTime,
	@IsAdmin			Bit 
AS
	--DECLARE @UserIdExist varChar(15), @IsTempVerified bit, @IsTempFake bit
	BEGIN
		--SELECT @UserIdExist=UserId, @IsTempVerified = IsVerified
		--FROM AxisBank_Users with(nolock)
		--WHERE Email=@Email
		--print @UserIdExist;
		--IF @UserId IS NULL
			--BEGIN
				BEGIN TRANSACTION TransUser				
					INSERT INTO AxisBank_Users(LoginId, FirstName, LastName, Email, PasswordSalt, PasswordHash, CreatedOn, IsAdmin,PasswordExpiry)
					VALUES (@LoginId, @FirstName, @LastName, @Email, @PasswordSalt, @PasswordHash, @CreatedOn, @IsAdmin,@PasswordExpiry)	

					SET @UserId = SCOPE_IDENTITY()  	
					SET @RegistrationStatus = 'N'		-- Means new Registration

					-- Now insert the key to the table  CustomerSecurityKey, in the field CustomerKey
					--before that check whether the key already exist. if it does the append the customer key at the end of the key
					--SELECT Userid FROM AxisBank_UserSecurityKey WHERE UserKey = @SecurityKey
					--IF @@ROWCOUNT = 0 
						--BEGIN
							INSERT AxisBank_UserSecurityKey (UserId, UserKey) VALUES(@UserId,  @SecurityKey)
						--END
					--ELSE
					--	BEGIN
					--		SET @SecurityKey = @SecurityKey + Convert(Varchar, @UserId)
					--		INSERT AxisBank_UserSecurityKey (UserId, UserKey) VALUES(@UserId,  @SecurityKey)
					--	END

					/*********************************************************************************
						Code to verify customer details with Database. If details is verified 
						mark customer as verified
					************************************************************************************/
								
					EXEC Axisbank_UpdateUserDetails_SP
						@UserId, @FirstName, @LastName, 1, 0
					Exec AxisBank_InsertInPasswordLog @UserId,@PasswordSalt,@PasswordHash
				COMMIT TRANSACTION TransUser		
			--END
		--ELSE
		--	BEGIN
		--		IF @IsTempVerified = 1 
		--			BEGIN
		--				SET @IsVerified = 1
		--			END
		--		ELSE
		--			BEGIN
		--				SET @IsVerified = 0
		--			END
				
		--		SET @RegistrationStatus = 'O' -- Old registration
		--	END
		
	END
