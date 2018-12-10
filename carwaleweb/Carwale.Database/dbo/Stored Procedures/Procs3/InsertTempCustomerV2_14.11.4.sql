IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertTempCustomerV2_14]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertTempCustomerV2_14]
GO

	
CREATE PROCEDURE [dbo].[InsertTempCustomerV2_14.11.4] 
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
	,@OpenIDVerified BIT = 0
	,@CustomerId NUMERIC OUTPUT
	,@RegistrationStatus CHAR(1) OUTPUT
	,@IsApproved BIT OUTPUT

AS
DECLARE @IsTempVerified BIT
	,@IsTempFake BIT
	,@TempFbId BIGINT
	,@TempGplusId NVARCHAR(50)

BEGIN
	SELECT @CustomerId = Id
		,@IsTempVerified = IsVerified
		,@IsTempFake = IsFake
		,@TempFbId =FbId
		,@TempGplusId =GplusId

	FROM Customers WITH (NOLOCK)
	WHERE Email = @Email

	IF @CustomerId IS NULL
	BEGIN
		BEGIN TRANSACTION TransCustomer

		INSERT INTO Customers (NAME,Email,Mobile,CityId,StateId,password,ReceiveNewsletters,RegistrationDateTime,IsVerified,IsFake,PwdSaltHashStr,FbId,GplusId)
		VALUES				(@Name,@Email,@Mobile,@CityId,@State,@password,@ReceiveNewsletters,@RegistrationDateTime,1,0,@PwdSaltHashStr,@FbId,@GplusId)

		SET @CustomerId = SCOPE_IDENTITY()
		SET @RegistrationStatus = 'N' -- Means new Registration

		SELECT CustomerId
		FROM CustomerSecurityKey  WITH(NOLOCK)
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

			
		IF @FbId IS NOT NULL AND @OpenIDVerified=1
				BEGIN
				IF @TempFbId IS NULL
					BEGIN
					UPDATE Customers SET FbId =@FbId Where Id=@CustomerId
					END
				SET @RegistrationStatus = 'S'
				END
		ELSE IF @GplusId IS NOT NULL AND @OpenIDVerified=1
				BEGIN
				IF @TempGplusId IS NULL
					BEGIN
					UPDATE Customers SET GplusId =@GplusId Where Id=@CustomerId
					END
				SET @RegistrationStatus = 'S'
				END
		ELSE
			BEGIN
			SET @RegistrationStatus = 'O'
			END
			
		END
END

