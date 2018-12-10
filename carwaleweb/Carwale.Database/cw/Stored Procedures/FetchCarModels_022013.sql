IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[FetchCarModels_022013]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[FetchCarModels_022013]
GO

	-- =============================================
-- Author:		Shikhar
-- Create date: 27-07-2012
-- Description:	Returns Car Models for a make id 
-- edited on :  07-02-2013
-- edited by :  Shikhar - Added the Specification, User Review, Price Quote availability and other fields in SP
-- Last Edited on: 08-02-2013 by Shikhar - The Specification column is currently commented as currently it is deployed
-- on production. Once, it is deployed, remove the comments.
-- =============================================
CREATE PROCEDURE [cw].[FetchCarModels_022013]
	@MakeId SMALLINT = 0
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;	
SELECT
	Mo.Name AS [Text],
	Mo.ID AS Value,
	Ma.Name AS CarMake,
	Mo.CarMakeId,  
	Mo.New, 
	Mo.Used, 
	Mo.Futuristic,
	Mo.SmallPic,
	Mo.HostURL,
	Mo.ReviewRate,
	Mo.ReviewCount,
	0 AS RoadTest,
	0 AS Specification,
	0 AS PriceQuote,
	0 AS UserReview,
	(SELECT COUNT(ID) FROM CustomerReviews WITH (NOLOCK) WHERE ModelId=Mo.ID AND IsActive=1  AND IsVerified = 1 ) AS TotalReviews, 
	(SELECT TOP 1 MIN(AvgPrice) FROM Con_NewCarNationalPrices WITH (NOLOCK) WHERE Con_NewCarNationalPrices.VersionId IN
    (SELECT ID FROM CarVersions WHERE CarModelId = Mo.ID AND New = 1 AND IsDeleted = 0) AND AvgPrice > 0)AS MinPrice,
    (SELECT TOP 1 MAX(AvgPrice) FROM Con_NewCarNationalPrices WITH (NOLOCK) WHERE Con_NewCarNationalPrices.VersionId IN
    (SELECT ID FROM CarVersions WHERE CarModelId = Mo.ID AND New = 1 AND IsDeleted = 0) AND AvgPrice > 0)AS MaxPrice
INTO #tempModel
FROM 
	CarModels Mo WITH(NOLOCK)
	INNER JOIN CarMakes Ma
		ON Ma.ID = Mo.CarMakeId
WHERE 
	(Mo.CarMakeId = @MakeId OR @MakeId = 0)
AND
	Mo.IsDeleted = 0

UPDATE #tempModel
SET RoadTest = 1
WHERE Value IN
( 
	SELECT Mo.ID
	FROM CarModels Mo WITH (NOLOCK)
	INNER JOIN Con_EditCms_Cars CC WITH (NOLOCK)
		ON Mo.ID = CC.ModelId
	INNER JOIN Con_EditCms_Basic CB WITH (NOLOCK)
		ON (CC.BasicId = CB.Id AND CC.IsActive = 1)
	WHERE Mo.New = 1 AND Mo.IsDeleted = 0
	AND CB.CategoryId = 8 AND CB.IsActive = 1 AND CB.IsPublished = 1 AND Mo.CarMakeId=@MakeId
)
	
--UPDATE #tempModel
--SET Specification = 1
--WHERE Value IN
--( SELECT CarModelId FROM CarVersions WITH (NOLOCK) WHERE IsDeleted = 0 AND IsSpecsAvailable = 1 )

UPDATE #tempModel
SET Specification = 1
WHERE Value IN
( SELECT CM.ID 
  FROM CarModels as CM WITH (NOLOCK) 
  join CarVersions as CV WITH (NOLOCK)  on CV.CarModelId=CM.ID 
  join NewCarSpecifications as NS WITH (NOLOCK)  on NS.CarVersionId=CV.ID
WHERE CM.New = 1  )

UPDATE #tempModel
SET PriceQuote = 1
WHERE Value IN
(	
	SELECT Mo.ID FROM CarModels Mo WITH (NOLOCK)
	INNER JOIN CarVersions CV WITH (NOLOCK)
		ON (Mo.ID = CV.CarModelId AND CV.IsDeleted = 0)
	INNER JOIN Con_NewCarNationalPrices NCP WITH (NOLOCK)
		ON (CV.ID = NCP.VersionId AND NCP.IsActive = 1)
	WHERE Mo.CarMakeId = @MakeId AND CV.New = 1 AND Mo.IsDeleted = 0
)

UPDATE #tempModel
SET 
	UserReview = 1
WHERE Value IN
(
	SELECT CR.ModelId FROM CarModels CMO WITH (NOLOCK)
	INNER JOIN CustomerReviews CR WITH (NOLOCK) ON CMO.ID = CR.ModelId 
	WHERE CMO.CarMakeId = @MakeId AND IsActive=1  AND IsVerified = 1
)

SELECT * FROM #tempModel
DROP TABLE #tempModel

END
