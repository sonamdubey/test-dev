IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[RetrieveSimilarCars]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[RetrieveSimilarCars]
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
-- ============================================= 
CREATE PROCEDURE [dbo].[RetrieveSimilarCars] --[dbo].[RetrieveSimilarCars] 250,10,1
  -- Add the parameters for the stored procedure here 
  @BasicId  NUMERIC=249,
  @ApplicationID INT=1
AS
  BEGIN
      -- SET NOCOUNT ON added to prevent extra result sets from 
      -- interfering with SELECT statements. 
      SET nocount ON;

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
             CC.id                     AS VideoCarId,
             Row_number()
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
              WHERE  CC.modelid IN(SELECT data
                                   FROM   [dbo].[Stringsplit](@SimilarModels,
                                          ',')
                                  )
                     AND CV.isactive = 1
					 AND cS.CategoryId=13
                     AND CC.isactive = 1
                     AND CB.ispublished = 1
					 AND CB.ApplicationID=@ApplicationID
                     AND CB.isactive = 1)a
      WHERE  row = 1
  END 