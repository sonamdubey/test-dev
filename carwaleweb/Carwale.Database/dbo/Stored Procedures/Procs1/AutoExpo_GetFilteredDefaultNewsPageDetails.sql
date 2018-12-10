IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AutoExpo_GetFilteredDefaultNewsPageDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AutoExpo_GetFilteredDefaultNewsPageDetails]
GO

	-- =============================================      
-- Author:  <Ravi Koshal>
-- Create date: <2/1/2014>      
-- Description: <Returns news list for the auto-expo site.> 
-- =============================================      
CREATE procedure [dbo].[AutoExpo_GetFilteredDefaultNewsPageDetails]      -- execute cw.AutoExpo_GetFilteredDefaultNewsPageDetails  9, 1, 200 , 2013,154,'2014-1-14'
 -- Add the parameters for the stored procedure here      
 @startindex INT,      
 @endindex INT,
 @YearPublished INT = 2013,
 @MakeId NUMERIC = NULL,
 @ModelId NUMERIC = NULL ,
 @Date Date = NULL
       
AS      
BEGIN      
 -- SET NOCOUNT ON added to prevent extra result sets from      
 -- interfering with SELECT statements.      
 SET NOCOUNT ON;  
     
 Select CB.Id AS BasicId, 
 CB.AuthorName, 
 CB.Description, 
 CB.DisplayDate, 
 CB.Views, 
 CB.Title, 
 CB.Url, 
 CI.IsMainImage, 
 CI.HostURL,
 CI.ImagePathThumbnail,
 CI.ImagePathLarge
 From Con_EditCms_Basic AS CB  
 INNER JOIN Con_EditCms_Images AS CI ON CI.BasicId = CB.Id
 INNER JOIN Con_EditCms_Cars AS CC ON CC.BasicId = CB.Id
 Where CB.CategoryId = 9 AND CB.IsActive = 1 AND CB.IsPublished = 1 AND CI.IsMainImage = 1 AND YEAR(CB.PublishedDate) >= @YearPublished 
 AND (CC.MakeId = @MakeId OR @MakeId IS NULL) AND (CC.ModelId = @ModelId OR @ModelId IS NULL) AND (CONVERT(DATE,CB.PublishedDate) = @Date OR @Date IS NULL) 
 			
			
 Select Count(CB.Id) AS Total
  From Con_EditCms_Basic As CB
  INNER JOIN Con_EditCms_Cars AS CC ON CC.BasicId = CB.Id
  Where CB.CategoryId = 9 AND CB.IsActive = 1 AND CB.IsPublished = 1 AND YEAR(CB.PublishedDate) >= @YearPublished
  AND (CC.MakeId = @MakeId OR @MakeId IS NULL) AND (CC.ModelId = @ModelId OR @ModelId IS NULL) AND (CONVERT(DATE,CB.PublishedDate) = @Date OR @Date IS NULL) 

END
