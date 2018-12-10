IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Microsite_GetCategoryWiseNavigation]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Microsite_GetCategoryWiseNavigation]
GO

	-- =============================================            
-- Author:  Vikas J            
-- Create date: 12/6/2013,             
-- Description: SP to Get the list of Category based navigation list.This navigation list is dynamic for a website based on dealerid.      
-- =============================================            
            
CREATE PROCEDURE [dbo].[Microsite_GetCategoryWiseNavigation]              
(              
 @ContentCatagoryId INT,      
 @DealerId INT             
)         
AS                
 --  fetching all the navigations avialable for that category and for that dealer              
 BEGIN               
   SELECT SubCatagoryId, SubCatagoryName, IsActive, UrlValue, SubCategoryOrder, NavigationId  FROM Microsite_DealerContentSubCategories  
   WHERE CategoryId=@ContentCatagoryId AND DealerId=@DealerId order by SubCatagoryId   
 END
