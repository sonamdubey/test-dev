IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_UpdateCustomerOtherDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_UpdateCustomerOtherDetails]
GO

	CREATE PROCEDURE [dbo].[CRM_UpdateCustomerOtherDetails]

	@CustomerId			Numeric,
	@DOB				VarChar(50),
	@Sex				VarChar(50),
	@MaritalStatus		VarChar(50),
	@Qualification		VarChar(100),
	@ResAddress			VarChar(500),
	@Fax				VarChar(50),
	@PinCode			VarChar(50),
	@ResStatus			VarChar(100),
	@YearsAtRes			VarChar(50),
	@MonthsAtRes		VarChar(50),
	@NoOfDependents		VarChar(50),
	@GrossMonthlyIncome	VarChar(50),
	@IdType				VarChar(50),
	@IdNumber			VarChar(50),
	@OccupationType		VarChar(50),
	@CompanyName		VarChar(200),
	@CompanyType		VarChar(100),
	@NatureOfBusiness	VarChar(50),
	@Designation		VarChar(50),
	@YearsInBusiness	VarChar(50),
	@MonthsInBusiness	VarChar(50),
	@OfficeAddress		VarChar(500),
	@OfficeLandLine		VarChar(50),
	@OfficePinCode		VarChar(50),
	@OfficeCityId		Numeric,
	@DetailsUpdatedOn	DateTime,
	@UpdatedBy			Numeric
				
 AS
	
BEGIN
	IF @CustomerId <> -1
		BEGIN
			UPDATE	CRM_CustomerOtherDetails SET
					DOB					= @DOB,
					Sex					= @Sex,
					MaritalStatus		= @MaritalStatus,
					Qualification		= @Qualification,
					ResAddress			= @ResAddress,
					Fax					= @Fax,
					PinCode				= @PinCode,
					ResStatus			= @ResStatus,
					YearsAtRes			= @YearsAtRes,
					MonthsAtRes			= @MonthsAtRes,
					NoOfDependents		= @NoOfDependents,
					GrossMonthlyIncome	= @GrossMonthlyIncome,
					IdType				= @IdType,
					IdNumber			= @IdNumber,
					OccupationType		= @OccupationType,
					CompanyName			= @CompanyName,
					CompanyType			= @CompanyType,
					NatureOfBusiness	= @NatureOfBusiness,
					Designation			= @Designation,
					YearsInBusiness		= @YearsInBusiness,
					MonthsInBusiness	= @MonthsInBusiness,
					OfficeAddress		= @OfficeAddress,
					OfficeLandLine		= @OfficeLandLine,
					OfficePinCode		= @OfficePinCode,
					OfficeCityId		= @OfficeCityId,
					UpdatedOn			= @DetailsUpdatedOn,
					UpdatedBy			= @UpdatedBy

			WHERE CRM_CustomerId = @CustomerId
		END
END








