IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertTelephonyCustomer]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertTelephonyCustomer]
GO

	
--THIS PROCEDURE IS FOR INSERTING AND UPDATING RECORDS FOR SellInquiries TABLE

CREATE PROCEDURE [dbo].[InsertTelephonyCustomer]
	@Name			VARCHAR(100),	-- Car RegistrationNo 
	@Email			VARCHAR(100),	-- Car Status Id
	@Phone1		VARCHAR(20),	-- Car Price
	@Phone2		VARCHAR(20),	-- Car Make-Year
	@Mobile		VARCHAR(20),	-- Mileage, Kilometers Done
	@PrimaryPhone		SMALLINT,	-- Car Color  
	@Address		VARCHAR(2000),	-- Dealer Comments
	@Area			VARCHAR(100),
	@OtherArea		VARCHAR(100),
	@City			VARCHAR(100),
	@State			INT,
	@Password		VARCHAR(20),
	@Industry		VARCHAR(100),	
	@Designation		VARCHAR(100),
	@Organization		VARCHAR(100),
	@DOB			DATETIME,
	@ContactHours		VARCHAR(50),
	@CurrentVehicle	VARCHAR(200),
	@CarwaleContact	VARCHAR(200),
	@RegistrationDateTime	DATETIME,	
	@SecurityKey		VARCHAR(50),
	@CustomerId		NUMERIC OUTPUT,
	@RegistrationStatus	CHAR(1) OUTPUT

AS
	BEGIN
		SELECT @CustomerId=Id FROM Customers WHERE Email=@Email
		
		IF @CustomerId IS NULL
			BEGIN
			IF @OtherArea <> ''
				BEGIN
					INSERT INTO TempCustomers(Name, Email, Phone1, Phone2, Mobile, PrimaryPhone, Address,City,
					Area, State, Password, Industry, Organization, Designation, DOB, ContactHours,
					ContactMode, CurrentVehicle, CarwaleContact,  RegistrationDateTime)
					VALUES(@Name, @Email, @Phone1, @Phone2, @Mobile, @PrimaryPhone, @Address,@City,	
					@OtherArea, @State, @Password, @Industry, @Organization, @Designation, @DOB, @ContactHours,
					'Phone',@CurrentVehicle, @CarwaleContact,@RegistrationDateTime)
					
					SET @CustomerId =  SCOPE_IDENTITY()  	
					
					INSERT INTO Customers(Name, Email, Phone1, Phone2, Mobile, PrimaryPhone, Address,CityId,
					AreaId, StateId, Password, Industry, Organization, Designation, DOB, ContactHours,
					ContactMode, CurrentVehicle, CarwaleContact,  RegistrationDateTime, TempId)
					VALUES(@Name, @Email, @Phone1, @Phone2, @Mobile, @PrimaryPhone, @Address,@City,	
					@Area, @State, @Password, @Industry, @Organization, @Designation, @DOB, @ContactHours,
					'Phone',@CurrentVehicle, @CarwaleContact,@RegistrationDateTime,@CustomerId)	
				END
			ELSE
			BEGIN	
				INSERT INTO Customers(Name, Email, Phone1, Phone2, Mobile, PrimaryPhone, Address,CityId,
					AreaId, StateId, Password, Industry, Organization, Designation, DOB, ContactHours,
					ContactMode, CurrentVehicle, CarwaleContact,  RegistrationDateTime, isVerified)
				VALUES(@Name, @Email, @Phone1, @Phone2, @Mobile, @PrimaryPhone, @Address,@City,	
					@Area, @State, @Password, @Industry, @Organization, @Designation, @DOB, @ContactHours,
					'Phone',@CurrentVehicle, @CarwaleContact,@RegistrationDateTime,1)
			END

			SET @CustomerId = SCOPE_IDENTITY()  							

			SET @RegistrationStatus = 'N'		-- Means Nwe Registration

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

			END
		ELSE
			BEGIN

			SET @RegistrationStatus = 'O' -- Old registration
			END
		
	END
