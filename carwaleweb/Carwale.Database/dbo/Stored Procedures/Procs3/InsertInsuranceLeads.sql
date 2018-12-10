IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertInsuranceLeads]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertInsuranceLeads]
GO

	--THIS PROCEDURE IS FOR INSERTING into insuranceleads

CREATE PROCEDURE [dbo].[InsertInsuranceLeads]
	@VersionId		NUMERIC,	-- Car Version Id
	@CustomerId		NUMERIC,	-- Car RegistrationNo 
	@UsedCarProfileId	VARCHAR(50),	-- Car RegistrationNo 
	@IsUsed		BIT,
	@FirstName		VARCHAR(50),
	@LastName		VARCHAR(50),
	@Email			VARCHAR(100),
	@Phone1		VARCHAR(50), 
	@Mobile		VARCHAR(50), 
	@CityId			NUMERIC, 
	@Comments		VARCHAR(1000), 
	@EntryDate		DATETIME,	-- Entry Date
	@isApproved		BIT,
	@SourceId		SMALLINT,
	@DOB			DATETIME,
	@IsRenewal		BIT,
	@ExpiryDate		DATETIME,
	@InquiryId		NUMERIC OUTPUT

 AS
	BEGIN
		INSERT INTO InsuranceLeads
			(
				VersionId, 		CustomerId, 		UsedCarProfileId, 	isUsed, 
				FirstName, 		LastName, 		Email, 			Phone1, 
				Mobile, 			CityId, 			Comments, 		EntryDate, 
				isApproved,		SourceId,		DOB,			IsRenewal, 	ExpiryDate
			)
		VALUES
			(
				@VersionId, 		@CustomerId, 		@UsedCarProfileId, 	@isUsed, 
				@FirstName, 		@LastName, 		@Email, 		@Phone1, 
				@Mobile, 		@CityId, 		@Comments, 		@EntryDate, 
				@isApproved,		@SourceId,		@DOB,			@IsRenewal, 	@ExpiryDate
			)
		
		SET @InquiryId = SCOPE_IDENTITY()
	END