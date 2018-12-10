IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CT_GetDealerDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CT_GetDealerDetails]
GO

	-- =============================================
-- Author:		Kartik Rathod	
-- Create date: 13 July 2016	
-- Description:	To get dealerdetails including last approved payment details
-- EXEC CT_GetDealerDetails 5
-- Modeified By : Mihir Chheda [29-07-2016] Get Package Id and InquiryPointCategory Id
-- =============================================
CREATE PROCEDURE [dbo].[CT_GetDealerDetails] 
	@DealerId BIGINT
AS
BEGIN
	
	SET NOCOUNT ON;
    
	SELECT  DISTINCT TOP 1  D.ID AS CwDealerId,D.Organization AS DealerName,P.Name AS PackageName,
			CASE WHEN (DATEDIFF(dd, GETDATE(), CCP.ExpiryDate)) >= 0 THEN 'Active' ELSE 'InActive'  END AS PackageStatus,
			CCP.ExpiryDate AS PackageExpiryDate,
			(SELECT COUNT(DISTINCT T.TC_InquiriesLeadId) FROM TC_InquiriesLead T WITH (NOLOCK) WHERE T.BranchId = D.ID AND MONTH(T.CreatedDate) = MONTH(GETDATE()) AND YEAR(T.CreatedDate) = YEAR(GETDATE())) AS LeadCurrentMonth,  -- current months lead count
			(SELECT COUNT(DISTINCT T.TC_InquiriesLeadId) FROM TC_InquiriesLead T WITH (NOLOCK) WHERE T.BranchId = D.ID AND MONTH(T.CreatedDate) = MONTH(DATEADD(mm, -1,GETDATE())) AND YEAR(T.CreatedDate) = YEAR(DATEADD(mm, -1,GETDATE()))) AS LeadLastMonth, -- Last months lead count
			PD.Amount AS LatestPayment, PD.ApprovedOn AS latestPaymentDate,--, PD.IsApproved
			D.Address1,D.EmailId AS DealerEmail,D.MobileNo AS DealerContact,COUNT(DISTINCT LL.Inquiryid) AS UploadedStockCount,
			(SELECT TOP 1 CPR.ApprovalDate FROM ConsumerPackageRequests CPR WITH(NOLOCK) 
			WHERE CPR.ConsumerId = @DealerId AND CPR.ConsumerType = 1 AND CPR.IsApproved = 1 AND CPR.PackageId = CCP.CustomerPackageId 
			ORDER BY CPR.Id DESC) AS PackageStartDate,
			CCP.PackageType AS PlanId,CCP.CustomerPackageId AS SubPlanId -- Mihir Chheda [29-07-2016] Get Package Id and InquiryPointCategory Id
	
	FROM Dealers AS D WITH (NOLOCK)
	INNER JOIN ConsumerCreditPoints CCP WITH (NOLOCK) ON CCP.ConsumerId = D.ID AND CCP.ConsumerType = 1
	INNER JOIN InquiryPointCategory P WITH (NOLOCK) ON CCP.PackageType = P.Id
	LEFT JOIN DCRM_SalesDealer DSD WITH (NOLOCK) ON CCP.ConsumerId = DSD.DealerId
	LEFT JOIN DCRM_PaymentDetails PD WITH (NOLOCK)	ON DSD.TransactionId = PD.TransactionId AND PD.IsApproved = 1
	LEFT JOIN livelistings LL WITH(NOLOCK) ON LL.DealerId = D.Id AND LL.SellerType = 1 -- SellerType = 1 : for delaler 
	WHERE D.ID = @DealerId 
	GROUP BY D.ID,D.Organization,P.Name,CCP.ExpiryDate,PD.Amount,D.Address1,D.EmailId,D.MobileNo,PD.ApprovedOn,CCP.CustomerPackageId,CCP.PackageType 
	ORDER BY PD.ApprovedOn DESC
END

