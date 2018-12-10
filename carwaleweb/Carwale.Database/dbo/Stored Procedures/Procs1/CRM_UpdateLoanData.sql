IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_UpdateLoanData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_UpdateLoanData]
GO

	CREATE PROCEDURE [dbo].[CRM_UpdateLoanData]

	@Id						Numeric,
	@LoanAmount				Numeric,
	@LoanTenure				Numeric,
	@EMI					Numeric,
	@InterestRate			Decimal(18,2),
	@FinancerId				Numeric,
	@IsLoanInterested		Bit,
	@IsCaseRegistered		Bit,
	@LoanApprovalStatusId	Int,
	@APCNumber				VarChar(50),
	@ApprovalRemarks		VarChar(500),
	@ApprovedOn				DateTime,
	@DocPickupRequestDate	DateTime,
	@DocPickupDate			DateTime,
	@IsDocCollected			Bit,
	@DocStatusId			SmallInt,
	@DocPickupComments		VarChar(1000),
	@IsDisbursementCompleted Bit,
	@LoanDisbursedBy		Numeric,
	@DisbursementDate		DateTime,
	@DisbursementRemark		VarChar(1000),
	@IsReleaseOrderIssued	Bit,
	@UpdatedBy				Numeric,
	@UpdatedOn				DateTime,
	@Status					Bit OutPut
				
 AS
	
BEGIN
	SET @Status = 0
	IF @Id <> -1
		BEGIN
			UPDATE CRM_LoanData 
			SET LoanAmount				= @LoanAmount,
				LoanTenure				= @LoanTenure,
				EMI						= @EMI,
				InterestRate			= @InterestRate,
				FinancerId				= @FinancerId,
				IsLoanInterested		= @IsLoanInterested,
				IsCaseRegistered		= @IsCaseRegistered,
				LoanApprovalStatusId	= @LoanApprovalStatusId,
				APCNumber				= @APCNumber,
				ApprovalRemarks			= @ApprovalRemarks,
				ApprovedOn				= @ApprovedOn,
				DocPickupRequestDate	= @DocPickupRequestDate,
				DocPickupDate			= @DocPickupDate,
				IsDocCollected			= @IsDocCollected,
				DocStatusId				= @DocStatusId,
				DocPickupComments		= @DocPickupComments,
				IsDisbursementCompleted	= @IsDisbursementCompleted,
				LoanDisbursedBy			= @LoanDisbursedBy,
				DisbursementDate		= @DisbursementDate,
				DisbursementRemark		= @DisbursementRemark,
				IsReleaseOrderIssued	= @IsReleaseOrderIssued,
				UpdatedBy				= @UpdatedBy,
				UpdatedOn				= @UpdatedOn
			WHERE Id = @Id
			
			SET @Status = 1
		END
END











