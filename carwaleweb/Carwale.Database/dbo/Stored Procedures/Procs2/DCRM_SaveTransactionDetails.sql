IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_SaveTransactionDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_SaveTransactionDetails]
GO

	


--=============================================
-- Author	:	Vinay Kumar Prajapati(24th Dec 2016)
-- Description	:	Save Complete transaction Details
 -- EXEC DCRM_SaveTransactionDetails 
 --Modified By:Komal Manjare(10 feb 2016)
 --Issuer Bank Name added
 -- Modified By : Kartik Rathod 0n 2 June 2016,added comments
-- =============================================
CREATE PROCEDURE [dbo].[DCRM_SaveTransactionDetails]
	@Source                 SMALLINT = 1,
	@SalesDealerIds			VARCHAR(50),
	@TransactionId          Int,
	@TotalClosingAmount		NUMERIC(18,2),
	@DiscountAmount			NUMERIC(18,2),
	@ProductAmount			NUMERIC(18,2),
	@FinalAmount			NUMERIC(18,2),

	-------------------Passed Static value 
	@ServiceTAX				FLOAT = null,
	@IsTDSGiven				BIT = null,
	@TDSAmount				FLOAT = null,
	@PANNumber				VARCHAR(10) = null,
	@TANNumber				VARCHAR(10) = null,

	@UpdatedBy				INT,
	@DCRM_TblPaymentDetails DCRM_TblPaymentDetails READONLY,
	@TransId			    INT OUTPUT
AS
 DECLARE  @Success INT 

BEGIN
	IF @TransactionId = -1
		BEGIN 
			 EXEC M_DCRMCreateTransaction   @SalesDealerIds,
											@TotalClosingAmount,
											@DiscountAmount,
											@ProductAmount,
											@ServiceTAX,
											@IsTDSGiven,
											@TDSAmount,
											@FinalAmount,
											@PANNumber,
											@TANNumber,
											@UpdatedBy,
											@Source,
											@TransactionId OUTPUT

               SET  @TransId = @TransactionId           

			END
	    ELSE
			 SET  @TransId = @TransactionId
	                    

   
	--- Same Working functionality  as SP - DCRM_InsertPaymentDetails

	INSERT INTO DCRM_PaymentDetails	(TransactionId,Amount, ServiceTax,IsTDSGiven,TDSAmount,PaymentReceived,AmountReceived,
	                                 PANNumber,TANNumber,Mode,CheckDDPdcNumber,AttachedFile,FileHostUrl,BankName,BranchName,
									 DrawerName,InFavorOf,PaymentDate,IsApproved,AddedBy,AddedOn,PaymentType,DepositedBy,
									 UtrTransactionId,ChequeDDPdcDate,DepositedDate,Source,HostUrl,OriginalImgPath,IssuerBankName ,Comments
									) 
						     SELECT		
									@TransId,PD.ProductAmount,PD.ServiceTax,PD.IsTDSGiven,PD.TDSAmount,PD.FinalProductAmount,PD.Amount,
									PD.PANNumber,PD.TANNumber,PD.Mode,PD.CheckNumber,PD.AttachedFile,PD.FileHostUrl,PD.BankName,PD.BranchName,
									PD.DrawerName,PD.InFavorOf,PD.PaymentDate,NULL,PD.UpdatedBy,PD.UpdatedOn ,PD.PaymentType,PD.DepositedBy,
									PD.UtrTransactionId,PD.ChequeDDPdcDate,PD.DepositedDate,@Source,PD.FileHostUrl,PD.AttachedFile,PD.IssuerBankName ,	PD.Comments
							  FROM 
									@DCRM_TblPaymentDetails PD

END





