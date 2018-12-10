IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[AutoExpo_ImageGallery]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[AutoExpo_ImageGallery]
GO

	
-- ============================================= 
-- Author: Prashant Vishe     
-- Create date:  16-01-2014
-- Description:   This SP gets the Gallery data (videos,images) for a makeid and landing page.
-- ============================================= 
CREATE PROCEDURE [cw].[AutoExpo_ImageGallery]     --Execute cw.AutoExpo_ImageGallery 5
  -- Add the parameters for the stored procedure here 
  @MakeId NUMERIC = NULL
AS
BEGIN
 -- SET NOCOUNT ON added to prevent extra result sets from 
-- interfering with SELECT statements. 
SET nocount ON;

-- Insert statements for procedure here 
DECLARE @vidCount INT

IF( @MakeId IS NULL )
  BEGIN
      SET @vidCount=(SELECT Count(*) AS VidCount
                     FROM   con_editcms_videos CV
                            INNER JOIN con_editcms_cars CC
                                    ON CC.basicid = CV.basicid
                            INNER JOIN con_editcms_basic CB
                                    ON CB.id = CV.basicid
                     WHERE  CB.categoryid = 15)

      -- (CC.makeid = @MakeId
      --      OR @MakeId IS NULL) )
      IF ( @vidCount > 2 )
        BEGIN
            SET @vidCount=2
        END

	  SELECT TOP(2) * FROM 
      (SELECT  ROW_NUMBER()
                      OVER(
                        PARTITION BY CC.MakeID
                        ORDER  BY CB.LastUpdatedTime DESC) AS ROWNUMBER,
					CC.MakeId AS MakeId,
                    CV.id,
                    videourl
      FROM   con_editcms_videos CV
             INNER JOIN con_editcms_cars CC
                     ON CC.basicid = CV.basicid
             INNER JOIN con_editcms_basic CB
                     ON CB.id = CV.basicid
                        AND CB.categoryid = 15) t
	  WHERE t.ROWNUMBER = 1

	  SELECT TOP(7-@vidCount) * FROM
     ( SELECT  ROW_NUMBER()
                                OVER(
                                  PARTITION BY CC.MakeID
                                  ORDER  BY CI.lastupdatedtime DESC) AS ROWNUMBER,
							  CC.MakeId,
                              CI.id,
                              CI.caption,
                              CI.imagename,
                              CI.hosturl,
                              CI.imagepathcustom,
                              CI.imagepathlarge,
                              CI.imagepathoriginal,
                              CI.imagepaththumbnail
      FROM   con_editcms_images CI
             INNER JOIN con_editcms_cars cc
                     ON CI.basicid = CC.basicid
             INNER JOIN con_editcms_basic CB
                     ON CB.id = CI.basicid
                        AND CB.categoryid = 15
                         AND CI.IsMainImage=1) t
	  WHERE t.ROWNUMBER = 1
	 
  END
ELSE
  BEGIN
      SET @vidCount=(SELECT Count(*) AS VidCount
                     FROM   con_editcms_videos CV
                            INNER JOIN con_editcms_cars CC
                                    ON CC.basicid = CV.basicid
                            INNER JOIN con_editcms_basic CB
                                    ON CB.id = CV.basicid
                     WHERE  ( CC.makeid = @MakeId )
                            AND CB.categoryid = 15)

      IF @vidCount > 2
        BEGIN
            SET @vidCount=2
        END

      SELECT TOP(2) CV.id,
                    videourl
      FROM   con_editcms_videos CV
             INNER JOIN con_editcms_cars CC
                     ON CC.basicid = CV.basicid
                        AND ( CC.makeid = @MakeId
                               OR @MakeId IS NULL )
             INNER JOIN con_editcms_basic CB
                     ON CB.id = CV.basicid
                        AND CB.categoryid = 15
      ORDER  BY CB.LastUpdatedTime DESC

      SELECT TOP(7-@vidCount) CI.id,
                              CI.caption,
                              CI.imagename,
                              CI.hosturl,
                              CI.imagepathcustom,
                              CI.imagepathlarge,
                              CI.imagepathoriginal,
                              CI.imagepaththumbnail
      FROM   con_editcms_images CI
             INNER JOIN con_editcms_cars cc
                     ON CI.basicid = CC.basicid
                        AND ( CC.makeid = @MakeId )
             INNER JOIN con_editcms_basic CB
                     ON CB.id = CI.basicid
                        AND CB.categoryid = 15
      ORDER  BY CI.lastupdatedtime DESC
  END  
	 
END

 