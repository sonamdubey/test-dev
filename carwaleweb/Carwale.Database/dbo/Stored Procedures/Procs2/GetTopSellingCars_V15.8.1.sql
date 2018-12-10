IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetTopSellingCars_V15]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetTopSellingCars_V15]
GO

	
-- Modified By : Ashish G. Kamble on 17 July 2013
-- Modified : Prices are showing for new delhi. Reference to nationalpricing table is removed.
-- Modified By : Akansha on 4.2.2014
-- Modified : Added MaskingName Column
-- [dbo].[GetTopSellingCars_15.6.1]  10
-- Modified By Satish Sharma ON Jun 4, 2015
-- Started Getting Model Images From CarModels Table
-- Added WITH(NOLOCK)
-- Modified By Ashwini Todkar ON July 22, 2015
-- Added provision for top record count
CREATE procedure [dbo].[GetTopSellingCars_V15.8.1]
@DisplayCount INT=0
AS
BEGIN
		with cte
		as (SELECT    TSC.modelid ModelId, 
				  CMD.HostURL,
				  CMD.OriginalImgPath, 
				  CMD.LargePic as imgpath, 
				  CMD.name   AS ModelName, 
				  CMK.name  AS MakeName, 
				  CMK.id,
				  TSC.sortorder,
				  E.BasicId, 
				  CEI.ImageCount,				 
				  CMD.MinPrice as MINPrice,
				  CMD.MaxPrice as MAXPrice,
				  CMD.MaskingName,
				  CMD.ReviewCount ,		--added by shalini on 09/09/14
				  ROW_NUMBER() over (partition by tsc.modelid order by basicid desc) as rownumber
		FROM con_topsellingcars  TSC WITH(NOLOCK)
		JOIN carmodels CMD  WITH(NOLOCK)  ON TSC.modelid = CMD.id
		JOIN carversions AS CV  WITH(NOLOCK)  ON CV.CarModelId=CMD.id   AND cv.new = 1 
		JOIN newcarspecifications NS  WITH(NOLOCK) on NS.CarVersionId=CV.ID		
		JOIN carmakes CMK  WITH(NOLOCK) ON CMD.carmakeid = CMK.id 
		LEFT JOIN (SELECT EC.basicid, EC.modelid  FROM   con_editcms_cars  EC  WITH(NOLOCK) INNER JOIN con_editcms_basic EB  WITH(NOLOCK)  ON EB.id = EC.basicid  AND EB.categoryid = 8 AND EB.IsPublished=1 AND EB.IsActive=1 AND EB.ApplicationID =1) AS E ON E.modelid = cmd.id 
		LEFT JOIN (SELECT CEI.modelid,Count(CEI.id) AS ImageCount FROM   con_editcms_images CEI WITH(NOLOCK) INNER JOIN con_editcms_basic EB WITH(NOLOCK) ON EB.id = CEI.basicid  AND EB.categoryid IN ( 10, 8 ) AND EB.ApplicationID =1
		GROUP  BY CEI.modelid) AS CEI ON CEI.modelid = cmd.id 
		WHERE  TSC.Status = 1
		GROUP BY  TSC.modelid, 
				  CMD.hosturl, 
				  CMD.OriginalImgPath,
				  CMD.LargePic, 
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
		select  * 
		into #TopSellingCars 
		from cte --where rownumber=1 order by sortorder		

		IF(@DisplayCount=0)
		BEGIN

		  SELECT * FROM #TopSellingCars where rownumber=1 order by sortorder

		END
		ELSE 
		BEGIN

		 SELECT TOP (@DisplayCount) * FROM #TopSellingCars where rownumber=1 order by sortorder

		END

		DROP TABLE #TopSellingCars
END


