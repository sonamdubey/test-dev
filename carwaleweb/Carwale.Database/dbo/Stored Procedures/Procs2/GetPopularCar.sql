IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetPopularCar]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetPopularCar]
GO

	--/****** Created Date: 7/6/2014 *******/--
--/****** Created By: Natesh Kumar ****/----
--/****** Description: to get car details based on review rate ****/---
-- Approved by Manish on 10-07-2014 05:20 pm
-- Modified by: Natesh on 20-7-2014 Added Application id flag for CMS merging
-- Exec [GetPopularCar]
CREATE PROCEDURE [dbo].[GetPopularCar]
@ApplicationId INT
AS
BEGIN
	with cte
		as(
			SELECT 
				Mk.ID As Id,
				Mk.Name AS MakeName, 
				Mo.Name AS ModelName, 
				Mo.MaskingName AS MaskingName,
				Mo.LargePic AS LargePic,
				Mo.SmallPic AS SmallPic,
				Mo.ReviewRate AS ReviewRate,
				Mo.ReviewCount AS ReviewCount,
				Mo.MinPrice AS MinPrice,
				Mo.MaxPrice AS MaxPrice,
				TSC.HostUrl AS HostUrl,
				
				--TSC.ImgPath AS SmallPic,
				TSC.SortOrder,
				ROW_NUMBER() over (partition by TSC.modelid order by basicid desc) as rownumber

				FROM CarModels Mo  
				JOIN con_topsellingcars TSC WITH (NOLOCK) ON TSC.modelid = Mo.ID
				JOIN carversions AS CV WITH (NOLOCK) ON CV.CarModelId=Mo.id  AND cv.new = 1  
				INNER JOIN CarMakes Mk WITH (NOLOCK) ON Mk.ID = Mo.CarMakeId
				LEFT JOIN (SELECT EC.basicid, EC.modelid  FROM   con_editcms_cars EC 
									INNER JOIN con_editcms_basic EB  WITH (NOLOCK) ON EB.id = EC.basicid  AND EB.categoryid = 8 AND EB.ApplicationID = @ApplicationId
									) AS E ON E.modelid = Mo.id 
				LEFT JOIN (SELECT CEI.modelid,Count(CEI.id) AS ImageCount FROM   con_editcms_images CEI 
									INNER JOIN con_editcms_basic EB WITH (NOLOCK) ON EB.id = CEI.basicid  AND EB.categoryid IN ( 10, 8 ) AND EB.ApplicationID = @ApplicationId
									GROUP  BY CEI.modelid) AS CEI  ON CEI.modelid = Mo.id 
				WHERE  TSC.ImgPath IS NOT NULL AND TSC.Status = 1

		)
		
		select top 5 * from cte where rownumber=1 order by sortorder
END
