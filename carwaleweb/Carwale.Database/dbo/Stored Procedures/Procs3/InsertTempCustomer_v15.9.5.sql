IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertTempCustomer_v15]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertTempCustomer_v15]
GO

	CREATE PROCEDURE [dbo].[InsertTempCustomer_v15.9.5]
	@Name			VARCHAR(100),
	@Email			VARCHAR(100),
	@Mobile			VARCHAR(20),
	--@City			VARCHAR(100),
	@CityId			NUMERIC,
	@State			INT,
	@Password		VARCHAR(20),	
	@PasswordSalt		VARCHAR(100) = NULL,	
	@PasswordHash		VARCHAR(100) = NULL,	
	@PwdSaltHashStr		VARCHAR(100) = NULL,	
	@ReceiveNewsletters	BIT,
	@RegistrationDateTime	DATETIME,	
	@SecurityKey		VARCHAR(50),
	@CustomerId		NUMERIC OUTPUT,
	@RegistrationStatus	CHAR(1) OUTPUT,
	@IsApproved			Bit OUTPUT
AS
	DECLARE @IsTempVerified bit, @IsTempFake bit,@OldEmail varchar(100)
	BEGIN
		 
		SELECT @CustomerId=Id, @OldEmail=Email, @IsTempVerified = IsVerified, @IsTempFake = IsFake 
		FROM Customers WITH(NOLOCK)
		WHERE Email=@Email or email=@Mobile+'@unknown.com'
		
		IF @CustomerId IS NULL
			BEGIN
			BEGIN TRANSACTION TransCustomer
			
			INSERT INTO Customers(
				Name,
				Email,
				Mobile,
				CityId,
				StateId,
				password,
				ReceiveNewsletters,
				RegistrationDateTime,
				IsVerified,
				IsFake,
				PwdSaltHashStr)
			VALUES (@Name, @Email, @Mobile, @CityId,@State, @password, @ReceiveNewsletters, @RegistrationDateTime , 1, 0, @PwdSaltHashStr)

			SET @CustomerId = SCOPE_IDENTITY()  	
			SET @RegistrationStatus = 'N'		-- Means new Registration

			SELECT CustomerId FROM CustomerSecurityKey WHERE CustomerKey = @SecurityKey
			IF @@ROWCOUNT = 0 
				BEGIN
					INSERT CustomerSecurityKey (CustomerId, CustomerKey) VALUES(@CustomerId,  @SecurityKey)
				END
			ELSE
				BEGIN
					SET @SecurityKey = @SecurityKey + Convert(Varchar, @CustomerId)
					INSERT CustomerSecurityKey (CustomerId, CustomerKey) VALUES(@CustomerId,  @SecurityKey)
				END
			
			COMMIT TRANSACTION TransCustomer		
			END
		ELSE
			BEGIN
				IF @OldEmail = @Mobile+'@unknown.com'
				BEGIN
					Update Customers set email=@Email where Id=@CustomerId
				END
				IF @IsTempVerified = 1 AND @IsTempFake = 0
				BEGIN
					SET @IsApproved = 1
				END
				ELSE
				BEGIN
					SET @IsApproved = 0
				END				
				SET @RegistrationStatus = 'O'
			END
	END








