IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_TransactionProductsList]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_TransactionProductsList]
GO

	-- =============================================
-- Author	:	Sachin Bharti(4th Feb 2015)
-- Description	:	Get transaction products list
-- Modifier	:	Sachin Bharti(3rd June 2015)
-- Description	:	Get product closing amount,generated invoice amount ,
--					rest invoice amount also for days/leads/quantity at product level
-- Modifier	:	Sachin Bharti(5th June 2015)
-- Purpose	:	Added columns for individual product details
-- Modifier	:	Sachin Bharti(15th June 2015)
-- Purpose	:	Added condition for package quantity 
-- Modifier : Amit Yadav(16th Feb 2016)
-- Purpose	: To remove problems when two same products are added in same transaction
-- Exec [dbo].[DCRM_TransactionProductsList] 11474
--Modified By:Komal Manjare on(19-Feb-2016)
--Desc:condition for quantity and type of product modified--Modified By:Komal Manjare on(16-Mar-2016)
--Desc:get legalname and dealername for transaction
-- Modified By : Komal Manjare(01-June-2016)
-- gte pendinginvoiceamount for particular transaction
-- =============================================
CREATE PROCEDURE [dbo].[DCRM_TransactionProductsList] 
	
	@TransactionId	INT

AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		DSD.Id AS SalesId,
		PK.Id AS PackageId,
		PK.Name AS Package,
		
		--At product level
		DSD.ClosingAmount AS ProductClosingAmount,
		
		--At transaction level
		DPT.TransactionId,
		DPT.TotalClosingAmount,
		DPT.DiscountAmount,
		DPT.FinalAmount AS FinalCollectionAmount,
		--SUM(DPD.Amount) AS CollectedProductAmount,
		(SELECT SUM(DPD1.Amount) FROM DCRM_PaymentDetails DPD1(NOLOCK) WHERE DPD1.TransactionId=@TransactionId) AS CollectedProductAmount,
		ISNULL((SELECT SUM (GD.ProductInvoiceAmount) FROM M_GeneratedInvoiceDetail GD(NOLOCK) 
				INNER JOIN M_GeneratedInvoice MGI(NOLOCK) ON MGI.Id = GD.InvoiceId AND MGI.Status IN (1,2) 
				WHERE 	MGI.TransactionId = @transactionId AND GD.PackageId = PK.Id  AND (DSD.Id = GD.SalesDealerId OR GD.SalesDealerId IS NULL) ),0)  AS GenInvAmount,--SalesDealerId added by Amit Yadav
		
		--CASE WHEN PK.InqPtCategoryId IN (24,32) THEN ISNULL(DSD.NoOfLeads,0) WHEN PK.InqPtCategoryId IN (33,37) THEN ISNULL(DSD.Quantity,0) ELSE ISNULL(DSD.PitchDuration,0) END AS TotalQuantity,
		--CASE WHEN PK.InqPtCategoryId IN (24,32) THEN 'Leads' WHEN PK.InqPtCategoryId IN (33,37) THEN 'Quantity' ELSE 'Days' END AS QuantityType,
		
		CASE 
			WHEN PK.InqPtCategoryId=24 THEN 
				CASE WHEN DSD.ContractType=1 THEN ISNULL(DSD.NoOfLeads,0) ELSE ISNULL(DSD.PitchDuration,0) END 
		    WHEN PK.InqPtCategoryId=32 THEN ISNULL(DSD.NoOfLeads,0) 
			WHEN PK.InqPtCategoryId IN (33,37) THEN ISNULL(DSD.Quantity,0) 
				ELSE  ISNULL(DSD.PitchDuration,0)
		END AS TotalQuantity,
		CASE 
			WHEN PK.InqPtCategoryId=24 THEN 
				CASE WHEN DSD.ContractType=1 THEN 'Leads'  ELSE 'Days' END 
		    WHEN PK.InqPtCategoryId=32 THEN 'Leads' 
			WHEN PK.InqPtCategoryId IN (33,37)  THEN 'Quantity'
				ELSE 'Days'
		END AS QuantityType,
		ISNULL((SELECT SUM(GD.Quantity) FROM M_GeneratedInvoiceDetail GD(NOLOCK)
				INNER JOIN M_GeneratedInvoice MGI(NOLOCK) ON MGI.Id = GD.InvoiceId 
				WHERE 	MGI.TransactionId = @transactionId AND GD.PackageId = PK.Id AND MGI.Status IN (1,2) AND (DSD.Id = GD.SalesDealerId OR GD.SalesDealerId IS NULL)),0) AS GenInvQuantity --only pending and approved invoice &&--SalesDealerId added by Amit Yadav
		,( 
			(	SELECT ISNULL(SUM(DPD.Amount),0) FROM DCRM_PaymentDetails DPD(NOLOCK) 
				WHERE DPD.TransactionId = DPT.TransactionId AND ISNULL(DPD.IsApproved,1) = 1 AND (( DPD.Mode = 8 AND DPD.ChequeDDPdcDate <=GETDATE()) OR DPD.Mode <> 8) )
				- 
			(	SELECT ISNULL(SUM(ISNULL(MG1.InvoiceAmount,0)),0) FROM M_GeneratedInvoice MG1 (NOLOCK) 
				WHERE MG1.TransactionId = DPT.TransactionId AND ISNULL(MG1.Status,2) IN(1,2))--taking approved and pending cases
		) AS PendingInvoiceAmount
		
		FROM	
		DCRM_PaymentTransaction DPT(NOLOCK) 
		INNER JOIN DCRM_PaymentDetails DPD(NOLOCK) ON DPT.TransactionId = DPD.TransactionId AND ISNULL(DPD.IsApproved,1) = 1 AND ISNULL(DPT.ProductAmount,0) > 1  AND (( DPD.Mode = 8 AND DPD.ChequeDDPdcDate <=GETDATE()) OR DPD.Mode <> 8)
		INNER JOIN DCRM_SalesDealer DSD(NOLOCK) ON DSD.TransactionId = DPT.TransactionId AND DSD.PitchingProduct <> 46 --exclude free listing products && 
		INNER JOIN Packages PK(NOLOCK) ON PK.Id = DSD.PitchingProduct
		LEFT JOIN M_InvoiceNumberSeries INS(NOLOCK) ON INS.InquiryPointId = PK.InqPtCategoryId
		LEFT JOIN M_GeneratedInvoice MGI (NOLOCK) ON MGI.TransactionId = DPT.TransactionId
		LEFT JOIN M_GeneratedInvoiceDetail MD (NOLOCK) ON MD.InvoiceId=MGI.Id
		
	WHERE
		DPT.TransactionId = @transactionId
		--AND (DSD.Id = MD.SalesDealerId OR MD.SalesDealerId IS NULL)--Added by Amit Yadav to remove problems when two same products are added in same transaction
	GROUP BY
		DSD.ClosingAmount,DSD.Id,PK.Name,PK.Id,DPT.TransactionId,DPT.DiscountAmount,DPT.FinalAmount,DPT.TotalClosingAmount,DSD.PitchingProduct,
		--CASE WHEN PK.InqPtCategoryId IN (24,32) THEN ISNULL(DSD.NoOfLeads,0) WHEN PK.InqPtCategoryId IN (33,37) THEN ISNULL(DSD.Quantity,0) ELSE ISNULL(DSD.PitchDuration,0) END,
		--CASE WHEN PK.InqPtCategoryId IN (24,32) THEN 'Leads' WHEN PK.InqPtCategoryId IN (33,37) THEN 'Quantity' ELSE 'Days' END
		CASE 
			WHEN PK.InqPtCategoryId=24 THEN 
				CASE WHEN DSD.ContractType=1 THEN ISNULL(DSD.NoOfLeads,0) ELSE ISNULL(DSD.PitchDuration,0) END 
		    WHEN PK.InqPtCategoryId=32 THEN ISNULL(DSD.NoOfLeads,0) 
			WHEN PK.InqPtCategoryId IN (33,37) THEN ISNULL(DSD.Quantity,0) 
				ELSE  ISNULL(DSD.PitchDuration,0)
		END ,
		CASE 
			WHEN PK.InqPtCategoryId=24 THEN 
				CASE WHEN DSD.ContractType=1 THEN 'Leads'  ELSE 'Days' END 
		    WHEN PK.InqPtCategoryId=32 THEN 'Leads' 
			WHEN PK.InqPtCategoryId IN (33,37)  THEN 'Quantity'
				ELSE 'Days'
		END 

		--SELECT SUM(Amount) AS CollectedProductAmount FROM DCRM_PaymentDetails WITH(NOLOCK) WHERE TransactionId=@TransactionId
END

------------------------------------------------------------------------------------------------------------------------------


