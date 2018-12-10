IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AutoExpo_GetDefaultNewsPageDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AutoExpo_GetDefaultNewsPageDetails]
GO

	-- =============================================      
-- Author:  <Ravi Koshal>
-- Create date: <2/1/2014>      
-- Description: <Returns news list for the auto-expo site.> 
-- =============================================      
CREATE procedure [dbo].[AutoExpo_GetDefaultNewsPageDetails]      -- execute cw.AutoExpo_GetDefaultNewsPageDetails  1, 10,2013,'Date'
 -- Add the parameters for the stored procedure here      
 @StartIndex INT,      
 @EndIndex INT,
 @YearPublished INT = 2013,
 @OrderBy Varchar(50)  = 'Date'     
AS      
BEGIN      
 -- SET NOCOUNT ON added to prevent extra result sets from      
 -- interfering with SELECT statements.      
 SET NOCOUNT ON;  
 SELECT * FROM
 (    
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
 CI.ImagePathLarge,
 --ROW_NUMBER() OVER (
	--			ORDER BY CASE WHEN (@OrderBy = 'Date')
	--			THEN
	--			 DisplayDate
	--			WHEN (@OrderBy = 'Views')
	--			THEN 
	--			 Views
	--			END 
	--			 Desc	
	--			) AS Row_No
	 ROW_NUMBER() OVER (
				ORDER BY CASE 
				WHEN (@OrderBy = 'Views')
				THEN 
				 Views
				END 
				 Desc,DisplayDate DESC	
				) AS Row_No
 From Con_EditCms_Basic AS CB  
 INNER JOIN Con_EditCms_Images AS CI ON CI.BasicId = CB.Id
 Where CB.CategoryId = 9 AND CB.IsActive = 1 AND CB.IsPublished = 1 AND CI.IsMainImage = 1 AND YEAR(CB.PublishedDate) >= @YearPublished
 --Order By
 --CASE WHEN (@OrderBy = 'Date')
 --THEN
 -- DisplayDate
 -- WHEN (@OrderBy = 'Views')
 -- THEN 
 -- Views
 --END 
 --Desc			
	) AS T 
	WHERE T.Row_No BETWEEN @StartIndex
			AND @EndIndex	
				
 Select Count(CB.Id) As Total 
 From Con_EditCms_Basic As CB 
  INNER JOIN Con_EditCms_Images AS CI ON CI.BasicId = CB.Id
 Where CB.CategoryId = 9 AND CB.IsActive = 1 AND CB.IsPublished = 1 AND CI.IsMainImage = 1 AND YEAR(CB.PublishedDate) >= @YearPublished

END

