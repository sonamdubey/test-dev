IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetPopularModelVideoCount_v16_10_1]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetPopularModelVideoCount_v16_10_1]
GO
-- Author:  Bhairavee
-- Create date: 17-10-2016    
-- Description: Get Popular Model Video Count for News Listing Page    
-- exec [dbo].[GetPopularModelVideoCount_v16_10_1]

CREATE PROCEDURE [dbo].[GetPopularModelVideoCount_v16_10_1]
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
			,CM.NAME AS ModelName
			,CM.MaskingName AS ModelMaskingName
			,CM.New AS IsNew
			,CM.Futuristic AS IsFuturistic
			,CK.NAME AS MakeName
			,CM.CarMakeId AS MakeId
			,CV.VideoId AS VideoId
			,count(CV.Id) OVER (PARTITION BY CM.Id) AS VideoCount
			,CM.ModelPopularity AS Popularity
			,CV.BasicId AS BasicId
			,CS.Name AS SubCategoryName
			,row_number() OVER (
				PARTITION BY CM.id ORDER BY BA.PublishedDate DESC
				) AS RowNumber
		FROM CarModels AS CM WITH (NOLOCK)
		INNER JOIN Con_EditCms_Cars AS CC WITH (NOLOCK) ON CC.ModelId = CM.ID
			AND CC.IsActive = 1
		INNER JOIN Con_EditCms_Basic AS BA WITH (NOLOCK) ON BA.Id = CC.BasicId
		INNER JOIN Con_EditCms_Videos AS CV WITH (NOLOCK) ON BA.Id = CV.BasicId
			AND CV.IsActive = 1
		INNER JOIN CarMakes AS CK WITH (NOLOCK) ON CM.CarMakeId = CK.Id
		INNER JOIN Con_EditCms_BasicSubCategories CBS WITH (NOLOCK) ON CBS.BasicId = CV.BasicId
		INNER JOIN Con_editCms_subcategories CS WITH (NOLOCK) ON CBS.SubCategoryId = CS.Id
		WHERE CM.Id = COALESCE(@modelid, CM.Id)
			AND CM.CarMakeId = COALESCE(@makeid, CM.CarMakeId)
			AND BA.IsPublished = 1
			AND BA.ApplicationID = @applicationid
			AND CM.IsDeleted = 0
			AND CS.CategoryId=13
	)
	SELECT CTE.ModelId
		,CTE.ModelName
		,CTE.ModelMaskingName
		,CTE.MakeId
		,CTE.MakeName
		,CTE.IsNew
		,CTE.IsFuturistic
		,CTE.VideoCount
		,CTE.VideoId
		,CTE.SubCategoryName
		,CTE.BasicId
	FROM CTE WITH (NOLOCK)
	WHERE RowNumber = 1
	ORDER BY CTE.Popularity DESC

	SELECT @@ROWCOUNT AS VideoRecordCount
END