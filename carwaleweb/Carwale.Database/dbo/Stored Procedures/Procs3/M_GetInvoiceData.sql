IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[M_GetInvoiceData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[M_GetInvoiceData]
GO

	-- =============================================
-- Author	:	Sachin Bharti(7th Feb 2015)
-- Description	:	Get all invoice data for that Dealer only
-- Modifier	:	Sachin Bharti(1st June 2015)
-- Purpose	:	Add InvoiceDayDiff to identify is invoice generated
--				before 1st june or after
-- Modifier :	Sachin Bharti(5th June 2015)
-- Purpose	:	Invoice approval date should be Invoice Date
-- Execute [dbo].[M_GetInvoiceData] 5
--- Updated by : Vinay Kumar Prajapati Add cleanIndiaMission (To display Swachh Bharat Cess(0.5))
-- Modified By : Komal Manjare on 24 dec 2015 , added condition for api in filter case
-- @State =null (All data)
-- @State = 0 (Rejected)
-- @State = 1 (accepted)
-- @State = 2 (pending)
-- Updated By vinay Kumar Prajapati 29th Desc 2015 Show  clean india cess 
-- Modified By:Komal Manjare(07-06-2016)
-- Changes for krshi kalyan tax to be added
-- =============================================
CREATE PROCEDURE [dbo].[M_GetInvoiceData] 
	
	 @DealerId	INT
	,@State SMALLINT =null
	,@TransactionId INT=null
	
AS
BEGIN
	DECLARE @InvoiceNo INT = 0
	SELECT
		DISTINCT MGI.Id AS InvoiceId,
		@DealerId AS DealerId,
		MGI.TransactionId,
		MGI.InvoiceName,
		MGI.InvoiceAmount,
		CASE WHEN MGI.InvoiceNumber IS NULL THEN 'Invoice' ELSE MGI.InvoiceNumber END AS InvoiceNumber,
		REPLACE(REPLACE(CONVERT(VARCHAR,MGI.InvoiceDate,106), ' ','-'), ',','') AS InvoiceDate,
		DATEDIFF(day,MGI.EntryDate,'06-01-15') AS InvoiceDayDiff,--invoices before 1st june 12.36%tax
		--CONVERT(VARCHAR,MGI.InvoiceDate,100) AS InvoiceDate1,
		CASE	
			WHEN ISNULL(MGI.Status,2) = 1 THEN 'Pending'
			WHEN ISNULL(MGI.Status,2) = 2 THEN ''
			WHEN (ISNULL(MGI.Status,2) = 3 OR ISNULL(MGI.Status,2) = 4) THEN 'Rejected'
		END AS InvoiceStatus,
		MGI.Status AS StatusId,
		CASE	
			WHEN ISNULL(MGI.Status,2) = 2 THEN '' ELSE 'hide'
		END AS IsPrintable,
		CASE MGI.IsCleanMissionManual WHEN 1 THEN 1 ELSE  ISNULL(DATEDIFF(day,'11-14-15',DPT.CreatedOn),1) END  AS cleanIndiaMission -- Added By Vinay Kumar prajapati 
		,CASE MGI.IsKrishiKalyanTaxManual  WHEN 1 THEN 1 ELSE  ISNULL(DATEDIFF(day,'06-06-16',DPT.CreatedOn),1) END  AS KrishiKalyanTax --Added By:Komal Manjare(07-05-2016) KrishiKalyanTax flag fetched.
		
	FROM
		M_GeneratedInvoice MGI(NOLOCK) 
		INNER JOIN DCRM_SalesDealer DSD(NOLOCK) ON DSD.TransactionId = MGI.TransactionId AND DSD.DealerId = @DealerId
		INNER JOIN Dealers D(NOLOCK) ON D.ID=DSD.DealerId
		LEFT JOIN DCRM_PaymentTransaction DPT(NOLOCK) ON DPT.TransactionId = MGI.TransactionId
	WHERE
		((@State IS NULL AND MGI.Status IN (1,2,3,4))--include only Pending,Approved,Rejected and ApprovedRejected invoices
		OR 
		(@State = 1 AND MGI.Status = 2)
		OR
		(@State = 0 AND  MGI.Status IN (3,4))
		OR 
		(@State = 2 AND MGI.Status = 1 )) AND
		(@TransactionId IS NULL OR DPT.TransactionId = @TransactionId)
	ORDER BY
		CASE WHEN MGI.InvoiceNumber IS NULL THEN 'Invoice' ELSE MGI.InvoiceNumber END DESC

END

------------------------------------------------------------------------------------------------------------------------------

