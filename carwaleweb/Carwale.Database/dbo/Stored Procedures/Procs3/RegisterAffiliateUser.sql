IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[RegisterAffiliateUser]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[RegisterAffiliateUser]
GO

	
CREATE PROCEDURE [dbo].[RegisterAffiliateUser]
	@Id			NUMERIC, -- 1 For Insertion & -1 for Updation
	@LoginId		VARCHAR(100),	
	@Passwd		VARCHAR(50),	
	@ContactPerson	VARCHAR(100),	
	@ContactEmail		VARCHAR(100),	
	@ContactNumber	VARCHAR(100),	
	
	@FaxNumber		VARCHAR(100),
	@ChequePayableTo	VARCHAR(200),
	@CompanyName	VARCHAR(200),
	@Address1		VARCHAR(150),
	@Address2		VARCHAR(150),
	@CityId			NUMERIC,
	@PinNo		VARCHAR(10),
	@CustomerId		NUMERIC OUTPUT, -- To Check Duplicacy
	@RegistrationStatus	CHAR(1) OUTPUT

AS
	IF @Id = -1
		BEGIN
			SELECT @CustomerId = Id FROM AffiliateMembers WHERE ContactEmail=@ContactEmail
			
			IF @CustomerId IS NULL
				BEGIN
				
					INSERT INTO AffiliateMembers ( LoginId, Passwd, ContactPerson, ContactEmail, ContactNumber,
						FaxNumber, ChequePayableTo, CompanyName, Address1, Address2, CityId, 
						PinNo )
					VALUES ( @LoginId, @Passwd, @ContactPerson, @ContactEmail, @ContactNumber,
						@FaxNumber, @ChequePayableTo, @CompanyName, @Address1, @Address2, @CityId, 
						@PinNo )

					SET @CustomerId = SCOPE_IDENTITY()
					SET @RegistrationStatus = 'N'		-- Means Nwe Registration
				END
	
			ELSE
				BEGIN
					SET @RegistrationStatus = 'O' -- Old registration
				END
			
		END
	ELSE
		BEGIN
				
			UPDATE  AffiliateMembers  
			SET 
				ContactPerson = @ContactPerson,  ContactNumber = @ContactNumber,
				FaxNumber = @FaxNumber, ChequePayableTo = @ChequePayableTo, CompanyName = @CompanyName,
				 Address1 = @Address1,  Address2 = @Address2, CityId = @CityId, PinNo = @PinNo
			WHERE
				ID = @Id
			
			SET @RegistrationStatus = 'U'	-- Successfully Updated

		END
