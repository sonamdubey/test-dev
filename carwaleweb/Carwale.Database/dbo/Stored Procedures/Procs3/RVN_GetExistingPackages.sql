IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[RVN_GetExistingPackages]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[RVN_GetExistingPackages]
GO

	-- =============================================
-- Author		:	Sachin Bharti(30th April 2014)
-- Description	:	Get existing active packages for Dealers
-- Modifier		:	Sachin Bharti(5th June 2014)
-- Description	:	Added comments column in the query 
-- =============================================
CREATE PROCEDURE [dbo].[RVN_GetExistingPackages] 
	
	@DealerId	INT ,
	@PackageID	SMALLINT = NULL,
	@MakeId		SMALLINT = NULL,
	@ModelId	SMALLINT = NULL	

AS
BEGIN
	
	SET NOCOUNT ON;
	
	SELECT 
			DISTINCT DPF.DealerPackageFeatureID AS ID, DPF.DealerId, DPF.PackageId,	DPF.PackageStartDate,
			D.Organization AS DealerName,	PK.Name AS PackageName,	DPF.PackageStatus,
			DPF.PackageEndDate,	DPF.LeadType, COUNT(DPF.IsActive) AS ActivePkgCnt ,	DPF.PackageRequestDate,	DPF.IsActive,
			CASE WHEN DPF.MakeId IS NULL THEN '' WHEN DPF.MakeId = -1 THEN 'All' ELSE CK.Name END AS Make,
			CASE WHEN DPF.ModelId IS NULL THEN '' WHEN DPF.ModelId = -1 THEN 'All' ELSE CM.Name END AS Model,
			DPF.LeadCount,PK.InqPtCategoryId,DPF.LastDealerPackageFeatureID,DPF.Comments,
			(CASE WHEN DPF.PackageStartDate< GETDATE() AND DPF.PackageEndDate > GETDATE() THEN 0 ELSE 1 END ) AS IsSuspend ,
			ISNULL(DPF.SalesDealerId,0) AS SalesDealerId, ISNULL(DPF.ClosingAmount , 0) AS ClosingAmount , DPF.AmountPaid ,
			ISNULL(DPF.DiscountAmount,0) AS  DiscountAmount,ISNULL(DPF.ProductAmount,0) AS ProductAmount,
			ISNULL(DPF.IsTDSGiven , 0) AS IsTDSGiven , ISNULL(DPF.TDSAmount , 0) AS TDSAmount,DPF.ServiceTax,
			ISNULL(DPF.PackageQuantity,0) AS RSAQuantity
	FROM	
			RVN_DealerPackageFeatures DPF(NOLOCK)
			INNER	JOIN Packages PK(NOLOCK) ON PK.Id = DPF.PackageId
			LEFT	JOIN Dealers D(NOLOCK) ON D.ID = DPF.DealerId
			--LEFT	JOIN TC_DealerMakes TD(NOLOCK) ON TD.MakeId = DPF.MakeId AND TD.DealerId = DPF.DealerId
			LEFT	JOIN CarMakes	CK(NOLOCK) ON CK.ID = DPF.MakeId AND DPF.DealerId = @DealerId 
			LEFT	JOIN CarModels CM(NOLOCK) ON CM.CarMakeId = DPF.MakeId AND CM.ID = DPF.ModelId AND CM.IsDeleted = 0 
	WHERE 
			DPF.DealerId = @DealerId 
			AND DPF.IsActive = 1 
			AND (@PackageID IS NULL OR DPF.PackageId = @PackageID)
			AND (@MakeId IS	NULL OR DPF.MakeId = @MakeId)
			AND (@ModelId IS NULL OR DPF.ModelId = @ModelId)
	GROUP BY 
			DPF.DealerPackageFeatureID, DPF.DealerId, DPF.PackageId, DPF.PackageStartDate,
			DPF.PackageEndDate,DPF.LeadType,DPF.PackageRequestDate,PK.InqPtCategoryId,DPF.PackageStatus,
			CASE WHEN DPF.MakeId IS NULL THEN '' WHEN DPF.MakeId = -1 THEN 'All' ELSE CK.Name END ,
			CASE WHEN DPF.ModelId IS NULL THEN '' WHEN DPF.ModelId = -1 THEN 'All' ELSE CM.Name END,
			D.Organization,PK.Name ,DPF.AmountPaid , DPF.LeadCount,
			(CASE WHEN DPF.PackageStartDate< GETDATE() AND DPF.PackageEndDate > GETDATE() THEN 0 ELSE 1 END ) ,
			DPF.LastDealerPackageFeatureID,DPF.IsActive,DPF.Comments,DPF.SalesDealerId ,ISNULL(DPF.ProductAmount,0),
			ISNULL(DPF.DiscountAmount,0),ISNULL(DPF.ClosingAmount , 0),ISNULL(DPF.IsTDSGiven , 0),ISNULL(DPF.TDSAmount , 0),DPF.ServiceTax,ISNULL(DPF.PackageQuantity,0)
	ORDER BY 
			DPF.PackageRequestDate DESC 
END
