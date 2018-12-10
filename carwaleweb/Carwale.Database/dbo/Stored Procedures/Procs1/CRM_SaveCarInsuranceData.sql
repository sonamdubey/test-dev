IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_SaveCarInsuranceData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_SaveCarInsuranceData]
GO

	CREATE PROCEDURE [dbo].[CRM_SaveCarInsuranceData]
	
	@CarInsuranceId			Numeric,
	@CarBasicDataId			Numeric,
	@InsAgencyId			Numeric,
	@InsCoverLetterNumber	VarChar(50),
	@InsComments			VarChar(1000),
	@UpdatedById			Numeric,
	
	@IsInsCoverLetterIssued	Bit,
	@IsInsPaymentCollected	Bit,
	@IsInsPaymentRealised	Bit,

	@InsStartDate			DateTime,
	@CreatedOn				DateTime,
	@UpdatedOn				DateTime,
	@currentId				Numeric OutPut
				
 AS
	
BEGIN
	SET @currentId = -1
	
	UPDATE CRM_CarInsuranceData
	SET InsAgencyId = @InsAgencyId, InsCoverLetterNumber = @InsCoverLetterNumber,
		InsComments = @InsComments, UpdatedBy = @UpdatedById, 
		IsInsCoverLetterIssued = @IsInsCoverLetterIssued, IsInsPaymentCollected = @IsInsPaymentCollected, 
		@IsInsPaymentRealised = @IsInsPaymentRealised, UpdatedOn = @UpdatedOn, InsStartDate = @InsStartDate
	WHERE CarBasicDataId = @CarBasicDataId
	
	IF @@ROWCOUNT = 0
		BEGIN

			INSERT INTO CRM_CarInsuranceData
			(
				CarBasicDataId, InsAgencyId, InsCoverLetterNumber,
				InsComments, UpdatedBy, IsInsCoverLetterIssued, 
				IsInsPaymentCollected, IsInsPaymentRealised,
				InsStartDate, CreatedOn, UpdatedOn
			)
			VALUES
			(
				@CarBasicDataId, @InsAgencyId, @InsCoverLetterNumber,
				@InsComments, @UpdatedById, @IsInsCoverLetterIssued,
				@IsInsPaymentCollected, @IsInsPaymentRealised,
				@InsStartDate, @CreatedOn, @UpdatedOn
			)
			
			SET @currentId = Scope_Identity()

		END

	ELSE

		BEGIN
			SET @currentId = @CarInsuranceId
		END
END