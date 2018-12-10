IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CW_SaveFinanceLeads]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CW_SaveFinanceLeads]
GO

	 -- =============================================
-- Author:		Vaibhav Kale
-- Create date: 1-Aug-2015
-- Description:	To save or update CarWale finance lead as per tie up with Banks
-- modified by : Piyus on 9/2/2016  Added clientid and platformid to save leads from all clients
-- =============================================
CREATE PROCEDURE [dbo].[CW_SaveFinanceLeads]
	-- Add the parameters for the stored procedure here
	@FinanceLeadId			NUMERIC OUTPUT,	
	@First_Name				VARCHAR(100) = NULL,
	@Last_Name				VARCHAR(100) = NULL,
	@Email					VARCHAR(100) = NULL,
	@Pan_No					VARCHAR(20) = NULL,
	@Res_address			VARCHAR(500) = NULL,
	@Res_address2			VARCHAR(500) = NULL,
	@Res_address3			VARCHAR(500) = NULL,
	@Resi_type				VARCHAR(200) = NULL,
	@CityId					INT = NULL,
	@Mobile					VARCHAR(15) = NULL,
	@LeadClickSource		INT = NULL,
	@Res_City				VARCHAR(50) = NULL,
	@Resi_City_other		VARCHAR(50) = NULL,
	@Resi_City_other1		VARCHAR(50) = NULL,
	@Res_Pin				VARCHAR(10) = NULL,
	@Company_Name			VARCHAR(200) = NULL,
	@DateOfBirth			DATETIME = NULL,
	@Designation			VARCHAR(50) = NULL,
	@Emp_Type				VARCHAR(50) = NULL,
	@Monthly_Income			INT = NULL,
	@Card_Held				VARCHAR(50) = NULL,
	@Source_Code			VARCHAR(50) = NULL,
	@Promo_Code				VARCHAR(50) = NULL,
	@Lead_Date_Time			DATETIME = NULL,
	@Product_Applied_For	VARCHAR(50) = NULL,
	@ExistingCust			VARCHAR(50) = NULL,
	@YearsInEmp				INT = NULL,
	@Emi_Paid				VARCHAR(20) = NULL,
	@Car_Make				VARCHAR(50) = NULL,
	@Car_Model				VARCHAR(50) = NULL,
	@TypeOfLoan				VARCHAR(50) = NULL,
	@IP_Address				VARCHAR(50) = NULL,
	@Indigo_UniqueKey		VARCHAR(50) = NULL,
	@Indigo_RequestFromYesNo	VARCHAR(10) = NULL,
	@VersionId				INT = NULL,
	@LoanAmount				FLOAT = NULL,
	@ROI					FLOAT = NULL,
	@LTV					FLOAT = NULL,
	@CarPrice				INT = NULL,
	@IsPermitted			BIT = NULL,
	@IsPushSuccess			BIT = NULL,
	@BankTypeId				SMALLINT = NULL,
	@ApiResponse			VARCHAR(100) = NULL,
	@BuyingPeriod			VARCHAR(50) = NULL,
	@IncomeTypeId			INT = NULL,
	@YearsInRes				INT = NULL,
	@FailureReason          VARCHAR(150)=NULL,
	@ClientId				SMALLINT = null, --added by piyush 9 feb 2016
	@PlatformId				SMALLINT = null  --added by piyush 9 feb 2016
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	IF @FinanceLeadId IS NULL OR @FinanceLeadId = -1
	BEGIN
		INSERT INTO CW_FinanceLeads
			(
				First_Name,Last_Name,Email,Pan_No,Res_address,Res_address2,Res_address3,Resi_type,CityId,Mobile,LeadClickSource,Res_City,Resi_City_other,Resi_City_other1,Res_Pin,
				Company_Name,DateOfBirth,Designation,Emp_Type,Monthly_Income,Card_Held,Source_Code,Promo_Code,Lead_Date_Time,Product_Applied_For,ExistingCust,
				YearsInEmp,Emi_Paid,Car_Make,Car_Model,TypeOfLoan,IP_Address,Indigo_UniqueKey,Indigo_RequestFromYesNo,VersionId,
				LoanAmount,ROI,LTV,CarPrice,IsPermitted,IsPushSuccess,BankTypeId,ApiResponse,BuyingPeriod,IncomeTypeId,YearsInRes,FailureReason,ClientId,PlatformId
			)
		VALUES
			(
				@First_Name,@Last_Name,@Email,@Pan_No,@Res_address,@Res_address2,@Res_address3,@Resi_type,@CityId,@Mobile,@LeadClickSource,@Res_City,@Resi_City_other,@Resi_City_other1,@Res_Pin,
				@Company_Name,@DateOfBirth,@Designation,@Emp_Type,@Monthly_Income,@Card_Held,@Source_Code,@Promo_Code,@Lead_Date_Time,@Product_Applied_For,@ExistingCust,
				@YearsInEmp,@Emi_Paid,@Car_Make,@Car_Model,@TypeOfLoan,@IP_Address,@Indigo_UniqueKey,@Indigo_RequestFromYesNo,@VersionId,
				@LoanAmount,@ROI,@LTV,@CarPrice,@IsPermitted,@IsPushSuccess,@BankTypeId,@ApiResponse,@BuyingPeriod,@IncomeTypeId,@YearsInRes,@FailureReason,@ClientId,@PlatformId
			)

		select SCOPE_IDENTITY() as FinanceLeadId
	END
	ELSE
		UPDATE CW_FinanceLeads
		SET	First_Name		= ISNULL(@First_Name, First_Name),
			Last_Name		= ISNULL(@Last_Name, Last_Name),
			Email			= ISNULL(@Email, Email),
			Pan_No			= ISNULL(@Pan_No, Pan_No),
			Res_address		= ISNULL(@Res_address, Res_address),
			Res_address2	= ISNULL(@Res_address2, Res_address2),
			Res_address3	= ISNULL(@Res_address3, Res_address3),
			Resi_type		= ISNULL(@Resi_type, Resi_type),
			CityId			= ISNULL(@CityId, CityId),
			Mobile			= ISNULL(@Mobile, Mobile),
			LeadClickSource	= ISNULL(@LeadClickSource, LeadClickSource),
			Res_City		= ISNULL(@Res_City, Res_City),
			Resi_City_other	= ISNULL(@Resi_City_other, Resi_City_other),
			Resi_City_other1	= ISNULL(@Resi_City_other1, Resi_City_other1),
			Res_Pin		= ISNULL(@Res_Pin, Res_Pin),
			Company_Name	= ISNULL(@Company_Name, Company_Name),
			DateOfBirth		= ISNULL(@DateOfBirth, DateOfBirth),
			Designation		= ISNULL(@Designation, Designation),
			Emp_Type		= ISNULL(@Emp_Type, Emp_Type),
			Monthly_Income	= ISNULL(@Monthly_Income, Monthly_Income),
			Card_Held		= ISNULL(@Card_Held, Card_Held),
			Source_Code		= ISNULL(@Source_Code, Source_Code),
			Promo_Code		= ISNULL(@Promo_Code, Promo_Code),
			Lead_Date_Time	= ISNULL(@Lead_Date_Time, Lead_Date_Time),
			Product_Applied_For = ISNULL(@Product_Applied_For, Product_Applied_For),
			ExistingCust	= ISNULL(@ExistingCust, ExistingCust),
			YearsInEmp		= ISNULL(@YearsInEmp, YearsInEmp),
			Emi_Paid		= ISNULL(@Emi_Paid, Emi_Paid),
			Car_Make		= ISNULL(@Car_Make, Car_Make),
			Car_Model		= ISNULL(@Car_Model, Car_Model),
			TypeOfLoan		= ISNULL(@TypeOfLoan, TypeOfLoan),
			IP_Address		= ISNULL(@IP_Address, IP_Address),
			Indigo_UniqueKey	= ISNULL(@Indigo_UniqueKey, Indigo_UniqueKey),
			Indigo_RequestFromYesNo	= ISNULL(@Indigo_RequestFromYesNo, Indigo_RequestFromYesNo),
			VersionId		= ISNULL(@VersionId, VersionId),
			LoanAmount		= ISNULL(@LoanAmount, LoanAmount),
			ROI				= ISNULL(@ROI, ROI),
			LTV				= ISNULL(@LTV, LTV),
			CarPrice		= ISNULL(@CarPrice, CarPrice),
			IsPermitted		= ISNULL(@IsPermitted, IsPermitted),
			IsPushSuccess	= ISNULL(@IsPushSuccess, IsPushSuccess),
			BankTypeId		= ISNULL(@BankTypeId, BankTypeId),
			ApiResponse		= ISNULL(@ApiResponse, ApiResponse),
			BuyingPeriod	= ISNULL(@BuyingPeriod, BuyingPeriod),
			IncomeTypeId	= ISNULL(@IncomeTypeId, IncomeTypeId),
			YearsInRes		= ISNULL(@YearsInRes, YearsInRes),
			UpdatedOn		= GETDATE(),
			FailureReason   = ISNULL(@FailureReason,FailureReason),
			ClientId		= ISNULL(@ClientId, ClientId),
			PlatformId		= ISNULL(@PlatformId, PlatformId)
		WHERE Id = @FinanceLeadId
END

