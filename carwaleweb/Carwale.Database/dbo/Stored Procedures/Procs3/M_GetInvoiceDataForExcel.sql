IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[M_GetInvoiceDataForExcel]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[M_GetInvoiceDataForExcel]
GO

	
-- =============================================
-- Author:		<Amit Yadav>
-- Create date: <7th August,2015>
-- Description:	<Get the data that has to be printed on the excel sheet>
--Modified by: Vaibhav K 12-feb-2016 include dealer classification also from dealers table (group/multi outlet/outlet)
--Modified By:Komal Manjare(22-06-2016)
-- desc:applicationId filter for excel data
-- Modified By : Sunil M. Yadav On 22nd june 2016, get krishiKalyanTax parameter while generating invoice link.
-- =============================================
CREATE PROCEDURE [dbo].[M_GetInvoiceDataForExcel] 

	@InvoiceStatus	SMALLINT = NULL,
	@DealerId         INT      = NULL,
	@ProductId        INT      = NULL,
	@DealerTypeId     INT      = NULL,
	@StateId          INT      = NULL,
	@CityId           INT      = NULL,
	@TransactionId    INT      = NULL,
	@FromDate		DATETIME  =  NULL,
	@ToDate			DATETIME  =  NULL,
	@ApplicationId	INT	=	NULL       --Komal Manjare(22-06-2016) applicationId filter for excel data
AS
BEGIN
	SELECT
		DISTINCT 
		DPT.TransactionId,
		MGI.InvoiceName,
		DPT.FinalAmount,
		MGI.InvoiceAmount,
		MIS.Name AS InvoiceStatus,
		ISNULL(MGI.InvoiceNumber,'') AS InvoiceNumber,
		MGI.Comments, 
		CONVERT(VARCHAR,MGI.EntryDate,106) AS GenerateOn,
		OU.UserName AS GeneratedBy,
		PK.Name AS Package,
		DSD.ClosingAmount As OriginalProductAmount,
		ISNULL(MD.ProductInvoiceAmount,0) AS ProductInvoiceAmount,
		ISNULL(CASE WHEN PK.InqPtCategoryId IN (24,32) THEN ISNULL(DSD.NoOfLeads,0) WHEN PK.InqPtCategoryId IN (33,37) THEN ISNULL(DSD.Quantity,0) ELSE ISNULL(DSD.PitchDuration,0) END,0) AS ProductQuantity,
		ISNULL(MD.Quantity,0) AS InvoiceQuantity,
		CASE WHEN PK.InqPtCategoryId IN (24,32) THEN 'Leads' WHEN PK.InqPtCategoryId IN (37,33) THEN 'Quantity' ELSE 'Days' END AS ProductType,
		DSD.PitchingProduct As PendingQuantity,
	
		CONVERT(VARCHAR(100),'http://opr.carwale.com/m/ProductInvoice.aspx?invId=')+CONVERT(VARCHAR,MGI.Id)+CONVERT(VARCHAR,'&dlrId=')
		+ CONVERT(VARCHAR,D.ID)+CONVERT(VARCHAR,'&isEarly=')+CONVERT(VARCHAR,DATEDIFF(day,MGI.InvoiceDate,'06-01-15'))+CONVERT(VARCHAR,'&invSerGrpId=')
		+ CONVERT(VARCHAR,MN.GroupId)+CONVERT(VARCHAR,'&cleanMission=')
		+ CASE MGI.IsCleanMissionManual WHEN 1 THEN '1' ELSE CONVERT(VARCHAR,ISNULL(DATEDIFF(day,'11-14-15',DPT.CreatedOn),'1')) END
		+ CONVERT(VARCHAR,'&krishiKalyanTax=') 
		+ CASE MGI.IsKrishiKalyanTaxManual  WHEN 1 THEN '1' ELSE CONVERT(VARCHAR,ISNULL(DATEDIFF(day,'2016-06-06',DPT.CreatedOn),'1')) END  AS ViewInvoice		-- Sunil M. Yadav On 22nd june 2016, get krishiKalyanTax parameter while generating invoice link.
		,CASE WHEN ISNULL(D.IsGroup,0) = 1 THEN 'Group' WHEN ISNULL(D.IsMultiOutlet,0) = 1 THEN 'MultMultiOutlet-MasterDealer' ELSE 'Outlet' END Classification --Addded by Vaibhav K 12-feb-2016 dealer classification(group/multi outlet/outlet)			
	FROM 
		M_GeneratedInvoice MGI(NOLOCK)
	INNER JOIN M_InvoiceStatus MIS(NOLOCK) ON MIS.Id = MGI.Status
	INNER JOIN M_GeneratedInvoiceDetail MD(NOLOCK) ON MGI.Id = MD.InvoiceId
	INNER JOIN M_InvoiceNumberSeries MN(NOLOCK) ON MGI.InvoiceSeriesId = MN.Id
	INNER JOIN DCRM_PaymentTransaction DPT(NOLOCK) ON DPT.TransactionId = MGI.TransactionId
	INNER JOIN Packages PK(NOLOCK) ON PK.Id = MD.PackageId
	INNER JOIN DCRM_SalesDealer DSD(NOLOCK) ON DSD.TransactionId = MGI.TransactionId AND DSD.PitchingProduct = MD.PackageId
	INNER JOIN Dealers D(NOLOCK) ON DSD.DealerId = D.ID
	INNER JOIN OprUsers OU(NOLOCK) ON OU.Id = MGI.GeneratedBy

	WHERE
		(@InvoiceStatus IS NULL OR MGI.Status = @InvoiceStatus) AND
		(@DealerId IS NULL OR DSD.DealerId=@DealerId )AND
		(@ProductId IS NULL OR MD.PackageId=@ProductId) AND 
		(@DealerTypeId IS NULL OR D.TC_DealerTypeId = @DealerTypeId) AND
		(@StateId IS NULL OR D.StateId = @StateId) AND
		(@CityId IS NULL OR D.CityId = @CityId) AND
		(@TransactionId IS NULL OR MGI.TransactionId=@TransactionId) AND
	    (@FromDate IS NULL OR (CASE WHEN @InvoiceStatus = 2 THEN MGI.InvoiceDate ELSE MGI.EntryDate END) BETWEEN @FromDate AND @ToDate) AND
		(@ApplicationId IS NULL OR D.ApplicationId = @ApplicationId) --Komal Manjare(22-06-2016) applicationId filter for excel data
		
END
