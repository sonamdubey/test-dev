IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_FetchCustomerData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_FetchCustomerData]
GO

	CREATE PROCEDURE [dbo].[CRM_FetchCustomerData]

	@CustomerId			Numeric,
	@FirstName			VarChar(200) OutPut,
	@LastName			VarChar(100) OutPut,
	@Email				VarChar(200) OutPut,
	@Landline			VarChar(50) OutPut,
	@Mobile				VarChar(15) OutPut,
	@StateId			Numeric OutPut,
	@StateName			VarChar(50) OutPut,
	@CityId				Numeric OutPut,
	@CityName			VarChar(50) OutPut,
	@Source				VarChar(200) OutPut,
	@SourceName			VarChar(200) OutPut,
	@CWCustId			Numeric OutPut,
	@Comments			VarChar(1500) OutPut,
	@AltEmail			VarChar(200) OutPut,
	@AltContactNo		VarChar(100) OutPut,
	@DOB				VarChar(50) OutPut,
	@Sex				VarChar(50) OutPut,
	@MaritalStatus		VarChar(50) OutPut,
	@Qualification		VarChar(100) OutPut,
	@ResAddress			VarChar(500) OutPut,
	@Fax				VarChar(50) OutPut,
	@PinCode			VarChar(50) OutPut,
	@ResStatus			VarChar(100) OutPut,
	@YearsAtRes			VarChar(50) OutPut,
	@MonthsAtRes		VarChar(50) OutPut,
	@NoOfDependents		VarChar(50) OutPut,
	@GrossMonthlyIncome	VarChar(50) OutPut,
	@IdType				VarChar(50) OutPut,
	@IdNumber			VarChar(50) OutPut,
	@OccupationType		VarChar(50) OutPut,
	@CompanyName		VarChar(200) OutPut,
	@CompanyType		VarChar(100) OutPut,
	@NatureOfBusiness	VarChar(50) OutPut,
	@Designation		VarChar(50) OutPut,
	@YearsInBusiness	VarChar(50) OutPut,
	@MonthsInBusiness	VarChar(50) OutPut,
	@OfficeAddress		VarChar(500) OutPut,
	@OfficeLandLine		VarChar(50) OutPut,
	@OfficePinCode		VarChar(50) OutPut,
	@OfficeStateId		Numeric OutPut,
	@OfficeStateName	VarChar(50) OutPut,
	@OfficeCityId		Numeric OutPut,
	@OfficeCityName		VarChar(50) OutPut,
	@CustomerUpdatedById	Numeric OutPut,
	@CustomerUpdatedByName	VarChar(100) OutPut,
	@DetailsUpdatedById		Numeric OutPut,
	@DetailsUpdatedByName	VarChar(100) OutPut,
	@IsVerified				Bit OutPut,
	@IsFake					Bit OutPut,
	@IsActive				Bit OutPut,
	@IsExist				Bit OutPut,
	@CustomerCreatedOn		DateTime OutPut,
	@CustomerUpdatedOn		DateTime OutPut,
	@DetailsCreatedOn		DateTime OutPut,
	@DetailsUpdatedOn		DateTime OutPut,
	@ActiveLeadDate			VarChar(50) OutPut,
	@Area					VarChar(50) = NULL OutPut,
	@BlockedDate			DateTime = NULL OutPut,
	@BlockedReasonId		Int = -1 OutPut,
	@BlockedReason			Varchar(100) = NUL OutPut
				
 AS
	
BEGIN

	SELECT	@FirstName			= FirstName,
			@LastName			= LastName,
			@Email				= Email,
			@Landline			= Landline,
			@Mobile				= Mobile,
			@StateId			= S.Id,
			@StateName			= S.Name,
			@CityId				= CC.CityId,
			@CityName			= C.Name,
			@Source				= CC.Source,
			@SourceName			= CASE CC.CWCustId WHEN '-1' THEN LA.Organization ELSE 'CarWale' END,
			@CWCustId			= CWCustId,
			@Comments			= Comments,
			@AltEmail			= AlternateEmail,
			@AltContactNo		= AlternateContactNo,
			@DOB				= DOB,
			@Sex				= Sex,
			@MaritalStatus		= MaritalStatus,
			@Qualification		= Qualification,
			@ResAddress			= ResAddress,
			@Fax				= Fax,
			@PinCode			= CCO.PinCode,
			@ResStatus			= ResStatus,
			@YearsAtRes			= YearsAtRes,
			@MonthsAtRes		= MonthsAtRes,
			@NoOfDependents		= NoOfDependents,
			@GrossMonthlyIncome	= GrossMonthlyIncome,
			@IdType				= IdType,
			@IdNumber			= IdNumber,
			@OccupationType		= OccupationType,
			@CompanyName		= CompanyName,
			@CompanyType		= CompanyType,
			@NatureOfBusiness	= NatureOfBusiness,
			@Designation		= CCO.Designation,
			@YearsInBusiness	= YearsInBusiness,
			@MonthsInBusiness	= MonthsInBusiness,
			@OfficeAddress		= OfficeAddress,
			@OfficeLandLine		= OfficeLandLine,
			@OfficePinCode		= OfficePinCode,
			@OfficeStateId		= OS.Id,
			@OfficeStateName	= OS.Name,
			@OfficeCityId		= OfficeCityId,
			@OfficeCityName		= OC.Name,
			@CustomerUpdatedById	= CC.UpdatedBy,
			@CustomerUpdatedByName	= OUC.UserName,
			@DetailsUpdatedById		= CCO.UpdatedBy,
			@DetailsUpdatedByName	= OUD.UserName,
			@IsVerified			= IsVerified,
			@IsFake				= IsFake,
			@IsActive			= CC.IsActive,
			@CustomerCreatedOn	= CC.CreatedOn,
			@CustomerUpdatedOn	= CC.UpdatedOn,
			@DetailsCreatedOn	= CCO.CreatedOn,
			@DetailsUpdatedOn	= CCO.UpdatedOn,
			@ActiveLeadDate		= CC.ActiveLeadDate,
			@Area               = A.Name + ' - ' + A.PinCode,
			@BlockedDate		= CASE CC.BlockDate WHEN NULL THEN CC.BlockDate ELSE CC.BlockDate - 1 END,
			@BlockedReasonId	= cc.BlockReason,
			@BlockedReason		= ISNULL(ET.Name, '') + ' - ' + ISNULL(DISP.Name, '')

	FROM ((((((((((CRM_Customers AS CC WITH (NOLOCK) LEFT JOIN CRM_CustomerOtherDetails AS CCO WITH (NOLOCK) ON CC.Id = CCO.CRM_CustomerId) 
			LEFT JOIN OprUsers AS OUC WITH (NOLOCK) ON OUC.Id = CC.UpdatedBy)
			LEFT JOIN OprUsers AS OUD WITH (NOLOCK) ON OUD.Id = CCO.UpdatedBy)
			LEFT JOIN Cities AS C WITH (NOLOCK) ON CC.CityId = C.Id)
			LEFT JOIN States AS S WITH (NOLOCK) ON C.StateId = S.Id)
			LEFT JOIN Cities AS OC WITH (NOLOCK) ON CCO.OfficeCityId = OC.Id)
			LEFT JOIN States AS OS WITH (NOLOCK) ON OC.StateId = OS.Id)
			LEFT JOIN LA_Agencies AS LA WITH (NOLOCK)ON CC.Source = CAST(LA.Id AS VarChar))
			LEFT JOIN Areas AS A WITH (NOLOCK) ON A.Id = CC.AreaId)
			LEFT JOIN CRM_ETDispositions DISP WITH (NOLOCK) ON CC.BlockReason = DISP.Id
			LEFT JOIN CRM_EventTypes ET WITH (NOLOCK) ON DISP.EventType = ET.Id)

	WHERE CC.Id = @CustomerId
	
	IF @@ROWCOUNT = 0
		BEGIN
			SET @IsExist = 0
		END
	ELSE
		BEGIN
			SET @IsExist = 1
		END
END











