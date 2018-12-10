IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[RVN_DealerPackageFeaturesData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[RVN_DealerPackageFeaturesData]
GO

	
-- =============================================
-- Author	:	Sachin Bharti(8th Oct 2014)
-- Description	:	Get data for all packages from 
--					RVN_DealerPackageFeatures table
-- Modifier	:	Sachin Bharti(27th Jan 2015)
-- Purpose	:	Added PackageQuantity for RSA packages
-- Modifier :   Ajay Singh(23 July 2015)
-- Modifier :	Amit Yadav(28-03-2016)
-- Purpose  :   To get filter data using transaction Id.
-- EXECUTE [dbo].[RVN_DealerPackageFeaturesData] 1
-- =============================================
CREATE PROCEDURE [dbo].[RVN_DealerPackageFeaturesData] 
	
	@PackageStatus	INT = NULL,
	@StateId		INT = NULL,
	@CityId			INT = NULL,
	@DealerId		INT = NULL,
	@ApprovedBy     INT = NULL,
	@Product        INT = NULL,
	@TransactionId	INT = NULL
AS
BEGIN
	
	SET NOCOUNT ON;

	SELECT 
			RVN.DealerPackageFeatureID AS CampaignId , RVN.DealerId,
			CASE	WHEN RVN.TransactionId IS NULL THEN RVN.AmountPaid 
					WHEN RVN.TransactionId IS NOT NULL THEN DPT.FinalAmount END AS AmountPaid,
			CONVERT(VARCHAR(15),RVN.EntryDate,106)AS EntryDate,
			RVN.ProductSalesDealerId AS SaledId,
			CONVERT(VARCHAR(15),RVN.PackageStartDate,106)AS PackageStartDate,
			CONVERT(VARCHAR(20),RVN.PackageEndDate,106) AS PackageEndDate,
			CONVERT(VARCHAR(20),RVN.ApprovedOn,106) AS ApprovedOn,
			RVN.ApprovedBy AS UserId,RVN.PackageStatus AS PackageStatusId,
			RVN.LeadCount AS LeadsCount,RVN.MakeId,RVN.ModelId,
			CASE WHEN RVN.MakeId = -1 THEN 'ALL' ELSE CK.Name END AS Make,
			CASE WHEN RVN.ModelId = -1 THEN 'ALL' ELSE CM.Name END AS Model,
			RPS.Status AS PackageStatus,D.Organization AS DealerName,C.Name AS City ,
			IPC.Name AS Product,IPC.Id AS ProductId,
			PK.Name AS SubProduct,PK.Id AS SubProductId,
			OU.UserName AS ApprovedBy,
			CASE WHEN RVN.PackageStatus = 2 AND RVN.isActive = 1 AND ( RVN.PackageId = 56) 
							THEN 1 ELSE 0 END AS IsDealerLocator, --To make dealer locator live
			RVN.PackageQuantity,
			RVN.TransactionId
	FROM	
			RVN_DealerPackageFeatures RVN(NOLOCK)
			INNER JOIN Dealers D(NOLOCK) ON RVN.DealerId = D.ID
			INNER JOIN Cities C (NOLOCK) ON C.ID = D.CityId
			INNER JOIN RVN_PackageStatus RPS(NOLOCK) ON RPS.PackageStatusID = RVN.PackageStatus
			INNER JOIN Packages PK(NOLOCK) ON PK.Id = RVN.PackageId
			INNER JOIN InquiryPointCategory IPC(NOLOCK) ON IPC.Id = PK.InqPtCategoryId 
			LEFT  JOIN DCRM_PaymentTransaction DPT(NOLOCK) ON DPT.TransactionId = RVN.TransactionId
			LEFT  JOIN OprUsers OU(NOLOCK) ON OU.Id = RVN.ApprovedBy 
			LEFT  JOIN CarMakes CK(NOLOCK) ON CK.ID = RVN.MakeId
			LEFT  JOIN CarModels CM(NOLOCK) ON CM.ID = RVN.ModelId
	WHERE 
			(@PackageStatus IS NULL OR RVN.PackageStatus = @PackageStatus)
			AND (@StateId IS NULL OR D.StateId = @StateId)
			AND (@CityId IS NULL OR D.CityId = @CityId)
			AND (@DealerId IS NULL OR RVN.DealerId = @DealerId)
			AND (@ApprovedBy IS NULL OR RVN.ApprovedBy=@ApprovedBy)
			AND (@Product IS NULL OR RVN.PackageId=@Product)
			AND (@TransactionId IS NULL OR RVN.TransactionId = @TransactionId)
	ORDER BY 
			RVN.UpdatedOn DESC

END
