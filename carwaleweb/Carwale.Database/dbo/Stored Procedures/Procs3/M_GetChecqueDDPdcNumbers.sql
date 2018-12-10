IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[M_GetChecqueDDPdcNumbers]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[M_GetChecqueDDPdcNumbers]
GO

	-- =============================================
-- Author	:	Sachin Bharti(9th March 2015)
-- Description	:	Get all entered Cheque,DD and PDC numbers of payment details
-- Modified By : Kartik Rathod on 14 Dec 2015,fetch ChequeDDPdcDate ,  Amount of check to display in ddl
--EXEC M_GetChecqueDDPdcNumbers 3377
-- Modifier : Kartik Rathod on 22 Dec 2015, fetch cash payment details for submitting the deposit slip AND fetch branch name,bank name and depositeddate for cash transction
--Modifier  : Ajay Singh on 28 dec 2015 , add INR before amount 
-- EXEC [M_GetChecqueDDPdcNumbers] 5
-- Modified By : Komal Manjare(05-06-2016)
-- Desc : get active transactions.
-- Modified By:Komal Manjare(16-June-2016)
-- Desc: get data for online payment.
-- Modified By:Komal Manjare(20-June-2016)
-- Desc:get productamount recceived to update payment details
-- =============================================
CREATE PROCEDURE [dbo].[M_GetChecqueDDPdcNumbers] 
	
	@DealerId	INT,
	@PaymentDetailsId INT = NULL
AS
BEGIN
	
	SET NOCOUNT ON;

	SELECT 
		DISTINCT DPT.ID AS VALUE ,
		DPT.CheckDDPdcNumber + ' - ' + CONVERT(VARCHAR ,CONVERT(date,DPT.ChequeDDPdcDate),113) + ' - INR ' + CONVERT(VArchAr ,DPT.AmountReceived) AS TEXT
	FROM 
		DCRM_PaymentDetails DPT (NOLOCK)
		INNER JOIN DCRM_PaymentTransaction DP(NOLOCK) ON DP.TransactionId=DPT.TransactionId AND DP.IsActive=1 -- Modified By : Komal Manjare(05-06-2016)-get active transactions
		INNER JOIN DCRM_SalesDealer DSD(NOLOCK) ON DPT.TransactionId = DSD.TransactionId 
		AND DSD.DealerId = @DealerId AND DPT.Mode IN (2,3,8) --For Checqu , DD and PDC case only
		AND ( ISNULL(DPT.BankName,'') = '' OR ISNULL(DPT.BranchName,'') = '')

	
	SELECT 
		DISTINCT DPT.ID AS VALUE ,
		CONVERT(VARCHAR,DPT.TransactionId) + ' - ' + CONVERT(VARCHAR ,CONVERT(date,ISNULL(DPT.DepositedDate,'')),113) + ' - INR' + CONVERT(VARCHAR ,DPT.AmountReceived) AS TEXT
	FROM 
		DCRM_PaymentDetails DPT (NOLOCK)
		INNER JOIN DCRM_PaymentTransaction DP(NOLOCK) ON DP.TransactionId=DPT.TransactionId AND DP.IsActive=1 -- Modified By : Komal Manjare(05-06-2016)-get active transactions		
		INNER JOIN DCRM_SalesDealer DSD(NOLOCK) ON DPT.TransactionId = DSD.TransactionId 
					AND DSD.DealerId = @DealerId AND DPT.Mode = 1 --For Cash Only
					AND ( ISNULL(DPT.DepSlipHostUrl,'') = '' OR ISNULL(DPT.DepSlipOriginalImgPath,'') = '')
					AND DPT.IsApproved IS NULL
					AND DPT.Amount > 0

    -- Added By:Komal Manjare (16-06-2016) get details for online payment
	SELECT 
	DISTINCT DPT.ID AS VALUE,
	CONVERT(VARCHAR,DPT.TransactionId)+'-'+CONVERT(VARCHAR ,CONVERT(date,ISNULL(DPT.PaymentDate,'')),113)+' - INR'+CONVERT(VARCHAR ,DPT.AmountReceived) AS TEXT
	FROM DCRM_PaymentDetails DPT(NOLOCK)
	INNER JOIN DCRM_PaymentTransaction DP(NOLOCK) ON DP.TransactionId=DPT.TransactionId AND DP.IsActive=1
	INNER JOIN DCRM_SalesDealer DSD(NOLOCK) ON DPT.TransactionId=DSD.TransactionId AND DSD.DealerId=@DealerId
	AND ( ISNULL(DPT.DepSlipHostUrl,'') = '' OR ISNULL(DPT.DepSlipOriginalImgPath,'') = '')
					AND DPT.Mode=4 -- For online only 
	AND DPT.IsApproved IS NULL AND DPT.Amount>0

	if(@PaymentDetailsId IS NOT NULL)
	BEGIN
		SELECT BankName,BranchName,DepositedDate,DepositedBy 
		FROm DCRM_PaymentDetails (NOLOCK)
		WHERE		ID = @PaymentDetailsId
	END	
END
