IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertTempCustomerV2_15]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertTempCustomerV2_15]
GO

	-- =============================================
-- Author:		--- wasn't mentioned
-- Create date: --- wasn't mentioned
-- Description:	--- wasn't mentioned
--modified by rohan sapkal on 17-05-2015 , Fetches Oauth, SETs Oauth if null
-- =============================================
CREATE PROCEDURE [dbo].[InsertTempCustomerV2_15.3.1] 
	@Name VARCHAR(100)
	,@Email VARCHAR(100)
	,@Mobile VARCHAR(20)
	,
	--@City			VARCHAR(100),
	@CityId NUMERIC
	,@State INT
	,@Password VARCHAR(20)
	,@PasswordSalt VARCHAR(100) = NULL
	,@PasswordHash VARCHAR(100) = NULL
	,@PwdSaltHashStr VARCHAR(100) = NULL
	,@ReceiveNewsletters BIT
	,@RegistrationDateTime DATETIME
	,@SecurityKey VARCHAR(50)
	,@FbId BIGINT =NULL
	,@GplusId NVARCHAR(50) =NULL
	,@NewOAuth	VARCHAR(50)=NULL
	,@OpenIDVerified BIT = 0
	,@CustomerId NUMERIC OUTPUT
	,@RegistrationStatus CHAR(1) OUTPUT
	,@IsApproved BIT OUTPUT
	,@OAuth VARCHAR(50) OUTPUT
AS
DECLARE @IsTempVerified BIT
	,@IsTempFake BIT
	,@TempIsEmailVerified BIT
	,@TempFbId BIGINT
	,@TempGplusId NVARCHAR(50)

BEGIN
	SELECT @CustomerId = Id
		,@IsTempVerified = IsVerified
		,@IsTempFake = IsFake
		,@TempIsEmailVerified= IsEmailVerified
		,@TempFbId =FbId
		,@TempGplusId =GplusId
		,@OAuth=OAuth

	FROM Customers WITH (NOLOCK)
	WHERE Email = @Email

	IF @CustomerId IS NULL
	BEGIN
		BEGIN TRANSACTION TransCustomer

		INSERT INTO Customers (NAME,Email,Mobile,CityId,StateId,password,ReceiveNewsletters,RegistrationDateTime,IsVerified,IsFake,PwdSaltHashStr,FbId,GplusId,OAuth,IsEmailVerified)
		VALUES				(@Name,@Email,@Mobile,@CityId,@State,@password,@ReceiveNewsletters,@RegistrationDateTime,1,0,@PwdSaltHashStr,@FbId,@GplusId,@NewOAuth,@OpenIDVerified)

		SET @CustomerId = SCOPE_IDENTITY()
		SET @RegistrationStatus = 'N' -- Means new Registration
		SET @OAuth=@NewOAuth

		SELECT CustomerId
		FROM CustomerSecurityKey
		WHERE CustomerKey = @SecurityKey

		IF @@ROWCOUNT = 0
		BEGIN
			INSERT CustomerSecurityKey (
				CustomerId
				,CustomerKey
				)
			VALUES (
				@CustomerId
				,@SecurityKey
				)
	END
	ELSE
		BEGIN
			SET @SecurityKey = @SecurityKey + Convert(VARCHAR, @CustomerId)

			INSERT CustomerSecurityKey (
				CustomerId
				,CustomerKey
				)
			VALUES (
				@CustomerId
				,@SecurityKey
				)
		END

		COMMIT TRANSACTION TransCustomer
	END
ELSE
	BEGIN
			IF @IsTempVerified = 1
				AND @IsTempFake = 0
				BEGIN
					SET @IsApproved = 1
				END
			ELSE
				BEGIN
					SET @IsApproved = 0
				END

		IF @OAuth is NULL
		BEGIN
		UPDATE Customers SET OAuth=@NewOAuth WHERE Id=@CustomerId
		END

			
		IF @FbId IS NOT NULL AND @OpenIDVerified=1
				BEGIN
				IF @TempFbId IS NULL
					BEGIN
					UPDATE Customers SET FbId =@FbId Where Id=@CustomerId
					END
				IF @TempIsEmailVerified =0
					BEGIN
					UPDATE Customers SET IsEmailVerified=1 where Id=@CustomerId
					END
				SET @RegistrationStatus = 'S'
				END
		ELSE IF @GplusId IS NOT NULL AND @OpenIDVerified=1
				BEGIN
				IF @TempGplusId IS NULL
					BEGIN
					UPDATE Customers SET GplusId =@GplusId Where Id=@CustomerId
					END
				IF @TempIsEmailVerified =0
					BEGIN
					UPDATE Customers SET IsEmailVerified=1 where Id=@CustomerId
					END
				SET @RegistrationStatus = 'S'
				END
		ELSE
			BEGIN
			SET @RegistrationStatus = 'O'
			END
			
		END
END



/****** Object:  StoredProcedure [dbo].[GetCustomerDetailsByIdOrEmail14.12.0]    Script Date: 19-02-2015 16:23:08 ******/
SET ANSI_NULLS ON
