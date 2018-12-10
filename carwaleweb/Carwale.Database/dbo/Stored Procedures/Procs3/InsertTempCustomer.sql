IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertTempCustomer]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertTempCustomer]
GO

	CREATE PROCEDURE [dbo].[InsertTempCustomer]
	@Name			VARCHAR(100),	-- Car RegistrationNo 
	@Email			VARCHAR(100),	-- Car Status Id
	@Phone1			VARCHAR(20),	-- Car Price
	@Phone2			VARCHAR(20),	-- Car Make-Year
	@Mobile			VARCHAR(20),	-- Mileage, Kilometers Done
	@PrimaryPhone	SMALLINT,	-- Car Color  
	@Address		VARCHAR(2000),	-- Dealer Comments
	@Area			VARCHAR(100),
	@City			VARCHAR(100),
	@CityId			NUMERIC,
	@State			INT,
	@PinCode		NUMERIC,
	@Password		VARCHAR(20),
	@Industry		VARCHAR(100),	
	@Designation		VARCHAR(100),
	@Organization		VARCHAR(100),
	@DOB			DATETIME,
	@ContactHours		VARCHAR(50),
	@ContactMode		VARCHAR(10),
	@CurrentVehicle	VARCHAR(200),
	@InternetUsePlace	VARCHAR(20),
	@CarwaleContact	VARCHAR(200),
	@InternetUseTime	INT,	
	@ReceiveNewsletters	BIT,
	@RegistrationDateTime	DATETIME,	
	@SecurityKey		VARCHAR(50),
	@CustomerId		NUMERIC OUTPUT,
	@RegistrationStatus	CHAR(1) OUTPUT,
	@IsApproved			Bit OUTPUT
AS
	DECLARE @IsTempVerified bit, @IsTempFake bit
	BEGIN
		
		SELECT @CustomerId=Id, @IsTempVerified = IsVerified, @IsTempFake = IsFake 
		FROM Customers with(nolock)
		WHERE Email=@Email
		
		PRINT @CustomerId

		IF @CustomerId IS NULL
			BEGIN
			BEGIN TRANSACTION TransCustomer
			INSERT INTO TempCustomers(Name, Email, Phone1, Phone2, Mobile, PrimaryPhone, Address,City,
					Area, State, PinCode, Password, Industry, Organization, Designation, DOB, ContactHours,
					ContactMode, CurrentVehicle, CarwaleContact, InternetUseTime, 
					ReceiveNewsletters, RegistrationDateTime)
			VALUES(@Name, @Email, @Phone1, @Phone2, @Mobile, @PrimaryPhone, @Address,@City,	
					@Area, @State, @PinCode, @Password, @Industry, @Organization, @Designation, @DOB, @ContactHours,
					@ContactMode,@CurrentVehicle, @CarwaleContact, @InternetUseTime, 
					@ReceiveNewsletters, @RegistrationDateTime)

			SET @CustomerId = SCOPE_IDENTITY()  							

			INSERT INTO Customers(Name, Email,  password, StateId, CityId, RegistrationDateTime, TempId, phone1, phone2, Mobile)
			VALUES (@Name, @Email,  @password, @State, @CityId, @RegistrationDateTime, @customerId, @Phone1, @Phone2, @Mobile)	

			SET @CustomerId = SCOPE_IDENTITY()  	
			SET @RegistrationStatus = 'N'		-- Means new Registration

			-- Now insert the key to the table  CustomerSecurityKey, in the field CustomerKey
			--before that check whether the key already exist. if it does the append the customer key at the end of the key
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

			/*********************************************************************************
				Code to verify customer details with Database. If details is verified 
				mark customer as verified
			************************************************************************************/
			Set @IsApproved = 1
			
			/*
			If ( @Name != '' AND @Email != '')
				BEGIN
					
					Declare @fName Varchar(50), @DomainName Varchar(50), @MobileCode Numeric, @UpdateDb Bit, 
							@blankIndex Int, @emailIndex Int, @IsNameVerified Bit, 	
							@IsEmailVerified Bit, @IsMobileVerified Bit		
		
					Set @blankIndex = CHARINDEX(' ', @Name)
					Set @emailIndex = CHARINDEX('@', @Email)
					Set @UpdateDb = 0

					If @blankIndex > 0
						BEGIN
							Set @fName = Substring(@Name, 1, @blankIndex - 1 )
						END
					Else Set @fName = @Name
					
					-- GET THE EMAIL DOMAIN NAME
					Set @DomainName = Substring(@Email, @emailIndex + 1, Len(@Email) - @emailIndex + 1)

					IF @Mobile != ''
					BEGIN
						-- GET FIRST FOUR CHAR OF MOBILE LIKE 9766
						Set @MobileCode = Substring(@Mobile, 1, 4)
					END
					ELSE
					BEGIN
						SET @MobileCode = 9999		--default set to delhi's number
					END
					
					-- SP to verify customer details
					EXEC Db_CustomerVerification @fName, @DomainName, @MobileCode, @UpdateDb, @IsNameVerified Output, @IsEmailVerified Output, @IsMobileVerified Output
					
					-- IF CUSTOMER IS VERIFIED, MARK IsApproved = 1 And IsFake = 0
					If( @IsNameVerified = 1 AND @IsEmailVerified = 1 AND @IsMobileVerified = 1 )
						BEGIN
							Set @IsApproved = 1
						END	
					Else
						BEGIN
							Set @IsApproved = 0
						END	
				END
				
			*/
			/************************************************************************************/
			/************************************************************************************/			
			--Set
			--update the customer details and make it verified
			
			EXEC UpdateCustomerDetails
				@CustomerId, @Name, @Email, @Address, @State, @CityId, -1, @Phone1, @Phone2, @Mobile, @PrimaryPhone, @Industry, @Designation, @Organization, 
				@DOB, @ContactHours, @ContactMode, '', '', @CarwaleContact, @InternetUseTime, @ReceiveNewsletters, @IsApproved, 0, ''

			COMMIT TRANSACTION TransCustomer		
			END
		ELSE
			BEGIN
				IF @IsTempVerified = 1 AND @IsTempFake = 0
				BEGIN
					SET @IsApproved = 1
				END
				ELSE
				BEGIN
					SET @IsApproved = 0
				END
				
				SET @RegistrationStatus = 'O' -- Old registration
			END
	END







