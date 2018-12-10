IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[M_GetTransactionForInvoice]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[M_GetTransactionForInvoice]
GO

	-- =============================================
-- Author	:	Sachin Bharti(5th Feb 2015)
-- Description	:	Get all transactions for invoice generation
-- execute [dbo].[M_GetTransactionForInvoice] 3377
-- Modified BY : Kartik Rathod on 2 Dec 2015 added condtition for CampaignType(5)ie for multi-outlet
-- Modified By : Komal Manjare on 24 dec 2015 , added condition for api in filter case
-- @State =null (All data)
-- @State = 0 (Rejected)
-- @State = 1 (accepted)
-- @State = 2 (pending)
--Modified By:Komal Manjare on(24-Dec-2015) get dealers legalname
-- Modified By : Amit Yadav(14th Feb 2016)
-- Purpose : To get only those transaction which has some payments which in not rejected.
-- Modifier : Amit Yadav (04-04-2016)
-- Purpose : To get only active transactions.
-- =============================================
CREATE PROCEDURE [dbo].[M_GetTransactionForInvoice] 
	
	@DealerId	INT,
	@State SMALLINT = null
AS
BEGIN

   SELECT 
	
		DISTINCT DPT.TransactionId,DPT.ProductAmount,D.LegalName,D.Organization,--Added By Komal Manjare to fetch dealer details
		( 
			(	SELECT ISNULL(SUM(DPD.Amount),0) FROM DCRM_PaymentDetails DPD(NOLOCK) 
				WHERE DPD.TransactionId = DPT.TransactionId AND ISNULL(DPD.IsApproved,1) = 1 AND (( DPD.Mode = 8 AND DPD.ChequeDDPdcDate <=GETDATE()) OR DPD.Mode <> 8) )
				- 
			(	SELECT ISNULL(SUM(ISNULL(MG1.InvoiceAmount,0)),0) FROM M_GeneratedInvoice MG1 (NOLOCK) 
				WHERE MG1.TransactionId = DPT.TransactionId AND ISNULL(MG1.Status,2) IN(1,2))--taking approved and pending cases
		) AS RestAmount,
		
		CASE 
			WHEN MG.TransactionId IS NULL THEN 'disableText' ELSE '' END AS IsAnyInvoice,
		CASE 
			WHEN MG.TransactionId IS NULL THEN ''
			WHEN(
					(	SELECT ISNULL(SUM(DPD.Amount),0) FROM DCRM_PaymentDetails DPD(NOLOCK) 
						WHERE DPD.TransactionId = DPT.TransactionId  AND ( ( DPD.Mode = 8 AND DPD.ChequeDDPdcDate <=GETDATE()) OR DPD.Mode <> 8) AND ISNULL(DPD.IsApproved,1) =1)
					- 
					(	SELECT ISNULL(SUM(ISNULL(MG1.InvoiceAmount,0)),0) FROM M_GeneratedInvoice MG1 (NOLOCK)
						WHERE MG1.TransactionId = DPT.TransactionId AND ISNULL(MG1.Status,2) IN(1,2))--taking approved and pending cases
				) > 1 THEN ''
		ELSE 
			'hide' 
		END AS IsGenerate

	FROM
		DCRM_PaymentTransaction DPT(NOLOCK)

		INNER JOIN DCRM_PaymentDetails DPD(NOLOCK) ON DPD.TransactionId = DPT.TransactionId AND ISNULL(DPT.ProductAmount,0) > 1  AND (( DPD.Mode = 8 AND DPD.ChequeDDPdcDate <=GETDATE()) OR DPD.Mode <> 8) AND ISNULL(DPD.IsApproved,1)=1 --Added by Amit Yadav  [(ISNULL(DPD.IsApproved,1)=1)] to get only those transactions with payments not rejected.
		INNER JOIN DCRM_SalesDealer DSD(NOLOCK) ON DSD.TransactionId = DPT.TransactionId AND DSD.DealerId = @DealerId AND ISNULL(DSD.CampaignType,3) IN (3,5) AND DSD.PitchingProduct <> 46 --exclude free listing products
		LEFT JOIN Dealers D WITH(NOLOCK) ON D.Id= DSD.DealerId
		LEFT JOIN M_GeneratedInvoice MG(NOLOCK) ON DPT.TransactionId = MG.TransactionId
	WHERE ((@State IS NULL)
			OR
		  (@State = 0 AND MG.Status in(3,4) )
		  OR
		  (@State = 1 AND MG.Status =2 )
		  OR
		  (@State =2 AND MG.Status = 1))
		  AND
		  ISNULL(DPT.IsActive,1) = 1 --Added By Amit Yadav on 04-04-2016

	GROUP BY 
		DPT.TransactionId,MG.TransactionId,DPT.ProductAmount,D.LegalName,D.Organization
	ORDER BY 
		DPT.TransactionId DESC  
END


------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

