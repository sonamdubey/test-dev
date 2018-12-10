IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TempBankCarLoan_Sp]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TempBankCarLoan_Sp]
GO

	-- =============================================
-- Author:		Satish
-- Create date: 26 Sep 08
-- Description:	n/a
-- =============================================
CREATE PROCEDURE [dbo].[TempBankCarLoan_Sp]
	-- Add the parameters for the stored procedure here
	@FirstName Varchar(50), @MiddleName Varchar(50), @LastName Varchar(50),
	@DOB DateTime, @Sex Varchar(50), @MaritialStatus Varchar(50), 
	@Qualification Varchar(50), @ResPhoneNo Varchar(50), @MobileNo Varchar(50),
	@ResAddress Varchar(200), @ResCityId Varchar(50),@ResPincode Varchar(50),
	@ResStatus Varchar(50), @ResYear Varchar(50), @ResMonth Varchar(50),
	@EMail Varchar(100), @IdType Varchar(50), @OtherIdName Varchar(50), 
	@IdNumber Varchar(50), @NoOfDependents Varchar(50), @GrossMonthlyIncome Numeric,
	@OccupationType Varchar(50),@CompanyName Varchar(100), @CompanyType Varchar(50),
	@BusinessNature Varchar(50), @Designation Varchar(50), @BusinessYear Varchar(50),
	@BusinessMonth Varchar(50), @OfficeAddress Varchar(200), @OfficeCityId Numeric,
	@OfficePinCode Varchar(50), @OfficePhoneNo Varchar(50), @DelCity Numeric,
	@CarVersion Numeric, @RegType Varchar(50), @CarPrice Numeric,
	@LoanAmount Numeric, @TenureInMonths Numeric, @LoanId NUMERIC OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO TempBankCarLoan
	(
		FirstName, MiddleName, LastName, 
		DOB, Sex, MaritialStatus, 
		Qualification, ResPhoneNo, MobileNo,
		ResAddress, ResCityId, ResPincode,
		ResStatus, ResYear, ResMonth,
		EMail, IdType, OtherIdName,
		IdNumber, NoOfDependents, GrossMonthlyIncome, 
		OccupationType, CompanyName, CompanyType,
		BusinessNature, Designation, BusinessYear,
		BusinessMonth, OfficeAddress, OfficeCityId,
		OfficePinCode, OfficePhoneNo, DelCity,
		CarVersion, RegType, CarPrice,
		LoanAmount, TenureInMonths, EntryDate
	)
	VALUES
	(	
		@FirstName, @MiddleName, @LastName, 
		@DOB, @Sex, @MaritialStatus, 
		@Qualification, @ResPhoneNo, @MobileNo,
		@ResAddress, @ResCityId, @ResPincode,
		@ResStatus, @ResYear, @ResMonth,
		@EMail, @IdType, @OtherIdName,
		@IdNumber, @NoOfDependents, @GrossMonthlyIncome, 
		@OccupationType, @CompanyName, @CompanyType,
		@BusinessNature, @Designation, @BusinessYear,
		@BusinessMonth, @OfficeAddress, @OfficeCityId,
		@OfficePinCode, @OfficePhoneNo, @DelCity,
		@CarVersion, @RegType, @CarPrice,
		@LoanAmount, @TenureInMonths, GetDate()
	)

	SET @LoanId = Scope_Identity()
END




