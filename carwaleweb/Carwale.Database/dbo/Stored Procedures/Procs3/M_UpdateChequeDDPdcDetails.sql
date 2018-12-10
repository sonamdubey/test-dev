IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[M_UpdateChequeDDPdcDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[M_UpdateChequeDDPdcDetails]
GO

	-- =============================================
-- Author	:	Sachin Bharti(9th March 2015)
-- Description	:	Update Cheque,DD and PDC details
-- Modified By : Kartik Rathod on 22 dec 2015, to set cash deposit slip details and DepositedBy
-- Modified By: Ajay Singh on 24 dec 2015,added Source column 1 for webapplication,2 for mobileApp
-- =============================================
CREATE PROCEDURE [dbo].[M_UpdateChequeDDPdcDetails]
	
	@PaymentDetailsId	INT , 
	@BankName	VARCHAR(100),
	@BranchName	VARCHAR(100),
	@UpdatedBy	INT,
	@IsAdded	INT OUTPUT,
	@DepositDate DATETIME = NULL,
	@DepSlipHostUrl    VARCHAR(50) = NULL,
	@DepSlipOriginalImgPath VARCHAR(300) = NULL,
	@DepositedBy VARCHAR(100) = NULL,
	@SourceType INT = 1
AS
BEGIN
	
	SET @IsAdded = 0
	
	UPDATE DCRM_PaymentDetails 
		SET	BankName	= @BankName ,
			BranchName	= @BranchName ,
			UpdatedBy	= @UpdatedBy ,
			UpdatedOn	= GETDATE(),
			DepositedDate = ISNULL(@DepositDate, DepositedDate),
			DepSlipHostUrl = ISNULL(@DepSlipHostUrl, DepSlipHostUrl),
			DepSlipOriginalImgPath = ISNULL(@DepSlipOriginalImgPath,DepSlipOriginalImgPath),
			DepositedBy = ISNULL(@DepositedBy,DepositedBy),
			Source      = @SourceType
	WHERE
		ID = @PaymentDetailsId

	IF @@ROWCOUNT = 1
		SET @IsAdded = 1
	
END




