IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[RVN_GetAvailableProducts]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[RVN_GetAvailableProducts]
GO

	
-- =============================================
-- Author	:	Sachin Bharti(10th Oct 2014)
-- Description	:	Get available products already exist for dealer
--Modified By : Vinay kumar Prajapati 23rd Nov 2015 get column ApprovedLeads (TotalLeads)
-- Modified By : Kartik Rathod on 8 dec 2015 get the list of outlet for MultiOutletdealer case and Approveleads and ContractCampaignMappingId for that dealersalesid
-- Modified By : KArtik Rathod on 9 dec 2015 for Multioutlet case send AApprove lead as Null
-- Modified by : Kritika Choudhary on 14th Dec 2015, added parameter TotalDeliveredin select query in get details from DCRM_SalesDealer
-- Modified By : Sunil Yadav On 08 jan 2016
-- Description :  package name , pitching duration and inquery points added  in get product details query.
-- Modified By : Sunil Yadav On 03 feb 2016
-- Description : get pitched outlet dealerId in case of group and multioutlet
-- EXEC RVN_GetAvailableProducts 9557,13084
-- Modified By : Sunil Yadav On 03 feb 2016
-- Description :  to get pitched outlet dealerId in case of group and multioutlet
-- =============================================
CREATE PROCEDURE [dbo].[RVN_GetAvailableProducts]
	
	@DealerId	INT,
	@SalesDealerId	INT = NULL

AS
BEGIN
	
	SET NOCOUNT ON;

	--get all existing campaign against the dealer
	SELECT 
			DPF.DealerPackageFeatureID AS CampaignId , 
			CONVERT(VARCHAR(20),DPF.PackageStartDate,106) AS PackageStartDate,
			CONVERT(VARCHAR(20),DPF.PackageEndDate,106) AS PackageEndDate,
			CONVERT(VARCHAR(20),DPF.ApprovedOn,106) AS ApprovedOn,
			CONVERT(VARCHAR(20),DPF.PackageStatusDate,106) AS SuspendedOn,
			ISNULL(DPF.LeadCount,0) AS LeadsCount,
			OU1.UserName AS SuspendedBy, OU.UserName AS ApprovedBy,
			DPF.PackageId AS ProductId , PK.Name AS ProductName,
			IPC.Id AS InquiryPointId,IPC.Name AS InquiryPoint,
			DPF.PackageStatus AS StatuId , RPS.Status ,
			CASE WHEN DPF.PackageId = 72 Then DPF.PercentageSlab END AS WarrantyPerSlab,
			CASE WHEN DPF.MakeId = -1 THEN 'ALL' ELSE CK.Name END AS Make,DPF.MakeId ,
			CASE WHEN DPF.ModelId = -1 THEN 'ALL' ELSE CM.Name END AS Model,DPF.ModelId,
			DPF.PackageQuantity AS RSAProductQuantity,
			CASE	WHEN DPF.TransactionId IS NULL THEN DPF.AmountPaid 
					WHEN DPF.TransactionId IS NOT NULL THEN DPT.FinalAmount END AS FinalAmount
			
	FROM	
			RVN_DealerPackageFeatures DPF(NOLOCK)
			INNER	JOIN Packages PK(NOLOCK) ON PK.Id = DPF.PackageId
			INNER	JOIN InquiryPointCategory IPC(NOLOCK) ON IPC.Id = PK.InqPtCategoryId
			INNER	JOIN RVN_PackageStatus RPS(NOLOCK) ON RPS.PackageStatusID = DPF.PackageStatus
			LEFT	JOIN DCRM_PaymentTransaction DPT(NOLOCK) ON DPT.TransactionId = DPF.TransactionId 
			LEFT  JOIN OprUsers OU(NOLOCK) ON OU.Id = DPF.ApprovedBy 
			LEFT  JOIN OprUsers OU1(NOLOCK) ON OU1.Id = DPF.UpdatedBy
			LEFT  JOIN CarMakes CK(NOLOCK) ON CK.ID = DPF.MakeId
			LEFT  JOIN CarModels CM(NOLOCK) ON CM.ID = DPF.ModelId
	WHERE	
			DPF.DealerId = @DealerId
	ORDER BY DPF.UpdatedOn DESC

	--get product details from DCRM_SalesDealer
	SELECT 
		DSD.ClosingAmount,--CASE WHEN DSD.IsMultiOutlet = 0 THEN CCM.TotalGoal END ApprovedLeads,
		CCM.TotalGoal ApprovedLeads,
		P.Name AS PackageName,
		P.InquiryPoints ,
		DSD.PitchDuration AS ActualValidity,
		--CASE WHEN DPF.DealerPackageFeatureID IS NULL OR DPF.LeadCount IS NULL THEN ISNULL(DSD.NoOfLeads,0) ELSE DPF.LeadCount END AS NoOfLeads, 
		ISNULL(DSD.NoOfLeads,0) AS NoOfLeads, 
		ISNULL(DSD.PercentageSlab,0) AS PercentageSlab,CONVERT(VARCHAR(11),CCM.StartDate,103) AS StartDate,CONVERT(VARCHAR(11),CCM.EndDate,103)AS EndDate
		,CCM.ContractBehaviour,CCM.ContractType,CCM.ReplacementContractId,CCM.Id as ContractCampaignMappingId,DPF.DealerPackageFeatureID,CCM.TotalDelivered
	FROM	
		DCRM_SalesDealer DSD(NOLOCK)
		LEFT JOIN Packages P WITH(NOLOCK) ON P.Id = DSD.PitchingProduct
		LEFT JOIN RVN_DealerPackageFeatures DPF(NOLOCK) ON DSD.Id = DPF.ProductSalesDealerId
		LEFT JOIN TC_ContractCampaignMapping CCM WITH(NOLOCK) ON CCM.ContractId = DPF.DealerPackageFeatureID
	WHERE
		DSD.ID = @SalesDealerId


	--get the list of outlet for MultiOutletdealer case
	
	--SELECT
	--	 MOM.DealerId AS OutletsDealerId,D.Organization,MOM.ApproveLeads,CCM.Id AS ContractCampaignMappingId
	--FROM 
	--	DCRM_MultiOutletMapping MOM (NOLOCK)
	--	INNER JOIN DCRM_SalesDealer DSD(NOLOCK) ON MOM.DCRM_SalesDealerId = DSD.Id
	--	INNER JOIN Dealers D (NOLOCK) ON MOM.DealerId = D.Id 
	--	LEFT JOIN RVN_DealerPackageFeatures DPF(NOLOCK) ON DSD.Id = DPF.ProductSalesDealerId
	--	LEFT JOIN TC_ContractCampaignMapping CCM WITH(NOLOCK) ON CCM.ContractId = DPF.DealerPackageFeatureID AND MOM.DealerId = CCM.DealerId
	--WHERE 
	--	MOM.DCRM_SalesDealerId = @SalesDealerId

	-- Added By Sunil Yadav to get pitched outlet dealerId in case of group and multioutlet
	SELECT D.Organization AS Organization,DSDM.DealerId FROM DCRM_SalesDealerMapping DSDM WITH(NOLOCK)
	--JOIN DCRM_SalesDealer DSD WITH(NOLOCK) ON DSD.Id = @SalesDealerId
	JOIN Dealers D WITH(NOLOCK) ON D.ID = DSDM.DealerId
	WHERE DSDM.SalesDealerId = @SalesDealerId

END

----------------------------------------------------------------------------------------------------------------------------------------------------------------

SET QUOTED_IDENTIFIER ON
