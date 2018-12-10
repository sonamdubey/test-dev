IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Microsite_GetDWModelFeatures]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Microsite_GetDWModelFeatures]
GO

	
CREATE PROCEDURE [dbo].[Microsite_GetDWModelFeatures] --execute Microsite_GetDWModelFeatures 4,5
@ModelId INT
,@DealerId INT
AS
--Created by:Rakesh Yadav ON 4 Jun 2015
--Desc: fetch highlights and features of models for dealer website on DealerModelId(DealerModelId is different from CarModels Id)
--Modified By Rakesh Yadav on 05 Aug 2015 to get OriginalImgPath
BEGIN
	-- fetch categories 
	SELECT DISTINCT Result.CategoryName,Result.CategoryId FROM 
	(
	SELECT ROW_NUMBER() OVER(ORDER BY DMFC.SortOrder) AS Rows,MF.CategoryName,MF.Id AS CategoryId
	FROM Microsite_DWModelFeatureCategories DMFC WITH (NOLOCK)
	JOIN Microsite_ModelFeatureCategories  MF WITH (NOLOCK) ON MF.Id=DMFC.Microsite_ModelFeatureCategoriesId
	WHERE DMFC.IsActive=1 AND DMFC.DealerId=@DealerId AND DMFC.DWModelId=@ModelId
	AND MF.IsActive=1
	) AS Result

	--fetch all categories data
	SELECT MF.FeatureTitle,MF.FeatureDescription,MF.Id,MF.HostUrl,MF.ImgPath,MF.ImgName,MFC.CategoryName,MFC.Id AS CategoryId,MF.OriginalImgPath
	FROM 
	Microsite_DWModelFeatures MF WITH (NOLOCK)
	JOIN Microsite_DWModelFeatureCategories DMFC WITH (NOLOCK) ON DMFC.Id=MF.Microsite_DWModelFeatureCategoriesId
	JOIN Microsite_ModelFeatureCategories MFC WITH (NOLOCK) ON MFC.Id=DMFC.Microsite_ModelFeatureCategoriesId
	WHERE 
	MF.IsActive=1 AND DMFC.IsActive=1 AND MFC.IsActive=1
	AND DMFC.DealerId=@DealerId AND DMFC.DWModelId=@ModelId
	ORDER BY DMFC.SortOrder
END