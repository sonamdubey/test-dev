IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[RVN_GetAllCompletedTransactions]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[RVN_GetAllCompletedTransactions]
GO

	

-- =============================================
-- Author	:	Sachin Bharti(24th April 2015)
-- Description	:	Get all completed transaction details for 
--					account approval process
-- Modifier :	Sachin Bharti(24 July 2015)
-- Purpose	:	Add new parameter @IsApproved for getting those transactions 
--				on which no action has been taken yet.
-- execute [RVN_GetAllCompletedTransactions] null,null,null,null,34,null,null,null
-- Modifier :	Amit Yadav(12th August 2015)
-- Purpose	:	Added LinkReport.
-- Modifier :	Amit Yadav(18th Nov 2015)
-- Purpose	:	Added @CheckDDPdcNumber.
-- Modifier :	Amit Yadav(2nd Dec 2015)
-- Purpose	:	To get the column 'transId' to bind it behind the delete button on CompletedTransaction page.
-- Modifier :	Kritika Choudhary (3rd Dec 2015)
-- Purpose	:	added join condition with RVN_DealerPackageFeatures
-- Modifier :	Kritika Choudhary (15th Dec 2015)
-- Purpose	:   added parameter IsPendingDelivery and NoApprovals and condition in where clause, also added join condition with TC_ContractCampaignMapping
-- Modifier :	Amit Yadav(28th Dec 2015)
-- Purpose	:	Added parameter @DepStartDate and @DepEndDate to filter data w.r.t. DepositDate.
-- Modified By : Kartik Rathod on 24 Dec 2015, --fetch new column for Lpa Image link As LPAHostUrl,LPAOriginalImgPath
-- Modifier : Amit Yadav(31st Dec 2015)
-- Purpose : Added parameters @ApplicationId to filter data w.r.t. application type.
-- Modified By : Kritika Choudhary on 4th jan 2015, added join with M_AttachedLpaDetails and modified lpa hosturl and originalimgpath
-- Modified By : Kritika Choudhary on 6th jan 2015, added columns ChequeHostUrl,ChequeOriginalImgPath,DepSlipHostUrl and DepSlipOriginalImgPath
--Modified by : Kritika Choudhary on 7th jan 2015, removed join with M_AttachedLpaDetails and host url , originalimgpath for lpa,cheque and depslip
--Modified by: Kritika Choudhary on 13th jan 2016, added Associationtype as parameter and condition in where clause
--Modified by : Vaibhav K 22-feb-2016 added dealer classification as - (outler/multi outlet/group)
-- =============================================
CREATE PROCEDURE [dbo].[RVN_GetAllCompletedTransactions]
	
	@TransactionId	INT = NULL,
	@DealerId		INT = NULL,
	@FromDate		DATETIME = NULL,
	@ToDate			DATETIME = NULL,
	@ProductId        INT    = NULL,
	@DealerTypeId     INT     = NULL,
	@StateId          INT     = NULL,
	@CityId           INT     = NULL,
	@IsApproved		  INT = NULL,
	@PaymentMode	  INT=NULL,
	@CheckDDPdcNumber VARCHAR(50)= NULL,
	@IsPendingDelivery	BIT=NULL,
	@NoApprovals  BIT=NULL,
	@DepStartDate	DATETIME = NULL,-- To filter data w.r.t. DepositDate
	@DepEndDate		DATETIME = NULL,
	@ApplicationId	INT		= NULL,
	@AssociationType INT=NULL
AS
BEGIN
	SELECT 
		DISTINCT D.ID AS DealerId,D.Organization,C.Name AS City,TDT.DealerType,DPT.FinalAmount AS FinalProductAmount,
		(SELECT SUM(DPD.Amount) FROM DCRM_PaymentDetails DPD(NOLOCK) WHERE DPD.TransactionId = DPT.TransactionId) AS CollectedProductAmount,
		(DPT.FinalAmount - (SELECT SUM(DPD.Amount) FROM DCRM_PaymentDetails DPD(NOLOCK) WHERE DPD.TransactionId = DPT.TransactionId)) AS CollectionPendingProductAmount,
		ISNULL((SELECT SUM(MGI.InvoiceAmount) FROM M_GeneratedInvoice MGI(NOLOCK) WHERE MGI.TransactionId = DPT.TransactionId AND MGI.Status = 2),0) AS InvoiceProductAmount,

		
		CONVERT(VARCHAR,(SELECT TOP 1  CASE WHEN (DPD.Mode IN(2,3,8) AND DPD.UpdatedOn IS NOT NULL) THEN DPD.UpdatedOn ELSE DPD.AddedOn END  FROM DCRM_PaymentDetails DPD(NOLOCK) WHERE DPD.TransactionId = DPT.TransactionId ORDER BY UpdatedOn DESC),106) AS UpdatedOn,

		(SELECT TOP 1  CASE WHEN (DPD1.Mode IN(2,3,8) AND DPD1.UpdatedOn IS NOT NULL) THEN DPD1.UpdatedOn ELSE DPD1.AddedOn END  FROM DCRM_PaymentDetails DPD1(NOLOCK) WHERE DPD1.TransactionId = DPT.TransactionId ORDER BY UpdatedOn DESC) AS UpdatedOn1,
		
		(DPT.FinalAmount - ISNULL((SELECT SUM(MGI.InvoiceAmount) FROM M_GeneratedInvoice MGI(NOLOCK) WHERE MGI.TransactionId = DPT.TransactionId AND MGI.Status = 2),0))AS InvoicePendingProductAmount,

		OU.UserName AS UpdatedBy,

		DPT.TransactionId,
		

		CONVERT(VARCHAR(100),'http://opr.carwale.com/Accounts/ApprovePayment.aspx?trId=')+CONVERT(VARCHAR,DPT.TransactionId)+CONVERT(VARCHAR,'&dlrId=')+CONVERT(VARCHAR,D.ID) AS LinkReport

		, CASE ISNULL(DPF.DealerPackageFeatureID,0) WHEN 0 THEN (CASE ISNULL((SELECT TOP 1 Id FROM M_GeneratedInvoice GI WITH (NOLOCK) WHERE GI.TransactionId = DPT.TransactionId),0) WHEN 0 THEN 1 ELSE 0 END) ELSE 0 END CanDelete--Transactions which are not yet approved ,invoice not generated,delivery not started can be deleted.
		
		,CASE WHEN ISNULL(D.IsGroup,0) = 1 THEN 'Group' WHEN ISNULL(D.IsMultiOutlet,0) = 1 THEN 'MultiOutlet-MasterDealer' ELSE 'Outlet' END AS Classification --Addded by Vaibhav K 22-feb-2016 dealer classification(group/multi outlet/outlet)
	FROM 
		DCRM_PaymentTransaction DPT(NOLOCK)
		INNER JOIN DCRM_SalesDealer DSD(NOLOCK) ON DSD.TransactionId = DPT.TransactionId
		INNER JOIN Dealers D(NOLOCK) ON D.ID = DSD.DealerId
		INNER JOIN Cities C(NOLOCK) ON C.ID = D.CityId
		INNER JOIN OprUsers OU(NOLOCK) ON OU.Id = DPT.CreatedBy
		INNER JOIN TC_DealerType TDT(NOLOCK) ON TDT.TC_DealerTypeId = D.TC_DealerTypeId
		INNER JOIN DCRM_PaymentDetails DPD(NOLOCK) ON DPD.TransactionId = DPT.TransactionId
		LEFT JOIN RVN_DealerPackageFeatures DPF (NOLOCK) ON DPF.TransactionId = DPD.TransactionId
		LEFT JOIN TC_ContractCampaignMapping CCM (NOLOCK) ON DPF.DealerPackageFeatureID = CCM.ContractId
		--LEFT JOIN M_AttachedLpaDetails LPAD (NOLOCK) ON LPAD.SalesDealerId = DSD.Id
	WHERE
		(@TransactionId IS NULL OR DPT.TransactionId = @TransactionId)
		AND (@DealerId IS NULL OR D.ID = @DealerId)
		AND (@FromDate IS NULL OR DPT.CreatedOn BETWEEN @FromDate AND @ToDate)
		AND(@ProductId IS NULL OR DSD.PitchingProduct=@ProductId)
		AND (@DealerTypeId IS NULL OR D.TC_DealerTypeId=@DealerTypeId)
	    AND(@StateId IS NULL OR D.StateId=@StateId)
		AND(@CityId IS NULL OR D.CityId=@CityId) 
		AND(@IsApproved IS NULL OR(DPF.IsActive=1 AND DPD.IsApproved IS NULL AND CCM.CampaignId IS NOT NULL)) --changed the condition(Modifier: Kritika Choudhary)
		--AND (@IsPendingDelivery IS NULL OR( DPF.IsActive=1 AND DPD.IsApproved IS NOT NULL AND CCM.CampaignId IS NULL)) --added by Kritika Choudhary/--Commented by Ajay Singh(3 feb 2016)
		AND (@IsPendingDelivery IS NULL OR (DPD.IsApproved = 1 AND DPF.PackageStatus!=4 ))--Added  by Ajay Singh(3 feb 2016)
		--AND (@NoApprovals IS NULL OR(DPF.IsActive=1 AND CCM.CampaignId IS NULL AND DPD.IsApproved IS NULL)) --added by Kritika Choudhary//--Commented by Ajay Singh(3 feb 2016)
		AND (@NoApprovals IS NULL OR (DPD.IsApproved IS NULL AND DPF.PackageStatus IS NULL)) --Added  by Ajay Singh(3 feb 2016)
		AND(@PaymentMode IS NULL OR DPD.Mode=@PaymentMode)
		AND(@CheckDDPdcNumber IS NULL OR DPD.CheckDDPdcNumber=@CheckDDPdcNumber)
		AND DPT.IsActive=1
		AND (@DepStartDate IS NULL OR @DepEndDate IS NULL OR CONVERT(DATE,DPD.DepositedDate) BETWEEN CONVERT(DATE,@DepStartDate) AND CONVERT(DATE,@DepEndDate)) --Added by Amit Yadav to filter w.r.t. DepositDate
		AND (@ApplicationId IS NULL OR D.ApplicationId = @ApplicationId)	-- Added by Amit Yadav to filter data w.r.t. application type.	
	    AND (@AssociationType IS NULL OR DPF.CampaignType = @AssociationType)
	ORDER BY 
		UpdatedOn1 DESC
END

