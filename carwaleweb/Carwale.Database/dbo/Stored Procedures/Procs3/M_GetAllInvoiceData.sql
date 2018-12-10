IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[M_GetAllInvoiceData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[M_GetAllInvoiceData]
GO

	
-- =============================================
-- Author	:	Sachin Bharti(14th April 2014)
-- Description	: Get all invoice data
-- Modifier	:	Sachin Bharti(29th May 2015)
-- Purpose	:	Get comments column to display
-- Modifier	:	Sachin Bharti(1st June 2015)
-- Purpose	:	Add InvoiceDayDiff to identify is invoice generated
--				before 1st june or after
-- Modifier :Ajay Singh(24 july 2015)         :
-- execute [dbo].[M_GetAllInvoiceData] null,52
-- Modifier    : Vinay Kumar prajapati 
-- Purpose  : According to CleanIndiaMission(Swacch Bharat Abhiyan 0.5% cess(Extra tax) is payable after 15th November)
-- Modifier : Amit Yadav
-- Purpose : Added parameters @DelFromDate and  @DelToDate. To filter data according to DeliveryStartDate.
-- Modified By : Kartik Rathod on 24 dec 2015,fetch new column for Lpa Image link As LPAHostUrl,LPAOriginalImgPath
-- Modifier : Amit Yadav(31st Dec 2015)
-- Purpose : Added parameters @ApplicationId to filter data w.r.t. application type.
-- Modified By : Kritika Choudhary on 4th jan 2015, added join with M_AttachedLpaDetails and modified lpa hosturl and originalimgpath
-- Modified By : Kritika Choudhary on 6th jan 2015, added columns ChequeHostUrl,ChequeOriginalImgPath,DepSlipHostUrl and DepSlipOriginalImgPath
-- Modified by kritika Choudhary on 7th Jan 2015, removed join with M_AttachedLpaDetails and DCRM_PaymentDetails
-- EXEC [M_GetAllInvoiceData] 
--Modified by: Vaibhav K 12-feb-2016 include dealer classification also from dealers table (group/multi outlet/outlet)
-- Modified By:Komal Manjare(02-06-2016) Fetch KrishiKalyanTax from M_GenerateInvoice
-- =============================================
CREATE PROCEDURE [dbo].[M_GetAllInvoiceData] 
	@InvoiceStatus	SMALLINT = NULL,
	@DealerId         INT      = NULL,
	@ProductId        INT      = NULL,
	@DealerTypeId     INT      = NULL,
	@StateId          INT      = NULL,
	@CityId           INT      = NULL,
	@TransactionId    INT      = NULL,
	@FromDate		DATETIME  =  NULL,
	@ToDate			DATETIME  =  NULL,
	@DelFromDate	DATETIME =	NULL,
	@DelToDate		DATETIME = NULL,
	@ApplicationId	INT	=	NULL
AS
BEGIN

	SELECT
		DISTINCT MGI.Id AS InvoiceId,
		DPT.TransactionId,
		D.ID AS DealerId,
		DPT.ProductAmount,
		MIS.Name AS InvoiceStatus,
		MGI.InvoiceAmount,
		MGI.InvoiceName,
		MN.GroupId AS InvSeriesGrpId,
		CONVERT(VARCHAR,MGI.InvoiceDate,106) AS InvoiceDate,
		MGI.Comments,
		ISNULL(MGI.InvoiceNumber,'') AS InvoiceNumber,
		CONVERT(VARCHAR,MGI.EntryDate,106) AS GenerateDate,
		CASE	WHEN @InvoiceStatus = 1 THEN MGI.EntryDate  
				WHEN @InvoiceStatus = 2 THEN MGI.InvoiceDate
				ELSE MGI.RejectedDate END AS InvDate,
		DATEDIFF(day,MGI.InvoiceDate,'06-01-15') AS InvoiceDayDiff,--invoices before 1st june 12.36%tax
		OU.UserName AS GeneratedBy,
		--ISNULL(DATEDIFF(day,'11-14-15',DPT.CreatedOn),1) AS cleanIndiaMission, -- Added By Vinay Kumar prajapati commented on 29th desc 2015		
		CASE MGI.IsCleanMissionManual WHEN 1 THEN 1 ELSE  ISNULL(DATEDIFF(day,'11-14-15',DPT.CreatedOn),1) END  AS cleanIndiaMission -- Added By Vinay Kumar prajapati 29th Desc 2015 
		
		,CASE WHEN ISNULL(D.IsGroup,0) = 1 THEN 'Group' WHEN ISNULL(D.IsMultiOutlet,0) = 1 THEN 'MultiOutlet-MasterDealer' ELSE 'Outlet' END Classification --Addded by Vaibhav K 12-feb-2016 dealer classification(group/multi outlet/outlet)
		,CASE MGI.IsKrishiKalyanTaxManual  WHEN 1 THEN 1 ELSE  ISNULL(DATEDIFF(day,'06-06-16',DPT.CreatedOn),1) END  AS KrishiKalyanTax --Added By:Komal Manjare(02-05-2016) KrishiKalyanTax flag fetched.
		,D.ApplicationId
	FROM 
		M_GeneratedInvoice MGI(NOLOCK)
		INNER JOIN M_GeneratedInvoiceDetail MD(NOLOCK) ON MGI.Id = MD.InvoiceId
		INNER JOIN M_InvoiceNumberSeries MN(NOLOCK) ON MGI.InvoiceSeriesId = MN.Id
		INNER JOIN DCRM_PaymentTransaction DPT(NOLOCK) ON DPT.TransactionId = MGI.TransactionId
		INNER JOIN DCRM_SalesDealer DSD(NOLOCK) ON MGI.TransactionId = DSD.TransactionId
		INNER JOIN Dealers D(NOLOCK) ON DSD.DealerId = D.ID
		INNER JOIN M_InvoiceStatus MIS(NOLOCK) ON MIS.Id = MGI.Status
		INNER JOIN OprUsers OU(NOLOCK) ON OU.Id = MGI.GeneratedBy
		
	WHERE
		(@InvoiceStatus IS NULL OR MGI.Status = @InvoiceStatus) AND
		(@DealerId IS NULL OR DSD.DealerId=@DealerId )AND
		(@ProductId IS NULL OR MD.PackageId=@ProductId) AND 
		(@DealerTypeId IS NULL OR D.TC_DealerTypeId=@DealerTypeId) AND
		(@StateId IS NULL OR D.StateId=@StateId) AND
		(@CityId IS NULL OR D.CityId=@CityId) AND
		(@TransactionId IS NULL OR MGI.TransactionId=@TransactionId) AND
	    (@FromDate IS NULL OR (CASE WHEN @InvoiceStatus = 2 THEN MGI.InvoiceDate ELSE MGI.EntryDate END) BETWEEN @FromDate AND @ToDate) AND
		(@ApplicationId IS NULL OR D.ApplicationId = @ApplicationId)-- Added by Amit Yadav to filter data w.r.t. application type.
	ORDER BY 
		InvDate DESC

	--get all generated invoice product,amount and quantity detail
	SELECT
		MD.InvoiceId ,
		MGI.InvoiceDate AS InvDate,
		PK.Name AS Package,
		DSD.ClosingAmount,
		ISNULL(MD.ProductInvoiceAmount,0) AS ProductInvoiceAmount,
		ISNULL(CASE WHEN PK.InqPtCategoryId IN (24,32) THEN ISNULL(DSD.NoOfLeads,0) WHEN PK.InqPtCategoryId IN (33,37) THEN ISNULL(DSD.Quantity,0) ELSE ISNULL(DSD.PitchDuration,0) END,0) AS ProductQuantity,
		ISNULL(MD.Quantity,0) AS InvoiceQuantity,
		CASE WHEN PK.InqPtCategoryId IN (24,32) THEN 'Leads' WHEN PK.InqPtCategoryId IN (37,33) THEN 'Quantity' ELSE 'Days' END AS ProductType,
		CONVERT(VARCHAR,RVN.PackageStartDate,106) AS DeliveryStartDate,
		CONVERT(VARCHAR,RVN.PackageEndDate,106) AS DeliveryEndDate,MGI.TransactionId
	FROM 
		M_GeneratedInvoiceDetail MD(NOLOCK)
		INNER JOIN M_GeneratedInvoice MGI(NOLOCK) ON MGI.Id = MD.InvoiceId
		INNER JOIN DCRM_SalesDealer DSD(NOLOCK) ON DSD.TransactionId = MGI.TransactionId AND DSD.PitchingProduct = MD.PackageId
		INNER JOIN Dealers D(NOLOCK) ON D.ID = DSD.DealerId
		INNER JOIN Packages PK(NOLOCK) ON PK.Id = MD.PackageId
		INNER JOIN InquiryPointCategory IPC(NOLOCK) ON IPC.Id = PK.InqPtCategoryId
		LEFT JOIN RVN_DealerPackageFeatures AS RVN WITH(NOLOCK) ON RVN.TransactionId = MGI.TransactionId AND MD.PackageId = RVN.PackageId

	WHERE
		(@InvoiceStatus IS NULL OR MGI.Status = @InvoiceStatus)AND
		(@DealerId IS NULL OR DSD.DealerId=@DealerId )AND
		(@ProductId IS NULL OR MD.PackageId=@ProductId) AND
		(@DealerTypeId IS NULL OR D.TC_DealerTypeId=@DealerTypeId) AND
		(@StateId IS NULL OR D.StateId=@StateId) AND
		(@CityId IS NULL OR D.CityId=@CityId) AND
	    (@DelFromDate IS NULL OR RVN.PackageStartDate BETWEEN @DelFromDate AND @DelToDate) 

	ORDER BY
		InvDate DESC
		
END
-------------------------------------------------------------------------------------------------------------------------------------


