IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[M_GetProductTypeForInvoice]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[M_GetProductTypeForInvoice]
GO

	 
-- =============================================
-- Author	:	Sachin Bharti(11th Feb 2015)
-- Description	:	Get product type for invoicing
-- Modifier	:	Sachin Bharti(25th May 2015)
-- Purpose	:	Showing quantity for RSA and warranty based product
-- Modifier	:	Sachin Bharti(8th June 2015)
-- Purpose	:	Get no of leads/days and rsa ,warranty quantity from 
--				M_GeneratedInvoiceDetail instead of DCRM_SalesDealer table
-- Modifier :	Amit Yadav(16th Feb 2016)
-- Purpoes	:	To get the CPL for the products.
-- Execute [dbo].[M_GetProductTypeForInvoice] 1424
-- =============================================
CREATE PROCEDURE [dbo].[M_GetProductTypeForInvoice]
	
	@InvoiceId		INT

AS
BEGIN
	SELECT 
		DISTINCT MID.Id AS InvDetailId,
		CASE	WHEN PK.InqPtCategoryId IN (24,32) THEN 'Leads' WHEN PK.InqPtCategoryId IN (37,33) THEN 'Quantity' ELSE 'Duration(Days)' END AS ProductType,
		CASE	WHEN PK.InqPtCategoryId IN (24,32) THEN (CASE WHEN MID.Quantity IS NULL THEN  ISNULL(DSD.NoOfLeads,0) ELSE MID.Quantity END)
				WHEN PK.InqPtCategoryId IN (37,33) THEN (CASE WHEN MID.Quantity IS NULL THEN  ISNULL(DSD.Quantity,0) ELSE MID.Quantity END)
				ELSE (CASE WHEN MID.Quantity IS NULL THEN DSD.PitchDuration ELSE MID.Quantity END ) END AS PitchDuration,
		DSD.PitchingProduct AS PkgId,
		CASE WHEN DSD.ContractType Is NOT NULL AND DSD.NoOfLeads IS NOT NUll THEN
         (CASE WHEN DSD.ContractType=1 AND DSD.NoOfLeads>0
           THEN  CAST((CAST(DSD.ClosingAmount AS FLOAT)/CAST(DSD.NoOfLeads AS FLOAT)) AS NUMERIC(18,0)) ELSE NULL END
          )
        ELSE  NULL  END  AS CostPerLead
	FROM 
		M_GeneratedInvoiceDetail MID(NOLOCK) 
		INNER JOIN M_GeneratedInvoice MG(NOLOCK) ON MG.Id = MID.InvoiceId AND MID.InvoiceId = @InvoiceId
		INNER JOIN DCRM_SalesDealer DSD(NOLOCK) ON DSD.TransactionId = MG.TransactionId AND DSD.PitchingProduct = MID.PackageId AND MG.Id = @InvoiceId
		INNER JOIN Packages PK(NOLOCK) ON PK.Id = DSD.PitchingProduct 
END
