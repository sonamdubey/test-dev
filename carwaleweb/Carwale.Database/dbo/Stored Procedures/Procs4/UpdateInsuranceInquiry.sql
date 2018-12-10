IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[UpdateInsuranceInquiry]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[UpdateInsuranceInquiry]
GO

	
--THIS PROCEDURE IS FOR INSERTING AND UPDATING RECORDS FOR SellInquiries TABLE

CREATE PROCEDURE [dbo].[UpdateInsuranceInquiry]
	@Id			NUMERIC,	-- Id
	@CarVersionId		NUMERIC,	-- Car Version Id
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
	@Comments		VARCHAR(2000)

 AS
	BEGIN
		UPDATE InsuranceInquiries SET
			CarVersionId		= @CarVersionId, 
			RegistrationNo		= @RegNo, 
			MakeYear		= @MakeYear, 
			Kilometers		= @Kms,
			Color			= @Color, 
			EngineNo		= @EngineNo, 
			ChassisNo		= @ChassisNo, 
			InsCompanyId		= @InsCompanyId, 
			InsType			= @InsType, 
			InsFrom			= @InsFrom, 
			InsTo			= @InsTo, 
			HypothecatedTo	= @HypothecatedTo,
			Premium		= @Premium, 
			SumInsured		= @SumInsured, 
			Comments		= @Comments
		WHERE
			ID = @ID
	END
