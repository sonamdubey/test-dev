IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CT_GetDealerPackageDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CT_GetDealerPackageDetails]
GO

	-- =============================================
-- Author:		Mihir A.Chheda
-- Create date: 26-08-2016
-- Description:	To get dealerdetails,Carwale Package Details ,CarTarde Package Details adn Add On Package Details 
-- Modify By  : Mihir A Chheda[30-08-2016] To get dealers last uploaded stock count 
-- =============================================
CREATE PROCEDURE [dbo].[CT_GetDealerPackageDetails]
	@DealerId INT 
AS
BEGIN

    -------Get Dealer Details------
	SELECT D.ID CwDealerId,CWCTM.CTDealerId,D.Organization DealerName,D.Address1,D.EmailId DealerEmail,D.MobileNo DealerContact
	,(SELECT COUNT(Inquiryid) FROM livelistings(NOLOCK) WHERE DealerId=@DealerId) AS UploadedStockCount --Mihir A Chheda[30-08-2016]
	FROM Dealers D (NOLOCK)
	LEFT JOIN CWCTDealerMapping CWCTM (NOLOCK) ON D.ID=CWCTM.CWDealerID
	WHERE D.ID=@DealerId

	------Get Dealers Carwale Package Deatils------
	SELECT IPC.Id PlanId,P.Id SubPlanId,IPC.Name PackageName,P.Name SubPackageName,
	CCP.ExpiryDate PackageEndDate,
	(SELECT TOP 1 CPR.ApprovalDate FROM ConsumerPackageRequests CPR (NOLOCK) 
	WHERE CPR.ConsumerId = @DealerId AND CPR.ConsumerType = 1 AND CPR.IsApproved = 1	
	ORDER BY CPR.Id DESC) PackageStartDate,
	CASE WHEN (DATEDIFF(dd, GETDATE(), CCP.ExpiryDate)) >= 0 THEN 'Active' ELSE 'InActive'  END PackageStatus	
	FROM ConsumerCreditPoints CCP (NOLOCK) 
	INNER JOIN InquiryPointCategory IPC (NOLOCK) ON CCP.PackageType = IPC.Id
	INNER JOIN Packages P (NOLOCK) ON CCP.CustomerPackageId=P.Id
	WHERE CCP.ConsumerId = @DealerId AND CCP.ConsumerType = 1

	------Get Dealers CarTrade Package Deatils------
	SELECT IPC.Id PlanId,P.Id SubPlanId,IPC.Name PackageName,P.Name SubPackageName, 
	CMP.PackageStartDate, CMP.PackageEndDate,CASE WHEN (DATEDIFF(dd, GETDATE(), CMP.PackageEndDate)) >= 0 THEN 'Active' ELSE 'InActive'  END PackageStatus
	FROM CWCTDealerMapping CMP (NOLOCK)
	INNER JOIN Packages P (NOLOCK) ON CMP.PackageId = P.Id
	INNER JOIN InquiryPointCategory IPC (NOLOCK) ON IPC.Id= P.InqPtCategoryId
	WHERE CMP.CWDealerID = @DealerId

	------Get Dealers Add On Package Details
	SELECT IPC.Id PlanId,P.Id SubPlanId,IPC.Name PackageName,P.Name SubPackageName, 
	AP.StartDate PackageStartDate, AP.EndDate PackageEndDate, CASE WHEN AP.IsActive=1 THEN 'Active' ELSE 'InActive'  END PackageStatus
	FROM CT_AddOnPackages AP (NOLOCK)
	INNER JOIN Packages P (NOLOCK) ON AP.AddOnPackageId = P.Id
	INNER JOIN InquiryPointCategory IPC (NOLOCK) ON IPC.Id= P.InqPtCategoryId
	WHERE AP.CWDealerId = @DealerId
END
