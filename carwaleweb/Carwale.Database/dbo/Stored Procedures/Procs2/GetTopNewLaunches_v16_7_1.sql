IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetTopNewLaunches_v16_7_1]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetTopNewLaunches_v16_7_1]
GO

	

-- =============================================      
-- Author:  <Prashant Vishe>      
-- Create date: <01 Nov 2012>      
-- Description: <Returns top 4 Recently Launched cars by launched date in descending order> EXEC cw.GetTopNewLaunches  
-- [cw].[GetTopNewLaunches]   8 
-- =============================================  
-- =============================================
-- Modified by:	Amit Verma
-- Modified on: 14/01/2013
-- Description:	Added '@ModelIDs' parameter to avoid those models to appear in the result set
-- Modified By: Vikas : 16-1-2013 : Added Period of 12 months for which the records need to be shown
-- =============================================
-- Modified By : Ashish G. Kamble on 17 July 2013
-- Description : Remove nationalpricing table reference. Prices are from new delhi.
-- Modified By : Akansha on 10.2.2.014	
-- Description : Added Masking Name Column
-- Modified ,Added ApplicationId=1 (by Rohan sapkal on 25-11-2014)
--==============================================
-- Modified by : Ashwini Todkar on 22 july 2015 created version sp of GetTopNewLaunches
-- Modified by Ajay Singh on 29-06-2016 fetched avgprice from carmodels
-- Modified by Ajay Singh on 27-09-2016 added condition for removing versions
CREATE PROCEDURE [dbo].[GetTopNewLaunches_v16_7_1] (
	@cnt TINYINT = 8
	,--default count value
	@modelIDs VARCHAR(max) = NULL
	)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from      
	-- interfering with SELECT statements.      
	SET NOCOUNT ON;

	WITH CTE
	AS (
		SELECT DISTINCT Sp.Id
			,Ma.NAME AS Make
			,Ma.ID as MakeId
			,Mo.NAME AS Model
			,Mo.Id AS ModelId
			,Mo.MaskingName
			,SP.LaunchDate AS LaunchDate
			,Mo.LargePic
			,Mo.SmallPic
			,MO.HostURL AS HostUrl
			,E.BasicId
			,CEI.ImageCount
			,Mo.MinPrice
			,Mo.MaxPrice
			,Mo.ReviewCount
			,Mo.OriginalImgPath
			,--added by shalini on 10/09/14
			-- NP.MinPrice,
			Mo.MinAvgPrice, --added by ajay singh on 29-06-2016
			ROW_NUMBER() OVER (
				PARTITION BY MO.Id ORDER BY Mo.MinPrice ASC
				) AS row
		FROM ExpectedCarLaunches SP WITH (NOLOCK)
		INNER JOIN CarModels Mo WITH (NOLOCK) ON MO.ID = SP.CarModelId
		INNER JOIN CarVersions VS WITH (NOLOCK) ON VS.CarModelId = MO.ID
		INNER JOIN CarMakes Ma WITH (NOLOCK) ON Ma.ID = Mo.CarMakeId
		--LEFT JOIN Con_NewCarNationalPrices NP WITH (NOLOCK) ON NP.VersionId = VS.ID  AND NP.IsActive = 1  
		LEFT JOIN (
			SELECT EC.BasicId
				,EC.ModelId
			FROM Con_EditCms_Cars EC WITH (NOLOCK)
			INNER JOIN Con_EditCms_Basic EB WITH (NOLOCK) ON EB.Id = EC.BasicId
				AND EB.CategoryId = 8
				AND EB.IsPublished = 1
				AND EB.ApplicationID = 1
			) AS E ON E.ModelId = MO.ID
		LEFT JOIN (
			SELECT CEI.ModelId
				,Count(CEI.Id) AS ImageCount
			FROM Con_EditCms_Images CEI WITH (NOLOCK)
			INNER JOIN Con_EditCms_Basic EB WITH (NOLOCK) ON EB.Id = CEI.BasicId
				AND EB.IsPublished = 1
				AND EB.CategoryId IN (
					10
					,8
					)
				AND EB.ApplicationID = 1
			GROUP BY CEI.ModelId
			) AS CEI ON CEI.ModelId = MO.ID
		WHERE SP.IsLaunched = 1
			AND SP.IsDeleted = 0
			AND Mo.New = 1
			AND Mo.Futuristic = 0
			AND Mo.IsDeleted = 0
			AND VS.New = 1
			AND VS.IsDeleted = 0
			AND SP.CarVersionId IS NULL  -- all version eliminate ajay singh 27 sep
			AND SP.LaunchDate >= DATEADD(YEAR, - 1, GETDATE())
			AND MO.ID NOT IN (
				SELECT items
				FROM dbo.SplitText(@modelIDs, ',')
				) --to avoid the ModelIDs present in @modelIDs to appear in the result set
		)
	SELECT TOP (@cnt) *
	FROM CTE
	WHERE row = 1
	ORDER BY LaunchDate DESC
END
