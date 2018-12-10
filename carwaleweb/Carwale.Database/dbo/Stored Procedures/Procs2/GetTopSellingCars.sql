IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetTopSellingCars]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetTopSellingCars]
GO

	-- Modified By : Ashish G. Kamble on 17 July 2013
-- Modified : Prices are showing for new delhi. Reference to nationalpricing table is removed.
-- Modified By : Akansha on 4.2.2014
-- Modified : Added MaskingName Column
-- [dbo].[GetTopSellingCars]  10
CREATE procedure [dbo].[GetTopSellingCars] 
@DisplayCount INT=0
AS
BEGIN
	--SELECT TOP (@DisplayCount) t1.ModelId,t1.HostUrl,t1.ImgPath,t1.Name AS ModelName,CMK.Name AS MakeName,CMK.ID,t1.SortOrder,
	--	(SELECT DISTINCT
	--	(SELECT TOP 1 MIN(AvgPrice) FROM Con_NewCarNationalPrices WHERE Con_NewCarNationalPrices.VersionId IN
	--	(SELECT ID FROM CarVersions WHERE CarModelId = CM.ID AND New = 1) AND AvgPrice > 0) AS MinPrice
	--FROM 
	--	CarModels AS CM, CarVersions AS CV, NewCarSpecifications NS
	--	LEFT JOIN (SELECT EC.BasicId, EC.ModelId FROM Con_EditCms_Cars EC INNER JOIN Con_EditCms_Basic EB ON EB.Id = EC.BasicId AND EB.CategoryId = 8) AS E ON E.ModelId = MO.ID  
	--		LEFT JOIN (SELECT CEI.ModelId, Count(CEI.Id) AS ImageCount FROM Con_EditCms_Images CEI INNER JOIN Con_EditCms_Basic EB ON EB.Id = CEI.BasicId AND EB.CategoryId IN (10,8) GROUP By CEI.ModelId) AS CEI ON CEI.ModelId = MO.ID   
		
	--WHERE 
	--	CM.IsDeleted = 0 AND CM.ID = t1.ModelId) as MINPrice
	--FROM 
	--	(SELECT TSC.ModelId,TSC.HostUrl,TSC.ImgPath,TSC.SortOrder,CMD.Name,CMD.CarMakeId FROM Con_TopSellingCars TSC LEFT JOIN CarModels CMD ON TSC.ModelID = CMD.ID) t1
	--	LEFT JOIN CarMakes CMK ON t1.CarMakeId = CMK.ID WHERE t1.ImgPath IS NOT NULL ORDER BY t1.SortOrder ASC
	
	
		with cte
		as (SELECT    TSC.modelid, 
				  TSC.hosturl, 
				  TSC.imgpath, 
				  CMD.name   AS ModelName, 
				  CMK.name  AS MakeName, 
				  CMK.id,
				  TSC.sortorder,
				  E.BasicId, 
				  CEI.ImageCount,
				  --MIN(CNP.AvgPrice) as MINPrice,
				  CMD.MinPrice as MINPrice,
				  CMD.MaxPrice as MAXPrice,
				  CMD.MaskingName,
				  CMD.ReviewCount ,		--added by shalini on 09/09/14
				  ROW_NUMBER() over (partition by tsc.modelid order by basicid desc) as rownumber
		FROM con_topsellingcars TSC
		JOIN carmodels CMD  ON TSC.modelid = CMD.id
		JOIN carversions AS CV ON CV.CarModelId=CMD.id   AND cv.new = 1 
		JOIN newcarspecifications NS on NS.CarVersionId=CV.ID
		--join con_newcarnationalprices as CNP on CNP.VersionId=CV.ID and CNP.AvgPrice>0
		JOIN carmakes CMK ON CMD.carmakeid = CMK.id 
		LEFT JOIN (SELECT EC.basicid, EC.modelid  FROM   con_editcms_cars EC INNER JOIN con_editcms_basic EB  ON EB.id = EC.basicid  AND EB.categoryid = 8 AND EB.IsPublished = 1 and EB.IsActive = 1 AND EB.applicationID = 1) AS E ON E.modelid = cmd.id 
		LEFT JOIN (SELECT CEI.modelid,Count(CEI.id) AS ImageCount FROM   con_editcms_images CEI INNER JOIN con_editcms_basic EB ON EB.id = CEI.basicid  AND EB.categoryid IN ( 10, 8 ) AND EB.applicationid = 1
		GROUP  BY CEI.modelid) AS CEI ON CEI.modelid = cmd.id 
		WHERE  TSC.imgpath IS NOT NULL AND TSC.Status = 1
		GROUP BY  TSC.modelid, 
				  TSC.hosturl, 
				  TSC.imgpath, 
				  CMD.name  , 
				  CMK.name , 
				  CMK.id,
				  TSC.sortorder,
				  E.BasicId, 
				  CEI.ImageCount,
				  CMD.MinPrice,
				  CMD.MaxPrice,
				  CMD.MaskingName,
				  CMD.ReviewCount --added by shalini on 09/09/14
				
		) 
		select  * from cte where rownumber=1 order by sortorder
END