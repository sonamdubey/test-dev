IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_InsertPaymentDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_InsertPaymentDetails]
GO

	



-- =============================================
--	Author	:	Sachin Bharti(13th June 2014)
--	Description	:	Insert DCRM_Package payment details in dcrm_paymentdetails
--	Modifier	:	Sachin Bharti(22nd Dec 2014)
--	Modifier	:	Sachin Bharti(28th April 2015)
--	Purpose		:	Make isApproved column is null
--  Modifier   :Komal Manjare(01-FEB-2016)
-- Issuer bank Name Added
-- =============================================
CREATE PROCEDURE [dbo].[DCRM_InsertPaymentDetails]( @DCRM_TblPaymentDetails DCRM_TblPaymentDetails READONLY, @Success INT OUTPUT)
	
AS
BEGIN
	
	SET @Success = -1
    INSERT INTO DCRM_PaymentDetails	(
										TransactionId,
										Amount, 
										ServiceTax,
										IsTDSGiven,
										TDSAmount,
										PaymentReceived,
										AmountReceived,
										PANNumber,
										TANNumber,
										Mode,
										CheckDDPdcNumber,
										AttachedFile,
										FileHostUrl,
										BankName,
										BranchName,
										DrawerName,
										InFavorOf,
										PaymentDate,
										IsApproved,
										AddedBy,
										AddedOn,
										PaymentType,
										DepositedBy,
										UtrTransactionId,
										ChequeDDPdcDate,
										DepositedDate,
										IssuerBankName
										
									) 
							SELECT		
										PD.TransactionId,
										PD.ProductAmount,
										PD.ServiceTax,
										PD.IsTDSGiven,
										PD.TDSAmount,
										PD.FinalProductAmount,
										PD.Amount,
										PD.PANNumber,
										PD.TANNumber,
										PD.Mode,
										PD.CheckNumber,
										PD.AttachedFile,
										PD.FileHostUrl,
										PD.BankName,
										PD.BranchName,
										PD.DrawerName,
										PD.InFavorOf,
										PD.PaymentDate,
										NULL,
										PD.UpdatedBy,
										PD.UpdatedOn ,
										PD.PaymentType,
										PD.DepositedBy,
										PD.UtrTransactionId,
										PD.ChequeDDPdcDate,
										PD.DepositedDate,
										PD.IssuerBankName
										

							FROM 
									@DCRM_TblPaymentDetails PD

	IF @@ROWCOUNT != 0
	BEGIN
		SET @Success = 1
	END
END


