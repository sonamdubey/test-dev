IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[RetrieveVideoParameters_14]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[RetrieveVideoParameters_14]
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
-- ============================================= 
CREATE PROCEDURE [dbo].[RetrieveVideoParameters_14.11.3]
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
			 CI.HostURL as ThumbnailHostURL,
			CI.VideoPathThumbNail as ThumbnailImage,
			NULL as ThumbnailDirPath,
             dbo.GetGoogleTags (CB.Id)AS Tag 
      FROM   con_editcms_videos CV  with(nolock)
             INNER JOIN con_editcms_basic CB  with(nolock)
                     ON CB.id = CV.basicid 
             LEFT JOIN con_editcms_basictags CBT  with(nolock)
                    ON CBT.basicid = CV.basicid 
             LEFT JOIN con_editcms_tags CT  with(nolock)
                    ON CT.id = CBT.tagid 
			 INNER JOIN Con_EditCms_Cars CC  with(nolock)
			        ON CC.BasicId=Cv.BasicId
			 INNER JOIN CarMakes C  with(nolock)
			        ON CC.MakeId=C.Id
			 INNER JOIN CarModels CM  with(nolock)
			        ON CC.ModelId=Cm.ID 
             Inner Join Con_EditCms_BasicSubCategories CBS with(nolock)
			       ON CBS.BasicId=CV.BasicId
			 inner join Con_editCms_subcategories CS with(nolock)
				   ON CBS.SubCategoryId=CS.Id
			LEFT JOIN Con_EditCms_Images CI WITH (NOLOCK) ON (CI.BasicId=CV.BasicId and CI.ImageCategoryId=@ThumbnailCategory)
      WHERE  CV.basicid = @BasicId and CB.IsActive=1 and CB.IsPublished=1 and CV.IsActive=1 and CS.CategoryId=13 AND CB.ApplicationID = @ApplicationID AND (CI.IsMainImage=1 OR CI.IsMainImage is NULL)
  END 

  
/****** Object:  StoredProcedure [dbo].[RetrieveVideosByMake_14.11.3]    Script Date: 26-12-2014 11:03:23 ******/
SET ANSI_NULLS ON
