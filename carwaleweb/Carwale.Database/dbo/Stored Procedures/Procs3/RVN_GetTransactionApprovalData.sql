IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[RVN_GetTransactionApprovalData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[RVN_GetTransactionApprovalData]
GO

	-- =============================================
-- Author	:	Sachin Bharti(28th April 2015)
-- Description	:	Get data for transaction approval page.
-- Updated By : Vinay Kumar Prajapati 10 Nov 2015 To add New column (Approved Lead And DeliverLead) in 2nd Table 
-- Modified By : Kartik Rathod on 26 Nov 2015, fetched Comments
-- Modified By : Ajay Singh on 26 Nov,fetch Model,Exception Model,LeadPerDay
-- Modified By : Kartik Rathod on 3 Dec 2015, fetch IsMultiOutlet for Multi Outlet Feature for ApproveCampaign  PAge
-- Modified By : Vaibhav K 7-Dec-2015 In collection details added new fields for Payment Rejected Reason
-- Modified by : Kritika CHoudhary on 16th Dec 2015, changed the condition of where clause in get collection details
-- Modified By : Vinay Kumar Parajapati on 29th desc 2015  show data for swacch bharat cess.
--Modified By  : Ajay Singh on 4 jan 2016 to get more data related to dealer
-- Modified By : Sunil Yadav On 05 Jan 2016 
-- Description : get state name,city Id in dealer details 
--				 validity, amount, isTopUp , InquiryPoints , GroupType ,InquiryPointCategory,DealerPackageFeatureID,IsPackageApproved In get product details
--Modified By : Komal Manjare on 6 Jan 2016 to get pitchingduration and closingamount
--				EXEC RVN_GetTransactionApprovalData 9,11174
--Modified by : Kritika Choudhary on 7th Jan 2016,added join with M_AttachedLpaDetailsin select query for  product details and added host url and originalimgpath for cheque and depslip in select query for collection details
-- Modified by : Sunil Yadav on 10th Jan 2016 
-- Description : CASE On packageId to check UCD inquiry points.
--Modified By:Komal Manjare(14-january-2016) 
--Desc-to get cpl, associationtype and depositedate
--Modified By:Komal Manjare(30-January-2015) 
--Desc-to get issuerBankName,product status contract Brhaviour,contractstartdate and contractType
--EXEC RVN_GetTransactionApprovalData 3797,11289
--Modified By:Sunil Yadav(08-february-2016)
--Desc-get Ismulti-outlet or group
-- Modifier : Mihir A Chheda On 3-May-2016
--desc--isactive column fetch with lpa image details
--Modified By  : Kartik Rathod on 2 Jun 2016, fetch Comments For Payments Details
--Modified By:Komal Manjare(20-6-2016) get I/O salesID
--Modified By:Komal Manjare(29-June-2016) get KrishiKalyan flag for approved invoices
--Modified By :Mihir Chheda[10-08-2016] get lap number from LPANumber Column instead of salesid
-- =============================================
CREATE PROCEDURE [dbo].[RVN_GetTransactionApprovalData]
	@DealerId	INT,
	@TransactionId	INT

AS
BEGIN
	--get dealer details 
	SELECT 
		D.ID As DealerId,
		D.Organization ,
		C.Name AS City,
		C.ID,
		S.Name AS State ,
		D.Address1 AS Address,
		D.Pincode,
		D.IsGroup,
		D.IsMultiOutlet
		

	FROM 
		Dealers D(NOLOCK)
		INNER JOIN Cities C(NOLOCK) ON C.ID = D.CityId
		INNER JOIN States S WITH(NOLOCK) ON C.StateId = S.ID
	WHERE
		D.ID = @DealerId

	--get transaction details
	SELECT 
		TOP 1 DPT.TransactionId,
		CONVERT(VARCHAR,DPT.CreatedOn,106) AS CreatedOn,
		CASE WHEN (DPD.Mode IN (2,3,8) AND DPD.UpdatedOn IS NOT NULL) THEN CONVERT(VARCHAR,DPD.UpdatedOn,106) 
				ELSE CONVERT(VARCHAR,DPT.CreatedOn,106) END AS UpdatedOn,
		DPT.TotalClosingAmount,
		DPT.DiscountAmount,
		DPT.FinalAmount,
		PT.Name AS PaymentType,
		(SELECT SUM(DPD.Amount) FROM DCRM_PaymentDetails DPD(NOLOCK) WHERE DPD.TransactionId = DPT.TransactionId AND (DPD.IsApproved=1 OR DPD.IsApproved IS NULL) ) AS TotalCollectedProductAmount, 
		(DPT.FinalAmount -  (SELECT SUM(DPD.Amount) FROM DCRM_PaymentDetails DPD(NOLOCK) WHERE DPD.TransactionId = DPT.TransactionId AND (DPD.IsApproved=1 OR DPD.IsApproved IS NULL)) ) AS TotalPendingProductAmount
	FROM   
		DCRM_PaymentTransaction DPT(NOLOCK)
		INNER JOIN DCRM_PaymentDetails DPD(NOLOCK) ON DPT.TransactionId = DPD.TransactionId
		LEFT JOIN DCRM_PaymentType PT(NOLOCK) ON PT.PaymentTypeId = DPD.PaymentType
	Where 
		DPT.TransactionId = @TransactionId
		ORDER BY PT.PaymentTypeId --ADDED BY AJAY SINGH ON (22 JAN 2015) 

	--get product details
	SELECT 
		DISTINCT DSD.Id AS SalesId,
		DSD.DealerId,
		PK.Name AS Product,
		PK.Id AS ProductId,
		PK.Validity,
		PK.Amount,
		PK.InquiryPoints,
		PK.IsTopup ,
		RVN.DealerPackageFeatureID AS DealerPackageFeatureID , -- to redirect approveCampaign page
		--CPR.Id AS RequestId, -- to redirect to approvePackageRequest page
	--	CPR.isApproved AS IsPackageApproved , -- to check whether package is approved or not.
	--	CPR.ContractId ,
		IPC.GroupType,
		IPC.Name AS InquiryPointCategory,
		DSD.ProductStatus,
		DSD.Quantity,
		DSD.PitchDuration,
		DSD.NoOfLeads,
		DSD.UpdatedOn AS UpdatedOn1,
		DSD.TransactionId,
		OU.UserName AS ClosedBy,				
		CONVERT(VARCHAR,DSD.UpdatedOn,106) AS UpdatedOn,
		PK.InqPtCategoryId,
		ISNULL(TCCM.TotalGoal,'') AS ApprovedLead ,
		ISNULL(TCCM.TotalDelivered,'') AS DeliveredLead  ,

		CASE WHEN PK.InqPtCategoryId IN (24,42,43,44) THEN 1	-- PK.InqPtCategoryId = 24 : new car lead , BikeWale packages : 42,43,44
			 ELSE 0  -- PK.InqPtCategoryId = 47 : Topup on Paid Packages , Pk.id = 96 : Free Group Top=up
			 END AS IsMultiApproval,
		CASE WHEN RVN.DealerPackageFeatureID IS NULL THEN 'show' WHEN RVN.DealerPackageFeatureID IS NOT NULL THEN 'hide' END AS IsApprove,
		CASE WHEN RVN.DealerPackageFeatureID IS NULL THEN 'Approve' WHEN RVN.DealerPackageFeatureID IS NOT NULL THEN 'Approved' END AS IsStarted,
		ISNULL(DSD.Comments ,'-') AS Comments,
		DSD.Model,
		DSD.ExceptionModel,
		DSD.LeadPerDay,
		--ISNULL(DSD.IsMultiOutlet,0) AS IsMultiOutlet,
		CONVERT(VARCHAR,DSD.StartDate,106) AS StartDate,
		DU.Status,--Added By Ajay Singh
		DU.IsTCDealer AS TcDealer,
	    CASE ISNULL(DU.Status,0) WHEN 'False' THEN 1 ELSE 0 END AS IsDealerActive , 
		CASE ISNULL(DU.IsDealerDeleted,0) WHEN 'True' THEN 1 ELSE 0 END AS IsDealerDeleted,
		DU.ApplicationId AS ApplicationId,
		DU.CityId AS CityId,
		DSD.PitchDuration AS ActualValidity,
		DSD.ClosingAmount AS ActualAmount,
		LPAD.HostURL AS LPAHostUrl , LPAD.OriginalImgPath AS LPAOriginalImgPath,LPAD.ID LPAImageId,DCT.CampaignType AS AssociationType,
		--TCCM.CostPerLead,
		--Added by komal manjare 
		 CASE WHEN DSD.ContractType Is NOT NULL AND DSD.NoOfLeads IS NOT NUll THEN
         (CASE WHEN DSD.ContractType=1 AND DSD.NoOfLeads>0
		   THEN  cast((cast(DSD.ClosingAmount as float)/cast(DSD.NoOfLeads as float))  AS NUMERIC(18,0)) ELSE NULL END
		  )
		ELSE  NULL  END  AS CostPerLead,

		DSD.ContractType AS ContractBehaviourType,
		CASE WHEN DSD.CampaignType=3 THEN 1 
			WHEN DSD.CampaignType=4 THEN 2
		    ELSE '' END As ContractType,
		--TCCM.ContractType,
		CASE WHEN DSD.StartDate  IS NOT NULL THEN CONVERT(VARCHAR, DSD.StartDate, 103) ELSE '' END AS ContractStartDate,
		CASE WHEN DSD.ContractType IS NOT NULL THEN
		 (CASE WHEN  DSD.ContractType=1 THEN 'Lead Based' ELSE 'Duration Based' END)
		 ELSE '' END AS ContractBehaviour,DAP.Name AS ProductClassification,
		 CASE WHEN OP.UserName IS NOT NULL THEN OP.UserName ELSE ''END AS ApprovedBy,
		 ISNULL(LPAD.IsActive,1) AS IsLpaImgActive -- Mihir A Chheda On 3-May-2016
		 ,CASE WHEN PK.Id IN(70,59) THEN DSD.LPANumber ELSE NULL END AS IOSalesId -- Mihir Chheda[10-08-2016]

	FROM	
		DCRM_SalesDealer DSD(NOLOCK)
		INNER JOIN Packages PK(NOLOCK) ON PK.Id = DSD.PitchingProduct
		INNER JOIN InquiryPointCategory IPC WITH(NOLOCK) ON IPC.Id = PK.InqPtCategoryId
		INNER JOIN OprUsers OU(NOLOCK) ON OU.Id = DSD.UpdatedBy		
		LEFT JOIN RVN_DealerPackageFeatures RVN(NOLOCK) ON RVN.ProductSalesDealerId = DSD.Id		
		LEFT JOIN  TC_ContractCampaignMapping AS TCCM WITH(NOLOCK) ON  TCCM.ContractId=RVN.DealerPackageFeatureID  --- Vinay Changes
		LEFT JOIN Dealers AS DU WITH(NOLOCK) ON DU.ID=DSD.DealerId --Addded by Ajay Singh
	    LEFT JOIN M_AttachedLpaDetails LPAD (NOLOCK) ON LPAD.SalesDealerId = DSD.Id
		LEFT JOIN DCRM_CampaignType DCT (NOLOCK) ON DCT.Id=DSD.CampaignType
		LEFT JOIN DCRM_ADM_ProductStatus DAP (NOLOCK) ON DAP.ID=DSD.ProductStatus
		LEFT JOIN OprUsers OP(NOLOCK) ON OP.Id =RVN.ApprovedBy
	WHERE
		DSD.TransactionId = @TransactionId
		AND DSD.DealerId = @DealerId
	ORDER BY
		UpdatedOn1 DESC

	--get collection details
	SELECT 
		DPD.ID AS PaymentDetailsId,
		DPD.Amount AS ProductCollectedAmount,
		ISNULL(((DPD.Amount * DPD.ServiceTax)/100),0.0) AS ServiceTaxAmount,
		ISNULL(DPD.TDSAmount,0.0)  AS TDSAmount,
		dbo.RoundUp(CASE WHEN (DPD.TDSAmount IS NOT NULL AND DPD.IsTdsGiven = 1 AND DPD.TDSAmount > 0) THEN 
			 CAST(ROUND((DPD.TDSAmount*100)/DPD.Amount,2) AS VARCHAR) ELSE ''END,2) AS TdsPercentage,
		DPD.AmountReceived,
		ROUND(DPD.AmountReceived,0) AS RoundUpAmountReceived,
		PM.ModeName,
		DPD.CheckDDPdcNumber,
		CASE WHEN DPD.ChequeDDPdcDate IS NOT NULL THEN CONVERT(VARCHAR,DPD.ChequeDDPdcDate,106) ELSE '' END AS ChequeDDPdcDate,
		DPD.BankName,
		DPD.DrawerName,
		DPD.UtrTransactionId,
		DPD.PANNumber,
		DPD.TANNumber,
		CASE WHEN DPD.PaymentDate IS NOT NULL THEN CONVERT(VARCHAR,DPD.PaymentDate,106) ELSE '' END AS PaymentDate,
		OU.UserName AS ApprovedBy,
		CASE WHEN DPD.ApprovedOn IS NOT NULL THEN CONVERT(VARCHAR,DPD.ApprovedOn,106) ELSE '' END AS ApprovedOn,
		CASE WHEN DPD.IsApproved IS NULL THEN 'show' ELSE 'hide' END AS IsApproved,
		CASE WHEN DPD.IsApproved IS NULL THEN 'hide' ELSE 'show' END AS ShowStatus,
		CASE WHEN DPD.IsApproved = 1 THEN 'Accepted' WHEN DPD.IsApproved = 0 THEN 'Rejected' END AS Status,
		CASE 
			WHEN ISNULL(DPD.PaymentRejectedReasonId, -1) = -1 THEN DPD.RejectedReason 
			ELSE
			DPRR.Name + ' - ' + DPD.RejectedReason 
		END AS PaymentRejectedReason,
		DPD.HostUrl AS ChequeHostUrl,DPD.OriginalImgPath AS ChequeOriginalImgPath, DPD.DepSlipHostUrl,DPD.DepSlipOriginalImgPath,
		CASE WHEN DPD.DepositedDate IS NOT NULL THEN CONVERT(VARCHAR,DPD.DepositedDate,106) ELSE '' END AS DepositDate,DPD.IssuerBankName 
		,DPD.Comments

	FROM
		DCRM_PaymentDetails DPD(NOLOCK)
		LEFT JOIN PaymentModes PM(NOLOCK) ON PM.Id = DPD.Mode  --AND ISNULL(DPD.IsApproved,1) <> 0 -- Added By Vkp  To filter Rejected Payment Details 
		LEFT JOIN OprUsers OU(NOLOCK) ON OU.Id = DPD.ApprovedBy
		LEFT JOIN DCRM_PaymentRejectedReasons DPRR WITH (NOLOCK) ON DPD.PaymentRejectedReasonId = DPRR.Id
	WHERE
		TransactionId = @TransactionId --Modified by Kritika Choudhary
	ORDER BY DPD.ApprovedOn DESC

	--get invoice amount 
	SELECT 
		ISNULL((SELECT SUM(MGI.InvoiceAmount) FROM M_GeneratedInvoice MGI(NOLOCK) WHERE MGI.TransactionId = DPT.TransactionId AND MGI.Status = 2),0) AS ApprovedInvoiceAmount,
		ISNULL((SELECT SUM(MGI.InvoiceAmount) FROM M_GeneratedInvoice MGI(NOLOCK) WHERE MGI.TransactionId = DPT.TransactionId AND MGI.Status = 1),0) AS PendingForAprvlInvoiceAmount,
		((SELECT SUM(DPD.Amount) FROM DCRM_PaymentDetails DPD(NOLOCK) WHERE DPD.TransactionId = DPT.TransactionId AND ( ( DPD.Mode = 8 AND DPD.ChequeDDPdcDate <=GETDATE()) OR DPD.Mode <> 8) ) -
		 ISNULL((SELECT SUM(MGI.InvoiceAmount) FROM M_GeneratedInvoice MGI(NOLOCK) WHERE MGI.TransactionId = DPT.TransactionId AND MGI.Status IN (1,2)),0)) AS PendingInvoicAmount
	FROM 
		DCRM_PaymentTransaction DPT WITH(NOLOCK)
	WHERE
		DPT.TransactionId = @TransactionId

	--get generated invoice details
	SELECT 
		@DealerId AS DealerId,
		MGI.Id AS InvoiceId,
		MGI.InvoiceNumber,
		MGI.InvoiceAmount,
		MGI.PostTaxInvoiceAmount,
		CONVERT(VARCHAR,MGI.InvoiceDate,106) AS InvoiceDate,
		CASE MGI.IsCleanMissionManual WHEN 1 THEN 1 ELSE  ISNULL(DATEDIFF(day,'11-14-15',DPT.CreatedOn),1) END  AS cleanIndiaMission -- Added By Vinay Kumar prajapati 
		,CASE MGI.IsKrishiKalyanTaxManual  WHEN 1 THEN 1 ELSE  ISNULL(DATEDIFF(day,'06-06-16',DPT.CreatedOn),1) END  AS KrishiKalyanTax --Added By:Komal Manjare(29-June-2016) get KrishiKalyan flag for approved invoices
	FROM	
		M_GeneratedInvoice MGI WITH(NOLOCK)
	    INNER JOIN DCRM_PaymentTransaction DPT(NOLOCK) ON DPT.TransactionId = MGI.TransactionId

	WHERE
		MGI.Status = 2
		AND MGI.TransactionId = @TransactionId
	ORDER BY 
		MGI.InvoiceDate DESC

END

