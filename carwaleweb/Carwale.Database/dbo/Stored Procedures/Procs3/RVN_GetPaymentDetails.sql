IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[RVN_GetPaymentDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[RVN_GetPaymentDetails]
GO

	
-- =============================================
-- Author	:	Sachin Bharti(16th June 2014)
-- Description	:	Get payment details of added package
-- M
-- =============================================
CREATE PROCEDURE [dbo].[RVN_GetPaymentDetails] --26
	@TransactionId	INT
AS
BEGIN
	
	SET NOCOUNT ON;

	SELECT	DPD.ID AS PaymentId ,
			PM.ModeName AS Mode , 
			DPD.Amount , 
			DPD.BankName ,
			DPD.BranchName ,
			DPD.UtrTransactionId,
			DPD.CheckDDPdcNumber , 
			CONVERT(VARCHAR,DPD.PaymentDate,6) AS PaymentDate,
			CONVERT(VARCHAR,DPD.ChequeDDPdcDate,6) AS ChequeDDPdcDate,
			DPD.AddedOn,
			DPD.DrawerName,
			OU.UserName AS AddedBy,
			OU1.UserName AS ApprovedBy,
			CONVERT(VARCHAR,DPD.ApprovedOn,6) AS ApprovedOn,
			CASE WHEN ISNULL(DPD.IsApproved,0) = 0 THEN 0 WHEN  ISNULL(DPD.IsApproved,0) = 1 THEN 1 END AS IsApproved

	FROM
			DCRM_PaymentDetails DPD(NOLOCK) 
			INNER JOIN PaymentModes PM(NOLOCK) ON PM.Id = DPD.Mode
			INNER JOIN OprUsers OU(NOLOCK) ON OU.Id = DPD.AddedBy
			LEFT JOIN OprUsers OU1(NOLOCK) ON OU1.Id = DPD.ApprovedBy
	WHERE 
			DPD.TransactionId = @TransactionId
	ORDER BY 
			DPD.AddedOn DESC


	SELECT ISNULL(PackageStatus,1) AS PackageStatus
		From RVN_DealerPackageFeatures(NOLOCK) WHERE TransactionId = @TransactionId
END

