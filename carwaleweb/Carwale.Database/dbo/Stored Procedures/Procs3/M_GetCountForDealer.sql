IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[M_GetCountForDealer]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[M_GetCountForDealer]
GO

	
-- =============================================
-- Author:		Amit Yadav
-- Create date: 4th Sept,2015
-- Description:	Get Count of Product, Transaction, Generate Invoice and Update Cheque/DD/PDC for Dealer Dashboard.
-- EXEC M_GetCountForDealer 5
-- Modified By : Komal Manjare(05-06-2016)
-- Desc : get active transactions 
-- =============================================
CREATE PROCEDURE [dbo].[M_GetCountForDealer]
	
	@DealerId	INT

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	--Query for Dealer Product which are Open and Closed
		SELECT 
			DSD.DealerId,
			COUNT(DSD.Id) AS ProductCount
		FROM 
			DCRM_SalesDealer AS DSD(NOLOCK)
			INNER JOIN Packages AS P(NOLOCK) ON DSD.PitchingProduct=P.Id
		WHERE 
			DSD.LeadStatus IN (1,2)
			AND DSD.TransactionId IS NULL --closed but transaction not created yet
			AND DSD.DealerId=@DealerId
		GROUP BY
			DSD.DealerId
		ORDER BY
			DSD.DealerId
		
	--Query for Dealer Transaction Details
		SELECT DISTINCT
			DSD.DealerId,
			DPT.TransactionId
		FROM 
			DCRM_PaymentTransaction AS DPT(NOLOCK) 
			INNER JOIN DCRM_SalesDealer AS DSD(NOLOCK) ON DSD.TransactionId = DPT.TransactionId 
		WHERE 
			DSD.DealerId = @DealerId
			AND DPT.IsActive=1
		GROUP BY
			DSD.DealerId,DPT.TransactionId,DPT.ProductAmount
		ORDER BY
			DSD.DealerId

	--Transaction on which action can be taken on
	SELECT DISTINCT DPT.TransactionId,DSD.DealerId

	FROM DCRM_PaymentTransaction DPT WITH(NOLOCK)
		INNER JOIN DCRM_PaymentDetails DPD WITH(NOLOCK) ON DPD.TransactionId = DPT.TransactionId AND ISNULL(DPT.ProductAmount,0) > 1  AND (( DPD.Mode = 8 AND DPD.ChequeDDPdcDate <=GETDATE()) OR DPD.Mode <> 8) AND DPT.IsActive=1 -- Modified By : Komal Manjare(05-06-2016)get active transactions 
		INNER JOIN DCRM_SalesDealer DSD WITH(NOLOCK) ON DSD.TransactionId = DPT.TransactionId AND ISNULL(DSD.CampaignType,3) IN (3,5) AND DSD.PitchingProduct <> 46 --Exclude free listing products.
		LEFT JOIN M_GeneratedInvoice MG WITH(NOLOCK) ON DPT.TransactionId = MG.TransactionId
	WHERE
	((MG.TransactionId IS NULL AND  ISNULL(DPD.IsApproved,1) =1) --Komal Manjare(05-06-2016)--get only approved or yet to be approved transaction
	OR
	(((	SELECT ISNULL(SUM(DPD.Amount),0) FROM DCRM_PaymentDetails DPD(NOLOCK) 
						WHERE DPD.TransactionId = DPT.TransactionId  AND ( ( DPD.Mode = 8 AND DPD.ChequeDDPdcDate <=GETDATE()) OR DPD.Mode <> 8) AND ISNULL(DPD.IsApproved,1) =1)
					- 
					(	SELECT (SUM(ISNULL(MG1.InvoiceAmount,0))) FROM M_GeneratedInvoice MG1 (NOLOCK)
						WHERE MG1.TransactionId = DPT.TransactionId AND ISNULL(MG1.Status,2) IN(1,2,5))--Taking approved and pending cases.
				) > 1))
				AND DSD.DealerId=@DealerId
	GROUP BY 
	   DPT.TransactionId,DSD.DealerId
	ORDER BY 
		DPT.TransactionId DESC  


--Commented by Amit Yadav count was wrong
----Added by Ajay Singh(5 frb 2016)
----Needed only those Invoices on which we can take action
--		SELECT 
--		DISTINCT DPT.TransactionId,DSD.DealerId
--	FROM
--		DCRM_PaymentTransaction DPT(NOLOCK)
--		INNER JOIN DCRM_PaymentDetails DPD(NOLOCK) ON DPD.TransactionId = DPT.TransactionId AND ISNULL(DPT.ProductAmount,0) > 1  AND (( DPD.Mode = 8 AND DPD.ChequeDDPdcDate <=GETDATE()) OR DPD.Mode <> 8)
--		INNER JOIN DCRM_SalesDealer DSD(NOLOCK) ON DSD.TransactionId = DPT.TransactionId AND DSD.DealerId = @DealerId AND ISNULL(DSD.CampaignType,3) IN (3,5) AND DSD.PitchingProduct <> 46 --exclude free listing products
--		LEFT JOIN Dealers D WITH(NOLOCK) ON D.Id= DSD.DealerId
--		LEFT JOIN M_GeneratedInvoice MG(NOLOCK) ON DPT.TransactionId = MG.TransactionId
--	WHERE 
--	   DPT.TransactionId NOT IN (SELECT DSD.TransactionId FROM M_GeneratedInvoice AS MGI WITH(NOLOCK)
--       INNER JOIN DCRM_SalesDealer AS DSD WITH(NOLOCK) ON DSD.TransactionId = MGI.TransactionId
--       WHERE DSD.DealerId = @DealerId
--       AND MGI.Status  IN (1,2,5)
--       GROUP BY DSD.TransactionId)
--	GROUP BY 
--	   DPT.TransactionId,DSD.DealerId
--	ORDER BY 
--		DPT.TransactionId DESC  




	--Query for Update Cheque/DD/PDC
		SELECT DISTINCT
			DPT.ID AS TransactionId,
			DSD.DealerId
		FROM
			DCRM_PaymentDetails DPT WITH(NOLOCK)
			INNER JOIN DCRM_PaymentTransaction DP(NOLOCK) ON DP.TransactionId=DPT.TransactionId AND DP.IsActive=1 -- Modified By : Komal Manjare(05-06-2016)get active transactions 
			INNER JOIN DCRM_SalesDealer DSD WITH(NOLOCK) ON DPT.TransactionId = DSD.TransactionId 
			AND
			((
			DPT.Mode IN (2,3,8) --For Cheque,DD and PDC case only.
			AND ( ISNULL(DPT.BankName,'') = '' OR ISNULL(DPT.BranchName,'') = '')
			)
			OR
			(DPT.Mode = 1 --For Cash Only.
			AND ( ISNULL(DPT.DepSlipHostUrl,'') = '' OR ISNULL(DPT.DepSlipOriginalImgPath,'') = '')
			AND DPT.IsApproved IS NULL
			AND DPT.Amount > 0
			))
		WHERE
			DSD.DealerId = @DealerId
END

-----------------------------------------------------------------------------------------------------------------------


