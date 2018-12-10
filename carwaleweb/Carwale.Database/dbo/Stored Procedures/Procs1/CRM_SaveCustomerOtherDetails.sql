IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_SaveCustomerOtherDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_SaveCustomerOtherDetails]
GO

	CREATE PROCEDURE [dbo].[CRM_SaveCustomerOtherDetails]

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
	@DetailsCreatedOn	DateTime,
	@DetailsUpdatedOn	DateTime,
	@UpdatedBy			Numeric
				
 AS
	
BEGIN
	IF @CustomerId <> -1
		BEGIN
			INSERT INTO CRM_CustomerOtherDetails
				( 
				  CRM_CustomerId, DOB, Sex, MaritalStatus, Qualification, ResAddress, Fax,
				  PinCode, ResStatus, YearsAtRes, MonthsAtRes, NoOfDependents, GrossMonthlyIncome,
				  IdType, IdNumber, OccupationType, CompanyName, CompanyType, NatureOfBusiness,
				  Designation, YearsInBusiness, MonthsInBusiness, OfficeAddress, OfficeLandLine,
				  OfficePinCode, OfficeCityId, CreatedOn, UpdatedOn, UpdatedBy
				)
			VALUES
				(
				  @CustomerId, @DOB, @Sex, @MaritalStatus, @Qualification, @ResAddress, @Fax,
				  @PinCode, @ResStatus, @YearsAtRes, @MonthsAtRes, @NoOfDependents, @GrossMonthlyIncome,
				  @IdType, @IdNumber, @OccupationType, @CompanyName, @CompanyType, @NatureOfBusiness,
				  @Designation, @YearsInBusiness, @MonthsInBusiness, @OfficeAddress, @OfficeLandLine,
				  @OfficePinCode, @OfficeCityId, @DetailsCreatedOn, @DetailsUpdatedOn, @UpdatedBy
				)
		END
END







