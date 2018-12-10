IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[RetrieveVideoParameters_16_10_5]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[RetrieveVideoParameters_16_10_5]

GO

-- ============================================= 
-- Author:Prashant Vishe    
-- Create date:12 Dec 2013
-- Description:Stored Procedure for retrieving video parameters. 
-- Modified On:9 Jan 2013 by Prashant vishe
-- Modification:added query for retrieving subcatName 
-- Modifed On:3 march 2013 by Prashant
-- Modification:added VideoId in query
-- Modified by: Natesh on 20-7-2014 Added Application id flag for CMS merging
-- Modified by Rohan.s on 18-12-2014 , Added columns to select thumbnail from CDN -on 24-12-2014 changed select n join statements for con_editcms_images
-- Modified by Ashwini on 5 Aug 2015 ,retrieved original image path and host url and versioning
-- Modified by Jitendra on 22 Feb 2016, Based on Application id = 2 used BikeMakes and BikeModels table
-- Modified by Rakesh Yadav on 1 March 2016, retrive DisplayDate
--Modified by Rakesh Yadav ON 15 March 2016 to show untagged video,(INNER JOINs on Con_EditCms_Cars, CarMakes & CarModels OR BikeMakes & BikeModels is changed to LEFT JOIN  )
--Modified by Ajay singh on 18 sep 2016 fetched categoryid
-- ============================================= 
CREATE PROCEDURE [dbo].[RetrieveVideoParameters_16_10_5]
  -- Add the parameters for the stored procedure here 
  @BasicId NUMERIC,
  @ApplicationID int 
AS 
  BEGIN 
      -- SET NOCOUNT ON added to prevent extra result sets from 
      -- interfering with SELECT statements. 
      SET nocount ON; 
DECLARE @ThumbnailCategory numeric = (SELECT Id from Con_PhotoCategory with (nolock) where Name='VideoThumbNail' and ApplicationId=@ApplicationID)
      -- Insert statements for procedure here 
     
	 if (@ApplicationID = 1)
	 Begin
	   SELECT Top 1(CC.Id) as CarId,
	         (CB.Id),
	         CV.videourl, 
			 CV.VideoId,
             CV.views, 
             CV.likes, 
             CB.description, 
             CB.title, 
			 CB.Url,
			 CV.Duration,
			 C.Name as Make,
			 CM.Name as Model,
			 CM.MaskingName ,
			 CBS.SubCategoryId as SubCatId,
			 CS.Name as SubCatName,
			 CC.MakeId as MakeId,
			 CC.ModelId as ModelId,
			 CI.HostURL as HostUrl,
			CI.OriginalImgPath as OriginalImgPath,
			NULL as ThumbnailDirPath,
             dbo.GetGoogleTags (CB.Id)AS Tag 
			 ,CB.DisplayDate
			 ,CB.CategoryId -- Added by ajay singh
      FROM   con_editcms_videos CV  with(nolock)
             INNER JOIN con_editcms_basic CB  with(nolock)
                     ON CB.id = CV.basicid 
             LEFT JOIN con_editcms_basictags CBT  with(nolock)
                    ON CBT.basicid = CV.basicid 
             LEFT JOIN con_editcms_tags CT  with(nolock)
                    ON CT.id = CBT.tagid 
			 LEFT JOIN Con_EditCms_Cars CC  with(nolock)
			        ON CC.BasicId=Cv.BasicId
			 LEFT JOIN CarMakes C  with(nolock)
			        ON CC.MakeId=C.Id
			 LEFT JOIN CarModels CM  with(nolock)
			        ON CC.ModelId=Cm.ID 
             INNER Join Con_EditCms_BasicSubCategories CBS with(nolock)
			       ON CBS.BasicId=CV.BasicId
			 INNER join Con_editCms_subcategories CS with(nolock)
				   ON CBS.SubCategoryId=CS.Id
			LEFT JOIN Con_EditCms_Images CI WITH (NOLOCK) ON (CI.BasicId=CV.BasicId and CI.ImageCategoryId=@ThumbnailCategory)
      WHERE  CV.basicid = @BasicId and CB.IsActive=1 and CB.IsPublished=1 and CV.IsActive=1 and CS.CategoryId=13 AND CB.ApplicationID = @ApplicationID AND (CI.IsMainImage=1 OR CI.IsMainImage is NULL)
  END
  if (@ApplicationID = 2)
  BEGIN
  SELECT Top 1(CC.Id) as CarId,
	         (CB.Id),
	         CV.videourl, 
			 CV.VideoId,
             CV.views, 
             CV.likes, 
             CB.description, 
             CB.title, 
			 CB.Url,
			 CV.Duration,
			 C.Name as Make,
			 CM.Name as Model,
			 CM.MaskingName ,
			 CBS.SubCategoryId as SubCatId,
			 CS.Name as SubCatName,
			 CC.MakeId as MakeId,
			 CC.ModelId as ModelId,
			 CI.HostURL as HostUrl,
			CI.OriginalImgPath as OriginalImgPath,
			NULL as ThumbnailDirPath,
             dbo.GetGoogleTags (CB.Id)AS Tag 
			 ,CB.DisplayDate
      FROM   con_editcms_videos CV  with(nolock)
             INNER JOIN con_editcms_basic CB  with(nolock)
                     ON CB.id = CV.basicid 
             LEFT JOIN con_editcms_basictags CBT  with(nolock)
                    ON CBT.basicid = CV.basicid 
             LEFT JOIN con_editcms_tags CT  with(nolock)
                    ON CT.id = CBT.tagid 
			 LEFT JOIN Con_EditCms_Cars CC  with(nolock)
			        ON CC.BasicId=Cv.BasicId
			 LEFT JOIN BikeMakes C  with(nolock)
			        ON CC.MakeId=C.Id
			 LEFT JOIN BikeModels CM  with(nolock)
			        ON CC.ModelId=Cm.ID 
             INNER Join Con_EditCms_BasicSubCategories CBS with(nolock)
			       ON CBS.BasicId=CV.BasicId
			 INNER join Con_editCms_subcategories CS with(nolock)
				   ON CBS.SubCategoryId=CS.Id
			LEFT JOIN Con_EditCms_Images CI WITH (NOLOCK) ON (CI.BasicId=CV.BasicId and CI.ImageCategoryId=@ThumbnailCategory)
      WHERE  CV.basicid = @BasicId and CB.IsActive=1 and CB.IsPublished=1 and CV.IsActive=1 and CS.CategoryId=13 AND CB.ApplicationID = @ApplicationID AND (CI.IsMainImage=1 OR CI.IsMainImage is NULL) 
  END
  END
