IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertInsuranceInquiry]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertInsuranceInquiry]
GO
	
--THIS PROCEDURE IS FOR INSERTING AND UPDATING RECORDS FOR SellInquiries TABLE

CREATE PROCEDURE [dbo].[InsertInsuranceInquiry]
	@CarVersionId		NUMERIC,	-- Car Version Id
	@CustomerId		NUMERIC,
	@RequestDateTime	DATETIME,	-- Entry Date
	@MakeYear		DATETIME,
	@RegNo		VARCHAR(50),
	@Kms			NUMERIC,
	@Color			VARCHAR(50),	
	@EngineNo		VARCHAR(50),
	@ChassisNo		VARCHAR(50),
	@InsCompanyId		INT,
	@InsType		VARCHAR(50),
	@InsFrom		DATETIME,
	@InsTo			DATETIME,
	@HypothecatedTo	VARCHAR(50),
	@Premium		NUMERIC,
	@SumInsured		NUMERIC,	
	@Comments		VARCHAR(2000),
	@IsApproved		BIT

 AS
	BEGIN
		INSERT INTO InsuranceInquiries( CustomerId, CarVersionId, RegistrationNo, 
			MakeYear, Kilometers,Color, EngineNo, ChassisNo, 
			InsCompanyId, InsType, InsFrom, InsTo, HypothecatedTo,
			Premium, SumInsured, Comments, RequestDateTime,IsApproved) 
			VALUES(@CustomerId, @CarVersionId, @RegNo,  
			@MakeYear, @Kms, @Color, @EngineNo, @ChassisNo, 
			@InsCompanyId, @InsType, @InsFrom, @InsTo, @HypothecatedTo,
			@Premium, @SumInsured, @Comments, @RequestDateTime,@IsApproved )

	END
