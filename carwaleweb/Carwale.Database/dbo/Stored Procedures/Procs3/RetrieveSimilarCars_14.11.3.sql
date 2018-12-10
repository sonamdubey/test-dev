IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[RetrieveSimilarCars_14]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[RetrieveSimilarCars_14]
GO

	
-- ============================================= 
-- Author:Prashant Vishe     
-- Create date:12 Dec 2013
-- Description:Stored Procedure for Retrieving Related Videos. 
-- modified On:9 Jan 2013 by Prashant vishe
-- modification:added query for retrieving subcatName
-- Modifed On:3 march 2013 by Prashant
-- Modification:added VideoId in query
-- Modified On:13 Aug 2014 by Rohan Sapkal
-- Modification:changed SELECT @SimilarModels to handle multiple models for one video
-- Modified by Manish on 13-08-2014 changed @SimilarModels datatype length from 500 to 250
-- Modified by Rohan.s On 12-09-2014 Removed TopCount,16-9-2014 commented out redundant rownum()
-- Modified by Rohan.s on 18-12-2014 , Added columns to select thumbnail from CDN -ON 24-12-2014 ,changed join n select for Con_editcms_images
-- ============================================= 
CREATE PROCEDURE [dbo].[RetrieveSimilarCars_14.11.3] --[dbo].[RetrieveSimilarCars] 13827,1
  -- Add the parameters for the stored procedure here 
  @BasicId  NUMERIC,
  @ApplicationID INT=1
AS
  BEGIN
      -- SET NOCOUNT ON added to prevent extra result sets from 
      -- interfering with SELECT statements. 
      SET nocount ON;
	  DECLARE @ThumbnailCategory numeric = (SELECT Id from Con_PhotoCategory with (nolock) where Name='VideoThumbNail' and ApplicationId=@ApplicationID)
      -- Insert statements for procedure here 
      DECLARE @SimilarModels VARCHAR(250)

		SELECT @SimilarModels = COALESCE(@SimilarModels + ',', '') + SCM.similarmodels
		FROM   similarcarmodels SCM with(nolock)
                                   INNER JOIN con_editcms_cars CC with(nolock)
                                           ON CC.modelid = SCM.modelid
                            WHERE  CC.basicid = @BasicId
                                   AND SCM.isactive = 1

      SELECT *
           --  ,Row_number()
           --    OVER(
           --      partition BY basicid
           --      ORDER BY videocarid) AS row
      FROM   (SELECT DISTINCT 
             CB.id                     AS BasicId,
             CC.modelid,
             CV.videourl,
			 CV.VideoId,
             CV.views,
             CV.likes,
             CV.isactive,
             CB.description,
             CB.title,
			 CB.Url,
             CV.duration,
             C.name                    AS Make,
             CM.name                   AS Model,
			 CM.MaskingName,
             dbo.Getgoogletags (CB.id) AS Tag,
			 CS.Name as SubCatName,
             CC.id                     AS VideoCarId
			 ,CI.HostURL as ThumbnailHostURL
			,CI.VideoPathThumbNail as ThumbnailImage
			,NULL as ThumbnailDirPath
             ,Row_number()
               OVER(
                 partition BY cv.basicid
                 ORDER BY CC.id)       AS row
              FROM   con_editcms_videos CV with(nolock)
                     INNER JOIN con_editcms_cars CC with(nolock)
                             ON CC.basicid = CV.basicid
                     INNER JOIN con_editcms_basic CB with(nolock)
                             ON CB.id = CV.basicid
                     LEFT JOIN con_editcms_basictags CBT with(nolock)
                            ON CBT.basicid = CV.basicid
                     LEFT JOIN con_editcms_tags CT with(nolock)
                            ON CT.id = CBT.tagid
                     INNER JOIN carmakes C with(nolock)
                             ON CC.makeid = C.id
                     INNER JOIN carmodels CM with(nolock)
                             ON CC.modelid = Cm.id
					Inner Join Con_EditCms_BasicSubCategories CBS with(nolock)
							ON CBS.BasicId=CV.BasicId
					 inner join Con_editCms_subcategories CS with(nolock)
							ON CBS.SubCategoryId=CS.Id
					LEFT JOIN Con_EditCms_Images CI WITH (NOLOCK) ON (CI.BasicId=CV.BasicId and CI.ImageCategoryId=@ThumbnailCategory)
              WHERE  CC.modelid IN(SELECT data
                                   FROM   [dbo].[Stringsplit](@SimilarModels,
                                          ',')
                                  )
                     AND CV.isactive = 1
					 AND cS.CategoryId=13
                     AND CC.isactive = 1
                     AND CB.ispublished = 1
					 AND CB.ApplicationID=@ApplicationID
                     AND CB.isactive = 1
					 AND (CI.IsMainImage=1 OR CI.IsMainImage is NULL))a
      WHERE  row = 1
  END 


  
  /****** Object:  StoredProcedure [dbo].[RetrieveVideoParameters_14.11.3]    Script Date: 26-12-2014 11:02:47 ******/
SET ANSI_NULLS ON
