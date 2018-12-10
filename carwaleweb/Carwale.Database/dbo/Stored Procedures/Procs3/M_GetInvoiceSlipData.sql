IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[M_GetInvoiceSlipData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[M_GetInvoiceSlipData]
GO

	-- =============================================
-- Author	:	Sachin Bharti(9th Feb 2015)
-- Description	:	Get invoice data for Dealer Invoice Slip
-- Execute [dbo].[M_GetInvoiceSlipData] 1126
--Modifier	:	Amit Yadav(2nd Nov 2015)
--Purppse	:	To show Contract summary on Invoice(Final Amount and Totalleads and Totalduartion)
--Modifier	:	Amit Yadav(25th Jan 2015)
--Purppse	:	To remove repeatative products from invoice slip.
--EXEC [M_GetInvoiceSlipData] 1444
-- =============================================
CREATE PROCEDURE [dbo].[M_GetInvoiceSlipData] 
	
	@InvoiceId	INT,
	@InvoiceSeriesGroupId	SMALLINT = NULL

AS
BEGIN

	--get product and other info of invoice
	SELECT 
		DISTINCT IPC.Name AS Product, 
		MGI.InvoiceNumber ,
		MD.ProductInvoiceAmount AS InvoiceAmount,
		MGI.InvoiceName,
		MD.Id AS InvDetailId,
		MGI.TransactionId,
		MGI.Status,
		CONVERT(VARCHAR,MGI.UpdatedOn,106) AS InvoiceDate,
		PK.Name AS SubProduct,
		PK.Id AS PkgId,
		MN.AccoutingLedgerName,
		MN.ServiceTaxCategory,
		MGI.TextToBePrinted,
	    DPT.FinalAmount,--Final Amount of a transaction
		CAST(CASE WHEN PK.InqPtCategoryId IN (24,32) THEN 'Leads' WHEN PK.InqPtCategoryId IN (37,33) THEN 'Quantity' ELSE 'Duration(Days)' END AS VARCHAR(20))+CAST(' : ' AS VARCHAR(5))+CAST(CASE WHEN PK.InqPtCategoryId IN (24,32) THEN DSD.NoOfLeads WHEN PK.InqPtCategoryId IN (37,33) THEN DSD.Quantity ELSE DSD.PitchDuration END AS VARCHAR(20)) AS TotalGoal,--ADDED TO GET TOTALGOAL
		MGI.ShowContractSummary
		
	FROM 
		M_GeneratedInvoice MGI(NOLOCK) 
		INNER JOIN M_GeneratedInvoiceDetail MD(NOLOCK) ON MD.InvoiceId = MGI.Id
		INNER JOIN Packages PK (NOLOCK)  ON PK.Id= MD.PackageId
		INNER JOIN InquiryPointCategory IPC (NOLOCK)  ON IPC.Id = PK.InqPtCategoryId
		INNER JOIN M_InvoiceNumberSeries MN(NOLOCK) ON MN.InquiryPointId = PK.InqPtCategoryId
		--INNER JOIN RVN_DealerPackageFeatures AS RVN(NOLOCK) ON RVN.TransactionId = MGI.TransactionId
		INNER JOIN DCRM_SalesDealer AS DSD(NOLOCK) ON DSD.TransactionId = MGI.TransactionId AND (DSD.Id = MD.SalesDealerId OR MD.SalesDealerId IS NULL) AND DSD.PitchingProduct = MD.PackageId-- Added by Amit Yadav to remove repeative products from Invoice Slip.
		INNER JOIN DCRM_PaymentTransaction AS DPT(NOLOCK) ON DPT.TransactionId = DSD.TransactionId
	WHERE
		MGI.ID = @InvoiceId 
	ORDER BY 
		MD.ProductInvoiceAmount DESC

	--get unused invoice numbers
	SELECT 
		MGI.InvoiceNumber +' ( '+ CONVERT(VARCHAR(25),MGI.InvoiceDate,106) +' )'AS InvoiceNumber,
		MGI.Id AS InvoiceId,
		MGI.InvoiceDate
	FROM 
		M_GeneratedInvoice MGI(NOLOCK)
		INNER JOIN M_InvoiceNumberSeries MN(NOLOCK) ON MGI.InvoiceSeriesId = MN.Id
	WHERE
		MGI.Status = 4 
		AND MN.GroupId = @InvoiceSeriesGroupId
		AND ((MONTH(UpdatedOn) BETWEEN 3 AND 12  AND YEAR(UpdatedOn) = YEAR(GETDATE())) OR (MONTH(UpdatedOn) BETWEEN 1 AND 3 AND YEAR(UpdatedOn) = (YEAR(GETDATE())) - 1) )
	ORDER BY
		MGI.InvoiceDate DESC
END



-------------------------------------------------------------------------------------------------------------------------------------------


--------------------------------------------------------------------- sunil yadav : to save suspended dealer log-------------------------------------------

SET ANSI_NULLS ON
