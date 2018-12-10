IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AutoExpo_MobileDefaultPage]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AutoExpo_MobileDefaultPage]
GO

	
-- ============================================= 
-- Author:   Supriya Khartode 
-- Create date:  15/1/2014
-- Description:  To fetch videos & images for mobile default page
-- ============================================= 
CREATE PROCEDURE [dbo].[AutoExpo_MobileDefaultPage]     --Execute dbo.AutoExpo_ImageGallery 
  -- Add the parameters for the stored procedure here 
AS
  BEGIN
      -- SET NOCOUNT ON added to prevent extra result sets from 
      -- interfering with SELECT statements. 
      SET nocount ON;

      -- Insert statements for procedure here 
      DECLARE @vidCount INT = 0

      SET @vidCount=(SELECT Count(*) AS VidCount
                     FROM   con_editcms_videos CV
                            INNER JOIN con_editcms_cars CC
                                    ON CC.basicid = CV.basicid)

      IF @vidCount > 0
        BEGIN
            SET @vidCount=1
        END

      SELECT TOP(1) CV.id,
                    videourl
      FROM   con_editcms_videos CV
             INNER JOIN con_editcms_cars CC
                     ON CC.basicid = CV.basicid 
             INNER JOIN con_editcms_basic CB
                     ON CB.id = CV.basicid AND CB.categoryid = 15
      ORDER  BY CB.LastUpdatedTime desc

      SELECT TOP(3-@vidCount) CI.id,
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
                     ON CB.id = CI.basicid AND CB.categoryid = 15
      ORDER  BY CI.lastupdatedtime DESC
  END  
