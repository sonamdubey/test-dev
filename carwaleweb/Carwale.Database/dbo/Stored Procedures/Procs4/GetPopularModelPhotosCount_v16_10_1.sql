IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetPopularModelPhotosCount_v16_10_1]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetPopularModelPhotosCount_v16_10_1]
GO
-- Author:  Bhairavee
-- Create date: 17-10-2016    
-- Description: Get Popular Model Images Count    
-- exec [dbo].[GetPopularModelPhotosCount_v16_10_1] 1,16,852

CREATE PROCEDURE [dbo].[GetPopularModelPhotosCount_v16_10_1]
	-- Add the parameters for the stored procedure here    
	@applicationid INT = 1
	,@makeid INT = NULL
	,@modelid INT = NULL
AS
BEGIN
	SET NOCOUNT ON;

	WITH CTE
	AS (
		SELECT CM.Id AS ModelId
			,count(IMG.OriginalImgPath) OVER (PARTITION BY CM.Id) AS ImageCount
			,CM.NAME AS ModelName
			,CM.MaskingName AS ModelMaskingName
			,CM.New AS IsNew
			,CM.Futuristic AS IsFuturistic
			,CM.CarMakeId AS MakeId
			,CK.NAME AS MakeName
			,CM.HostURL
			,CM.OriginalImgPath
			,CM.ModelPopularity
			,IMG.OriginalImgPath AS GalleryImagePath
			,IMG.Id
			,IMG.Sequence
		    ,row_number() OVER (
				PARTITION BY CM.id ORDER BY IMG.Sequence DESC,IMG.Id DESC
				) AS RowNumber
		FROM CarModels AS CM WITH (NOLOCK)
		INNER JOIN Con_EditCms_Images AS IMG WITH (NOLOCK) ON CM.Id = IMG.ModelId
		INNER JOIN Con_EditCms_Basic AS BA WITH (NOLOCK) ON BA.Id = IMG.BasicId
		INNER JOIN CarMakes AS CK WITH (NOLOCK) ON CM.CarMakeId = CK.Id
		WHERE CM.Id = COALESCE(@modelid, CM.Id)
			AND CM.CarMakeId = COALESCE(@makeid, CM.CarMakeId)
			AND (BA.CategoryId = 8 OR BA.CategoryId = 10)
			AND BA.IsPublished = 1
			AND BA.ApplicationID = @applicationid
			AND IMG.IsActive = 1
			AND CM.IsDeleted = 0
			AND CK.IsDeleted = 0
		)
	SELECT CTE.ModelId
		,CTE.ModelName
		,CTE.ModelMaskingName
		,CTE.MakeId
		,CTE.MakeName
		,CTE.IsNew
		,CTE.IsFuturistic
		,CTE.HostURL
		,CTE.OriginalImgPath
		,CTE.ImageCount
		,CTE.GalleryImagePath
	FROM CTE WITH (NOLOCK)
	WHERE RowNumber=1
	ORDER BY CTE.ModelPopularity DESC 

	SELECT @@ROWCOUNT AS ImageRecordCount
END