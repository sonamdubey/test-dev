IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_FetchMaskedDealerDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_FetchMaskedDealerDetails]
GO

	-- =============================================
-- Author:		Sunil M. Yadav 
-- Create date: 20th Oct 2016
-- Description:	This sp is converted from inline query.
-- EXEC DCRM_FetchMaskedDealerDetails NULL,NULL,NULL,NULL,0
-- =============================================
CREATE PROCEDURE [dbo].[DCRM_FetchMaskedDealerDetails]
	@CityId INT = NULL,
	@StateId INT = NULL,
	@DealerName VARCHAR(100) = NULL,
	@MaskingNumber VARCHAR(21) = NULL,
	@IsPackageExpired BIT = 0
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	 SELECT DISTINCT SM.MM_SellerMobileMaskingId AS Id,SM.ConsumerId AS DealerId,D.Organization AS Dealer,SM.MaskingNumber AS MaskingNumber,SM.Mobile AS Mobile, 
			CASE SM.DealerType WHEN 1 THEN 'UCD' WHEN 2 THEN 'NCD' WHEN 3 THEN 'UCD+NCD' ELSE NULL END AS DealerType, 
			CMK.Name AS Make,CM.Name AS Model,SM.CreatedOn, 
			-- CASE SM.ProductTypeId WHEN 1 THEN 'UCD - Classified Listing' WHEN 2 THEN 'UCD Branding' WHEN 3 THEN 'NCD Branding' ELSE NULL END AS ProductType, 
			MPT.Name AS ProductType,
			PDS.Id AS CampaignId, SM.ProductTypeId 
			,D.ApplicationId
	FROM MM_SellerMobileMasking SM WITH (NOLOCK) 
	INNER JOIN MM_ProductTypes MPT WITH(NOLOCK) ON MPT.Id = SM.ProductTypeId
	INNER JOIN Dealers D  WITH (NOLOCK) ON D.ID = SM.ConsumerId 
	INNER JOIN Cities C WITH (NOLOCK) ON C.ID = D.CityId 
	INNER JOIN States S WITH (NOLOCK) ON S.ID = C.StateId 
	LEFT JOIN CarModels CM WITH (NOLOCK) ON CM.ID = SM.NCDBrandId 
	LEFT JOIN CarMakes CMK WITH (NOLOCK) ON CMK.ID = CM.CarMakeId 
	LEFT JOIN PQ_DealerSponsored PDS WITH(NOLOCK) ON SM.LeadCampaignId = PDS.Id 
	LEFT JOIN RVN_DealerPackageFeatures RVN WITH(NOLOCK) ON RVN.DealerId=D.ID 
	WHERE	(@CityId IS NULL OR C.ID = @CityId)
		AND (@StateId IS NULL OR S.ID = @StateId)
		AND (@DealerName IS NULL OR D.Organization LIKE '%'+@DealerName+'%')
		AND (@MaskingNumber IS NULL OR SM.MaskingNumber = @MaskingNumber)
		AND (@IsPackageExpired = 0 OR RVN.PackageStatus IN (4,5))   -- RVN_PackageStatus : 4 - Package is completed as per deal , 5 - No action is taken till date
	 ORDER BY SM.CreatedOn DESC
END
