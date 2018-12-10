IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetContentSegmentCount_v16_10_1]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetContentSegmentCount_v16_10_1]
GO
-- Author:Bhairavee
-- Created on:21.10.2016
-- Description: Get all the Category with masking name,id and count associated with them
-- Exec [dbo].[GetContentSegmentCount_v16_10_1] 16,852

CREATE PROCEDURE [dbo].[GetContentSegmentCount_v16_10_1]
 @makeid INT=NULL
,@modelid INT=NULL
AS
BEGIN

SELECT cat.Id as SubCategoryId, cat.DisplayName as SubCategoryName,RecordCount= 
CASE
WHEN cat.Id IN (1,2,6,8,12,19,22) THEN (
CASE 
WHEN @modelid IS NULL and @makeid IS NULL THEN(
			SELECT count(cb.id)
			FROM con_editcms_basic cb WITH (NOLOCK)
			WHERE cb.isactive = 1
				AND cb.ispublished = 1
				AND cb.applicationid = 1
				AND cb.CategoryId=cat.Id
				AND cb.displaydate <= getdate()
			GROUP BY cb.CategoryId
)
ELSE 
(
		SELECT count(cb.id) FROM  Con_EditCms_Category AS ctg WITH(NOLOCK) INNER JOIN con_editcms_basic cb WITH (NOLOCK)
			ON cb.CategoryId=ctg.Id INNER JOIN (select distinct(BasicId) FROM Con_EditCms_Cars tag WITH (NOLOCK) WHERE (@makeid IS NULL OR tag.MakeId = @makeid) and (@modelid IS NULL OR tag.ModelId = @modelid)) tag on tag.BasicId = cb.id
			WHERE cb.isactive = 1
				AND cb.ispublished = 1
				AND cb.applicationid = 1
				AND cb.displaydate <= getdate()
				AND ctg.IsActive = 1
				AND cb.CategoryId=cat.Id
				GROUP BY cb.CategoryId
)
END
)
WHEN cat.Id =10 THEN (	
	SELECT count(*) from(SELECT CM.Id AS ModelId		
	FROM CarModels AS CM WITH (NOLOCK)
	INNER JOIN Con_EditCms_Images AS IMG WITH (NOLOCK) ON CM.Id = IMG.ModelId
	INNER JOIN Con_EditCms_Basic AS BA WITH (NOLOCK) ON BA.Id = IMG.BasicId
		WHERE CM.Id = COALESCE(@modelid, CM.Id)
			AND CM.CarMakeId = COALESCE(@makeid, CM.CarMakeId)
			AND (BA.CategoryId = 8 OR BA.CategoryId = 10)
			AND BA.IsPublished = 1
			AND BA.ApplicationID =1
			AND IMG.IsActive = 1
			AND CM.IsDeleted = 0
		GROUP BY CM.id) as tempTable
)
WHEN cat.Id=13 THEN (
	SELECT count(*) from(SELECT CM.Id AS ModelId
		FROM CarModels AS CM WITH (NOLOCK)
		INNER JOIN Con_EditCms_Cars AS CC WITH (NOLOCK) ON CC.ModelId = CM.ID
			AND CC.IsActive = 1
		INNER JOIN Con_EditCms_Basic AS BA WITH (NOLOCK) ON BA.Id = CC.BasicId
		INNER JOIN Con_EditCms_Videos AS CV WITH (NOLOCK) ON BA.Id = CV.BasicId
			AND CV.IsActive = 1
			WHERE CM.Id = COALESCE(@modelid, CM.Id)
			AND CM.CarMakeId = COALESCE(@makeid, CM.CarMakeId)
			AND BA.IsPublished = 1
			AND BA.ApplicationID = 1
			AND CM.IsDeleted = 0
			group by CM.Id) as tempTable
)
END
FROM Con_EditCms_Category as cat WITH (NOLOCK)
END
