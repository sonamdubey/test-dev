IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[AutoExpo_Photo_Gallery]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[AutoExpo_Photo_Gallery]
GO

	
-- ============================================= 
-- Author: Ravi Koshal     
-- Create date:  1/14/2013 
-- Description:   Gets the data for photos page of autoexpo
-- ============================================= 
CREATE PROCEDURE [cw].[AutoExpo_Photo_Gallery]     --Execute cw.AutoExpo_Photo_Gallery
  -- Add the parameters for the stored procedure here 
AS
  BEGIN
      -- SET NOCOUNT ON added to prevent extra result sets from 
      -- interfering with SELECT statements. 
      SET nocount ON;

	  SELECT CB.Id As BasicId, CB.Title As Title, CB.Url As Url, CB.IsPublished ,  CI.Id As ImageId, CI.HostUrl,CI.ImagePathThumbnail,CI.ImagePathLarge 
	  FROM Con_EditCms_Images AS CI
	  LEFT JOIN Con_EditCms_Basic AS CB ON CB.Id = CI.BasicId
	  Where CategoryId = 15 AND CI.Isactive = 1 AND CB.ShowGallery = 1 AND YEAR(CB.PublishedDate ) >= 2013
	  Order By CI.Id desc
	  
	  SELECT COUNT(ID) FROM Con_EditCms_Basic WHERE CategoryId = 15  AND ShowGallery = 1 AND YEAR(publisheddate) >= 2013
	  
  END  
