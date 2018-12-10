IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertFinanceInquiry]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertFinanceInquiry]
GO

	
--THIS PROCEDURE IS FOR INSERTING AND UPDATING RECORDS FOR SellInquiries TABLE

CREATE PROCEDURE [dbo].[InsertFinanceInquiry]
	@CustomerId		NUMERIC,	-- Car RegistrationNo 
	@RequestDateTime	DATETIME,	-- Entry Date
	@DOB			DATETIME,
	@ResidenceDuration	INT,
	@Employer		VARCHAR(50),	
	@JobTitle		VARCHAR(50),
	@Income		VARCHAR(50),
	@OfficePhone		VARCHAR(20),
	@OfficeExt		VARCHAR(5),
	@ServiceDuration	INT,
	@PanCardNo		VARCHAR(50),
	@Comments		VARCHAR(2000),
	@IsApproved		BIT	

 AS
	BEGIN
		INSERT INTO FinanceInquiries( CustomerId, DOB,LengthResidence, Employer,
			JobTitle, MonthlyIncome, OfficePhone, PhoneExtension, LengthService,
			PanCardNo, Comments, RequestDateTime,IsApproved) 
			VALUES(@CustomerId, @DOB, @ResidenceDuration, @Employer,
			@JobTitle, @Income, @OfficePhone, @OfficeExt, @ServiceDuration, 
			@PanCardNo, @Comments, @RequestDateTime,@IsApproved )

	END
