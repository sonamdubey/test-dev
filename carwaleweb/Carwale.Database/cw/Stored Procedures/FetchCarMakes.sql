IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[FetchCarMakes]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[FetchCarMakes]
GO

	-- =============================================
-- Author:		Shikhar
-- Create date: 27-07-2012
-- Description:	Returns Car Makes with various Attributes
-- Last Modified: Jan 22, 2013 by Shikhar
-- Mod. Description: Add the various fields in the CarMakes
-- =============================================
CREATE PROCEDURE [cw].[FetchCarMakes]
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
SELECT
	 Ma.Id AS Value,
	 Ma.Name AS [Text],
	 Ma.New,
	 Ma.Used,
	 Ma.Futuristic,
	 0 AS RoadTest,
	 0 AS PriceQuote,
	 0 AS UserReview,
	 Ma.LogoUrl
INTO #tempMakes
FROM 
	CarMakes Ma WITH(NOLOCK)
WHERE
	Ma.IsDeleted = 0

UPDATE #tempMakes
SET RoadTest = 1
WHERE Value IN (
		SELECT CMA.Id FROM
        Con_EditCms_Basic CB WITH(NOLOCK)
        JOIN Con_EditCms_Cars CC WITH(NOLOCK)
			ON CC.BasicId = CB.Id AND CC.IsActive = 1
        JOIN CarMakes CMA WITH(NOLOCK)
			ON CMA.ID = CC.MakeId
        WHERE 
			CMA.New = 1 AND CMA.IsDeleted = 0 AND CB.CategoryId = 8 
			AND CB.IsActive = 1 AND CB.IsPublished = 1
        )

UPDATE #tempMakes
SET PriceQuote = 1
WHERE Value IN (
	SELECT Ma.ID AS Value 
	FROM CarMakes Ma, CarModels MO, CarVersions Cv, Con_NewCarNationalPrices NAV
	WHERE Ma.IsDeleted = 0 AND Ma.ID = MO.CarMakeId AND MO.ID = Cv.CarModelId AND Cv.ID = NAV.VersionId AND NAV.IsActive = 1 AND Ma.New=1
)                

UPDATE #tempMakes
SET UserReview = 1
WHERE Value IN (
	SELECT CM.Id 
	FROM CarMakes AS CM
	WHERE CM.Id IN (SELECT MakeId FROM CustomerReviews WHERE IsActive = 1 AND IsVerified = 1)
)
                
SELECT * FROM #tempMakes                
DROP TABLE #tempMakes
	
END
