IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetModelVideos]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetModelVideos]
GO

	    
-- =============================================    
-- Author:  Ravi Koshal
-- Create date: 7/2/2014
-- Description: To get model specific videos.
-- Modified by: Natesh on 20-7-2014 Added Application id flag for CMS merging
-- =============================================    
CREATE PROCEDURE [dbo].[GetModelVideos]     
 -- Add the parameters for the stored procedure here    
 @ModelId numeric 
 ,@ApplicationId INT
AS    
BEGIN    

 SET NOCOUNT ON;    
  
WITH CTE AS 
( 
SELECT CB.Title AS VideoTitle, CV.VideoUrl AS VideoSrc , ISNULL(CV.Likes,0) AS Likes , ISNULL(CV.Views,0) AS Views , CV.VideoId  , CB.Description , CB.PublishedDate ,CB.IsSticky,
ROW_NUMBER() OVER(PARTITION BY CV.VideoId ORDER BY CB.PublishedDate DESC) AS RowNumber
FROM Con_EditCms_Basic AS CB 
inner join Con_EditCms_Videos AS CV WITH(NOLOCK) ON CB.Id = CV.BasicId AND CV.IsActive = 1
inner join Con_EditCms_Cars AS CC WITH(NOLOCK) ON CC.BasicId=CV.BasicId AND CC.ModelId = @ModelId AND CC.IsActive = 1
Where CB.IsPublished = 1 AND CB.ApplicationID = @ApplicationId
)
SELECT VideoTitle,VideoSrc,Likes,Views,VideoId,Description,PublishedDate,IsSticky FROM CTE WHERE RowNumber=1


END    
/****** Object:  StoredProcedure [cw].[GetRoadTestDetails]    Script Date: 8/13/2014 8:13:22 AM ******/
SET ANSI_NULLS ON
