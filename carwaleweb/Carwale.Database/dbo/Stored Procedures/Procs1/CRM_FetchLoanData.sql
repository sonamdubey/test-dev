IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_FetchLoanData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_FetchLoanData]
GO

	CREATE PROCEDURE [dbo].[CRM_FetchLoanData]

	@LoanId					Numeric,
	@LeadId					Numeric OutPut,
	@LoanAmount				Numeric OutPut,
	@LoanTenure				Numeric OutPut,
	@EMI					Numeric OutPut,
	@InterestRate			Decimal(18,2) OutPut,
	@FinancerId				Numeric OutPut,
	@FinancerName			VarChar(200) OutPut,
	@LoanApprovalStatusId	Int OutPut,
	@LoanApprovalStatus		VarChar(100) OutPut,
	@APCNumber				VarChar(50) OutPut,
	@ApprovalRemarks		VarChar(500) OutPut,
	@DocStatusId			SmallInt OutPut,
	@DocStatus				VarChar(100) OutPut,
	@DocPickupComments		VarChar(1000) OutPut,
	@LoanDisbursedById		Numeric OutPut,
	@LoanDisbursedByName	VarChar(100) OutPut,
	@DisbursementRemark		VarChar(1000) OutPut,
	@UpdatedById			Numeric OutPut,
	@UpdatedByName			VarChar(100) OutPut,
	
	@IsLoanInterested			Bit OutPut,
	@IsCaseRegistered			Bit OutPut,
	@IsDocCollected				Bit OutPut,
	@IsDisbursementCompleted	Bit OutPut,
	@IsReleaseOrderIssued		Bit OutPut,

	@ApprovedOn				DateTime OutPut,
	@DocPickupRequestDate	DateTime OutPut,
	@DocPickupDate			DateTime OutPut,
	@DisbursementDate		DateTime OutPut,
	@CreatedOn				DateTime OutPut,
	@UpdatedOn				DateTime OutPut
				
 AS
	
BEGIN

	SELECT	
		@LeadId					= CLD.LeadId,
		@LoanAmount				= CLD.LoanAmount,
		@LoanTenure				= CLD.LoanTenure,
		@EMI					= CLD.EMI,
		@InterestRate			= CLD.InterestRate,
		@FinancerId				= CLD.FinancerId,
		@FinancerName			= NFA1.Name,
		@LoanApprovalStatusId	= CLD.LoanApprovalStatusId,
		@LoanApprovalStatus		= LAS.Name,
		@APCNumber				= CLD.APCNumber,
		@ApprovalRemarks		= CLD.ApprovalRemarks,
		@DocStatusId			= CLD.DocStatusId,
		@DocStatus				= LDS.Name,
		@DocPickupComments		= CLD.DocPickupComments,
		@LoanDisbursedById		= CLD.LoanDisbursedBy,
		@LoanDisbursedByName	= NFA2.Name,
		@DisbursementRemark		= CLD.DisbursementRemark,
		@UpdatedById			= CLD.UpdatedBy,
		@UpdatedByName			= OU.UserName,
		
		@IsLoanInterested			= CLD.IsLoanInterested,
		@IsCaseRegistered			= CLD.IsCaseRegistered,
		@IsDocCollected				= CLD.IsDocCollected,
		@IsDisbursementCompleted	= CLD.IsDisbursementCompleted,
		@IsReleaseOrderIssued		= CLD.IsReleaseOrderIssued,

		@ApprovedOn				= CLD.ApprovedOn,
		@DocPickupRequestDate	= CLD.DocPickupRequestDate,
		@DocPickupDate			= CLD.DocPickupDate,
		@DisbursementDate		= CLD.DisbursementDate,
		@CreatedOn				= CLD.CreatedOn,
		@UpdatedOn				= CLD.UpdatedOn

	FROM (((((CRM_LoanData AS CLD LEFT JOIN OprUsers AS OU ON CLD.UpdatedBy = OU.Id)
			LEFT JOIN CRM_EventTypes AS LAS ON CLD.LoanApprovalStatusId = LAS.Id)
			LEFT JOIN CRM_EventTypes AS LDS ON CLD.DocStatusId = LDS.Id)
			LEFT JOIN NCS_FinanceAgency AS NFA1 ON CLD.FinancerId = NFA1.Id)
			LEFT JOIN NCS_FinanceAgency AS NFA2 ON CLD.LoanDisbursedBy = NFA2.Id)

	WHERE CLD.Id = @LoanId
END












