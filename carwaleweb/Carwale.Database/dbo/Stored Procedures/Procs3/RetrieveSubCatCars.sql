IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[RetrieveSubCatCars]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[RetrieveSubCatCars]
GO

	-- =============================================    
-- Author: Prashant Vishe       
-- Create date:22 Dec 2013     
-- Description: Query for fetching videos of different subcategories such as expert reviews,interior show,miscelleneous  
-- modified On:9 Jan 2013 by Prashant Vishe
-- modification:added query for retrieving subcatname 
-- modified By:Prashant vishe On 14 Jan 2013
-- modification:added query for ordering records in 3rd query by displaydate..  
-- Modifed On:3 march 2013 by Prashant
-- Modification:added VideoId in query
-- ============================================= 
CREATE PROCEDURE [dbo].[RetrieveSubCatCars]
  -- Add the parameters for the stored procedure here    
  @SubCat   INT,
  @ModelId  NUMERIC,
  @MakeId   NUMERIC,
  @basicIDs VARCHAR(500),
  @startindex int,
  @endindex int
AS
  BEGIN
      -- SET NOCOUNT ON added to prevent extra result sets from    
      -- interfering with SELECT statements.    
      SET nocount ON;

      -- Insert statements for procedure here    
      IF @SubCat = 1
        --for featured and latest videos 
        BEGIN
            SELECT * FROM   ((SELECT  * FROM   ((select top(3) * from (SELECT CV.basicid,
                                             CV.videourl,
											 CV.VideoId,
                                             CV.likes,
                                             CV.views,
                                             CB.description,
                                             CB.title,
											 CB.Url,
                                             CB.isfeatured,
                                             CV.duration,
                                             C.name
                                             AS
                                             Make,
                                             CM.name
                                             AS
                                             Model
                                             ,
											 CM.MaskingName,
                                             CC.id
                                             AS
                                             VideoCarId,
											 CB.displaydate,
											 CS.Name as SubCatName,
                                             Row_number()
                                               OVER(
                                                 partition BY CV.basicid
                                                 ORDER BY CC.id)
                                             AS
                                             row
                                             ,
                             Row_number()
                               OVER (
                                 ORDER BY CB.displaydate DESC ) RowNo
                              FROM   con_editcms_videos AS CV with(nolock)
                                     INNER JOIN con_editcms_basic AS CB with(nolock)
                                             ON CB.id = CV.basicid
                                     INNER JOIN con_editcms_cars AS CC with(nolock)
                                             ON CC.basicid = CV.basicid
                                     INNER JOIN carmakes C with(nolock)
                                             ON CC.makeid = C.id
                                     INNER JOIN carmodels CM with(nolock)
                                             ON CC.modelid = CM.id
									 Inner Join Con_EditCms_BasicSubCategories CBS with(nolock)
											ON CBS.BasicId=CV.BasicId
									 inner join Con_editCms_subcategories CS with(nolock)
											ON CBS.SubCategoryId=CS.Id
                              WHERE  CV.isactive = 1
							         AND cS.CategoryId=13
                                     AND CC.isactive = 1
                                     AND CB.ispublished = 1
                                     AND CB.isfeatured = 1
                                     AND @basicIDs IS NULL
                                     AND CB.isactive = 1)as x where x.row=1)
                             UNION ALL
                             (SELECT CV.basicid,
                                     CV.videourl,
									 CV.VideoId,
                                     CV.likes,
                                     CV.views,
                                     CB.description,
                                     CB.title,
									 CB.Url,
                                     CB.isfeatured,
                                     CV.duration,
                                     C.name                             AS Make,
                                     CM.name                            AS Model
                                     ,
									 CM.MaskingName,
                                     CC.id
                                     AS
                                     VideoCarId,
									  CB.displaydate,
									  CS.Name as SubCatName,
                                     Row_number()
                                       OVER(
                                         partition BY CV.basicid
                                         ORDER BY CC.id)                AS row,
                                     Row_number()
                                       OVER (
                                         ORDER BY CB.displaydate DESC ) RowNo
                              FROM   con_editcms_videos AS CV with(nolock)
                                     INNER JOIN con_editcms_basic AS CB with(nolock)
                                             ON CB.id = CV.basicid
                                     INNER JOIN con_editcms_cars AS CC with(nolock)
                                             ON CC.basicid = CV.basicid
                                     INNER JOIN carmakes C with(nolock)
                                             ON CC.makeid = C.id
                                     INNER JOIN carmodels CM with(nolock)
                                             ON CC.modelid = CM.id
									 Inner Join Con_EditCms_BasicSubCategories CBS with(nolock)
											ON CBS.BasicId=CV.BasicId
									 inner join Con_editCms_subcategories CS with(nolock)
											ON CBS.SubCategoryId=CS.Id
                              WHERE  CV.isactive = 1
                                     AND CC.isactive = 1
									 AND cS.CategoryId=13
                                     AND CB.ispublished = 1
                                     AND CB.isactive = 1
                                     AND CB.id NOT IN(SELECT items
                                                      FROM
                                         dbo.Splittext(@basicIDs, ','))
                                     AND cb.id NOT IN
                                         (SELECT TOP 3 CCB.id
                                          FROM
                                         con_editcms_videos CCV
                                         INNER JOIN con_editcms_basic
                                                    CCB
                                                 ON CCV.basicid =
                                                    CCB.id
                                                       WHERE  CCB.isactive = 1
                                                              AND  CCB.ispublished= 1
                                                              AND CCB.isfeatured = 1
                                                       ORDER  BY displaydate DESC) )) AS T))a WHERE  row = 1
        END
      ELSE IF @SubCat = 2
        --for most popular Videos  
        BEGIN
             select * from (SELECT *,Row_number() OVER (ORDER BY views  desc) RowNo
            FROM   (SELECT DISTINCT  Top(1000) CV.basicid,
                   CV.videourl,
				   CV.VideoId,
                   CV.likes,
                   CV.views,
                   CB.description,
                   CV.duration,
                   CB.title,
				   CB.Url,
                   C.name              AS Make,
                   CM.name             AS Model,
				   CM.MaskingName,
                   CC.id               AS VideoCarId,
				   CS.Name as SubCatName,
                   Row_number()
                     OVER(
                       partition BY cv.basicid
                       ORDER BY CC.id) AS row
                    FROM   con_editcms_videos AS CV with(nolock)
                           INNER JOIN con_editcms_basic AS CB with(nolock)
                                   ON CB.id = CV.basicid
                           INNER JOIN con_editcms_cars AS CC with(nolock)
                                   ON CC.basicid = CV.basicid
                           INNER JOIN carmakes C with(nolock)
                                   ON CC.makeid = C.id
                           INNER JOIN carmodels CM with(nolock)
                                   ON CC.modelid = CM.id
						   Inner Join Con_EditCms_BasicSubCategories CBS with(nolock)
								ON CBS.BasicId=CV.BasicId
						   inner join Con_editCms_subcategories CS with(nolock)
								ON CBS.SubCategoryId=CS.Id
                    WHERE  CV.isactive = 1
                           AND CC.isactive = 1
						   AND cS.CategoryId=13
                           AND CB.ispublished = 1
                           AND CB.isactive = 1
                           AND ( CC.makeid = @MakeId
                                  OR @MakeId IS NULL )
                           AND ( CC.modelid = @ModelId
                                  OR @ModelId IS NULL )
                    ORDER  BY CV.views DESC)a
            WHERE  row = 1) AS T
	        WHERE T.RowNo BETWEEN @startindex
			   AND @endindex
		    ORDER BY RowNo  
        END
      ELSE
        --for expert reviews(roadtest,comparison test,first drive),interio show and miscelleneous categories
        BEGIN
            select * from (SELECT *,Row_number() OVER (ORDER BY  displaydate desc) RowNo
            FROM   (SELECT DISTINCT CV.basicid ,
                   CV.videourl,
				   CV.VideoId,
                   CV.likes,
                   CV.views,
                   CB.description,
                   CV.duration,
                   CB.title,
				   CB.Url,
                   CK.name             AS Make,
                   CM.name             AS Model,
				   CM.MaskingName,
                   CC.id               AS VideoCarId,
				   CS.Name as SubCatName,
				   CB.displaydate,
                   Row_number()OVER(partition BY cv.basicid ORDER BY CC.id) AS row
                    FROM   con_editcms_category AS C with(nolock)
                           INNER JOIN con_editcms_subcategories AS SC with(nolock)
                                   ON SC.categoryid = C.id 
                           INNER JOIN con_editcms_basicsubcategories AS BSC with(nolock)
                                   ON BSC.subcategoryid = SC.id
                           INNER JOIN con_editcms_videos AS CV with(nolock)
                                   ON CV.basicid = BSC.basicid
                           INNER JOIN con_editcms_basic AS CB with(nolock)
                                   ON CB.id = CV.basicid
                           INNER JOIN con_editcms_cars AS CC with(nolock)
                                   ON CC.basicid = CV.basicid
                           INNER JOIN carmakes CK with(nolock)
                                   ON CC.makeid = CK.id
                           INNER JOIN carmodels CM with(nolock)
                                   ON CC.modelid = CM.id
							Inner Join Con_EditCms_BasicSubCategories CBS with(nolock)
									ON CBS.BasicId=CV.BasicId
							inner join Con_editCms_subcategories CS with(nolock)
									ON CBS.SubCategoryId=CS.Id
                    WHERE  C.id = 13
                           AND CV.isactive = 1
                           AND CC.isactive = 1
						   AND cS.CategoryId=13
                           AND CB.ispublished = 1
                           AND CB.isactive = 1
                           AND ( CC.makeid = @MakeId
                                  OR @MakeId IS NULL )
                           AND ( CC.modelid = @ModelId
                                  OR @ModelId IS NULL )
                           AND ( ( @SubCat = 3 AND BSC.subcategoryid IN (47,48,50 ) ) OR( @SubCat = 4 AND BSC.subcategoryid IN (51,52,53 ) ) OR  ( @SubCat = 5 AND BSC.subcategoryid= 49  ) ))a
            WHERE  row = 1 ) AS T
	        WHERE T.RowNo BETWEEN @startindex
			   AND @endindex
		    ORDER BY RowNo  
        END
  END
