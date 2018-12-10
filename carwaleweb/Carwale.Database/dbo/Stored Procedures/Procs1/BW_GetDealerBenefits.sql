IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BW_GetDealerBenefits]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BW_GetDealerBenefits]
GO

	

-- =============================================
-- Author		:	Sumit Kate
-- Create date	:	10 Mar 2016
-- Description	:	Select dealer benefits
-- Parameters	
--	@BenefitIds	:	Dealer Id
-- =============================================
CREATE PROC BW_GetDealerBenefits
(
	@DealerId INT
)
AS
BEGIN
SELECT
	benefit.Id AS BenefitId,
	benefitCat.ID AS CatId,
	benefitCat.Name  AS CategoryText,
	benefit.DealerId AS DealerId,
	benefit.EntryDate AS EntryDate,
	benefit.BenefitText AS BenefitText,
	C.Name AS CityName
FROM BW_PQ_DealerBenefit AS benefit WITH(NOLOCK)
INNER JOIN BW_PQ_DealerBenefit_Category benefitCat WITH (NOLOCK) ON benefit.CatId = benefitCat.ID AND benefit.IsActive = 1 AND benefitCat.IsActive = 1
INNER JOIN Cities C WITH (NOLOCK) ON C.ID = benefit.CityId AND C.IsDeleted=0
WHERE DealerId = @DealerId
END
